using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Rubik.Toolkit.AttachedProperties
{
    public class PasswordBoxBinding
    {
        public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached("Attach", typeof(bool), typeof(PasswordBoxBinding), new PropertyMetadata(OnAttachPropertyChanged));
        public static bool GetAttach(DependencyObject obj)
        {
            return (bool)obj.GetValue(AttachProperty);
        }
        public static void SetAttach(DependencyObject obj, bool value)
        {
            obj.SetValue(AttachProperty, value);
        }

        private static void OnAttachPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = d as PasswordBox;
            if (passwordBox == null)
                return;

            var isAttach = (bool)e.NewValue;

            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
            if (isAttach)
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;

            if (GetPassword(passwordBox) != passwordBox.Password)
            {
                SetPassword(passwordBox, passwordBox.Password);
                passwordBox.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(passwordBox, new object[] { passwordBox.Password.Length, 0 });
            }
        }

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxBinding), new PropertyMetadata(OnPasswordPropertyChanged));
        public static string GetPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordProperty);
        }
        public static void SetPassword(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = d as PasswordBox;
            if (passwordBox == null)
                return;

            passwordBox.Password = (string)e.NewValue;
        }
    }
}
