using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Facturation;

namespace Fred.Business.Facturation
{
    /// <summary>
    /// Facturation Type Manager
    /// </summary>
    public class FacturationTypeManager : Manager<FacturationTypeEnt>, IFacturationTypeManager
    {
        public FacturationTypeManager(IUnitOfWork uow, IRepository<FacturationTypeEnt> repository)
        : base(uow, repository)
        {
        }

        /// <inheritdoc/>
        public FacturationTypeEnt Get(int code)
        {
            return Repository.Query().Filter(x => x.Code == code).Get().FirstOrDefault();
        }
    }
}
