using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.Prime.Controllers
{
  /// <summary>
  /// Contrôleur MVC des EtablissementPaies
  /// </summary>
  [Authorize]
  public class PrimeController : Controller
  {
    /// <summary>
    /// GET: EtablissementPaie/EtablissementPaie
    /// </summary>
    /// <returns>Retourne une action MVC</returns>
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuPrimeIndex)]
    public ActionResult Index()
    {
      ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Prime;
      return this.View();
    }
  }
}