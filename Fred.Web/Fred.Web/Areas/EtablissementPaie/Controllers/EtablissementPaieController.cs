using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.EtablissementPaie.Controllers
{
  /// <summary>
  /// Contrôleur MVC des EtablissementPaies
  /// </summary>
  [Authorize]
  public class EtablissementPaieController : Controller
  {
    /// <summary>
    /// GET: EtablissementPaie/EtablissementPaie
    /// </summary>
    /// <returns>Retourne une action MVC</returns>
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuEtablissementPaieIndex)]
    public ActionResult Index()
    {
      ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Etablissement_Paie;
      return this.View();
    }
  }
}