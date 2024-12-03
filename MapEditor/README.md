# MapEditor
C#, Winform을 사용해 간단한 맵에디터의 기능을 구현

## 각 Form의 기능
- MainForm 맵에디터 기능 오른쪽 패널에서 Png이미지를 선택해 왼쪽의 패널에 그리는 방식으로 맵을 편집
- SpriteEditor 하나의 타일을 어떻게 그릴지 편집 오른쪽에서 선택한 색상으로 왼쪽의 패널에 한 도트씩 그리는 방식으로 이미지를 편집

## DrawingPanel.cs
- MapDrawingPanel과 DotDrawingPanel의 부모
- 그리기 패널의 기본적인 기능 들을 정의(화면 스크롤, 마우스 위치, 더블 버퍼 적용, 그리드 선 그리기)