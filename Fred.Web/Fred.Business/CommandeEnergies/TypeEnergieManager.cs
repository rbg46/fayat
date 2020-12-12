using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;

namespace Fred.Business.CommandeEnergies
{
    /// <summary>
    /// Gestionaire des Types Energie
    /// </summary>
    public class TypeEnergieManager : Manager<TypeEnergieEnt>, ITypeEnergieManager
    {
        public TypeEnergieManager(IUnitOfWork uow, IRepository<TypeEnergieEnt> typeEnergieRepo)
        : base(uow, typeEnergieRepo)
        {
        }

        /// <inheritdoc/>
        public List<TypeEnergieEnt> GetAll()
        {
            return Repository.Get().ToList();
        }

        /// <inheritdoc/>
        public TypeEnergieEnt Get(int typeEnergieId)
        {
            return Repository.Query().Get().FirstOrDefault(x => x.TypeEnergieId == typeEnergieId);
        }

        /// <inheritdoc/>
        public TypeEnergieEnt GetByCode(string code)
        {
            return Repository.Query().Get().FirstOrDefault(x => x.Code == code);
        }
    }
}
