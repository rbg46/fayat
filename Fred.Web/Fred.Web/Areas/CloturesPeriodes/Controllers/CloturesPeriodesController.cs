using System.Web.Mvc;
using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;

namespace Fred.Web.Areas.CloturesPeriodes.Controllers
{
    public class CloturesPeriodesController : Controller
    {
        // GET: CloturesPeriodes/CloturesPeriodes
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuDatesClotureComptableEnMasseIndex)]
        public ActionResult Index()
        {
            return View();
        }
    }
}
