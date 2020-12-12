using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
    /// <summary>
    /// Bundle pour CommandeIndex
    /// </summary>
    public class CommandeIndexBundleFactory : BundleFactory
    {
        public CommandeIndexBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/CommandeIndexBundle.js");
            Js("~/Areas/Commande/Scripts/commande.service.js",
                 "~/Areas/Commande/Scripts/commande.controller.js",
                 "~/Scripts/directive/searchFilter.js",
                 "~/Scripts/module/DateTimePicker/datetimepickerDirective.js",
                 "~/Scripts/module/BarreRecherche/barre-recherche.js",
                 "~/Areas/Pointage/RapportListe/Scripts/Services/rapport.service.js",
                 "~/Scripts/helpers/throttle.js",
                 "~/Scripts/directive/table/fred-table.directive.js",
                 "~/Scripts/directive/changeHeightOnResize/change-height-on-resize.js",

                 // Lookup spécifique fournisseurs - agences
                 "~/Scripts/Components/LookupPanel/Custom/lookup-panel-fournisseur-agence.component.js"
                 );

            CssBundleName("~/CommandeIndexBundle.css");
            Css("~/Areas/Commande/Content/style.css",
                 "~/Content/BarreRecherche/barre-recherche-light.css",
                 "~/Content/BarreRecherche/barre-recherche.css",
                 "~/Content/BarreRecherche/rightSearch.css");
        }
    }
}

