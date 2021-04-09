using System.Windows;

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
    }
}
