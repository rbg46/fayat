using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
    /// <summary>
    /// Bundle pour PersonnelIndex
    /// </summary>
    public class PersonnelIndexBundleFactory : BundleFactory
    {
        public PersonnelIndexBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/PersonnelIndexBundle.js");
            Js("~/Scripts/directive/searchFilter.js",
                 "~/Areas/Pointage/RapportListe/Scripts/Services/rapport.service.js",
                 "~/Areas/Personnel/Scripts/personnel.controller.js",
                 "~/Areas/Personnel/Scripts/personnel.service.js",
                 "~/Areas/Personnel/Scripts/personnel-search-filter.component.js",
                 "~/Scripts/helpers/throttle.js",
                 "~/Scripts/directive/table/fred-table.directive.js");

            CssBundleName("~/PersonnelIndexBundle.css");
            Css("~/Areas/Personnel/Content/Style.css",
                "~/Content/BarreRecherche/barre-recherche.css",
                "~/Content/BarreRecherche/rightSearch.css");
        }
    }
}

