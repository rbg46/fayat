using Fred.DesignPatterns.DI;
using Fred.ImportExport.Business.Commande;
using Fred.ImportExport.Business.Email;

namespace Fred.ImportExport.Bootstrapper.Hangfire
{
    public static class RecurringJobsLauncher
    {
        public static void Start(IDependencyInjectionService dependencyInjectionService)
        {
            var commandScheduler = dependencyInjectionService.Resolve<CommandeScheduler>();
            commandScheduler.AddCommandeAbonnementRecurringJob();

            var emailScheduler = dependencyInjectionService.Resolve<EmailScheduler>();
            emailScheduler.RegisterEmailCampaigns();
        }
    }
}
