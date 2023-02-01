using Prism.Ioc;
using Prism;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Microsoft.Extensions.DependencyInjection;

namespace MultiModuleDemo.ServiceAdapter
{
    public class ServiceContainerExtension : IContainerExtension<IServiceProvider>
    {
        private readonly ServiceCollection _services;
        public ServiceContainerExtension()
        {
            _services = new ServiceCollection();
            _services.AddSingleton<IServiceProvider>(p => this.Instance!);
            _services.AddSingleton<IContainerExtension>(p => this);
            _services.AddSingleton<IContainerProvider>(p => this);

        }
        private IServiceProvider _instance;
        public IServiceProvider Instance => _instance;

        public IScopedProvider CurrentScope => throw new NotImplementedException();

        public IScopedProvider CreateScope()
        {
            return new ServiceScopedProvider(_instance);
        }

        public void FinalizeExtension()
        {
            _instance = _services.BuildServiceProvider();
        }

        public bool IsRegistered(Type type)
        {
            return Instance.GetRequiredService<IServiceProviderIsService>().IsService(type);
        }

        public bool IsRegistered(Type type, string name)
        {
            throw new NotSupportedException();
            //return Instance.GetRequiredService<IServiceProviderIsService>().IsService(name, type);
        }

        public IContainerRegistry Register(Type from, Type to)
        {
            _services.AddSingleton(from, to);
            //_build.RegisterType(to).As(from);
            return this;
        }

        public IContainerRegistry Register(Type from, Type to, string name)
        {
            throw new NotSupportedException();
            //_build.RegisterType(to).As(from);
            return this;
        }

        public IContainerRegistry Register(Type type, Func<object> factoryMethod)
        {
            _services.AddSingleton(type, factoryMethod);
            //_build.RegisterInstance(() => factoryMethod).As(type);
            return this;
        }

        public IContainerRegistry Register(Type type, Func<IContainerProvider, object> factoryMethod)
        {
            _services.AddSingleton(type, provider => factoryMethod(provider.GetRequiredService<IContainerProvider>()));
            //_build.RegisterInstance((IComponentContext ctx) => factoryMethod(ctx.Resolve<IContainerProvider>())).As(type);
            return this;
        }

        public IContainerRegistry RegisterInstance(Type type, object instance)
        {
            _services.AddSingleton(type, instance);
            //_build.RegisterInstance(instance).As(type);
            return this;
        }

        public IContainerRegistry RegisterInstance(Type type, object instance, string name)
        {
            throw new NotSupportedException();
            //_build.RegisterInstance(instance).Named(name, type);
            return this;
        }

        public IContainerRegistry RegisterMany(Type type, params Type[] serviceTypes)
        {
            throw new NotImplementedException();
        }

        public IContainerRegistry RegisterManySingleton(Type type, params Type[] serviceTypes)
        {
            throw new NotImplementedException();
        }

        public IContainerRegistry RegisterScoped(Type from, Type to)
        {
            _services.AddScoped(from, to);
            //_build.RegisterType(to).As(from).InstancePerLifetimeScope();
            return this;
        }

        public IContainerRegistry RegisterScoped(Type type, Func<object> factoryMethod)
        {
            _services.AddScoped(type, provider => factoryMethod());
            //_build.RegisterInstance(() => factoryMethod()).As(type).InstancePerLifetimeScope();
            return this;
        }

        public IContainerRegistry RegisterScoped(Type type, Func<IContainerProvider, object> factoryMethod)
        {
            _services.AddScoped(type, provider => factoryMethod(provider.GetRequiredService<IContainerProvider>()));
            //_build.RegisterInstance((IComponentContext ctx) => factoryMethod(ctx.Resolve<IContainerProvider>())).As(type)
            //    .InstancePerLifetimeScope();
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to)
        {
            _services.AddSingleton(from, to);
            //_build.RegisterType(to).As(from).SingleInstance();
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to, string name)
        {
            throw new NotSupportedException();
            //_build.RegisterType(to).Named(name, from).SingleInstance();
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type type, Func<object> factoryMethod)
        {
            _services.AddSingleton(type, provider => factoryMethod());
            //_build.RegisterInstance(() => factoryMethod()).As(type).SingleInstance();
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type type, Func<IContainerProvider, object> factoryMethod)
        {
            _services.AddSingleton(type, provider => factoryMethod(provider.GetRequiredService<IContainerProvider>()));
            //_build.RegisterInstance((IComponentContext ctx) => factoryMethod(ctx.Resolve<IContainerProvider>())).As(type);
            return this;
        }

        public object Resolve(Type type)
        {
            if (!IsRegistered(type) && !type.IsInterface && !type.IsAbstract)
            {
                var constructors = type.GetConstructors();
                var parasCounts = constructors.Select(p => p.GetParameters().Count());
                var constructorIndex = parasCounts.ToList().IndexOf(parasCounts.Max());
                var constructor = constructors[constructorIndex];
                var paras = constructor.GetParameters();
                (Type t, object v)[] values = new (Type t, object v)[paras.Length];
                for (int i = 0; i < paras.Length; i++)
                {
                    values[i] = (paras[i].ParameterType, Resolve(paras[i].ParameterType));
                }
                return constructor.Invoke(values.Select(p => p.v).ToArray());
            }
            return Instance.GetRequiredService(type);

        }

        public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
        {
            throw new NotSupportedException();
            //return Instance.Resolve(type, parameters.Select(p => new TypedParameter(p.Type, p.Instance)));
        }

        public object Resolve(Type type, string name)
        {
            throw new NotSupportedException();
            //return Instance.ResolveNamed(name, type);
        }

        public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
        {
            throw new NotSupportedException();
            //return Instance.ResolveNamed(name, type, parameters.Select(p => new TypedParameter(p.Type, p.Instance)));
        }
    }
}
