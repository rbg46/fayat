using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.CodeDeplacement.Controllers
{
  /// <summary>
  /// Contrôleur MVC des CodeDeplacements
  /// </summary>
  [Authorize]
  public class CodeDeplacementController : Controller
  {
    /// <summary>
    /// GET: CodeDeplacement/CodeDeplacement
    /// </summary>
    /// <returns>Retourne une action MVC</returns>
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuCodeDeplacementIndex)]
    public ActionResult Index()
    {

      ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Code_Deplacement;
      return this.View();
    }
  }
}