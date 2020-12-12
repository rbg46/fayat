using System.Web.Mvc;
using Fred.Business.Habilitation.Interfaces;
using Fred.Entities.Permission;
using Fred.Web.Modules.Authorization;

namespace Fred.Web.Areas.Rapport.Controllers
{
    /// <summary>
    /// Controlleur MVC des rapports de pointage
    /// </summary>
    [Authorize]
    public class RapportController : Controller
    {
        private readonly IHabilitationForRapportManager habilitationForRapportManager;

        public RapportController(IHabilitationForRapportManager habilitationForRapportManager)
        {
            this.habilitationForRapportManager = habilitationForRapportManager;
        }

        /// <summary>
        /// Détail d'un rapport
        /// </summary>
        /// <returns>Retourne une action MVC</returns>
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuAreasPointageViewsRapportNew)]
        [Route("Pointage/Rapport/New")]
        public ActionResult New()
        {
            ViewBag.id = default(int?);
            ViewBag.duplicate = false;
            ViewBag.titre = ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Pointage;
            return this.View("Detail");
        }

        /// <summary>
        /// Détail d'un rapport
        /// </summary>
        /// <param name="id">Identifiant du rapport</param>
        /// <param name="duplicate">???</param>
        /// <returns>Retourne une action MVC</returns>
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuAreasPointageViewsRapportDetail)]
        [Route("Pointage/Rapport/Detail/{id}/{duplicate?}")]
        public ActionResult Detail(int id, bool duplicate = false)
        {
            bool isAuthorized = habilitationForRapportManager.IsAuthorizedForEntity(id, true);
            if (!isAuthorized)
            {
                return RedirectToAction("UnAuthorized", "Error", new { area = string.Empty });
            }
            ViewBag.id = id;
            ViewBag.duplicate = duplicate;
            ViewBag.titre = Fred.Web.Shared.App_LocalResources.Page_Title.Title_App + Fred.Web.Shared.App_LocalResources.Page_Title.Title_Pointage;
            return this.View();
        }

        [Route("Pointage/Rapport/Index/{favoriId?}")]
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuAreasPointageViewsRapportIndex)]
        public ActionResult Index(int? favoriId = 0)
        {
            ViewBag.titre = "Liste des rapports";
            ViewBag.favoriId = favoriId;
            return this.View();
        }

        [Route("Pointage/Rapport/PointageHebdoRapportOuvrier")]
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuAreasPointageRapportHebdomadaire)]
        public ActionResult PointageHebdoRapportOuvrier()
        {
            return this.View();
        }

        [Route("Pointage/Rapport/PointageHebdoRapportETAMIAC")]
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuAreasPointageRapportETAMIAC)]
        public ActionResult PointageHebdoRapportETAMIAC()
        {
            return this.View();
        }

        [Route("Pointage/Rapport/PointageEtamIacSyntheseMensuelle")]
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuAreasPointageRapportSyntheseMensuelle)]
        public ActionResult PointageEtamIacSyntheseMensuelle()
        {
            return this.View();
        }

        [Route("Pointage/Rapport/ValidationAffairesOuvriers")]
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuAreasValidationAffairesOuvriersForResponsable)]
        public ActionResult ValidationAffairesOuvriers()
        {
            return this.View();
        }
    }
}
