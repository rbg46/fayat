using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.Bareme.Controllers
{
  /// <summary>
  /// Controlleur exemple (multi-controlleur avec ExploitationController) pour la future gestion des barèmes budget.
  /// </summary>
  [Authorize()]
  public class BaremeController : Controller
  {
    /// <summary>
    /// GET: Bareme/Bareme/BudgetTest.
    /// </summary>
    /// <returns>La vue exemple</returns>
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuBaremeIndex)]
    public ActionResult Budget()
    {
      return View("BudgetTest");
    }
  }
}
