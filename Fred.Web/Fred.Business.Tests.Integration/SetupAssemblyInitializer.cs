using System;
using System.Collections.Generic;
using Bootstrapper.AutoMapper;
using CommonServiceLocator;
using Fred.Business.Budget.Details;
using Fred.Business.CI;
using Fred.Business.OperationDiverse;
using Fred.Business.Rapport;
using Fred.Business.Reception.Validators;
using Fred.Business.Referential.Materiel;
using Fred.Business.RepartitionEcart;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.DesignPatterns.DI;
using Fred.DesignPatterns.DI.FluentValidation;
using Fred.DesignPatterns.DI.Framework;
using Fred.DesignPatterns.DI.Managers;
using Fred.DesignPatterns.DI.Repositories;
using Fred.DesignPatterns.DI.Services;
using Fred.DesignPatterns.DI.Unity;
using Fred.Entities.Utilisateur;
using Fred.EntityFramework;
using Fred.Framework.ExternalServices.ImportExport;
using Fred.Framework.Security;
using Fred.GroupSpecific.Default;
using Fred.GroupSpecific.Default.ExternalServices;
using Fred.GroupSpecific.Default.Societe;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.ServiceLocation;

namespace Fred.Business.Tests.Integration
{
    [TestClass]
    public static class SetupAssemblyInitializer
    {
        public static UnityContainer Container = new UnityContainer();
        public const int SuperAdminUserId = 1;

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            RegisterTypes(Container, GetTypeLifetimeManager, GetFactoryLifetimeManager);

            InitSuperAdminUserOrganisationAndRole();

            ITypeLifetimeManager GetTypeLifetimeManager() => new PerResolveLifetimeManager();

            IFactoryLifetimeManager GetFactoryLifetimeManager() => new PerResolveLifetimeManager();
        }

        private static void InitSuperAdminUserOrganisationAndRole()
        {
            var utilisateurManager = Container.Resolve<IUtilisateurManager>();
            var listAffectations = new List<AffectationSeuilUtilisateurEnt>();

            UtilisateurEnt utilisateur = utilisateurManager.GetById(SuperAdminUserId);

            AffectationSeuilUtilisateurEnt affectation1 = new AffectationSeuilUtilisateurEnt
            {
                UtilisateurId = utilisateur.UtilisateurId,
                OrganisationId = 1,
                RoleId = 1
            };

            listAffectations.Add(affectation1);

            AffectationSeuilUtilisateurEnt affectation2 = new AffectationSeuilUtilisateurEnt
            {
                UtilisateurId = utilisateur.UtilisateurId,
                OrganisationId = 2,
                RoleId = 1
            };
            listAffectations.Add(affectation2);

            utilisateurManager.UpdateRole(utilisateur.UtilisateurId, listAffectations);
        }

        private static void RegisterTypes(IUnityContainer container, Func<ITypeLifetimeManager> typeLifetimeManagerFactory, Func<IFactoryLifetimeManager> factoryLifetimeManagerFactory)
        {
            var dependencyInjectionService = new UnityDependencyInjectionService(container, typeLifetimeManagerFactory, factoryLifetimeManagerFactory);

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
                new GroupSpecificServicesRegistrar(dependencyInjectionService)
            };

            string connectionString = GetConnectionString();
            var optionsBuilder = new DbContextOptionsBuilder<FredDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            string GetConnectionString()
            {
                IConfigurationRoot builder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.test.json")
                    .Build();

                return builder.GetConnectionString("FredConnection");
            }

            container.RegisterType<FredDbContext>(typeLifetimeManagerFactory(), new InjectionConstructor(optionsBuilder.Options));
            dependencyInjectionService.RegisterType<IUnitOfWork, UnitOfWork>();

            foreach (DependencyRegistrar registrar in registrars)
                registrar.RegisterTypes();

            var securityManagerMock = new Mock<ISecurityManager>();
            securityManagerMock.Setup(s => s.GetUtilisateurId())
                .Returns(SuperAdminUserId);
            dependencyInjectionService.RegisterInstance(typeof(ISecurityManager), securityManagerMock.Object);

            dependencyInjectionService.RegisterType<ISocieteManager, DefaultSocieteManager>();
            dependencyInjectionService.RegisterType<ICIManager, DefaultCIManager>();
            dependencyInjectionService.RegisterType<IImportExportServiceDescriptor, DefaultImportExportServiceDescriptor>();
            dependencyInjectionService.RegisterType<IReceptionQuantityRulesValidator, ReceptionQuantityRulesValidator>();
            dependencyInjectionService.RegisterType<IBudgetDetailsExportExcelFeature, BudgetDetailsExportExcelFeature>();
            dependencyInjectionService.RegisterType<IMaterielManager, DefaultMaterielManager>();
            dependencyInjectionService.RegisterType<IOperationDiverseManager, DefaultOperationDiverseManager>();
            dependencyInjectionService.RegisterType<ISearchFeature, SearchFeature>();
            dependencyInjectionService.RegisterType<ICrudFeature, CrudFeature>();
            dependencyInjectionService.RegisterType<IRepartitionEcartManager, DefaultRepartitionEcartManager>();
            dependencyInjectionService.RegisterInstance(AutoMapperConfig.CreateMapper());

            var locator = new UnityServiceLocator(Container);
            ServiceLocator.SetLocatorProvider(() => locator);
        }
    }
}
