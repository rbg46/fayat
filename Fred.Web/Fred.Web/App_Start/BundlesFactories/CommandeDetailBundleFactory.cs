using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
    /// <summary>
    /// Bundle pour CommandeDetail
    /// </summary>
    public class CommandeDetailBundleFactory : BundleFactory
    {
        public CommandeDetailBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/CommandeDetailBundle.js");
            Js("~/Areas/Commande/Scripts/commande.service.js",
                 "~/Areas/DatesClotureComptable/Scripts/dates-cloture-comptable.service.js",
                 "~/Areas/Commande/Scripts/commande-detail.controller.js",
                 "~/Scripts/Module/Google/Map/module.js",
                 "~/Scripts/module/DateTimePicker/datetimepickerDirective.js",

                 //Modals
                 "~/Areas/Commande/Scripts/modals/post-commande-validation-modal.component.js",
                 "~/Areas/Commande/Scripts/modals/validate-commande-modal.component.js",
                 "~/Areas/Commande/Scripts/modals/import-lignes-commande-excel-modal.js",
                 //Services
                 "~/Scripts/Module/Fayat/Service.js",
                 "~/Areas/Commande/Scripts/services/commande-attachment.service.js",
                 "~/Areas/Commande/Scripts/services/commande-helper.service.js",
                 "~/Areas/Commande/Scripts/services/commande-divers-service.js",
                 "~/Areas/Commande/Scripts/services/commande-historique.service.js",
                 "~/Areas/Commande/Scripts/services/commande-add-avenant-provider.service.js",
                 "~/Areas/Commande/Scripts/services/commande-ligne-manager.service.js",
                 "~/Areas/Commande/Scripts/services/commande-import-export-excel.service.js",
                 "~/Areas/Commande/Scripts/services/commande-ligne-lock.service.js",
                 // Components
                 "~/Areas/Commande/Scripts/components/panel-historique.component.js",
                 "~/Areas/Commande/Scripts/components/commande-ligne-verrou.component.js",

                 //Helpers
                 "~/Scripts/helpers/exportFile.js",

                 // Lookup spécifique fournisseurs - agences
                 "~/Scripts/Components/LookupPanel/Custom/lookup-panel-fournisseur-agence.component.js",
                 "~/Scripts/directive/lookup/custom-header-templates/personnel-with-seuil-validation-header.component.js",
                 "~/Areas/Commande/Scripts/modals/verify-external-material-unity-modal.component.js");

            CssBundleName("~/CommandeDetailBundle.css");
            Css("~/Areas/Commande/Content/style.css");
        }
    }
}
