using System;
using System.Net.Http;
using System.Runtime.Caching;
using System.Web.Http;
using Fred.Business.Budget.BudgetComparaison;
using Fred.Business.Common.ExportDocument;

namespace Fred.Web.API
{
    /// <summary>
    /// Controlleur de la comparaison de budget.
    /// </summary>
    public class BudgetComparaisonController : ApiControllerBase
    {
        protected readonly IBudgetComparaisonManager budgetComparaisonManager;
        protected readonly IExportDocumentService exportDocumentService;

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="budgetComparaisonManager">Gestionnaire de la comparaison de budget.</param>
        /// <param name="exportDocumentService">Manageur des téléchargements de fichier.</param>
        public BudgetComparaisonController(
            IBudgetComparaisonManager budgetComparaisonManager,
            IExportDocumentService exportDocumentService)
        {
            this.budgetComparaisonManager = budgetComparaisonManager;
            this.exportDocumentService = exportDocumentService;
        }

        /// <summary>
        /// Retourne les révisions de budget d'un CI.
        /// </summary>
        /// <param name="ciId">L'identifiant du CI concerné.</param>
        /// <param name="page">L'index de la page.</param>
        /// <param name="pageSize">La taille d'une page.</param>
        /// <param name="recherche">Le texte recherché.</param>
        /// <returns>Les révisions de budget du CI.</returns>
        [HttpGet]
        [Route("api/BudgetComparaison/SearchBudgetRevisions/{page?}/{pageSize?}/{recherche?}/{ciId?}")]
        public HttpResponseMessage SearchBudgetRevisions(int? ciId, int page = 1, int pageSize = 20, string recherche = "")
        {
            return Get(() => budgetComparaisonManager.SearchBudgetRevisions(ciId.Value, page, pageSize, recherche));
        }

        /// <summary>
        /// Compare des budgets.
        /// </summary>
        /// <param name="request">La requête de comparaison.</param>
        /// <returns>Le résultat de la comparaison.</returns>
        [HttpPost]
        [Route("api/BudgetComparaison/Compare/")]
        public HttpResponseMessage Compare(Business.Budget.BudgetComparaison.Dto.Comparaison.RequestDto request)
        {
            return Post(() => budgetComparaisonManager.Compare(request));
        }

        /// <summary>
        /// Met en cache l'export Excel d'une comparaison de budget.
        /// </summary>
        /// <param name="request">La requête de comparaison.</param>
        /// <returns>L'identifiant dans le cache du fichier généré ainsi que l'erreur, le cas échéant.</returns>
        [HttpPost]
        [Route("api/BudgetComparaison/ExcelExport/")]
        public HttpResponseMessage ExcelExport(Business.Budget.BudgetComparaison.Dto.ExcelExport.RequestDto request)
        {
            return Post(() =>
            {
                var result = budgetComparaisonManager.ExcelExport(request);
                string cacheId = string.Empty;
                if (string.IsNullOrEmpty(result.Erreur))
                {
                    var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(30), Priority = CacheItemPriority.NotRemovable };
                    cacheId = exportDocumentService.GetNewCacheId(false);
                    MemoryCache.Default.Add(cacheId, result.Data, policy);
                }
                return new { CacheId = cacheId, result.Erreur };
            });
        }

        /// <summary>
        /// Récupération d'un fichier mis en cache
        /// </summary>
        /// <param name="cacheId">Identifiant dans le cache.</param>
        /// <param name="fileName">Nom du fichier.</param>
        /// <returns>Le fichier sous forme de tableau d'octets.</returns>
        [HttpGet]
        [Route("api/BudgetComparaison/ExtractDocument/{cacheId}/{fileName}")]
        public HttpResponseMessage ExtractDocument(string cacheId, string fileName)
        {
            var bytes = MemoryCache.Default.Get(cacheId) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheId);
            }
            var exportDocument = exportDocumentService.GetDocumentFileName(fileName, false);
            var result = exportDocumentService.CreateResponseForDownloadDocument(exportDocument, bytes);
            return result;
        }
    }
}
