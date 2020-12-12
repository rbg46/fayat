using System.Web.Mvc;
using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;

namespace Fred.Web.Areas.CommandeEnergies.Controllers
{
    public class CommandeEnergieController : Controller
    {
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuPreparerEnergies)]
        /// <summary>
        /// GET: CommandeEnergie/Index
        /// </summary>
        /// <param name="favoriId">favoriId</param>
        /// <returns>Current view</returns>
        public ActionResult Index(int? favoriId)
        {
            ViewBag.favoriId = favoriId;
            return View();
        }

        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuPreparerEnergies)]
        public ActionResult Detail(int? id = null)
        {
            ViewBag.id = id;
            return this.View();
        }
    }
}
