using System.Net.Http;

namespace Fred.Business.Common.ExportDocument
{
    /// <summary>
    ///  Interface pour le service pour le telechargement de fichier excel ou pdf
    /// </summary>
    public interface IExportDocumentService
    {
        /// <summary>
        /// Retourne un identifiant unique de cache en fonction du type de document.
        /// </summary>
        /// <param name="isPdf">True pour un fichier PDF, false pour un fchier Excel.</param>
        /// <returns>Un identifiant unique de cache.</returns>
        string GetNewCacheId(bool isPdf);

        /// <summary>
        /// Determine le nom du document en fonction de l'id et du type de document
        /// </summary>
        /// <param name="id">id du document</param>
        /// <param name="isPdf">est un pdf ou un excel</param>
        /// <returns>Le nom du document</returns>
        string GetCacheName(string id, bool isPdf);

        /// <summary>
        /// Creer un ExportDocumentInfo qui contient le contentType et le nom du document(avec extension)
        /// </summary>
        /// <param name="fileName">Le nom du fichier qui sera afficher lors du telechargement</param>
        /// <param name="isPdf">Si c'est un pdf ou un excel</param>
        /// <returns>ExportDocumentInfo</returns>
        string GetDocumentFileName(string fileName, bool isPdf);

        /// <summary>
        /// Creer un ExportDocumentInfo qui contient le contentType et le nom du document(avec extension)
        /// </summary>
        /// <param name="fileName">Le nom du fichier qui sera afficher lors du telechargement</param>
        /// <param name="typeExport">Si c'est un pdf ou un excel (cf: Enumeration TypeExport)</param>
        /// <returns>ExportDocumentInfo</returns>
        string GetDocumentFileName(string fileName, int typeExport);

        /// <summary>
        /// Creer un reponse pour le telechargement d'un fichier
        /// </summary>
        /// <param name="exportfileName">exportfileName</param>
        /// <param name="data">byte array represantant le document</param>
        /// <returns>HttpResponseMessage</returns>
        HttpResponseMessage CreateResponseForDownloadDocument(string exportfileName, byte[] data);
    }
}
