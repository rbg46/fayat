using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.Referential.Controllers
{
  [Authorize]
  public class DeviseController : Controller
  {
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuAreasReferentialViewsDeviseIndex)]
    public ActionResult Index()
    {
      ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Referentiel;
      return View();
    }
  }
}
