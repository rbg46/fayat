using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities;

namespace Fred.Business.Societe
{
    public class TypeSocieteManager : Manager<TypeSocieteEnt, ITypeSocieteRepository>, ITypeSocieteManager
    {
        public TypeSocieteManager(IUnitOfWork uow, ITypeSocieteRepository typeSocieteRepo)
            : base(uow, typeSocieteRepo)
        {
        }

        public List<TypeSocieteEnt> GetAll()
        {
            return Repository.Query().Get().ToList();
        }

        public TypeSocieteEnt GetByCode(string code)
        {
            return Repository.Query().Get().FirstOrDefault(x => x.Code == code);
        }
    }
}
