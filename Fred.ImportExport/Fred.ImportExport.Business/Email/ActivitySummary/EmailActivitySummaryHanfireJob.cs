using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Web.Hosting;
using Fred.Business.Email.ActivitySummary;
using Fred.Entities;
using Fred.Framework.DateTimeExtend;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.Framework.Exceptions;
using Hangfire;

namespace Fred.ImportExport.Business.Email.ActivitySummary
{
    public class EmailActivitySummaryHanfireJob : IEmailActivitySummaryHanfireJob
    {
        private readonly IActivitySummaryService activitySummaryService;

        public EmailActivitySummaryHanfireJob(IActivitySummaryService activitySummaryService)
        {
            this.activitySummaryService = activitySummaryService;
        }

        /// <summary>
        ///   Génération automatique de mail recapitulant les activités en cours.
        ///   Lancé tous les jours à 3h du matin    
        /// </summary>
        [AutomaticRetry(Attempts = 0)]
        [DisplayName("[EMAIL][RECAPITULATIF_ACTIVITE] Génération automatique de mail recapitulant les activités en cours.")]
        public async Task SendEmailJob()
        {
            await JobRunnerApiRestHelper.PostAsync("SendEmail", Constantes.CodeGroupeDefault);
        }

        public void SendEmail()
        {
            try
            {
                DateTime dateOfExecution = DateTime.UtcNow;
                NLog.LogManager.GetCurrentClassLogger().Info($"[EMAIL][RECAPITULATIF_ACTIVITE] Demarrage de la génération du {dateOfExecution}.");
                if (dateOfExecution.IsBusinessDay())
                {
                    activitySummaryService.Execute(dateOfExecution, HostingEnvironment.ApplicationPhysicalPath);
                }
                else
                {
                    NLog.LogManager.GetCurrentClassLogger().Info($"[EMAIL][RECAPITULATIF_ACTIVITE] la date suivante {dateOfExecution} est un jour non ouvré. Il n'y aura pas d'envoie de mail envoyé.");
                }
            }
            catch (Exception e)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(e, "[EMAIL][RECAPITULATIF_ACTIVITE] Erreur lors de la génération automatique de mail.");
                throw new FredIeBusinessException(e.Message, e);
            }
        }
    }
}
