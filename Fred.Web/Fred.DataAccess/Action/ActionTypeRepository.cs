using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Action;
using Fred.EntityFramework;

namespace Fred.DataAccess.Action
{
    public class ActionTypeRepository : FredRepository<ActionTypeEnt>, IActionTypeRepository
    {
        public ActionTypeRepository(FredDbContext context)
            : base(context)
        {
        }

        public ActionTypeEnt FindById(int id)
        {
            return Context.ActionType.Where(at => at.ActionTypeId == id).FirstOrDefault();
        }

        public ActionTypeEnt FindByCode(string code)
        {
            return Context.ActionType.Where(at => at.Code == code).FirstOrDefault();
        }
    }
}
