using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.Util
{
    class PointHashGenerator
    {
        public static string GenerateHash(Point point)
        {
            string pointData = $"{point.X},{point.Y}";

            byte[] byteData = Encoding.UTF8.GetBytes(pointData);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(byteData);

                StringBuilder hashString = new StringBuilder();

                foreach(byte b in hashBytes)
                {
                    hashString.Append(b.ToString("x2"));
                }

                return hashString.ToString();
            }
        }
    }
}
