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
        private Label _currentColorLabel;
        private Color _currentColor;
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

            Color[] presetColors = {
                Color.Red,
                Color.Orange,
                Color.Yellow,
                Color.Green,
                Color.Blue,
                Color.Indigo,
                Color.Purple,
                Color.White,
                Color.Black,
            };

            _colorPanels = new Panel[presetColors.Length];

            int columnCount = _colorPanels.Length;
            for (int i = 0; i < columnCount; i++)
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

            for (int i = 0; i < columnCount; i++)
            {
                _colorPanels[i] = new Panel
                {
                    Size = new Size(30, 30),
                    Location = new Point(10 + (i * 35), 50),
                    BackColor = Color.Transparent,
                    BorderStyle = BorderStyle.FixedSingle
                };

                _colorPanels[i].Click += ColorPanel_Click;
                this.Controls.Add(_colorPanels[i]);
            }

            _currentColorLabel = new Label
            {
                Text = "",
                AutoSize = true,
                Location = new Point(10, 90)
            };
            this.Controls.Add(_currentColorLabel);

            CurrentColorChanged(Color.Black);
        }

        // 미리 정의된 색상 패널 클릭 이벤트
        private void ColorPanel_Click(object sender, EventArgs e)
        {
            Panel clickedPanel = sender as Panel;

            // 미리 지정된 색상이 없다면 다이얼로그에서 선택하고 해당 색상으로 변경한다.
            if ( clickedPanel.BackColor == Color.Transparent)
            {
                if(_colorDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                clickedPanel.BackColor = _colorDialog.Color;
            }

            CurrentColorChanged(clickedPanel.BackColor);
            ColorSelected?.Invoke(this, clickedPanel.BackColor);  // 선택된 색상을 이벤트로 전달
        }

        // Color가 변경
        private void CurrentColorChanged(Color color)
        {
            _currentColor = color;
            _currentColorLabel.Text = color.ToString();
        }
    }
}
