using MapEditor.Types;
using MapEditor.Util;
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
        private readonly int LAYER_MAX = 5;
        private int _column = 64;
        private int _row = 64;
        private int _currentLayer = 0;

        private Dictionary<string, Tile>[] _tiles;
        public MapDrawingPanel()
        {
            _tiles = new Dictionary<string, Tile>[LAYER_MAX];

            for(int i = 0; i < LAYER_MAX; i++)
            {
                _tiles[i] = new Dictionary<string, Tile>();
            }
        }
        
        public void SetImage(Point point, string fileName)
        {
            //Tile tile = _tileList[_currentLayer].Find(
            //    item => item._x == point.X && item._y == point.Y);

            //if (tile == null)
            //{
            //    Tile newTile = new Tile(point);
            //    newTile.SetImage(fileName);
            //    _tileList[_currentLayer].Add(newTile);
            //}
            //else
            //{
            //    tile.SetImage(fileName);
            //}

            var hashKey = PointHashGenerator.GenerateHash(point);
            if(_tiles[_currentLayer].ContainsKey(hashKey))
            {
                _tiles[_currentLayer][hashKey].SetImage(fileName);
            }
            else
            {
                Tile newTile = new Tile(point);
                newTile.SetImage(fileName);
                _tiles[_currentLayer].Add(hashKey, newTile);
            }
        }

        //public void MapDrawingPanel_Paint(object sender, PaintEventArgs e)
        //{

        //}

        public Dictionary<string, Tile>[] GetTilesData()
        {
            return _tiles;
        }

        public void SetTilesData(Dictionary<string, Tile>[] tiles)
        {
            _tiles = tiles;
        }
        

        private void ChangeLayer(int layer)
        {
            _currentLayer = layer;
        }

        private void ChangeMapSize(int column, int row)
        {
            _column = column;
            _row = row;
        }
        
    }
}
