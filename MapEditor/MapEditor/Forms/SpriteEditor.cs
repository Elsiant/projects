using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MapEditor.Forms.Panels;

namespace MapEditor.Forms
{
    public partial class SpriteEditor : Form
    {
        private Color _currentColor = Color.Transparent;
        private Panel _drawingPanel;

        public SpriteEditor()
        {
            InitializeComponent();

            InitializeForm();
        }

        private void InitializeForm()
        {
            // 왼쪽 패널 그리기 화면
            _drawingPanel = new Panel();
            _drawingPanel.Dock = DockStyle.Fill;
            _drawingPanel.BackColor = Color.White;
            _drawingPanel.BorderStyle = BorderStyle.FixedSingle;
            _drawingPanel.AutoScroll = true;
            splitContainer.Panel1.Controls.Add(_drawingPanel);

            // 오른쪽 패널 색상 선택
            var rightPanel = new ColorPickerPanel();
            rightPanel.Dock = DockStyle.Fill;
            rightPanel.ColorSelected += RightPanel_ColorSelected;
            splitContainer.Panel2.Controls.Add(rightPanel);

            
        }

        private void RightPanel_ColorSelected(object sender, Color e)
        {
            if (_currentColor == e)
            {
                return;
            }

            _currentColor = e;
        }
    }
}
