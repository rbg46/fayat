using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fred.ImportExport.Business;
using Fred.ImportExport.Business.Depense;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Groupe;
using Fred.ImportExport.Business.Kilometre;
using Fred.ImportExport.Business.Personnel;
using Fred.ImportExport.Business.ReceptionInterimaire;
using Fred.ImportExport.Business.Stair.ExportCI;
using Fred.ImportExport.Business.Stair.ExportPersonnel;
using Fred.ImportExport.Models.Groupe;
using Fred.ImportExport.Resources.Resources;
using Fred.ImportExport.Web.Models;
using Hangfire;
using Hangfire.Storage;

namespace Fred.ImportExport.Web.Controllers
{
    public class ExportController : ControllerBase
    {
        private readonly IFluxManager fluxManager;
        private readonly KlmFluxManager klmFluxManager;
        private readonly StairPersonnelFluxManager stairPersonnelFluxManager;
        private readonly ReceptionInterimaireFluxManager receptionInterimaireFluxManager;
        private readonly DepenseFluxManager depenseFluxManager;
        private readonly StairCiFluxManager stairCiFluxManager;
        private readonly PersonnelFluxManager personnelFluxManager;
        private readonly IGroupeInterimaireManager groupeInterimaireManager;

        public ExportController(
            IFluxManager fluxManager,
            KlmFluxManager klmFluxManager,
            StairPersonnelFluxManager stairPersonnelFluxManager,
            ReceptionInterimaireFluxManager receptionInterimaireFluxManager,
            DepenseFluxManager depenseFluxManager,
            StairCiFluxManager stairCiFluxManager,
            PersonnelFluxManager personnelFluxManager,
            IGroupeInterimaireManager groupeInterimaireManager)
        {
            this.fluxManager = fluxManager;
            this.klmFluxManager = klmFluxManager;
            this.stairPersonnelFluxManager = stairPersonnelFluxManager;
            this.receptionInterimaireFluxManager = receptionInterimaireFluxManager;
            this.depenseFluxManager = depenseFluxManager;
            this.stairCiFluxManager = stairCiFluxManager;
            this.personnelFluxManager = personnelFluxManager;
            this.groupeInterimaireManager = groupeInterimaireManager;
        }

        /// <summary>
        ///   Retourne la vue principale avec le model
        /// </summary>
        /// <returns>Action result</returns>    
        public ActionResult Index()
        {
            ExportViewModel vm = GetExportViewModel();
            return View(vm);
        }

        #region Klm

        /// <summary>
        /// Export des KLM.
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ExportKlm()
        {
            var vm = new ExportViewModel();
            try
            {
                klmFluxManager.ExecuteExport();
                vm = GetExportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }

            return View("Index", vm);
        }

        /// <summary>
        ///   Toggle export récurrent des KLM.
        /// </summary>    
        /// <param name="activate">Activer la planification ou la supprimer</param>
        /// <returns>ActionResult</returns>
        public ActionResult ToggleScheduleExportKlm(bool activate)
        {
            var vm = new ExportViewModel();
            try
            {
                if (activate)
                {
                    klmFluxManager.ScheduleExport("30 1,11 * * *");   // 02h00 ou 03h00 et 12h30 ou 13h30 heure locale
                }
                else
                {
                    RecurringJob.RemoveIfExists(klmFluxManager.ImportJobId);
                }

                vm = GetExportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        #endregion Matériel
        #region Réception Intérimaire

        /// <summary>
        /// Export des réceptions intérimaires.
        /// </summary>
        /// <param name="model">Model de la vue</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public ActionResult ExportReceptionInterimaire(ExportViewModel model)
        {
            List<int> listSocieteId = new List<int>();

            foreach (GroupeInterimaireModel groupe in model.GroupeInterimaire)
            {
                listSocieteId.AddRange(groupe.SocieteNotInterimaires.Where(s => s.Checked).Select(s => s.SocieteId));
            }

            try
            {
                receptionInterimaireFluxManager.ExecuteExport(listSocieteId);
                model = GetExportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }

            ModelState.Clear();
            return View("Index", model);
        }

        /// <summary>
        ///   Toggle export récurrent des réceptions intérimaires.
        /// </summary>    
        /// <param name="activate">Activer la planification ou la supprimer</param>
        /// <returns>ActionResult</returns>
        public ActionResult ToggleScheduleExportReceptionInterimaire(bool activate)
        {
            var vm = new ExportViewModel();
            try
            {
                if (activate)
                {
                    receptionInterimaireFluxManager.ScheduleExport("59 21 * * 1");   // Tous les lundis à 23h59
                }
                else
                {
                    RecurringJob.RemoveIfExists(receptionInterimaireFluxManager.ExportJobId);
                }

                vm = GetExportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        #endregion Réception Intérimaire

        #region STAIR Personnel

        /// <summary>
        /// Export des KLM.
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ExportStairPersonnel()
        {
            var vm = new ExportViewModel();
            try
            {
                stairPersonnelFluxManager.ExecuteExport();
                vm = GetExportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }

            return View("Index", vm);
        }

        /// <summary>
        ///   Toggle export récurrent des KLM.
        /// </summary>    
        /// <param name="activate">Activer la planification ou la supprimer</param>
        /// <returns>ActionResult</returns>
        public ActionResult ToggleScheduleExportStairPersonnel(bool activate)
        {
            var vm = new ExportViewModel();
            try
            {
                if (activate)
                {
                    stairPersonnelFluxManager.ScheduleExport("30 1,11 * * *");   // 02h00 ou 03h00 et 12h30 ou 13h30 heure locale
                }
                else
                {
                    RecurringJob.RemoveIfExists(stairPersonnelFluxManager.ExportJobId);
                }

                vm = GetExportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        #endregion STAIR Personnel
        #region STAIR CI

        /// <summary>
        /// Export des KLM.
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ExportStairCI()
        {
            var vm = new ExportViewModel();
            try
            {
                stairCiFluxManager.ExecuteExport();
                vm = GetExportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }

            return View("Index", vm);
        }

        /// <summary>
        ///   Toggle export récurrent des KLM.
        /// </summary>    
        /// <param name="activate">Activer la planification ou la supprimer</param>
        /// <returns>ActionResult</returns>
        public ActionResult ToggleScheduleExportStairCI(bool activate)
        {
            var vm = new ExportViewModel();
            try
            {
                if (activate)
                {
                    stairCiFluxManager.ScheduleExport("30 1,11 * * *");   // 02h00 ou 03h00 et 12h30 ou 13h30 heure locale
                }
                else
                {
                    RecurringJob.RemoveIfExists(stairCiFluxManager.ExportJobId);
                }

                vm = GetExportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        #endregion STAIR CI

        #region Réception Matériel Externe

        /// <summary>
        /// Export des réceptions matériel externe
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ExportReceptionMaterielExterne()
        {
            var vm = new ExportViewModel();
            try
            {
                depenseFluxManager.ExecuteExport();
                vm = GetExportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }

            return View("Index", vm);
        }

        /// <summary>
        ///   Toggle export récurrent des réceptions intérimaires.
        /// </summary>    
        /// <param name="activate">Activer la planification ou la supprimer</param>
        /// <returns>ActionResult</returns>
        public ActionResult ToggleScheduleExportReceptionMaterielExterne(bool activate)
        {
            var vm = new ExportViewModel();
            try
            {
                if (activate)
                {
                    depenseFluxManager.ScheduleExport("59 21 * * 1");   // Tous les lundis à 23h59
                }
                else
                {
                    RecurringJob.RemoveIfExists(depenseFluxManager.CodeMaterielExterne);
                }

                vm = GetExportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        #endregion Réception Matériel Externe

        #region Personnel FES (FIGGO)

        /// <summary>
        /// Export des réceptions matériel externe
        /// </summary>
        /// <param name="byPassDate">Ignore la dernière date d'exécution</param>
        /// <returns>ActionResult</returns>
        public ActionResult ExportPersonnelFesToTibco(bool byPassDate)
        {
            var vm = new ExportViewModel();
            try
            {
                personnelFluxManager.ExecuteExportPersonnelToTibco(byPassDate);
                vm = GetExportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }

            return View("Index", vm);
        }

        /// <summary>
        ///   Toggle export récurrent des réceptions intérimaires.
        /// </summary>    
        /// <param name="activate">Activer la planification ou la supprimer</param>
        /// <returns>ActionResult</returns>
        public ActionResult ToggleScheduleExportPersonnelFesToTibco(bool activate)
        {
            var vm = new ExportViewModel();
            try
            {
                if (activate)
                {
                    personnelFluxManager.ScheduleExportPersonnelToTibco("59 21 * * *");   // Tous les jours à 23h59
                }
                else
                {
                    RecurringJob.RemoveIfExists(personnelFluxManager.ExportJobId);
                }

                vm = GetExportViewModel();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                ViewBag.Error = Resource.Error_Exception;
            }
            return View("Index", vm);
        }

        #endregion Personnel FES (FIGGO)


        /// <summary>
        ///   Load model
        /// </summary>
        /// <returns>ExportViewModel</returns>
        private ExportViewModel GetExportViewModel()
        {
            var vm = new ExportViewModel();
            try
            {
                List<RecurringJobDto> recurringJobs = JobStorage.Current.GetConnection().GetRecurringJobs();

                vm.KlmFlux = fluxManager.GetByCode(klmFluxManager.ImportJobId);
                vm.KlmRecurringJob = recurringJobs.Find(x => x.Id == klmFluxManager.ImportJobId);

                vm.ReceptionInterimaireFlux = fluxManager.GetByCode(receptionInterimaireFluxManager.ExportJobId);
                vm.ReceptionInterimaireRecurringJob = recurringJobs.Find(x => x.Id == receptionInterimaireFluxManager.ExportJobId);

                vm.ReceptionMaterielExterneFlux = fluxManager.GetByCode(depenseFluxManager.CodeMaterielExterne);
                vm.ReceptionMaterielExterneRecurringJob = recurringJobs.Find(x => x.Id == depenseFluxManager.CodeMaterielExterne);

                vm.StairPersonnelFlux = fluxManager.GetByCode(stairPersonnelFluxManager.ExportJobId);
                vm.StairPersonnelRecurringJob = recurringJobs.Find(x => x.Id == stairPersonnelFluxManager.ExportJobId);

                vm.StairCIFlux = fluxManager.GetByCode(stairCiFluxManager.ExportJobId);
                vm.StairCIRecurringJob = recurringJobs.Find(x => x.Id == stairCiFluxManager.ExportJobId);

                vm.PersonnelFesFlux = fluxManager.GetByCode(personnelFluxManager.ExportJobId);
                vm.PersonnelFesRecurringJob = recurringJobs.Find(x => x.Id == personnelFluxManager.ExportJobId);

                vm.GroupeInterimaire = groupeInterimaireManager.GetGroupeInterimaire();
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
