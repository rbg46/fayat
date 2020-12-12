using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Fred.Business.Common.ExportDocument;
using Fred.Business.ObjectifFlash;
using Fred.Business.ObjectifFlash.Reporting;
using Fred.Entities.ObjectifFlash;
using Fred.Entities.ObjectifFlash.Search;
using Fred.Web.Models.ObjectifFlash;
using Fred.Web.Models.Rapport;
using Fred.Web.Shared.Models.ObjectifFlash.Panel;
using Fred.Web.Shared.Models.ObjectifFlash.Search;

namespace Fred.Web.API
{
    public class ObjectifFlashController : ApiControllerBase
    {
        protected readonly IMapper mapper;
        private readonly IObjectifFlashManager objectifFlashManager;
        private readonly IObjectifFlashBudgetManager objectifFlashBudgetManager;
        private readonly IObjectifFlashTacheManager objectifFlashTacheManager;
        private readonly IBilanFlashExportManager bilanFlashExportManager;
        private readonly IBilanFlashSyntheseExportManager bilanFlashSyntheseExportManager;
        private readonly IExportDocumentService exportDocumentService;

        public ObjectifFlashController(
            IMapper mapper,
            IObjectifFlashManager objectifFlashManager,
            IObjectifFlashBudgetManager objectifFlashBudgetManager,
            IObjectifFlashTacheManager objectifFlashTacheManager,
            IBilanFlashExportManager bilanFlashExportManager,
            IBilanFlashSyntheseExportManager bilanFlashSyntheseExportManager,
            IExportDocumentService exportDocumentService)
        {
            this.mapper = mapper;
            this.objectifFlashManager = objectifFlashManager;
            this.objectifFlashBudgetManager = objectifFlashBudgetManager;
            this.objectifFlashTacheManager = objectifFlashTacheManager;
            this.bilanFlashExportManager = bilanFlashExportManager;
            this.bilanFlashSyntheseExportManager = bilanFlashSyntheseExportManager;
            this.exportDocumentService = exportDocumentService;
        }

        /// <summary>
        /// GET Objectif flash by Id
        /// </summary>
        /// <param name="objectifFlashId">Objectif flash id</param>
        /// <returns>Objectif flash</returns>
        [HttpGet]
        [Route("api/ObjectifFlash/{objectifFlashId?}")]
        public HttpResponseMessage GetObjectifFlashById(int objectifFlashId)
        {
            return Get(() => mapper.Map<ObjectifFlashModel>(this.objectifFlashManager.GetObjectifFlashWithTachesById(objectifFlashId)));
        }

        /// <summary>
        /// GET new objectif flash
        /// </summary>
        /// <returns>Objectif flash</returns>
        [HttpGet]
        [Route("api/ObjectifFlash/New")]
        public HttpResponseMessage GetNewObjectifFlash()
        {
            return Get(() => mapper.Map<ObjectifFlashModel>(this.objectifFlashManager.GetNewObjectifFlash()));
        }

        /// <summary>
        /// POST Ajout d'un objectif flash en base
        /// </summary>
        /// <param name="objectifFlashModel">Commande à traiter</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/ObjectifFlash")]
        public HttpResponseMessage AddObjectifFlash(ObjectifFlashModel objectifFlashModel)
        {
            return this.Post(() => mapper.Map<ObjectifFlashModel>(this.objectifFlashManager.AddObjectifFlash(mapper.Map<ObjectifFlashEnt>(objectifFlashModel))));
        }

        /// <summary>
        /// PUT Udpate d'un Objectif Flash
        /// </summary>
        /// <param name="objectifFlashModel">Objectif Flash à mettre à jour</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPut]
        [Route("api/ObjectifFlash")]
        public HttpResponseMessage UpdateObjectifFlash(ObjectifFlashModel objectifFlashModel)
        {
            return this.Put(() => mapper.Map<ObjectifFlashModel>(this.objectifFlashManager.UpdateObjectifFlash(mapper.Map<ObjectifFlashEnt>(objectifFlashModel))));
        }

        /// <summary>
        /// PUT Activation d'un Objectif Flash
        /// </summary>
        /// <param name="objectifFlashId">identifiant de l'objectif flash à activer</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPut]
        [Route("api/ObjectifFlash/Activate/{objectifFlashId}")]
        public HttpResponseMessage ActivateObjectifFlash(int objectifFlashId)
        {
            return this.Put(() => this.objectifFlashManager.ActivateObjectifFlash(objectifFlashId));
        }

        /// <summary>
        /// POST Duplication d'un Objectif Flash
        /// </summary>
        /// <param name="objectifFlashId">objectif flash id</param>
        /// <param name="dateDebut">date de début de l'objectif flash dupliqué</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/ObjectifFlash/Duplicate/{objectifFlashId}/{dateDebut}")]
        public HttpResponseMessage DuplicateObjectifFlash(int objectifFlashId, DateTime dateDebut)
        {
            return this.Post(() => this.objectifFlashManager.DuplicateObjectifFlash(objectifFlashId, dateDebut));
        }
        /// <summary>
        /// PUT Suppression de l'Objectif Flash
        /// </summary>
        /// <param name="objectifFlashId">Objectif Flash identifier</param>
        /// <returns>Http response</returns>
        [HttpPut]
        [Route("api/ObjectifFlash/DeleteObjectifFlash/{objectifFlashId}")]
        public HttpResponseMessage DeleteObjectifFlash(int objectifFlashId)
        {
            return this.Put(() => { this.objectifFlashManager.DeleteObjectifFlashById(objectifFlashId); return true; });
        }

        /// <summary>
        /// GET Récupère les ressources liée à une tache dans un budget en application pour l'identifiant de ci donné 
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="tacheId">Tache identifier</param>
        /// <returns>Http response</returns>
        [HttpGet]
        [Route("api/ObjectifFlash/GetRessourcesInBudgetEnApplicationByCiId/{ciId}/{tacheId}")]
        public HttpResponseMessage GetRessourcesInBudgetEnApplicationByCiId(int ciId, int tacheId)
        {
            return this.Get(() => mapper.Map<List<ChapitrePanelModel>>(this.objectifFlashBudgetManager.GetRessourcesInBudgetEnApplicationByCiId(ciId, tacheId)));
        }

        /// <summary>
        /// GET Récupère les ressources liée à une tache dans un budget en application pour l'identifiant de ci donné 
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="deviseId">Devise identifier</param>
        /// <param name="filter">filtre sur les libellés</param>
        /// <param name="page">numéro de page à charger</param>
        /// <param name="pageSize">Nombre d'éléments par page</param>
        /// <returns>Http response</returns>
        [HttpGet]
        [Route("api/ObjectifFlash/GetRessourcesInBibliothequePrix")]
        public HttpResponseMessage GetRessourcesInBibliothequePrix(int ciId, int deviseId, string filter = "", int page = 1, int pageSize = 25)
        {
            return this.Get(() => mapper.Map<List<ChapitrePanelModel>>(this.objectifFlashBudgetManager.GetRessourcesInBibliothequePrix(ciId, deviseId, filter, page, pageSize)));
        }

        /// <summary>
        /// GET Récupère les ressources liée à une tache dans un budget en application pour l'identifiant de ci donné 
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="periode">periode basé sur la date de début de l'objectif flash</param>
        /// <returns>Http response</returns>
        [HttpGet]
        [Route("api/ObjectifFlash/GetRessourcesInBaremeExploitation")]
        public HttpResponseMessage GetRessourcesInBaremeExploitation(int ciId, DateTime periode)
        {
            return this.Get(() => mapper.Map<List<ChapitrePanelModel>>(this.objectifFlashBudgetManager.GetRessourcesInBaremeExploitation(ciId, periode)));
        }

        /// <summary>
        ///   GET Nouveau filtre d'objectif flash
        /// </summary>
        /// <returns>Objet de recherche des objectifs flash</returns>
        [HttpGet]
        [Route("api/ObjectifFlash/Filter")]
        public HttpResponseMessage GetNewObjectifFlashSearch()
        {
            return Get(() => mapper.Map<SearchObjectifFlashModel>(this.objectifFlashManager.GetNewFilter()));
        }

        /// <summary>
        /// GET Recherche d'objectif flash paginée
        /// </summary>
        /// <param name="filter">Objet de recherche</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Liste des objectifs flash correspondant aux critères de recherche</returns>
        [HttpPost]
        [Route("api/ObjectifFlash/Search/{page?}/{pageSize?}")]
        public async Task<IHttpActionResult> SearchAsync(SearchObjectifFlashModel filter, int page = 1, int pageSize = 30)
        {
            return Ok(mapper.Map<SearchObjectifFlashListWithFilterResultModel>(await objectifFlashManager.SearchObjectifFlashListWithFilterAsync(mapper.Map<SearchObjectifFlashEnt>(filter), page, pageSize).ConfigureAwait(false)));
        }

        /// <summary>
        /// Retourne un Objectif flash initialisé avec une nouvelle journalisation ventilée.
        /// </summary>
        /// <param name="objectifFlashModel">Objectif Flash à journaliser</param>
        /// <returns>ObjectifFlashModel</returns>
        [HttpPost]
        [Route("api/ObjectifFlash/NewJournalisation")]
        public HttpResponseMessage NewJournalisation(ObjectifFlashModel objectifFlashModel)
        {
            return Post(() => mapper.Map<ObjectifFlashModel>(objectifFlashManager.GetNewJournalisation(mapper.Map<ObjectifFlashEnt>(objectifFlashModel))));
        }

        /// <summary>
        /// Reporte les journalisations de l'objectif flash à partir de la date de début indiquée.
        /// </summary>
        /// <param name="dateDebut">date de début de journalisation</param>
        /// <param name="objectifFlashModel">Objectif Flash à journaliser</param>
        /// <returns>ObjectifFlashModel</returns>
        [HttpPost]
        [Route("api/ObjectifFlash/ReportJournalisation/{dateDebut}")]
        public HttpResponseMessage ReportJournalisation(DateTime dateDebut, ObjectifFlashModel objectifFlashModel)
        {
            return Post(() => mapper.Map<ObjectifFlashModel>(objectifFlashManager.GetReportJournalisation(mapper.Map<ObjectifFlashEnt>(objectifFlashModel), dateDebut)));
        }

        /// <summary>
        /// Retourne les objectifs flash liés au rapport avec les taches filtrées correspondant à celles du rapport.
        /// </summary>
        /// <param name="rapport">rapport</param>
        /// <returns>list d'objectif flash</returns>
        [HttpPost]
        [Route("api/ObjectifFlash/GetObjectifFlashRapport")]
        public HttpResponseMessage GetObjectifFlashRapport(RapportModel rapport)
        {
            return Post(() => mapper.Map<List<ObjectifFlashModel>>(objectifFlashTacheManager.GetObjectifFlashListByDateCiIdAndTacheIds(rapport.DateChantier, rapport.CiId, rapport.RapportId, rapport.ListTaches.Select(x => x.TacheId).ToList())));
        }


        /// <summary>
        /// Update des taches réalisées d'un rapport
        /// </summary>
        /// <param name="rapportId">identifiant du rapport</param>
        /// <param name="tacheRapportRealises">liste des taches réalisées</param>
        /// <returns>liste des taches réalisées modifiées</returns>
        [HttpPost]
        [Route("api/ObjectifFlash/UpdateObjectifFlashTacheRapportRealise/{rapportId}")]
        public HttpResponseMessage UpdateTacheRapportRealise(int rapportId, List<ObjectifFlashTacheRapportRealiseModel> tacheRapportRealises)
        {
            return Post(() => mapper.Map<List<ObjectifFlashTacheRapportRealiseModel>>(objectifFlashTacheManager.UpdateObjectifFlashTacheRapportRealise(rapportId, mapper.Map<List<ObjectifFlashTacheRapportRealiseEnt>>(tacheRapportRealises))));
        }

        /// <summary>
        /// Génération d'un export de bilan flash et mise en cache
        /// </summary>
        /// <param name="objectifFlashId">Identifiant d'objectif flash</param>
        /// <param name="startDate">date de début</param>
        /// <param name="endDate">date de fin</param>
        /// <param name="isPdfConverted">flag de conversion pdf</param>
        /// <returns>identifiant de cache</returns>
        [HttpGet]
        [Route("api/ObjectifFlash/ExportBilanFlash/{objectifFlashId?}/{startDate?}/{endDate?}/{isPdfConverted?}")]
        public async Task<IHttpActionResult> ExportBilanFlashAsync(int? objectifFlashId, DateTime? startDate, DateTime? endDate, bool isPdfConverted)
        {
            object exportBilanFlash = await bilanFlashExportManager.ExportBilanFlashAsync(objectifFlashId, startDate, endDate, isPdfConverted).ConfigureAwait(false);
            return Ok(exportBilanFlash);
        }


        /// <summary>
        /// Génération d'un export synthèse de bilan flash et mise en cache
        /// </summary>
        /// <param name="objectifFlashId">Identifiant d'objectif flash</param>
        /// <param name="startDate">date de début</param>
        /// <param name="endDate">date de fin</param>
        /// <param name="isPdfConverted">flag de conversion pdf</param>
        /// <returns>identifiant de cache</returns>
        [HttpGet]
        [Route("api/ObjectifFlash/ExportBilanFlashSynthese/{objectifFlashId?}/{startDate?}/{endDate?}/{isPdfConverted?}")]
        public async Task<IHttpActionResult> ExportBilanFlashSyntheseAsync(int? objectifFlashId, DateTime? startDate, DateTime? endDate, bool isPdfConverted)
        {
            object exportBilanFlashSynthese = await bilanFlashSyntheseExportManager.ExportBilanFlashSyntheseAsync(objectifFlashId, startDate, endDate, isPdfConverted).ConfigureAwait(false);
            return Ok(exportBilanFlashSynthese);
        }

        /// <summary>
        /// Download de l'export généré
        /// </summary>
        /// <param name="cacheId">identifiant de cache</param>
        /// <param name="exportFileName">nom du fichier</param>
        /// <returns>tableau d'octets</returns>
        [HttpGet]
        [Route("api/ObjectifFlash/ExportBilanFlashDownload/{cacheId}/{exportFileName}")]
        public HttpResponseMessage ExportBilanFlashDownload(string cacheId, string exportFileName)
        {
            byte[] bytes = MemoryCache.Default.Get(cacheId) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheId);
            }
            return exportDocumentService.CreateResponseForDownloadDocument(exportFileName, bytes);
        }
    }
}
