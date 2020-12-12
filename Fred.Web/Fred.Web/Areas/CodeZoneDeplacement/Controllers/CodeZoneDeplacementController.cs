using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.CodeZoneDeplacement.Controllers
{
  /// <summary>
  /// Contrôleur MVC des modules
  /// </summary>
  [Authorize]
  public class CodeZoneDeplacementController : Controller
  {
    /// <summary>
    /// GET: Module/Module
    /// </summary>
    /// <returns>Retourne une action MVC</returns>
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuCodeZoneDeplacementIndex)]
    public ActionResult Index()
    {
      ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Code_Zone_Deplacement;
      return View();
    }
  }
}