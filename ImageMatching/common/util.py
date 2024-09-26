from typing import TypeVar

NumType = TypeVar("NumType", int, float)
                  
def clamp(value: NumType, min_value: NumType, max_value: NumType) -> NumType:
    """범위 자르기"""
    return max(min(value, max_value), min_value)
