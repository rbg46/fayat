using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.ReferentielEtendu.Controllers
{
  [Authorize]
  public class ReferentielEtenduController : Controller
  {
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuReferentielEtenduIndex)]
    public ActionResult Index()
    {
      ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Referentiel_Etendu;
      return View();
    }
  }
}
