using System;
using System.Collections.Generic;

using Prism.Mvvm;

namespace Rubik.Demo.ExpressionEditor.ViewModels
{
    public class MainControlViewModel : BindableBase
    {
        private Func<string, IEnumerable<string>> _completionDataFunc = null;
        public Func<string, IEnumerable<string>> CompletionDataFunc
        {
            get
            {
                if (_completionDataFunc == null)
                    _completionDataFunc = key =>
                    {
                        return new List<string> { "OR", "AND", "command_para", "CmdParam", "DeviceType", "mode", "TMN043", "TMZ274", "TMZ033" };
                    };

                return _completionDataFunc;
            }
        }
    }
}
