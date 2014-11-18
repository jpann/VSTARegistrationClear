using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using VSTARegistrationClear.Managers;

namespace VSTARegistrationClear
{
    public class DependencyModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IRegistryManager>().To<RegistryManager>();
        }
    }
}
