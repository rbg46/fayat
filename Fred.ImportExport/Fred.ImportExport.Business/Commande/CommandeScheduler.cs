using System;
using System.ComponentModel;
using Fred.Business.Reception.Services;
using Fred.ImportExport.Framework.Exceptions;
using Hangfire;

namespace Fred.ImportExport.Business.Commande
{
    /// <summary>
    ///   Ordonnanceur commande
    /// </summary>
    public class CommandeScheduler
    {
        private const string jobId = "CommandeAbonnementJob";

        private readonly IReceptionsAutomaticGenerator receptionsAutomaticGenerator;
        private readonly string cron = Cron.Daily(3, 10); // Tous les jours à 5h10 du matin

        public CommandeScheduler(IReceptionsAutomaticGenerator receptionsAutomaticGenerator)
        {
            this.receptionsAutomaticGenerator = receptionsAutomaticGenerator;
        }

        /// <summary>
        ///   Ajout du job récurrent des commandes abonnement
        /// </summary>
        public void AddCommandeAbonnementRecurringJob()
        {
            try
            {
                RecurringJob.AddOrUpdate(jobId, () => CommandeAbonnementJob(), cron);
            }
            catch (Exception e)
            {
                var exception = new FredIeBusinessException(e.Message, e);
                NLog.LogManager.GetCurrentClassLogger().Error(exception);
                throw exception;
            }
        }

        /// <summary>
        ///   Génération automatique des réceptions des commandes abonnement
        ///   Lancé tous les jours à 3h du matin    
        /// </summary>
        [AutomaticRetry(Attempts = 0)]
        [DisplayName("[COMMANDE][ABONNEMENT] Génération automatique des réceptions des commandes abonnement.")]
        public void CommandeAbonnementJob()
        {
            try
            {
                receptionsAutomaticGenerator.GenerateReceptions();
            }
            catch (Exception e)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(e, "[COMMANDE][ABONNEMENT] Erreur lors de la génération automatique des réceptions des commandes abonnement.");
                throw new FredIeBusinessException(e.Message, e);
            }
        }
    }
}
