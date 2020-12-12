using System.Web.Mvc;

namespace Fred.Web.Areas.Facture.Controllers
{
  /// <summary>
  /// Contrôleur MVC des factures
  /// </summary>
  [Authorize]
  public class FactureController : Controller
  {
    /// <summary>
    /// GET: Facture/Facture
    /// </summary>
    /// <param name="id">Identifiant du favori de la recherche des factures</param>
    /// <returns>Retourne une action MVC</returns>    
    public ActionResult Index(int? id)
    {
      ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Facture;
      ViewBag.favoriId = id;
      return this.View();
    }

    [Route("Pointage/Rapport/SearchFilterTemplate")]
    public ActionResult SearchFilterTemplate()
    {
      return this.View();
    }
  }
}