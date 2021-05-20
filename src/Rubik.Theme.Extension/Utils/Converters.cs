using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Globalization;

using ICSharpCode.AvalonEdit.Editing;

namespace Rubik.Theme.Extension.Utils
{
    public class LineNumbersWithoutLineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var leftMargins = (ObservableCollection<UIElement>)value;
            return leftMargins.Where(lm => !DottedLineMargin.IsDottedLineMargin(lm));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
