using Fred.DesignPatterns.DI;
using Fred.SignalR;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace Fred.Framework.Web.SignalR
{
    public static class SignalRRegistrar
    {
        public static HubConfiguration GetHubConfiguration(IDependencyInjectionService dependencyInjectionService)
        {
            SignalRConfiguration signalRConfiguration = BuildSignalRConfiguration(dependencyInjectionService);

            return new HubConfiguration { Resolver = signalRConfiguration.DependencyResolver };
        }

        public static void RegisterServerNotificationsEngineDependencies(IDependencyInjectionService dependencyInjectionService)
        {
            BuildSignalRConfiguration(dependencyInjectionService);
        }

        private static SignalRConfiguration BuildSignalRConfiguration(IDependencyInjectionService dependencyInjectionService)
        {
            var signalRConfiguration = new SignalRConfiguration(dependencyInjectionService);
            signalRConfiguration.RegisterDependencies();

            return signalRConfiguration;
        }

        private class SignalRConfiguration
        {
            private readonly IDependencyInjectionService dependencyInjectionService;

            public IDependencyResolver DependencyResolver { get; }

            public SignalRConfiguration(IDependencyInjectionService dependencyInjectionService)
            {
                this.dependencyInjectionService = dependencyInjectionService;

                DependencyResolver = new SignalRDependencyResolver(dependencyInjectionService);
            }

            public void RegisterDependencies()
            {
                var connectionManager = DependencyResolver.Resolve<IConnectionManager>();

                dependencyInjectionService.RegisterInstance(connectionManager);
            }
        }
    }
}
