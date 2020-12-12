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
using Fred.Entities.RapportPrime;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.Business.Hangfire.Parameters;
using Fred.ImportExport.Business.ValidationPointage.Common;
using Fred.ImportExport.Business.ValidationPointage.Ftp.Common;
using Fred.ImportExport.Business.ValidationPointage.Ftp.Common.Interfaces;
using Fred.ImportExport.Business.ValidationPointage.Ftp.ControleVrac.Interfaces;
using Fred.ImportExport.Business.ValidationPointage.Ftp.Mail;
using Fred.Web.Shared.App_LocalResources;
using Hangfire;
using Hangfire.Server;

namespace Fred.ImportExport.Business.ValidationPointage.Ftp.ControleVrac
{
    public class ControleVracFtpHangFireJob : IControleVracFtpHangFireJob
    {
        private readonly IValidationPointageContextDataProvider validationPointageContextDataProvider;
        private readonly ILotPointageManager lotPointageManager;
        private readonly IPointageManager pointageManager;
        private readonly IControleVracFtpQueryBuilder controleVracFtpQueryBuilder;
        private readonly INotificationManager notificationManager;
        private readonly IRapportPrimeLignePrimeManager rapportPrimeLignePrimeManager;
        private readonly IFtpQueryBuilder ftpQueryBuilder;
        private readonly IFtpQueryExecutor ftpQueryExecutor;
        private readonly IControleVracFtpQueryExecutor controleVracFtpQueryExecutor;
        private readonly IControlePointageManager controlePointageManager;

        public ControleVracFtpHangFireJob(
            IValidationPointageContextDataProvider validationPointageContextDataProvider,
            ILotPointageManager lotPointageManager,
            IPointageManager pointageManager,
            IControleVracFtpQueryBuilder controleVracFtpQueryBuilder,
            INotificationManager notificationManager,
            IRapportPrimeLignePrimeManager rapportPrimeLignePrimeManager,
            IFtpQueryBuilder ftpQueryBuilder,
            IFtpQueryExecutor ftpQueryExecutor,
            IControleVracFtpQueryExecutor controleVracFtpQueryExecutor,
            IControlePointageManager controlePointageManager)
        {
            this.validationPointageContextDataProvider = validationPointageContextDataProvider;
            this.lotPointageManager = lotPointageManager;
            this.pointageManager = pointageManager;
            this.controleVracFtpQueryBuilder = controleVracFtpQueryBuilder;
            this.notificationManager = notificationManager;
            this.rapportPrimeLignePrimeManager = rapportPrimeLignePrimeManager;
            this.ftpQueryBuilder = ftpQueryBuilder;
            this.ftpQueryExecutor = ftpQueryExecutor;
            this.controleVracFtpQueryExecutor = controleVracFtpQueryExecutor;
            this.controlePointageManager = controlePointageManager;
        }

        /// <summary>
        ///   Gestion du contrôle vrac
        /// </summary>
        /// <param name="controlePointageEnt">objet contrôle de pointage</param>
        /// <param name="utilisateurId">Identifiant d'utilisateur</param>
        /// <param name="filtre">Filtre</param>
        /// <param name="context">Context hangfire</param>
        /// <remarks>Le jobId hangfire sera utilisé pour que l'appel au programme AS400 de contrôle vrac soit unique. 
        /// Donc l'insertion des pointages et des primes utiliseront également cet identifiant unique "jobId"</remarks>
        /// AutomaticRetry = 1 car la connexion au serveur AS400 est KO par intermittence</remarks>
        [AutomaticRetry(Attempts = 0)]
        [DisplayName("[VALIDATION POINTAGE] Exécution du Contrôle Vrac : ControlePointageId = {0}, LotPointageId = {1}, UtilisateurId : {2}")]
        public async Task ControleVracJob(ControlePointageEnt controlePointageEnt, int utilisateurId, PointageFiltre filtre, PerformContext context)
        {
            var parameter = new ControleVracJobWithEntityParameter { ControlePointageEnt = controlePointageEnt, UtilisateurId = utilisateurId, Filtre = filtre, BackgroundJobId = context.BackgroundJob.Id };

            await JobRunnerApiRestHelper.PostAsync("ControleVracFtpJob", Constantes.CodeGroupeFTP, parameter);
        }

        public async Task ControleVracJobAsync(ControleVracJobWithEntityParameter parameter)
        {
            ControlePointageEnt controlePointageEnt = parameter.ControlePointageEnt;
            int utilisateurId = parameter.UtilisateurId;
            PointageFiltre filtre = parameter.Filtre;
            string backgroundJobId = parameter.BackgroundJobId;

            LotPointageEnt lotPointage = await lotPointageManager.FindByIdNotTrackedAsync(controlePointageEnt.LotPointageId);
            // contrainte ANAEL : le controle doit se faire sur toutes les lignes de rapport du périmètre (société ou etablissement)
            // remplacement des ligne de rapport du lot de validation par toutes les lignes de rapport filtrées sur la période et la société/etablissements paie
            // Cela permet d'éviter les erreurs les personnels du perimetre societe/etablissement associés à un autre lot de pointage
            lotPointage.RapportLignes = pointageManager.GetAllLockedPointagesForSocieteOrEtablissement(lotPointage.Periode, filtre.SocieteId, filtre.EtablissementPaieIdList).ToList();

            ValidationPointageContextData globalData = validationPointageContextDataProvider.GetGlobalData(utilisateurId, filtre.SocieteId, backgroundJobId);
            try
            {
                var filter = new ValidationPointageFtpFilter();

                // Application du filtre sur les lignes de pointages du lot
                lotPointage.RapportLignes = filter.ApplyRapportLigneFilter(lotPointage.RapportLignes, filtre).ToList();

                // Application du filtre sur les rapportLignePrimes
                lotPointage.RapportLignes = filter.ApplyRapportLignePrimesFilter(lotPointage.RapportLignes).ToList();

                RvgPointagesAndPrimes rvgPointagesAndPrimes = ftpQueryBuilder.GetRvgPointagesAndPrimes(lotPointage.Periode, filtre);

                List<RapportPrimeLignePrimeEnt> listPrimeMensuelle = rapportPrimeLignePrimeManager.GetRapportPrimeLignePrime(controlePointageEnt.DateDebut, false);

                string query = controleVracFtpQueryBuilder.BuildControleVracQuery(globalData, lotPointage.Periode, filtre);

                // Envoie des mails
                ValidationPointageMailSenderFtp.Send(globalData, lotPointage.RapportLignes, listPrimeMensuelle, rvgPointagesAndPrimes, lotPointage.Periode, ftpQueryBuilder, query);

                // Insertion des pointages et des primes dans AS400
                ftpQueryExecutor.InsertPointageAndPrime(globalData, lotPointage.Periode, lotPointage.RapportLignes, rvgPointagesAndPrimes, controlePointageEnt);

                controleVracFtpQueryExecutor.ExecuteControleVrac(globalData, controlePointageEnt, query);

                // Mise à jour du statut du contrôle pointage
                controlePointageManager.UpdateControlePointage(controlePointageEnt, FluxStatus.Done.ToIntValue());
                await notificationManager.CreateNotificationAsync(utilisateurId, TypeNotification.NotificationUtilisateur, string.Format(FeatureValidationPointage.VPManager_ControleVracJobSuccess, controlePointageEnt.DateDebut.ToLocalTime()));
            }
            catch (Exception e)
            {
                const string errorMsg = "Erreur lors de l'exécution du contrôle vrac.";
                NLog.LogManager.GetCurrentClassLogger().Error(e, errorMsg);

                controlePointageManager.UpdateControlePointage(controlePointageEnt, FluxStatus.Failed.ToIntValue());
                await notificationManager.CreateNotificationAsync(utilisateurId, TypeNotification.NotificationUtilisateur, string.Format(FeatureValidationPointage.VPManager_ControleVracJobFailed, controlePointageEnt.DateDebut.ToLocalTime()));

                throw new FredBusinessException(errorMsg, e);
            }
        }
    }
}
