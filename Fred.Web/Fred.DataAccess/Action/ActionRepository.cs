using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Action;
using Fred.EntityFramework;

namespace Fred.DataAccess.Action
{
    public class ActionRepository : FredRepository<ActionEnt>, IActionRepository
    {
        public ActionRepository(FredDbContext context)
            : base(context)
        {
        }
    }
}
