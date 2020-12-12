using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.EcritureComptable.Validator;
using Fred.DataAccess.Interfaces;
using Fred.Entities.EcritureComptable;

namespace Fred.Business.EcritureComptable
{
    public class EcritureComptableCumulManager : Manager<EcritureComptableCumulEnt, IEcritureComptableCumulRepository>, IEcritureComptableCumulManager
    {
        public EcritureComptableCumulManager(
            IUnitOfWork uow,
            IEcritureComptableCumulRepository ecritureComptableCumulRepository,
            IEcritureComptableCumulValidator ecritureComptableCumulValidator)
            : base(uow, ecritureComptableCumulRepository, ecritureComptableCumulValidator)
        { }

        public async Task<IReadOnlyList<EcritureComptableCumulEnt>> GetEcritureComptableCumulByCiIdAndPartNumberAsync(List<EcritureComptableCumulEnt> ecritureComptableCumulEnts)
        {
            return await Repository.GetEcritureComptableCumulByCiIdAndPartNumberAsync(ecritureComptableCumulEnts.Select(q => q.CiId), ecritureComptableCumulEnts.Select(q => q.NumeroPiece)).ConfigureAwait(false);
        }

        public void InsertListByTransaction(ICollection<EcritureComptableCumulEnt> ecritureComptableCumulsToInsert)
        {
            Repository.InsertListByTransaction(ecritureComptableCumulsToInsert);
        }
    }
}
