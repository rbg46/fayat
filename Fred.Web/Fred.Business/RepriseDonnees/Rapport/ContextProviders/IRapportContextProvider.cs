using System.Collections.Generic;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Rapport.ContextProviders
{
    /// <summary>
    /// Service qui fournit les données necessaires a l'import des cis
    /// </summary>
    public interface IRapportContextProvider : IService
    {
        /// <summary>
        /// Fournit les données necessaires a l'import des rapports
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="repriseExcelRapports">repriseExcelRapports</param>
        /// <returns>les données necessaires a l'import des rapports</returns>
        ContextForImportRapport GetContextForImportRapports(int groupeId, List<RepriseExcelRapport> repriseExcelRapports);
    }
}
