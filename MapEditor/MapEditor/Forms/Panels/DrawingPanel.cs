using System;
using System.Drawing;
using System.Windows.Forms;

namespace MapEditor.Forms.Panels
{
    class DrawingPanel : Panel
    {
        public DrawingPanel()
        {
            InitializeDrawingPanel();
        }

        private void InitializeDrawingPanel()
        {
            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.AutoScroll = true;
        }
    }
}
