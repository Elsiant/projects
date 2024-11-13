using System;
using System.Drawing;
using System.Windows.Forms;

using MapEditor.Forms;
using MapEditor.Forms.Panels;

namespace MapEditor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            InitializeForm();
        }

        private void InitializeForm()
        {
            var leftPanel = new MapDrawingPanel();
            leftPanel.MouseLeftClicked += LeftPanelCliced;
            leftPanel.Dock = DockStyle.Fill;

            splitContainer.Panel1.Controls.Add(leftPanel);
        }

        private void spriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var spriteEditor = new SpriteEditor();
            spriteEditor.Show();
        }

        private void LeftPanelCliced(object sender, Point e)
        {
            var drawingPanel = sender as DotDrawingPanel;
            //drawingPanel.SetBitmap(e, filePath);
        }
    }
}
