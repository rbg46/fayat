using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.ReferentielFixes.Controllers
{
  [Authorize]
  public class ReferentielFixesController : Controller
  {
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuReferentielFixesIndex)]
    public ActionResult Index()
    {
      ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Referentiel_Fixe;
      return View();
    }
  }
}