using MapEditor.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.Forms.Panels
{
    class MapDrawingPanel : DrawingPanel
    {
        private int _column = 64;
        private int _row = 64;
        private int _currentLayer = 0;

        private Tile[,] _tiles;
        public MapDrawingPanel()
        {
            _tiles = new Tile[_column, _row];
        }

        public void SetImage(Point point, string fileName)
        {
            _tiles[point.X, point.Y] = new Tile(point);
            //_tiles[point.X, point.Y] = new Tile(_fileName);
        }

        //public void MapDrawingPanel_Paint(object sender, PaintEventArgs e)
        //{

        //}

        private void SaveData(string fileName)
        {

        }

        private void LoadData(string fileName)
        {

        }

        private void ChangeLayer(int layer)
        {
            _currentLayer = layer;
        }
    }
}
