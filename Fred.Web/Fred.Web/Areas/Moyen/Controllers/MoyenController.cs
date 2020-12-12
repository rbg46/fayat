using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;
using System.Web.Mvc;

namespace Fred.Web.Areas.Moyen.Controllers
{
  [Authorize]
  public class MoyenController : Controller
  {
    /// <summary>
    /// GET: Moyen/Moyen
    /// </summary>
    /// <returns>Retourne une action MVC</returns>  
    [Route("Moyen/Moyen/GestionMoyen")]
    [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageGestionMoyenIndex)]
    public ActionResult GestionMoyen()
    {
      return this.View();
    }
  }
}