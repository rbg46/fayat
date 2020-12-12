using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.Lookup.Controllers
{
  [Authorize]
  public class LookupController : Controller
  {
    // GET: Lookup/Lookup
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuLookupIndex)]
    public ActionResult Index()
    {
      ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Lookup;
      return View();
    }
  }
}