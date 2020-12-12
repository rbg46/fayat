using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour ViewsHomeIndex
  /// </summary>
  public class ViewsHomeIndexBundleFactory : BundleFactory
  {
    public ViewsHomeIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/ViewsHomeIndexBundle.js");
      Js("~/Scripts/Controllers/HomeController.js",
           "~/Scripts/Controllers/main/main.controller.js");

      CssBundleName("~/ViewsHomeIndexBundle.css");
      Css("~/Content/dashboard.css");
    }
  }
}

