import sys
from PyQt6.QtWidgets import QApplication

from common.widgets import ImageMatcingDialog

app = QApplication(sys.argv)
main_window = ImageMatcingDialog()
main_window.show()
sys.exit(app.exec())