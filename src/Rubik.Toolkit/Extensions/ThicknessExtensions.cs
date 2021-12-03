using System.Windows;

namespace Rubik.Toolkit.Extensions
{
    public static class ThicknessExtensions
    {
        public static bool IsValid(this Thickness thickness, bool allowNegative, bool allowNaN, bool allowPositiveInfinity, bool allowNegativeInfinity)
        {
            if (!allowNegative)
            {
                if (thickness.Left < 0d || thickness.Right < 0d || thickness.Top < 0d || thickness.Bottom < 0d)
                {
                    return false;
                }
            }

            if (!allowNaN)
            {
                if (double.IsNaN(thickness.Left) || double.IsNaN(thickness.Right) || double.IsNaN(thickness.Top) || double.IsNaN(thickness.Bottom))
                {
                    return false;
                }
            }

            if (!allowPositiveInfinity)
            {
                if (double.IsPositiveInfinity(thickness.Left) || double.IsPositiveInfinity(thickness.Right) || double.IsPositiveInfinity(thickness.Top) || double.IsPositiveInfinity(thickness.Bottom))
                {
                    return false;
                }
            }

            if (!allowNegativeInfinity)
            {
                if (double.IsNegativeInfinity(thickness.Left) || double.IsNegativeInfinity(thickness.Right) || double.IsNegativeInfinity(thickness.Top) || double.IsNegativeInfinity(thickness.Bottom))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
