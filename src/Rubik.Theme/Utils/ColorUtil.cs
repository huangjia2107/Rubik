using System;
using System.Windows.Media;

namespace Rubik.Theme.Utils
{
    public static class ColorUtil
    {
        public static Brush GetBrushFromString(string value)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(value));
        }

        public static Brush GetRandomBrush()
        {
            var rgb = GetRandomRGB();

            return new SolidColorBrush(Color.FromRgb((byte)rgb.R, (byte)rgb.G, (byte)rgb.B));
        }

        public static System.Drawing.Color GetRandomDrawingColor()
        {
            var rgb = GetRandomRGB();

            return System.Drawing.Color.FromArgb(rgb.R, rgb.G, rgb.B);
        }

        public static (int R, int G, int B) GetRandomRGB()
        {
            var tick = DateTime.Now.Ticks;
            var ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            var R = ran.Next(255);
            var G = ran.Next(255);
            var B = ran.Next(255);
            B = (R + G > 400) ? R + G - 400 : B;//0 : 380 - R - G;
            B = (B > 255) ? 255 : B;

            return (R, G, B);
        }
    }
}
