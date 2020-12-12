using System.Web.Mvc;
using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;

namespace Fred.Web.Areas.OperationDiverse.Controllers
{
    [Authorize()]
    public class OperationDiverseController : Controller
    {
        // GET: OperationDiverse/OperationDiverse
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuOperationDiverseIndex)]
        public ActionResult Index(int? favoriId, int? ciId = null, int? year = null, int? month = null)
        {
            if (ciId != null && month != null && year != null)
            {
                ViewBag.CiId = ciId;
                ViewBag.Month = month;
                ViewBag.Year = year;
            }
            ViewBag.favoriId = favoriId;
            return View();
        }

        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuOperationDiverseIndex)]
        public ActionResult Detaille(int societeId, int id, int year, int month, int famille, string codeFamille)
        {
            ViewBag.societeId = societeId;
            ViewBag.id = id;
            ViewBag.month = month;
            ViewBag.year = year;
            ViewBag.famille = famille;
            ViewBag.codeFamille = codeFamille;
            return View("Detaille");
        }
    }
}
