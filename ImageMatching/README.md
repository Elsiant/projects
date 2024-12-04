# Image Matching
자동화 테스트 툴을 개발하는 중 화면상에 표시된 이미지 중 원하는 이미지가 있는지 확인 하기 위해서 기능 추가

## 매칭 방법
- Template Matching
- Keypoint Matching

## Template Matching
- template_matching.py
- 화면과 찾는 이미지의 도트를 값을 비교해 유사도를 판단
- 게임의 해상도가 변경 될 경우 대비하여 반복문을 통해서 이미지의 크기를 바꿔가면서 탐색

## Keypoint Matching
- keypoint_matching.py
- 도트의 값을 비교하는 대신 유사한 Keypoint의 존재 유무로 유사도를 판단
- 찾는 이미지가 어느정도 회전, 크기 변화해도 찾을 수 있다.

## 실행전 주의 사항
- 가상 환경 셋팅을 위해선 scripts/init.sh을 bash로 실행 해줘야 합니다.

## 사용 방법
1. Select Search Image 버튼을 눌러 찾고자 하는 이미지 등록
2. Select Source Image 버튼을 눌러 탐색할 이미지 등록
3. Template Matching 혹은 Keypoint Matching 버튼을 눌러 이미지 매칭 테스트