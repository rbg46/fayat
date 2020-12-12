using System.Web.Http;
using Fred.Framework.Web.SignalR;
using Fred.ImportExport.Api.App_Start;
using Fred.ImportExport.Bootstrapper.DependencyInjection;
using Fred.ImportExport.Bootstrapper.Owin;
using Owin;

namespace Fred.ImportExport.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            OwinConfig.ApiConfigureAuth(app);

            app.RunServerNotificationsEngine(DependencyInjectionConfig.DependencyInjectionService);

            HangfireConfig.ApplyHangFire(app);
            VersioningConfig.ApplyApiExplorer(GlobalConfiguration.Configuration);
            SwaggerConfig.Configure(GlobalConfiguration.Configuration);
        }
    }
}
