using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

using Rubik.Theme.Extension.Datas;
using Rubik.Toolkit.Utils;

namespace Rubik.Theme.Extension.Controls
{
    public class ExpressionEditor : Control
    {
        private static readonly Type _typeofSelf = typeof(ExpressionEditor);

        private const string TextEditorTemplateName = "PART_TextEditor";
        private TextEditor _partTextEditor = null;

        private CompletionWindow _completionWindow = null;

        public string Text
        {
            get
            {
                if (_partTextEditor != null)
                    return _partTextEditor.Text;

                return string.Empty;
            }
            set
            {
                if (_partTextEditor != null)
                    _partTextEditor.Text = value;
            }
        }

        public bool CanRedo
        {
            get { return _partTextEditor != null && _partTextEditor.CanRedo; }
        }

        public bool CanUndo
        {
            get { return _partTextEditor != null && _partTextEditor.CanUndo; }
        }

        static ExpressionEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(_typeofSelf, new FrameworkPropertyMetadata(_typeofSelf));
        }

        #region RouteEvent

        public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent("TextChanged", RoutingStrategy.Bubble, typeof(RoutedEventArgs), _typeofSelf);
        public event RoutedEventHandler TextChanged
        {
            add { AddHandler(TextChangedEvent, value); }
            remove { RemoveHandler(TextChangedEvent, value); }
        }

        #endregion

        #region Property

        public static readonly DependencyProperty IsReadOnlyProperty = TextEditor.IsReadOnlyProperty.AddOwner(_typeofSelf);
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty WordWrapProperty = TextEditor.WordWrapProperty.AddOwner(_typeofSelf);
        public bool WordWrap
        {
            get { return (bool)GetValue(WordWrapProperty); }
            set { SetValue(WordWrapProperty, value); }
        }

        public static readonly DependencyProperty CompletionDataFuncProperty = DependencyProperty.Register("CompletionDataFunc", typeof(Func<string, IEnumerable<string>>), _typeofSelf);
        public Func<string, IEnumerable<string>> CompletionDataFunc
        {
            get { return (Func<string, IEnumerable<string>>)GetValue(CompletionDataFuncProperty); }
            set { SetValue(CompletionDataFuncProperty, value); }
        }

        #endregion

        #region Override

        public override void OnApplyTemplate()
        {
            if (_partTextEditor != null)
                _partTextEditor.TextArea.TextEntered -= TextArea_TextEntered;

            base.OnApplyTemplate();

            _partTextEditor = GetTemplateChild(TextEditorTemplateName) as TextEditor;

            if (_partTextEditor != null)
            {
                _partTextEditor.TextArea.TextEntering += TextArea_TextEntering;
                _partTextEditor.TextArea.TextEntered += TextArea_TextEntered;

                _partTextEditor.Options = new TextEditorOptions { ConvertTabsToSpaces = true };
                _partTextEditor.TextArea.SelectionCornerRadius = 0;
                _partTextEditor.TextArea.SelectionBrush = ColorUtil.GetBrushFromString("#FFADD6FF");
                _partTextEditor.TextArea.SelectionBorder = null;
                _partTextEditor.TextArea.SelectionForeground = null;

                InitSyntaxHighlighting();
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            if (Focusable && _partTextEditor != null)
                _partTextEditor.Focus();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.Key)
            {
                case Key.Left:
                    {
                        if (_partTextEditor.SelectionLength < 2 || Keyboard.Modifiers != ModifierKeys.None)
                            return;

                        _partTextEditor.TextArea.Caret.Offset = _partTextEditor.SelectionStart + 1;
                        break;
                    }
                case Key.Right:
                    {
                        if (_partTextEditor.SelectionLength < 2 || Keyboard.Modifiers != ModifierKeys.None)
                            return;

                        _partTextEditor.TextArea.Caret.Offset = _partTextEditor.SelectionStart + _partTextEditor.SelectionLength - 1;
                        break;
                    }
            }
        }

        #endregion

        #region Event

        private void TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (IsReadOnly || _completionWindow == null || e.Text.Length == 0)
                return;

            if (!char.IsLetter(e.Text[0]))
            {
                // Whenever a non-letter is typed while the completion window is open,
                // insert the currently selected element.
                _completionWindow.CompletionList.RequestInsertion(e);
            }
        }

        private void TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (IsReadOnly)
                return;

            var offset = _partTextEditor.TextArea.Caret.Offset;

            switch (e.Text)
            {
                case "+":
                case "-":
                case "*":
                case "/":
                case "%":
                case "^":
                    {
                        InsertText(" ", false);
                        return;
                    }
                case "(":
                    {
                        InsertText(")");
                        return;
                    }
                case "!":
                    {
                        if (_partTextEditor.Text.Length > offset)
                        {
                            var afterChar = _partTextEditor.Text[offset];
                            if (afterChar == '=')
                                break;
                        }

                        InsertText("= ", false);
                        return;
                    }
                case "=":
                    {
                        if (_partTextEditor.Text.Length > 1)
                        {
                            var preChar = _partTextEditor.Text[offset - 2];
                            if (preChar == '>' || preChar == '<' || preChar == '=' || preChar == '!')
                            {
                                InsertText(" ", false);
                                break;
                            }
                        }

                        if (_partTextEditor.Text.Length > offset)
                        {
                            var afterChar = _partTextEditor.Text[offset];
                            if (afterChar == '=')
                                break;
                        }

                        InsertText("= ", false);
                        return;
                    }
            }

            if (char.IsLetter(e.Text[0]))
            {
                if (_partTextEditor.Text.Length > 1 && char.IsLetter(_partTextEditor.Text[offset - 2]))
                    return;

                ShowCompletionWindow(e.Text);
            }
        }

        private void CompletionList_InsertionRequested(object sender, EventArgs e)
        {
            if (CompletionDataFunc == null)
                return;

            var selectedItem = (sender as CompletionList).SelectedItem?.Text;
            if (string.IsNullOrEmpty(selectedItem))
                return;

            _partTextEditor.Document.Remove(_partTextEditor.TextArea.Caret.Offset - selectedItem.Length - 1, 1);
            InsertText(" ", false);
        }

        #endregion

        #region Func

        public void Redo()
        {
            _partTextEditor?.Redo();
        }

        public void Undo()
        {
            _partTextEditor?.Undo();
        }

        public void Dispose()
        {
            _completionWindow?.Close();

            UnsubscribeEvents();
        }

        private void UnsubscribeEvents()
        {
            if (_partTextEditor != null)
            {
                _partTextEditor.TextArea.TextEntering -= TextArea_TextEntering;
                _partTextEditor.TextArea.TextEntered -= TextArea_TextEntered;
            }
        }

        private void InitSyntaxHighlighting()
        {
            if (_partTextEditor == null)
                return;

            var streamInfo = Application.GetResourceStream(new Uri(@"/Rubik.Theme.Extension;component/Xshds/ExpressionEditor.xshd", UriKind.Relative));
            if (streamInfo != null)
            {
                using (var stream = streamInfo.Stream)
                {
                    using (var reader = new XmlTextReader(stream))
                    {
                        _partTextEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                    }
                }
            }
        }

        private void AfterCloseCompletionWindow(EventHandler handler = null)
        {
            if (_completionWindow != null)
            {
                _completionWindow.CompletionList.InsertionRequested -= CompletionList_InsertionRequested;

                _completionWindow.Closed -= handler;
                _completionWindow.Resources = null;
                _completionWindow = null;
            }
        }

        private void InsertText(string text, bool caretFallBack = true, int fallbackLength = 1)
        {
            _partTextEditor.TextArea.Document.Insert(_partTextEditor.TextArea.Caret.Offset, text);

            if (caretFallBack)
                _partTextEditor.TextArea.Caret.Column -= fallbackLength;
        }

        private void ShowCompletionWindow(string input)
        {
            if (_completionWindow != null || CompletionDataFunc == null)
                return;

            var showDatas = CompletionDataFunc(input);
            if (showDatas == null || !showDatas.Any())
                return;

            _completionWindow = new CompletionWindow(_partTextEditor.TextArea);
            _completionWindow.Resources = Resources;
            _completionWindow.MinWidth = 300;
            _completionWindow.MaxHeight = 300;
            _completionWindow.SizeToContent = SizeToContent.WidthAndHeight;

            _completionWindow.CompletionList.InsertionRequested += CompletionList_InsertionRequested;

            foreach (var d in showDatas)
                _completionWindow.CompletionList.CompletionData.Add(new CompletionData(d));

            _completionWindow.CompletionList.SelectItem(input);

            EventHandler handler = null;
            handler = (s, e) =>
            {
                AfterCloseCompletionWindow(handler);
            };

            _completionWindow.Closed -= handler;
            _completionWindow.Closed += handler;
            _completionWindow.Show();
        }

        #endregion
    }
}
