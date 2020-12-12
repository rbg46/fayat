using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour PrimeIndex
  /// </summary>
  public class PrimeIndexBundleFactory : BundleFactory
  {
    public PrimeIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/PrimeIndexBundle.js");
      Js("~/Areas/Prime/Scripts/prime.service.js",
           "~/Areas/Prime/Scripts/prime.controller.js");

      CssBundleName("~/PrimeIndexBundle.css");
      Css("~/Content/FormPanel/style.css",
           "~/Content/BarreRecherche/barre-recherche-light.css",
           "~/Content/Referentials/style.css",
           "~/Areas/Prime/Content/style.css");
    }
  }
}

