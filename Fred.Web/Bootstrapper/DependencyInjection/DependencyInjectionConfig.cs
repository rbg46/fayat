using System;
using System.Collections.Generic;
using Bootstrapper.AutoMapper;
using Fred.Business.Budget.Details;
using Fred.DesignPatterns.DI;
using Fred.DesignPatterns.DI.EntityFramework;
using Fred.DesignPatterns.DI.FluentValidation;
using Fred.DesignPatterns.DI.Framework;
using Fred.DesignPatterns.DI.Managers;
using Fred.DesignPatterns.DI.Repositories;
using Fred.DesignPatterns.DI.Services;
using Fred.DesignPatterns.DI.SignalR;
using Fred.DesignPatterns.DI.Unity;
using Fred.GroupSpecific.GroupResolution;
using Unity;
using Unity.Lifetime;

namespace Fred.Web.Bootstrapper.DependencyInjection
{
    public static class DependencyInjectionConfig
    {
        private static readonly Lazy<IUnityContainer> LazyContainer = new Lazy<IUnityContainer>(CreateContainer);
        private static readonly Lazy<IDependencyInjectionService> LazyDependencyInjectionService = new Lazy<IDependencyInjectionService>(CreateDependencyInjectionService);
        private static readonly Lazy<IDependencyInjectionService> LazyTestDependencyInjectionService = new Lazy<IDependencyInjectionService>(CreateTestDependencyInjectionService);

        public static IUnityContainer Container => LazyContainer.Value;
        public static IDependencyInjectionService DependencyInjectionService => LazyDependencyInjectionService.Value;
        public static IDependencyInjectionService TestDependencyInjectionService => LazyTestDependencyInjectionService.Value;

        private static IUnityContainer CreateContainer() => new UnityContainer();

        private static IDependencyInjectionService CreateDependencyInjectionService()
        {
            var dependencyInjectionService = new UnityDependencyInjectionService(Container, GetTypeLifetimeManager, GetFactoryLifetimeManager);
            RegisterTypes(dependencyInjectionService);

            return dependencyInjectionService;

            ITypeLifetimeManager GetTypeLifetimeManager() => new DependencyInjectionMvcLifetimeManager();

            IFactoryLifetimeManager GetFactoryLifetimeManager() => new DependencyInjectionMvcLifetimeManager();
        }

        private static IDependencyInjectionService CreateTestDependencyInjectionService()
        {
            var dependencyInjectionService = new UnityDependencyInjectionService(Container, GetTypeLifetimeManager, GetFactoryLifetimeManager);
            RegisterTypes(dependencyInjectionService);

            return dependencyInjectionService;

            ITypeLifetimeManager GetTypeLifetimeManager() => new PerThreadLifetimeManager();

            IFactoryLifetimeManager GetFactoryLifetimeManager() => new PerThreadLifetimeManager();
        }

        private static void RegisterTypes(IDependencyInjectionService dependencyInjectionService)
        {
            var registrars = new List<DependencyRegistrar>
            {
                new RepositoriesRegistrar(dependencyInjectionService),
                new FrameworkRegistrar(dependencyInjectionService),
                new BusinessManagersRegistrar(dependencyInjectionService),
                new ExternalManagersRegistrar(dependencyInjectionService),
                new FeatureManagersRegistrar(dependencyInjectionService),
                new ServicesRegistrar(dependencyInjectionService),
                new ValidatorsRegistrar(dependencyInjectionService),
                new MultipleRepositoriesRegistrar(dependencyInjectionService),
                new ExternalRepositoriesRegistrar(dependencyInjectionService),
                new UnitOfWorkRegistrar(dependencyInjectionService),
                new GroupSpecificServicesRegistrar(dependencyInjectionService),
                new SignalRRegistrar(dependencyInjectionService)
            };

            foreach (DependencyRegistrar registrar in registrars)
                registrar.RegisterTypes();

            dependencyInjectionService.RegisterType<IDependencyInjectionService, UnityDependencyInjectionService>();
            dependencyInjectionService.RegisterType<IDependencyInjectionFactoryResolver, UserGroupAwareServiceResolver>();

            dependencyInjectionService.RegisterType<IBudgetDetailsExportExcelFeature, BudgetDetailsExportExcelFeature>();
            dependencyInjectionService.RegisterInstance(AutoMapperConfig.CreateMapper());
        }
    }
}
