using System.Web.Mvc;
using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;

namespace Fred.Web.Areas.RessourcesRecommandees.Controllers
{
    [Authorize]
    public class RessourcesRecommandeesController : Controller
    {
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuRessourcesRecommandeesIndex)]
        // GET: RessourcesRecommandees/RessourcesRecommandees
        public ActionResult Index()
        {
            ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Ressources_Recommandees;

            return View();
        }
    }
}
