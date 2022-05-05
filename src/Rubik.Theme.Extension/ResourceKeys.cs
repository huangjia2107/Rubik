using System;

namespace Rubik.Theme.Extension
{
    public static class ResourceKeys
    {
        #region Xaml

        public static Uri RubikExtensionThemeSource { get; } = new Uri("pack://application:,,,/Rubik.Theme.Extension;component/Themes/Generic.xaml");

        public static Uri CodeCompletionStyleSource { get; } = new Uri("pack://application:,,,/Rubik.Theme.Extension;component/Styles/CodeCompletionStyle.xaml");
        public static Uri ExpressionEditorStyleSource { get; } = new Uri("pack://application:,,,/Rubik.Theme.Extension;component/Styles/ExpressionEditorStyle.xaml");

        public static Uri CodeViewerStyleSource { get; } = new Uri("pack://application:,,,/Rubik.Theme.Extension;component/Styles/CodeViewerStyle.xaml");
        public static Uri LiveXamlStyleSource { get; } = new Uri("pack://application:,,,/Rubik.Theme.Extension;component/Styles/LiveXamlStyle.xaml");

        #endregion
    }
}
