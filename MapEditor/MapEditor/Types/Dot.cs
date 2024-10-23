using System.Drawing;

namespace MapEditor.Types
{
    class Dot
    {
        public int _x;
        public int _y;
        public Color _color;

        public Dot(int x, int y, Color color)
        {
            _x = x;
            _y = y;
            _color = color;
        }

        public Dot(Point point, Color color)
        {
            _x = point.X;
            _y = point.Y;
            _color = color;
        }
    }
}
