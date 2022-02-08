using System.Windows;
using System.Windows.Controls;

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
    }
}
