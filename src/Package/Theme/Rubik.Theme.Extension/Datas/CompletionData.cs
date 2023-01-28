using System;
using System.Windows.Media;

using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace Rubik.Theme.Extension.Datas
{
    public abstract class CompletionData : ICompletionData
    {
        public abstract string Text { get; set; }
        public abstract object Content { get; set; }
        public abstract object Description { get; set; }

        public virtual ImageSource Image { get; set; }

        public virtual double Priority { get; set; } = 1;

        public virtual void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, this.Text);
        }
    }
}
