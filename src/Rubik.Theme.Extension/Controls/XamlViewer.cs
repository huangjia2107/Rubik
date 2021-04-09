using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Markup;

using ICSharpCode.AvalonEdit;

using Rubik.Theme.Utils;

namespace Rubik.Theme.Extension.Controls
{
    [TemplatePart(Name = TextEditorTemplateName, Type = typeof(TextEditor))]
    public class XamlViewer : Control
    {
        private static readonly Type _typeofSelf = typeof(XamlViewer);

        private const string TextEditorTemplateName = "PART_TextEditor";
        private TextEditor _partTextEditor = null;

        static XamlViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(_typeofSelf, new FrameworkPropertyMetadata(_typeofSelf));
        }

        #region Properties

        private static readonly DependencyPropertyKey ContentPropertyKey =
            DependencyProperty.RegisterReadOnly("Content", typeof(object), _typeofSelf, new PropertyMetadata(null));
        public static readonly DependencyProperty ContentProperty = ContentPropertyKey.DependencyProperty;
        public object Content
        {
            get { return GetValue(ContentProperty); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), _typeofSelf, new PropertyMetadata(null));
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty XamlProperty = DependencyProperty.Register("Xaml", typeof(string), _typeofSelf, new PropertyMetadata(null, OnXamlPropertyChanged));
        public string Xaml
        {
            get { return (string)GetValue(XamlProperty); }
            set { SetValue(XamlProperty, value); }
        }

        private static void OnXamlPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as XamlViewer;
            var xaml = (string)(e.NewValue ?? string.Empty);

            if (ctrl._partTextEditor != null)
                ctrl._partTextEditor.Text = xaml;

            ctrl.ParseXaml(xaml);
        }

        public static readonly DependencyProperty WordWrapProperty = TextEditor.WordWrapProperty.AddOwner(_typeofSelf);
        public bool WordWrap
        {
            get { return (bool)GetValue(WordWrapProperty); }
            set { SetValue(WordWrapProperty, value); }
        }

        public static readonly DependencyProperty ShowLineNumbersProperty = TextEditor.ShowLineNumbersProperty.AddOwner(_typeofSelf);
        public bool ShowLineNumbers
        {
            get { return (bool)GetValue(ShowLineNumbersProperty); }
            set { SetValue(ShowLineNumbersProperty, value); }
        }

        #endregion

        #region Override

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            if (Focusable && _partTextEditor != null)
                _partTextEditor.Focus();
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);

            if (_partTextEditor == null)
                return;

            var textEditor = TreeUtil.FindVisualParent<TextEditor>(e.OriginalSource as DependencyObject);
            if (textEditor == null)
                return;

            if (e.Delta > 0)
                _partTextEditor.ScrollToHorizontalOffset(Math.Max(0, _partTextEditor.HorizontalOffset - e.Delta));
            else
                _partTextEditor.ScrollToHorizontalOffset(_partTextEditor.HorizontalOffset - e.Delta);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _partTextEditor = GetTemplateChild(TextEditorTemplateName) as TextEditor;

            if (_partTextEditor != null)
            {
                _partTextEditor.Options = new TextEditorOptions { ConvertTabsToSpaces = true };
                _partTextEditor.TextArea.SelectionCornerRadius = 0;
                _partTextEditor.TextArea.SelectionBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFADD6FF"));
                _partTextEditor.TextArea.SelectionBorder = null;
                _partTextEditor.TextArea.SelectionForeground = null;

                _partTextEditor.Text = Xaml;
            }
        }

        #endregion

        #region Func

        private void ParseXaml(string xaml)
        {
            try
            {
                SetValue(ContentPropertyKey, XamlReader.Parse(xaml));
            }
            catch (Exception ex)
            {
                ShowLocalText("Error: " + ex.Message);
            }
        }

        private void ShowLocalText(string text, double fontSize = 14d)
        {
            SetValue(ContentPropertyKey, new TextBlock
            {
                Text = text,
                Margin = new Thickness(5),
                FontSize = fontSize,
                TextWrapping = TextWrapping.Wrap,
                Foreground = Brushes.DarkSlateGray,
                FontWeight = FontWeights.Medium,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            });
        }

        #endregion
    }
}
