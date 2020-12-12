using System.Net.Http;
using System.Runtime.Caching;
using System.Web.Http;
using Fred.Business.Common.ExportDocument;

namespace Fred.Web.API
{
    public class ExportExcelPdfController : ApiControllerBase
    {
        private readonly IExportDocumentService exportDocumentService;

        public ExportExcelPdfController(IExportDocumentService exportDocumentService)
        {
            this.exportDocumentService = exportDocumentService;
        }

        /// <summary>
        /// Méthode d'extraction d'une liste de dépenses au format excel.
        /// </summary>
        /// <param name="fileName">Nom du fichier generé</param>
        /// <param name="id">id du cache</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/ExportExcelPdf/RetrieveExcel/{fileName}/{id}/")]
        public HttpResponseMessage ExtractExcel(string fileName, string id)
        {
            string cacheName = exportDocumentService.GetCacheName(id, isPdf: false);
            byte[] bytes = MemoryCache.Default.Get(cacheName) as byte[];
            if (bytes != null)
            {
                MemoryCache.Default.Remove(cacheName);
            }
            string exportDocument = exportDocumentService.GetDocumentFileName(fileName: fileName, isPdf: false);
            return exportDocumentService.CreateResponseForDownloadDocument(exportDocument, bytes);
        }
    }
}
