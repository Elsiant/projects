using System;
using System.Windows.Forms;

using MapEditor.Forms;

namespace MapEditor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void spriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var spriteEditor = new SpriteEditor();
            spriteEditor.Show();
        }
    }
}
