using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    internal class Constants
    {
        public static readonly SKSizeI IMG_SIZE = new(200_000, 200);
        public static double PIXEL_PER_METER = 200;

        // jpeg形式などは縦横65535までの規格なのでpngくらいしか使えない
        public static readonly String IMG_PATH = "quickstart.png";
    }
}
