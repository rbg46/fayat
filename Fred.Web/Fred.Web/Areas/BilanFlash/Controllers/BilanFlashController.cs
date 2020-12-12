using System.Web.Mvc;
using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;

namespace Fred.Web.Areas.BilanFlash.Controllers
{
    [Authorize]
    public class BilanFlashController : Controller
    {
        /// <summary>
        /// GET: BilanFlash/BilanFlash
        /// </summary>
        /// <returns>Current view</returns>
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuBilanFlashIndex)]
        public ActionResult Index()
        {
            return View("Index");
        }

        /// <summary>
        /// GET: BilanFlash/BilanFlash/ObjectifFlash
        /// </summary>
        /// /// <param name="id">id</param>
        /// <returns>Current view</returns>
        public ActionResult ObjectifFlash(int? id = null)
        {
            ViewBag.id = id;
            return View();
        }
    }
}
