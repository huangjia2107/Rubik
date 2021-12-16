using System;
using System.Windows.Media;

namespace Rubik.Toolkit.Utils
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

        public static Color InvertColor(Color color)
        {
            return Color.FromRgb((byte)~color.R, (byte)~color.G, (byte)~color.B);
        }

        public static Color ColorFromAhsb(double a, double h, double s, double b)
        {
            var r = ColorFromHsb(h, s, b);
            r.A = (byte)Math.Round(a * 255d);

            return r;
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

        public static (double H, double S, double B) HsbFromColor(Color C)
        {
            double r = C.R / 255d;
            double g = C.G / 255d;
            double b = C.B / 255d;

            var max = Math.Max(Math.Max(r, g), b);
            var min = Math.Min(Math.Min(r, g), b);
            var delta = max - min;

            var hue = 0d;
            var saturation = DoubleUtil.GreaterThan(max, 0) ? (delta / max) : 0.0;
            var brightness = max;

            if (!DoubleUtil.IsZero(delta))
            {
                if (DoubleUtil.AreClose(r, max))
                    hue = (g - b) / delta;
                else if (DoubleUtil.AreClose(g, max))
                    hue = 2 + (b - r) / delta;
                else if (DoubleUtil.AreClose(b, max))
                    hue = 4 + (r - g) / delta;

                hue = hue * 60;
                if (DoubleUtil.LessThan(hue, 0d))
                    hue += 360;
            }

            return (hue / 360d, saturation, brightness);
        }

        public static Color ColorFromHsb(double H, double S, double B)
        {
            double red = 0.0, green = 0.0, blue = 0.0;

            if (DoubleUtil.IsZero(S))
                red = green = blue = B;
            else
            {
                var h = DoubleUtil.IsOne(H) ? 0d : (H * 6.0);
                int i = (int)Math.Floor(h);

                var f = h - i;
                var r = B * (1.0 - S);
                var s = B * (1.0 - S * f);
                var t = B * (1.0 - S * (1.0 - f));

                switch (i)
                {
                    case 0: red = B; green = t; blue = r; break;
                    case 1: red = s; green = B; blue = r; break;
                    case 2: red = r; green = B; blue = t; break;
                    case 3: red = r; green = s; blue = B; break;
                    case 4: red = t; green = r; blue = B; break;
                    case 5: red = B; green = r; blue = s; break;
                }
            }

            return Color.FromRgb((byte)Math.Round(red * 255.0), (byte)Math.Round(green * 255.0), (byte)Math.Round(blue * 255.0));
        }
    }
}
