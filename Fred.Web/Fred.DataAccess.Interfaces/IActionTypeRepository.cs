using Fred.Entities.Action;

namespace Fred.DataAccess.Interfaces
{
    public interface IActionTypeRepository : IFredRepository<ActionTypeEnt>
    {
        ActionTypeEnt FindById(int id);

        ActionTypeEnt FindByCode(string code);
    }
}
