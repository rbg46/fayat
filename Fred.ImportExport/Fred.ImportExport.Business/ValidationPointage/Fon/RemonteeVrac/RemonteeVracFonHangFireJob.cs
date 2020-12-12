using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
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
using Fred.ImportExport.Business.ValidationPointage.Fon.Common;
using Fred.ImportExport.Business.ValidationPointage.Fon.Logs;
using Fred.ImportExport.Business.ValidationPointage.Fon.Mail;
using Fred.ImportExport.Business.ValidationPointage.Fon.RemonteeVrac.Interfaces;
using Fred.Web.Shared.App_LocalResources;
using Hangfire;
using Hangfire.Server;

namespace Fred.ImportExport.Business.ValidationPointage.Fon.RemonteeVrac
{
    public class RemonteeVracFonHangFireJob : IRemonteeVracFonHangFireJob
    {
        private readonly IRemonteeVracManager remonteeVracManager;
        private readonly IPointageManager pointageManager;
        private readonly INotificationManager notificationManager;
        private readonly IFonQueryBuilder fonQueryBuilder;
        private readonly IFonQueryExecutor fonQueryExecutor;
        private readonly IRemonteeVracFonQueryBuilder remonteeVracAnaelQueryBuilder;
        private readonly IRemonteeVracFonQueryExecutor remonteeVracAnaelQueryExecutor;
        private readonly IValidationPointageContextDataProvider validationPointageContextDataProvider;
        private readonly ValidationPointageFonLogger logger;

        private readonly Dictionary<string, string> recipients = new Dictionary<string, string>
        {
            { "Nicolas CALLOIX", "n.calloix@fayatit.fayat.com" },
            { "Aziz AZIZI", "a.azizi@fayatit.fayat.com" },
            { "Franck FONTAINE", "f.Fontaine@fayatit.fayat.com" }
        };

        public RemonteeVracFonHangFireJob(
            IRemonteeVracManager remonteeVracManager,
            IPointageManager pointageManager,
            INotificationManager notificationManager,
            IFonQueryBuilder fonQueryBuilder,
            IFonQueryExecutor fonQueryExecutor,
            IRemonteeVracFonQueryBuilder remonteeVracAnaelQueryBuilder,
            IRemonteeVracFonQueryExecutor remonteeVracAnaelQueryExecutor,
            IValidationPointageContextDataProvider validationPointageContextDataProvider,
            ValidationPointageFonLogger logger)
        {
            this.remonteeVracManager = remonteeVracManager;
            this.pointageManager = pointageManager;
            this.notificationManager = notificationManager;
            this.fonQueryBuilder = fonQueryBuilder;
            this.fonQueryExecutor = fonQueryExecutor;
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
        [DisplayName("[REMONTEE VRAC FON] Exécution de la Remontée Vrac : RemonteeVracId = {0}, UtilisateurId = {2}")]
        public async Task RemonteeVracJob(int remonteeVracId, DateTime periode, int utilisateurId, PointageFiltre filtre, PerformContext context)
        {
            var parameter = new RemonteeVracJobParameter { RemonteeVracId = remonteeVracId, Periode = periode, UtilisateurId = utilisateurId, Filtre = filtre, BackgroundJobId = context.BackgroundJob.Id };

            await JobRunnerApiRestHelper.PostAsync("RemonteeVracFonJob", Constantes.CodeGroupeFON, parameter);
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
                // 1 ***** Récupération des pointages
                List<RapportLigneEnt> pointages = (await pointageManager.GetPointagesAsync(periode))
                                    .Where(x => x.Rapport.RapportStatutId == RapportStatutEnt.RapportStatutVerrouille.Key).ToList();

                List<RapportLignePrimeEnt> listprime = new List<RapportLignePrimeEnt>();
                foreach (RapportLigneEnt item in pointages)
                {
                    listprime.AddRange(item.ListRapportLignePrimes);
                }

                // 1.1 **** Récupération des pointages verrouillés
                DateTime previousPeriode = periode.AddMonths(-1);
                IEnumerable<RapportLigneEnt> pointagesVerrouillesPreviousPeriode = (await pointageManager.GetPointagesAsync(previousPeriode))
                        .Where(x => x.Rapport.RapportStatutId == RapportStatutEnt.RapportStatutVerrouille.Key || x.IsGenerated);

                //création objet filter
                var filter = new ValidationPointageFonFilter();

                // 2 ***** Application du filtre sur les lignes de pointages du lot
                pointages = filter.ApplyRapportLigneFilter(pointages, filtre).ToList();

                //2.1 ***** Application du filtre sur des lignes de pointage verrouillés
                pointagesVerrouillesPreviousPeriode = filter.ApplyRapportLigneFilter(pointagesVerrouillesPreviousPeriode, filtre).ToList();

                // liste des pointages verrouillés
                List<RapportLigneEnt> pointagesVerrouilles = pointages.Where(x => x.Rapport.RapportStatutId == RapportStatutEnt.RapportStatutVerrouille.Key).ToList();

                // liste des pointages générés un samedi non verrouillés qui correspondent à un pointage verrouillé du vendredi ou du jeudi précédent (si vendredi ferié)
                // controle sur la période courante et la période précédente pour gérer le cas du samedi CP qui tomberait le 1er du mois.
                List<RapportLigneEnt> pointagesGeneres = pointages.Where(x => x.IsGenerated
                                                     && x.DatePointage.DayOfWeek == DayOfWeek.Saturday
                                                     && x.Rapport.RapportStatutId != RapportStatutEnt.RapportStatutVerrouille.Key
                                                     && ((pointagesVerrouilles.Any(v => v.DatePointage.Date == x.DatePointage.AddDays(-1).Date && v.PersonnelId == x.PersonnelId))
                                                     || (pointagesVerrouilles.Any(v => v.DatePointage.Date == x.DatePointage.AddDays(-2).Date && v.PersonnelId == x.PersonnelId))
                                                     || (pointagesVerrouillesPreviousPeriode.Any(v => v.DatePointage.Date == x.DatePointage.AddDays(-1).Date && v.PersonnelId == x.PersonnelId))
                                                     || (pointagesVerrouillesPreviousPeriode.Any(v => v.DatePointage.Date == x.DatePointage.AddDays(-2).Date && v.PersonnelId == x.PersonnelId)))).ToList();


                pointages = (pointagesVerrouilles.Concat(pointagesGeneres)).ToList();

                // Application du filtre à des rapportLignePrimes
                pointages = filter.ApplyRapportLignePrimesFilter(pointages).ToList();


                // 3 ***** Construction des requêtes SQL
                string remonteeVracQuery = remonteeVracAnaelQueryBuilder.BuildRemonteeVracQuery(globalData, periode, filtre);

                IEnumerable<QueryInfo> insertsQueries = fonQueryBuilder.BuildAnaelInsertsQueries(globalData, periode, listprime, pointages);

                // 4 ***** Envoie des mails
                ValidationPointageMailSender.Send(globalData, pointages, remonteeVracQuery, insertsQueries, recipients);

                IEnumerable<QueryInfo> queries = insertsQueries.Where(q => !q.IsComment);

                // 5 ***** Insertion des pointages et des primes dans AS400
                fonQueryExecutor.ExecuteAnaelInserts(globalData, queries, remonteeVrac);

                // 6 ***** exécution de la remontée vrac
                remonteeVracAnaelQueryExecutor.ExectuteRemonteeVrac(globalData, remonteeVrac, remonteeVracQuery);

                // 7 ***** Mise à jour du statut du contrôle pointage
                remonteeVrac.DateFin = DateTime.UtcNow;

                remonteeVracManager.UpdateRemonteeVrac(remonteeVrac, FluxStatus.Done.ToIntValue());

                // 8 ***** Notification envoyé a FredWeb
                await notificationManager.CreateNotificationAsync(utilisateurId, TypeNotification.NotificationUtilisateur, string.Format(FeatureValidationPointage.VPManager_RemonteVracJobSuccess, remonteeVrac.DateDebut.ToLocalTime()));
            }
            catch (Exception e)
            {
                string errorMsg = logger.LogRemonteeVracGlobalError(e);

                remonteeVrac.DateFin = DateTime.UtcNow;

                remonteeVracManager.UpdateRemonteeVrac(remonteeVrac, FluxStatus.Failed.ToIntValue());

                await notificationManager.CreateNotificationAsync(utilisateurId, TypeNotification.NotificationUtilisateur, string.Format(FeatureValidationPointage.VPManager_RemonteVracJobFailed, remonteeVrac.DateDebut.ToLocalTime()));

                throw new FredBusinessException(errorMsg, e);
            }
        }
    }
}
