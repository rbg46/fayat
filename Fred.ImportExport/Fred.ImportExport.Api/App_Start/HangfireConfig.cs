using Fred.DesignPatterns.DI;
using Fred.Framework.Web.SignalR;
using Fred.ImportExport.Bootstrapper.DependencyInjection;
using Fred.ImportExport.Bootstrapper.Extensions;
using Fred.ImportExport.Bootstrapper.Hangfire;
using Hangfire;
using Hangfire.Logging;
using Hangfire.Logging.LogProviders;
using Owin;

namespace Fred.ImportExport.Api
{
    public static class HangfireConfig
    {
        public static void ApplyHangFire(IAppBuilder app)
        {
            IDependencyInjectionService dependencyInjectionService = DependencyInjectionConfig.HangfireDependencyInjectionService;

            app.RegisterServerNotificationsEngineDependencies(dependencyInjectionService);

            GlobalConfiguration.Configuration.UseDependencyActivator(dependencyInjectionService);
            GlobalConfiguration.Configuration.UseSqlServerStorage("HangfireConnection");
            GlobalJobFilters.Filters.Add(new ProlongExpirationTimeAttribute());
            RecurringJobsLauncher.Start(dependencyInjectionService);

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new DashboardAuthorizationFilter() }    // Autorisation d'accès à Hangfire dans les différents environnements
            });
            app.UseHangfireServer();

            LogProvider.SetCurrentLogProvider(new NLogLogProvider());
        }
    }
}
