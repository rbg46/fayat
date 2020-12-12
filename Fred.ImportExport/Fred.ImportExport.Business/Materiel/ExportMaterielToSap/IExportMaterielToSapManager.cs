using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fred.ImportExport.Business.Materiel.ExportMaterielToSap
{
    /// <summary>
    /// Manager d'export des pointages materiel vers SAP
    /// </summary>
    public interface IExportMaterielToSapManager
    {
        /// <summary>
        /// Exporte les pointages materiel a partir d'un rapportId
        /// </summary>
        /// <param name="rapportId">rapportId</param>
        /// <param name="backgroundJobId">Background job identifiant</param>
        Task<List<Exception>> ExportPointageToStormAsync(int rapportId, string backgroundJobId);

        /// <summary>
        ///     Envoi des pointages de chaque rapport de la liste 
        /// </summary>
        /// <param name="rapportIds">Liste de rapport</param>
        /// <param name="backgroundJobId">Background job identifiant</param>
        Task ExportPointageToStormAsync(List<int> rapportIds, string backgroundJobId);
    }
}
