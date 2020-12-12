using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
    /// <summary>
    /// Bundle pour Commande Energie Détail
    /// </summary>
    public class CommandeEnergieDetailBundleFactory : BundleFactory
    {
        public CommandeEnergieDetailBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/CommandeEnergieDetailBundle.js");
            Js("~/Areas/CommandeEnergie/Scripts/services/commande-energie.service.js",
               "~/Areas/CommandeEnergie/Scripts/services/commande-energie-helper.service.js",
               "~/Areas/CommandeEnergie/Scripts/services/type-energie.service.js",
               "~/Areas/CommandeEnergie/Scripts/commande-energie-detail.controller.js",
               "~/Areas/CommandeEnergie/Scripts/modals/commande-energie-header-modal.component.js",
               "~/Scripts/module/DateTimePicker/datetimepickerDirective.js");

            CssBundleName("~/CommandeEnergieDetailBundle.css");
            Css("~/Areas/CommandeEnergie/Content/commande-energie-detail.css",
                "~/Areas/Commande/Content/style.css");
        }
    }
}

