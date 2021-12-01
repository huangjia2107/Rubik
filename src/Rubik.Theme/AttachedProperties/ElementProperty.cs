using System.Windows;
using System.Windows.Controls;

namespace Rubik.Theme.AttachedProperties
{
    public class ElementProperty
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(ElementProperty), new FrameworkPropertyMetadata(new CornerRadius(0d)));
        public static CornerRadius GetIsAttached(DependencyObject dpo)
        {
            return (CornerRadius)dpo.GetValue(CornerRadiusProperty);
        }
        public static void SetIsAttached(DependencyObject dpo, CornerRadius value)
        {
            dpo.SetValue(CornerRadiusProperty, value);
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
