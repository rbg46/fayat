using System.Web.Optimization;

namespace Fred.Web.App_Start.BundlesFactories
{
    /// <summary>
    /// Pointage hebdomadaire : Ecrans d'entrée bundle
    /// </summary>
    public class RapportHebdoBundleFactory : BundleFactory
    {
        public RapportHebdoBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/RapportHebdoBundle.js");
            Js(  "~/Areas/Pointage/RapportHebdo/Scripts/pointage-hebdo-etam-iac.controller.js",
                 "~/Areas/Pointage/RapportHebdo/Scripts/pointage-hebdo-ouvrier.controller.js",

                 "~/Areas/Pointage/RapportHebdo/Scripts/Components/pointage-hebdo-entree-par-affaire.component.js",
                 "~/Areas/Pointage/RapportHebdo/Scripts/Components/pointage-hebdo-entree-par-ouvrier.component.js",
                 "~/Areas/Pointage/RapportHebdo/Scripts/Components/pointage-hebdo-rapport-astreinte-tab.js",
                 "~/Areas/Pointage/RapportHebdo/Scripts/Components/pointage-hebdo-rapport-majoration-tab.js",
                 "~/Areas/Pointage/RapportHebdo/Scripts/Components/pointage-hebdo-rapport-pointage-tab.js",
                 "~/Areas/Pointage/RapportHebdo/Scripts/Components/pointage-hebdo-rapport-prime-tab.js",
                 "~/Areas/Pointage/RapportHebdo/Scripts/Components/pointage-hebdo-rapport.component.js",

                 "~/Areas/Pointage/RapportHebdo/Scripts/Modals/popup-commentaire.component.js",

                 "~/Areas/Pointage/RapportHebdo/Scripts/Services/pointage-hebdo-service.js",

                 "~/Scripts/module/DateTimePicker/datetimepickerDirective.js",
                 "~/Scripts/Factory/favori/favori.component.js",
                 "~/Scripts/Factory/favori/favori.service.js"
               );

            CssBundleName("~/RapportHebdoBundle.css");
            Css(
                "~/Areas/Pointage/RapportHebdo/Content/PointageHebdoEntree.css",
                "~/Areas/Pointage/RapportHebdo/Content/PointageHebdoRapport.css",
                 "~/Content/BarreRecherche/barre-recherche.css"
               );
        }
    }
}
