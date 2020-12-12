using Fred.Entities.Action;

namespace Fred.DataAccess.Interfaces
{
    public interface IActionStatusRepository : IFredRepository<ActionStatusEnt>
    {
        ActionStatusEnt FindById(int id);

        ActionStatusEnt FindByName(string name);
    }
}
