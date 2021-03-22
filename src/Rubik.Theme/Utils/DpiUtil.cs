using System.Windows;
using System.Windows.Media;

namespace Rubik.Theme.Utils
{
    public static class DpiUtil
    {
        public static (double X, double Y) GetDpi(Visual visual)
        {
            var source = PresentationSource.FromVisual(visual);

            var dpiX = 96.0;
            var dpiY = 96.0;

            if (source?.CompositionTarget != null)
            {
                dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;
                dpiY = 96.0 * source.CompositionTarget.TransformToDevice.M22;
            }

            return (dpiX, dpiY);
        }
    }
}
