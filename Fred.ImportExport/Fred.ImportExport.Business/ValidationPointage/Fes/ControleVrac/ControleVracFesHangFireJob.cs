using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Business.EtatPaie;
using Fred.Business.Notification;
using Fred.Business.ValidationPointage;
using Fred.Entities;
using Fred.Entities.Notification;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.Business.Hangfire.Parameters;
using Fred.ImportExport.Business.ValidationPointage.Common;
using Fred.ImportExport.Business.ValidationPointage.Fes.Common;
using Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac.Interfaces;
using Fred.ImportExport.Business.ValidationPointage.Fes.Logs;
using Fred.ImportExport.Business.ValidationPointage.Fes.Mail;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.ValidationPointage;
using Hangfire;
using Hangfire.Server;

namespace Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac
{
    public class ControleVracFesHangFireJob : IControleVracFesHangFireJob
    {
        private readonly ILotPointageManager lotPointageManager;
        private readonly IControlePointageManager controlePointageManager;
        private readonly IEtatPaieManager etatPaieManager;
        private readonly INotificationManager notificationManager;
        private readonly IFesQueryBuilder fesQueryBuilder;
        private readonly IControleVracFesQueryBuilder controleVracFesQueryBuilder;
        private readonly IControleVracFesQueryExecutor controleVracFesQueryExecutor;
        private readonly FesQueryExecutor fesQueryExecutor;
        private readonly IValidationPointageContextDataProvider validationPointageContextDataProvider;
        private readonly ValidationPointageFesLogger logger;

        public ControleVracFesHangFireJob(
            ILotPointageManager lotPointageManager,
            IControlePointageManager controlePointageManager,
            IEtatPaieManager etatPaieManager,
            INotificationManager notificationManager,
            IFesQueryBuilder fesQueryBuilder,
            IControleVracFesQueryBuilder controleVracFesQueryBuilder,
            IControleVracFesQueryExecutor controleVracFesQueryExecutor,
            FesQueryExecutor fesQueryExecutor,
            IValidationPointageContextDataProvider validationPointageContextDataProvider,
            ValidationPointageFesLogger logger)
        {
            this.lotPointageManager = lotPointageManager;
            this.controlePointageManager = controlePointageManager;
            this.etatPaieManager = etatPaieManager;
            this.notificationManager = notificationManager;
            this.fesQueryBuilder = fesQueryBuilder;
            this.controleVracFesQueryBuilder = controleVracFesQueryBuilder;
            this.controleVracFesQueryExecutor = controleVracFesQueryExecutor;
            this.fesQueryExecutor = fesQueryExecutor;
            this.validationPointageContextDataProvider = validationPointageContextDataProvider;
            this.logger = logger;
        }
        private readonly Dictionary<string, string> recipients = new Dictionary<string, string>
            {
                { "Aziz AZIZI", "a.azizi@fayatit.fayat.com" },
                { "Niels Benichou", "n.benichou@fayatit.fayat.com" },
                { "Franck FONTAINE", "f.Fontaine@fayatit.fayat.com" },
                { "Nicolas CALLOIX", "n.calloix@fayatit.fayat.com" }
            };



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
        [DisplayName("[CONTROLE VRAC FES] Exécution du Contrôle Vrac : ControlePointageId = {0}, LotPointageId = {1}, UtilisateurId : {2}")]
        public async Task ControleVracJob(int ctrlPointageId, int lotPointageId, int utilisateurId, PointageFiltre filtre, PerformContext context)
        {
            var parameter = new ControleVracJobParameter { CtrlPointageId = ctrlPointageId, LotPointageId = lotPointageId, UtilisateurId = utilisateurId, Filtre = filtre, BackgroundJobId = context.BackgroundJob.Id };

            await JobRunnerApiRestHelper.PostAsync("ControleVracFesJob", Constantes.CodeGroupeFES, parameter);
        }

        public async Task ControleVracJobAsync(ControleVracJobParameter parameter)
        {
            int utilisateurId = parameter.UtilisateurId;
            ControlePointageEnt ctrlPointage = null;
            try
            {
                int ctrlPointageId = parameter.CtrlPointageId;
                int lotPointageId = parameter.LotPointageId;
                PointageFiltre filtre = parameter.Filtre;
                string backgroundJobId = parameter.BackgroundJobId;

                ctrlPointage = controlePointageManager.Get(ctrlPointageId);

                var includes = new List<Expression<Func<LotPointageEnt, object>>>();

                includes.Include(x => x.RapportLignes.Select(oo => oo.Rapport))
                        .Include(x => x.RapportLignes.Select(oo => oo.Personnel.Societe.Groupe))
                        .Include(x => x.RapportLignes.Select(oo => oo.Ci.CIType))
                        .Include(x => x.RapportLignes.Select(oo => oo.CodeAbsence))
                        .Include(x => x.RapportLignes.Select(oo => oo.CodeMajoration))
                        .Include(x => x.RapportLignes.Select(oo => oo.CodeDeplacement))
                        .Include(x => x.RapportLignes.Select(oo => oo.CodeZoneDeplacement))
                        .Include(x => x.RapportLignes.Select(oo => oo.ListCodePrimeAstreintes.Select(xoo => xoo.CodeAstreinte)))
                        .Include(x => x.RapportLignes.Select(oo => oo.ListRapportLigneAstreintes.Select(xoo => xoo.Astreinte)))
                        .Include(x => x.RapportLignes.Select(oo => oo.ListCodePrimeAstreintes.Select(xoo => xoo.RapportLigneAstreinte)))
                        .Include(x => x.RapportLignes.Select(oo => oo.ListRapportLigneTaches.Select(xoo => xoo.Tache)))
                        .Include(x => x.RapportLignes.Select(oo => oo.ListRapportLignePrimes.Select(xoo => xoo.Prime)))
                        .Include(x => x.RapportLignes.Select(oo => oo.ListRapportLigneMajorations.Select(xoo => xoo.CodeMajoration)));

                LotPointageEnt lotPointage = lotPointageManager.Get(lotPointageId, includes);

                ValidationPointageContextData globalData = validationPointageContextDataProvider.GetGlobalData(utilisateurId, filtre.SocieteId, backgroundJobId);

                var filter = new ValidationPointageFesFilter();

                // Application du filtre sur les lignes de pointages du lot                
                lotPointage.RapportLignes = filter.ApplyRapportLigneFilter(lotPointage.RapportLignes, filtre).ToList();

                filter.ApplyRapportLignePrimesFilter(lotPointage.RapportLignes);

                string controleVracQuery = controleVracFesQueryBuilder.BuildControleVracQuery(globalData, lotPointage.Periode, filtre);

                IEnumerable<QueryInfo> pointagesAndPrimesQueries = fesQueryBuilder.BuildAnaelInsertsQueries(globalData, lotPointage.Periode, lotPointage.RapportLignes, out List<InsertQueryPrimeParametersModel> listPrimeParameters, out List<InsertQueryPointageParametersModel> listPointageParameters);

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
                    ValidationPointageMailSender.SendInsertQuery(globalData, controleVracQuery, pathPointageFile, pathPrimeFile, recipients);
                }

                //Envoie des mails
                ValidationPointageMailSender.Send(globalData, lotPointage.RapportLignes, controleVracQuery, pointagesAndPrimesQueries, recipients);

                //Insertion des pointages et des primes dans AS400
                IEnumerable<QueryInfo> queries = pointagesAndPrimesQueries.Where(q => !q.IsComment);

                fesQueryExecutor.ExecuteAnaelInserts(globalData, queries, ctrlPointage);

                controleVracFesQueryExecutor.ExectuteControleVrac(globalData, ctrlPointage, controleVracQuery);

                // Mise à jour du statut du contrôle pointage
                controlePointageManager.UpdateControlePointage(ctrlPointage, FluxStatus.Done.ToIntValue());

                await notificationManager.CreateNotificationAsync(utilisateurId, TypeNotification.NotificationUtilisateur, string.Format(FeatureValidationPointage.VPManager_ControleVracJobSuccess, ctrlPointage.DateDebut.ToLocalTime()));
            }
            catch (Exception e)
            {
                string errorMsg = logger.LogControleVracGlobalError(e);
                if (ctrlPointage != null)
                {
                    controlePointageManager.UpdateControlePointage(ctrlPointage, FluxStatus.Failed.ToIntValue());
                    await notificationManager.CreateNotificationAsync(utilisateurId, TypeNotification.NotificationUtilisateur, string.Format(FeatureValidationPointage.VPManager_ControleVracJobFailed, ctrlPointage.DateDebut.ToLocalTime()));
                }

                throw new FredBusinessException(errorMsg, e);
            }
        }
    }
}
