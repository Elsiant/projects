using MapEditor.Data;
using MapEditor.Types;
using MapEditor.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapEditor.Forms.Panels
{
    class MapDrawingPanel : DrawingPanel
    {
        private readonly int LAYER_MAX = 3; // 0 바닥, 1 건물, 2 오브젝트
        private int _column = 64;
        private int _row = 64;
        private int _currentLayer = 0;

        private Dictionary<string, Tile>[] _tiles;
        private Bitmap[] _mapImages;

        public MapDrawingPanel()
        {
            _tiles = new Dictionary<string, Tile>[LAYER_MAX];
            _mapImages = new Bitmap[LAYER_MAX];

            for(int i = 0; i < LAYER_MAX; i++)
            {
                _tiles[i] = new Dictionary<string, Tile>();
                _mapImages[i] = new Bitmap(Tile.TILE_SIZE * _column, Tile.TILE_SIZE * _row);
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

            // Point 타입은 키값으로 적절하지 않기 떄문에 고유한 hash 값으로 변환해 키로 사용한다.
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
            
            // 변경된 Tile에 맞게 _mapImage 수정
            var image = TileManager.Instance.GetImage(fileName);
            int x = point.X * Tile.TILE_SIZE;
            int y = point.Y * Tile.TILE_SIZE;

            for (int i = 0; i < Tile.TILE_SIZE; i++)
            {
                for(int j = 0; j < Tile.TILE_SIZE; j++)
                {
                    _mapImages[_currentLayer].SetPixel(
                        x + i, y + j, image.GetPixel(i, j)
                        );
                }
            }

            this.Refresh();
        }

        override protected void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

            for (int i = 0; i < LAYER_MAX; i++)
            {
                g.DrawImage(_mapImages[i],
                _offset.X + 0,
                _offset.Y + 0,
                Width,
                Height);
            }

            base.ChangeTransForm(e);
            MapPanelDrawGrid(e);
            base.DrawCurrentRect(e);
        }

        private void MapPanelDrawGrid(PaintEventArgs e)
        {
            // 그리드 선 그리기
            int gridSize = GRID_GAP * _scale;
            int width = gridSize * _column;
            int height = gridSize * _row;

            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.LightGray);

            for (int y = 0; y <= width; y += gridSize)
            {
                g.DrawLine(pen,
                    0,
                    y,
                    width,
                    y);
            }

            for (int x = 0; x <= height; x += gridSize)
            {
                g.DrawLine(pen,
                    x,
                    0,
                    x,
                    height);
            }

            pen.Dispose();
        }

        public Dictionary<string, Tile>[] GetTilesData()
        {
            return _tiles;
        }

        public void SetTilesData(Dictionary<string, Tile>[] tiles)
        {
            _tiles = tiles;

            this.Refresh();
        }
        

        private void ChangeLayer(int layer)
        {
            _currentLayer = layer;
        }

        private void ChangeMapSize(int column, int row)
        {
            _column = column;
            _row = row;
            this.Refresh();
        }
        
    }
}
