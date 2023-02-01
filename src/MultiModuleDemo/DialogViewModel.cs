using Common;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MultiModuleDemo
{
    public class DialogViewModel : BindableBase, IDialogAware
    {
        private string title;
        public string Title => title;
        private string content;

        public string Content
        {
            get { return content; }
            set { SetProperty(ref content, value); }
        }


        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            title = parameters.GetValue<string>(DialogNames.Title);
            Content = parameters.GetValue<string>(DialogNames.Content);
        }

        private DelegateCommand<string> _closeDialogCommand;
        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));
        private void CloseDialog(string para)
        {
            ButtonResult result = ButtonResult.None;
            if (string.Equals(para, "true", StringComparison.OrdinalIgnoreCase))
            {
                result = ButtonResult.OK;
            }
            else if (string.Equals(para, "false", StringComparison.OrdinalIgnoreCase))
            {
                result = ButtonResult.Cancel;
            }

            RaiseRequestClose(new DialogResult(result));
        }
        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

    }
}
