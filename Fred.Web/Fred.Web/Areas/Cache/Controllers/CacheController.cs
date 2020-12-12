using System.Web.Mvc;
using Fred.Entities.Role;

namespace Fred.Web.Areas.Cache.Controllers
{
    [Authorize(Roles = ApplicationRole.SuperAdmin)]
    public class CacheController : Controller
    {
        // GET: Cache/Cache
        public ActionResult Index()
        {
            return View();
        }
    }
}
