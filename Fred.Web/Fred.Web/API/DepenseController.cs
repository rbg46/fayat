using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Fred.Business.CI;
using Fred.Business.Common.ExportDocument;
using Fred.Business.Depense;
using Fred.Business.Depense.Services;
using Fred.Business.ExplorateurDepense;
using Fred.Entities.Depense;
using Fred.Framework.Reporting;
using Fred.Web.Models;
using Fred.Web.Models.Commande;
using Fred.Web.Models.Depense;
using Fred.Web.Shared.Models;

namespace Fred.Web.API
{
    /// <summary>
    /// Représente une instance de contrôleur Web API pour les dépenses.
    /// <seealso cref="ApiController" />
    /// </summary>
    public class DepenseController : ApiControllerBase
    {
        private readonly IDepenseManager depenseManager;
        private readonly IMapper mapper;
        private readonly IExplorateurDepenseManager explorateurDepenseManager;
        private readonly IExportDocumentService exportDocumentService;
        private readonly ICIManager cIManager;
        private readonly IDepenseServiceMediator depenseServiceMediator;
        private readonly IExplorateurDepenseManagerFayatTP explorateurDepenseManagerFayatTP;

        public DepenseController(IDepenseManager depenseMgr, IMapper mapper, IExplorateurDepenseManager explorateurDepenseManager, IExportDocumentService exportDocumentService, ICIManager cIManager, IDepenseServiceMediator depenseServiceMediator, IExplorateurDepenseManagerFayatTP explorateurDepenseManagerFayatTP)
        {
            depenseManager = depenseMgr;
            this.mapper = mapper;
            this.explorateurDepenseManager = explorateurDepenseManager;
            this.exportDocumentService = exportDocumentService;
            this.cIManager = cIManager;
            this.depenseServiceMediator = depenseServiceMediator;
            this.explorateurDepenseManagerFayatTP = explorateurDepenseManagerFayatTP;
        }


        /// <summary>
        /// GET Récupération des dépenses
        /// </summary>
        /// <returns>Liste de dépenses</returns>
        [HttpGet]
        [Route("api/Depense/")]
        public IHttpActionResult Get()
        {
            return Ok(mapper.Map<IEnumerable<DepenseAchatModel>>(depenseManager.GetDepenseList()));
        }

        /// <summary>
        /// GET Récupère la liste des CI d'une organisation
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation</param>
        /// <returns>Liste des CI</returns>
        [HttpGet]
        [Route("api/Depense/GetCiIdList/{organisationId}")]
        public IHttpActionResult GetCiIdList(int organisationId)
        {
            return Ok(cIManager.GetAllCIbyOrganisation(organisationId));
        }

        /// <summary>
        /// POST Recherche de Dépense en fonction des filtres
        /// </summary>
        /// <param name="filter">Filtre dépense</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Liste de dépenses triées et filtrées</returns>
        [HttpPost]
        [Route("api/Depense/SearchWithFilters/{page?}/{pageSize}/")]
        public IHttpActionResult GetList(SearchDepenseModel filter, int page = 1, int pageSize = 8)
        {
            return Ok(mapper.Map<IEnumerable<DepenseAchatModel>>(depenseManager.SearchDepenseListWithFilter(mapper.Map<SearchDepenseEnt>(filter), page, pageSize)));
        }

        /// <summary>
        /// POST  Calcul du montant total des dépenses selon un filtre
        /// </summary>
        /// <param name="filter">Filtre dépense</param>
        /// <returns>Montant total de la liste de dépenses triées et filtrées</returns>
        [HttpPost]
        [Route("api/Depense/MontantTotal")]
        public IHttpActionResult GetMontantTotalDepense(SearchDepenseModel filter)
        {
            return Ok(depenseManager.GetMontantTotal(mapper.Map<SearchDepenseEnt>(filter)));
        }

        /// <summary>
        /// POST Création d'une dépense
        /// </summary>
        /// <param name="depenseModel">Depense à ajouter</param>
        /// <returns>Retourne une réponse HTTP</returns>       
        [HttpPost]
        [Route("api/Depense/")]
        public IHttpActionResult AddDepense(DepenseAchatModel depenseModel)
        {
            return Ok(mapper.Map<DepenseAchatModel>(depenseManager.AddDepense(mapper.Map<DepenseAchatEnt>(depenseModel))));
        }

        /// <summary>
        /// PUT Mise à jour d'une dépense
        /// </summary>    
        /// <param name="depenseModel">dépense à traiter</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPut]
        [Route("api/Depense/")]
        public HttpResponseMessage UpdateDepense(DepenseAchatModel depenseModel)
        {
            return Put(() => depenseManager.UpdateDepense(mapper.Map<DepenseAchatEnt>(depenseModel)));
        }

        /// <summary>
        /// DELETE Suppression d'une dépense
        /// </summary>
        /// <param name="depenseId">Identifiant de la dépense</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpDelete]
        [Route("api/Depense/{depenseId}")]
        public HttpResponseMessage DeleteDepense(int depenseId)
        {
            return Delete(() => depenseManager.DeleteDepenseById(depenseId));
        }

        /// <summary>
        /// GET Récupère une nouvelle dépense
        /// </summary>
        /// <param name="commandeLigneId">Ligne de commande à réceptionner</param>
        /// <returns>Une nouvelle réception</returns>
        [HttpGet]
        [Route("api/Depense/New/{commandeLigneId}")]
        public IHttpActionResult GetNewReception(int commandeLigneId)
        {
            return Ok(mapper.Map<DepenseAchatModel>(depenseManager.GetNewDepense(commandeLigneId)));
        }

        /// <summary>
        /// GET Récupère un nouveau filtre dépense
        /// </summary>
        /// <returns>Objet de recherche des dépenses</returns>
        [HttpGet]
        [Route("api/Depense/Filter")]
        public IHttpActionResult Filter()
        {
            return Ok(mapper.Map<SearchDepenseModel>(depenseManager.GetNewFilter()));
        }

        #region Génération excel

        /// <summary>
        /// Méthode de génération d'une liste de dépenses dépenses au format excel  
        ///       spécifier taille des fichiers ?
        /// </summary>
        /// <param name="commandes">Liste des commande</param>
        /// <returns>Identifiant du cache</returns>
        [HttpPost]
        [Route("api/Depense/GenerateExcel/")]
        public object GenerateExcel(List<CommandeModel> commandes)
        {
            ExcelFormat excelFormat = new ExcelFormat();
            string pathName = HttpContext.Current.Server.MapPath("/Templates/TemplateReceptions.xlsx").ToString();
            string cacheId = Guid.NewGuid().ToString();

            CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(30), Priority = CacheItemPriority.NotRemovable };

            foreach (CommandeModel commande in commandes)
            {
                if (commande.Lignes != null)
                {
                    foreach (CommandeLigneModel ligne in commande.Lignes)
                    {
                        ligne.Commande = commande;
                        foreach (DepenseAchatModel reception in ligne.DepensesReception)
                        {
                            reception.CommandeLigne = ligne;
                        }
                    }
                }
            }

            List<DepenseAchatModel> receptions = commandes.SelectMany(c => c.Lignes != null ? c.Lignes.SelectMany(l => l.DepensesReception) : new List<DepenseAchatModel>().ToArray()).ToList();

            byte[] excelBytes = excelFormat.GenerateExcelDepenses<DepenseAchatModel>(pathName, receptions);
            MemoryCache.Default.Add("excelBytes_" + cacheId, excelBytes, policy);

            return new { id = cacheId };
        }

        #endregion

        /// <summary>
        /// Récupération d'un filtre explorateurDepense
        /// </summary    
        /// <returns>Filtre explorateur des dépenses</returns>
        [HttpGet]
        [Route("api/Depense/ExplorateurDepense/Filter")]
        public IHttpActionResult GetExplorateurDepenseFilter()
        {
            return Ok(mapper.Map<SearchExplorateurDepenseModel>(explorateurDepenseManager.GetNewFilter()));
        }

        /// <summary>
        /// Récupération de l'arbre d'exploration des dépenses
        /// </summary>
        /// <param name="filtre">Filtre d'explorateurDépense</param>    
        /// <returns>Arbre d'exploration des dépenses</returns>
        [HttpPost]
        [Route("api/Depense/ExplorateurDepense/Tree/")]
        public async Task<IHttpActionResult> GetExplorateurDepenseTreeAsync(SearchExplorateurDepenseModel filtre)
        {
            IEnumerable<ExplorateurAxe> explorateurAxes = await explorateurDepenseManager.GetAsync(mapper.Map<SearchExplorateurDepense>(filtre)).ConfigureAwait(false);
            return Ok(mapper.Map<IEnumerable<ExplorateurAxeModel>>(explorateurAxes));
        }

        /// <summary>
        /// Récupération des dépenses selon les axes choisit
        /// </summary>    
        /// <param name="filtre">Filtre d'explorateurDépense</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Liste des dépenses</returns>
        [HttpPost]
        [Route("api/Depense/ExplorateurDepense/List/{page?}/{pageSize?}")]
        public async Task<IHttpActionResult> GetDepensesByAxeAsync(SearchExplorateurDepenseModel filtre, int page = 1, int pageSize = 20)
        {
            ExplorateurDepenseResult explorateurDepenseResult = await explorateurDepenseManager.GetDepensesAsync(mapper.Map<SearchExplorateurDepense>(filtre), page, pageSize).ConfigureAwait(false);
            return Ok(mapper.Map<ExplorateurDepenseResultModel>(explorateurDepenseResult));
        }

        /// <summary>
        /// Récupération des dépenses selon les axes choisit, pour Fayat TP
        /// </summary>    
        /// <param name="filtre">Filtre d'explorateurDépense</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Liste des dépenses</returns>
        [HttpPost]
        [Route("api/Depense/ExplorateurDepenseFayatTP/List/{page?}/{pageSize?}")]
        public async Task<IHttpActionResult> GetDepensesByAxeForFayatTPAync(SearchExplorateurDepenseModel filtre, int page = 1, int pageSize = 20)
        {
            ExplorateurDepenseResult explorateurDepenseResult = await explorateurDepenseManagerFayatTP.GetDepensesAsync(mapper.Map<SearchExplorateurDepense>(filtre), page, pageSize).ConfigureAwait(false);
            return Ok(mapper.Map<ExplorateurDepenseResultModel>(explorateurDepenseResult));
        }

        /// <summary>
        /// Génération du fichier Excel et mise en cache
        /// </summary>
        /// <param name="filtre">Filtre utilisé</param>
        /// <returns>Document Excel</returns>
        [HttpPost]
        [Route("api/Depense/ExplorateurDepense/Export/")]
        public async Task<IHttpActionResult> GenerateExportExcelAsync(SearchExplorateurDepenseModel filtre)
        {
            byte[] bytes = await explorateurDepenseManager.GetExplorateurDepensesExcelAsync(mapper.Map<SearchExplorateurDepense>(filtre)).ConfigureAwait(false);
            return HandleExportExcelGeneration(bytes);
        }

        /// <summary>
        /// Génération du fichier Excel et mise en cache, pour Fayat TP
        /// </summary>
        /// <param name="filtre">Filtre utilisé</param>
        /// <returns>Document Excel</returns>
        [HttpPost]
        [Route("api/Depense/ExplorateurDepenseFayatTP/Export/")]
        public async Task<IHttpActionResult> GenerateExportExcelForFayatTPAsync(SearchExplorateurDepenseModel filtre)
        {
            byte[] bytes = await explorateurDepenseManagerFayatTP.GetExplorateurDepensesExcelAsync(mapper.Map<SearchExplorateurDepense>(filtre)).ConfigureAwait(false);
            return HandleExportExcelGeneration(bytes);
        }

        /// <summary>
        /// Télécharge l'export Excel des réceptions
        /// </summary>
        /// <param name="id">Identifiant du cache</param>
        /// <param name="codeCi">Code CI</param>
        /// <returns>Fichier excel des réceptions</returns>
        [HttpGet]
        [Route("api/Depense/ExplorateurDepense/Export/{id}/{codeCi}")]
        public IHttpActionResult GetExportExcel(string id, string codeCi)
        {
            string cacheName = exportDocumentService.GetCacheName(id, isPdf: false);
            byte[] bytes = MemoryCache.Default.Get(cacheName) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheName);
            }
            string exportFilename = exportDocumentService.GetDocumentFileName("Depenses_" + codeCi, false);
            HttpResponseMessage result = exportDocumentService.CreateResponseForDownloadDocument(exportFilename, bytes);
            return ResponseMessage(result);
        }

        /// <summary>
        /// Vérifie si un CI est attaché à une société SEP ou non
        /// </summary>
        /// <param name="ciId">id du Ci</param>
        /// <returns>Renvoie true si le CI est attaché à une société SEP sinon faux</returns>
        [HttpGet]
        [Route("api/Depense/ExplorateurDepense/IsSep/{ciId}")]
        public IHttpActionResult IsSep(int ciId)
        {
            return Ok(new
            {
                id = ciId,
                isSep = depenseServiceMediator.IsSep(ciId)
            });
        }

        private IHttpActionResult HandleExportExcelGeneration(byte[] bytes)
        {
            const string typeCache = "excelBytes_";
            string cacheId = Guid.NewGuid().ToString();

            CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(300), Priority = CacheItemPriority.NotRemovable };
            MemoryCache.Default.Add(typeCache + cacheId, bytes, policy);

            return Ok(new { id = cacheId });
        }
    }
}
