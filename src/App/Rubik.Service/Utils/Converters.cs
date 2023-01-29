﻿using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows;

namespace Rubik.Service.Utils
{
    public class BoolOrBoolToBoolMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Contains(DependencyProperty.UnsetValue) || values.Contains(null))
                return false;

            return !(bool)values[0] && !(bool)values[1];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
