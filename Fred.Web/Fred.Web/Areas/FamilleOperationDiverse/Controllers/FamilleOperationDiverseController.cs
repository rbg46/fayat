using System.Web.Mvc;
using Fred.Business.FeatureFlipping;
using Fred.Entities.Permission;
using Fred.Framework.FeatureFlipping;
using Fred.Web.Modules.Authorization;

namespace Fred.Web.Areas.FamilleOperationDiverse.Controllers
{
    /// <summary>
    /// Contrôleur MVC Famille Operation Diverse
    /// </summary>
    [Authorize()]
    public class FamilleOperationDiverseController : Controller
    {
        private readonly IFeatureFlippingManager featureFlippingManager;

        /// <summary>
        /// FamilleOperationDiverseController
        /// </summary>
        /// <param name="featureFlippingManager">Manager des features flipping</param>
        public FamilleOperationDiverseController(IFeatureFlippingManager featureFlippingManager)
        {
            this.featureFlippingManager = featureFlippingManager;
        }

        // GET: FamilleOperationDiverse/FamilleOperationDiverse
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuFamilleOperationDiverse)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET: FamilleOperationDiverse/ListFamilleOperationDiverse/List
        /// </summary>
        /// <returns>Retourne une action MVC</returns>
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuListFamilleOperationDiverse)]
        public ActionResult List()
        {
            if (featureFlippingManager.IsActivated(EnumFeatureFlipping.ActivationUS13085_6000))
            {
                return this.View();
            }

            return HttpNotFound();
        }

        /// <summary>
        /// GET: FamilleOperationDiverse/ListFamilleOperationDiverse/AssociationFodNaturesJournaux
        /// </summary>
        /// <returns>Retourne une action MVC</returns>
        [FredAspAuthorize(globalPermissionKey: PermissionKeys.AffichageMenuAssociationFodJournauxNatures)]
        public ActionResult AssociationFodNaturesJournaux()
        {
            if (featureFlippingManager.IsActivated(EnumFeatureFlipping.ActivationUS13085_6000))
            {
                return this.View();
            }

            return HttpNotFound();
        }
    }
}
