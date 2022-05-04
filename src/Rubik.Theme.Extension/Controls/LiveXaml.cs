using System;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

using Rubik.Toolkit.Utils;
using Rubik.Toolkit.UI;

namespace Rubik.Theme.Extension.Controls
{
    [TemplatePart(Name = TextEditorTemplateName, Type = typeof(TextEditor))]
    public class LiveXaml : Control
    {
        private static readonly Type _typeofSelf = typeof(LiveXaml);

        private const string TextEditorTemplateName = "PART_TextEditor";
        private TextEditor _partTextEditor = null;

        private FoldingManager _foldingManager = null;
        private XmlFoldingStrategy _foldingStrategy = null;

        private DispatcherTimer _timer = null;
        private bool _disabledTimer = false;

        private static RoutedCommand _openXamlCommand = null;
        private static RoutedCommand _saveXamlCommand = null;
        private static RoutedCommand _resetXamlCommand = null;
        private static RoutedCommand _copyXamlCommand = null;
        private static RoutedCommand _parseXamlCommand = null;

        private static RoutedCommand _horSplitCommand = null;
        private static RoutedCommand _verSplitCommand = null;

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
                {
                    _disabledTimer = true;
                    _partTextEditor.Text = value;
                }
            }
        }

        static LiveXaml()
        {
            InitializeCommands();

            DefaultStyleKeyProperty.OverrideMetadata(_typeofSelf, new FrameworkPropertyMetadata(_typeofSelf));
        }

        public LiveXaml()
        {
            _foldingStrategy = new XmlFoldingStrategy() { ShowAttributesWhenFolded = true };

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(Math.Max(1, Delay)) };
            _timer.Tick += _timer_Tick;

        }

        #region Command

        private static void InitializeCommands()
        {
            _openXamlCommand = new RoutedCommand("OpenXaml", _typeofSelf);
            _saveXamlCommand = new RoutedCommand("SaveXaml", _typeofSelf);
            _resetXamlCommand = new RoutedCommand("ResetXaml", _typeofSelf);
            _copyXamlCommand = new RoutedCommand("CopyXaml", _typeofSelf);
            _parseXamlCommand = new RoutedCommand("ParseXaml", _typeofSelf);

            _horSplitCommand = new RoutedCommand("HorSplit", _typeofSelf);
            _verSplitCommand = new RoutedCommand("VerSplit", _typeofSelf);

            CommandManager.RegisterClassCommandBinding(_typeofSelf, new CommandBinding(_openXamlCommand, OnOpenXamlCommand));
            CommandManager.RegisterClassCommandBinding(_typeofSelf, new CommandBinding(_saveXamlCommand, OnSaveXamlCommand));
            CommandManager.RegisterClassCommandBinding(_typeofSelf, new CommandBinding(_resetXamlCommand, OnResetXamlCommand));
            CommandManager.RegisterClassCommandBinding(_typeofSelf, new CommandBinding(_copyXamlCommand, OnCopyXamlCommand));
            CommandManager.RegisterClassCommandBinding(_typeofSelf, new CommandBinding(_parseXamlCommand, OnParseXamlCommand));
            CommandManager.RegisterClassCommandBinding(_typeofSelf, new CommandBinding(_horSplitCommand, OnHorSplitCommand));
            CommandManager.RegisterClassCommandBinding(_typeofSelf, new CommandBinding(_verSplitCommand, OnVerSplitCommand));
        }

        public static RoutedCommand OpenXamlCommand
        {
            get { return _openXamlCommand; }
        }

        public static RoutedCommand SaveXamlCommand
        {
            get { return _saveXamlCommand; }
        }

        public static RoutedCommand ResetXamlCommand
        {
            get { return _resetXamlCommand; }
        }

        public static RoutedCommand CopyXamlCommand
        {
            get { return _copyXamlCommand; }
        }

        public static RoutedCommand ParseXamlCommand
        {
            get { return _parseXamlCommand; }
        }

        public static RoutedCommand HorSplitCommand
        {
            get { return _horSplitCommand; }
        }

        public static RoutedCommand VerSplitCommand
        {
            get { return _verSplitCommand; }
        }

        private async static void OnOpenXamlCommand(object sender, RoutedEventArgs e)
        {
            var fileName = DialogUtil.ShowOpenFileDialog("XAML|*.xaml");
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
                return;

            var ctrl = sender as LiveXaml;

            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    ctrl.Text = await sr.ReadToEndAsync();
                }
            }
        }

        private async static void OnSaveXamlCommand(object sender, RoutedEventArgs e)
        {
            var fileName = DialogUtil.ShowSaveFileDialog("MyXaml.xaml", "XAML|*.xaml");
            if (string.IsNullOrEmpty(fileName))
                return;

            var ctrl = sender as LiveXaml;

            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    await sw.WriteAsync(ctrl.Text);
                }
            }
        }

        private static void OnResetXamlCommand(object sender, RoutedEventArgs e)
        {
            var ctrl = sender as LiveXaml;

            if (ctrl.ResetXaml != null)
                ctrl.Text = ctrl.ResetXaml();
        }

        private static void OnCopyXamlCommand(object sender, RoutedEventArgs e)
        {
            var ctrl = sender as LiveXaml;
            Clipboard.SetText(ctrl.Text);
        }

        private static void OnParseXamlCommand(object sender, RoutedEventArgs e)
        {
            var ctrl = sender as LiveXaml;
            ctrl.ParseXaml(ctrl.Text);
        }

        private static void OnHorSplitCommand(object sender, RoutedEventArgs e)
        {
            var ctrl = sender as LiveXaml;
            ctrl.HorSplit();
        }

        private static void OnVerSplitCommand(object sender, RoutedEventArgs e)
        {
            var ctrl = sender as LiveXaml;
            ctrl.VerSplit();
        }

        #endregion

        #region Property

        private static readonly DependencyPropertyKey ContainerAnglePropertyKey = DependencyProperty.RegisterReadOnly("ContainerAngle", typeof(double), _typeofSelf, new PropertyMetadata(0d));
        public static readonly DependencyProperty ContainerAngleProperty = ContainerAnglePropertyKey.DependencyProperty;
        public double ContainerAngle
        {
            get { return (double)GetValue(ContainerAngleProperty); }
        }

        private static readonly DependencyPropertyKey ControlPanelAnglePropertyKey = DependencyProperty.RegisterReadOnly("ControlPanelAngle", typeof(double), _typeofSelf, new PropertyMetadata(0d));
        public static readonly DependencyProperty ControlPanelAngleProperty = ControlPanelAnglePropertyKey.DependencyProperty;
        public double ControlPanelAngle
        {
            get { return (double)GetValue(ControlPanelAngleProperty); }
        }

        private static readonly DependencyPropertyKey SplitterCursorSourcePropertyKey = DependencyProperty.RegisterReadOnly("SplitterCursorSource", typeof(string), _typeofSelf, new PropertyMetadata(ResourceKeys.UpDownSplitterCur));
        public static readonly DependencyProperty SplitterCursorSourceProperty = SplitterCursorSourcePropertyKey.DependencyProperty;
        public string SplitterCursorSource
        {
            get { return (string)GetValue(SplitterCursorSourceProperty); }
        }

        private static readonly DependencyPropertyKey PanelAnglePropertyKey = DependencyProperty.RegisterReadOnly("PanelAngle", typeof(double), _typeofSelf, new PropertyMetadata(0d));
        public static readonly DependencyProperty PanelAngleProperty = PanelAnglePropertyKey.DependencyProperty;
        public double PanelAngle
        {
            get { return (double)GetValue(PanelAngleProperty); }
        }

        private static readonly DependencyPropertyKey HorSplitAnglePropertyKey = DependencyProperty.RegisterReadOnly("HorSplitAngle", typeof(double), _typeofSelf, new PropertyMetadata(0d));
        public static readonly DependencyProperty HorSplitAngleProperty = HorSplitAnglePropertyKey.DependencyProperty;
        public double HorSplitAngle
        {
            get { return (double)GetValue(HorSplitAngleProperty); }
        }

        private static readonly DependencyPropertyKey VerSplitAnglePropertyKey = DependencyProperty.RegisterReadOnly("VerSplitAngle", typeof(double), _typeofSelf, new PropertyMetadata(0d));
        public static readonly DependencyProperty VerSplitAngleProperty = VerSplitAnglePropertyKey.DependencyProperty;
        public double VerSplitAngle
        {
            get { return (double)GetValue(VerSplitAngleProperty); }
        }

        private static readonly DependencyPropertyKey ContentPropertyKey = DependencyProperty.RegisterReadOnly("Content", typeof(object), _typeofSelf, new PropertyMetadata(null));
        public static readonly DependencyProperty ContentProperty = ContentPropertyKey.DependencyProperty;
        public object Content
        {
            get { return GetValue(ContentProperty); }
        }

        public static readonly DependencyProperty LineNumbersForegroundProperty = DependencyProperty.Register("LineNumbersForeground", typeof(Brush), _typeofSelf, new PropertyMetadata(Brushes.Black));
        public Brush LineNumbersForeground
        {
            get { return (Brush)GetValue(LineNumbersForegroundProperty); }
            set { SetValue(LineNumbersForegroundProperty, value); }
        }

        public static readonly DependencyProperty LineNumbersBackgroundProperty = DependencyProperty.Register("LineNumbersBackground", typeof(Brush), _typeofSelf, new PropertyMetadata(Brushes.Transparent));
        public Brush LineNumbersBackground
        {
            get { return (Brush)GetValue(LineNumbersBackgroundProperty); }
            set { SetValue(LineNumbersBackgroundProperty, value); }
        }

        public static readonly DependencyProperty SyntaxHighlightingProperty = DependencyProperty.Register("SyntaxHighlighting", typeof(IHighlightingDefinition), _typeofSelf);
        [TypeConverter(typeof(HighlightingDefinitionTypeConverter))]
        public IHighlightingDefinition SyntaxHighlighting
        {
            get { return (IHighlightingDefinition)GetValue(SyntaxHighlightingProperty); }
            set { SetValue(SyntaxHighlightingProperty, value); }
        }

        public static readonly DependencyProperty AllowFoldingProperty = DependencyProperty.Register("AllowFolding", typeof(bool), _typeofSelf, new PropertyMetadata(true, OnAllowFoldingPropertyChanged));
        public bool AllowFolding
        {
            get { return (bool)GetValue(AllowFoldingProperty); }
            set { SetValue(AllowFoldingProperty, value); }
        }

        private static void OnAllowFoldingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as LiveXaml;
            var allowFolding = (bool)e.NewValue;

            if (allowFolding)
                ctrl.InstallFolding();
            else
                ctrl.UninstallFolding();
        }

        public static readonly DependencyProperty AutoParseProperty = DependencyProperty.Register("AutoParse", typeof(bool), _typeofSelf, new PropertyMetadata(false));
        public bool AutoParse
        {
            get { return (bool)GetValue(AutoParseProperty); }
            set { SetValue(AutoParseProperty, value); }
        }

        public static readonly DependencyProperty LinePositionProperty = DependencyProperty.Register("LinePosition", typeof(int), _typeofSelf, new PropertyMetadata(OnLinePositionPropertyChanged));
        public int LinePosition
        {
            get { return (int)GetValue(LinePositionProperty); }
            set { SetValue(LinePositionProperty, value); }
        }

        private static void OnLinePositionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as LiveXaml;
            var pos = (int)e.NewValue;

            ctrl._partTextEditor.TextArea.Caret.Column = pos;
        }

        public static readonly DependencyProperty LineNumberProperty = DependencyProperty.Register("LineNumber", typeof(int), _typeofSelf, new PropertyMetadata(OnLineNumberPropertyChanged));
        public int LineNumber
        {
            get { return (int)GetValue(LineNumberProperty); }
            set { SetValue(LineNumberProperty, value); }
        }

        private static void OnLineNumberPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as LiveXaml;
            var line = (int)e.NewValue;

            ctrl._partTextEditor.TextArea.Caret.Line = line;
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

        public static readonly DependencyProperty IsReadOnlyProperty = TextEditor.IsReadOnlyProperty.AddOwner(_typeofSelf);
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register("Delay", typeof(double), _typeofSelf, new PropertyMetadata(1d, OnDelayPropertyChanged));
        public double Delay
        {
            get { return (double)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }

        private static void OnDelayPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as LiveXaml;
            var delay = (double)e.NewValue;

            if (ctrl._timer != null)
                ctrl._timer.Interval = TimeSpan.FromSeconds(Math.Max(1, delay));
        }

        public static readonly DependencyProperty RestXamlProperty = DependencyProperty.Register("ResetXaml", typeof(Func<string>), _typeofSelf);
        public Func<string> ResetXaml
        {
            get { return (Func<string>)GetValue(RestXamlProperty); }
            set { SetValue(RestXamlProperty, value); }
        }

        #endregion

        #region Override

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

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _partTextEditor = GetTemplateChild(TextEditorTemplateName) as TextEditor;

            if (_partTextEditor != null)
            {
                _partTextEditor.TextChanged += _partTextEditor_TextChanged;
                _partTextEditor.TextArea.TextEntered += TextArea_TextEntered;
                _partTextEditor.TextArea.Caret.PositionChanged += Caret_PositionChanged;
                DataObject.AddPastingHandler(_partTextEditor, _partTextEditor_Pasting);

                _partTextEditor.Options = new TextEditorOptions { ConvertTabsToSpaces = true };
                _partTextEditor.TextArea.SelectionCornerRadius = 0;
                _partTextEditor.TextArea.SelectionBrush = ColorUtil.GetBrushFromString("#FFADD6FF");
                _partTextEditor.TextArea.SelectionBorder = null;
                _partTextEditor.TextArea.SelectionForeground = null;
            }
        }

        #endregion

        #region Event

        private void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();

            if (AutoParse)
                ParseXaml(Text);
        }

        private void _partTextEditor_TextChanged(object sender, EventArgs e)
        {
            RefreshFoldings();
            //ParseXaml(Text);

            if (_disabledTimer)
            {
                _disabledTimer = false;
                return;
            }

            if (_timer != null)
            {
                _timer.Stop();
                _timer.Start();
            }
        }

        private void _partTextEditor_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            //for invoke DelayArrivedEvent
            _disabledTimer = false;
        }

        private void Caret_PositionChanged(object sender, EventArgs e)
        {
            if (_partTextEditor == null)
                return;

            LineNumber = _partTextEditor.TextArea.Caret.Location.Line;
            LinePosition = _partTextEditor.TextArea.Caret.Location.Column;
        }

        private void TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (IsReadOnly)
                return;

            var offset = _partTextEditor.TextArea.Caret.Offset;

            switch (e.Text)
            {
                case "{":
                    {
                        InsertText("}");
                        break;
                    }

                case "=": //get values
                    {
                        if (!IsEvenQuoteInElement(offset - 2))
                            break;

                        InsertText("\"\"");
                        break;
                    }

                case "\"": //get values
                    {
                        InsertText("\"");
                        break;
                    }

                case ">":  // auto add </XXX>
                    {
                        if (FindPreviousNonSpaceChars(offset - 1, 2) == "/>")
                            break;

                        var element = GetElement(offset - 2);
                        if (!string.IsNullOrEmpty(element))
                        {
                            var insertStr = $"</{element}>";
                            InsertText(insertStr, true, insertStr.Length);
                        }

                        break;
                    }

                case "/":
                    {
                        if (FindPreviousNonSpaceChars(offset - 1, 2) == "</")
                        {
                            var parentOffset = -1;
                            var parentElement = GetParentElement(offset - 3, ref parentOffset);
                            if (!string.IsNullOrEmpty(parentElement))
                                InsertText($"{parentElement}>");
                        }

                        break;
                    }

                case "\n":  // auto align or insert one space line
                    DealBreak();
                    break;
            }
        }

        #endregion

        #region IntelliSense

        private string FindPreviousNonSpaceChars(int startOffset, int charLength = 1, int minOffset = 0)
        {
            var foundChars = string.Empty;

            while (startOffset >= minOffset)
            {
                var curChar = _partTextEditor.Text[startOffset];

                if (!char.IsWhiteSpace(curChar))
                {
                    foundChars += curChar;
                    if (foundChars.Length == charLength)
                        break;
                }

                startOffset--;
            }

            if (string.IsNullOrEmpty(foundChars) || foundChars.Length == 1)
                return foundChars;

            return new string(foundChars.Reverse().ToArray());
        }

        private string FindNextNonSpaceChars(int startOffset, int charLength = 1, int maxOffset = -1)
        {
            var foundChars = string.Empty;
            var length = maxOffset < 0 ? _partTextEditor.Text.Length : (maxOffset + 1);

            while (startOffset < length)
            {
                var curChar = _partTextEditor.Text[startOffset];

                if (!char.IsWhiteSpace(curChar))
                {
                    foundChars += curChar;
                    if (foundChars.Length == charLength)
                        break;
                }

                startOffset++;
            }

            return foundChars;
        }

        private string GetParentElement(int startOffset, ref int parentOffset)
        {
            var foundEnd = false;
            var ignoreNextEndCount = 0;

            for (int i = startOffset; i >= 0; i--)
            {
                var curChar = _partTextEditor.Text[i];

                if (curChar == '>' && i > 0 && FindPreviousNonSpaceChars(i - 1) != "/")
                {
                    foundEnd = true;
                    continue;
                }

                if (curChar == '/' && i > 0 && FindPreviousNonSpaceChars(i - 1) == "<")
                {
                    foundEnd = false;
                    ignoreNextEndCount++;

                    continue;
                }

                if (curChar == '<' && foundEnd)
                {
                    if (ignoreNextEndCount > 0)
                    {
                        foundEnd = false;
                        ignoreNextEndCount--;

                        continue;
                    }

                    parentOffset = i;
                    var element = "";
                    for (int j = i + 1; j <= startOffset; j++)
                    {
                        curChar = _partTextEditor.Text[j];
                        var isLetterOrPoint = char.IsLetter(curChar) || curChar == '.';

                        if (!string.IsNullOrEmpty(element) && !isLetterOrPoint)
                            return element;

                        if (isLetterOrPoint)
                            element += curChar;
                    }

                    if (!string.IsNullOrEmpty(element))
                        return element;
                }
            }

            return null;
        }

        private string GetElementInFrontOfSymbol(int startOffset)
        {
            var element = "";

            for (int i = startOffset; i >= 0; i--)
            {
                var curChar = _partTextEditor.Text[i];
                var isLetter = char.IsLetter(curChar);

                if (!string.IsNullOrEmpty(element) && !isLetter)
                    return new string(element.Reverse().ToArray());

                if (isLetter)
                    element += curChar;
            }

            return element;
        }

        private Tuple<string, string> GetElementAndAttributeInFrontOfSymbol(int startOffset)
        {
            var finishAttribute = false;
            var attribute = "";

            var element = "";

            for (int i = startOffset; i >= 0; i--)
            {
                var curChar = _partTextEditor.Text[i];

                //attribute
                var isLetter = char.IsLetter(curChar);
                if (!string.IsNullOrEmpty(attribute) && !isLetter && !finishAttribute)
                {
                    finishAttribute = true;
                    attribute = new string(attribute.Reverse().ToArray());
                }

                if (isLetter && !finishAttribute)
                    attribute += curChar;

                //element
                if (curChar == '>')
                    break;

                if (curChar == '/' && i > 0 && FindPreviousNonSpaceChars(i - 1) == "<")
                    break;

                if (curChar == '<')
                {
                    for (int j = i + 1; j <= startOffset; j++)
                    {
                        curChar = _partTextEditor.Text[j];
                        var isLetterOrPoint = char.IsLetter(curChar) || curChar == '.';

                        if (!string.IsNullOrEmpty(element) && !isLetterOrPoint)
                            break;

                        if (isLetterOrPoint)
                            element += curChar;
                    }
                }
            }

            return new Tuple<string, string>(element, attribute);
        }

        private string GetElement(int startOffset)
        {
            for (int i = startOffset; i >= 0; i--)
            {
                var curChar = _partTextEditor.Text[i];
                if (curChar == '>')
                    return null;

                if (curChar == '/' && i > 0 && FindPreviousNonSpaceChars(i - 1) == "<")
                    return null;

                if (curChar == '<')
                {
                    var element = "";
                    for (int j = i + 1; j <= startOffset; j++)
                    {
                        curChar = _partTextEditor.Text[j];
                        var isLetterOrPoint = char.IsLetter(curChar) || curChar == '.';

                        if (!string.IsNullOrEmpty(element) && !isLetterOrPoint)
                            return element;

                        if (isLetterOrPoint)
                            element += curChar;
                    }

                    if (!string.IsNullOrEmpty(element))
                        return element;
                }
            }

            return null;
        }

        private void FormatIndentInElement(string lastLineText)
        {
            if (string.IsNullOrWhiteSpace(lastLineText))
                return;

            var curOffset = _partTextEditor.TextArea.Caret.Offset;

            for (int i = 0; i < lastLineText.Length; i++)
            {
                var curChar = lastLineText[i];
                if (char.IsWhiteSpace(curChar))
                    continue;

                if (curChar == '<')
                {
                    var startOffset = i;

                    var foundSpace = false;
                    for (int j = i + 1; j < lastLineText.Length; j++)
                    {
                        curChar = lastLineText[j];
                        var isWhiteSpace = char.IsWhiteSpace(curChar);

                        if (!isWhiteSpace)
                        {
                            if (!foundSpace)
                                continue;
                            else
                            {
                                startOffset = j;
                                break;
                            }
                        }
                        else
                        {
                            foundSpace = true;
                        }
                    }

                    if (startOffset == i)
                        _partTextEditor.Document.Insert(curOffset, string.Join("", Enumerable.Repeat(" ", 4)));
                    else
                        _partTextEditor.Document.Insert(curOffset, string.Join("", Enumerable.Repeat(" ", startOffset - i)));
                }

                break;
            }
        }

        private void DealBreak()
        {
            var docLine = _partTextEditor.Document.GetLineByNumber(_partTextEditor.TextArea.Caret.Line - 1);
            if (docLine == null)
                return;

            var lineText = _partTextEditor.Document.GetText(docLine.Offset, docLine.Length);
            if (string.IsNullOrWhiteSpace(lineText))
                return;

            var curOffset = _partTextEditor.TextArea.Caret.Offset;
            if (curOffset == 0)
                return;

            var element = GetElement(curOffset - 1);
            if (!string.IsNullOrEmpty(element))
            {
                FormatIndentInElement(lineText);
                return;
            }

            var parentOffset = -1;
            var parentElement = GetParentElement(curOffset - 1, ref parentOffset);
            if (!string.IsNullOrEmpty(parentElement))
            {
                var parentColumn = _partTextEditor.Document.GetLocation(parentOffset).Column;
                var curColumn = _partTextEditor.TextArea.Caret.Column;

                var targetColumn = parentColumn + 4;

                if (targetColumn > curColumn)
                    _partTextEditor.Document.Insert(curOffset, string.Join("", Enumerable.Repeat(" ", targetColumn - curColumn)));
                else if (targetColumn < curColumn)
                    _partTextEditor.TextArea.Caret.Offset -= (curColumn - targetColumn);

                curOffset = _partTextEditor.TextArea.Caret.Offset;
                var thisLine = _partTextEditor.Document.GetLineByOffset(curOffset);

                if (FindNextNonSpaceChars(curOffset, 2, thisLine.EndOffset) == "</")
                {
                    _partTextEditor.Document.Insert(curOffset, "\n" + (parentColumn > 0 ? string.Join("", Enumerable.Repeat(" ", parentColumn - 1)) : ""));
                    _partTextEditor.TextArea.Caret.Offset = curOffset;
                }
            }
        }

        private bool IsEvenQuoteInElement(int startOffset)
        {
            var quoteCount = 0;

            for (int i = startOffset; i >= 0; i--)
            {
                var curChar = _partTextEditor.Text[i];
                if (curChar == '>')
                    return false;

                if (curChar == '/' && i > 0 && FindPreviousNonSpaceChars(i - 1) == "<")
                    return false;

                if (curChar == '\"')
                    quoteCount++;

                if (curChar == '<')
                    return quoteCount % 2 == 0;
            }

            return false;
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

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Tick -= _timer_Tick;
                _timer = null;
            }

            UninstallFolding();
            UnsubscribeEvents();
        }

        private void InsertText(string text, bool caretFallBack = true, int fallbackLength = 1)
        {
            _partTextEditor.TextArea.Document.Insert(_partTextEditor.TextArea.Caret.Offset, text);

            if (caretFallBack)
                _partTextEditor.TextArea.Caret.Column -= fallbackLength;
        }

        private void UnsubscribeEvents()
        {
            if (_partTextEditor != null)
            {
                _partTextEditor.TextChanged -= _partTextEditor_TextChanged;
                _partTextEditor.TextArea.TextEntered -= TextArea_TextEntered;
                _partTextEditor.TextArea.Caret.PositionChanged -= Caret_PositionChanged;
                DataObject.RemovePastingHandler(_partTextEditor, _partTextEditor_Pasting);
            }
        }

        private void RefreshFoldings()
        {
            if (_partTextEditor == null || !AllowFolding)
                return;

            InstallFolding();

            _foldingStrategy.UpdateFoldings(_foldingManager, _partTextEditor.Document);
        }

        private void InstallFolding()
        {
            if (_foldingManager != null || _partTextEditor == null)
                return;

            _foldingManager = FoldingManager.Install(_partTextEditor.TextArea);
        }

        private void UninstallFolding()
        {
            if (_foldingManager != null)
            {
                FoldingManager.Uninstall(_foldingManager);
                _foldingManager = null;
            }
        }

        private void ParseXaml(string xaml)
        {
            if (string.IsNullOrWhiteSpace(xaml))
            {
                ShowLocalText("Error: No XAML!");
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

        private void HorSplit()
        {
            SetValue(ContainerAnglePropertyKey, 0d);
            SetValue(ControlPanelAnglePropertyKey, 0d);
            SetValue(PanelAnglePropertyKey, 0d);
            SetValue(HorSplitAnglePropertyKey, 0d);
            SetValue(VerSplitAnglePropertyKey, 0d);
            SetValue(SplitterCursorSourcePropertyKey, ResourceKeys.UpDownSplitterCur);
            
        }

        private void VerSplit()
        {
            SetValue(ContainerAnglePropertyKey, -90d);
            SetValue(ControlPanelAnglePropertyKey, 180d);
            SetValue(PanelAnglePropertyKey, 90d);
            SetValue(HorSplitAnglePropertyKey, 90d);
            SetValue(VerSplitAnglePropertyKey, 90d);
            SetValue(SplitterCursorSourcePropertyKey, ResourceKeys.LeftRightSplitterCur);
        }

        #endregion
    }
}
