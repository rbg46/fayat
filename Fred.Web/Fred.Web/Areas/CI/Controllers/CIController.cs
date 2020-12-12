using System.Web.Mvc;
using Fred.Business.Habilitation.Interfaces;
using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;

namespace Fred.Web.Areas.CI.Controllers
{
    /// <summary>
    /// Contrôleur MVC des cis
    /// </summary>
    [Authorize]
    public class CIController : Controller
    {
        private readonly IHabilitationForCiManager habilitationForCiManager;

        public CIController(IHabilitationForCiManager habilitationForCiManager)
        {
            this.habilitationForCiManager = habilitationForCiManager;
        }

        /// <summary>
        /// GET: CI/CI/Détail/id
        /// </summary>
        /// <param name="id">Identifiant de la ci</param>
        /// <returns>Retourne une action MVC</returns>
        [Route("CI/CI/Detail/{id?}")]
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuCIDetail)]
        public ActionResult Detail(int? id = null)
        {
            bool isAuthorized = habilitationForCiManager.IsAuthorizedForEntity(id, true);
            if (!isAuthorized)
            {
                return RedirectToAction("UnAuthorized", "Error", new { area = string.Empty });
            }
            ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Ci;
            ViewBag.id = id;
            return this.View(id);
        }

        /// <summary>
        /// GET: Commande/Commande/Détail/id
        /// </summary>
        /// <param name="favoriId">Identifiant de la commande</param>
        /// <param name="duplicate">???</param>
        /// <returns>Retourne une action MVC</returns>
        [Route("CI/CI/Index/{favoriId?}/{duplicate}")]
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuCIIndex)]
        public ActionResult Index(int? favoriId = 0, bool duplicate = false)
        {
            ViewBag.favoriId = favoriId;
            ViewBag.duplicate = duplicate;
            return this.View();
        }

        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuCIIndex)]
        public ActionResult SearchFilterTemplate()
        {
            return this.View();
        }
    }
}
