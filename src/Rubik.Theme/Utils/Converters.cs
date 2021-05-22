using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Collections;
using System.Linq;

namespace Rubik.Theme.Utils
{
    public class DoubleToGridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (double)value;
            return new GridLength(v);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class KeyToValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dic = (IDictionary)parameter;

            return dic[value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ScrollOffsetToVisibilityMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Contains(null) || values.Contains(DependencyProperty.UnsetValue))
                return Visibility.Collapsed;

            var offset = (double)values[0];
            var Length = (double)values[1];

            if (DoubleUtil.IsNaN(Length) || DoubleUtil.IsZero(Length))
                return Visibility.Collapsed;

            var isLeftOrTop = parameter == null;

            if (isLeftOrTop)
                return DoubleUtil.IsZero(offset) ? Visibility.Collapsed : Visibility.Visible;

            return DoubleUtil.AreClose(offset, Length) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
