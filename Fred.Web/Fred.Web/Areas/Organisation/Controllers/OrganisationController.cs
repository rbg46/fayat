using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.Organisation.Controllers
{
  [Authorize]
  public class OrganisationController : Controller
  {
    // GET: Organisation/Organisation
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuOrganisationIndex)]
    public ActionResult Index()
    {
      ViewBag.titre = ViewBag.titre = Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Organisation;
      return View();
    }
  }
}