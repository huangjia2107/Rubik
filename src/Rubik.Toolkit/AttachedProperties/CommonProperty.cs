using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rubik.Toolkit.AttachedProperties
{
    public partial class ElementProperty
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(ElementProperty), new FrameworkPropertyMetadata(new CornerRadius(0d)));
        public static CornerRadius GetCornerRadius(DependencyObject obj)
        {
            return (CornerRadius)obj.GetValue(CornerRadiusProperty);
        }
        public static void SetCornerRadius(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.RegisterAttached("Orientation", typeof(Orientation), typeof(ElementProperty), new PropertyMetadata(Orientation.Horizontal));
        public static Orientation GetOrientation(DependencyObject obj)
        {
            return (Orientation)obj.GetValue(OrientationProperty);
        }
        public static void SetOrientation(DependencyObject obj, Orientation value)
        {
            obj.SetValue(OrientationProperty, value);
        }

        public static readonly DependencyProperty CursorSourceProperty = DependencyProperty.RegisterAttached("CursorSource", typeof(string), typeof(ElementProperty), new PropertyMetadata(OnCursorSourcePropertyChanged));
        public static string GetCursorSource(DependencyObject obj)
        {
            return (string)obj.GetValue(CursorSourceProperty);
        }
        public static void SetCursorSource(DependencyObject obj, string value)
        {
            obj.SetValue(CursorSourceProperty, value);
        }
        static void OnCursorSourcePropertyChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = target as FrameworkElement;
            var uri = (string)e.NewValue;

            if (ctrl == null || string.IsNullOrWhiteSpace(uri))
                return;

            var streamInfo = Application.GetResourceStream(new Uri(uri, UriKind.Relative));
            if (streamInfo == null)
                return;

            using (var stream = streamInfo.Stream)
            {
                ctrl.Cursor = new Cursor(stream);
            }
        }
    }
}
