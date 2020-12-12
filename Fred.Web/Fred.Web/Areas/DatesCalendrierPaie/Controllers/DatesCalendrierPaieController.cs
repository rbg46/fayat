using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.DatesCalendrierPaie.Controllers
{
  /// <summary>
  /// Contrôleur MVC des DatesCalendrierPaie
  /// </summary>
  [Authorize]
  public class DatesCalendrierPaieController : Controller
  {
    /// <summary>
    /// GET: Module/Module
    /// </summary>
    /// <returns>Retourne une action MVC</returns>
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuDatesCalendrierPaieIndex)]
    public ActionResult Index()
    {
      ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Date_Paie;
      return View();
    }
  }
}