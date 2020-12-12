using System;
using System.Collections.Generic;
using System.Linq;
using Unity;
using Unity.Lifetime;

namespace Fred.DesignPatterns.DI.Unity
{
    public class UnityDependencyInjectionService : IDependencyInjectionService
    {
        private readonly IUnityContainer container;
        private readonly Func<ITypeLifetimeManager> typeLifetimeManagerFactory;
        private readonly Func<IFactoryLifetimeManager> factoryLifetimeManagerFactory;

        public UnityDependencyInjectionService(IUnityContainer container, Func<ITypeLifetimeManager> typeLifetimeManagerFactory, Func<IFactoryLifetimeManager> factoryLifetimeManagerFactory)
        {
            this.container = container;
            this.typeLifetimeManagerFactory = typeLifetimeManagerFactory;
            this.factoryLifetimeManagerFactory = factoryLifetimeManagerFactory;
        }

        public void RegisterFactory<T>() => container.RegisterFactory<T>(c => c.Resolve<IDependencyInjectionFactoryResolver>().Resolve<T>(), factoryLifetimeManagerFactory());

        public void RegisterInstance<TInterface>(TInterface instance) => container.RegisterInstance(instance);

        public void RegisterInstance(Type t, object instance) => container.RegisterInstance(t, instance, new ContainerControlledLifetimeManager());

        public void RegisterType<T>() => container.RegisterType<T>(typeLifetimeManagerFactory());

        public void RegisterType<TFrom, TTo>() where TTo : TFrom => container.RegisterType<TFrom, TTo>(typeLifetimeManagerFactory());

        public void RegisterType<TFrom, TTo>(string name) where TTo : TFrom => container.RegisterType<TFrom, TTo>(name, typeLifetimeManagerFactory());

        public void RegisterType(Type fromType, Type toType) => container.RegisterType(fromType, toType, typeLifetimeManagerFactory());

        public T Resolve<T>() => container.Resolve<T>();

        public T Resolve<T>(string name) => container.Resolve<T>(name);

        public object Resolve(Type type) => container.Resolve(type);

        public IEnumerable<object> ResolveAll(Type type) => container.ResolveAll(type);

        public bool IsRegistered<T>(string name) => container.IsRegistered<T>(name);

        public bool IsRegistered(Type type) => container.IsRegistered(type);

        public IEnumerable<IDependencyInjectionRegistration> GetRegistrations() => container.Registrations.Select(cr => new DependencyInjectionRegistration(cr));

        private class DependencyInjectionRegistration : IDependencyInjectionRegistration
        {
            public string Name { get; }
            public Type MappedToType { get; }

            public DependencyInjectionRegistration(IContainerRegistration containerRegistration)
            {
                Name = containerRegistration.Name;
                MappedToType = containerRegistration.MappedToType;
            }
        }
    }
}