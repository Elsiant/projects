import sys
from PyQt6.QtCore import Qt
from PyQt6.QtWidgets import (
    QWidget, QVBoxLayout, QHBoxLayout,
    QPushButton, QLabel, QFileDialog, QDialog
)
from PyQt6.QtGui import QPixmap

class ImagePreviewDialog(QDialog):
    def __init__(self, parent=None):
        super().__init__(parent)
        self.setWindowTitle("Image Preview")
        self.setFixedSize(400, 400)

        self.image_label = QLabel(self)
        self.image_label.setScaledContents(True)
        self.image_label.setFixedSize(380, 380)

        layout = QVBoxLayout()
        layout.addWidget(self.image_label)
        self.setLayout(layout)

    def set_image(self, image_path):
        pixmap = QPixmap(image_path)
        self.image_label.setPixmap(pixmap)

class ImageMatcingDialog(QWidget):
    def __init__(self):
        super().__init__()
        self.setWindowTitle("Image Selector")
        self.setGeometry(100, 100, 800, 400)

        layout = QHBoxLayout(self)

        # 왼쪽 레이아웃
        left_layout = QVBoxLayout()
        self.left_button = QPushButton("Select Left Image")
        self.left_button.clicked.connect(lambda: self.select_image("left"))
        left_layout.addWidget(self.left_button)

        self.left_preview = QLabel("Left Image Preview")
        left_layout.addWidget(self.left_preview)

        layout.addLayout(left_layout)

        # 오른쪽 레이아웃
        right_layout = QVBoxLayout()
        self.right_button = QPushButton("Select Right Image")
        self.right_button.clicked.connect(lambda: self.select_image("right"))
        right_layout.addWidget(self.right_button)

        self.right_preview = QLabel("Right Image Preview")
        right_layout.addWidget(self.right_preview)

        layout.addLayout(right_layout)

    def select_image(self, side):
        file_name, _ = QFileDialog.getOpenFileName(self, "Select an Image", "", "Images (*.png *.jpg *.jpeg *.bmp)")
        if not file_name:
            return
        
        if side == "left":
            self.left_preview.setText(file_name)
            pixmap = QPixmap(file_name)
            self.left_preview.setPixmap(pixmap.scaled(200, 200, Qt.AspectRatioMode.KeepAspectRatio))
        else:
            self.right_preview.setText(file_name)
            pixmap = QPixmap(file_name)
            self.right_preview.setPixmap(pixmap.scaled(200, 200, Qt.AspectRatioMode.KeepAspectRatio))