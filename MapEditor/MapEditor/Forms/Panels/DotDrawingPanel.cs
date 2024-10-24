using System.Drawing;
using System.Windows.Forms;
using MapEditor.Types;

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
