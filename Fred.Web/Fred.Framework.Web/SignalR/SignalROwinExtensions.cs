using Fred.DesignPatterns.DI;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;

namespace Fred.Framework.Web.SignalR
{
    public static class SignalROwinExtensions
    {
        public static IAppBuilder MapServerNotificationsEngine(this IAppBuilder app, IDependencyInjectionService dependencyInjectionService)
        {
            HubConfiguration hubConfiguration = SignalRRegistrar.GetHubConfiguration(dependencyInjectionService);

            return app.MapSignalR(hubConfiguration);
        }

        public static IAppBuilder RunServerNotificationsEngine(this IAppBuilder app, IDependencyInjectionService dependencyInjectionService)
        {
            HubConfiguration hubConfiguration = SignalRRegistrar.GetHubConfiguration(dependencyInjectionService);

            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                map.RunSignalR(hubConfiguration);
            });

            return app;
        }

        public static IAppBuilder RegisterServerNotificationsEngineDependencies(this IAppBuilder app, IDependencyInjectionService dependencyInjectionService)
        {
            SignalRRegistrar.RegisterServerNotificationsEngineDependencies(dependencyInjectionService);

            return app;
        }
    }
}