using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour NatureIndex
  /// </summary>
  public class NatureIndexBundleFactory : BundleFactory
  {
    public NatureIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/NatureIndexBundle.js");
      Js("~/Areas/Nature/Scripts/nature.service.js",
           "~/Areas/Nature/Scripts/nature.controller.js");

      CssBundleName("~/NatureIndexBundle.css");
      Css("~/Content/FormPanel/style.css",
           "~/Content/BarreRecherche/barre-recherche-light.css",
           "~/Content/Referentials/style.css",
           "~/Areas/Nature/Content/style.css");
    }
  }
}

