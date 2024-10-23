using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MapEditor.Types;

namespace MapEditor.Forms.Panels
{
    class DrawingPanel : Panel
    {
        public static readonly int GRID_GAP = 32;
        public EventHandler<Point> MouseClicked;
        private int _scale = 1;

        private int _width = Tile.TILE_SIZE;
        private int _height = Tile.TILE_SIZE;

        private Point _mousePoint = new Point();
        private Color[,] _dotColors;

        public DrawingPanel()
        {
            this.Width = GRID_GAP * Tile.TILE_SIZE;
            this.Height = GRID_GAP * Tile.TILE_SIZE;

            InitializeDrawingPanel();

            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.MouseMove += DrawingPanel_MouseMove;
            this.MouseClick += DrawingPanel_MouseClick;
            this.Paint += DrawingPanel_Paint;

            _dotColors = new Color[_width, _height];
        }

        public void SetDotColor(Point point, Color color)
        {
            _dotColors[point.X, point.Y] = color;

            this.Refresh();
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

        private void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            int gridSize = GRID_GAP * _scale;
 
            Graphics g = e.Graphics;
            Pen grayPen = new Pen(Color.LightGray);
            Pen blackPen = new Pen(Color.Black);
            
            // 타일 그리기
            for (int i = 0; i < _width; i++)
            {
                for(int j = 0; j < _height; j++)
                {
                    if (_dotColors[i, j] == Color.Empty)
                    {
                        continue;
                    }

                    using (SolidBrush solidBrush = new SolidBrush(_dotColors[i, j]))
                    {
                        g.FillRectangle(
                            solidBrush, 
                            i * gridSize, 
                            j * gridSize,
                            gridSize,
                            gridSize);
                    }
                }
            }
            

            // 그리드 선 그리기
            for (int y = 0; y < this.Height; y += gridSize)
            {
                g.DrawLine(grayPen, 0, y, this.Width, y);
            }

            for (int x = 0; x < this.Width; x += gridSize)
            {
                g.DrawLine(grayPen, x, 0, x, this.Height);
            }

            // 선택한 타일 처리

            // 마우스 현재 타일 처리
            
            g.DrawRectangle(blackPen, new Rectangle(
                _mousePoint.X * gridSize,
                _mousePoint.Y * gridSize,
                gridSize,
                gridSize));

            grayPen.Dispose();
            blackPen.Dispose();
        }

        private void InitializeDrawingPanel()
        {
            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.AutoScroll = true;
        }
    }
}
