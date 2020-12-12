using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour ParamTarifsReferentielsIndex
  /// </summary>
  public class ParamTarifsReferentielsIndexBundleFactory : BundleFactory
  {
    public ParamTarifsReferentielsIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/ParamTarifsReferentielsIndexBundle.js");
      Js("~/Areas/ParamTarifsReferentiels/Scripts/param-tarifs-referentiels-helper.service.js",
           "~/Areas/ParamTarifsReferentiels/Scripts/param-tarifs-referentiels.service.js",
           "~/Areas/ParamTarifsReferentiels/Scripts/param-tarifs-referentiels.controller.js",
           "~/Areas/ReferentielEtendu/Scripts/referentiel-etendu-filter.service.js",
           "~/Scripts/helpers/throttle.js",
           "~/Scripts/directive/changeHeightOnResize/change-height-on-resize.js");

      CssBundleName("~/ParamTarifsReferentielsIndexBundle.css");
      Css("~/Areas/ParamTarifsReferentiels/Content/style.css");
    }
  }
}

