using System;
using System.Threading.Tasks;

namespace Fred.Business.ObjectifFlash.Reporting
{
    /// <summary>
    /// Manager de la synthese de bilan flash
    /// </summary>
    public interface IBilanFlashSyntheseExportManager : IManager
    {
        /// <summary>
        /// Export du bilan flash
        /// </summary>
        /// <param name="objectifFlashId">Id de l'objectif flash</param>
        /// <param name="dateDebut"> date de ébut</param>
        /// <param name="dateFin">date de fin</param>
        /// <param name="isPdfConverted">flag de conversion pdf</param>
        /// <returns>Identifiant d'export</returns>
        Task<object> ExportBilanFlashSyntheseAsync(int? objectifFlashId, DateTime? dateDebut, DateTime? dateFin, bool isPdfConverted);
    }
}
