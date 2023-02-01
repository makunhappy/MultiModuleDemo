using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiModuleDemo.ServiceAdapter
{
    public class ServiceScopedProvider : IScopedProvider
    {
        private readonly IServiceProvider _provider;
        public ServiceScopedProvider(IServiceProvider container)
        {
            _provider = container;
        }
        public bool IsAttached { get; set; }

        public IScopedProvider CurrentScope => this;

        public IScopedProvider CreateScope() => this;

        public void Dispose()
        {
        }

        public object Resolve(Type type)
        {
            return _provider.GetRequiredService(type);
        }

        public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
        {
            throw new NotSupportedException();
            //return Container.Resolve(type, parameters.Select(p => new TypedParameter(p.Type, p.Instance)));
        }

        public object Resolve(Type type, string name)
        {
            throw new NotSupportedException();
            //return Container.ResolveNamed(name, type);
        }

        public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
        {
            throw new NotSupportedException();
            //return Container.ResolveNamed(name, type, parameters.Select(p => new TypedParameter(p.Type, p.Instance)));
        }
    }
}
