using System.Web.Optimization;

namespace Fred.Web.App_Start.BundlesFactories
{
  /// <summary>
  /// Pointage Etam iac synthese bundle factory
  /// </summary>
  public class ValidationDeMonServiceBundleFactory : BundleFactory
  {
    public ValidationDeMonServiceBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/ValidationDeMonServiceBundle.js");
      Js(
          "~/Areas/Pointage/ValidationDeMonService/Scripts/pointage-etam-iac-synthese-mensulle.controller.js",
          "~/Areas/Pointage/ValidationDeMonService/Scripts/Components/pointage-etam-iac-entree-synthese-mensuelle.js",

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

      CssBundleName("~/ValidationDeMonServiceBundle.css");
      Css(
          "~/Areas/Pointage/ValidationDeMonService/Content/PointageEtamIacSynthese.css",
          "~/Areas/Pointage/RapportHebdo/Content/PointageHebdoRapport.css"
          );
    }
  }
}
