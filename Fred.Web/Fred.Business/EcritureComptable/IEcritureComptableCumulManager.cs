using Fred.Entities.EcritureComptable;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fred.Business.EcritureComptable
{
    /// <summary>
    /// Interface du manager EcritureComptableCumuls
    /// </summary>
    public interface IEcritureComptableCumulManager : IManager<EcritureComptableCumulEnt>
    {
        Task<IReadOnlyList<EcritureComptableCumulEnt>> GetEcritureComptableCumulByCiIdAndPartNumberAsync(List<EcritureComptableCumulEnt> ecritureComptableCumulEnts);

        /// <summary>
        /// Insert une liste d'écriture comptable cumulée
        /// </summary>
        /// <param name="ecritureComptableCumulsToInsert">Liste d'écriture comptable cumulée</param>
        void InsertListByTransaction(ICollection<EcritureComptableCumulEnt> ecritureComptableCumulsToInsert);
    }
}
