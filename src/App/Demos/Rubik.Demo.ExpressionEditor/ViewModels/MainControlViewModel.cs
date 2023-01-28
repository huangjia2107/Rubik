using System;
using System.Collections.Generic;
using System.Windows;

using Prism.Commands;
using Prism.Mvvm;

using Rubik.Demo.ExpressionEditor.Models;
using Rubik.Demo.ExpressionEditor.Views;
using Rubik.Theme.Extension.Datas;

namespace Rubik.Demo.ExpressionEditor.ViewModels
{
    public class MainControlViewModel : BindableBase
    {
        public DelegateCommand<RoutedEventArgs> LoadedCommand { get; private set; }
        public DelegateCommand InsertCommand { get; private set; }

        private Theme.Extension.Controls.ExpressionEditor _expressionEditor = null;

        public MainControlViewModel()
        {
            LoadedCommand = new DelegateCommand<RoutedEventArgs>(Loaded);
            InsertCommand = new DelegateCommand(Insert);
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        private Func<string, IEnumerable<CompletionData>> _completionDataFunc = null;
        public Func<string, IEnumerable<CompletionData>> CompletionDataFunc
        {
            get
            {
                if (_completionDataFunc == null)
                    _completionDataFunc = key =>
                    {
                        return new List<CompletionDataModel>
                        {
                            new CompletionDataModel{ Text = "OR", Content = "OR"},
                            new CompletionDataModel{ Text = "AND", Content = "AND"},
                            new CompletionDataModel{ Text = "TM043", Content = "TM043-HelloWord"},
                            new CompletionDataModel{ Text = "TMZ274", Content = "TMZ274-HiHiHi"},
                            new CompletionDataModel{ Text = "TMZ033", Content = "TMZ033-OKOKOKOKOK"}
                        };
                    };

                return _completionDataFunc;
            }
        }

        private void Loaded(RoutedEventArgs e)
        {
            _expressionEditor = (e.OriginalSource as MainControl).Editor;
        }

        private void Insert()
        {
            _expressionEditor.InsertText(Text);
        }
    }
}
