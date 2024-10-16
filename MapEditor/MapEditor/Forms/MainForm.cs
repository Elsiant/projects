using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
