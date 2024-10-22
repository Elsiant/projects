using System.Drawing;

namespace MapEditor.Types
{
    class Tile
    {
        public static readonly int TILE_SIZE = 32;
        public int _x;
        public int _y;

        public Tile(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public Tile(Point point)
        {
            _x = point.X;
            _y = point.Y;
        }

        public Rectangle GetRect()
        {
            return new Rectangle(
                _x * TILE_SIZE,
                _y * TILE_SIZE,
                TILE_SIZE,
                TILE_SIZE);
        }
    }
}
