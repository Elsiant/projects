"""
keypoint_matching.py
두 이미지에서 유사한 특징점 이용해 이미지 매칭하는 방법
"""
import logging
from typing import Optional

import cv2
import numpy as np

from common import util
from common.types import Point


class KeypointsData:
    """
    특징점들의 데이터를 저장
    """

    def __init__(self, points: list[Point]) -> None:

        self._count = len(points)
        if self._count == 0:
            raise ValueError

        x_sum = 0
        y_sum = 0

        for point in points:
            x_sum += point.x
            y_sum += point.y

        self._middle_point = Point(int(x_sum / self._count), int(y_sum / self._count))

        distance_x_sum = 0
        distance_y_sum = 0

        for point in points:
            distance_x_sum += abs(self._middle_point.x - point.x)
            distance_y_sum += abs(self._middle_point.y - point.y)

        self._average_distance = (
            distance_x_sum / self._count,
            distance_y_sum / self._count,
        )

    def middle_point(self) -> Point:
        """중심점 반환"""
        return self._middle_point

    def average_distance(self) -> tuple[float, float]:
        """중심점에서 특징점까지의 평균 거리"""
        return self._average_distance


class KeypointMatching:
    """
    특징점을 이용해서 유사한 이미지를 찾아내는 방식
    정규화를 통해서 밝기 변화에 대해 어느정도 대응
    회전, 크기 변화에 어느정도 대응
    특징점이 충분히 있다면 이미지의 변형에도 대응(버튼의 빨간점, N마크 등)
    화면에서 찾으려는 이미지와 비슷한 부분이 많을 경우 못찾는 경우 발생(공통된 버튼의 모서리, 글자)
    """

    # 0~1 범위의 값을 가진다. 0.4 ~ 0.6 권장사항 0.6초과 할 경우 잘못된 특징점이 좋은 특징점이 되는 경우도 발생한다.
    # 0에 가까울 수록 강한 필터링
    # 1에 가까울 수록 느슨한 필터링
    _FILTER_RATIO = 0.6
    _SCALE_LIMIT = 5.0  # 이미지의 크기 범위 1 / _SCALE_LIMIT ~ _SCALE_LIMIT 까지만 찾는다
    _THREASHOLD = 0.65  # 0 ~ 1.0 범위 이미지의 유사도 임계값. 0 역일치 1 일치

    @classmethod
    def find_best_result(
        cls, image_search: cv2.Mat, image_source: cv2.Mat
    ) -> Optional[Point]:
        """
        특징 매칭 방법을 이용해서 가장 유사한 좌표를 찾는다.

        parameters:
            image_search 찾을 이미지
            image_source 화면 이미지
        """

        keypoints_source, keypoints_search, good = cls._find_keypoints(
            image_search, image_source
        )

        good_count = len(good)
        logging.info(f"good keypoint count : {good_count}")

        if good_count < 2:
            logging.warning("Too few good keypoints")
            return None
        elif good_count in [2, 3]:
            origin_result = cls._few_good_points(
                keypoints_source, keypoints_search, good
            )
        else:
            origin_result = cls._many_good_points(
                keypoints_source, keypoints_search, good
            )

        if origin_result is None:
            return None

        search_data, source_data = origin_result

        x_scale = source_data.average_distance()[0] / search_data.average_distance()[0]
        y_scale = source_data.average_distance()[1] / search_data.average_distance()[1]

        source_height, source_width = image_source.shape[:2]

        search_height, search_width = image_search.shape[:2]
        search_middle = search_data.middle_point()
        source_middle = source_data.middle_point()

        y_min = util.clamp(
            int(source_middle.y - (search_middle.y * y_scale)), 0, source_height
        )
        y_max = util.clamp(
            int(source_middle.y + ((search_height - search_middle.y) * y_scale)),
            0,
            source_height,
        )
        x_min = util.clamp(
            int(source_middle.x - (search_middle.x * x_scale)), 0, source_width
        )
        x_max = util.clamp(
            int(source_middle.x + ((search_width - search_middle.x) * x_scale)),
            0,
            source_width,
        )

        width = int((x_max - x_min) / x_scale)
        height = int((y_max - y_min) / y_scale)

        # 가로 혹은 세로 픽셀 수가 5 미만이면 인식 실패로 판단한다.
        if width < 5 or height < 5:
            logging.warning("target_imgage is too small")
            return None

        target_imgage = image_source[y_min:y_max, x_min:x_max]
        resized_source = cv2.resize(target_imgage, (width, height))

        # resize_source 같은 경우 화면이 잘리면 self._image_search보다 크기가 작을 수 있기 때문에 순서를 바꾼다.
        confidence = cal_ccoeff_confidence(resized_source, image_search)

        if confidence < cls._THREASHOLD:
            print(f"confidence less than _THREASHOLD : {confidence}")
            return None

        # 화면을 벗어난 경우 보정
        middle_point_x = util.clamp(source_middle.x, 0, source_width)
        middle_point_y = util.clamp(source_middle.y, 0, source_height)

        return Point(middle_point_x, middle_point_y)

    @classmethod
    def _find_keypoints(
        cls, image_search: cv2.Mat, image_source: cv2.Mat
    ) -> tuple[list, list, list]:
        """특징점을 찾아서 반환한다."""
        # 속도가 느려도 이미지 피라미드를 이용해서 크기 변화에 따른 특징 검출의 문제를 어느정도 해결한 SIFT를 사용한다.
        detector = cv2.SIFT_create()

        keypoints_source, desciptors_source = detector.detectAndCompute(
            image_source, None
        )
        keypoints_search, desciptors_search = detector.detectAndCompute(
            image_search, None
        )

        matcher = cv2.BFMatcher(cv2.NORM_L2)
        matches = matcher.knnMatch(desciptors_search, desciptors_source, k=2)

        good = []
        for m, n in matches:
            if m.distance < cls._FILTER_RATIO * n.distance:
                good.append((m))

        good_diff = []
        diff_good_point: list[list[int]] = [[]]
        for m in good:
            diff_point = [
                int(keypoints_source[m.trainIdx].pt[0]),
                int(keypoints_source[m.trainIdx].pt[1]),
            ]
            if diff_point not in diff_good_point:
                good_diff.append(m)
                diff_good_point.append(diff_point)
        good = good_diff

        return keypoints_source, keypoints_search, good

    @classmethod
    def _many_good_points(
        cls, keypoints_source: list, keypoints_search: list, good: list
    ) -> Optional[tuple[KeypointsData, KeypointsData]]:
        """좋은 특징점이 4개 이상일 경우"""

        def convert_to_ndarray(input_list: list[list[int]]) -> np.ndarray:
            """list[list[int]] 타입을 ndarray[float32]로 변환"""
            return np.array(input_list, dtype=np.float32).reshape((-1, 1, 2))

        search_points = convert_to_ndarray(
            [keypoints_search[m.queryIdx].pt for m in good]
        )

        source_points = convert_to_ndarray(
            [keypoints_source[m.trainIdx].pt for m in good]
        )

        _, mask = cv2.findHomography(search_points, source_points, cv2.RANSAC, 5.0)
        matches_mask = mask.ravel().tolist()

        # 두 좋은 특징점 중간에서 더 정확한 점을 걸러냅니다(양호한 점의 대부분이 정확하다고 가정하고 비율=0.7로 보장됨)
        selected = [v for k, v in enumerate(good) if matches_mask[k]]

        search_keypoints: list[Point] = []
        source_keypoints: list[Point] = []

        for keypoint in selected:
            search_keypoints.append(
                Point(
                    keypoints_search[keypoint.queryIdx].pt[0],
                    keypoints_search[keypoint.queryIdx].pt[1],
                )
            )
            source_keypoints.append(
                Point(
                    keypoints_source[keypoint.trainIdx].pt[0],
                    keypoints_source[keypoint.trainIdx].pt[1],
                )
            )

        search_data = KeypointsData(search_keypoints)
        source_data = KeypointsData(source_keypoints)

        if not cls._scale_check(search_data, source_data):
            return None

        return search_data, source_data

    @classmethod
    def _few_good_points(
        cls, keypoints_source: list, keypoints_search: list, good: list
    ) -> Optional[tuple[KeypointsData, KeypointsData]]:
        """좋은 특징점이 2, 3개 일 경우"""
        search_keypoints: list[Point] = []
        source_keypoints: list[Point] = []

        for keypoint in good:
            search_keypoints.append(
                Point(
                    keypoints_search[keypoint.queryIdx].pt[0],
                    keypoints_search[keypoint.queryIdx].pt[1],
                )
            )
            source_keypoints.append(
                Point(
                    keypoints_source[keypoint.trainIdx].pt[0],
                    keypoints_source[keypoint.trainIdx].pt[1],
                )
            )

        # 특징점들의 x축 또는 y축이 모두 같으면(src 또는 sch에 관계없이) 대상 직사각형 영역을 계산할 수 없으며 반환 값은 good=1과 같다.
        if (
            all(point.x == search_keypoints[0].x for point in search_keypoints)
            or all(point.y == search_keypoints[0].y for point in search_keypoints)
            or all(point.x == source_keypoints[0].x for point in source_keypoints)
            or all(point.y == source_keypoints[0].y for point in source_keypoints)
        ):
            return None

        search_data = KeypointsData(search_keypoints)
        source_data = KeypointsData(source_keypoints)

        if not cls._scale_check(search_data, source_data):
            return None

        return search_data, source_data

    @classmethod
    def _scale_check(
        cls, search_data: KeypointsData, source_data: KeypointsData
    ) -> bool:
        """찾아낸 이미지의 scale 값이 너무 크거나 작지 않은지 확인"""
        x_scale = source_data.average_distance()[0] / search_data.average_distance()[0]
        y_scale = source_data.average_distance()[1] / search_data.average_distance()[1]

        if (
            x_scale > cls._SCALE_LIMIT
            or x_scale < 1 / cls._SCALE_LIMIT
            or y_scale > cls._SCALE_LIMIT
            or y_scale < 1 / cls._SCALE_LIMIT
        ):
            logging.warning("KeypointMatching failed. scale out of range _SCALE_LIMIT")
            return False

        return True


def cal_ccoeff_confidence(search: cv2.Mat, source: cv2.Mat) -> float:
    """두 이미지의 유사도를 TM_CCOEFF_NORMED 메소드를 이용해서 계산"""
    # 확장된 신뢰도 계산 영역
    copy_source = cv2.copyMakeBorder(source, 10, 10, 10, 10, cv2.BORDER_REPLICATE)
    # 알고리즘이 작은 차이를 과도하게 증폭하지 않도록 값 범위 간섭을 추가합니다.
    copy_source[0, 0] = 0
    copy_source[0, 1] = 255

    source_gray = cv2.cvtColor(copy_source, cv2.COLOR_BGR2GRAY)
    search_gray = cv2.cvtColor(search, cv2.COLOR_BGR2GRAY)

    result = cv2.matchTemplate(source_gray, search_gray, cv2.TM_CCOEFF_NORMED)
    _, max_value, _, _ = cv2.minMaxLoc(result)

    return (1 + max_value) / 2
