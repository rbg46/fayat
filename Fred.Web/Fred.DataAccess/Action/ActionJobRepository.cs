using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Action;
using Fred.EntityFramework;

namespace Fred.DataAccess.Action
{
    public class ActionJobRepository : FredRepository<ActionJobEnt>, IActionJobRepository
    {
        public ActionJobRepository(FredDbContext context)
            : base(context)
        {
        }
    }
}
