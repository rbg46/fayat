using System.Collections.Generic;
using Fred.Business.RepriseDonnees.IndemniteDeplacement.Models;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.IndemniteDeplacement.ContextProviders
{
    /// <summary>
    /// Service qui fournit les données necessaires a l'import des Indemnités de Déplacement
    /// </summary>
    public interface IIndemniteDeplacementContextProvider : IService
    {
        /// <summary>
        /// Fournit les données necessaires a l'import des Indémnités de Déplacement
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="repriseExcelIndemniteDeplacement">repriseExcelIndemniteDeplacement</param>
        /// <returns>Les données necessaires a l'import des Indémnités de Déplacement</returns>
        ContextForImportIndemniteDeplacement GetContextForImportIndemniteDeplacement(int groupeId, List<RepriseExcelIndemniteDeplacement> repriseExcelIndemniteDeplacement);
    }
}
