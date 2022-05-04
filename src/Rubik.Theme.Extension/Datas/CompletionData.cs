using System;
using System.Windows.Media;

using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace Rubik.Theme.Extension.Datas
{
    public class CompletionData : ICompletionData
    {
        public string Text { get; private set; }

        public CompletionData(string text)
        {
            Text = text;
        }

        public object Content => Text;

        public object Description { get; }
        public ImageSource Image { get; }

        public double Priority => 1;

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, this.Text);
        }
    }
}
