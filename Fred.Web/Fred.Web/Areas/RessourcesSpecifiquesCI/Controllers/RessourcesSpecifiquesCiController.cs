using System.Web.Mvc;
using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;

namespace Fred.Web.Areas.RessourcesSpecifiquesCI.Controllers
{
    [Authorize]
    public class RessourcesSpecifiquesCiController : Controller
    {
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuRessourcesSpecifiquesCiIndex)]
        public ActionResult Index()
        {
            ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Ressources_Specifiques_CI;
            return View();
        }
    }
}
