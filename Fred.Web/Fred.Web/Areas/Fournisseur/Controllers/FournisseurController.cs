using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.Fournisseur.Controllers
{
  [Authorize]
  public class FournisseurController : Controller
  {
    /// <summary>
    ///   Action view Index
    /// </summary>
    /// <param name="id">Identifiant du favori</param>
    /// <returns>Return Index View</returns>
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuFournisseurIndex)]
    public ActionResult Index(int? id)
    {
      ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Founisseur;
      ViewBag.favoriId = id;
      return View();
    }
  }
}
