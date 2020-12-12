using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.CodeMajoration.Controllers
{
  /// <summary>
  /// Contrôleur MVC des CodeMajorations
  /// </summary>
  [Authorize]
  public class CodeMajorationController : Controller
  {
    /// <summary>
    /// GET: CodeMajoration/CodeMajoration
    /// </summary>
    /// <returns>Retourne une action MVC</returns>
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuCodeMajorationIndex)]
    public ActionResult Index()
    {
      ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Code_Majoration;
      return this.View();
    }
  }
}