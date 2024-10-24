using System;
using System.Drawing;
using System.Windows.Forms;
using MapEditor.Types;

namespace MapEditor.Forms.Panels
{
    class DrawingPanel : Panel
    {
        public static readonly int GRID_GAP = 32;
        public EventHandler<Point> MouseClicked;
        protected int _scale = 1;
        protected int _width;
        protected int _height;

        protected Point _mousePoint = new Point();

        public DrawingPanel()
        {
            InitializeDrawingPanel();
        }


        private void DrawingPanel_MouseClick(object sender, MouseEventArgs e)
        {
            int gridSize = GRID_GAP * _scale;
            MouseClicked.Invoke(this, new Point(e.X / gridSize, e.Y / gridSize));
        }

        private void DrawingPanel_MouseMove(object sender, MouseEventArgs e)
        {
            int gridSize = GRID_GAP * _scale;
            _mousePoint = new Point(e.X / gridSize, e.Y / gridSize);

            this.Refresh();
        }

        virtual protected void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            DrawGrid(e);
            DrawCurrentRect(e);
        }

        protected void DrawCurrentRect(PaintEventArgs e)
        {
            int gridSize = GRID_GAP * _scale;
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Black);
            
            // 마우스 위치 그리기
            g.DrawRectangle(pen, new Rectangle(
                _mousePoint.X * gridSize,
                _mousePoint.Y * gridSize,
                gridSize,
                gridSize));

            pen.Dispose();
        }

        protected void DrawGrid(PaintEventArgs e)
        {
            int gridSize = GRID_GAP * _scale;

            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.LightGray);

            // 그리드 선 그리기
            for (int y = 0; y < this.Height; y += gridSize)
            {
                g.DrawLine(pen, 0, y, this.Width, y);
            }

            for (int x = 0; x < this.Width; x += gridSize)
            {
                g.DrawLine(pen, x, 0, x, this.Height);
            }

            pen.Dispose();
        }

        private void InitializeDrawingPanel()
        {
            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.AutoScroll = true;
            
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.MouseMove += DrawingPanel_MouseMove;
            this.MouseClick += DrawingPanel_MouseClick;
            this.Paint += DrawingPanel_Paint;
        }
    }
}
