using System.Drawing;
using System.Windows.Forms;
using MapEditor.Types;
using Newtonsoft.Json;

namespace MapEditor.Forms.Panels
{
    class DotDrawingPanel : DrawingPanel
    {
        public DotDrawingPanel()
        {
            this.Width = GRID_GAP * Tile.TILE_SIZE;
            this.Height = GRID_GAP * Tile.TILE_SIZE;
            _width = Tile.TILE_SIZE;
            _height = Tile.TILE_SIZE;
            
            _bitmap = new Bitmap(_width, _height);
        }
        
        public void SetDotColor(Point point, Color color)
        {
            _bitmap.SetPixel(point.X, point.Y, color);

            this.Refresh();
        }

        public Bitmap GetBitmap()
        {
            return _bitmap;
        }

        public void SetBitmap(Bitmap bitmap)
        {
            _bitmap = bitmap;

            this.Refresh();
        }

        override protected void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            g.DrawImage(_bitmap,
                _offset.X + 0,
                _offset.Y + 0, 
                Width, 
                Height);

            base.DrawingPanel_Paint(sender, e);
        }
    }
}
