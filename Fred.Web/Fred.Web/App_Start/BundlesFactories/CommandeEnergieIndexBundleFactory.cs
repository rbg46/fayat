using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
    /// <summary>
    /// Bundle pour Commande Energie Index
    /// </summary>
    public class CommandeEnergieIndexBundleFactory : BundleFactory
    {
        public CommandeEnergieIndexBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/CommandeEnergieIndexBundle.js");
            Js("~/Areas/CommandeEnergie/Scripts/services/commande-energie.service.js",
               "~/Areas/CommandeEnergie/Scripts/commande-energie-list.controller.js",
               "~/Scripts/module/DateTimePicker/datetimepickerDirective.js");

            CssBundleName("~/CommandeEnergieIndexBundle.css");
            Css("~/Areas/CommandeEnergie/Content/commande-energie-list.css",
                "~/Areas/Commande/Content/style.css");
        }
    }
}

