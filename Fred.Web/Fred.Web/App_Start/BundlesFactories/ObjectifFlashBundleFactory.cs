using System.Web.Optimization;

namespace Fred.Web.App_Start.BundlesFactories
{
    public class ObjectifFlashBundleFactory : BundleFactory
    {
        public ObjectifFlashBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/ObjectifFlashBundle.js");
            Js(
              "~/Scripts/module/DateTimePicker/datetimepickerDirective.js",

              //Service
              "~/Areas/BilanFlash/Scripts/services/objectif-flash.service.js",

              //Controller
              "~/Areas/BilanFlash/Scripts/objectif-flash.controller.js",

              //Panel Ressources
              "~/Areas/BilanFlash/Scripts/Ressources/ressources-component.js",
              "~/Areas/BilanFlash/Scripts/Ressources/Ressource/ressource-component.js",

              // Export Bilan Flash Modal
              "~/Areas/BilanFlash/Scripts/modals/export-bilanFlash-modal.component.js",

              // Ajout Quantité Modal
              "~/Areas/BilanFlash/Scripts/modals/ajout-quantite-modal.component.js",

              // Report Objectif Flash Modal
              "~/Areas/BilanFlash/Scripts/modals/report-et-duplication-objectifFlash-modal.component.js",

              //Confirmation Annulation/Activation/CLoture Objectif Flash
              "~/Areas/BilanFlash/Scripts/modals/confirmation-modal.component.js"
              );

            CssBundleName("~/ObjectifFlashBundle.css");
            Css("~/Areas/BilanFlash/Content/objectifFlash.css",
                "~/Areas/BilanFlash/Content/common.css",
                "~/Content/FormPanel/style.css");
        }
    }
}

