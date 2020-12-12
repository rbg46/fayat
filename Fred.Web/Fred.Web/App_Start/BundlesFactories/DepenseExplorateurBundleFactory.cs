using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
    /// <summary>
    /// Bundle pour DepenseExplorateur
    /// </summary>
    public class DepenseExplorateurBundleFactory : BundleFactory
    {
        public DepenseExplorateurBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/DepenseExplorateurBundle.js");
            Js("~/Areas/Depense/Scripts/explorateur-depense.controller.js",
               "~/Areas/Depense/Scripts/explorateur-depense-helper.service.js",
               "~/Areas/Depense/Scripts/modals/export-depense-modal.component.js",
               "~/Areas/Depense/Scripts/modals/replace-task-modal.component.js",
               "~/Areas/Depense/Scripts/modals/replace-task-history-modal.component.js",
               "~/Areas/Depense/Scripts/services/depense.service.js",
               "~/Areas/Depense/Scripts/services/remplacement-tache.service.js",
               "~/Areas/CI/Scripts/services/ci.service.js",
               "~/Areas/FamilleOperationDiverse/Scripts/famille-od.service.js",
               "~/Scripts/module/DateTimePicker/datetimepickerDirective.js",
               "~/Scripts/helpers/throttle.js",
               "~/Scripts/directive/table/fred-table.directive.js",
               "~/Scripts/directive/changeHeightOnResize/change-height-on-resize.js",
               "~/Scripts/directive/table/fred-table-header.directive.js",
               "~/Scripts/Components/LookupPanel/Custom/lookup-panel-fournisseur-agence.component.js"
               );

            CssBundleName("~/DepenseExplorateurBundle.css");
            Css("~/Content/FormPanel/style.css",
                "~/Areas/Depense/Content/ExplorateurDepense.css",
                "~/Scripts/directive/table/fred-table-header.css",
                "~/Content/BarreRecherche/rightSearch.css");
        }
    }
}

