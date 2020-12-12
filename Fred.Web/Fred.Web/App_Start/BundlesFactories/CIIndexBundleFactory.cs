using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
    /// <summary>
    /// Bundle pour CIIndex
    /// </summary>
    public class CIIndexBundleFactory : BundleFactory
    {
        public CIIndexBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/CIIndexBundle.js");
            Js("~/Scripts/directive/searchFilter.js",
                 "~/Areas/CI/Scripts/ci.controller.js",
                 "~/Areas/CI/Scripts/services/ci.service.js",
                 "~/Scripts/module/DateTimePicker/datetimepickerDirective.js",
                 "~/Areas/Pointage/RapportListe/Scripts/Services/rapport.service.js",
                 "~/Scripts/module/BarreRecherche/barre-recherche.js",
                  "~/Scripts/helpers/throttle.js",
                 "~/Scripts/directive/table/fred-table.directive.js",
                 "~/Scripts/directive/changeHeightOnResize/change-height-on-resize.js");

            CssBundleName("~/CIIndexBundle.css");
            Css("~/Areas/CI/Content/indexCi.css",
                 "~/Content/filtersBtn.css",
                 "~/Content/BarreRecherche/barre-recherche.css",
                 "~/Content/BarreRecherche/rightSearch.css");
        }
    }
}

