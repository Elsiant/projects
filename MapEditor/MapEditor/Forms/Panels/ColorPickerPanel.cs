using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace MapEditor.Forms.Panels
{
    class ColorPickerPanel : Panel
    {
        private int WIDTH = 400;
        private int HEIGHT = 600;
        private Panel[] _colorPanels;
        private Label _moreColorLabel;
        private ColorDialog _colorDialog;

        public event EventHandler<Color> ColorSelected;  // 색상이 선택되면 발생

        public ColorPickerPanel()
        {
            InitializePanel();
        }

        private void InitializePanel()
        {
            this.Size = new Size(WIDTH, HEIGHT);
            this.BorderStyle = BorderStyle.FixedSingle;

            _colorDialog = new ColorDialog();

            Color[] presetColors = { Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.White, Color.Black };

            _colorPanels = new Panel[presetColors.Length];

            for (int i = 0; i < _colorPanels.Length; i++)
            {
                _colorPanels[i] = new Panel
                {
                    Size = new Size(30, 30),
                    Location = new Point(10 + (i * 35), 10),
                    BackColor = presetColors[i],
                    BorderStyle = BorderStyle.FixedSingle
                };

                _colorPanels[i].Click += ColorPanel_Click;
                this.Controls.Add(_colorPanels[i]);
            }

            // "More Colors..." Label
            _moreColorLabel = new Label
            {
                Text = "More Colors...",
                Size = new Size(100, 20),
                Location = new Point(10, 50),
                ForeColor = Color.Blue
            };
            _moreColorLabel.Click += MoreColorsLabel_Click;
            this.Controls.Add(_moreColorLabel);
        }

        // 미리 정의된 색상 패널 클릭 이벤트
        private void ColorPanel_Click(object sender, EventArgs e)
        {
            Panel clickedPanel = sender as Panel;
            ColorSelected?.Invoke(this, clickedPanel.BackColor);  // 선택된 색상을 이벤트로 전달
        }

        // "More Colors..." 클릭 시 ColorDialog 표시
        private void MoreColorsLabel_Click(object sender, EventArgs e)
        {
            if (_colorDialog.ShowDialog() == DialogResult.OK)
            {
                ColorSelected?.Invoke(this, _colorDialog.Color);  // ColorDialog에서 선택된 색상을 이벤트로 전달
            }
        }
    }
}
