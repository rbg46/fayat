using AutoMapper;
using Fred.Business.Common.ExportDocument;
using Fred.Business.FeatureFlipping;
using Fred.Business.Journal;
using Fred.Business.OperationDiverse;
using Fred.Business.Referential.Nature;
using Fred.Business.Societe;
using Fred.Framework.FeatureFlipping;
using Fred.Framework.Reporting;
using Fred.Web.Models;
using Fred.Web.Models.Societe;
using Fred.Web.Shared.Models.OperationDiverse;
using Fred.Web.Shared.Models.OperationDiverse.ExportExcel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Http;


namespace Fred.Web.API
{
    /// <summary>
    /// Controller WEB API des Famille OD
    /// </summary>
    public class FamilleOperationDiverseController : ApiControllerBase
    {
        private const string ExcelErreurTemplate = "Templates/FamilleOperationDiverse/TemplateFamilleOperationDiverseErreursParametrage.xlsx";
        private readonly IFamilleOperationDiverseManager familleOperationDiverseManager;
        private readonly IFamilleOperationDiverseExportExcelManager familleOperationDiverseExportExcelManager;
        private readonly IExportDocumentService exportDocumentService;
        private readonly ISocieteManager societeManager;
        private readonly IMapper mapper;
        private readonly IFeatureFlippingManager featureFlippingManager;
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="FamilleOperationDiverseController" />.
        /// </summary>
        /// <param name="mapper">Mapper Model / ModelVue</param>
        /// <param name="familleOperationDiverseManager">Gestionnaire des familles OD</param>
        /// <param name="exportDocumentService">Service d'export</param>
        /// <param name="societeManager">Gestionnaire des sociétés</param>
        /// <param name="familleOperationDiverseExportExcelManager">Manager du Excel des familles d'opération diverses</param>
        /// <param name="featureFlippingManager">Manager des features flipping</param>
        public FamilleOperationDiverseController(IMapper mapper, IFamilleOperationDiverseManager familleOperationDiverseManager, IExportDocumentService exportDocumentService, ISocieteManager societeManager, IFamilleOperationDiverseExportExcelManager familleOperationDiverseExportExcelManager, IFeatureFlippingManager featureFlippingManager)
        {
            this.mapper = mapper;
            this.familleOperationDiverseManager = familleOperationDiverseManager;
            this.familleOperationDiverseExportExcelManager = familleOperationDiverseExportExcelManager;
            this.societeManager = societeManager;
            this.exportDocumentService = exportDocumentService;
            this.featureFlippingManager = featureFlippingManager;
        }

        /// <summary>
        /// Méthode GET de recherche des familles OD
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Retourne la liste des familles OD</returns>
        [HttpPost]
        [Route("api/FamilleOperationDiverse/SearchAllBySocieteId/{societeId}")]
        public IHttpActionResult SearchAllBySocieteId(int societeId)
        {
            return Ok(familleOperationDiverseManager.GetFamiliesOdOrdered(societeId));
        }

        /// <summary>
        /// Méthode PUT de mise à jour des familles OD
        /// </summary>
        /// <param name="familleOperationDiverseModel">familleOperationDiverseModel</param>
        /// <returns>Retourne une réponse HTTP</returns>
        /// <exception cref="ValidationException">"Erreur de validation"</exception>
        [HttpPut]
        [Route("api/FamilleOperationDiverse/UpdateFamilleOperationDiverse")]
        public async Task<IHttpActionResult> UpdateAsync(FamilleOperationDiverseModel familleOperationDiverseModel)
        {
            try
            {
                await familleOperationDiverseManager.UpdateFamilleOperationDiverseAsync(familleOperationDiverseModel).ConfigureAwait(false);

                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            
        }

        /// <summary>
        /// Génère un fichier Excel des erreurs de paramétrage
        /// </summary>
        /// <param name="familleODErreur">Liste d'erreur de paramétrage</param>
        /// <returns>Le GUID du fichier Excel</returns>
        [HttpPost]
        [Route("api/FamilleOperationDiverse/GenerateExportExcelErreursParametrage")]
        public IHttpActionResult GenerateExportExcelErreursParametrage(List<ControleParametrageFamilleOperationDiverseModel> familleODErreur)
        {
            ExcelFormat excelFormat = new ExcelFormat();
            byte[] excelByte = null;

            List<FamilleOperationDiverseErreurExportModel> familleOdErreurExportModel = new List<FamilleOperationDiverseErreurExportModel>();
            familleODErreur.ForEach(a => familleOdErreurExportModel.Add(new FamilleOperationDiverseErreurExportModel()
            {
                Type = a.TypeFamilleOperationDiverse,
                Code = a.Code,
                Libelle = a.Libelle,
                Erreur = a.Erreur
            }));

            excelByte = excelFormat.GenerateExcel(AppDomain.CurrentDomain.BaseDirectory + ExcelErreurTemplate, familleOdErreurExportModel);


            string typeCache = "excelBytes_";
            string cacheId = Guid.NewGuid().ToString();

            CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(300), Priority = CacheItemPriority.NotRemovable };
            MemoryCache.Default.Add(typeCache + cacheId, excelByte, policy);

            return Ok(new { id = cacheId });
        }

        /// <summary>
        /// Télécharge l'export Excel des erreurs de paramétrage
        /// </summary>
        /// <param name="id">Identifiant du cache</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Fichier excel des erreurs de paramétrage</returns>
        [HttpGet]
        [Route("api/FamilleOperationDiverse/ExportErreursParametrage/{id}/{societeId}")]
        public IHttpActionResult GetExportExcelErreursParametrage(string id, int societeId)
        {
            string cacheName = exportDocumentService.GetCacheName(id, isPdf: false);
            byte[] bytes = MemoryCache.Default.Get(cacheName) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheName);
            }
            string exportFilename = exportDocumentService.GetDocumentFileName("Erreurs_Parametrage_" + societeId, false);

            HttpResponseMessage result = exportDocumentService.CreateResponseForDownloadDocument(exportFilename, bytes);

            return ResponseMessage(result);
        }

        /// <summary>
        /// Génère un fichier Excel des familles d'OD et journaux
        /// </summary>
        /// <param name="societeId">Identifiant de la societe</param>
        /// <param name="typeDonnee">Permet de savoir quel type de données on récupère</param>
        /// <returns>Le GUID du fichier Excel</returns>
        [HttpPost]
        [Route("api/FamilleOperationDiverse/GenerateExportExcelForJournal/{societeId}/{typeDonnee}")]
        public IHttpActionResult GenerateExportExcelForJournal(int societeId, string typeDonnee)
        {
            byte[] excelByte = null;

            excelByte = familleOperationDiverseExportExcelManager.GetExportExcelForJournal(societeId, typeDonnee);

            string typeCache = "excelBytes_";
            string cacheId = Guid.NewGuid().ToString();

            CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(300), Priority = CacheItemPriority.NotRemovable };
            MemoryCache.Default.Add(typeCache + cacheId, excelByte, policy);

            return Ok(new { id = cacheId });
        }

        /// <summary>
        /// Génère un fichier Excel des familles d'OD et natures
        /// </summary>
        /// <param name="societeId">Identifiant de la societe</param>
        /// <param name="typeDonnee">Permet de savoir quel type de données on récupère</param>
        /// <returns>Le GUID du fichier Excel</returns>
        [HttpPost]
        [Route("api/FamilleOperationDiverse/GenerateExportExcelForNature/{societeId}/{typeDonnee}")]
        public IHttpActionResult GenerateExportExcelForNature(int societeId, string typeDonnee)
        {
            byte[] excelByte = null;

            excelByte = familleOperationDiverseExportExcelManager.GetExportExcelForNature(societeId, typeDonnee);

            string typeCache = "excelBytes_";
            string cacheId = Guid.NewGuid().ToString();

            CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(300), Priority = CacheItemPriority.NotRemovable };
            MemoryCache.Default.Add(typeCache + cacheId, excelByte, policy);

            return Ok(new { id = cacheId });
        }

        /// <summary>
        /// Télécharge l'export Excel des familles d'OD
        /// </summary>
        /// <param name="id">Identifiant du cache</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="typeDonnee">Permet de savoir quel type de données on récupère</param>
        /// <returns>Fichier excel des familles OD et journaux</returns>
        [HttpGet]
        [Route("api/FamilleOperationDiverse/Export/{id}/{societeId}/{typeDonnee}")]
        public IHttpActionResult GetExportExcel(string id, int societeId, string typeDonnee)
        {
            string cacheName = exportDocumentService.GetCacheName(id, isPdf: false);
            byte[] bytes = MemoryCache.Default.Get(cacheName) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheName);
            }
            string exportFilename = exportDocumentService.GetDocumentFileName("Parametrage_" + typeDonnee + "_" + societeId, false);
            HttpResponseMessage result = exportDocumentService.CreateResponseForDownloadDocument(exportFilename, bytes);

            return ResponseMessage(result);
        }

        [HttpGet]
        [Route("api/FamilleOperationDiverse/Get/{societeId}")]
        public IHttpActionResult GetFamilleOperationDiverses(int societeId)
        {
            return Ok(mapper.Map<FamilleOperationDiverseModel>(familleOperationDiverseManager.GetFamiliesBySociety(societeId)));
        }

        /// <summary>
        /// Méthode de lancement du contrôle paramétrage pour les journaux
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Retourne une liste d'erreurs ou non</returns>
        [HttpPost]
        [Route("api/FamilleOperationDiverse/LaunchControleParametrageForJournal/{societeId}")]
        public IHttpActionResult LaunchControleParametrageForJournal(int societeId)
        {
            return Ok(familleOperationDiverseManager.LaunchControleParametrageForJournal(societeId));
        }

        /// <summary>
        /// Méthode de lancement du contrôle paramétrage pour les natures
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Retourne une liste d'erreurs ou non</returns>
        [HttpPost]
        [Route("api/FamilleOperationDiverse/LaunchControleParametrageForNature/{societeId}")]
        public IHttpActionResult LaunchControleParametrageForNature(int societeId)
        {
            return Ok(familleOperationDiverseManager.LaunchControleParametrageForNature(societeId));
        }

        /// <summary>
        /// Méthode de recherche d'une société
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Retourne une société</returns>
        [HttpPost]
        [Route("api/FamilleOperationDiverse/GetSociete/{societeId}")]
        public IHttpActionResult GetSociete(int societeId)
        {
            return Ok(mapper.Map<SocieteModel>(societeManager.GetSocieteById(societeId)));
        }

        /// <summary>
        /// Méthode pour enregistrer un parametrage familleOD, Nature et Journal
        /// </summary>
        /// <param name="fod">famille operation diverse</param>
        /// <returns>Retourne une réponse HTTP</returns>
        /// <exception cref="ValidationException">"Erreur de validation"</exception>
        [HttpPost]
        [Route("api/FamilleOperationDiverse/ParametrageNaturesJournaux")]
        public async Task<IHttpActionResult> SetParametrageNaturesJournaux(FamilleOperationDiverseModel fod)
        {
            if (featureFlippingManager.IsActivated(EnumFeatureFlipping.ActivationUS13085_6000))
            {
                return Ok(familleOperationDiverseManager.SetParametrageNaturesJournaux(fod));
            }
            return NotFound();
        }

        /// <summary>
        /// Méthode pour récuperer les parametrage familleOD, Nature et Journal pour une société
        /// </summary>
        /// <param name="societeId">l'identifiant de la societe</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpGet]
        [Route("api/FamilleOperationDiverse/ParametrageNaturesJournaux/{societeId}")]
        public async Task<IHttpActionResult> GetAllParametrageFamilleOperationDiverseNaturesJournaux(int societeId)
        {
            if (featureFlippingManager.IsActivated(EnumFeatureFlipping.ActivationUS13085_6000))
            {
                return Ok(familleOperationDiverseManager.GetAllParametrageFamilleOperationDiverseNaturesJournaux(societeId));
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/FamilleOperationDiverse/GetOdTypesByCompanyId/{societeId}")]
        public IHttpActionResult GetOdTypesByCompanyId(int societeId)
        {
            if (featureFlippingManager.IsActivated(EnumFeatureFlipping.ActivationUS13085_6000))
            {
                return Ok(mapper.Map<IEnumerable<TypeOdFilterExplorateurDepense>>(familleOperationDiverseManager.GetTypeOdFilter(societeId)));
            }

            return NotFound();
        }
    }
}
