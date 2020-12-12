using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Fred.Entities.Organisation.Tree;
using Fred.ImportExport.Business.Cache.SynchronizationFredWeb.HangfireFilter;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.RepriseDonnees;
using Fred.ImportExport.Resources.Resources;
using Fred.ImportExport.Web.Models;

namespace Fred.ImportExport.Web.Controllers
{
    public class RepriseDonneesController : Controller
    {
        private readonly IRepriseDonneesViewManager repriseDonneesViewManager;

        public RepriseDonneesController(IRepriseDonneesViewManager repriseDonneesViewManager)
        {
            this.repriseDonneesViewManager = repriseDonneesViewManager;
        }

        // GET: RepriseDonnees
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UpdateCis()
        {
            var model = new RepriseDonneesModel();

            model.Groupes = GetListItems();

            return View(nameof(UpdateCis), model);
        }

        private List<SelectListItem> GetListItems()
        {
            var groupes = repriseDonneesViewManager.GetAllGroupes();

            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var groupe in groupes)
            {
                items.Add(new SelectListItem { Text = groupe.Code, Value = groupe.GroupeId.ToString() });
            }
            return items;
        }

        private List<SelectListItem> GetListItems(List<int> groupeIds)
        {
            var groupes = repriseDonneesViewManager.GetAllGroupes().Where(x => groupeIds.Contains(x.GroupeId));

            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var groupe in groupes)
            {
                items.Add(new SelectListItem { Text = groupe.Code, Value = groupe.GroupeId.ToString() });
            }
            return items;
        }

        [HttpPost]
        public ActionResult UpdateCis(RepriseDonneesModel repriseDonneesModel)
        {
            Execute(repriseDonneesModel, (groupeId, file) => repriseDonneesViewManager.ImportCis(groupeId, file.InputStream).ErrorMessages);

            repriseDonneesModel.Groupes = GetListItems();

            return View(repriseDonneesModel);
        }

        public ActionResult CreateCommandesAndReceptions()
        {
            var model = new RepriseDonneesModel();

            model.Groupes = GetListItems();

            return View(nameof(CreateCommandesAndReceptions), model);
        }


        [HttpPost]
        public ActionResult CreateCommandesAndReceptions(RepriseDonneesModel repriseDonneesModel)
        {
            Execute(repriseDonneesModel, (groupeId, file) => repriseDonneesViewManager.CreateCommandeAndReceptions(groupeId, file.InputStream).ErrorMessages);

            repriseDonneesModel.Groupes = GetListItems();

            return View(repriseDonneesModel);
        }

        public ActionResult CreateRapports()
        {
            var model = new RepriseDonneesModel
            {
                Groupes = GetListItems()
            };
            return View(nameof(CreateRapports), model);
        }


        [HttpPost]
        public ActionResult CreateRapports(RepriseDonneesModel repriseDonneesModel)
        {
            Execute(repriseDonneesModel, (groupeId, file) => repriseDonneesViewManager.ImportRapports(groupeId, file.InputStream).ErrorMessages);
            repriseDonneesModel.Groupes = GetListItems();

            return View(repriseDonneesModel);
        }


        public ActionResult ValidationCommandes()
        {
            var model = new RepriseDonneesModel();

            return View(nameof(ValidationCommandes), model);
        }


        [HttpPost]
        public ActionResult ValidationCommandes(RepriseDonneesModel repriseDonneesModel)
        {
            //ici pas de selection de groupe donc groupeid = 0 => pas d'incidence sur le code de la validation car non utilisé dans le code de la validation
            // pas de refacto du code Execute car complexification inutile
            repriseDonneesModel.SelectedGroupe = "0";

            Execute(repriseDonneesModel, (groupeId, file) => repriseDonneesViewManager.ValidationCommandes(groupeId, file.InputStream).ErrorMessages);

            repriseDonneesModel.Groupes = GetListItems();

            return View(repriseDonneesModel);
        }

        private void Execute(RepriseDonneesModel repriseDonneesModel, Func<int, HttpPostedFileBase, List<string>> executeFunc)
        {
            if (GetUploadedFile(repriseDonneesModel, out HttpPostedFileBase file))
            {
                return;
            }

            if (GetSelectedGroup(repriseDonneesModel, out int groupeId))
            {
                return;
            }

            try
            {
                repriseDonneesModel.Errors = executeFunc(groupeId, file);

                FillViewBag(repriseDonneesModel);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "ERROR:"
                    + Environment.NewLine
                    + " - "
                    + ex.Message
                    + Environment.NewLine
                    + " - "
                    + ex.InnerException?.Message
                    + Environment.NewLine
                    + " - "
                    + ex.InnerException?.InnerException?.Message;
            }
        }

        private async Task ExecuteAsync(RepriseDonneesModel repriseDonneesModel, Func<int, HttpPostedFileBase, Task<List<string>>> executeFunc)
        {
            if (GetUploadedFile(repriseDonneesModel, out HttpPostedFileBase file))
            {
                return;
            }

            if (GetSelectedGroup(repriseDonneesModel, out int groupeId))
            {
                return;
            }

            try
            {
                repriseDonneesModel.Errors = await executeFunc(groupeId, file);

                FillViewBag(repriseDonneesModel);

            }
            catch (Exception ex)
            {
                ViewBag.Message = "ERROR:"
                                  + Environment.NewLine
                                  + " - "
                                  + ex.Message
                                  + Environment.NewLine
                                  + " - "
                                  + ex.InnerException?.Message
                                  + Environment.NewLine
                                  + " - "
                                  + ex.InnerException?.InnerException?.Message;
            }
        }

        private bool GetUploadedFile(RepriseDonneesModel repriseDonneesModel, out HttpPostedFileBase file)
        {
            file = repriseDonneesModel.Files[0];
            if (file != null && file.ContentLength > 0)
            {
                return false;
            }

            ViewBag.Message = Resource.RepriseDonneesController_Veuillez_selectionner_un_Fichier;

            return true;

        }

        private bool GetSelectedGroup(RepriseDonneesModel repriseDonneesModel, out int groupeId)
        {
            if (int.TryParse(repriseDonneesModel.SelectedGroupe, out groupeId))
            {
                return false;
            }

            ViewBag.Message = Resource.RepriseDonneesController_Veuillez_selectionner_un_groupe;

            return true;

        }

        private void FillViewBag(RepriseDonneesModel repriseDonneesModel)
        {
            if (repriseDonneesModel.Errors != null && repriseDonneesModel.Errors.Any())
            {
                ViewBag.Message = Resource.RepriseDonneesController_Echec_Insertion_dans_Fred;
            }
            else
            {
                ViewBag.Message = Resource.RepriseDonneesController_Insertion_dans_Fred_reussie;
            }
        }

        public ActionResult CreatePlanTaches()
        {
            var model = new RepriseDonneesModel
            {
                Groupes = GetListItems()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult CreatePlanTaches(RepriseDonneesModel repriseDonneesModel)
        {
            Execute(repriseDonneesModel, (groupeId, file) => repriseDonneesViewManager.CreatePlanTaches(groupeId, file.InputStream).ErrorMessages);

            repriseDonneesModel.Groupes = GetListItems();

            return View(repriseDonneesModel);
        }

        public ActionResult CreatePersonnel()
        {
            RepriseDonneesModel model = new RepriseDonneesModel
            {
                Groupes = GetListItems()
            };

            return View(nameof(CreatePersonnel), model);
        }

        [HttpPost]
        public ActionResult CreatePersonnel(RepriseDonneesModel repriseDonneesModel)
        {
            Execute(repriseDonneesModel, (groupeId, file) => repriseDonneesViewManager.CreatePersonnel(groupeId, file.InputStream).ErrorMessages);

            repriseDonneesModel.Groupes = GetListItems();

            return View(repriseDonneesModel);
        }

        public ActionResult CreateMateriel()
        {
            RepriseDonneesModel model = new RepriseDonneesModel
            {
                Groupes = GetListItems()
            };

            return View(nameof(CreateMateriel), model);
        }

        [HttpPost]
        public ActionResult CreateMateriel(RepriseDonneesModel repriseDonneesModel)
        {
            Execute(repriseDonneesModel, (groupeId, file) => repriseDonneesViewManager.CreateMateriel(groupeId, file.InputStream).ErrorMessages);

            repriseDonneesModel.Groupes = GetListItems();

            return View(repriseDonneesModel);
        }

        public ActionResult CreateIndemniteDeplacement()
        {
            RepriseDonneesModel model = new RepriseDonneesModel
            {
                Groupes = GetListItems()
            };

            return View(nameof(CreateIndemniteDeplacement), model);
        }

        [HttpPost]
        public ActionResult CreateIndemniteDeplacement(RepriseDonneesModel repriseDonneesModel)
        {
            Execute(repriseDonneesModel, (groupeId, file) => repriseDonneesViewManager.CreateIndemniteDeplacement(groupeId, file.InputStream).ErrorMessages);

            repriseDonneesModel.Groupes = GetListItems();

            return View(repriseDonneesModel);
        }

        public ActionResult ImportCiFromAnaelAndSendToSap()
        {
            var model = new RepriseDonneesModel();

            model.Groupes = GetListItems();

            return View(nameof(ImportCiFromAnaelAndSendToSap), model);
        }
        public ActionResult ImportFournisseursFromAnaelAndSendToSap()
        {
            var model = new RepriseDonneesModel();

            model.Groupes = GetListItems(new List<int> { 1 });

            return View(nameof(ImportFournisseursFromAnaelAndSendToSap), model);
        }


        [HttpPost]
        [CacheSynchronization(OrganisationTreeCacheKey.FredOrganisationTreeCacheKey)]
        public async Task<ActionResult> ImportCiFromAnaelAndSendToSap(RepriseDonneesModel repriseDonneesModel)
        {
            await ExecuteAsync(repriseDonneesModel, async (groupeId, file) =>
            {
                Business.CI.AnaelSystem.Context.Common.ImportResult importResult = await repriseDonneesViewManager.ImportCiFromAnaelAndSendToSapAsync(groupeId, file.InputStream);

                return importResult.ErrorMessages;
            });

            repriseDonneesModel.Groupes = GetListItems();

            return View(repriseDonneesModel);
        }

        [HttpPost]
        public async Task<ActionResult> ImportFournisseursFromAnaelAndSendToSap(RepriseDonneesModel repriseDonneesModel)
        {
            //Seulement pour le groupe RZB
            await ExecuteAsync(repriseDonneesModel, async (groupeId, file) =>
            {
                ImportResult importResult = await repriseDonneesViewManager.ImportFournisseursFromAnaelAndSendToSapAsync(groupeId, file.InputStream);

                return importResult.ErrorMessages;
            });

            repriseDonneesModel.Groupes = GetListItems(new List<int> { 1 });

            return View(repriseDonneesModel);
        }

    }
}
