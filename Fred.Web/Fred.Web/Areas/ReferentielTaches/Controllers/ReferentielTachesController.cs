using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.ReferentielTaches.Controllers
{
  [Authorize]
  public class ReferentielTachesController : Controller
  {
    // GET: ReferentielTaches/ReferentielTaches

    
    /// <summary>
    /// GET: ReferentielTaches/ReferentielTaches
    /// </summary>
    /// <param name="favoriId">Identifiant de la commande</param>
    /// <returns>Retourne une action MVC</returns>
    [Route("ReferentielTaches/ReferentielTaches/{favoriId?}")]
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuReferentielTachesIndex)]
    public ActionResult Index(int? favoriId = 0)
    {
      ViewBag.favoriId = favoriId;
      ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Referentiel_Tache;
      return View();
    }
  }
}