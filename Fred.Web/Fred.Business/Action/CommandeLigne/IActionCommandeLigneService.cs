using Fred.Business.Action.CommandeLigne.Models;
using Fred.Entities.Commande;
using System.Threading.Tasks;

namespace Fred.Business.Action.CommandeLigne
{
    public interface IActionCommandeLigneService
    {
        Task<ActionCommandeLigneEnt> FindByIdAsync(int id);

        void CreateActionCommandeLigne(ActionCommandeLigneInputModel inputModel);
    }
}
