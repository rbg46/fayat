using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.EcritureComptable;
using Fred.Framework.DateTimeExtend;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Repository des EcritureComptable
    /// </summary>
    public interface IEcritureComptableRepository : IRepository<EcritureComptableEnt>
    {
        void InsertListByTransaction(IEnumerable<EcritureComptableEnt> ecritureComptablesToInsert);

        Task<IEnumerable<EcritureComptableEnt>> GetAllAsync(List<int> ciIds, DateTime dateComptable);

        Task<IEnumerable<EcritureComptableEnt>> GetAllAsync(int ciId, DateTime dateComptableDebut, DateTime dateComptableFin);

        Task<IEnumerable<EcritureComptableEnt>> GetListOfNumeroPiecesAsync(List<int> cisOfSociete, MonthLimits monthLimits);

        Task<IEnumerable<EcritureComptableEnt>> GetByCiIdsAndPeriodAsync(List<int> cisOfSociete, MonthLimits monthLimits);

        Task<IEnumerable<EcritureComptableEnt>> GetAsync(int ciId, MonthLimits monthLimits);

        Task<IReadOnlyList<EcritureComptableEnt>> GetByCiIdAndLabelAsync(IEnumerable<int> ciIds, IEnumerable<string> labels);

        Task<IReadOnlyList<EcritureComptableEnt>> GetByCommandeIdsAsync(List<int> commandeIds);

        Task<IReadOnlyList<EcritureComptableEnt>> GetByFamilleOdIdsAsync(List<int> familleOperationDiverseIds);

        IReadOnlyList<EcritureComptableEnt> GetByNumerosFacturesSAP(List<string> numeroFacturesSAP);

        Task<decimal> GetEcritureComptableCumulSumAsync(int ecritureComptableId);

        EcritureComptableEnt GetById(int ecritureComptableId);
    }
}
