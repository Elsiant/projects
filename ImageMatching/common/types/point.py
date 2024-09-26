"""
point.py
"""

from typing import NamedTuple


class Point(NamedTuple):
    """
    좌표 표현하는 Int형 Point
    """

    x: int = 0
    y: int = 0
