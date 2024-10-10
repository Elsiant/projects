from PyQt6.QtCore import Qt
from PyQt6.QtWidgets import (
    QWidget, QVBoxLayout, QHBoxLayout,
    QPushButton, QLabel, QFileDialog, QDialog
)
from PyQt6.QtGui import QPixmap

import cv2

from image_matching.find_image_matching import template_matching_location, keypoint_matching_location

from common.widgets.image_preview_dialog import ImagePreviewDialog

class ImageMatchingDialog(QWidget):
    def __init__(self):
        super().__init__()
        self.setWindowTitle("Image Selector")
        self.setGeometry(100, 100, 800, 400)

        self.source_image = None
        self.search_image = None

        layout = QVBoxLayout(self)

        # 윗쪽 매칭 버튼
        self.template_matching_button = QPushButton("Matching Image")
        self.template_matching_button.clicked.connect(lambda : self.find_matching_point("template"))
        layout.addWidget(self.template_matching_button)

        bottom_layout = QHBoxLayout(self)
        # 왼쪽 레이아웃
        left_layout = QVBoxLayout()
        self.left_button = QPushButton("Select Left Image")
        self.left_button.clicked.connect(lambda: self.select_image("left"))
        left_layout.addWidget(self.left_button)

        self.left_preview = QLabel("Left Image Preview")
        left_layout.addWidget(self.left_preview)

        bottom_layout.addLayout(left_layout)

        # 오른쪽 레이아웃
        right_layout = QVBoxLayout()
        self.right_button = QPushButton("Select Right Image")
        self.right_button.clicked.connect(lambda: self.select_image("right"))
        right_layout.addWidget(self.right_button)

        self.right_preview = QLabel("Right Image Preview")
        right_layout.addWidget(self.right_preview)

        bottom_layout.addLayout(right_layout)

        layout.addLayout(bottom_layout)

    def select_image(self, side):
        file_name, _ = QFileDialog.getOpenFileName(self, "Select an Image", "", "Images (*.png *.jpg *.jpeg *.bmp)")
        if not file_name:
            return
        
        pixmap = QPixmap(file_name)
        image = cv2.imread(file_name, cv2.IMREAD_UNCHANGED)

        if side == "left":
            self.search_image = image
            self.left_preview.setText(file_name)
            self.left_preview.setPixmap(pixmap.scaled(200, 200, Qt.AspectRatioMode.KeepAspectRatio))
        else:
            self.source_image = image
            self.right_preview.setText(file_name)
            self.right_preview.setPixmap(pixmap.scaled(200, 200, Qt.AspectRatioMode.KeepAspectRatio))


    def find_matching_point(self, matching_mode: str) -> None:
        if self.source_image is None or self.search_image is None :
            print("image not found")
            return
        
        matching_point = None
        if matching_mode == "template":
            matching_point = template_matching_location(self.search_image, self.source_image)
        elif matching_mode == "keypoint":
            matching_point = keypoint_matching_location(self.search_image, self.source_image)
        else:
            print("invalid matching mode")
            return

        if matching_point is None:
            print("matching point is None")
            return
        
        preive_image = cv2.circle(self.source_image, matching_point, 100, (0, 0, 255), 5)
        
        dialog = ImagePreviewDialog(self)
        dialog.set_image(preive_image)
        dialog.show()