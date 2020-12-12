using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
    /// <summary>
    /// Bundle pour ReceptionIndex
    /// </summary>
    public class ReceptionIndexBundleFactory : BundleFactory
    {
        public ReceptionIndexBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/ReceptionIndexBundle.js");
            Js("~/Scripts/helpers/throttle.js",
               "~/Areas/Reception/Scripts/services/reception.service.js",
               "~/Areas/Reception/Scripts/reception/reception.controller.js",
               "~/Scripts/module/DateTimePicker/datetimepickerDirective.js",
               "~/Scripts/module/BarreRecherche/barre-recherche.js",
               "~/Scripts/directive/searchFilter.js",
               "~/Scripts/services/groupe-feature.service.js",
               "~/Areas/Pointage/RapportListe/Scripts/Services/rapport.service.js",
               "~/Areas/Reception/Scripts/reception/components/reception-modal-mode-enum.js",
               "~/Areas/Reception/Scripts/reception/components/reception-modal.component.js",
               "~/Areas/Reception/Scripts/reception/components/add-reception-list-modal.component.js",
               "~/Areas/Reception/Scripts/reception/components/commande-buyer-modal.component.js",
               "~/Areas/Reception/Scripts/reception/components/commande-ligne-verrou.component.js",
               "~/Areas/Reception/Scripts/reception/components/filter-selected-tiles.component.js",
               "~/Areas/Reception/Scripts/reception/services/action-button-enable.service.js",
               "~/Areas/Reception/Scripts/reception/services/attachement-manager.service.js",
               "~/Areas/Reception/Scripts/reception/services/commande-commande-ligne-selector.service.js",
               "~/Areas/Reception/Scripts/reception/services/commande-formator.service.js",
               "~/Areas/Reception/Scripts/reception/services/commande-ligne-common.service.js",
               "~/Areas/Reception/Scripts/reception/services/commande-ligne-is-receptionnable.service.js",
               "~/Areas/Reception/Scripts/reception/services/commande-ligne-lock.service.js",
               "~/Areas/Reception/Scripts/reception/services/commande-ligne-visibility.service.js",
               "~/Areas/Reception/Scripts/reception/services/confirmation-popup.service.js",
               "~/Areas/Reception/Scripts/reception/services/current-bon-livraison-manager.service.js",
               "~/Areas/Reception/Scripts/reception/services/filter.service.js",
               "~/Areas/Reception/Scripts/reception/services/montant-quantite-pourcentage-calculator.service.js",
               // Lookup spécifique fournisseurs - agences
               "~/Scripts/Components/LookupPanel/Custom/lookup-panel-fournisseur-agence.component.js"
               );

            CssBundleName("~/ReceptionIndexBundle.css");
            Css("~/Areas/Reception/Content/style.css",
                "~/Content/BarreRecherche/barre-recherche.css",
                "~/Content/BarreRecherche/rightSearch.css");
        }
    }
}
