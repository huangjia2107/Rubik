﻿
using Prism.Services.Dialogs;

namespace Rubik.Service.Dialogs
{
    public class TDialogResult<T> : DialogResult
    {
        public TDialogResult(ButtonResult result) : base(result)
        {

        }

        public T Data { get; set; }
    }
}
