using Common;
using Common.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ModuleA.ViewModels
{
    public class ModuleAViewModel : ViewModelBase
    {
        private readonly IEventAggregator _aggregator;
        private readonly IDialogService _dialogSerivce;
        private readonly ILogService _logSerivce;
        public ModuleAViewModel(IEventAggregator eventAggregator,
            IDialogService dialogService,
            ILanguageChangedService languageChangedService,
            ILogService logService
            ) : base(languageChangedService, "moduleA")
        {
            _aggregator = eventAggregator;
            _dialogSerivce = dialogService;
            _logSerivce = logService;
        }
        protected override void IsActiveChangedCore(bool isActive)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            {
                MessageBox.Show($"module a {IsActive}");
            }));
        }

        private DelegateCommand sendCommand;
        public DelegateCommand SendCommand => sendCommand == null ? sendCommand = new DelegateCommand(OnSend) : sendCommand;
        private void OnSend()
        {
            _logSerivce.Info(ServiceCategory.TubeWarmUp, "send message to module b");
            _aggregator.GetEvent<DemoEvent>().Publish();
        }

        private DelegateCommand dialogCommand;
        public DelegateCommand DialogCommand => dialogCommand == null ? dialogCommand = new DelegateCommand(OnDialog) : dialogCommand;
        private void OnDialog()
        {
            var res = _dialogSerivce.ShowDialogX("Info", "show your message");
        }

    }
}
