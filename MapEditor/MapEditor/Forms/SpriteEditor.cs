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

        public SpriteEditor()
        {
            InitializeComponent();

            var rightPanel = new ColorPickerPanel();
            rightPanel.Dock = DockStyle.Fill;
            rightPanel.ColorSelected += RightPanel_ColorSelected;
            RightPanel.Controls.Add(rightPanel);
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
