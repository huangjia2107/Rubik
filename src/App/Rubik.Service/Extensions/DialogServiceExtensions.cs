using System;

using Prism.Services.Dialogs;

using Rubik.Service.Dialogs;
using Rubik.Service.Models;

namespace Rubik.Service.Extensions
{
    public static class DialogServiceExtensions
    {
        public static void ShowMessage(this IDialogService dialogService, string message, MessageButton button = MessageButton.OK, MessageType type = MessageType.Information, Action<IDialogResult> callBack = null)
        {
            var parameters = new DialogParameters();
            parameters.Add("Message", message);
            parameters.Add("Button", button);
            parameters.Add("Type", type);

            dialogService.ShowDialog(nameof(MessageDialog), parameters, callBack);
        }
    }
}
