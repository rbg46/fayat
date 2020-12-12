using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour ValidationPointageIndex
  /// </summary>
  public class ValidationPointageIndexBundleFactory : BundleFactory
  {
    public ValidationPointageIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/ValidationPointageIndexBundle.js");
      Js("~/Areas/ValidationPointage/Scripts/ControlePointageErreur/controle-pointage-erreur.component.js",
           "~/Areas/ValidationPointage/Scripts/LotPointage/lot-pointage.component.js",
           "~/Areas/ValidationPointage/Scripts/Modals/controle-vrac-modal.component.js",
           "~/Areas/ValidationPointage/Scripts/Modals/remontee-vrac-modal.component.js",
           "~/Areas/ValidationPointage/Scripts/Modals/remontee-vrac-erreur-modal.component.js",
           "~/Areas/ValidationPointage/Scripts/Services/validation-pointage.service.js",
           "~/Areas/ValidationPointage/Scripts/validation-pointage.controller.js",
           "~/Areas/ValidationPointage/Scripts/Modals/verification-ci-sep-modal.component.js",
           "~/Scripts/module/DateTimePicker/datetimepickerDirective.js");

      CssBundleName("~/ValidationPointageIndexBundle.css");
      Css("~/Areas/ValidationPointage/Content/validation-pointage.css",
           "~/Content/BarreRecherche/barre-recherche-light.css",
           "~/Content/Referentials/style.css");
    }
  }
}

