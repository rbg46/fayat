using System.Collections.Generic;
using Fred.Business.RepriseDonnees.Personnel.Models;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Personnel.ContextProdivers
{
    /// <summary>
    /// Service qui fournit les données necessaires a l'import des Personnels
    /// </summary>
    public interface IPersonnelContextProvider : IService
    {
        /// <summary>
        /// Fournit les données necessaires a l'import des Personnels
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="repriseExcelPersonnel">repriseExcelPersonnel</param>
        /// <returns>les données necessaires a l'import des Personnel</returns>
        ContextForImportPersonnel GetContextForImportPersonnel(int groupeId, List<RepriseExcelPersonnel> repriseExcelPersonnel);
    }
}
