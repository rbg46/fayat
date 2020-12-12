
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Depense;
using Microsoft.EntityFrameworkCore;

namespace Fred.Business.Depense
{
    /// <summary>
    ///     Gestionnaire Depense Type
    /// </summary>
    public class DepenseTypeManager : Manager<DepenseTypeEnt>, IDepenseTypeManager
    {
        public DepenseTypeManager(IUnitOfWork uow, IRepository<DepenseTypeEnt> repository)
        : base(uow, repository)
        {
        }

        /// <inheritdoc/>
        public DepenseTypeEnt Get(int code)
        {
            return this.Repository.Query().Filter(x => x.Code == code).Get().FirstOrDefault();
        }

        /// <summary>
        ///     Récupère un DepenseType par son code sans le tracker dans le context
        /// </summary>
        /// <param name="code">Code (entier)</param>
        /// <returns>DepenseTypeEnt</returns>
        public DepenseTypeEnt GetByCode(int code)
        {
            return this.Repository.Query().Filter(x => x.Code == code).Get().AsNoTracking().FirstOrDefault();
        }
    }
}
