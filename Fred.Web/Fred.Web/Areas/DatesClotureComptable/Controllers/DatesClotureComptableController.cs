using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.DatesClotureComptable.Controllers
{
  /// <summary>
  /// Contrôleur MVC des DatesClotureComptable
  /// </summary>
  [Authorize]
  public class DatesClotureComptableController : Controller
  {
    /// <summary>
    /// GET: Module/Module
    /// </summary>
    /// <returns>Retourne une action MVC</returns>
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuDatesClotureComptableIndex)]
    public ActionResult Index()
    {
      ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Cloture_Comptable;
      return View();
    }
  }
}