using System;
using System.Windows;

using Rubik.Toolkit.Utils;

namespace Rubik.Theme.Extensions
{
    public static class CornerRadiusExtensions
    {
        public static bool IsValid(this CornerRadius cornerRadius, bool allowNegative, bool allowNaN, bool allowPositiveInfinity, bool allowNegativeInfinity)
        {
            if (!allowNegative)
            {
                if (DoubleUtil.LessThan(cornerRadius.BottomLeft, 0)
                || DoubleUtil.LessThan(cornerRadius.BottomRight, 0)
                || DoubleUtil.LessThan(cornerRadius.TopLeft, 0)
                || DoubleUtil.LessThan(cornerRadius.TopRight, 0))
                {
                    return false;
                }
            }

            if (!allowNaN)
            {
                if (double.IsNaN(cornerRadius.BottomLeft) || double.IsNaN(cornerRadius.BottomRight) || double.IsNaN(cornerRadius.TopLeft) || double.IsNaN(cornerRadius.TopRight))
                {
                    return false;
                }
            }

            if (!allowPositiveInfinity)
            {
                if (double.IsPositiveInfinity(cornerRadius.BottomLeft) || double.IsPositiveInfinity(cornerRadius.BottomRight) || double.IsPositiveInfinity(cornerRadius.TopLeft) || double.IsPositiveInfinity(cornerRadius.TopRight))
                {
                    return false;
                }
            }

            if (!allowNegativeInfinity)
            {
                if (double.IsNegativeInfinity(cornerRadius.BottomLeft) || double.IsNegativeInfinity(cornerRadius.BottomRight) || double.IsNegativeInfinity(cornerRadius.TopLeft) || double.IsNegativeInfinity(cornerRadius.TopRight))
                {
                    return false;
                }
            }

            return true;
        }

        public static CornerRadius Coerce(this CornerRadius cornerRadius, double availableWidth, double availableHeight)
        {
            double? topLeft = null;
            double? bottomLeft = null;
            if (DoubleUtil.LessThan(availableHeight, cornerRadius.TopLeft + cornerRadius.BottomLeft))
            {
                topLeft = cornerRadius.TopLeft * availableHeight / (cornerRadius.TopLeft + cornerRadius.BottomLeft);
                bottomLeft = cornerRadius.BottomLeft * availableHeight / (cornerRadius.TopLeft + cornerRadius.BottomLeft);
            }

            double? topRight = null;
            double? bottomRight = null;
            if (DoubleUtil.LessThan(availableHeight, cornerRadius.TopRight + cornerRadius.BottomRight))
            {
                topRight = cornerRadius.TopRight * availableHeight / (cornerRadius.TopRight + cornerRadius.BottomRight);
                bottomRight = cornerRadius.BottomRight * availableHeight / (cornerRadius.TopRight + cornerRadius.BottomRight);
            }

            if (DoubleUtil.LessThan(availableWidth, cornerRadius.TopLeft + cornerRadius.TopRight))
            {
                var tl = cornerRadius.TopLeft * availableWidth / (cornerRadius.TopLeft + cornerRadius.TopRight);
                topLeft = topLeft == null ? tl : Math.Min(tl, topLeft.Value);

                var tr = cornerRadius.TopRight * availableWidth / (cornerRadius.TopLeft + cornerRadius.TopRight);
                topRight = topRight == null ? tr : Math.Min(tr, topRight.Value);
            }

            if (DoubleUtil.LessThan(availableWidth, cornerRadius.BottomLeft + cornerRadius.BottomRight))
            {
                var bl = cornerRadius.BottomLeft * availableWidth / (cornerRadius.BottomLeft + cornerRadius.BottomRight);
                bottomLeft = bottomLeft == null ? bl : Math.Min(bl, bottomLeft.Value);

                var br = cornerRadius.BottomRight * availableWidth / (cornerRadius.BottomLeft + cornerRadius.BottomRight);
                bottomRight = bottomRight == null ? br : Math.Min(br, bottomRight.Value);
            }

            if (topLeft != null || topRight != null || bottomLeft != null || bottomRight != null)
                return new CornerRadius(topLeft.Value, topRight.Value, bottomRight.Value, bottomLeft.Value);

            return cornerRadius;
        }
    }
}
