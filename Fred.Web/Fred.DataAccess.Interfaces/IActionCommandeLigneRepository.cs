using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Commande;

namespace Fred.DataAccess.Interfaces
{
    public interface IActionCommandeLigneRepository : IFredRepository<ActionCommandeLigneEnt>
    {
        Task<IEnumerable<ActionCommandeLigneEnt>> GetByCommandeLigneIdAsync(int commandeLigneId);

        Task<IEnumerable<ActionCommandeLigneEnt>> GetByCommandeLigneIdAndActionId(int commandeLigneId, int actionId);

        Task InsertActionCommandeLigneAsync(ActionCommandeLigneEnt commandeLigneEnt);
    }
}
