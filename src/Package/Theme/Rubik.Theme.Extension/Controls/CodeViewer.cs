using System;
using System.IO;
using System.Xml;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Markup;
using System.ComponentModel;

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

using Rubik.Toolkit.Utils;
using Rubik.Theme.UI;

namespace Rubik.Theme.Extension.Controls
{
    [TemplatePart(Name = TextEditorTemplateName, Type = typeof(TextEditor))]
    public class CodeViewer : Control
    {
        private static readonly Type _typeofSelf = typeof(CodeViewer);

        private const string TextEditorTemplateName = "PART_TextEditor";
        private TextEditor _partTextEditor = null;

        static CodeViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(_typeofSelf, new FrameworkPropertyMetadata(_typeofSelf));
        }

        #region Properties

        public static readonly DependencyProperty SyntaxHighlightingProperty = DependencyProperty.Register("SyntaxHighlighting", typeof(IHighlightingDefinition), _typeofSelf, new PropertyMetadata(OnSyntaxHighlightingPropertyChanged));
        [TypeConverter(typeof(HighlightingDefinitionTypeConverter))]
        public IHighlightingDefinition SyntaxHighlighting
        {
            get { return (IHighlightingDefinition)GetValue(SyntaxHighlightingProperty); }
            set { SetValue(SyntaxHighlightingProperty, value); }
        }

        private static void OnSyntaxHighlightingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as CodeViewer;
            ctrl.ParseXaml(ctrl.Code);
        }

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

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), _typeofSelf, new PropertyMetadata(null));
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty CodeProperty = DependencyProperty.Register("Code", typeof(string), _typeofSelf, new PropertyMetadata(null, OnCodePropertyChanged));
        public string Code
        {
            get { return (string)GetValue(CodeProperty); }
            set { SetValue(CodeProperty, value); }
        }

        private static void OnCodePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as CodeViewer;
            var code = (string)(e.NewValue ?? string.Empty);

            if (ctrl._partTextEditor != null)
                ctrl._partTextEditor.Text = code;

            ctrl.ParseXaml(code);
        }

        public static readonly DependencyProperty JustShowCodeProperty =
            DependencyProperty.Register("JustShowCode", typeof(bool), _typeofSelf, new PropertyMetadata(false, OnJustShowCodePropertyChanged));
        public bool JustShowCode
        {
            get { return (bool)GetValue(JustShowCodeProperty); }
            set { SetValue(JustShowCodeProperty, value); }
        }

        private static void OnJustShowCodePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as CodeViewer;
            ctrl.ParseXaml(ctrl.Code);
        }

        public static readonly DependencyProperty AllowCollapseCodeProperty =
            DependencyProperty.Register("AllowCollapseCode", typeof(bool), _typeofSelf, new PropertyMetadata(true));
        public bool AllowCollapseCode
        {
            get { return (bool)GetValue(AllowCollapseCodeProperty); }
            set { SetValue(AllowCollapseCodeProperty, value); }
        }

        public static readonly DependencyProperty WordWrapProperty = TextEditor.WordWrapProperty.AddOwner(_typeofSelf);
        public bool WordWrap
        {
            get { return (bool)GetValue(WordWrapProperty); }
            set { SetValue(WordWrapProperty, value); }
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

            var scrollViewer = TreeUtil.FindVisualChild<ScrollViewer>(textEditor);
            if (scrollViewer != null
                && Keyboard.Modifiers == ModifierKeys.Control
                && DoubleUtil.GreaterThan(scrollViewer.ScrollableWidth, 0))
            {
                if (e.Delta > 0)
                    _partTextEditor.ScrollToHorizontalOffset(Math.Max(0, _partTextEditor.HorizontalOffset - e.Delta));
                else
                    _partTextEditor.ScrollToHorizontalOffset(_partTextEditor.HorizontalOffset - e.Delta);

                return;
            }

            if (_partTextEditor.VerticalScrollBarVisibility == ScrollBarVisibility.Disabled
                || scrollViewer != null && DoubleUtil.IsZero(scrollViewer.ScrollableHeight))
            {
                this.RaiseEvent(new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                {
                    RoutedEvent = MouseWheelEvent,
                    Source = this
                });
            }
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

                _partTextEditor.Text = Code;
            }
        }

        #endregion

        #region Func

        public void LoadSyntaxHighlighting(string fileName)
        {
            if (!File.Exists(fileName) || _partTextEditor == null)
                return;

            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new XmlTextReader(fs))
                {
                    _partTextEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
        }

        private void ParseXaml(string xaml)
        {
            if (SyntaxHighlighting == null)
                return;

            if (JustShowCode || !SyntaxHighlighting.Name.ToLower().Contains("xml"))
            {
                SetValue(ContentPropertyKey, null);
                return;
            }

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
