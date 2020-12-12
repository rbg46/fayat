using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Action.CommandeLigne
{
    public class ActionCommandeLigneRepository : FredRepository<ActionCommandeLigneEnt>, IActionCommandeLigneRepository
    {
        public ActionCommandeLigneRepository(FredDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<ActionCommandeLigneEnt>> GetByCommandeLigneIdAsync(int commandeLigneId)
        {
            return await Context.ActionCommandeLigne
                .Include(a => a.Action)
                .Where(a => a.CommandeLigneId == commandeLigneId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<ActionCommandeLigneEnt>> GetByCommandeLigneIdAndActionId(int commandeLigneId, int actionId)
        {
            return await Context.ActionCommandeLigne
                .Include(a => a.Action)
                .Where(a => a.CommandeLigneId == commandeLigneId && a.ActionId == actionId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task InsertActionCommandeLigneAsync(ActionCommandeLigneEnt actionCommandeLigneEnt)
        {
            if (actionCommandeLigneEnt != null)
            {
                await Context.ActionCommandeLigne.AddAsync(actionCommandeLigneEnt);
            }
        }
    }
}
