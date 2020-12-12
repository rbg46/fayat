using Fred.ImportExport.Bootstrapper.Hangfire;
using Hangfire;
using Owin;

namespace Fred.ImportExport.Web
{
    public static class HangfireConfig
    {
        public static void ApplyHangFire(IAppBuilder app)
        {
            GlobalJobFilters.Filters.Add(new ProlongExpirationTimeAttribute());

            // Autorisation d'accès à Hangfire dans les différents environnements
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new DashboardAuthorizationFilter() }
            });
        }
    }
}
