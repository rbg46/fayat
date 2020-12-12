using System.Linq;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Societe
{
    /// <summary>
    /// TypeSocieteRepository
    /// </summary>
    public class TypeSocieteRepository : FredRepository<TypeSocieteEnt>, ITypeSocieteRepository
    {
        private readonly FredDbContext context;

        /// <summary>
        /// TypeSocieteRepository
        /// </summary>
        /// <param name="context">FredDbContext</param>
        /// <param name="logMgr">ILogManager</param>
        public TypeSocieteRepository(
            FredDbContext context,
            ILogManager logMgr)
            : base(context)
        {
            this.context = context;
        }

        /// <inheritdoc/>
        public async Task<TypeSocieteEnt> FindByIdAsync(int typeSocieteId)
        {
            return await context.TypeSocietes
                .SingleOrDefaultAsync(ts => ts.TypeSocieteId == typeSocieteId);
        }

        /// <inheritdoc/>
        public TypeSocieteEnt FindById(int typeSocieteId)
        {
            return context.TypeSocietes
                .SingleOrDefault(ts => ts.TypeSocieteId == typeSocieteId);
        }
    }
}
