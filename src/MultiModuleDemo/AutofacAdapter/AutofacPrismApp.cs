using Common;
using Prism;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MultiModuleDemo.AutofacAdapter
{
    public abstract class AutofacPrismApp : PrismApplicationBase
    {
        protected override IContainerExtension CreateContainerExtension()
        {
            return new AutofacContainerExtension();
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<SelectorRegionAdapter>();
            containerRegistry.Register<ItemsControlRegionAdapter>();
            containerRegistry.Register<ContentControlRegionAdapter>();

            RegisterBehaviorsType(containerRegistry);

            containerRegistry.RegisterDialog<DialogView>(DialogNames.DialogView);
            containerRegistry.RegisterDialogWindow<DialogWindow>();

            //add service
            containerRegistry.RegisterSingleton<ILanguageChangedService, LanguageChangedService>();
            containerRegistry.RegisterSingleton<ILogService, LogService>();
        }
        private void RegisterBehaviorsType(IContainerRegistry containerRegistry)
        {
            var ass = AppDomain.CurrentDomain.GetAssemblies();
            var assembly = ass.First(p => !string.IsNullOrEmpty(p.FullName) && p.FullName.Contains("Prism.Wpf"));
            var types = assembly.GetTypes();
            var behaviros = types.Where(p => p.Namespace == "Prism.Regions.Behaviors").Where(p => !(p.IsInterface || p.IsAbstract));
            foreach (var item in behaviros)
            {
                containerRegistry.Register(item);
            }
        }
    }
}
