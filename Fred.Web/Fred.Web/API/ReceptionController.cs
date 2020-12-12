using AutoMapper;
using Fred.Business;
using Fred.Business.Commande;
using Fred.Business.Common.ExportDocument;
using Fred.Business.DatesClotureComptable;
using Fred.Business.ExternalService;
using Fred.Business.ExternalService.CommandeLigne;
using Fred.Business.Reception;
using Fred.Business.Reception.Services;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Web.Models.Depense;
using Fred.Web.Shared.Models;
using Fred.Web.Shared.Models.Commande;
using Fred.Web.Shared.Models.Reception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fred.Web.API
{
    public class ReceptionController : ApiControllerBase
    {

        private readonly ISearchCommandeService searchCommandeService;
        private readonly IReceptionManager receptionManager;
        private readonly IReceptionManagerExterne receptionManagerExterne;
        private readonly IExportDocumentService exportDocumentService;
        private readonly ICommandeLigneManager commandeLigneManager;
        private readonly IDatesClotureComptableManager datesClotureComptableManager;
        private readonly IReceptionExportExcelService receptionExportExcelService;
        private readonly ISearchReceptionsService searchReceptionsService;
        private readonly IReceptionUpdateOrchestrator receptionUpdateOrchestrator;
        private readonly ICommandeLigneManagerExterne commandeLigneManagerExterne;
        private readonly IMapper mapper;

        public ReceptionController(ISearchCommandeService searchCommandeService,
            IReceptionManager receptionManager,
            IReceptionManagerExterne receptionManagerExterne,
            IExportDocumentService exportDocumentService,
            ICommandeLigneManager commandeLigneManager,
            IDatesClotureComptableManager datesClotureComptableManager,
            IReceptionExportExcelService receptionExportExcelService,
            ISearchReceptionsService searchReceptionsService,
            IReceptionUpdateOrchestrator receptionUpdateOrchestrator,
            ICommandeLigneManagerExterne commandeLigneManagerExterne,
            IMapper mapper)
        {
            this.searchCommandeService = searchCommandeService;
            this.receptionManager = receptionManager;
            this.receptionManagerExterne = receptionManagerExterne;
            this.exportDocumentService = exportDocumentService;
            this.commandeLigneManager = commandeLigneManager;
            this.datesClotureComptableManager = datesClotureComptableManager;
            this.receptionExportExcelService = receptionExportExcelService;
            this.searchReceptionsService = searchReceptionsService;
            this.receptionUpdateOrchestrator = receptionUpdateOrchestrator;
            this.commandeLigneManagerExterne = commandeLigneManagerExterne;
            this.mapper = mapper;
        }


        /// <summary>
        ///     Récupération de la liste des commandes à réceptions selon les filtres
        /// </summary>
        /// <param name="commandeFilter">Filtre</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Liste des commandes à réceptionner</returns>
        [HttpPost]
        [Route("api/Reception/GetCommandesToReceive/{page?}/{pageSize?}")]
        public IHttpActionResult SearchReceptionnable(SearchReceivableOrdersFilterModel commandeFilter, int page = 1, int pageSize = 20)
        {
            var filter = mapper.Map<SearchReceivableOrdersFilter>(commandeFilter);

            var result = mapper.Map<SearchReceptionnableResultModel>(searchCommandeService.SearchReceivableOrders(filter, page, pageSize));

            return Ok(result);
        }

        /// <summary>
        /// Genere un fichier excel pour l'ecran reception
        /// </summary>    
        /// <param name="commandeFilter">Filtre</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Reception/GenerateExcel/")]
        public IHttpActionResult GenerateExcel(SearchReceivableOrdersFilterModel commandeFilter)
        {

            var filter = mapper.Map<SearchReceivableOrdersFilter>(commandeFilter);

            //retrieve
            SearchReceptionnableResult searchCommandesReceptionnableResult = searchCommandeService.SearchReceivableOrders(filter, 1, int.MaxValue);

            // transform
            IEnumerable<DepenseAchatEnt> depenses = receptionExportExcelService.TransformCommandesToDepensesForExport(searchCommandesReceptionnableResult.Commandes);
            var result = ReceptionExport.ToReceptionExportModel(depenses);

            var cacheExcelId = receptionExportExcelService.CustomizeExcelFileForExport("/Templates/TemplateReceptions.xls", result);
            var id = new { id = cacheExcelId };
            return Ok(id);

        }

        /// <summary>
        ///   Export des reception vers SAP
        /// </summary>
        /// <param name="receptionIds">Liste des identifiants des réceptions</param>
        /// <returns>Vrai si export lancé sinon faux</returns>
        [HttpPost]
        [Route("api/Reception/ExportReception/")]
        public HttpResponseMessage ExportReceptionListToSap(IEnumerable<int> receptionIds)
        {
            return Post(() => receptionManagerExterne.ExportReceptionListToSapAsync(receptionIds));
        }

        /// <summary>
        ///   Récupération d'un nouveau filtre de Réception
        /// </summary>
        /// <returns>Filtre Réception</returns>
        [HttpGet]
        [Route("api/Reception/Filter/")]
        public IHttpActionResult GetNewFilter()
        {
            // ECRAN TABLEAU DES RECEPTIONS
            var filter = mapper.Map<SearchDepenseModel>(receptionManager.GetNewFilter());
            return Ok(filter);
        }

        /// <summary>
        ///   Récupération de la liste des réceptions selon un filtre
        /// </summary>
        /// <param name="filtre">Filtre des réceptions</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Retourne la liste des Dépenses Achat de type Réception</returns>
        [HttpPost]
        [Route("api/Reception/Search/{page?}/{pageSize?}")]
        public HttpResponseMessage SearchReceptions(SearchDepenseModel filtre, int page = 1, int pageSize = 20)
        {
            // ECRAN TABLEAU DES RECEPTIONS
            return Post(() => mapper.Map<TableauReceptionResultModel>(this.searchReceptionsService.SearchReceptionsWithTotals(mapper.Map<SearchDepenseEnt>(filtre), page, pageSize)));
        }

        [HttpPost]
        [Route("api/Reception/SearchNextReceptions")]
        public HttpResponseMessage SearchNextReceptions(SearchNextReceptionModel model)
        {
            // ECRAN TABLEAU DES RECEPTIONS
            return Post(() => mapper.Map<List<DepenseAchatModel>>(this.searchReceptionsService.SearchReceptionsByIds(model.DateFrom, model.DateTo, model.ReceptionIds)));
        }

        /// <summary>
        ///   Mise à jour d'une liste de réceptions
        /// </summary>
        /// <param name="receptions">Liste de réceptions</param>
        /// <returns>Vrai si toutes les réceptions ont été enregistrées, sinon faux</returns>
        [HttpPut]
        [Route("api/Reception/UpdateReceptions")]
        public HttpResponseMessage UpdateReceptions(List<DepenseAchatModel> receptions)
        {
            return Put(() =>
            {
                var mappedReceptions = mapper.Map<List<DepenseAchatEnt>>(receptions);

                receptionUpdateOrchestrator.UpdateReceptions(mappedReceptions);

                return receptions.Select(x => x.DepenseId).ToList();
            });
        }

        /// <summary>
        ///   Génération du fichier Excel et mise en cache
        /// </summary>
        /// <param name="filtre">Filtre utilisé</param>
        /// <returns>Document Excel</returns>
        [HttpPost]
        [Route("api/Reception/Export/")]
        public object GenerateExportReceptionsExcel(SearchDepenseModel filtre)
        {
            try
            {
                byte[] bytes = this.receptionExportExcelService.GetReceptionsExcel(mapper.Map<SearchDepenseEnt>(filtre));
                const string typeCache = "excelBytes_";
                string cacheId = Guid.NewGuid().ToString();

                CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(300), Priority = CacheItemPriority.NotRemovable };
                MemoryCache.Default.Add(typeCache + cacheId, bytes, policy);

                return new { id = cacheId };
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        ///   Télécharge l'export Excel des réceptions
        /// </summary>
        /// <param name="id">Identifiant du cache</param>
        /// <returns>Fichier excel des réceptions</returns>
        [HttpGet]
        [Route("api/Reception/Export/{id}")]
        public HttpResponseMessage GetExportReceptionsExcel(string id)
        {
            var cacheName = exportDocumentService.GetCacheName(id, isPdf: false);
            var bytes = MemoryCache.Default.Get(cacheName) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheName);
            }
            var exportFilename = exportDocumentService.GetDocumentFileName(receptionExportExcelService.GetReceptionsFilename(), false);
            return exportDocumentService.CreateResponseForDownloadDocument(exportFilename, bytes);
        }

        /// <summary>
        ///   Récupère une nouvelle Dépense Achat de type Réception
        /// </summary>
        /// <param name="commandeLigneId">Ligne de commande à réceptionner</param>
        /// <returns>Une nouvelle réception</returns>
        [HttpGet]
        [Route("api/Reception/New/{commandeLigneId}")]
        public HttpResponseMessage GetNewReception(int commandeLigneId)
        {
            // ECRAN COMMANDE A RECEPTIONNER
            return Get(() => mapper.Map<DepenseAchatModel>(receptionManager.GetNew(commandeLigneId)));
        }

        /// <summary>
        ///   Création d'une réception
        /// </summary>
        /// <param name="reception">Depense à ajouter</param>
        /// <returns>La réception créée</returns>       
        [HttpPost]
        [Route("api/Reception/")]
        public IHttpActionResult AddReception(DepenseAchatModel reception)
        {
            // ECRAN COMMANDE A RECEPTIONNER
            var addAndValidateResult = receptionManager.AddAndValidate(mapper.Map<DepenseAchatEnt>(reception));

            var result = mapper.Map<DepenseAchatModel>(addAndValidateResult);

            return Ok(result);
        }

        /// <summary>
        ///   Mise à jour d'une réception
        /// </summary>    
        /// <param name="reception">réception à traiter</param>
        /// <returns>La réception mise à jour</returns>
        [HttpPut]
        [Route("api/Reception/")]
        public IHttpActionResult UpdateReception(DepenseAchatModel reception)
        {
            // ECRAN COMMANDE A RECEPTIONNER
            var mappedReception = mapper.Map<DepenseAchatEnt>(reception);

            DepenseAchatEnt dbReception = receptionUpdateOrchestrator.UpdateReception(mappedReception);

            var result = mapper.Map<DepenseAchatModel>(dbReception);

            return Ok(result);
        }

        /// <summary>
        ///   Suppression d'une réception
        /// </summary>
        /// <param name="receptionId">Identifiant de la réception</param>
        /// <returns>La réception supprimée</returns>
        [HttpPut]
        [Route("api/Reception/{receptionId}")]
        public IHttpActionResult DeleteReception(int receptionId)
        {
            // ECRAN COMMANDE A RECEPTIONNER
            this.receptionManager.Delete(receptionId);

            return Ok();
        }



        /// <summary>
        ///   Récupère une copie de la réception
        /// </summary>
        /// <param name="commandeLigneId">Identifiant de la ligne de commande</param>
        /// <param name="receptionId">Identifiant de la réception</param>
        /// <returns>Réception copiée</returns>
        [HttpGet]
        [Route("api/Reception/Duplicate/{commandeLigneId}/{receptionId}")]
        public IHttpActionResult DuplicateReception(int commandeLigneId, int receptionId)
        {
            // ECRAN COMMANDE A RECEPTIONNER

            var result = mapper.Map<DepenseAchatModel>(receptionManager.Duplicate(commandeLigneId, receptionId));

            return Ok(result);
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////  VERFICATION SI UNE OU PLUSIEURS RECEPTIONS SONT 'BLOQUÉES EN RECEPTION' ///////////////////////////////////////      
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Verification si une reception OU PLUSIEURS est bloqué en reception

        /// <summary>
        ///   Vérifie si une réception possède une date dont la période est bloquée en réception ou non
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="period">Periode à vérifier</param>
        /// <returns>Vrai s'il existe une réception bloquée, sinon faux</returns>       
        [HttpGet]
        [Route("api/Reception/IsCiBlockedInReception/{ciId}/{period:datetime}")]
        public HttpResponseMessage IsCiBlockedInReception(int ciId, DateTime period)
        {
            // ECRAN COMMANDE A RECEPTIONNER
            return Get(() => datesClotureComptableManager.IsBlockedInReception(ciId, period.Year, period.Month));
        }

        /// <summary>
        ///   Vérifie si au moins une réception possède une date dont la période est bloquée en réception ou non
        /// </summary>
        /// <param name="receptionIds">Liste de réceptions à vérifier</param>
        /// <returns>Vrai s'il existe une réception bloquée, sinon faux</returns>       
        [HttpPost]
        [Route("api/Reception/IsBlockedInReception")]
        public HttpResponseMessage IsBlockedInReception(List<int> receptionIds)
        {
            // ECRAN COMMANDE A RECEPTIONNER
            return Post(() =>
            {
                return this.receptionManager.CheckAnyReceptionsIsBlocked(receptionIds);
            });

        }

        #endregion

        /// <summary>
        ///   Viser les réceptions
        /// </summary>
        /// <param name="receptionIds">Liste de réceptions ids</param>
        /// <returns>Vrai si toutes les réceptions ont été enregistrées et visées, sinon faux</returns>
        [HttpPost]
        [Route("api/Reception/StampReceptionsByIds")]
        public async Task<IHttpActionResult> StampReceptionsByIds(List<int> receptionIds)
        {
            await receptionManager.ViserReceptionsAsync(receptionIds,
                     callFredIeAndSetHangfireJobIdAction: (receptions) => receptionManagerExterne.ExportReceptionListToSapAsync(receptions));

            return Ok();
        }

        [HttpPut]
        [Route("api/Reception/CommandeLigne/{commandeLigneId}/Lock")]
        public async Task<IHttpActionResult> LockAsync(int commandeLigneId)
        {
            await commandeLigneManagerExterne.ExportManualLockLigneDeCommandeToSap(commandeLigneId,
                 onLockOnFredWeb: () => commandeLigneManager.LockAsync(commandeLigneId));

            return Ok();
        }

        [HttpPut]
        [Route("api/Reception/CommandeLigne/{commandeLigneId}/UnLock")]
        public async Task<IHttpActionResult> UnLockAsync(int commandeLigneId)
        {
            await commandeLigneManagerExterne.ExportManualUnlockLigneDeCommandeToSap(commandeLigneId,
                onUnLockOnFredWeb: () => commandeLigneManager.UnlockAsync(commandeLigneId));

            return Ok();
        }

        [HttpPut]
        [Route("api/Reception/CommandeLigne/{commandeLigneId}/IsLocked")]
        public async Task<IHttpActionResult> IsLocked(int commandeLigneId)
        {
            return Ok(await commandeLigneManager.IsCommandeLigneLockedAsync(commandeLigneId));
        }

    }
}
