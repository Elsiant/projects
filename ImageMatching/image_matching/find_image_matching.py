"""
find_Image_location.py
"""
import time
from typing import Callable, Optional

import cv2

from image_matching.keypoint_matching import KeypointMatching
from image_matching.template_matching import TemplateMatching
from common.types import Point


def template_matching_location(
        search_image: cv2.Mat,
        source_image: cv2.Mat
) -> Optional[Point]:
    return TemplateMatching.find_best_result(search_image, source_image)


def keypoint_matching_location(
        search_image: cv2.Mat,
        source_image: cv2.Mat
) -> Optional[Point]:
    none_alpha_search_image: Optional[cv2.Mat] = None

    result = None

    color_channel_count = search_image.shape[2]
    if none_alpha_search_image is None:
        none_alpha_search_image = (
            cv2.cvtColor(search_image, cv2.COLOR_BGRA2BGR)
            if color_channel_count == 4
            else search_image
        )

        # Search Image 알파 제거 실패.
        if none_alpha_search_image is None:
            return result

    color_channel_count = source_image.shape[2]
    none_alpha_source_image = (
        cv2.cvtColor(source_image, cv2.COLOR_BGRA2BGR)
        if color_channel_count == 4
        else source_image
    )

    # Source Image 알파 제거 실패.
    if none_alpha_search_image is None:
        return result

    result = KeypointMatching.find_best_result(
        none_alpha_search_image, none_alpha_source_image
    )

    # Keypoint Matching으로 이미지를 찾았다.
    return result


def find_image_location(
    search_image: cv2.Mat,
    source_image_getter: Callable[[], cv2.Mat],
    check_keypoint_matching: bool,
    wait_sec: float,
) -> Optional[Point]:
    """
    Source 이미지에서 Search 이미지를 찾아 위치를 반환합니다.

    TemplateMatching 시도 후 못찾았다면 BGRA to BGR(알파 제거) 이후 KeypointMatching 진행.

    Args:
        source_image_getter: search_image를 찾을 이미지를 반환하는 델이게이트 (BGR or BGRA)
        search_image: source_image에서 찾을 이미지 (BGR or BGRA)
        duration: 검색 시간 (검색 시간 내 블록킹)

    Returns:
        source_image에서의 search_image가 위치한 중앙 좌표
    """

    none_alpha_search_image: Optional[cv2.Mat] = None

    result = None

    start_time = time.time()
    while True:
        source_image = source_image_getter()
        if source_image is None:
            break

        result = TemplateMatching.find_best_result(search_image, source_image)

        # Template Matching으로 이미지를 찾았다면.
        if result is not None:
            break

        # Keypoint 매칭 체크
        if not check_keypoint_matching:
            continue

        color_channel_count = search_image.shape[2]
        if none_alpha_search_image is None:
            none_alpha_search_image = (
                cv2.cvtColor(search_image, cv2.COLOR_BGRA2BGR)
                if color_channel_count == 4
                else search_image
            )

            # Search Image 알파 제거 실패.
            if none_alpha_search_image is None:
                break

        color_channel_count = source_image.shape[2]
        none_alpha_source_image = (
            cv2.cvtColor(source_image, cv2.COLOR_BGRA2BGR)
            if color_channel_count == 4
            else source_image
        )

        # Source Image 알파 제거 실패.
        if none_alpha_search_image is None:
            break

        result = KeypointMatching.find_best_result(
            none_alpha_search_image, none_alpha_source_image
        )

        # Keypoint Matching으로 이미지를 찾았다면.
        if result is not None:
            break

        # 시간 체크
        check_time = time.time() - start_time
        if check_time > wait_sec:
            break

        time.sleep(0.001)

    return result
