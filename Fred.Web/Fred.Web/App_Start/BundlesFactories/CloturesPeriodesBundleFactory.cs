using System.Web.Optimization;

namespace Fred.Web.App_Start.BundlesFactories
{
    public class CloturesPeriodesBundleFactory : BundleFactory
    {
        public CloturesPeriodesBundleFactory(BundleCollection bundles)
                  : base(bundles)
        {
            JsBundleName("~/CloturesPeriodesIndexBundle.js");
            Js("~/Areas/CloturesPeriodes/Scripts/clotures-periodes.controller.js",
                "~/Areas/CloturesPeriodes/Scripts/clotures-periodes.service.js",
                "~/Areas/CloturesPeriodes/Scripts/components/clotures-periodes-filter.component.js",
                "~/Scripts/Angularjs/1.5.8/i18n/angular-locale_fr-fr.js",
                "~/Scripts/Module/Notification/module.js",
                 "~/Scripts/module/DateTimePicker/datetimepickerDirective.js",
                 "~/Areas/CI/Scripts/services/ci.service.js",
                 "~/Scripts/directive/searchFilter.js",
                 "~/Areas/Pointage/RapportListe/Scripts/Services/rapport.service.js",
                 "~/Scripts/module/BarreRecherche/barre-recherche.js",
                 "~/Scripts/helpers/throttle.js",
                 "~/Scripts/directive/table/fred-table.directive.js",
                 "~/Scripts/directive/changeHeightOnResize/change-height-on-resize.js");

            CssBundleName("~/CloturesPeriodesIndexBundle.css");
            Css("~/Areas/CloturesPeriodes/Content/style.css",
                "~/Content/module/Notification/style.css",
                  "~/Content/filtersBtn.css",
           "~/Content/BarreRecherche/rightSearch.css");

        }
    }
}
