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
        private readonly int LAYER_MAX = 5;
        private int _width = 64;
        private int _height = 64;
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

        private void Resize(int width, int height)
        {
            _width = width;
            _height = height;
        }
        
    }
}
