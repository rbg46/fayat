using System.Collections.Generic;
using Fred.Business.RepriseDonnees.Materiel.Models;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Materiel.ContextProviders
{
    /// <summary>
    /// Service qui fournit les données necessaires a l'import des Materiels
    /// </summary>
    public interface IMaterielContextProvider : IService
    {
        /// <summary>
        /// Fournit les données necessaires a l'import des Materiels
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="repriseExcelMateriel">repriseExcelMateriel</param>
        /// <returns>les données necessaires a l'import des Materiels</returns>
        ContextForImportMateriel GetContextForImportMateriel(int groupeId, List<RepriseExcelMateriel> repriseExcelMateriel);
    }
}
