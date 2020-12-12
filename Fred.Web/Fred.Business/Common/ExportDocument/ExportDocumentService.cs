using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Fred.Entities;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;

namespace Fred.Business.Common.ExportDocument
{
    /// <summary>
    /// Service pour le telechargement de fichier excel ou pdf
    /// </summary>
    public class ExportDocumentService : IExportDocumentService
    {
        private const string TypeCachePdf = "pdfBytes_";
        private const string TypeCacheExcel = "excelBytes_";
        private const string ExtensionPdf = ".pdf";
        private const string ExtensionExcel = ".xlsx";

        /// <summary>
        /// Retourne un identifiant unique de cache en fonction du type de document.
        /// </summary>
        /// <param name="isPdf">True pour un fichier PDF, false pour un fchier Excel.</param>
        /// <returns>Un identifiant unique de cache.</returns>
        public string GetNewCacheId(bool isPdf)
        {
            return GetCacheName(Guid.NewGuid().ToString(), isPdf);
        }

        /// <summary>
        /// Determine le nom du document en fonction de l'id et du type de document
        /// </summary>
        /// <param name="id">id du document</param>
        /// <param name="isPdf">est un pdf ou un excel</param>
        /// <returns>Le nom du document</returns>
        public string GetCacheName(string id, bool isPdf)
        {
            if (isPdf)
            {
                return string.Format("{0}{1}", TypeCachePdf, id);
            }
            else
            {
                return string.Format("{0}{1}", TypeCacheExcel, id);
            }
        }

        /// <summary>
        /// Creer un ExportDocumentInfo qui contient le contentType et le nom du document(avec extension)
        /// </summary>
        /// <param name="fileName">Le nom du fichier qui sera afficher lors du telechargement</param>
        /// <param name="isPdf">Si c'est un pdf ou un excel</param>
        /// <returns>ExportDocumentInfo</returns>
        public string GetDocumentFileName(string fileName, bool isPdf)
        {
            if (isPdf)
            {
                return CreatePdfDocumentFileName(fileName);
            }
            else
            {
                return CreateExcelDocumentFileName(fileName);
            }
        }

        /// <summary>
        /// Creer un ExportDocumentInfo qui contient le contentType et le nom du document(avec extension)
        /// </summary>
        /// <param name="fileName">Le nom du fichier qui sera afficher lors du telechargement</param>
        /// <param name="typeExport">Si c'est un pdf ou un excel (cf: Enumeration TypeExport)</param>
        /// <returns>ExportDocumentInfo</returns>
        public string GetDocumentFileName(string fileName, int typeExport)
        {
            if (typeExport == TypeExport.Pdf.ToIntValue())
            {
                return CreatePdfDocumentFileName(fileName);
            }
            else if (typeExport == TypeExport.Excel.ToIntValue())
            {
                return CreateExcelDocumentFileName(fileName);
            }
            else
            {
                throw new FredTechnicalException(BusinessResources.TypeExport_Error);
            }
        }

        private static string CreatePdfDocumentFileName(string fileName)
        {
            return fileName + ExtensionPdf;
        }

        private static string CreateExcelDocumentFileName(string fileName)
        {
            return fileName + ExtensionExcel;
        }

        /// <summary>
        /// Creer un reponse pour le telechargement d'un fichier
        /// </summary>
        /// <param name="exportfileName">exportfileName</param>
        /// <param name="data">byte array represantant le document</param>
        /// <returns>HttpResponseMessage</returns>
        public HttpResponseMessage CreateResponseForDownloadDocument(string exportfileName, byte[] data)
        {
            HttpResponseMessage result = null;
            if (data != null)
            {
                result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(data)
                };

                var contentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = exportfileName
                };

                result.Content.Headers.ContentDisposition = contentDisposition;

                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            }
            else
            {
                result = new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            return result;
        }
    }
}
