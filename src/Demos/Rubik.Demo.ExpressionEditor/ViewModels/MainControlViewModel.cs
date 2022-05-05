using System;
using System.Collections.Generic;

using Prism.Mvvm;

using Rubik.Demo.ExpressionEditor.Models;
using Rubik.Theme.Extension.Datas;

namespace Rubik.Demo.ExpressionEditor.ViewModels
{
    public class MainControlViewModel : BindableBase
    {
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
    }
}
