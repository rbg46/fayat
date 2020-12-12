using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Business.Notification;
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
using Fred.ImportExport.Business.ValidationPointage.Fon.ControleVrac.Interfaces;
using Fred.ImportExport.Business.ValidationPointage.Fon.Logs;
using Fred.ImportExport.Business.ValidationPointage.Fon.Mail;
using Fred.Web.Shared.App_LocalResources;
using Hangfire;
using Hangfire.Server;

namespace Fred.ImportExport.Business.ValidationPointage.Fon.ControleVrac
{
    public class ControleVracFonHangFireJob : IControleVracFonHangFireJob
    {
        private readonly ILotPointageManager lotPointageManager;
        private readonly IControlePointageManager controlePointageManager;
        private readonly INotificationManager notificationManager;
        private readonly IFonQueryBuilder queryBuilder;
        private readonly IControleVracFonQueryBuilder controleVracAnaelQueryBuilder;
        private readonly IFonQueryExecutor queryExecutor;
        private readonly IControleVracFonQueryExecutor controleVracAnaelQueryExecutor;
        private readonly IValidationPointageContextDataProvider validationPointageContextDataProvider;
        private readonly ValidationPointageFonLogger logger;

        private readonly Dictionary<string, string> recipients = new Dictionary<string, string>
        {
            { "Nicolas CALLOIX", "n.calloix@fayatit.fayat.com" },
            { "Aziz AZIZI", "a.azizi@fayatit.fayat.com" },
            { "Franck FONTAINE", "f.Fontaine@fayatit.fayat.com" }
        };

        public ControleVracFonHangFireJob(
            ILotPointageManager lotPointageManager,
            IControlePointageManager controlePointageManager,
            INotificationManager notificationManager,
            IFonQueryBuilder queryBuilder,
            IControleVracFonQueryBuilder controleVracAnaelQueryBuilder,
            IFonQueryExecutor queryExecutor,
            IControleVracFonQueryExecutor controleVracAnaelQueryExecutor,
            IValidationPointageContextDataProvider validationPointageContextDataProvider,
            ValidationPointageFonLogger logger)
        {
            this.lotPointageManager = lotPointageManager;
            this.controlePointageManager = controlePointageManager;
            this.notificationManager = notificationManager;
            this.queryBuilder = queryBuilder;
            this.controleVracAnaelQueryBuilder = controleVracAnaelQueryBuilder;
            this.queryExecutor = queryExecutor;
            this.controleVracAnaelQueryExecutor = controleVracAnaelQueryExecutor;
            this.validationPointageContextDataProvider = validationPointageContextDataProvider;
            this.logger = logger;
        }

        /// <summary>
        ///   Gestion du contrôle vrac
        /// </summary>
        /// <param name="ctrlPointageId">Identifiant du contrôle de pointage</param>
        /// <param name="lotPointageId">Identifiant du lot de pointage</param>
        /// <param name="utilisateurId">Identifiant d'utilisateur</param>
        /// <param name="filtre">Filtre</param>
        /// <param name="context">Context hangfire</param>
        /// <remarks>Le jobId hangfire sera utilisé pour que l'appel au programme AS400 de contrôle vrac soit unique. 
        /// Donc l'insertion des pointages et des primes utiliseront également cet identifiant unique "jobId"
        /// AutomaticRetry = 1 car la connexion au serveur AS400 est KO par intermittence</remarks>
        [AutomaticRetry(Attempts = 0)]
        [DisplayName("[CONTROLE VRAC FON] Exécution du Contrôle Vrac : ControlePointageId = {0}, LotPointageId = {1}, UtilisateurId : {2}")]
        public async Task ControleVracJob(int ctrlPointageId, int lotPointageId, int utilisateurId, PointageFiltre filtre, PerformContext context)
        {
            var parameter = new ControleVracJobParameter { CtrlPointageId = ctrlPointageId, LotPointageId = lotPointageId, UtilisateurId = utilisateurId, Filtre = filtre, BackgroundJobId = context.BackgroundJob.Id };

            await JobRunnerApiRestHelper.PostAsync("ControleVracFonJob", Constantes.CodeGroupeFON, parameter);
        }

        public async Task ControleVracJobAsync(ControleVracJobParameter parameter)
        {
            int ctrlPointageId = parameter.CtrlPointageId;
            int lotPointageId = parameter.LotPointageId;
            int utilisateurId = parameter.UtilisateurId;
            PointageFiltre filtre = parameter.Filtre;
            string backgroundJobId = parameter.BackgroundJobId;

            ControlePointageEnt ctrlPointage = controlePointageManager.Get(ctrlPointageId);

            var includes = new List<Expression<Func<LotPointageEnt, object>>>();

            includes.Include(x => x.RapportLignes.Select(oo => oo.Rapport))
                    .Include(x => x.RapportLignes.Select(oo => oo.CodeMajoration))
                    .Include(x => x.RapportLignes.Select(oo => oo.Personnel))
                    .Include(x => x.RapportLignes.Select(oo => oo.Ci))
                    .Include(x => x.RapportLignes.Select(oo => oo.ListRapportLignePrimes))
                    .Include(x => x.RapportLignes.Select(oo => oo.ListRapportLignePrimes.Select(zz => zz.Prime)));

            // 1 ***** Récupération des pointages
            LotPointageEnt lotPointage = lotPointageManager.Get(lotPointageId, includes);

            try
            {
                ValidationPointageContextData globalData = validationPointageContextDataProvider.GetGlobalData(utilisateurId, filtre.SocieteId, backgroundJobId);

                var filter = new ValidationPointageFonFilter();

                // 2 ***** Application du filtre sur les lignes de pointages du lot
                lotPointage.RapportLignes = filter.ApplyRapportLigneFilter(lotPointage.RapportLignes, filtre).ToList();

                // Application du filtre sur les rapportLignePrimes
                lotPointage.RapportLignes = filter.ApplyRapportLignePrimesFilter(lotPointage.RapportLignes).ToList();

                // 3 ***** Construction des requêtes SQL
                string controleVracQuery = controleVracAnaelQueryBuilder.BuildControleVracQuery(globalData, lotPointage.Periode, filtre);
                List<RapportLignePrimeEnt> listprime = new List<RapportLignePrimeEnt>();
                foreach (RapportLigneEnt item in lotPointage.RapportLignes)
                {
                    listprime.AddRange(item.ListRapportLignePrimes);
                }
                IEnumerable<QueryInfo> pointagesAndPrimesQueries = queryBuilder.BuildAnaelInsertsQueries(globalData, lotPointage.Periode, listprime, lotPointage.RapportLignes).ToList();

                // 4 ***** Envoie des mails
                ValidationPointageMailSender.Send(globalData, lotPointage.RapportLignes, controleVracQuery, pointagesAndPrimesQueries, recipients);

                // 5 ***** Insertion des pointages et des primes dans AS400
                IEnumerable<QueryInfo> queries = pointagesAndPrimesQueries.Where(q => !q.IsComment);

                queryExecutor.ExecuteAnaelInserts(globalData, queries, ctrlPointage);

                // 6 ***** exécution de la remontée vrac
                controleVracAnaelQueryExecutor.ExectuteControleVrac(globalData, ctrlPointage, controleVracQuery);

                // 7 ***** Mise à jour du statut du contrôle pointage
                controlePointageManager.UpdateControlePointage(ctrlPointage, FluxStatus.Done.ToIntValue());

                // 8 ***** Notification envoyé a FredWeb
                await notificationManager.CreateNotificationAsync(utilisateurId, TypeNotification.NotificationUtilisateur, string.Format(FeatureValidationPointage.VPManager_ControleVracJobSuccess, ctrlPointage.DateDebut.ToLocalTime()));
            }
            catch (Exception e)
            {
                string errorMsg = logger.LogControleVracGlobalError(e);

                controlePointageManager.UpdateControlePointage(ctrlPointage, FluxStatus.Failed.ToIntValue());

                await notificationManager.CreateNotificationAsync(utilisateurId, TypeNotification.NotificationUtilisateur, string.Format(FeatureValidationPointage.VPManager_ControleVracJobFailed, ctrlPointage.DateDebut.ToLocalTime()));

                throw new FredBusinessException(errorMsg, e);
            }
        }
    }
}
