using System.Web.Optimization;

namespace Fred.Web.App_Start.BundlesFactories
{
    /// <summary>
    /// Moyen bundle factory
    /// </summary>
    public class MoyenBundleFactory : BundleFactory
    {
        public MoyenBundleFactory(BundleCollection bundles)
      : base(bundles)
        {
            JsBundleName("~/MoyenBundle.js");
            Js("~/Areas/Moyen/Scripts/moyen.controller.js",
                "~/Scripts/module/BarreRecherche/barre-recherche.js",
                "~/Areas/Moyen/Scripts/Services/moyen.service.js",
                "~/Areas/Moyen/Scripts/Components/affectation-moyen.component.js",
                "~/Areas/Moyen/Scripts/Components/restitution-moyen.component.js",
                "~/Areas/Moyen/Scripts/Components/location-moyen.component.js",
                "~/Areas/Moyen/Scripts/Components/date-selection-moyen.component.js",
                "~/Areas/Moyen/Scripts/Components/Tables/rapport-moyen-table.component.js",
                "~/Scripts/module/DateTimePicker/datetimepickerDirective.js");

            CssBundleName("~/MoyenBundle.css");
            Css("~/Content/FormPanel/style.css",
                "~/Content/BarreRecherche/barre-recherche.css",
                "~/Content/BarreRecherche/rightSearch.css",
                "~/Areas/Moyen/Content/Moyen.css");
        }
    }
}
