using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace ModuleA
{
    public class ModuleAModule : ModuleBase
    {
        public override Type ViewType => typeof(Views.ModuleAView);


        public override void Initialized(IContainerProvider containerProvide)
        {
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
