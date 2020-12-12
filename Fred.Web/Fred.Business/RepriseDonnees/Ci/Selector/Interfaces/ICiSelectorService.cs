using Fred.Business.RepriseDonnees.Ci.Models;
using Fred.Entities.CI;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Ci.Selector
{
    /// <summary>
    /// Service qui recuper le ci de la base en fonction des données contextuelle a l'import
    /// </summary>
    public interface ICiSelectorService : IService
    {

        /// <summary>
        /// Recupere le CiEnt de fred, en fonction du codeci
        /// </summary>
        /// <param name="repriseExcelCi"> le ci type excel</param>       
        /// <param name="context">les données contextuelle a l'import</param>
        /// <returns>le ci de la base correspondant au code ci et </returns>
        CIEnt GetCiOfDatabase(RepriseExcelCi repriseExcelCi, ContextForImportCi context);
    }
}
