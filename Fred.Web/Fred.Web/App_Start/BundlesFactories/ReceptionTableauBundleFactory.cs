using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
    /// <summary>
    /// Bundle pour ReceptionTableau
    /// </summary>
    public class ReceptionTableauBundleFactory : BundleFactory
    {
        public ReceptionTableauBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/ReceptionTableauBundle.js");
            Js("~/Areas/Reception/Scripts/tableau/services/reception-tableau.service.js",
               "~/Areas/Reception/Scripts/tableau/services/reception-tableau-selector.service.js",
               "~/Areas/Reception/Scripts/tableau/services/reception-tableau-data-format.services.js",
               "~/Areas/Reception/Scripts/tableau/services/reception-tableau-is-visable.service.js",
               "~/Areas/Reception/Scripts/tableau/services/reception-stamp-button.service.js",
               "~/Areas/Reception/Scripts/tableau/services/reception-tableau-quantity.service.js",
               "~/Areas/Reception/Scripts/tableau/reception-tableau.controller.js",
               "~/Scripts/module/DateTimePicker/datetimepickerDirective.js",
               "~/Scripts/directive/searchFilter.js",
               "~/Scripts/helpers/throttle.js",
               "~/Areas/Pointage/RapportListe/Scripts/Services/rapport.service.js",
               "~/Scripts/directive/table/fred-table.directive.js",
               "~/Scripts/directive/changeHeightOnResize/change-height-on-resize.js");

            CssBundleName("~/ReceptionTableauBundle.css");
            Css("~/Areas/Reception/Content/tableau.css",
                 "~/Content/BarreRecherche/rightSearch.css");
        }
    }
}
