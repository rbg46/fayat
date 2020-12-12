using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.Nature.Controllers
{
  /// <summary>
  /// Contrôleur MVC des Natures
  /// </summary>
  [Authorize]
  public class NatureController : Controller
  {
    /// <summary>
    /// GET: Nature/Nature
    /// </summary>
    /// <returns>Retourne une action MVC</returns>
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuNatureIndex)]
    public ActionResult Index()
    {
      ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Nature;
      return this.View();
    }
  }
}