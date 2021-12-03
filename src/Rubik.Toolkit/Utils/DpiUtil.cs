using System.Windows;
using System.Windows.Media;

using Rubik.Toolkit.Datas;

namespace Rubik.Toolkit.Utils
{
    public static class DpiUtil
    {
        private const double DIP = 96.0;
        private const double DIU = 1 / 96.0;

        public static Point GetDpiFactor(Visual visual)
        {
            if (visual == null)
                return new Point();

            var source = PresentationSource.FromVisual(visual);
            var matrix = source.CompositionTarget.TransformToDevice;

            return new Point(matrix.M11, matrix.M22);
        }

        public static Point GetDpi(Visual visual)
        {
            var source = PresentationSource.FromVisual(visual);

            var dpiX = 96.0;
            var dpiY = 96.0;

            if (source?.CompositionTarget != null)
            {
                dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;
                dpiY = 96.0 * source.CompositionTarget.TransformToDevice.M22;
            }

            return new Point(dpiX, dpiY);
        }

        public static double GetDevicePixelUnit(Visual visual)
        {
            if (visual == null)
                return 0;

            return DIU * GetDpi(visual).X;
        }

        public static double PtToPixel(double pt)
        {
            return pt * 1 / 72.0 * DIP;
        }

        public static double GetPixelPerUnit(RulerUnit unit)
        {
            double result = 1;
            switch (unit)
            {
                case RulerUnit.Pixel:
                    result = 1;
                    break;
                case RulerUnit.Inch:
                    result = 96;
                    break;
                case RulerUnit.Foot:
                    result = 1152;
                    break;
                case RulerUnit.Millimeter:
                    result = 3.7795;
                    break;
                case RulerUnit.Centimeter:
                    result = 37.795;
                    break;
            }

            return result;
        }
    }
}
