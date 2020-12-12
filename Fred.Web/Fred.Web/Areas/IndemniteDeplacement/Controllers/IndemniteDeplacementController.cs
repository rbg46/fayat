using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.IndemniteDeplacement.Controllers
{
  /// <summary>
  /// Contrôleur MVC des modules
  /// </summary>
  [Authorize]
  public class IndemniteDeplacementController : Controller
  {
    /// <summary>
    /// GET: Module/Module
    /// </summary>
    /// <param name="id">Identifiant de l'indemnité de déplacement</param>
    /// <returns>Retourne une action MVC</returns>
    [Route("IndemniteDeplacement/IndemniteDeplacement/Index/{id?}")]
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuPersonnelEdit)]
    public ActionResult Index(int? id = null)
    {
      ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Indemnite_Deplacement;
      ViewBag.id = id;
      return View();
    }

  }
}