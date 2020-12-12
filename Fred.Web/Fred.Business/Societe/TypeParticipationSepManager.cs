using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities;

namespace Fred.Business.Societe
{
    /// <summary>
    /// Manager Type Participation
    /// </summary>
    public class TypeParticipationSepManager : Manager<TypeParticipationSepEnt>, ITypeParticipationSepManager
    {
        public TypeParticipationSepManager(IUnitOfWork uow, IRepository<TypeParticipationSepEnt> typeParticipationSepRepo)
        : base(uow, typeParticipationSepRepo)
        {
        }

        /// <inheritdoc/>
        public List<TypeParticipationSepEnt> GetAll()
        {
            return Repository.Query().Get().ToList();
        }

        /// <inheritdoc/>
        public TypeParticipationSepEnt Get(string code)
        {
            return Repository.Query().Filter(x => x.Code == code).Get().FirstOrDefault();
        }
    }
}
