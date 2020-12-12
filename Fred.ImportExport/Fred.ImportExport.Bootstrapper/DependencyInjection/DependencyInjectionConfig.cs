using System;
using System.Collections.Generic;
using CommonServiceLocator;
using Fred.DesignPatterns.DI;
using Fred.DesignPatterns.DI.EntityFramework;
using Fred.DesignPatterns.DI.FluentValidation;
using Fred.DesignPatterns.DI.Framework;
using Fred.DesignPatterns.DI.Managers;
using Fred.DesignPatterns.DI.Repositories;
using Fred.DesignPatterns.DI.Services;
using Fred.DesignPatterns.DI.SignalR;
using Fred.DesignPatterns.DI.Transversal;
using Fred.DesignPatterns.DI.Unity;
using Fred.GroupSpecific.GroupResolution;
using Fred.ImportExport.Bootstrapper.Automappers;
using Fred.ImportExport.Business;
using Fred.ImportExport.Business.Commande;
using Fred.ImportExport.Business.Personnel.Logs;
using Fred.ImportExport.Business.ValidationPointage.Fes.Logs;
using Fred.ImportExport.DataAccess.ExternalService;
using Fred.ImportExport.Framework.Log;
using Unity;
using Unity.AspNet.Mvc;
using Unity.Lifetime;
using Unity.ServiceLocation;

namespace Fred.ImportExport.Bootstrapper.DependencyInjection
{
    public static class DependencyInjectionConfig
    {
        private static readonly Lazy<IUnityContainer> LazyContainer = new Lazy<IUnityContainer>(CreateContainer);
        private static readonly Lazy<IDependencyInjectionService> LazyDependencyInjectionService = new Lazy<IDependencyInjectionService>(CreateDependencyInjectionService);
        private static readonly Lazy<IDependencyInjectionService> LazyHangfireDependencyInjectionService = new Lazy<IDependencyInjectionService>(CreateHangfireDependencyInjectionService);

        public static IUnityContainer Container => LazyContainer.Value;
        public static IDependencyInjectionService DependencyInjectionService => LazyDependencyInjectionService.Value;
        public static IDependencyInjectionService HangfireDependencyInjectionService => LazyHangfireDependencyInjectionService.Value;

        private static IUnityContainer CreateContainer() => new UnityContainer();

        private static IDependencyInjectionService CreateDependencyInjectionService()
        {
            var dependencyInjectionService = new UnityDependencyInjectionService(Container, GetTypeLifetimeManager, GetFactoryLifetimeManager);
            RegisterTypes(dependencyInjectionService);

            return dependencyInjectionService;

            ITypeLifetimeManager GetTypeLifetimeManager() => new PerRequestLifetimeManager();

            IFactoryLifetimeManager GetFactoryLifetimeManager() => new PerRequestLifetimeManager();
        }

        private static IDependencyInjectionService CreateHangfireDependencyInjectionService()
        {
            var container = new UnityContainer();
            var dependencyInjectionService = new UnityDependencyInjectionService(container, GetHangfireTypeLifetimeManager, GetHangfireFactoryLifetimeManager);
            RegisterTypes(dependencyInjectionService);

            var locator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => locator);

            return dependencyInjectionService;

            ITypeLifetimeManager GetHangfireTypeLifetimeManager() => new PerResolveLifetimeManager();

            IFactoryLifetimeManager GetHangfireFactoryLifetimeManager() => new PerResolveLifetimeManager();
        }

        private static void RegisterTypes(IDependencyInjectionService dependencyInjectionService)
        {
            var registrars = new List<DependencyRegistrar>
            {
                new ImportExportUnitOfWorkRegistrar(dependencyInjectionService),
                new ImportExportBusinessFactoriesRegistrar(dependencyInjectionService),
                new ImportExportBusinessManagersRegistrar(dependencyInjectionService),
                new ImportExportBusinessHelpersRegistrar(dependencyInjectionService),
                new ImportExportRepositoriesRegistrar(dependencyInjectionService),
                new RepositoriesRegistrar(dependencyInjectionService),
                new MultipleRepositoriesRegistrar(dependencyInjectionService),
                new FrameworkRegistrar(dependencyInjectionService),
                new BusinessManagersRegistrar(dependencyInjectionService),
                new FeatureManagersRegistrar(dependencyInjectionService),
                new ValidatorsRegistrar(dependencyInjectionService),
                new ServicesRegistrar(dependencyInjectionService),
                new ImportExportServicesRegistrar(dependencyInjectionService),
                new UnitOfWorkRegistrar(dependencyInjectionService),
                new GroupSpecificServicesRegistrar(dependencyInjectionService),
                new ImportExportGroupSpecificServicesRegistrar(dependencyInjectionService),
                new ImportExportSignalRRegistrar(dependencyInjectionService)
            };

            foreach (DependencyRegistrar registrar in registrars)
                registrar.RegisterTypes();

            dependencyInjectionService.RegisterType<IDependencyInjectionService, UnityDependencyInjectionService>();
            dependencyInjectionService.RegisterType<IDependencyInjectionFactoryResolver, ServiceAccountGroupAwareServiceResolver>();

            dependencyInjectionService.RegisterType<IFredIEService, LoggingAdministratorService>();
            dependencyInjectionService.RegisterType<ILoggingAdministratorExternalService, LoggingAdministratorExternalService>();
            dependencyInjectionService.RegisterType<IImportExportLoggingService, ImportExportLoggingService>();
            dependencyInjectionService.RegisterType<IValidationPointageFesLogger, ValidationPointageFesLogger>();
            dependencyInjectionService.RegisterType<CommandeScheduler>();
            dependencyInjectionService.RegisterType<ImportPersonnelFesLogs>();
            dependencyInjectionService.RegisterInstance(AutoMapperConfig.CreateMapper());
        }
    }
}
