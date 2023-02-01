using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiModuleDemo.AutofacAdapter
{
    public class AutofacScopedProvider : IScopedProvider
    {
        IContainer Container;
        public AutofacScopedProvider(IContainer container)
        {
            Container = container;
        }
        public bool IsAttached { get; set; }

        public IScopedProvider CurrentScope => this;

        public IScopedProvider CreateScope() => this;

        public void Dispose()
        {
            Container.Dispose();
            Container = null;
        }

        public object Resolve(Type type)
        {
            return Container.Resolve(type);
        }

        public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
        {
            return Container.Resolve(type, parameters.Select(p => new TypedParameter(p.Type, p.Instance)));
        }

        public object Resolve(Type type, string name)
        {
            return Container.ResolveNamed(name, type);
        }

        public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
        {
            return Container.ResolveNamed(name, type, parameters.Select(p => new TypedParameter(p.Type, p.Instance)));
        }
    }
}
