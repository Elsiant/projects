using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MapEditor.Types;

namespace MapEditor.Forms.Panels
{
    class DrawingPanel : Panel
    {
        public static readonly int GRID_GAP = 32;
        public static readonly int MAX_SCALE = 5;

        public EventHandler<Point> DrawAtPoint;
        public EventHandler<Point> MouseLeftUp;

        protected int _scale = 1;
        protected int _width;
        protected int _height;
        protected Bitmap _bitmap;

        protected Point _mousePoint = Point.Empty;
        protected Point _mouseDown = Point.Empty;
        protected Point _offset = new Point(0, 0);
        protected Point _lastMousePoint = Point.Empty;

        private Matrix _transformMatrix = new Matrix();
        
        public DrawingPanel()
        {
            InitializeDrawingPanel();
        }

        virtual protected void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            ChangeTransForm(e);
            DrawGrid(e);
            DrawCurrentRect(e);
        }

        protected void ChangeTransForm(PaintEventArgs e)
        {
            // 변환 매트릭스 생성
            _transformMatrix = new Matrix();
            _transformMatrix.Translate(_offset.X, _offset.Y); // 팬 오프셋 적용
            _transformMatrix.Scale(_scale, _scale);       // 줌 적용

            // 그래픽스에 매트릭스 변환 적용
            e.Graphics.Transform = _transformMatrix;
        }

        protected void DrawCurrentRect(PaintEventArgs e)
        {
            // 마우스 위치 그리기
            int gridSize = GRID_GAP * _scale;
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Black);
            
            g.DrawRectangle(pen, new Rectangle(
                _mousePoint.X * gridSize,
                _mousePoint.Y * gridSize,
                gridSize,
                gridSize));

            pen.Dispose();
        }

        protected void DrawGrid(PaintEventArgs e)
        {
            // 그리드 선 그리기
            int gridSize = GRID_GAP * _scale;

            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.LightGray);

            for (int y = 0; y <= this.Height; y += gridSize)
            {
                g.DrawLine(pen,
                    0,
                    y,
                    this.Width,
                    y);
            }

            for (int x = 0; x <= this.Width; x += gridSize)
            {
                g.DrawLine(pen,
                    x,
                    0,
                    x,
                    this.Height);
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
            this.MouseDown += DrawingPanel_MouseDown;
            this.MouseMove += DrawingPanel_MouseMove;
            this.MouseUp += DrawingPanel_MouseUp;
            this.MouseClick += DrawingPanel_MouseClick;
            //this.MouseWheel += DrawingPanel_MouseWheel;
            this.Paint += DrawingPanel_Paint;
        }

        private void DrawingPanel_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseDown = e.Location;
            _lastMousePoint = e.Location;
        }

        private void DrawingPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseLeftUp?.Invoke(this, _mousePoint);
            }
        }

        private void DrawingPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int gridSize = GRID_GAP * _scale;
                DrawAtPoint.Invoke(this, _mousePoint);
            }
        }

        private void DrawingPanel_MouseMove(object sender, MouseEventArgs e)
        {
            // 마우스 드래그 시 팬 이동
            if (e.Button == MouseButtons.Right)
            {
                _offset.X += e.X - _lastMousePoint.X;
                _offset.Y += e.Y - _lastMousePoint.Y;
                _lastMousePoint = e.Location;
            }
            
            int gridSize = GRID_GAP * _scale;
            _mousePoint.X = (e.X - _offset.X) / gridSize;
            _mousePoint.Y = (e.Y - _offset.Y) / gridSize;

            if (e.Button == MouseButtons.Left)
            {
                DrawAtPoint.Invoke(this, _mousePoint);
            }
            
            this.Refresh();
        }

        private void DrawingPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            // 마우스 휠로 확대/축소 조정
            if (e.Delta > 0)
            {
                _scale += 1;
                _scale = Math.Min(_scale, MAX_SCALE);
            }
            else
            {
                _scale -= 1;
                _scale = Math.Max(_scale, 1);
            }
            
            // 패널 다시 그리기
            this.Invalidate();
        }
    }
}
