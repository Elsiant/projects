using System;
using System.Drawing;

namespace MapEditor.Types
{
    class Tile
    {
        public static readonly int TILE_SIZE = 16;
        public int _x;
        public int _y;

        private string _fileName = string.Empty;
        private Bitmap _bitmap = null;

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

        public Tile(string data)
        {
            var datas = data.Split(',');

            if(datas.Length != 3)
            {
                Console.WriteLine($"Error Invalid data : {data}");
                return;
            }

            if(Int32.TryParse(datas[0], out _x) == false)
            {
                Console.WriteLine($"Error Invalid data _x : {data[0]}");
                return;
            }

            if (Int32.TryParse(datas[1], out _y) == false)
            {
                Console.WriteLine($"Error Invalid data _y : {data[1]}");
                return;
            }

            _fileName = datas[2];
        }

        public Rectangle GetRect()
        {
            return new Rectangle(
                _x * TILE_SIZE,
                _y * TILE_SIZE,
                TILE_SIZE,
                TILE_SIZE);
        }

        public void SetFileName(string fileName)
        {
            _fileName = fileName;
        }

        public string GetFileName()
        {
            return _fileName;
        }

        public Point GetPoint()
        {
            return new Point(_x, _y);
        }

        public string GetData()
        {
            return $"{_x},{_y},{_fileName}";
        }
    }
}
