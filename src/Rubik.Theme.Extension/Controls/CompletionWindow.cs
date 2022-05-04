using System;
using System.Windows;
using ICSharpCode.AvalonEdit.Editing;

namespace Rubik.Theme.Extension.Controls
{
    public class CompletionWindow : ICSharpCode.AvalonEdit.CodeCompletion.CompletionWindow
    {
        public CompletionWindow(TextArea textArea)
            : base(textArea)
        {
        } 

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            if (SizeToContent != SizeToContent.Manual && WindowState == WindowState.Normal)
                InvalidateMeasure();
        }
    }
}
