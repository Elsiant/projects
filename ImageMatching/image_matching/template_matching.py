"""
template_matching.py
두개의 이미지의 도트의 전체적인 유사도를 비교해서 이미지 매칭하는 방법
"""
from typing import Optional

import cv2

from common.types import Point

class TemplateMatching:
    """
    각각의 픽셀을 비교해서 유사한 이미지를 찾아내는 방식
    정규화를 통해서 밝기 변화에 대해 어느정도 대응
    반복문을 통해서 크기 변화에 대해 어느정도 대응
    각각의 픽셀을 비교하는 특성상 회전에는 대응 불가능(이미지가 회전 된다면 사용 불가능)
    변형된 이미지(버튼의 빨간점, N마크 등)대응 불가능
    """

    _THREASHOLD = 0.8  # 이미지의 유사도 임계값
    _END_RATIO = 0.95  # 동일한 이미지라 판단하고 중단할 유사도 비율
    _SCALE_LIMIT = 3.0  # 이미지의 크기 범위 1 / _SCALE_LIMIT ~ _SCALE_LIMIT 까지만 찾는다
    _STEP = 100  # template이미지의 사이즈를 바꾸며 찾는 횟수

    @classmethod
    def find_best_result(
        cls, image_search: cv2.Mat, image_source: cv2.Mat
    ) -> Optional[Point]:
        """
        TemplateMatching방식으로 가장 유사한 좌표를 찾는다.

        parameters:
            image_search 찾을 이미지
            image_source 화면 이미지
        """

        ret_value = None

        origin = image_search

        if origin.shape[2] == 4:  # rgb 3, rgba 4
            with_alpha = True
            code = cv2.COLOR_BGRA2GRAY
        else:
            with_alpha = False
            code = cv2.COLOR_BGR2GRAY

        template = cv2.cvtColor(image_search, code)
        screen = cv2.cvtColor(image_source, code)

        template_height, template_width = template.shape[:2]
        screen_height, screen_width = screen.shape[:2]

        min_width = int(1 / cls._SCALE_LIMIT * template_width)
        min_height = int(1 / cls._SCALE_LIMIT * template_height)

        diff_width = int(cls._SCALE_LIMIT * template_width) - min_width
        diff_height = int(cls._SCALE_LIMIT * template_height) - min_height

        best_confidence = cls._THREASHOLD

        for i in range(cls._STEP):
            width = min_width + int(i * diff_width / cls._STEP)
            height = min_height + int(i * diff_height / cls._STEP)

            if width > screen_width or height > screen_height:
                # 높이나 넓이가 화면보다 커진 경우 중단
                # 중단 안할경우 cv2.matchTemplate에서 에러발생
                break

            resized_template = cv2.resize(template, (width, height))

            if with_alpha:
                resized_origin = cv2.resize(origin, (width, height))

                cv2.normalize(
                    resized_template, resized_template, 0, 255, cv2.NORM_MINMAX
                )
                resized_template = cv2.bitwise_and(
                    resized_template, resized_origin[:, :, 3]
                )

            result = cv2.matchTemplate(screen, resized_template, cv2.TM_CCOEFF_NORMED)

            _, max_value, _, max_location = cv2.minMaxLoc(result)

            if max_value > best_confidence:
                best_confidence = max_value

                left, top = max_location

                ret_value = Point(
                    left + int(width / 2),
                    top + int(height / 2),
                )

                if best_confidence > cls._END_RATIO:
                    break

        return ret_value
