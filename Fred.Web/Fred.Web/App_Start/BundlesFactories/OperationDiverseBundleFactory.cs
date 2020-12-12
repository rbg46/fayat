using System.Web.Optimization;

namespace Fred.Web.App_Start.BundlesFactories
{
    public class OperationDiverseBundleFactory : BundleFactory
    {
        public OperationDiverseBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/OperationDiverseIndexBundle.js");
            Js("~/Areas/OperationDiverse/Scripts/operation-diverse.controller.js",
               "~/Scripts/Angularjs/1.5.8/i18n/angular-locale_fr-fr.js",
               "~/Areas/OperationDiverse/Scripts/services/operation-diverse.service.js",
               "~/Areas/DatesClotureComptable/Scripts/dates-cloture-comptable.service.js",
               "~/Areas/OperationDiverse/Scripts/modals/ventilation-modal.controller.js",
               "~/Areas/OperationDiverse/Scripts/modals/ecart-modal.controller.js",
               "~/Areas/OperationDiverse/Scripts/services/ventilation.service.js",
               "~/Areas/OperationDiverse/Scripts/detaille.controller.js",
               "~/Areas/CI/Scripts/services/ci.service.js",
               "~/Areas/CloturesPeriodes/Scripts/clotures-periodes.service.js",
               "~/Scripts/module/DateTimePicker/datetimepickerDirective.js");

            CssBundleName("~/OperationDiverseIndexBundle.css");
            Css("~/Areas/OperationDiverse/Content/Style.css",
                "~/Content/FormPanel/style.css",
                "~/Areas/OperationDiverse/Content/autresFrais.css",
                "~/Areas/OperationDiverse/Content/Immo.css");
        }
    }
}

