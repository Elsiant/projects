using System.Drawing;
using System.Windows.Forms;
using MapEditor.Forms.Panels;

namespace MapEditor.Forms
{
    public partial class SpriteEditor : Form
    {
        private Color _currentColor = Color.Transparent;
        private DotDrawingPanel _drawingPanel;

        public SpriteEditor()
        {
            InitializeComponent();

            InitializeForm();
        }

        private void InitializeForm()
        {
            // 왼쪽 패널 그리기 화면
            _drawingPanel = new DotDrawingPanel();
            _drawingPanel.MouseClicked += LeftPanel_Clicked;
            splitContainer.Panel1.Controls.Add(_drawingPanel);

            // 오른쪽 패널 색상 선택
            var rightPanel = new ColorPickerPanel();
            rightPanel.Dock = DockStyle.Fill;
            rightPanel.ColorSelected += RightPanel_ColorSelected;
            splitContainer.Panel2.Controls.Add(rightPanel);
        }

        private void LeftPanel_Clicked(object sender, Point e)
        {
            var drawingPanel = sender as DotDrawingPanel;
            drawingPanel.SetDotColor(e, _currentColor);
        }

        private void RightPanel_ColorSelected(object sender, Color e)
        {
            if (_currentColor == e)
            {
                return;
            }

            _currentColor = e;
        }

        private void saveToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
        }

        private void saveAsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {

        }

        private void loadToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
        }
    }
}
