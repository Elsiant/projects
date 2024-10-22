using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MapEditor.Types;

namespace MapEditor.Forms.Panels
{
    class DrawingPanel : Panel
    {
        public EventHandler<Point> MouseClicked;
        private int _scale = 1;
        private Point _mousePoint = new Point();
        private Dictionary<Point, Color> _tilesColor;

        public DrawingPanel()
        {
            InitializeDrawingPanel();

            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.MouseMove += DrawingPanel_MouseMove;
            this.MouseClick += DrawingPanel_MouseClick;
            this.Paint += DrawingPanel_Paint;
        }

        public void SetTileColor(Point point, Color color)
        {
            foreach (var tileColor in _tilesColor)
            {
                if (tileColor.Key.Equals(point))
                {
                    _tilesColor.Add(tileColor.Key, color);
                    return;
                }
            }

            _tilesColor.Add(point, color);
        }

        private void DrawingPanel_MouseClick(object sender, MouseEventArgs e)
        {
            MouseClicked.Invoke(this, new Point(e.X / Tile.TILE_SIZE, e.Y / Tile.TILE_SIZE));
        }

        private void DrawingPanel_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = new Point(e.X / Tile.TILE_SIZE, e.Y / Tile.TILE_SIZE);
            if (point.Equals(_mousePoint))
            {
                return;
            }
            _mousePoint = point;

            this.Refresh();
        }

        private void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            int gridSize = Tile.TILE_SIZE * _scale;
 
            Graphics g = e.Graphics;
            Pen grayPen = new Pen(Color.LightGray);
            Pen blackPen = new Pen(Color.Black);

            // 타일 그리기
            

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
            var currentMouse = new Tile(_mousePoint);
            var rect = currentMouse.GetRect();
            
            g.DrawRectangle(blackPen, rect);

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
