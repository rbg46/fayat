using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.Notification;
using Fred.Business.Rapport.Pointage;
using Fred.Business.RapportPrime;
using Fred.Business.ValidationPointage;
using Fred.Entities;
using Fred.Entities.Notification;
using Fred.Entities.Rapport;
using Fred.Entities.RapportPrime;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.Business.Hangfire.Parameters;
using Fred.ImportExport.Business.ValidationPointage.Common;
using Fred.ImportExport.Business.ValidationPointage.Ftp.Common;
using Fred.ImportExport.Business.ValidationPointage.Ftp.Common.Interfaces;
using Fred.ImportExport.Business.ValidationPointage.Ftp.Mail;
using Fred.ImportExport.Business.ValidationPointage.Ftp.RemonteeVrac.Interfaces;
using Fred.Web.Shared.App_LocalResources;
using Hangfire;
using Hangfire.Server;

namespace Fred.ImportExport.Business.ValidationPointage.Ftp.RemonteeVrac
{
    public class RemonteeVracFtpHangFireJob : IRemonteeVracFtpHangFireJob
    {
        private readonly IValidationPointageContextDataProvider validationPointageContextDataProvider;
        private readonly IRemonteeVracManager remonteeVracManager;
        private readonly IRemonteeVracFtpQueryBuilder remonteeVracAnaelQueryBuilder;
        private readonly IRemonteeVracFtpQueryExecutor remonteeVracAnaelQueryExecutor;
        private readonly INotificationManager notificationManager;
        private readonly IPointageManager pointageManager;
        private readonly IRapportPrimeLignePrimeManager rapportPrimeLignePrimeManager;
        private readonly IFtpQueryBuilder ftpQueryBuilder;
        private readonly IFtpQueryExecutor ftpQueryExecutor;

        public RemonteeVracFtpHangFireJob(
            IValidationPointageContextDataProvider validationPointageContextDataProvider,
            IRemonteeVracManager remonteeVracManager,
            IRemonteeVracFtpQueryBuilder remonteeVracAnaelQueryBuilder,
            IRemonteeVracFtpQueryExecutor remonteeVracAnaelQueryExecutor,
            INotificationManager notificationManager,
            IPointageManager pointageManager,
            IRapportPrimeLignePrimeManager rapportPrimeLignePrimeManager,
            IFtpQueryBuilder ftpQueryBuilder,
            IFtpQueryExecutor ftpQueryExecutor)
        {
            this.validationPointageContextDataProvider = validationPointageContextDataProvider;
            this.remonteeVracManager = remonteeVracManager;
            this.remonteeVracAnaelQueryBuilder = remonteeVracAnaelQueryBuilder;
            this.remonteeVracAnaelQueryExecutor = remonteeVracAnaelQueryExecutor;
            this.notificationManager = notificationManager;
            this.pointageManager = pointageManager;
            this.rapportPrimeLignePrimeManager = rapportPrimeLignePrimeManager;
            this.ftpQueryBuilder = ftpQueryBuilder;
            this.ftpQueryExecutor = ftpQueryExecutor;
        }

        /// <summary>
        ///   Gestion de la Remontée vrac
        /// </summary>
        /// <param name="remonteeVracId">Identifiant de la Remontée Vrac</param>
        /// <param name="periode">Période choisie</param>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        /// <param name="filtre">Filtre</param>
        /// <param name="context">Context hangfire</param>
        /// <remarks>Le jobId hangfire sera utilisé pour que l'appel au programme AS400 de remontée vrac soit unique. 
        /// Donc l'insertion des pointages et des primes utiliseront également cet identifiant unique "jobId"
        /// AutomaticRetry = 1 car la connexion au serveur AS400 est KO par intermittence</remarks>
        [AutomaticRetry(Attempts = 0)]
        [DisplayName("[VALIDATION POINTAGE] Exécution de la Remontée Vrac : RemonteeVracId = {0}, UtilisateurId = {2}")]
        public async Task RemonteeVracJob(int remonteeVracId, DateTime periode, int utilisateurId, PointageFiltre filtre, PerformContext context)
        {
            var parameter = new RemonteeVracJobParameter { RemonteeVracId = remonteeVracId, Periode = periode, UtilisateurId = utilisateurId, Filtre = filtre, BackgroundJobId = context.BackgroundJob.Id };

            await JobRunnerApiRestHelper.PostAsync("RemonteeVracFtpJob", Constantes.CodeGroupeFTP, parameter);
        }

        public async Task RemonteeVracJobAsync(RemonteeVracJobParameter parameter)
        {
            int remonteeVracId = parameter.RemonteeVracId;
            DateTime periode = parameter.Periode;
            int utilisateurId = parameter.UtilisateurId;
            PointageFiltre filtre = parameter.Filtre;
            string backgroundJobId = parameter.BackgroundJobId;

            ValidationPointageContextData globalData = validationPointageContextDataProvider.GetGlobalData(utilisateurId, filtre.SocieteId, backgroundJobId);

            RemonteeVracEnt remonteeVrac = remonteeVracManager.Get(remonteeVracId);
            try
            {
                // Récupération des pointages
                IEnumerable<RapportLigneEnt> pointages = (await pointageManager.GetPointagesAsync(periode))
                    .Where(x => x.Rapport.RapportStatutId == RapportStatutEnt.RapportStatutVerrouille.Key);

                var filter = new ValidationPointageFtpFilter();

                //Application des filtres sur les rapportLignes
                pointages = filter.ApplyRapportLigneFilter(pointages, filtre).ToList();

                // Application du filtre sur les rapportLignePrimes
                pointages = filter.ApplyRapportLignePrimesFilter(pointages).ToList();

                string queryRemonteeVrac = remonteeVracAnaelQueryBuilder.BuildRemonteeVracQuery(globalData, periode, filtre);

                List<RapportPrimeLignePrimeEnt> listPrimeMensuelle = rapportPrimeLignePrimeManager.GetRapportPrimeLignePrime(periode, false);

                // Envoie des mails
                ValidationPointageMailSenderFtp.Send(globalData, pointages, listPrimeMensuelle, null, remonteeVrac.Periode, ftpQueryBuilder, queryRemonteeVrac);

                // Insertion des pointages et des primes dans AS400
                ftpQueryExecutor.InsertPointageAndPrime(globalData, periode, pointages, null, remonteeVrac);

                remonteeVracAnaelQueryExecutor.ExectuteRemonteeVrac(globalData, remonteeVrac, queryRemonteeVrac);

                // Mise à jour du statut du contrôle pointage
                remonteeVrac.DateFin = DateTime.UtcNow;
                remonteeVracManager.UpdateRemonteeVrac(remonteeVrac, FluxStatus.Done.ToIntValue());
                await notificationManager.CreateNotificationAsync(utilisateurId, TypeNotification.NotificationUtilisateur, string.Format(FeatureValidationPointage.VPManager_RemonteVracJobSuccess, remonteeVrac.DateDebut.ToLocalTime()));
            }
            catch (Exception e)
            {
                const string errorMsg = "Erreur lors de l'exécution de la remontée vrac.";
                NLog.LogManager.GetCurrentClassLogger().Error(e, errorMsg);
                remonteeVrac.DateFin = DateTime.UtcNow;
                remonteeVracManager.UpdateRemonteeVrac(remonteeVrac, FluxStatus.Failed.ToIntValue());
                await notificationManager.CreateNotificationAsync(utilisateurId, TypeNotification.NotificationUtilisateur, string.Format(FeatureValidationPointage.VPManager_RemonteVracJobFailed, remonteeVrac.DateDebut.ToLocalTime()));

                throw new FredBusinessException(errorMsg, e);
            }
        }
    }
}
