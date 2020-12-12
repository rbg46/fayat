using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Fred.GroupSpecific.Rzb.Societe;
using Fred.ImportExport.Business;
using Fred.ImportExport.Business.BaremeExploitation;
using Fred.ImportExport.Business.ContratInterimaire;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.JournauxComptable;
using Fred.ImportExport.Business.Materiel;
using Fred.ImportExport.Business.Materiel.ImportMaterielFaytTp;
using Fred.ImportExport.Business.Personnel;
using Fred.ImportExport.Business.Personnel.EtlFactory;
using Fred.ImportExport.Business.Stair;
using Fred.ImportExport.Business.Utilisateur;
using Fred.ImportExport.Resources.Resources;
using Fred.ImportExport.Web.Models;
using Hangfire;
using Hangfire.Storage;

namespace Fred.ImportExport.Web.Controllers
{
    public class ImportController : ControllerBase
    {
        private readonly BaremeExploitationFluxManager baremeExploitationFluxManager;
        private readonly CIFluxManager ciFluxManager;
        private readonly FournisseurFluxManager fournisseurFluxManager;
        private readonly EtablissementComptableFluxManager etablissementComptableFluxManager;
        private readonly PersonnelFluxManager personnelFluxManager;
        private readonly PersonnelFluxMultipleSocieteManager personnelFluxMultipleSocieteManager;
        private readonly RzbEcritureComptableFluxManager ecritureComptableFluxManagerRzb;
        private readonly MoulinsEcritureComptableFluxManager ecritureComptableFluxManagerMoulins;
        private readonly IFluxManager fluxMrg;
        private readonly MaterielFluxManager materielFluxMgr;
        private readonly MaterielFluxFayatTpManager materielFaytaTPFluxMgr;
        private readonly StairFluxManager stairFluxManager;
        private readonly SphinxFluxManager sphinxFluxManager;
        private readonly JournauxComptableFluxManagerRzb journauxComptableFluxManagerRzb;
        private readonly JournauxComptableFluxManagerMoulins journauxComptableFluxManagerMoulins;
        private readonly ContratInterimaireFluxManager contratInterimaireFluxManager;
        private readonly CleaningOutgoingUsersFluxManager cleaningOutgoingUsersFluxManager;

        public string ImportContratInterimaireFluxCode { get; } = ConfigurationManager.AppSettings["flux:import:contratInterim:grzb"];
        public string CleaningOutgoingUsersFluxCode { get; } = ConfigurationManager.AppSettings["flux:clean:outgoing:users"];

        public ImportController(
            BaremeExploitationFluxManager baremeExploitationFluxManager,
            CIFluxManager ciFluxManager,
            FournisseurFluxManager fournisseurFluxManager,
            EtablissementComptableFluxManager etablissementComptableFluxManager,
            PersonnelFluxManager personnelFluxManager,
            PersonnelFluxMultipleSocieteManager personnelFluxMultipleSocieteManager,
            RzbEcritureComptableFluxManager ecritureComptableFluxManagerRzb,
            MoulinsEcritureComptableFluxManager ecritureComptableFluxManagerMoulins,
            IFluxManager fluxMrg,
            MaterielFluxManager materielFluxMgr,
            MaterielFluxFayatTpManager materielFaytaTPFluxMgr,
            StairFluxManager stairFluxManager,
            SphinxFluxManager sphinxFluxManager,
            JournauxComptableFluxManagerRzb journauxComptableFluxManagerRzb,
            JournauxComptableFluxManagerMoulins journauxComptableFluxManagerMoulins,
            ContratInterimaireFluxManager contratInterimaireFluxManager,
            CleaningOutgoingUsersFluxManager cleaningOutgoingUsersFluxManager)
        {
            this.baremeExploitationFluxManager = baremeExploitationFluxManager;
            this.ciFluxManager = ciFluxManager;
            this.fournisseurFluxManager = fournisseurFluxManager;
            this.etablissementComptableFluxManager = etablissementComptableFluxManager;
            this.personnelFluxManager = personnelFluxManager;
            this.personnelFluxMultipleSocieteManager = personnelFluxMultipleSocieteManager;
            this.ecritureComptableFluxManagerRzb = ecritureComptableFluxManagerRzb;
            this.ecritureComptableFluxManagerMoulins = ecritureComptableFluxManagerMoulins;
            this.fluxMrg = fluxMrg;
            this.materielFluxMgr = materielFluxMgr;
            this.materielFaytaTPFluxMgr = materielFaytaTPFluxMgr;
            this.stairFluxManager = stairFluxManager;
            this.sphinxFluxManager = sphinxFluxManager;
            this.journauxComptableFluxManagerRzb = journauxComptableFluxManagerRzb;
            this.journauxComptableFluxManagerMoulins = journauxComptableFluxManagerMoulins;
            this.contratInterimaireFluxManager = contratInterimaireFluxManager;
            this.cleaningOutgoingUsersFluxManager = cleaningOutgoingUsersFluxManager;
        }

        /// <summary>
        /// Retourne la vue principale avec le model
        /// </summary>
        /// <returns>Action result</returns>    
        public ActionResult Index()
        {
            ImportViewModel vm = GetImportViewModel();
            return View(vm);
        }

        /// <summary>
        /// Importation des barêmes
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ImportBaremes()
        {
            var vm = new ImportViewModel();
            try
            {
                baremeExploitationFluxManager.ExecuteImport();
                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        #region CI GENERIQUE
        /// <summary>
        /// Importation des CI générique
        /// </summary>
        /// <param name="codeFlux">Code du flux . Exemple : CI_GRZB</param>
        /// <param name="byPassDate">Ignore la dernière date d'exécution</param>
        /// <returns>ActionResult</returns>
        public ActionResult ImportCIGenerique(string codeFlux, bool byPassDate = false)
        {
            var vm = new ImportViewModel();
            try
            {
                ciFluxManager.ExecuteImportByCodeFlux(byPassDate, codeFlux);
                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        /// <summary>
        /// Toggle importation récurrente des CI
        /// </summary>    
        /// <param name="activate">Activer la planification ou la supprimer</param>
        /// <param name="codeFlux">Code du flux . Exemple : CI_GRZB</param>
        /// <param name="cron">cycle définit d'exécution: 0 2 * * * => tous les jours à 2h du matin par défaut</param>
        /// <returns>ActionResult</returns>
        public ActionResult ToggleScheduleImportCIGRZB(bool activate, string codeFlux, string cron = "0 2 * * *")
        {
            var vm = new ImportViewModel();
            try
            {
                ciFluxManager.ToggleScheduleImportByCodeFlux(activate, cron, codeFlux);
                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        #endregion

        #region CI
        /// <summary>
        /// Importation des CI
        /// </summary>
        /// <param name="societeCodeAnael">Code Anael de la société pour laquelle effectuer l'import</param>
        /// <param name="byPassDate">Ignore la dernière date d'exécution</param>
        /// <returns>ActionResult</returns>
        public ActionResult ImportCI(string societeCodeAnael, bool byPassDate = false)
        {
            var vm = new ImportViewModel();
            try
            {
                var codeFlux = ConfigurationManager.AppSettings["flux:ci"];
                ciFluxManager.ExecuteImportByCodeFlux(byPassDate, codeFlux);
                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        /// <summary>
        /// Toggle importation récurrente des CI
        /// </summary>    
        /// <param name="activate">Activer la planification ou la supprimer</param>
        /// <param name="societeCodeAnael">Code Anael de la société pour laquelle effectuer l'import</param>
        /// <param name="cron">cycle définit d'exécution: 0 1 * * * => tous les jours à 1h du matin par défaut</param>
        /// <returns>ActionResult</returns>
        public ActionResult ToggleScheduleImportCI(bool activate, string societeCodeAnael, string cron = "0 1 * * *")
        {
            var vm = new ImportViewModel();
            try
            {
                //Flux Societe RZB
                var codeFlux = ConfigurationManager.AppSettings["flux:ci"];
                ciFluxManager.ToggleScheduleImportByCodeFlux(activate, cron, codeFlux);
                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }

            return View("Index", vm);
        }

        #endregion

        #region Fournisseur
        /// <summary>
        /// Importation des fournisseurs
        /// </summary>
        /// <param name="byPassDate">Ignore la dernière date d'exécution</param>
        /// <param name="isStormOutputDesactivated">Détermine si on active l'export vers STORM ou pas</param>
        /// <returns>ActionResult</returns>
        public ActionResult ImportFournisseur(bool byPassDate = false, bool isStormOutputDesactivated = false)
        {
            var vm = new ImportViewModel();
            try
            {
                const string typeSequences = "'TIERS', 'TIERS2', 'GROUPE'";
                const string regleGestions = "'F'";
                const string societeCodeAnael = "1000"; // Par défaut : société Razel-Bec

                fournisseurFluxManager.ExecuteImport(byPassDate, societeCodeAnael, typeSequences, regleGestions, isStormOutputDesactivated);
                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        /// <summary>
        /// Toggle importation récurrente des fournisseurs
        /// </summary>    
        /// <param name="activate">Activer la planification ou la supprimer</param>
        /// <param name="cron">cycle définit d'exécution: 0 * * * * => toutes les heures par défaut</param>
        /// <param name="isStormOutputDesactivated">Détermine si on active l'export vers STORM ou pas</param>
        /// <returns>ActionResult</returns>
        public ActionResult ToggleScheduleImportFournisseur(bool activate, string cron = "0 * * * *", bool isStormOutputDesactivated = false)
        {
            var vm = new ImportViewModel();
            try
            {
                const string typeSequences = "'TIERS', 'TIERS2', 'GROUPE'";
                const string regleGestions = "'F'";
                const string societeCodeAnael = "1000"; // Par défaut : société Razel-Bec

                if (activate)
                {
                    fournisseurFluxManager.ScheduleImport(cron, societeCodeAnael, typeSequences, regleGestions, isStormOutputDesactivated);
                }
                else
                {
                    RecurringJob.RemoveIfExists(fournisseurFluxManager.ImportJobId);
                }

                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        #endregion

        #region Etablissement comptable
        /// <summary>
        /// Importation des établissements comptables
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ImportEtablissementComptable(string codeFlux)
        {
            var vm = new ImportViewModel();
            try
            {
                etablissementComptableFluxManager.ExecuteImport(codeFlux);
                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }

            return View("Index", vm);
        }

        /// <summary>
        /// Importation des établissements comptables
        /// </summary>
        /// <param name="activate">Activer la planification ou la supprimer</param>
        /// <param name="codeFlux">Code du flux.</param>
        /// <param name="cron">cycle définit d'exécution: 0 0 * * * => tous les jours à minuit par défaut</param>
        /// <returns>ActionResult</returns>
        public ActionResult ToggleScheduleImportEtablissementComptable(bool activate, string codeFlux, string cron = "0 0 * * *")
        {
            var vm = new ImportViewModel();
            try
            {
                if (activate)
                {
                    etablissementComptableFluxManager.ScheduleImportByCodeFlux(cron, codeFlux);
                }
                else
                {
                    RecurringJob.RemoveIfExists(codeFlux);
                }

                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        #endregion

        #region Personnel GENERIQUE

        /// <summary>
        /// Importation des personnels
        /// </summary>
        /// <param name="codeFlux">Code du flux . Exemple : PERSONNEL_FES</param>
        /// <param name="byPassDate">Ignore la dernière date d'exécution</param>
        /// <returns>ActionResult</returns>
        public ActionResult ImportPersonnelGenerique(string codeFlux, bool byPassDate = false)
        {
            var vm = new ImportViewModel();
            try
            {
                personnelFluxManager.ExecuteImportByCodeFlux(byPassDate, codeFlux);
                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        /// <summary>
        /// Toggle importation récurrente des Personnels
        /// </summary>    
        /// <param name="activate">Activer la planification ou la supprimer</param>
        /// <param name="codeFlux">Code du flux . Exemple : PERSONNEL_FES</param>
        /// <param name="cron">cycle définit d'exécution: 0 1 * * * => tous les jours à 1h du matin par défaut</param>
        /// <returns>ActionResult</returns>
        public ActionResult ToggleScheduleImportPersonnelGenerique(bool activate, string codeFlux, string cron = "0 2 * * *")
        {
            var vm = new ImportViewModel();
            try
            {
                if (activate)
                {
                    personnelFluxManager.ScheduleImportByCodeFlux(cron, codeFlux);
                }
                else
                {
                    RecurringJob.RemoveIfExists(codeFlux);
                }

                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        #endregion

        #region Personnel  IMPORT de plusieurs societe ne meme temps

        /// <summary>
        /// Importation des personnels
        /// </summary>
        /// <param name="codeFlux">Code du flux . Exemple : PERSONNEL_FES</param>
        /// <param name="byPassDate">Ignore la dernière date d'exécution</param>
        /// <returns>ActionResult</returns>
        public ActionResult ImportPersonnelMultipleSocieteInSameTime(string codeFlux, bool byPassDate = false)
        {
            var vm = new ImportViewModel();
            try
            {
                personnelFluxMultipleSocieteManager.ExecuteImportAllSocieteInSameTime(byPassDate, codeFlux);

                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        /// <summary>
        /// Toggle importation récurrente des Personnels
        /// </summary>    
        /// <param name="activate">Activer la planification ou la supprimer</param>
        /// <param name="codeFlux">Code du flux . Exemple : PERSONNEL_FES</param>
        /// <param name="cron">cycle définit d'exécution: 0 1 * * * => tous les jours à 1h du matin par défaut</param>
        /// <returns>ActionResult</returns>
        public ActionResult ToggleScheduleImportPersonnelMultipleSocieteInSameTime(bool activate, string codeFlux, string cron = "0 3 * * *")
        {
            var vm = new ImportViewModel();
            try
            {
                if (activate)
                {
                    personnelFluxMultipleSocieteManager.ScheduleImportAllSocieteInSameTime(cron, codeFlux);
                }
                else
                {
                    RecurringJob.RemoveIfExists(codeFlux);// PERSONNEL_FES
                }

                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        #endregion

        #region ECRITURES COMPTABLES
        /// <summary>
        /// Importation des ECRITURES COMPTABLES
        /// </summary>
        /// <param name="societeCodeAnael">Code comptable (application Anael) de la société</param>
        /// <param name="dateComptable">Date comptable des imports</param>
        /// <param name="codeEtablissement">code de l'établissement comptable</param>
        /// <returns>ActionResult</returns>
        public ActionResult ImportEcrituresComptables(string societeCodeAnael, DateTime dateComptable, string codeEtablissement)
        {
            ImportViewModel vm = new ImportViewModel();
            try
            {
                ecritureComptableFluxManagerRzb.ExecuteImportRecurring(societeCodeAnael, dateComptable, codeEtablissement);
                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        /// <summary>
        /// Importation des ECRITURES COMPTABLES
        /// </summary>
        /// <param name="societeCodeAnael">Code comptable (application Anael) de la société</param>
        /// <param name="dateDebutComptable">Date Debut comptable des imports</param>
        /// <param name="dateFinComptable">Date Fin comptable des imports</param>
        /// <param name="codeEtablissement">code de l'établissement comptable</param>
        /// <returns>ActionResult</returns>
        public ActionResult ImportEcrituresComptablesRange(string societeCodeAnael, DateTime dateDebutComptable, DateTime dateFinComptable, string codeEtablissement)
        {
            ImportViewModel vm = new ImportViewModel();
            try
            {
                ecritureComptableFluxManagerRzb.ExecuteImportRecurring(societeCodeAnael, dateDebutComptable, dateFinComptable, codeEtablissement);
                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        public ActionResult ImportPartialEcrituresComptablesRange(string societeCodeAnael, DateTime dateDebutComptable, DateTime dateFinComptable, string codeEtablissement)
        {
            ImportViewModel vm = new ImportViewModel();
            try
            {
                ecritureComptableFluxManagerRzb.ExecutePartialImportRecurring(societeCodeAnael, dateDebutComptable, dateFinComptable, codeEtablissement);
                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }


        /// <summary>
        /// Toggle importation récurrente des ECRITURES COMPTABLES
        /// </summary>
        /// <param name="societeCodeAnael">Code comptable (application Anael) de la société</param>
        /// <param name="activate">Activer la planification ou la supprimer</param>
        /// <param name="cron">cycle définit d'exécution: 0 3 * * * => tous les jours à 1h du matin par défaut</param>
        /// <returns>ActionResult</returns>
        public ActionResult ToggleScheduleImportEcrituresComptables(string societeCodeAnael, bool activate, string cron = "0 3 * * *")
        {
            ImportViewModel vm = new ImportViewModel();
            try
            {
                if (activate)
                {
                    ecritureComptableFluxManagerRzb.ScheduleImportRecurring(cron, societeCodeAnael);
                }
                else
                {
                    RecurringJob.RemoveIfExists(RzbEcritureComptableFluxManager.ImportJobIdRzb);
                }

                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        /// <summary>
        /// Importation des ECRITURES COMPTABLES
        /// </summary>
        /// <param name="societeCodeAnael">Code comptable (application Anael) de la société</param>
        /// <param name="dateComptable">Date comptable des imports</param>
        /// <param name="codeEtablissement">code de l'établissement comptable</param>
        /// <returns>ActionResult</returns>
        public ActionResult ImportEcrituresComptablesMoulins(string societeCodeAnael, DateTime dateComptable, string codeEtablissement)
        {
            ImportViewModel vm = new ImportViewModel();
            try
            {
                ecritureComptableFluxManagerMoulins.ExecuteImportRecurring(societeCodeAnael, dateComptable, codeEtablissement);
                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        /// <summary>
        /// Importation des ECRITURES COMPTABLES
        /// </summary>
        /// <param name="societeCodeAnael">Code comptable (application Anael) de la société</param>
        /// <param name="dateDebutComptable">Date Debut comptable des imports</param>
        /// <param name="dateFinComptable">Date Fin comptable des imports</param>
        /// <param name="codeEtablissement">code de l'établissement comptable</param>
        /// <returns>ActionResult</returns>
        public ActionResult ImportEcrituresComptablesRangeMoulins(string societeCodeAnael, DateTime dateDebutComptable, DateTime dateFinComptable, string codeEtablissement)
        {
            ImportViewModel vm = new ImportViewModel();
            try
            {
                ecritureComptableFluxManagerMoulins.ExecuteImportRecurring(societeCodeAnael, dateDebutComptable, dateFinComptable, codeEtablissement);
                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        public ActionResult ImportPartialEcrituresComptablesRangeMoulins(string societeCodeAnael, DateTime dateDebutComptable, DateTime dateFinComptable, string codeEtablissement)
        {
            ImportViewModel vm = new ImportViewModel();
            try
            {
                ecritureComptableFluxManagerMoulins.ExecutePartialImportRecurring(societeCodeAnael, dateDebutComptable, dateFinComptable, codeEtablissement);
                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        /// <summary>
        /// Toggle importation récurrente des ECRITURES COMPTABLES
        /// </summary>
        /// <param name="societeCodeAnael">Code comptable (application Anael) de la société</param>
        /// <param name="activate">Activer la planification ou la supprimer</param>
        /// <param name="cron">cycle définit d'exécution: 0 3 * * * => tous les jours à 1h du matin par défaut</param>
        /// <returns>ActionResult</returns>
        public ActionResult ToggleScheduleImportEcrituresComptablesMoulins(string societeCodeAnael, bool activate, string cron = "0 3 * * *")
        {
            ImportViewModel vm = new ImportViewModel();
            try
            {
                if (activate)
                {
                    ecritureComptableFluxManagerMoulins.ScheduleImportRecurring(cron, societeCodeAnael);
                }
                else
                {
                    RecurringJob.RemoveIfExists(MoulinsEcritureComptableFluxManager.ImportJobIdMoulins);
                }

                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }
        #endregion

        #region Matériel RZB et MTP

        /// <summary>
        /// Importation des matériels depuis le 1er janvier 1900
        /// </summary>
        /// <param name="isFull">Active a une sunchronisation full.</param>
        /// <returns>ActionResult</returns>
        public ActionResult ImportMateriel(bool isFull)
        {
            var vm = new ImportViewModel();
            try
            {
                materielFluxMgr.ExecuteImport(isFull);
                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }

            return View("Index", vm);
        }

        /// <summary>
        /// Toggle importation récurrente des matériels
        /// </summary>    
        /// <param name="activate">Activer la planification ou la supprimer</param>
        /// <returns>ActionResult</returns>
        public ActionResult ToggleScheduleImportMateriel(bool activate)
        {
            var vm = new ImportViewModel();
            try
            {
                if (activate)
                {
                    materielFluxMgr.ScheduleImport("0 * * * *");
                }
                else
                {
                    RecurringJob.RemoveIfExists(materielFluxMgr.ImportJobId);
                }

                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        /// <summary>
        /// Importation des matériels depuis le 1er janvier 1900
        /// </summary>
        /// <param name="isFull">Active a une sunchronisation full.</param>
        /// <returns>ActionResult</returns>
        public ActionResult ImportMaterielFayatTP(bool isFull)
        {
            var vm = new ImportViewModel();
            try
            {
                materielFaytaTPFluxMgr.ExecuteImport(isFull);
                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }

            return View("Index", vm);
        }

        /// <summary>
        ///   Toggle importation récurrente des matériels
        /// </summary>    
        /// <param name="activate">Activer la planification ou la supprimer</param>
        /// <returns>ActionResult</returns>
        public ActionResult ToggleScheduleImportMaterielFayatTP(bool activate)
        {
            var vm = new ImportViewModel();
            try
            {
                if (activate)
                {
                    materielFaytaTPFluxMgr.ScheduleImport("15 * * * *");
                }
                else
                {
                    RecurringJob.RemoveIfExists(materielFaytaTPFluxMgr.ImportJobId);
                }

                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        #endregion Matériel

        #region Stair

        /// <summary>
        /// Importation des fichiers STAIR
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ImportIndicateursSafeStair()
        {
            var vm = new ImportViewModel();
            try
            {
                stairFluxManager.ExecuteImport();
                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        /// <summary>
        /// Toggle importation récurrente des fichiers STAIR
        /// </summary>    
        /// <param name="activate">Activer la planification ou la supprimer</param>
        /// <returns>ActionResult</returns>
        public ActionResult ToggleScheduleImportIndicateursSafeStair(bool activate)
        {
            var vm = new ImportViewModel();
            try
            {
                if (activate)
                {
                    stairFluxManager.ScheduleImport("0 * * * *");
                }
                else
                {
                    RecurringJob.RemoveIfExists(stairFluxManager.ImportJobId);
                }

                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        /// <summary>
        /// Importation des fichiers STAIR
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ImportFormulaireSphinxStair()
        {
            var vm = new ImportViewModel();
            try
            {
                sphinxFluxManager.ExecuteImport();
                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }

            return View("Index", vm);
        }

        /// <summary>
        /// Toggle importation récurrente des fichiers STAIR
        /// </summary>    
        /// <param name="activate">Activer la planification ou la supprimer</param>
        /// <returns>ActionResult</returns>
        public ActionResult ToggleScheduleImportFormulaireSphinxStair(bool activate)
        {
            var vm = new ImportViewModel();
            try
            {
                if (activate)
                {
                    sphinxFluxManager.ScheduleImport("0 * * * *");
                }
                else
                {
                    RecurringJob.RemoveIfExists(sphinxFluxManager.ImportJobId);
                }

                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }
        #endregion Stair

        #region Journaux Comptable
        /// <summary>
        /// Importation des Journaux comptables
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ImportJournauxComptableRzb()
        {
            var vm = new ImportViewModel();
            try
            {
                journauxComptableFluxManagerRzb.ExecuteImport();
                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }

            return View("Index", vm);
        }

        /// <summary>
        /// Importation des Journaux comptables Moulins
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ImportJournauxComptableMoulins()
        {
            ImportViewModel vm = new ImportViewModel();
            try
            {
                journauxComptableFluxManagerMoulins.ExecuteImport();
                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }
        #endregion

        #region Suppression des rôles et des logins des personnels ayant quittés une société

        ///<summary>
        /// suppression des roles des personnels
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult CleaningOutgoingUsers()
        {
            var vm = new ImportViewModel();
            try
            {
                cleaningOutgoingUsersFluxManager.ExecuteImport();
                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        /// <summary>
        /// Toggle importation récurrente des Suppression des rôles et des logins des personnels ayant quittés une société
        /// </summary>    
        /// <param name="activate">Activer la planification ou la supprimer</param>
        /// <returns>ActionResult</returns>
        public ActionResult ToggleScheduleCleaningOutgoingUsers(bool activate)
        {
            var vm = new ImportViewModel();

            try
            {
                if (activate)
                {
                    cleaningOutgoingUsersFluxManager.ScheduleImportByCodeFlux(CleaningOutgoingUsersFluxCode);
                }
                else
                {
                    RecurringJob.RemoveIfExists(CleaningOutgoingUsersFluxCode);
                }

                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }

            return View("Index", vm);
        }

        #endregion

        #region Personnel GENERIQUE

        /// <summary>
        /// Importation des contrats interimaires
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ImportContratInterimaire()
        {
            var vm = new ImportViewModel();
            try
            {
                contratInterimaireFluxManager.ExecuteImportByCodeFlux(ImportContratInterimaireFluxCode);
                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        /// <summary>
        /// Toggle importation récurrente des contrats interimaires
        /// </summary>    
        /// <param name="activate">Activer la planification ou la supprimer</param>
        /// <param name="cron">Command On Run</param>
        /// <returns>ActionResult</returns>
        public ActionResult ToggleScheduleImportContratInterimaire(bool activate, string cron = "0 3,13 * * *")
        {
            var vm = new ImportViewModel();
            string codeFlux = "CTR_RZB";
            try
            {
                if (activate)
                {
                    contratInterimaireFluxManager.ScheduleImportByCodeFlux(ImportContratInterimaireFluxCode, cron);
                }
                else
                {
                    RecurringJob.RemoveIfExists(codeFlux);
                }

                vm = GetImportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        #endregion

        /// <summary>
        /// Load model
        /// </summary>
        /// <returns>ImportViewModel</returns>
        private ImportViewModel GetImportViewModel()
        {
            ImportViewModel vm = new ImportViewModel();
            try
            {
                List<RecurringJobDto> recurringJobs = JobStorage.Current.GetConnection().GetRecurringJobs();

                var codeFluxRzb = ConfigurationManager.AppSettings["flux:ci"];
                vm.CiFlux = fluxMrg.GetByCode(codeFluxRzb);
                vm.CiRecurringJob = recurringJobs.Find(x => x.Id == codeFluxRzb);

                var codeFluxGroupeRzb = ConfigurationManager.AppSettings["flux:ci_GRZB"];
                vm.CIFluxGRZB = fluxMrg.GetByCode(codeFluxGroupeRzb);
                vm.CIRecurringJobGRZB = recurringJobs.Find(x => x.Id == codeFluxGroupeRzb);

                var codeFluxEtablissementComptable = ConfigurationManager.AppSettings["flux:etablissement:comptable"];
                vm.EtablissementComptableFlux = fluxMrg.GetByCode(codeFluxEtablissementComptable);
                vm.EtablissementComptableRecurringJob = recurringJobs.Find(x => x.Id == codeFluxEtablissementComptable);

                vm.FournisseurFlux = fluxMrg.GetByCode(fournisseurFluxManager.ImportJobId);
                vm.FournisseurRecurringJob = recurringJobs.Find(x => x.Id == fournisseurFluxManager.ImportJobId);

                vm.PersonnelFlux = fluxMrg.GetByCode(PersonnelFluxCode.CodeFluxRzb);
                vm.PersonnelRecurringJob = recurringJobs.Find(x => x.Id == PersonnelFluxCode.CodeFluxRzb);

                vm.PersonnelFluxGRZB = fluxMrg.GetByCode(PersonnelFluxCode.CodeFluxGrzb);
                vm.PersonnelRecurringJobGRZB = recurringJobs.Find(x => x.Id == PersonnelFluxCode.CodeFluxGrzb);

                vm.PersonnelFluxGFTP = fluxMrg.GetByCode(PersonnelFluxCode.CodeFluxFtp);
                vm.PersonnelRecurringJobGFTP = recurringJobs.Find(x => x.Id == PersonnelFluxCode.CodeFluxFtp);

                vm.PersonnelFluxFES = fluxMrg.GetByCode(PersonnelFluxCode.CodeFluxFes);
                vm.PersonnelRecurringJobFES = recurringJobs.Find(x => x.Id == PersonnelFluxCode.CodeFluxFes);

                vm.PersonnelFluxFON = fluxMrg.GetByCode(PersonnelFluxCode.CodeFluxFon);
                vm.PersonnelRecurringJobFON = recurringJobs.Find(x => x.Id == PersonnelFluxCode.CodeFluxFon);

                vm.EcritureComptableFluxRzb = fluxMrg.GetByCode(RzbEcritureComptableFluxManager.ImportJobIdRzb);
                vm.EcritureComptableRecurringJobRzb = recurringJobs.Find(x => x.Id == RzbEcritureComptableFluxManager.ImportJobIdRzb);
                vm.EcritureComptableDateDebutComptableRzb = DateTime.UtcNow;
                vm.EcritureComptableDateFinComptableRzb = DateTime.UtcNow.AddMonths(1);

                vm.EcritureComptableFluxMoulins = fluxMrg.GetByCode(MoulinsEcritureComptableFluxManager.ImportJobIdMoulins);
                vm.EcritureComptableRecurringJobMoulins = recurringJobs.Find(x => x.Id == MoulinsEcritureComptableFluxManager.ImportJobIdMoulins);
                vm.EcritureComptableDateDebutComptableMoulins = DateTime.UtcNow;
                vm.EcritureComptableDateFinComptableMoulins = DateTime.UtcNow.AddMonths(1);

                vm.MaterielFlux = fluxMrg.GetByCode(materielFluxMgr.ImportJobId);
                vm.MaterielRecurringJob = recurringJobs.Find(x => x.Id == materielFluxMgr.ImportJobId);

                vm.MaterielFayatTpFlux = fluxMrg.GetByCode(materielFaytaTPFluxMgr.ImportJobId);
                vm.MaterielFayatTpRecurringJob = recurringJobs.Find(x => x.Id == materielFaytaTPFluxMgr.ImportJobId);

                vm.StairFlux = fluxMrg.GetByCode(stairFluxManager.ImportJobId);
                vm.StairRecurringJob = recurringJobs.Find(x => x.Id == stairFluxManager.ImportJobId);

                vm.SphinxFlux = fluxMrg.GetByCode(sphinxFluxManager.ImportJobId);
                vm.SphinxRecurringJob = recurringJobs.Find(x => x.Id == sphinxFluxManager.ImportJobId);

                vm.JournauxComptableFluxRzb = fluxMrg.GetByCode(journauxComptableFluxManagerRzb.FluxCode);
                vm.JournauxComptableRecurringJobRzb = recurringJobs.Find(x => x.Id == journauxComptableFluxManagerRzb.FluxCode);

                vm.JournauxComptableFluxMoulins = fluxMrg.GetByCode(journauxComptableFluxManagerMoulins.FluxCode);
                vm.JournauxComptableRecurringJobMoulins = recurringJobs.Find(x => x.Id == journauxComptableFluxManagerMoulins.FluxCode);

                var codeFluxContratInterimaire = ImportContratInterimaireFluxCode;
                vm.ContratInteriamireFlux = fluxMrg.GetByCode(codeFluxContratInterimaire);
                vm.ContratInteriamireRecurringJob = recurringJobs.Find(x => x.Id == codeFluxContratInterimaire);

                vm.CleaningOutgoingUsers = fluxMrg.GetByCode(CleaningOutgoingUsersFluxCode);
                vm.CleaningOutgoingUsersJob = recurringJobs.Find(x => x.Id == CleaningOutgoingUsersFluxCode);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return vm;
        }
    }
}