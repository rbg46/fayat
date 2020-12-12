using System;
using System.IO;
using Fred.Business.Commande.Models;

namespace Fred.Business.Commande.Services
{
    /// <summary>
    /// Interface d'import Export Commandes Lignes
    /// </summary>
    public interface ICommandeImportExportExcelService : IService
    {
        /// <summary>
        /// Generate File Excel for Lignes Ordered
        /// </summary>
        /// <param name="ciId">Id CI</param>
        /// <param name="isAvenant">Type Avenant</param>
        /// <returns>retourne un object </returns>
        ImportResultImportLignesCommande GenerateExempleExcel(int ciId, bool isAvenant);

        /// <summary>
        /// Generate File Excel for Lignes Ordered
        /// </summary>
        /// <param name="checkinValue">Valeur a checkin</param>
        /// <param name="ciId">id Ci</param>
        /// <param name="stream">File format Stream</param>
        /// <param name="isAvenant">Type avenant</param>
        /// <returns>retourne un object </returns>
        ImportResultImportLignesCommande ImportCommandeLignes(string checkinValue, int ciId, Stream stream, bool isAvenant);
    }
}
