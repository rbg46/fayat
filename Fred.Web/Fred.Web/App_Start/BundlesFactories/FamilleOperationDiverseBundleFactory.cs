using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
    /// <summary>
    /// Bundle pour FamilleOperationDiverseBundleFactory
    /// </summary>
    public class FamilleOperationDiverseBundleFactory : BundleFactory
    {
        public FamilleOperationDiverseBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/FamilleOperationDiverseBundleFactory.js");
            Js("~/Scripts/Module/ProgressBar/module.js",
                "~/Scripts/Module/Notification/module.js",
                "~/Areas/FamilleOperationDiverse/Scripts/famille-od.service.js",
                "~/Areas/FamilleOperationDiverse/Scripts/famille-od.controller.js",
                "~/Areas/Nature/Scripts/nature.service.js",
                "~/Areas/JournalComptable/Scripts/journal-comptable.service.js",
                "~/Areas/FamilleOperationDiverse/Scripts/fod-natures-journaux.controller.js",
                "~/Areas/FamilleOperationDiverse/Scripts/modals/duplicate-parametrage-modal.component.js");
            CssBundleName("~/FamilleOperationDiverseBundleFactory.css");
            Css("~/Content/module/ProgressBar/style.css",
                "~/Content/module/Notification/style.css",
                "~/Content/FormPanel/style.css",
                "~/Content/BarreRecherche/barre-recherche-light.css",
                "~/Areas/FamilleOperationDiverse/Content/style.css");
        }
    }
}

