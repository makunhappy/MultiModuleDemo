using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Common
{
    public abstract class ModuleBase : IModule
    {
        /// <summary>
        /// 模块界面类型
        /// </summary>
        public abstract Type ViewType { get; }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            IRegion region = regionManager.Regions[RegionNames.ContentRegion];

            var view = containerProvider.Resolve(ViewType);
            region.Add(view);
        }
        public abstract void Initialized(IContainerProvider containerProvide);
        public abstract void RegisterTypes(IContainerRegistry containerRegistry);
    }
}
