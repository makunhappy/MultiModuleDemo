using Common;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleB
{
    public class ModuleBModule : ModuleBase
    {
        public override Type ViewType => typeof(Views.ModuleBView);

        public override void Initialized(IContainerProvider containerProvide)
        {
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
