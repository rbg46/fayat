using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Fred.DesignPatterns.DI;
using Fred.EntityFramework;
using Fred.GroupSpecific.GroupResolution;
using Fred.Web.Bootstrapper.DependencyInjection;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Web.Bootstrapper.Tests
{
    [TestClass]
    public class BootstrapperTests
    {
        private static TestContext testContext;
        private static IDependencyInjectionService dependencyInjectionService;

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            BootstrapperTests.testContext = testContext;
            dependencyInjectionService = DependencyInjectionConfig.TestDependencyInjectionService;
        }

        [TestMethod]
        public void AssertEveryRegistrationCanBeResolved()
        {
            var resolutionErrors = new List<string>();

            testContext.WriteLine("FredDbContext registration replaced with in memory");
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<FredDbContext>();
            dbContextOptionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            dependencyInjectionService.RegisterInstance(new FredDbContext(dbContextOptionsBuilder.Options));

            testContext.WriteLine("IDependencyInjectionFactoryResolver registration replaced with a mock");
            var groupAwareServiceResolverMock = new Mock<GroupAwareServiceResolver>(dependencyInjectionService);
            groupAwareServiceResolverMock.Setup(gasr => gasr.GetCurrentGroupCode()).Returns("Test");
            dependencyInjectionService.RegisterInstance<IDependencyInjectionFactoryResolver>(groupAwareServiceResolverMock.Object);

            testContext.WriteLine("IConnectionManager (SignalR runtime dependency) registration replaced with a mock");
            dependencyInjectionService.RegisterInstance(new Mock<IConnectionManager>().Object);

            IEnumerable<IDependencyInjectionRegistration> registrations = dependencyInjectionService.GetRegistrations().ToList();
            testContext.WriteLine($"{registrations.LongCount()} type registration(s) found.");

            foreach (IDependencyInjectionRegistration registration in registrations)
            {
                try
                {
                    if (IsRegistrationExcluded(registration))
                        continue;

                    dependencyInjectionService.Resolve(registration.MappedToType);
                }
                catch (Exception e)
                {
                    resolutionErrors.Add($"{registration.MappedToType} can't be resolved:{Environment.NewLine + e.Message}");
                }
            }

            bool hasResolutionErrors = resolutionErrors.Any();
            Assert.IsFalse(hasResolutionErrors, $"At least one {resolutionErrors.Count} registration can't be resolved:{Environment.NewLine + string.Join(Environment.NewLine, resolutionErrors)}");

            bool IsRegistrationExcluded(IDependencyInjectionRegistration registration)
            {
                if (registration.MappedToType.IsGenericType)
                    return true;
                if (registration.MappedToType == typeof(Mapper))
                    return true;

                return false;
            }
        }
    }
}
