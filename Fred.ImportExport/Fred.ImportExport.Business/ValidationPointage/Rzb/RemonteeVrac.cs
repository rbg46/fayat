using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Fred.Business.FonctionnaliteDesactive;
using Fred.Business.Notification;
using Fred.Business.Personnel;
using Fred.Business.Rapport.Pointage;
using Fred.Business.RapportPrime;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.Business.ValidationPointage;
using Fred.Entities;
using Fred.Entities.Notification;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.RapportPrime;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.Framework.Tool;
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
    public class RemonteeVrac
    {
        private const string CallRemonteeVracQuery = "CALL INTRB.SVRFRE01C ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')";
        private readonly string sqlScriptPath = "ValidationPointage.Rzb.SELECT_ERREUR_REMONTEE_VRAC.sql";

        private readonly ControleHelper controleHelper;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IRemonteeVracManager remonteeVracManager;
        private readonly IEtablissementPaieManager etablissementPaieManager;
        private readonly IPersonnelManager personnelManager;
        private readonly ISocieteManager societeManager;
        private readonly IPointageManager pointageManager;
        private readonly INotificationManager notificationManager;
        private readonly IRapportPrimeLignePrimeManager rapportPrimeLignePrimeManager;
        private readonly IFluxManager fluxManager;
        private readonly IFonctionnaliteDesactiveManager fonctionnaliteDesactiveManager;
        private readonly string code = ConfigurationManager.AppSettings["flux:validationPointage"];
        private readonly string chaineConnexionAnael = ConfigurationManager.AppSettings["ANAEL:ConnectionString:Groupe:GRZB"];

        private const string AllMatricule = "******";
        private readonly char spacePadChar = ' ';
        private readonly int etablissementCodeLenght = 10;
        private readonly int jobIdLenght = 10;

        private readonly Dictionary<string, string> recipients = new Dictionary<string, string>
            {
                { "Niels BENICHOU", "n.benichou@fayatit.fayat.com" },
                { "Carine MARTAGEIX", "c.martageix@fayatit.fayat.com" },
                { "Nicolas CALLOIX", "n.calloix@fayatit.fayat.com" }
            };

        public RemonteeVrac(ControleHelper controleHelper,
            IUtilisateurManager utilisateurManager,
            IRemonteeVracManager remonteeVracManager,
            IEtablissementPaieManager etablissementPaieManager,
            IPersonnelManager personnelManager,
            ISocieteManager societeManager,
            IPointageManager pointageManager,
            INotificationManager notificationManager,
            IRapportPrimeLignePrimeManager rapportPrimeLignePrimeManager,
            IFluxManager fluxManager,
            IFonctionnaliteDesactiveManager fonctionnaliteDesactiveManager)
        {
            this.controleHelper = controleHelper;
            this.utilisateurManager = utilisateurManager;
            this.remonteeVracManager = remonteeVracManager;
            this.etablissementPaieManager = etablissementPaieManager;
            this.personnelManager = personnelManager;
            this.societeManager = societeManager;
            this.pointageManager = pointageManager;
            this.notificationManager = notificationManager;
            this.rapportPrimeLignePrimeManager = rapportPrimeLignePrimeManager;
            this.fluxManager = fluxManager;
            this.fonctionnaliteDesactiveManager = fonctionnaliteDesactiveManager;
        }

        public RemonteeVracResult Execute(int utilisateurId, DateTime periode, PointageFiltre filtre)
        {
            RemonteeVracEnt remonteeVrac = CreateRemonteeVrac(utilisateurId, periode);
            FluxEnt flux = fluxManager.GetByCode(code);

            if (flux == null)
            {
                var exception = new FredBusinessException("Ce flux : " + code + " n'a pas été trouvé en base de données.");
                NLog.LogManager.GetCurrentClassLogger().Error(exception, "Flux inexistant.");
                throw exception;
            }

            if (flux.IsActif)
            {
                BackgroundJob.Enqueue(() => RemonteeVracJob(remonteeVrac.RemonteeVracId, periode, utilisateurId, filtre, null));
            }
            else
            {
                remonteeVrac.Statut = FluxStatus.Refused.ToIntValue();
                remonteeVrac.DateFin = DateTime.UtcNow;
                remonteeVracManager.UpdateRemonteeVrac(remonteeVrac);

                var exception = new FredBusinessException("Ce flux : " + code + " n'est pas activé.");
                NLog.LogManager.GetCurrentClassLogger().Error(exception, "Flux inactif.");
                throw exception;
            }

            return new RemonteeVracResult
            {
                DateDebut = DateTime.SpecifyKind(remonteeVrac.DateDebut, DateTimeKind.Utc),
                DateFin = (remonteeVrac.DateFin.HasValue) ? DateTime.SpecifyKind(remonteeVrac.DateFin.Value, DateTimeKind.Utc) : default(DateTime?),
                Periode = DateTime.SpecifyKind(remonteeVrac.Periode, DateTimeKind.Utc),
                Statut = remonteeVrac.Statut,
                RemonteeVracId = remonteeVrac.RemonteeVracId,
                AuteurCreationPrenomNom = remonteeVrac.AuteurCreation.PrenomNom
            };
        }

        [AutomaticRetry(Attempts = 0)]
        [DisplayName("[VALIDATION POINTAGE] Exécution de la Remontée Vrac : RemonteeVracId = {0}, UtilisateurId = {2}")]
        public async Task RemonteeVracJob(int remonteeVracId, DateTime periode, int utilisateurId, PointageFiltre filtre, PerformContext context)
        {
            var parameter = new RemonteeVracJobParameter { RemonteeVracId = remonteeVracId, Periode = periode, UtilisateurId = utilisateurId, Filtre = filtre, BackgroundJobId = context.BackgroundJob.Id };

            await JobRunnerApiRestHelper.PostAsync("RemonteeVracRzbJob", Constantes.CodeGroupeRZB, parameter);
        }

        public async Task RemonteeVracJobAsync(RemonteeVracJobParameter parameter)
        {
            int remonteeVracId = parameter.RemonteeVracId;
            DateTime periode = parameter.Periode;
            int utilisateurId = parameter.UtilisateurId;
            PointageFiltre filtre = parameter.Filtre;
            string backgroundJobId = parameter.BackgroundJobId;

            RemonteeVracEnt remonteeVrac = remonteeVracManager.Get(remonteeVracId);
            UtilisateurEnt utilisateur = utilisateurManager.GetById(utilisateurId);
            string nomUtilisateur = utilisateur?.Personnel?.Nom.Replace(" ", string.Empty).Replace("'", string.Empty).Replace("-", string.Empty).ToUpper();
            SocieteEnt societe = societeManager.GetSocieteById(filtre.SocieteId);
            string queryRemonteeVrac = BuildRemonteeVracQuery(backgroundJobId, periode, nomUtilisateur, filtre, societe.CodeSocietePaye);

            try
            {
                // Récupération et application du filtre sur les pointages verrouillés uniquement
                IEnumerable<RapportLigneEnt> pointages = (await pointageManager.GetPointagesAsync(periode))
                        .Where(x => x.Rapport.RapportStatutId == RapportStatutEnt.RapportStatutVerrouille.Key || x.IsGenerated);

                pointages = controleHelper.ApplyRapportLigneFilter(pointages, filtre).ToList();

                // Pointages de la période précédente uniquement pour les pointages générés
                DateTime previousPeriode = periode.AddMonths(-1);
                // Récupération et application du filtre sur les pointages verrouillés uniquement
                IEnumerable<RapportLigneEnt> pointagesVerrouillesPreviousPeriode = (await pointageManager.GetPointagesAsync(previousPeriode))
                        .Where(x => x.Rapport.RapportStatutId == RapportStatutEnt.RapportStatutVerrouille.Key || x.IsGenerated);

                pointagesVerrouillesPreviousPeriode = controleHelper.ApplyRapportLigneFilter(pointagesVerrouillesPreviousPeriode, filtre).ToList();

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

                pointages = pointagesVerrouilles.Concat(pointagesGeneres);

                // Application du filtre sur les rapportLignePrimes
                pointages.ForEach(x => x.ListRapportLignePrimes = controleHelper.ApplyRapportLignePrimeFilter(x.ListRapportLignePrimes).ToList());

                List<RapportPrimeLignePrimeEnt> listPrimeMensuelle = new List<RapportPrimeLignePrimeEnt>();
                if (!fonctionnaliteDesactiveManager.IsFonctionnaliteDesactiveForSociete(FonctionnaliteCodeConstantes.ExecuterRemontéePrimesMensuellesEnPaye, filtre.SocieteId))
                {
                    listPrimeMensuelle = rapportPrimeLignePrimeManager.GetRapportPrimeLignePrime(periode, false)
                                                                                                      .Where(q => q.RapportPrimeLigne.Personnel.SocieteId == filtre.SocieteId)
                                                                                                      .ToList();
                }

                // Envoie des mails
                VracMail.Send(pointages, listPrimeMensuelle, null, backgroundJobId, remonteeVrac.Periode, nomUtilisateur, filtre.SocieteId, controleHelper, queryRemonteeVrac, recipients, societe.CodeSocietePaye);

                // Insertion des pointages et des primes dans AS400
                controleHelper.InsertPointageAndPrime(backgroundJobId, periode, nomUtilisateur, pointages, null, remonteeVrac, societe.CodeSocietePaye);

                using (DataAccess.Common.Database destinationDatabase = DatabaseFactory.GetNewDatabase(TypeBdd.Db2, chaineConnexionAnael))
                {
                    // Appel de la procédure effectuant la remontée vrac
                    CallRemonteeVracAS400(remonteeVrac, queryRemonteeVrac, destinationDatabase);

                    // Récupération des erreurs et enregistrement dans la base Fred.Database
                    await GetRemonteeVracErreur(remonteeVrac, nomUtilisateur, backgroundJobId, destinationDatabase);
                }

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

        public void CallRemonteeVracAS400(RemonteeVracEnt remonteeVrac, string query, DataAccess.Common.Database destinationDatabase)
        {
            try
            {
                destinationDatabase.ExecuteNonQuery(query);
            }
            catch (Exception e)
            {
                string errorMsg = "Erreur lors de l'appel à la procédure AS400 de la Remontée vrac. Requête : " + query;
                NLog.LogManager.GetCurrentClassLogger().Error(e, errorMsg);
                remonteeVrac.DateFin = DateTime.UtcNow;
                remonteeVracManager.UpdateRemonteeVrac(remonteeVrac, FluxStatus.Failed.ToIntValue());

                throw new FredBusinessException(errorMsg, e);
            }
        }

        public async Task GetRemonteeVracErreur(RemonteeVracEnt remonteeVrac, string nomUtilisateur, string numeroLot, DataAccess.Common.Database destinationDatabase)
        {
            string query = SqlScriptProvider.GetEmbeddedSqlScriptContent(Assembly.GetExecutingAssembly(), sqlScriptPath);
            query = string.Format(query, nomUtilisateur, numeroLot);

            try
            {
                Dictionary<string, int> companyIdsByPayrollCompanyCodes = await societeManager.GetCompanyIdsByPayrollCompanyCodesAsync();
                List<EtablissementPaieEnt> etablissements = etablissementPaieManager.GetEtablissementPaieList().ToList();
                using (IDataReader reader = destinationDatabase.ExecuteReader(query))
                {

                    while (reader.Read())
                    {
                        string matricule = Convert.ToString(reader["Matricule"]);
                        if (!string.IsNullOrEmpty(matricule))
                        {
                            int? societeId = default;

                            string payrollCompanyCode = Convert.ToString(reader["Societe"]);
                            if (companyIdsByPayrollCompanyCodes.ContainsKey(payrollCompanyCode))
                            {
                                societeId = companyIdsByPayrollCompanyCodes[payrollCompanyCode];
                            }

                            List<EtablissementPaieEnt> etablissementsOfSociete = etablissements.Where(e => e.SocieteId == societeId && societeId != null).ToList();
                            int? etablissementPaieId = etablissementsOfSociete.FirstOrDefault(x => x.Code == Convert.ToString(reader["Etablissement"]))?.EtablissementPaieId;

                            InsertRemonteeVracErreur(remonteeVrac, reader, matricule, societeId, etablissementPaieId);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string errorMsg = "Erreur lors de la récupération des erreurs du Contrôle Vrac. Requête : " + query;
                NLog.LogManager.GetCurrentClassLogger().Error(e, errorMsg);
                remonteeVrac.DateFin = DateTime.UtcNow;
                remonteeVracManager.UpdateRemonteeVrac(remonteeVrac, FluxStatus.Failed.ToIntValue());
                throw new FredBusinessException(errorMsg, e);
            }
        }

        private void InsertRemonteeVracErreur(RemonteeVracEnt remonteeVrac, System.Data.IDataReader reader, string matricule, int? societeId, int? etablissementPaieId)
        {
            PersonnelEnt personnel = personnelManager.GetPersonnel(societeId: societeId.Value, matricule: matricule);
            if (personnel != null && societeId.HasValue && etablissementPaieId.HasValue)
            {
                DateTime? dateDebut = reader["DateDebut"] != DBNull.Value ? Convert.ToDateTime(reader["DateDebut"]) : default(DateTime?);
                DateTime? dateFin = reader["DateFin"] != DBNull.Value ? Convert.ToDateTime(reader["DateFin"]) : default(DateTime?);

                var erreur = new RemonteeVracErreurEnt
                {
                    PersonnelId = personnel.PersonnelId,
                    SocieteId = societeId.Value,
                    EtablissementPaieId = etablissementPaieId.Value,
                    DateDebut = dateDebut ?? DateTime.UtcNow,
                    DateFin = dateFin,
                    RemonteeVracId = remonteeVrac.RemonteeVracId,
                    CodeAbsenceFred = Convert.ToString(reader["CodeAbsenceFred"]),
                    CodeAbsenceAnael = Convert.ToString(reader["CodeAbsenceAnael"])
                };

                remonteeVracManager.AddRemonteeVracErreur(erreur);
            }
            else
            {
                string errorMsg = "[ERR-REMONTEVRAC-RZB] Erreur lors de l'insertion des erreurs de la remontée vrac dans fred : matricule " + matricule + " non trouvé .";
                NLog.LogManager.GetCurrentClassLogger().Error(errorMsg);
            }
        }

        public RemonteeVracEnt CreateRemonteeVrac(int utilisateurId, DateTime periode)
        {
            var remonteeVrac = new RemonteeVracEnt
            {
                AuteurCreationId = utilisateurId,
                DateDebut = DateTime.UtcNow,
                Periode = periode,
                Statut = FluxStatus.InProgress.ToIntValue(),
                Erreurs = new List<RemonteeVracErreurEnt>()
            };
            remonteeVracManager.AddRemonteeVrac(remonteeVrac);
            remonteeVrac.AuteurCreation = utilisateurManager.GetById(utilisateurId);

            return remonteeVrac;
        }

        public string BuildRemonteeVracQuery(string jobId, DateTime periode, string nomUtilisateur, PointageFiltre filtre, string codeSocietePaye)
        {
            string etsCodeList = controleHelper.ConcatEtablissementPaieCode(filtre.EtablissementPaieIdList);
            DateTime localPeriode = periode.ToLocalTime();
            nomUtilisateur = nomUtilisateur.Length > 10 ? nomUtilisateur.Substring(0, 10) : nomUtilisateur;

            return string.Format(CallRemonteeVracQuery,
                          codeSocietePaye,
                          localPeriode.Month.ToString("00"),
                          localPeriode.Year,
                          string.IsNullOrEmpty(filtre.Matricule) ? AllMatricule : filtre.Matricule.Trim(),
                          "S",
                          "O", // Remise à blanc (toujours à Oui)
                          filtre.UpdateAbsence ? "O" : "N", // Mise à jour des absences
                          etsCodeList.PadRight(etablissementCodeLenght, spacePadChar),
                          nomUtilisateur.FormatUsername(),
                          jobId.PadLeft(jobIdLenght, spacePadChar));
        }
    }
}
