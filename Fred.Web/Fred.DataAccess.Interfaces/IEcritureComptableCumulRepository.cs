
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.EcritureComptable;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Repository des Ecritures Comptables Cumulées
    /// </summary>
    public interface IEcritureComptableCumulRepository : IFredRepository<EcritureComptableCumulEnt>
    {
        void InsertListByTransaction(ICollection<EcritureComptableCumulEnt> ecritureComptableCumul);

        Task<IReadOnlyList<EcritureComptableCumulEnt>> GetEcritureComptableCumulByCiIdAndPartNumberAsync(IEnumerable<int> ciIds, IEnumerable<string> partNumbers);
    }
}
