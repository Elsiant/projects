import sys
from PyQt6.QtWidgets import QApplication

from common.widgets import ImageMatchingDialog

app = QApplication(sys.argv)
main_window = ImageMatchingDialog()
main_window.show()
sys.exit(app.exec())