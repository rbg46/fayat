using System.Collections.Generic;
using Fred.Business.FeatureFlipping;
using Fred.Framework.FeatureFlipping;

namespace Fred.Business.Authentification
{
    public class MaintenanceManager : IMaintenanceManager
    {
        private readonly List<string> authorizedLogins = new List<string>()
        {
            "super_fred",
            "super_RB",
            "madmin",
            "admRZB",
            "gspsatelec"
        };

        private readonly IFeatureFlippingManager featureFlippingManager;

        public MaintenanceManager(IFeatureFlippingManager featureFlippingManager)
        {
            this.featureFlippingManager = featureFlippingManager;
        }

        public bool IsAuthorizedToAccessTheWebsite(string login)
        {
            if (!featureFlippingManager.IsActivated(EnumFeatureFlipping.WebSiteInMaintenance))
            {
                return true;
            }

            return authorizedLogins.Contains(login);
        }

    }
}
