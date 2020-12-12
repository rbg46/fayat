using System.Web.Optimization;

namespace Fred.Web.App_Start.BundlesFactories
{
    /// <summary>
    /// Bundle pour Rapport Prime
    /// </summary>
    public class RapportPrimeIndexBundleFactory : BundleFactory
    {
        /// <summary>
        /// instancie un RapportPrimeIndexBundleFactory
        /// </summary>
        /// <param name="bundles">bundle</param>
        public RapportPrimeIndexBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/RapportPrimeIndexBundle.js");
            Js(
                "~/Scripts/module/DateTimePicker/datetimepickerDirective.js",
                "~/Areas/RapportPrime/Scripts/rapport-prime.controller.js",
                "~/Areas/RapportPrime/Scripts/rapport-prime.service.js",
                "~/Areas/DatesClotureComptable/Scripts/dates-cloture-comptable.service.js",
                "~/Areas/RapportPrime/Scripts/rapportPrime-personnel-typePrime-CI.filter.js",
                "~/Scripts/directive/table/fred-table.directive.js",

                // Bootstrap Datepicker
                "~/Scripts/module/BootstrapDatepicker/js/bootstrap-datepicker.js",
                "~/Scripts/module/BootstrapDatepicker/js/locales/bootstrap-datepicker.fr.js"
            );

            CssBundleName("~/RapportPrimeIndexBundle.css");
            Css(
                "~/Content/FormPanel/style.css",
                "~/Content/BarreRecherche/barre-recherche-light.css",
                "~/Content/Referentials/style.css",
                "~/Areas/RapportPrime/Content/style.css",

                // Bootstrap Datepicker
                "~/Scripts/module/BootstrapDatepicker/js/bootstrap-datepicker3.css"
            );
        }
    }
}
