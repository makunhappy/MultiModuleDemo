using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public static class IDialogServiceExtension
    {
        public static bool ShowDialogX(this IDialogService service,
            string title,
            string message)
        {
            ButtonResult result = ButtonResult.Cancel;
            using (AutoResetEvent autoResetEvent = new(false))
            {
                service.ShowDialog(DialogNames.DialogView,
                    new DialogParameters().AddTitle(title).AddMessage(message),
                    r =>
                    {
                        autoResetEvent.Set();
                        result = r.Result;
                    });
                autoResetEvent.WaitOne();
            }
            return result == ButtonResult.OK;
        }
        public static void ShowX(this IDialogService service,
            string title,
            string message)
        {
            service.Show(DialogNames.DialogView,
                    new DialogParameters().AddTitle(title).AddMessage(message),
                    null);
        }
    }
    public static class IDialogParametersExtension
    {
        public static IDialogParameters AddTitle(this IDialogParameters parameters, string title)
        {
            parameters.Add(DialogNames.Title, title);
            return parameters;
        }
        public static IDialogParameters AddMessage(this IDialogParameters parameters, string message)
        {
            parameters.Add(DialogNames.Content, message);
            return parameters;
        }
    }
}
