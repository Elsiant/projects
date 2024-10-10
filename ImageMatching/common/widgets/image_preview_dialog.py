from typing import Optional
from PyQt6.QtWidgets import (
     QWidget, QVBoxLayout, QLabel, QDialog,
)
from PyQt6.QtGui import QPixmap, QImage

class ImagePreviewDialog(QDialog):
    def __init__(self, parent : Optional[QWidget]=None) -> None:
        super().__init__(parent)
        self.setWindowTitle("Image Region")
        self.setFixedSize(400, 400)

        self.image_label = QLabel(self)
        self.image_label.setScaledContents(True)
        self.image_label.setFixedSize(380, 380)

        layout = QVBoxLayout()
        layout.addWidget(self.image_label)
        self.setLayout(layout)


    def set_image(self, image) -> None:
        height, width, _ = image.shape
        bytesPerLine = 3 * width
        qImg = QImage(image.data, width, height, bytesPerLine, QImage.Format.Format_BGR888)
        self.image_label.setPixmap(QPixmap(qImg))