using System.Collections.Generic;
using Fred.Business.RepriseDonnees.Ci.Models;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Ci.ContextProviders
{
    /// <summary>
    /// Service qui fournit les données necessaires a l'import des cis
    /// </summary>
    public interface ICiContextProvider : IService
    {
        /// <summary>
        /// Fournit les données necessaires a l'import des cis
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="repriseExcelCis">repriseExcelCis</param>
        /// <returns>les données necessaires a l'import des cis</returns>
        ContextForImportCi GetContextForImportCis(int groupeId, List<RepriseExcelCi> repriseExcelCis);
    }
}
