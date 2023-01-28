using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Rubik.Theme.Utils
{
    public static class DrawUtil
    {
        public static FormattedText DrawText(string text, Brush brush, double fontSize = 13)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            var ft = new FormattedText(
                       text,
                       CultureInfo.CurrentCulture,
                       FlowDirection.LeftToRight,
                       new Typeface("Arial"),
                       fontSize,
                       brush,
                       1);

            ft.SetFontWeight(FontWeights.Normal);
            ft.TextAlignment = TextAlignment.Left;

            return ft;
        }
    }
}
