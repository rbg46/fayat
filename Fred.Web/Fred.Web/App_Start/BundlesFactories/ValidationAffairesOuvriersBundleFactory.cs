using System.Web.Optimization;

namespace Fred.Web.App_Start.BundlesFactories
{
    /// <summary>
    /// Pointage Etam iac synthese bundle factory
    /// </summary>
    public class ValidationAffairesOuvriersBundleFactory : BundleFactory
    {
        public ValidationAffairesOuvriersBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/ValidationAffairesOuvriersBundle.js");
            Js(
                "~/Areas/Pointage/ValidationAffairesOuvriers/validation-affaires-ouvriers.controller.js",
                "~/Areas/Pointage/ValidationAffairesOuvriers/Scripts/Components/validation-affaires-ouvriers-synthese.component.js",
                "~/Areas/Pointage/RapportHebdo/Scripts/Components/pointage-hebdo-rapport-astreinte-tab.js",
                "~/Areas/Pointage/RapportHebdo/Scripts/Components/pointage-hebdo-rapport-majoration-tab.js",
                "~/Areas/Pointage/RapportHebdo/Scripts/Components/pointage-hebdo-rapport-pointage-tab.js",
                "~/Areas/Pointage/RapportHebdo/Scripts/Components/pointage-hebdo-rapport-prime-tab.js",
                "~/Areas/Pointage/RapportHebdo/Scripts/Components/pointage-hebdo-rapport.component.js",
                "~/Scripts/module/DateTimePicker/datetimepickerDirective.js",
                "~/Areas/Pointage/RapportHebdo/Scripts/Modals/popup-commentaire.component.js",
                "~/Areas/Pointage/RapportHebdo/Scripts/Services/pointage-hebdo-service.js"
               );

            CssBundleName("~/ValidationAffairesOuvriersBundle.css");
            Css(
                "~/Areas/Pointage/ValidationAffairesOuvriers/Content/ValidationAffairesOuvriers.css",
                "~/Areas/Pointage/RapportHebdo/Content/PointageHebdoRapport.css"
                );
        }
    }
}
