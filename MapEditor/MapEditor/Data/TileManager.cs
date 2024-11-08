using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.Data
{
    class TileManager : IDisposable
    {
        private static Dictionary<string, Bitmap> _tilesImage = new Dictionary<string, Bitmap>();
        private static readonly TileManager _instance = new TileManager();
        public static TileManager Instance => _instance;

        private TileManager()
        {

        }

        public void Dispose()
        {
            foreach(var tileImage in _tilesImage.Values)
            {
                tileImage.Dispose();
            }
            _tilesImage.Clear();
        }

        public Bitmap LoadImage(string fileName, bool reload)
        {
            if(reload == false &&
                _tilesImage.ContainsKey(fileName))
            {
                return _tilesImage[fileName];
            }

            var bitmap = new Bitmap(fileName);

            if(_tilesImage.ContainsKey(fileName))
            {
                _tilesImage[fileName]?.Dispose();
                _tilesImage[fileName] = bitmap;
            }
            else
            {
                _tilesImage.Add(fileName, bitmap);
            }
            
            return bitmap;
        }

        public Bitmap GetImage(string fileName)
        {
            if (_tilesImage.ContainsKey(fileName))
            {
                return _tilesImage[fileName];
            }
            else
            {
                Console.WriteLine($"Error Invalid fileName : {fileName}");
                return null;
            }
        }

    }
}
