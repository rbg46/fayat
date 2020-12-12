using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.EtatPaie;
using Fred.Business.Notification;
using Fred.Business.Rapport.Pointage;
using Fred.Business.ValidationPointage;
using Fred.Entities;
using Fred.Entities.Notification;
using Fred.Entities.Rapport;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.Business.Hangfire.Parameters;
using Fred.ImportExport.Business.ValidationPointage.Common;
using Fred.ImportExport.Business.ValidationPointage.Fes.Common;
using Fred.ImportExport.Business.ValidationPointage.Fes.Logs;
using Fred.ImportExport.Business.ValidationPointage.Fes.Mail;
using Fred.ImportExport.Business.ValidationPointage.Fes.RemonteeVrac.Interfaces;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.ValidationPointage;
using Hangfire;
using Hangfire.Server;

namespace Fred.ImportExport.Business.ValidationPointage.Fes.RemonteeVrac
{
    public class RemonteeVracFesHangFireJob : IRemonteeVracFesHangFireJob
    {
        private readonly IRemonteeVracManager remonteeVracManager;
        private readonly IPointageManager pointageManager;
        private readonly IEtatPaieManager etatPaieManager;
        private readonly INotificationManager notificationManager;
        private readonly IFesQueryBuilder fesQueryBuilder;
        private readonly FesQueryExecutor fesQueryExecutor;
        private readonly IRemonteeVracFesQueryBuilder remonteeVracAnaelQueryBuilder;
        private readonly IRemonteeVracFesQueryExecutor remonteeVracAnaelQueryExecutor;
        private readonly IValidationPointageContextDataProvider validationPointageContextDataProvider;
        private readonly ValidationPointageFesLogger logger;

        private readonly Dictionary<string, string> recipients = new Dictionary<string, string>
        {
            { "Aziz AZIZI", "a.azizi@fayatit.fayat.com" },
            { "Niels Benichou", "n.benichou@fayatit.fayat.com" },
            { "Franck FONTAINE", "f.Fontaine@fayatit.fayat.com" },
            { "Nicolas CALLOIX", "n.calloix@fayatit.fayat.com" }
        };

        public RemonteeVracFesHangFireJob(
            IRemonteeVracManager remonteeVracManager,
            IPointageManager pointageManager,
            IEtatPaieManager etatPaieManager,
            INotificationManager notificationManager,
            IFesQueryBuilder fesQueryBuilder,
            FesQueryExecutor fesQueryExecutor,
            IRemonteeVracFesQueryBuilder remonteeVracAnaelQueryBuilder,
            IRemonteeVracFesQueryExecutor remonteeVracAnaelQueryExecutor,
            IValidationPointageContextDataProvider validationPointageContextDataProvider,
            ValidationPointageFesLogger logger)
        {
            this.remonteeVracManager = remonteeVracManager;
            this.pointageManager = pointageManager;
            this.etatPaieManager = etatPaieManager;
            this.notificationManager = notificationManager;
            this.fesQueryBuilder = fesQueryBuilder;
            this.fesQueryExecutor = fesQueryExecutor;
            this.remonteeVracAnaelQueryBuilder = remonteeVracAnaelQueryBuilder;
            this.remonteeVracAnaelQueryExecutor = remonteeVracAnaelQueryExecutor;
            this.validationPointageContextDataProvider = validationPointageContextDataProvider;
            this.logger = logger;
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
        /// AutomaticRetry = 0 car sinon on execute plusieurs fois le job</remarks>
        [AutomaticRetry(Attempts = 0)]
        [DisplayName("[REMONTEE VRAC FES] Exécution de la Remontée Vrac : RemonteeVracId = {0}, UtilisateurId = {2}")]
        public async Task RemonteeVracJob(int remonteeVracId, DateTime periode, int utilisateurId, PointageFiltre filtre, PerformContext context)
        {
            var parameter = new RemonteeVracJobParameter { RemonteeVracId = remonteeVracId, Periode = periode, UtilisateurId = utilisateurId, Filtre = filtre, BackgroundJobId = context.BackgroundJob.Id };

            await JobRunnerApiRestHelper.PostAsync("RemonteeVracFesJob", Constantes.CodeGroupeFES, parameter);
        }

        public async Task RemonteeVracJobAsync(RemonteeVracJobParameter parameter)
        {
            int remonteeVracId = parameter.RemonteeVracId;
            DateTime periode = parameter.Periode;
            int utilisateurId = parameter.UtilisateurId;
            PointageFiltre filtre = parameter.Filtre;
            string backgroundJobId = parameter.BackgroundJobId;

            ValidationPointageContextData globalData = this.validationPointageContextDataProvider.GetGlobalData(utilisateurId, filtre.SocieteId, backgroundJobId);

            RemonteeVracEnt remonteeVrac = this.remonteeVracManager.Get(remonteeVracId);

            try
            {
                // Récupération et application du filtre sur les pointages
                List<RapportLigneEnt> pointages = (await pointageManager.GetPointagesAsync(periode)).ToList();

                var filter = new ValidationPointageFesFilter();

                // Application du filtre sur les lignes de pointages du lot
                pointages = filter.ApplyRapportLigneFilter(pointages, filtre).ToList();

                pointages = filter.ApplyRapportLigneVerrouilleFilter(pointages).ToList();

                filter.ApplyRapportLignePrimesFilter(pointages);

                string remonteeVracQuery = remonteeVracAnaelQueryBuilder.BuildRemonteeVracQuery(globalData, periode, filtre);

                IEnumerable<QueryInfo> insertsQueries = fesQueryBuilder.BuildAnaelInsertsQueries(globalData, periode, pointages, out List<InsertQueryPrimeParametersModel> listPrimeParameters, out List<InsertQueryPointageParametersModel> listPointageParameters);

                // Export Excel par Mail
                string pathPointageFile = string.Empty;
                string pathPrimeFile = string.Empty;
                if (listPointageParameters != null && listPointageParameters.Count > 0)
                {
                    pathPointageFile = etatPaieManager.BuildInsertQueryPointageParameters(listPointageParameters);
                }

                if (listPrimeParameters != null && listPrimeParameters.Count > 0)
                {
                    pathPrimeFile = etatPaieManager.BuildInsertQueryPrimeParameters(listPrimeParameters);
                }

                if (!string.IsNullOrEmpty(pathPointageFile) || !string.IsNullOrEmpty(pathPrimeFile))
                {
                    ValidationPointageMailSender.SendInsertQuery(globalData, remonteeVracQuery, pathPointageFile, pathPrimeFile, recipients);
                }

                // Envoie des mails
                ValidationPointageMailSender.Send(globalData, pointages, remonteeVracQuery, insertsQueries, recipients);

                IEnumerable<QueryInfo> queries = insertsQueries.Where(q => !q.IsComment);

                // Insertion des pointages et des primes dans AS400
                fesQueryExecutor.ExecuteAnaelInserts(globalData, queries, remonteeVrac);

                remonteeVracAnaelQueryExecutor.ExectuteRemonteeVrac(globalData, remonteeVrac, remonteeVracQuery);

                // Mise à jour du statut du contrôle pointage
                remonteeVrac.DateFin = DateTime.UtcNow;

                this.remonteeVracManager.UpdateRemonteeVrac(remonteeVrac, FluxStatus.Done.ToIntValue());

                await notificationManager.CreateNotificationAsync(utilisateurId, TypeNotification.NotificationUtilisateur, string.Format(FeatureValidationPointage.VPManager_RemonteVracJobSuccess, remonteeVrac.DateDebut.ToLocalTime()));
            }
            catch (Exception e)
            {
                string errorMsg = this.logger.LogRemonteeVracGlobalError(e);

                remonteeVrac.DateFin = DateTime.UtcNow;

                this.remonteeVracManager.UpdateRemonteeVrac(remonteeVrac, FluxStatus.Failed.ToIntValue());

                await notificationManager.CreateNotificationAsync(utilisateurId, TypeNotification.NotificationUtilisateur, string.Format(FeatureValidationPointage.VPManager_RemonteVracJobFailed, remonteeVrac.DateDebut.ToLocalTime()));

                throw new FredBusinessException(errorMsg, e);
            }
        }
    }
}
