using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.ValidationPointage.Controllers
{
  [Authorize]
  public class ValidationPointageController : Controller
  {
    // GET: ValidationPointage/ValidationPointage
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuValidationPointageIndex)]
    public ActionResult Index()
    {
      ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Validation_Pointage;
      return View();
    }

  }
}