using System.Web.Hosting;
using Fred.DesignPatterns.DI;
using Fred.Framework.Web.SignalR;
using Fred.ImportExport.Bootstrapper.DependencyInjection;
using Fred.ImportExport.Bootstrapper.Extensions;
using Fred.ImportExport.Bootstrapper.Hangfire;
using Hangfire;

namespace Fred.ImportExport.Web
{
    /// <summary>
    /// Configuration d'Hangfire
    /// Faire en Sorte que le serveur hangfire tourne toujours 
    /// https://docs.hangfire.io/en/latest/deployment-to-production/making-aspnet-app-always-running.html
    /// </summary>
    public class HangfireBootstrapper : IRegisteredObject
    {
        public static readonly HangfireBootstrapper Instance = new HangfireBootstrapper();

        private readonly object lockObject = new object();
        private bool started;

        private BackgroundJobServer backgroundJobServer;

        private HangfireBootstrapper()
        {
        }

        public void Start()
        {
            lock (lockObject)
            {
                if (started)
                {
                    return;
                }

                started = true;

                HostingEnvironment.RegisterObject(this);

                IDependencyInjectionService dependencyInjectionService = DependencyInjectionConfig.HangfireDependencyInjectionService;
                SignalRRegistrar.RegisterServerNotificationsEngineDependencies(dependencyInjectionService);

                GlobalConfiguration.Configuration.UseDependencyActivator(dependencyInjectionService);
                GlobalConfiguration.Configuration.UseSqlServerStorage("HangfireConnection");

                RecurringJobsLauncher.Start(dependencyInjectionService);

                backgroundJobServer = new BackgroundJobServer();
            }
        }

        public void Stop()
        {
            lock (lockObject)
            {
                if (backgroundJobServer != null)
                {
                    backgroundJobServer.Dispose();
                }

                HostingEnvironment.UnregisterObject(this);
            }
        }

        void IRegisteredObject.Stop(bool immediate)
        {
            Stop();
        }
    }
}
