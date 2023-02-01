using Common;
using Prism;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiModuleDemo.ServiceAdapter
{
    public abstract class ServicePrismApp : PrismApplicationBase
    {
        protected override IContainerExtension CreateContainerExtension()
        {
            return new ServiceContainerExtension();
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<SelectorRegionAdapter>();
            containerRegistry.Register<ItemsControlRegionAdapter>();
            containerRegistry.Register<ContentControlRegionAdapter>();

            RegisterBehaviorsType(containerRegistry);

            containerRegistry.RegisterDialog<DialogView>(DialogNames.DialogView);
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
