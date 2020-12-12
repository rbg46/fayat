using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Business;
using Fred.ImportExport.Business.Common;

namespace Fred.ImportExport.Business.Personnel
{
    /// <summary>
    /// Gere l'import depuis l'interface Fred IE web.
    /// Itere sur les codesocietecomptable.
    /// </summary>
    public interface IPersonnelFluxAnaelSystemService : IService
    {
        /// <summary>
        /// Importe depuis le code flux
        /// </summary>
        /// <param name="codeFlux">codeFlux</param>
        /// <param name="bypassDate">bypassDate</param>
        Task ImportationByCodeFluxAsync(string codeFlux, bool bypassDate);

        /// <summary>
        /// Importe depuis une liste d'ids
        /// </summary>
        /// <param name="ids">list des ids des personnels</param>
        /// <returns>result object</returns>
        Task<ImportResult> ImportationByPersonnelsIdsAsync(List<int> ids);
    }
}
