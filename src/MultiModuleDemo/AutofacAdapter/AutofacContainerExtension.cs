using Autofac;
using Autofac.Core;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiModuleDemo.AutofacAdapter
{
    public class AutofacContainerExtension : IContainerExtension<IContainer>/*, IContainerInfo*/
    {
        private readonly ContainerBuilder _build;
        public AutofacContainerExtension()
        {
            _build = new ContainerBuilder();
            _build.Register(p => Instance!);
            _build.Register<IContainerExtension>(p => this);
            _build.Register<IContainerProvider>(p => this);
        }
        private IContainer _instance;
        public IContainer Instance => _instance;

        public IScopedProvider CurrentScope => throw new NotImplementedException();

        public IScopedProvider CreateScope()
        {
            return new AutofacScopedProvider(_instance);
        }

        public void FinalizeExtension()
        {
            _instance = _build.Build();
        }

        public bool IsRegistered(Type type)
        {
            return Instance.IsRegistered(type);
        }

        public bool IsRegistered(Type type, string name)
        {
            return Instance.IsRegisteredWithName(name, type);
        }

        public IContainerRegistry Register(Type from, Type to)
        {
            _build.RegisterType(to).As(from);
            return this;
        }

        public IContainerRegistry Register(Type from, Type to, string name)
        {
            _build.RegisterType(to).Named(name,from);
            return this;
        }

        public IContainerRegistry Register(Type type, Func<object> factoryMethod)
        {
            _build.RegisterInstance(() => factoryMethod).As(type);
            return this;
        }

        public IContainerRegistry Register(Type type, Func<IContainerProvider, object> factoryMethod)
        {
            _build.RegisterInstance((IComponentContext ctx) => factoryMethod(ctx.Resolve<IContainerProvider>())).As(type);
            return this;
        }

        public IContainerRegistry RegisterInstance(Type type, object instance)
        {
            _build.RegisterInstance(instance).As(type);
            return this;
        }

        public IContainerRegistry RegisterInstance(Type type, object instance, string name)
        {
            _build.RegisterInstance(instance).Named(name, type);
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
            _build.RegisterType(to).As(from).InstancePerLifetimeScope();
            return this;
        }

        public IContainerRegistry RegisterScoped(Type type, Func<object> factoryMethod)
        {
            _build.RegisterInstance(() => factoryMethod()).As(type).InstancePerLifetimeScope();
            return this;
        }

        public IContainerRegistry RegisterScoped(Type type, Func<IContainerProvider, object> factoryMethod)
        {
            _build.RegisterInstance((IComponentContext ctx) => factoryMethod(ctx.Resolve<IContainerProvider>())).As(type)
                .InstancePerLifetimeScope();
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to)
        {
            _build.RegisterType(to).As(from).SingleInstance();
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to, string name)
        {
            _build.RegisterType(to).Named(name, from).SingleInstance();
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type type, Func<object> factoryMethod)
        {
            _build.RegisterInstance(() => factoryMethod()).As(type).SingleInstance();
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type type, Func<IContainerProvider, object> factoryMethod)
        {
            _build.RegisterInstance((IComponentContext ctx) => factoryMethod(ctx.Resolve<IContainerProvider>())).As(type);
            return this;
        }

        public object Resolve(Type type)
        {
            if (!Instance.IsRegistered(type) && !type.IsInterface && !type.IsAbstract)
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
            return Instance.Resolve(type);

        }

        public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
        {
            return Instance.Resolve(type, parameters.Select(p => new TypedParameter(p.Type, p.Instance)));
        }

        public object Resolve(Type type, string name)
        {
            return Instance.ResolveNamed(name, type);
        }

        public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
        {
            return Instance.ResolveNamed(name, type, parameters.Select(p => new TypedParameter(p.Type, p.Instance)));
        }
    }
}
