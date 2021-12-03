using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Collections;
using System.Linq;

using Rubik.Toolkit.Datas;

namespace Rubik.Toolkit.Utils
{
    public class BoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var trueVisibility = (Visibility)(parameter ?? Visibility.Visible);
            return (bool)value ? trueVisibility : (trueVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

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

    public class DoubleToCornerRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (double)value;
            var ratio = parameter == null ? 1d : double.Parse(parameter.ToString());

            return new CornerRadius(v * ratio);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EnumToVisibilityConverter : IValueConverter
    {
        public Visibility EqualVisibility { get; set; } = Visibility.Visible;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var originalValue = (int)value;
            var compareValue = (int)System.Convert.ChangeType(parameter, typeof(int));

            return originalValue == compareValue ? EqualVisibility : (EqualVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible);
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

    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nullVisibility = (Visibility)(parameter ?? Visibility.Collapsed);
            return value == null ? nullVisibility : (nullVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeSpan = (TimeSpan)value;
            return $"{(int)timeSpan.TotalHours:#00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)value == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ZoomBoxRulerShiftMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Contains(null) || values.Contains(DependencyProperty.UnsetValue))
                return 0d;

            var originShift = (double)values[0];
            var scale = (double)values[1];
            var unit = (RulerUnit)values[2];

            return originShift / DpiUtil.GetPixelPerUnit(unit) / scale;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
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
