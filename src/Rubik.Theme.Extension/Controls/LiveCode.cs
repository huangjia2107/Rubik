using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace Rubik.Theme.Extension.Controls
{
    [TemplatePart(Name = TextEditorTemplateName, Type = typeof(TextEditor))]
    public class LiveCode : ContentControl
    {
        private static readonly Type _typeofSelf = typeof(LiveCode);

        private const string TextEditorTemplateName = "PART_TextEditor";

        private TextEditor _partTextEditor = null;

        static LiveCode()
        {
            DefaultStyleKeyProperty.OverrideMetadata(_typeofSelf, new FrameworkPropertyMetadata(_typeofSelf));
        }

        #region Property

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), _typeofSelf, new PropertyMetadata(Orientation.Horizontal));
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty CodeBackgroundProperty = DependencyProperty.Register("CodeBackground", typeof(Brush), _typeofSelf, new PropertyMetadata(Brushes.Transparent));
        public Brush CodeBackground
        {
            get { return (Brush)GetValue(CodeBackgroundProperty); }
            set { SetValue(CodeBackgroundProperty, value); }
        }

        public static readonly DependencyProperty SyntaxHighlightingProperty = DependencyProperty.Register("SyntaxHighlighting", typeof(IHighlightingDefinition), _typeofSelf);
        [TypeConverter(typeof(HighlightingDefinitionTypeConverter))]
        public IHighlightingDefinition SyntaxHighlighting
        {
            get { return (IHighlightingDefinition)GetValue(SyntaxHighlightingProperty); }
            set { SetValue(SyntaxHighlightingProperty, value); }
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
            }
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            if (_partTextEditor != null)
                _partTextEditor.Text = GetXamlString(newContent);
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

        private string GetXamlString(object content)
        {
            return XamlWriter.Save(content);
        }

        #endregion
    }
}
