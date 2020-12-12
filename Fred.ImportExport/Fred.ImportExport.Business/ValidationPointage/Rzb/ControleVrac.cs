using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.FonctionnaliteDesactive;
using Fred.Business.Notification;
using Fred.Business.Personnel;
using Fred.Business.Rapport.Pointage;
using Fred.Business.RapportPrime;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.Business.ValidationPointage;
using Fred.Entities;
using Fred.Entities.Notification;
using Fred.Entities.Personnel;
using Fred.Entities.RapportPrime;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.ImportExport.Business.Anael;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.Business.Hangfire.Parameters;
using Fred.ImportExport.DataAccess.Common;
using Fred.ImportExport.Entities.ImportExport;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models;
using Hangfire;
using Hangfire.Server;

namespace Fred.ImportExport.Business.ValidationPointage.Rzb
{
    public class ControleVrac
    {
        private const string CallControleVracQuery = "CALL INTRB.VRAFR29C ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')";
        private const string GetControleVracErreurQuery = "SELECT EDTETB, EDTAFF, EDTMAT, EDTNOM, EDTPRE, EDTAAP, EDTMMP, EDTJJP, EDTERR FROM INTRB.ERR_{0}";

        private readonly IFluxManager fluxManager;
        private readonly ControleHelper controleHelper;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly ILotPointageManager lotPointageManager;
        private readonly IPointageManager pointageManager;
        private readonly IControlePointageManager controlePointageManager;
        private readonly IPersonnelManager personnelManager;
        private readonly INotificationManager notificationManager;
        private readonly IRapportPrimeLignePrimeManager rapportPrimeLignePrimeManager;
        private readonly ISocieteManager societeManager;
        private readonly IFonctionnaliteDesactiveManager fonctionnaliteDesactiveManager;

        private readonly string code = ConfigurationManager.AppSettings["flux:validationPointage"];
        private readonly string chaineConnexionAnael = ConfigurationManager.AppSettings["ANAEL:ConnectionString:Groupe:GRZB"];
        private readonly int codeSocieteComptaLenght = 5;
        private readonly char zeroPadChar = '0';
        private readonly char spacePadChar = ' ';
        private readonly int etablissementCodeLenght = 10;
        private readonly int jobIdLenght = 10;

        private readonly Dictionary<string, string> recipients = new Dictionary<string, string>
            {
                { "Niels BENICHOU", "n.benichou@fayatit.fayat.com" },
                { "Carine MARTAGEIX", "c.martageix@fayatit.fayat.com" },
                { "Nicolas CALLOIX", "n.calloix@fayatit.fayat.com" }
            };

        public ControleVrac(IFluxManager fluxManager,
            ControleHelper controleHelper,
            IUtilisateurManager utilisateurManager,
            ILotPointageManager lotPointageManager,
            IPointageManager pointageManager,
            IControlePointageManager controlePointageManager,
            IPersonnelManager personnelManager,
            INotificationManager notificationManager,
            IRapportPrimeLignePrimeManager rapportPrimeLignePrimeManager,
            ISocieteManager societeManager,
            IFonctionnaliteDesactiveManager fonctionnaliteDesactiveManager)
        {
            this.controleHelper = controleHelper;
            this.utilisateurManager = utilisateurManager;
            this.lotPointageManager = lotPointageManager;
            this.pointageManager = pointageManager;
            this.controlePointageManager = controlePointageManager;
            this.personnelManager = personnelManager;
            this.notificationManager = notificationManager;
            this.rapportPrimeLignePrimeManager = rapportPrimeLignePrimeManager;
            this.societeManager = societeManager;
            this.fluxManager = fluxManager;
            this.fonctionnaliteDesactiveManager = fonctionnaliteDesactiveManager;
        }

        public ControlePointageResult Execute(int utilisateurId, int lotPointageId, PointageFiltre filtre)
        {
            ControlePointageEnt ctrlPointage = CreateControlePointage(utilisateurId, lotPointageId, TypeControlePointage.ControleVrac.ToIntValue());
            FluxEnt flux = fluxManager.GetByCode(code);

            if (flux == null)
            {
                var exception = new FredBusinessException("Ce flux : " + code + " n'a pas été trouvé en base de données.");
                NLog.LogManager.GetCurrentClassLogger().Error(exception, "Flux inexistant.");
                throw exception;
            }

            if (flux.IsActif)
            {
                BackgroundJob.Enqueue(() => ControleVracJob(ctrlPointage.ControlePointageId, lotPointageId, utilisateurId, filtre, null));
            }
            else
            {
                ctrlPointage.Statut = FluxStatus.Refused.ToIntValue();
                controlePointageManager.UpdateControlePointage(ctrlPointage);

                var exception = new FredBusinessException("Ce flux : " + code + " n'est pas activé.");
                NLog.LogManager.GetCurrentClassLogger().Error(exception, "Flux inactif.");
                throw exception;
            }

            return new ControlePointageResult
            {
                DateDebut = DateTime.SpecifyKind(ctrlPointage.DateDebut, DateTimeKind.Utc),
                DateFin = (ctrlPointage.DateFin.HasValue) ? DateTime.SpecifyKind(ctrlPointage.DateFin.Value, DateTimeKind.Utc) : default(DateTime?),
                Statut = ctrlPointage.Statut,
                TypeControle = ctrlPointage.TypeControle,
                LotPointageId = ctrlPointage.LotPointageId,
                AuteurCreationPrenomNom = ctrlPointage.AuteurCreation.PrenomNom
            };
        }

        [AutomaticRetry(Attempts = 0)]
        [DisplayName("[VALIDATION POINTAGE] Exécution du Contrôle Vrac : ControlePointageId = {0}, LotPointageId = {1}, UtilisateurId : {2}")]
        public async Task ControleVracJob(int ctrlPointageId, int lotPointageId, int utilisateurId, PointageFiltre filtre, PerformContext context)
        {
            var parameter = new ControleVracJobParameter { CtrlPointageId = ctrlPointageId, LotPointageId = lotPointageId, UtilisateurId = utilisateurId, Filtre = filtre, BackgroundJobId = context.BackgroundJob.Id };

            await JobRunnerApiRestHelper.PostAsync("ControleVracRzbJob", Constantes.CodeGroupeRZB, parameter);
        }

        public async Task ControleVracJobAsync(ControleVracJobParameter parameter)
        {
            int ctrlPointageId = parameter.CtrlPointageId;
            int lotPointageId = parameter.LotPointageId;
            int utilisateurId = parameter.UtilisateurId;
            PointageFiltre filtre = parameter.Filtre;
            string backgroundJobId = parameter.BackgroundJobId;

            ControlePointageEnt ctrlPointage = controlePointageManager.Get(ctrlPointageId);

            LotPointageEnt lotPointage = await lotPointageManager.FindByIdNotTrackedAsync(lotPointageId);
            // contrainte ANAEL : le controle doit se faire sur toutes les lignes de rapport du périmètre (société ou etablissement)
            // remplacement des ligne de rapport du lot de validation par toutes les lignes de rapport filtrées sur la période et la société/etablissements paie
            // Cela permet d'éviter les erreurs les personnels du perimetre societe/etablissement associés à un autre lot de pointage
            lotPointage.RapportLignes = pointageManager.GetAllLockedPointagesForSocieteOrEtablissement(lotPointage.Periode, filtre.SocieteId, filtre.EtablissementPaieIdList).ToList();

            UtilisateurEnt utilisateur = utilisateurManager.GetById(utilisateurId);
            string nomUtilisateur = utilisateur.Personnel.Nom.ToUpper().Replace(" ", string.Empty).Replace("'", string.Empty).Replace("-", string.Empty).ToUpper();
            SocieteEnt societe = societeManager.GetSocieteById(filtre.SocieteId);
            string query = BuildControleVracQuery(backgroundJobId, lotPointage.Periode, nomUtilisateur, filtre, societe);

            try
            {
                // Application du filtre sur les lignes de pointages du lot
                lotPointage.RapportLignes = controleHelper.ApplyRapportLigneFilter(lotPointage.RapportLignes, filtre).ToList();

                // Application du filtre sur les rapportLignePrimes
                lotPointage.RapportLignes.ForEach(x => x.ListRapportLignePrimes = controleHelper.ApplyRapportLignePrimeFilter(x.ListRapportLignePrimes).ToList());

                RvgPointagesAndPrimes rvgPointagesAndPrimes = controleHelper.GetRvgPointagesAndPrimes(lotPointage.Periode, filtre);

                List<RapportPrimeLignePrimeEnt> listPrimeMensuelle = new List<RapportPrimeLignePrimeEnt>();
                if (!fonctionnaliteDesactiveManager.IsFonctionnaliteDesactiveForSociete(FonctionnaliteCodeConstantes.ExecuterRemontéePrimesMensuellesEnPaye, filtre.SocieteId))
                {
                    listPrimeMensuelle = rapportPrimeLignePrimeManager.GetRapportPrimeLignePrime(lotPointage.Periode, false)
                                                                                                  .Where(q => q.RapportPrimeLigne.Personnel.SocieteId == filtre.SocieteId)
                                                                                                  .ToList();
                }

                VracMail.Send(lotPointage.RapportLignes, listPrimeMensuelle, rvgPointagesAndPrimes, backgroundJobId, lotPointage.Periode, nomUtilisateur, filtre.SocieteId, controleHelper, query, recipients, societe.CodeSocietePaye);

                // Insertion des pointages et des primes dans AS400
                controleHelper.InsertPointageAndPrime(backgroundJobId, lotPointage.Periode, nomUtilisateur, lotPointage.RapportLignes, rvgPointagesAndPrimes, ctrlPointage, societe.CodeSocietePaye);

                using (DataAccess.Common.Database destinationDatabase = DatabaseFactory.GetNewDatabase(TypeBdd.Db2, chaineConnexionAnael))
                {
                    // Appel de la procédure effectuant le contrôle vrac
                    CallControleVracAS400(ctrlPointage, query, destinationDatabase);

                    // Récupération des erreurs et enregistrement dans la base Fred.Database
                    GetControleVracErreur(ctrlPointage, nomUtilisateur, destinationDatabase, societe.CodeSocietePaye);
                }

                // Mise à jour du statut du contrôle pointage
                controlePointageManager.UpdateControlePointage(ctrlPointage, FluxStatus.Done.ToIntValue());
                await notificationManager.CreateNotificationAsync(utilisateurId, TypeNotification.NotificationUtilisateur, string.Format(FeatureValidationPointage.VPManager_ControleVracJobSuccess, ctrlPointage.DateDebut.ToLocalTime()));
            }
            catch (Exception e)
            {
                const string errorMsg = "Erreur lors de l'exécution du contrôle vrac.";
                NLog.LogManager.GetCurrentClassLogger().Error(e, errorMsg);

                controlePointageManager.UpdateControlePointage(ctrlPointage, FluxStatus.Failed.ToIntValue());
                await notificationManager.CreateNotificationAsync(utilisateurId, TypeNotification.NotificationUtilisateur, string.Format(FeatureValidationPointage.VPManager_ControleVracJobFailed, ctrlPointage.DateDebut.ToLocalTime()));

                throw new FredBusinessException(errorMsg, e);
            }
        }

        public void CallControleVracAS400(ControlePointageEnt ctrlPointage, string query, DataAccess.Common.Database destinationDatabase)
        {
            try
            {
                destinationDatabase.ExecuteNonQuery(query);
            }
            catch (Exception e)
            {
                string errorMsg = "Erreur lors de l'appel à la procédure AS400 du Contrôle vrac. Requête : " + query;
                NLog.LogManager.GetCurrentClassLogger().Error(e, errorMsg);
                controlePointageManager.UpdateControlePointage(ctrlPointage, FluxStatus.Failed.ToIntValue());

                throw new FredBusinessException(errorMsg, e);
            }
        }

        public void GetControleVracErreur(ControlePointageEnt ctrlPointage, string nomUtilisateur, DataAccess.Common.Database destinationDatabase, string codeSocietePaye)
        {
            nomUtilisateur = nomUtilisateur.FormatUsername();

            string tableErreurPrefixe = (nomUtilisateur.Length > 6) ? nomUtilisateur.Substring(0, 6) : nomUtilisateur;
            string query = string.Format(GetControleVracErreurQuery, tableErreurPrefixe);

            try
            {
                int day = 0, month = 0, year = 0;
                DateTime? dateRapport;
                List<PersonnelEnt> personnels = personnelManager.GetPersonnelListByCodeSocietePaye(codeSocietePaye).ToList();
                using (IDataReader reader = destinationDatabase.ExecuteReader(query))
                {
                    while (reader.Read())
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(reader["EDTMAT"])))
                        {
                            PersonnelEnt personnel = personnels.FirstOrDefault(p => p.Matricule == Convert.ToString(reader["EDTMAT"]));
                            if (personnel != null)
                            {
                                day = Convert.ToInt32(reader["EDTJJP"]);
                                month = Convert.ToInt32(reader["EDTMMP"]);
                                year = Convert.ToInt32(reader["EDTAAP"]);
                                dateRapport = day > 0 && month > 0 && year > 0 ? new DateTime(year, month, day) : default(DateTime?);

                                var erreur = new ControlePointageErreurEnt
                                {
                                    DateRapport = dateRapport,
                                    Message = Convert.ToString(reader["EDTERR"]).Trim(),
                                    ControlePointageId = ctrlPointage.ControlePointageId,
                                    PersonnelId = personnel.PersonnelId,
                                    CodeCi = Convert.ToString(reader["EDTAFF"]).Trim()
                                };
                                controlePointageManager.AddControlePointageErreur(erreur);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string errorMsg = "Erreur lors de la récupération des erreurs du Contrôle Vrac. Requête : " + query;
                NLog.LogManager.GetCurrentClassLogger().Error(e, errorMsg);
                controlePointageManager.UpdateControlePointage(ctrlPointage, FluxStatus.Failed.ToIntValue());
                throw new FredBusinessException(errorMsg, e);
            }
        }

        public ControlePointageEnt CreateControlePointage(int utilisateurId, int lotPointageId, int typeControle)
        {
            LotPointageEnt lotPointage = lotPointageManager.Get(lotPointageId);
            ControlePointageEnt ctrlPointage = new ControlePointageEnt
            {
                DateDebut = DateTime.UtcNow,
                AuteurCreationId = utilisateurId,
                LotPointageId = lotPointage.LotPointageId,
                Statut = FluxStatus.InProgress.ToIntValue(),
                TypeControle = typeControle,
                Erreurs = new List<ControlePointageErreurEnt>()
            };

            controlePointageManager.AddControlePointage(ctrlPointage);
            ctrlPointage.AuteurCreation = utilisateurManager.GetById(utilisateurId);

            return ctrlPointage;
        }

        public string BuildControleVracQuery(string jobId, DateTime periode, string nomUtilisateur, PointageFiltre filtre, SocieteEnt societe)
        {
            string etsCodeList = controleHelper.ConcatEtablissementPaieCode(filtre.EtablissementPaieIdList);
            periode = periode.ToLocalTime();
            nomUtilisateur = (nomUtilisateur.Length > 10) ? nomUtilisateur.Substring(0, 10) : nomUtilisateur;

            return string.Format(CallControleVracQuery,
                           societe.CodeSocietePaye,
                           periode.Month.ToString("00"),
                           periode.Year,
                           societe.CodeSocieteComptable.PadLeft(codeSocieteComptaLenght, zeroPadChar),
                           etsCodeList.PadRight(etablissementCodeLenght, spacePadChar),
                           nomUtilisateur.FormatUsername(),
                           jobId.PadLeft(jobIdLenght, spacePadChar));
        }
    }
}
