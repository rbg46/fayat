using AutoMapper;
using Fred.Business.Budget;
using Fred.Business.Budget.Avancement.Excel;
using Fred.Business.Budget.BibliothequePrix;
using Fred.Business.Budget.BibliothequePrix.Validator;
using Fred.Business.Budget.BudgetManager.Dtao;
using Fred.Business.Budget.ControleBudgetaire.Models;
using Fred.Business.CI;
using Fred.Business.Common.ExportDocument;
using Fred.Business.Utilisateur;
using Fred.Entities.Permission;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.Web.Models.Budget.Liste;
using Fred.Web.Models.ReferentielFixe.Light;
using Fred.Web.Modules.Authorization.Api;
using Fred.Web.Shared.Models.Budget;
using Fred.Web.Shared.Models.Budget.Avancement;
using Fred.Web.Shared.Models.Budget.Avancement.Excel;
using Fred.Web.Shared.Models.Budget.BibliothequePrix;
using Fred.Web.Shared.Models.Budget.ControleBudgetaire;
using Fred.Web.Shared.Models.Budget.Details;
using Fred.Web.Shared.Models.Budget.Recette;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Http;
using Fred.Business.Budget.BudgetManager;

namespace Fred.Web.API
{
    public class BudgetController : ApiControllerBase
    {
        protected readonly IMapper Mapper;
        private readonly UtilisateurEnt utilisateurConnecte;
        private readonly IExportDocumentService exportDocumentService;
        private readonly IBudgetMainManager budgetMainManager;
        private readonly ICIManager ciManager;
        private readonly IBudgetManager budgetManager;
        private readonly IBudgetCopieManager budgetCopieManager;
        private readonly IControleBudgetaireManager controleBudgetaireManager;
        private readonly IAvancementExportExcelManager avancementExportExcelManager;
        private readonly IBudgetBibliothequePrixManager budgetBibliothequePrixManager;
        private readonly IControleBudgetaireExcelManager controleBudgetaireExcelManager;
        private readonly ICopierBudgetSourceToCible copierBudgetSourceToCible;

        public BudgetController(
            IMapper mapper,
            IUtilisateurManager utilisateurManager,
            IExportDocumentService exportDocumentService,
            IBudgetMainManager budgetMainManager,
            ICIManager ciManager,
            IBudgetManager budgetManager,
            IBudgetCopieManager budgetCopieManager,
            IControleBudgetaireManager controleBudgetaireManager,
            IAvancementExportExcelManager avancementExportExcelManager,
            IBudgetBibliothequePrixManager budgetBibliothequePrixManager,
            IControleBudgetaireExcelManager controleBudgetaireExcelManager,
            ICopierBudgetSourceToCible copierBudgetSourceToCible
            )
        {
            this.Mapper = mapper;
            this.exportDocumentService = exportDocumentService;
            this.budgetMainManager = budgetMainManager;
            this.ciManager = ciManager;
            this.budgetManager = budgetManager;
            this.budgetCopieManager = budgetCopieManager;
            this.controleBudgetaireManager = controleBudgetaireManager;
            this.avancementExportExcelManager = avancementExportExcelManager;
            this.budgetBibliothequePrixManager = budgetBibliothequePrixManager;
            this.controleBudgetaireExcelManager = controleBudgetaireExcelManager;
            this.copierBudgetSourceToCible = copierBudgetSourceToCible;

            utilisateurConnecte = utilisateurManager.GetContextUtilisateur();
        }


        [HttpGet]
        [Route("api/Budget/GetDetail")]
        public IHttpActionResult GetDetail(int budgetId)
        {
            var detail = budgetMainManager.GetDetail(budgetId);

            return Ok(detail);
        }

        [HttpPut]
        [Route("api/Budget/GetDetail/Excel")]
        public IHttpActionResult GetExportExcelDetailsBudget(BudgetDetailsExportExcelLoadModel model)
        {
            var excelBytes = budgetMainManager.GetBudgetDetailExportExcel(model);
            string typeCache = model.IsPdfConverted ? "pdfBytes_" : "excelBytes_";
            string cacheId = Guid.NewGuid().ToString();

            CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(300), Priority = CacheItemPriority.NotRemovable };
            MemoryCache.Default.Add(typeCache + cacheId, excelBytes, policy);

            return Ok(new { id = cacheId });
        }

        [HttpPost]
        [Route("api/Budget/SaveDetail")]
        public IHttpActionResult SaveDetail(BudgetDetailSave.Model model)
        {
            var detail = budgetMainManager.SaveDetail(model);

            return Created(string.Empty, detail);
        }

        [HttpPost]
        [Route("api/Budget/ValidateDetailBudget/")]
        public IHttpActionResult ValidateDetailBudget(int budgetId, string commentaire)
        {
            var detail = budgetMainManager.ValidateDetailBudget(budgetId, commentaire);

            return Created(string.Empty, detail);
        }

        [HttpPost]
        [Route("api/Budget/RetourBrouillon/")]
        public IHttpActionResult RetourBrouillon(int budgetId, string commentaire)
        {
            var detail = budgetMainManager.RetourBrouillon(budgetId, commentaire);

            return Created(string.Empty, detail);
        }

        [HttpGet]
        [Route("api/Budget/GetNextTacheCode")]
        public IHttpActionResult GetNextTacheCode(int tacheParenteId)
        {
            var nextTacheCode = budgetMainManager.GetNextTacheCode(tacheParenteId);

            return Ok(nextTacheCode);
        }

        [HttpGet]
        [Route("api/Budget/GetNextTacheCodes")]
        public IHttpActionResult GetNextTacheCodes(int tacheParenteId, int count)
        {
            var nextTacheCodes = budgetMainManager.GetNextTacheCodes(tacheParenteId, count);

            return Ok(nextTacheCodes);
        }

        [HttpPost]
        [Route("api/Budget/CreateTache4")]
        public IHttpActionResult CreateTache4(ManageT4Create.Model model)
        {
            try
            {
                var t4 = budgetMainManager.CreateTache4(model);

                return Created(string.Empty, t4);
            }
            catch (FredBusinessException ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Conflict, ex));
            }
        }

        [HttpPost]
        [Route("api/Budget/CreateTaches4")]
        public IHttpActionResult CreateTaches4(CreateTaches4.Model model)
        {
            try
            {
                var t4s = budgetMainManager.CreateTaches4(model);

                return Created(string.Empty, t4s);
            }
            catch (FredBusinessException ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Conflict, ex));
            }
        }

        [HttpPost]
        [Route("api/Budget/ChangeTache4")]
        public IHttpActionResult ChangeTache4(ManageT4Change.Model model)
        {
            try
            {
                var t4 = budgetMainManager.ChangeTache4(model);

                return Created(string.Empty, t4);
            }
            catch (FredBusinessException ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Conflict, ex));
            }
        }

        [HttpPost]
        [Route("api/Budget/DeleteTache4")]
        public IHttpActionResult DeleteTache4(ManageT4Delete.Model model)
        {
            try
            {
                var t4 = budgetMainManager.DeleteTache4(model);

                return Created(string.Empty, t4);
            }
            catch (FredBusinessException ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Conflict, ex));
            }
        }

        [HttpGet]
        [Route("api/Budget/GetSousDetail")]
        public IHttpActionResult GetSousDetail(int ciId, int budgetT4Id)
        {
            var sousDetail = budgetMainManager.GetSousDetail(ciId, budgetT4Id);

            return Ok(sousDetail);
        }

        [HttpPost]
        [Route("api/Budget/SaveSousDetail")]
        public IHttpActionResult SaveSousDetail(BudgetSousDetailSave.Model model)
        {
            var sousDetail = budgetMainManager.SaveSousDetail(model);

            return Created(string.Empty, sousDetail);
        }

        [HttpGet]
        [Route("api/Budget/GetChapitres")]
        public IHttpActionResult GetChapitres(int ciId, int deviseId, string filter = "", int page = 1, int pageSize = 25)
        {
            var bibliothequePrix = budgetMainManager.GetChapitres(ciId, deviseId, filter, page, pageSize);

            return Ok(bibliothequePrix);
        }

        [HttpGet]
        [Route("api/Budget/GereRessourcesRecommandees")]
        public IHttpActionResult GereRessourcesRecommandees(int ciId)
        {
            var etablissementComptable = ciManager.GetEtablissementComptableByCIId(ciId);
            var gereRessourceRecommandees = etablissementComptable != null && etablissementComptable.RessourcesRecommandeesEnabled;

            return Ok(gereRessourceRecommandees);
        }

        [HttpPost]
        [Route("api/Budget/CreateRessource")]
        public IHttpActionResult CreateRessource(RessourceLightModel ressource, int ciId)
        {
            var createdResource = budgetMainManager.CreateRessource(Mapper.Map<RessourceEnt>(ressource));

            return Created(string.Empty, createdResource);
        }

        [HttpPut]
        [Route("api/Budget/UpdateRessource")]
        public IHttpActionResult UpdateRessource(RessourceLightModel ressource, int ciId)
        {
            var updatedResource = budgetMainManager.UpdateRessource(Mapper.Map<RessourceEnt>(ressource));

            return Ok(updatedResource);
        }

        [HttpGet]
        [Route("api/Budget/GetMessageMiseEnApllication")]
        public IHttpActionResult GetMessageMiseEnApllication(int ciId, string version)
        {
            var message = budgetMainManager.GetMessageMiseEnApllication(ciId, version);

            return Ok(message);
        }

        [HttpGet]
        [Route("api/Budget/GetBudgetRevisions/{ciId}")]
        public IHttpActionResult GetBudgetRevisions(int ciId)
        {
            var revisions = budgetManager.GetBudgetRevisions(ciId).ToList();

            return Ok(revisions);
        }

        [HttpPost]
        [Route("api/Budget/CopySousDetails")]
        public IHttpActionResult CopySousDetails(BudgetSousDetailCopier.Model model)
        {
            var sousDetail = budgetMainManager.CopySousDetails(model);

            return Created(string.Empty, sousDetail);
        }

        [HttpPost]
        [Route("api/Budget/GetTache4Inutilisees")]
        public IHttpActionResult GetTache4Inutilisees(Tache4Inutilisees.Model model)
        {
            var tache4Inutilisees = budgetMainManager.GetTache4Inutilisees(model);

            return Created(string.Empty, tache4Inutilisees);
        }

        [HttpGet]
        [Route("api/Budget/CheckPlanDeTacheIdentiques/{ciId1}/{ciId2}")]
        public IHttpActionResult CheckPlanDeTacheIdentiques(int ciId1, int ciId2)
        {
            var checkPlanDeTacheIdentiques = copierBudgetSourceToCible.CheckPlanDeTacheIdentiques(ciId1, ciId2);

            return Ok(checkPlanDeTacheIdentiques);
        }

        [HttpPost]
        [Route("api/Budget/Copier")]
        [FredWebApiAuthorize(globalPermissionKey: PermissionKeys.AffichageBoutonCopierRessourcesBudget)]
        public async Task<IHttpActionResult> Copier(CopierBudgetDto.Request request)
        {
            var copie = await budgetCopieManager.CopierBudgetAsync(request);

            return Created(string.Empty, copie);
        }

        [HttpPost]
        [Route("api/Budget/ControleBudgetaire/")]
        public async Task<IHttpActionResult> GetControleBudgetaire(ControleBudgetaireLoadModel filtre)
        {
            var controleBudgetaireModel = await controleBudgetaireManager.GetControleBudgetaireAsync(filtre).ConfigureAwait(false);

            return Created(string.Empty, controleBudgetaireModel);
        }

        [HttpGet]
        [Route("api/Budget/ControleBudgetaire/Ajustement/{budgetId}/{periode}")]
        public IHttpActionResult GetMontantAjustement(int budgetId, int periode)
        {
            var ressourceAjustementModels = Mapper.Map<IEnumerable<TacheRessourceAjustementModel>>(controleBudgetaireManager.GetControleBudgetaireValeurs(budgetId, periode));

            return Ok(ressourceAjustementModels);
        }

        [HttpGet]
        [Route("api/Budget/ControleBudgetaire/Brouillon/Periode/{budgetId}/{periode}")]
        public IHttpActionResult GetPeriodeControleBudgetaireBrouillon(int budgetId, int periode)
        {
            var allPeriodesBetweenPeriodeAndLastValidation = controleBudgetaireManager.GetAllPeriodesBetweenPeriodeAndLastValidation(budgetId, periode);

            return Ok(allPeriodesBetweenPeriodeAndLastValidation);
        }

        [HttpPut]
        [Route("api/Budget/ControleBudgetaire/")]
        public IHttpActionResult SauveControleBudgetaire(ControleBudgetaireSaveModel model)
        {
            var controleBudgetaire = controleBudgetaireManager.SauveControleBudgetaire(model);

            return Ok(controleBudgetaire);
        }

        [HttpPut]
        [Route("api/Budget/ControleBudgetaire/Etat/{budgetId}/{periode}/{codeEtat}")]
        public IHttpActionResult ChangeEtatControleBudgetaire(int budgetId, int periode, string codeEtat)
        {
            var changeEtatControleBudgetaire = controleBudgetaireManager.ChangeEtatControleBudgetaire(budgetId, periode, codeEtat);

            return Ok(changeEtatControleBudgetaire);
        }

        [HttpPut]
        [Route("api/Budget/ControleBudgetaire/Excel/")]
        public async Task<IHttpActionResult> GetExportExcel(ControleBudgetaireExcelLoadModel controleBudgetaire)
        {
            try
            {
                byte[] excelBytes = await controleBudgetaireExcelManager.GetExportExcelAsync(controleBudgetaire).ConfigureAwait(false);
                string typeCache = controleBudgetaire.IsPdfConverted ? "pdfBytes_" : "excelBytes_";
                string cacheId = Guid.NewGuid().ToString();

                CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(300), Priority = CacheItemPriority.NotRemovable };
                MemoryCache.Default.Add(typeCache + cacheId, excelBytes, policy);

                return Ok(new { id = cacheId });
            }
            catch (Exception ex)
            {
                logger.Log(NLog.LogLevel.Error, ex);
                return null;
            }
        }

        [HttpGet]
        [Route("api/Budget/{ciId}")]
        public IHttpActionResult GetBudget(int ciId)
        {
            var budgets = budgetManager.GetBudgetVisiblePourUserSurCi(utilisateurConnecte.UtilisateurId, ciId);
            var mappedBudget = Mapper.Map<IEnumerable<ListeBudgetModel>>(budgets);

            return Ok(mappedBudget);
        }

        [HttpGet]
        [Route("api/Budget/Application/Brouillons/{ciId}/{deviseId}")]
        public IHttpActionResult GetBudgetBrouillonDuBudgetEnApplication(int ciId, int deviseId)
        {
            var brouillonDuBudgetEnApplication = budgetManager.GetBudgetBrouillonDuBudgetEnApplication(ciId, deviseId);

            return Ok(brouillonDuBudgetEnApplication);
        }

        [HttpGet]
        [Route("api/Budget/GetBudgetsBrouillons/{ciId}/{deviseId}")]
        public IHttpActionResult GetBudgetsBrouillons(int ciId, int deviseId)
        {
            var brouillons = budgetManager.GetBudgetsBrouillons(ciId, deviseId);

            return Ok(brouillons);
        }

        [HttpGet]
        [Route("api/Budget/Id/{budgetId}")]
        public IHttpActionResult GetBudgetByBudgetId(int budgetId)
        {
            var budgets = budgetManager.GetBudget(budgetId);
            var mappedBudget = Mapper.Map<ListeBudgetModel>(budgets);

            return Ok(mappedBudget);
        }

        [HttpDelete]
        [Route("api/Budget/Suppression/{budgetId}")]
        public IHttpActionResult SupprimeBudget(int budgetId)
        {
            var supprimeBudget = budgetManager.SupprimeBudget(budgetId);

            return Ok(supprimeBudget);
        }

        [HttpPut]
        [Route("api/Budget/Suppression/{budgetId}")]
        public IHttpActionResult RestaureBudget(int budgetId)
        {
            var restaurationSuccess = budgetManager.RestaurerBudget(budgetId);

            return Ok(restaurationSuccess);
        }

        [HttpPost]
        [Route("api/Budget/{budgetId}/{useBibliothequeDesPrix}")]
        public async Task<IHttpActionResult> CopieBudgetDansMemeCiAsync(int budgetId, bool useBibliothequeDesPrix)
        {
            //On récupère l'utilisateur connecté pour connaitre l'auteur de la modification du budget
            var nouveauBudget = await budgetCopieManager.CopierBudgetDansMemeCiAsync(budgetId, utilisateurConnecte.UtilisateurId, useBibliothequeDesPrix);
            var mappedBudget = Mapper.Map<ListeBudgetModel>(nouveauBudget);

            return Created(string.Empty, mappedBudget);
        }

        [HttpPut]
        [Route("api/Budget/Liste")]
        public IHttpActionResult SaveBudgetChangeInListView(ListeBudgetModel budgetListeModel)
        {
            var budgetUpdated = budgetManager.SaveBudgetChangeInListView(budgetListeModel);
            var mappedBudget = Mapper.Map<ListeBudgetModel>(budgetUpdated);

            return Ok(mappedBudget);
        }

        [HttpPost]
        [Route("api/Budget/Partage/{budgetId}")]
        public IHttpActionResult PartageOuDepartageBudget(int budgetId)
        {
            var partage = budgetManager.PartageOuPrivatiseBudget(budgetId);

            return Created(string.Empty, partage);
        }

        [HttpPost]
        [Route("api/Budget/{ciId}")]
        public IHttpActionResult CreateEmptyBudgetSurCi(int ciId)
        {
            //On récupère l'utilisateur connecté pour connaitre l'auteur de la modification du budget
            var nouveauBudget = budgetManager.CreateEmptyBudgetSurCi(ciId, utilisateurConnecte.UtilisateurId);
            var mappedBudget = Mapper.Map<ListeBudgetModel>(nouveauBudget);

            return Created(string.Empty, mappedBudget);
        }

        [HttpGet]
        [Route("api/Budget/LoadPeriodeRecalage/{ciId}")]
        public IHttpActionResult LoadPeriodeCloture(int ciId)
        {
            var periodeRecalage = budgetMainManager.LoadPeriodeRecalage(ciId);

            return Ok(periodeRecalage);
        }

        [HttpPost]
        [Route("api/Budget/RecalageBudgetaire/{budgetId}/{periodeFin}")]
        public async Task<IHttpActionResult> RecalageBudgetaireAsync(int budgetId, int periodeFin)
        {
            var recalage = await budgetMainManager.RecalageBudgetaireAsync(budgetId, utilisateurConnecte.UtilisateurId, periodeFin);

            return Created(string.Empty, recalage);
        }

        [HttpPost]
        [Route("api/Budget/UpdateDateDeleteNotificationNewTask/{budgetId}")]
        public IHttpActionResult UpdateDateDeleteNotificationNewTask(int budgetId)
        {
            budgetManager.UpdateDateDeleteNotificationNewTask(budgetId);

            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Created));
        }

        [HttpGet]
        [Route("api/Budget/Avancement")]
        public IHttpActionResult GetAvancement(int ciId, int periode)
        {
            var avancement = budgetMainManager.GetAvancement(ciId, periode);

            return Ok(avancement);
        }

        [HttpPut]
        [Route("api/Budget/Avancement/Excel")]
        public IHttpActionResult GetExportExcelAvancement(AvancementExcelLoadModel model)
        {
            try
            {
                byte[] excelBytes = avancementExportExcelManager.GetExportExcel(model);
                string typeCache = model.IsPdfConverted ? "pdfBytes_" : "excelBytes_";
                string cacheId = Guid.NewGuid().ToString();

                var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(300), Priority = CacheItemPriority.NotRemovable };
                MemoryCache.Default.Add(typeCache + cacheId, excelBytes, policy);

                return Ok(new { id = cacheId });
            }
            catch (Exception ex)
            {
                logger.Log(NLog.LogLevel.Error, ex);
                return null;
            }
        }

        [HttpPost]
        [Route("api/Budget/SaveAvancement")]
        public IHttpActionResult SaveAvancement(AvancementSaveModel model)
        {
            budgetMainManager.SaveAvancement(model);

            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Created));
        }

        [HttpPost]
        [Route("api/Budget/ValidateAvancement/{etatAvancement}")]
        public IHttpActionResult ValidateAvancement(AvancementSaveModel model, string etatAvancement)
        {
            budgetMainManager.ValidateAvancementModel(model, etatAvancement);

            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Created));
        }

        [HttpPost]
        [Route("api/Budget/RetourBrouillonAvancement")]
        public IHttpActionResult RetourBrouillonAvancement(AvancementSaveModel model)
        {
            budgetMainManager.RetourBrouillonAvancement(model);

            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Created));
        }

        [HttpGet]
        [Route("api/Budget/AvancementRecette")]
        public IHttpActionResult GetAvancementRecette(int ciId, int periode)
        {
            var avancementRecette = budgetMainManager.GetAvancementRecette(ciId, periode);

            return Ok(avancementRecette);
        }

        [HttpPost]
        [Route("api/Budget/SaveAvancementRecette")]
        public IHttpActionResult SaveAvancementRecette(AvancementRecetteSaveModel model)
        {
            var avancementRecette = budgetMainManager.SaveAvancementRecette(model);

            return Created(string.Empty, avancementRecette);
        }

        [HttpGet]
        [Route("api/Budget/GetBibliothequePrix/{organisationId}/{deviseId}")]
        public IHttpActionResult GetBibliothequePrix(int organisationId, int deviseId)
        {
            var deviseIdToUse = deviseId == 0 ? null : (int?)deviseId;
            var load = budgetBibliothequePrixManager.Load(organisationId, deviseIdToUse);

            return Ok(load);
        }

        [HttpGet]
        [Route("api/Budget/GetBibliothequePrixForCopy/{organisationId}/{deviseId}")]
        public IHttpActionResult GetBibliothequePrixForCopy(int organisationId, int deviseId)
        {
            var resultModel = budgetBibliothequePrixManager.LoadForCopy(organisationId, deviseId);

            return Ok(resultModel);
        }

        [HttpPost]
        [Route("api/Budget/SaveBibliothequePrix")]
        public IHttpActionResult SaveBibliothequePrix(BibliothequePrixSave.Model model)
        {
            var validator = new BibliothequePrixSaveModelValidator();
            var result = validator.Validate(model);
            if (!result.IsValid)
            {
                var message = string.Join(Environment.NewLine, result.Errors);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, message));
            }

            var resultModel = budgetBibliothequePrixManager.Save(model);

            return Created(string.Empty, resultModel);
        }

        [HttpPost]
        [Route("api/Budget/Brouilllon/BibliothequePrix")]
        public IHttpActionResult ApplyNewBibliothequePrixToBudgetBrouillon(ApplyBibliothequePrixBudgetsBrouillonsModel model)
        {
            budgetBibliothequePrixManager.ApplyNewBibliothequePrixToBudgetBrouillon(model);

            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Created));
        }

        [HttpGet]
        [Route("api/Budget/LoadBibliothequePrixItemHistorique/{organisationId}/{deviseId}/{ressourceId}")]
        public IHttpActionResult LoadBibliothequePrixItemHistorique(int organisationId, int deviseId, int ressourceId)
        {
            var resultModel = budgetBibliothequePrixManager.LoadItemHistorique(organisationId, deviseId, ressourceId);

            return Ok(resultModel);
        }

        [HttpGet]
        [Route("api/Budget/BibliothequePrixExists/{organisationId}/{deviseId}")]
        public IHttpActionResult BibliothequePrixExists(int organisationId, int deviseId)
        {
            var resultModel = budgetBibliothequePrixManager.Exists(organisationId, deviseId);

            return Ok(resultModel);
        }

        [HttpGet]
        [Route("api/Budget/Export/{id}/{isPdf}/{fileName}")]
        public IHttpActionResult GetExport(string id, bool isPdf, string fileName)
        {
            var cacheName = exportDocumentService.GetCacheName(id, isPdf: isPdf);
            var bytes = MemoryCache.Default.Get(cacheName) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheName);
            }
            var exportFilename = exportDocumentService.GetDocumentFileName(fileName, isPdf);
            return ResponseMessage(exportDocumentService.CreateResponseForDownloadDocument(exportFilename, bytes));
        }
    }
}
