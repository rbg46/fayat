using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.EtablissementComptable.Controllers
{
  /// <summary>
  /// Contrôleur MVC des modules
  /// </summary>
  /// 
  [Authorize]
  public class EtablissementComptableController : Controller
  {
    /// <summary>
    /// GET: EtablissementComptableController/EtablissementComptableController
    /// </summary>
    /// <returns>Retourne une action MVC</returns>
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuEtablissementComptableIndex)]
    public ActionResult Index()
    {
      ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Etablissement_Comptable;
      return View();
    }
  }
}