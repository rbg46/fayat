using System.Configuration;
using Fred.Framework.Web.SignalR;
using Fred.ImportExport.Bootstrapper.DependencyInjection;
using Fred.ImportExport.Bootstrapper.Owin;
using Owin;

namespace Fred.ImportExport.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            OwinConfig.WebConfigureAuth(app);

            app.RegisterServerNotificationsEngineDependencies(DependencyInjectionConfig.DependencyInjectionService);

            var startHangfire = bool.Parse(ConfigurationManager.AppSettings["HangFire:Start"] ?? "false");
            if (startHangfire)
            {
                HangfireConfig.ApplyHangFire(app);
            }
        }
    }
}
