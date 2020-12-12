using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Action;
using Fred.EntityFramework;

namespace Fred.DataAccess.Action
{
    public class ActionStatusRepository : FredRepository<ActionStatusEnt>, IActionStatusRepository
    {
        public ActionStatusRepository(FredDbContext context)
            : base(context)
        {
        }

        public ActionStatusEnt FindById(int id)
        {
            return Context.ActionStatus.Where(at => at.ActionStatusId == id).FirstOrDefault();
        }

        public ActionStatusEnt FindByName(string name)
        {
            return Context.ActionStatus.Where(at => at.Name == name).FirstOrDefault();
        }
    }
}
