using System.Drawing;
using System.Windows.Forms;
using MapEditor.Types;
using Newtonsoft.Json;

namespace MapEditor.Forms.Panels
{
    class DotDrawingPanel : DrawingPanel
    {
        private Color[,] _dotColors;

        public DotDrawingPanel()
        {
            this.Width = GRID_GAP * Tile.TILE_SIZE;
            this.Height = GRID_GAP * Tile.TILE_SIZE;
            _width = Tile.TILE_SIZE;
            _height = Tile.TILE_SIZE;

            _dotColors = new Color[_width, _height];
        }
        
        public void SetDotColor(Point point, Color color)
        {
            _dotColors[point.X, point.Y] = color;

            this.Refresh();
        }

        public Bitmap GetBitmap()
        {
            var bitmap = new Bitmap(Tile.TILE_SIZE, Tile.TILE_SIZE);

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    if (_dotColors[x, y] == Color.Empty)
                    {
                        continue;
                    }

                    bitmap.SetPixel(x, y, _dotColors[x, y]);
                }
            }

            return bitmap;
        }

        public void SetBitmap(Bitmap bitmap)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    _dotColors[x, y] = bitmap.GetPixel(x, y);
                }
            }

            this.Refresh();
        }

        override protected void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int gridSize = GRID_GAP * _scale;

            // 도트 그리기
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
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

            base.DrawingPanel_Paint(sender, e);
        }
    }
}
