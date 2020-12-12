using System.Web.Optimization;

namespace Fred.Web.App_Start.BundlesFactories
{
  /// <summary>
  /// Bundle pour ValorisationIndex
  /// </summary>
  public class ValorisationIndexBundleFactory : BundleFactory
  {
    public ValorisationIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/ValorisationIndexBundle.js");
      Js(
        "~/Areas/Valorisation/Scripts/valorisation.controller.js",
        "~/Areas/Valorisation/Scripts/services/valorisation.service.js",
        "~/Scripts/helpers/throttle.js",
        "~/Scripts/module/DateTimePicker/datetimepickerDirective.js",
        "~/Scripts/directive/table/fred-table.directive.js");

      CssBundleName("~/ValorisationIndexBundle.css");
      Css("~/Areas/Valorisation/Content/Style.css",
           "~/Content/BarreRecherche/barre-recherche.css");
    }
  }
}