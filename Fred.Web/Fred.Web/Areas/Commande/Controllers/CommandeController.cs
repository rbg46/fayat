using Fred.Business.Habilitation.Interfaces;
using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.Commande.Controllers
{
  /// <summary>
  /// Contrôleur MVC des commandes
  /// </summary>
  [Authorize()]
  public class CommandeController : Controller
  {
    private readonly IHabilitationForCommandeManager habilitationForCommandeManager;

    public CommandeController(IHabilitationForCommandeManager habilitationForCommandeManager)
    {
      this.habilitationForCommandeManager = habilitationForCommandeManager;
    }

    /// <summary>
    /// GET: Commande/Commande
    /// </summary>
    /// <param name="favoriId">favoriId</param>
    /// <returns>Retourne une action MVC</returns>
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuCommandeIndex)]
    public ActionResult Index(int? favoriId)
    {
      ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Commande;
      ViewBag.favoriId = favoriId;
      return this.View();
    }

    /// <summary>
    /// GET: Commande/Commande/Détail/id
    /// </summary>
    /// <param name="id">Identifiant de la commande</param>
    /// <param name="duplicate">duplicate</param>
    /// <returns>Retourne une action MVC</returns>
    [Route("Commande/Commande/Detail/{id?}/{duplicate}")]
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuCommandeDetail)]
    public ActionResult Detail(int? id = null, bool duplicate = false)
    {
      bool isAuthorized = habilitationForCommandeManager.IsAuthorizedForEntity(id, true);
      if (!isAuthorized)
      {
        return RedirectToAction("UnAuthorized", "Error", new { area = string.Empty });
      }
      ViewBag.id = id;
      ViewBag.duplicate = duplicate;
      return this.View();
    }

    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuCommandeIndex)]
    public ActionResult SearchFilterTemplate()
    {
      return this.View();
    }
  }
}