using MapEditor.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

        private List<Tile>[] _tileList;
        public MapDrawingPanel()
        {
            _tileList = new List<Tile>[LAYER_MAX];

            for(int i = 0; i < LAYER_MAX; i++)
            {
                _tileList[i] = new List<Tile>();
            }
        }
        
        public void SetImage(Point point, string fileName)
        {
            Tile tile = _tileList[_currentLayer].Find(
                item => item._x == point.X && item._y == point.Y);

            if (tile == null)
            {
                Tile newTile = new Tile(point);
                newTile.SetImage(fileName);
                _tileList[_currentLayer].Add(newTile);
            }
            else
            {
                tile.SetImage(fileName);
            }
        }

        //public void MapDrawingPanel_Paint(object sender, PaintEventArgs e)
        //{

        //}

        private void SaveData(string filePath)
        {
            string json = JsonConvert.SerializeObject(_tileList, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        private void LoadData(string filePath)
        {
            string json = File.ReadAllText(filePath);
            _tileList = JsonConvert.DeserializeObject<List<Tile>[]>(json);
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
