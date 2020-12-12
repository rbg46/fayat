using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.Reception.Controllers
{
  /// <summary>
  /// Contrôleur MVC des réceptions
  /// </summary>
  [Authorize]
  public class ReceptionController : Controller
  {
    /// <summary>
    /// GET: Réception/Réception
    /// </summary>
    /// <param name="favoriId">favoriId</param>
    /// <returns>Retourne une action MVC</returns>
    [Route("Reception/Reception/Index/")]
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuReceptionIndex)]
    public ActionResult Index(int? favoriId)
    {
      ViewBag.favoriId = favoriId;
      return this.View();
    }
    
    /// <summary>
    /// GET: Réception/Réception/Tableau
    /// </summary>
    /// <param name="favoriId">favoriId</param>
    /// <returns>Retourne une action MVC</returns>
    [Route("Reception/Reception/Tableau/")]
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuReceptionTableau)]
    public ActionResult Tableau(int? favoriId)
    {
      ViewBag.favoriId = favoriId;
      return this.View("Tableau");
    }
    /// <summary>
    /// Retourne la vue index avec les filtres
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="commandeSoldee">commandeSoldee</param>
    /// <returns>Retourne la vue index</returns>
    [Route("Reception/Reception/Search")]
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuReceptionIndex)]
    public ActionResult Search(string id, bool commandeSoldee)
    {
      ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Reception;
      ViewBag.id = id;
      ViewBag.commandeSoldee = commandeSoldee;
      return this.View("~/Areas/Reception/Views/Reception/Index.cshtml");
    }
  }
}