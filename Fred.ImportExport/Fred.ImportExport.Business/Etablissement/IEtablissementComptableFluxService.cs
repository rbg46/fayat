using System.Threading.Tasks;
using Fred.Business;

namespace Fred.ImportExport.Business.Etablissement
{
    /// <summary>
    /// Gere l'import des etablissement comptable depuis l'interface Fred IE web.
    /// </summary>
    public interface IEtablissementComptableFluxService : IService
    {
        /// <summary>
        /// Methode d'import des etablissement comptable
        /// </summary>
        Task ImportEtablissementComptableAsync();
    }
}
