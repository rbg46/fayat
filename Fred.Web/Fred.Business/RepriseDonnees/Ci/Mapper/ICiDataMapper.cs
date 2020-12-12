using System.Collections.Generic;
using Fred.Business.RepriseDonnees.Ci.Models;
using Fred.Entities.CI;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Ci.Mapper
{
    /// <summary>
    /// Mappe les données du ci excel vers le ciEnt
    /// </summary>
    public interface ICiDataMapper : IService
    {
        /// <summary>
        /// Affecte les nouvelles valeurs a certains champs des ci
        /// </summary>
        /// <param name="context">context contenant les data necessaire a l'import</param>
        /// <param name="repriseExcelCis">les ci sous la forme excel</param>
        /// <returns>Liste de ci avec certains champs modifiés</returns>
        List<CIEnt> Map(ContextForImportCi context, List<RepriseExcelCi> repriseExcelCis);
    }
}
