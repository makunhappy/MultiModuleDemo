using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Common;
using Common.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace ModuleB.ViewModels
{
    public class ModuleBViewModel : ViewModelBase
    {
        private readonly IEventAggregator _aggregator;
        public ModuleBViewModel(IEventAggregator eventAggregator,
            ILanguageChangedService languageChangedService) : base(languageChangedService, "moduleB")
        {
            _aggregator = eventAggregator;
            _aggregator.GetEvent<DemoEvent>().Subscribe(() =>
            {
                MessageBox.Show("get demo event within moduleb");
            });
        }
        private DelegateCommand loadedCommand;
        public DelegateCommand LoadedCommand => loadedCommand ?? (loadedCommand = new DelegateCommand(OnLoaded));
        private void OnLoaded()
        {
            //MessageBox.Show("ModuleBViewModel loaded");
        }
        private DelegateCommand unloadedCommand;
        public DelegateCommand UnloadedCommand => unloadedCommand ?? (unloadedCommand = new DelegateCommand(OnUnloaded));
        private void OnUnloaded()
        {
            //MessageBox.Show("ModuleBViewModel unloaded");
        }
    }
}
