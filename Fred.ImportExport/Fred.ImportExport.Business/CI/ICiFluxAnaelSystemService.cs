using System.Threading.Tasks;
using Fred.Business;

namespace Fred.ImportExport.Business.CI
{
    /// <summary>
    /// Gere l'import depuis l'interface Fred IE web.
    /// Itere sur les codesocietecomptable.
    /// </summary>
    public interface ICiFluxAnaelSystemService : IService
    {
        /// <summary>
        /// Importe depuis le code flux
        /// </summary>
        /// <param name="codeFlux">codeFlux</param>
        /// <param name="bypassDate">bypassDate</param>
        Task ImportationByCodeFluxAsync(string codeFlux, bool bypassDate);
    }
}
