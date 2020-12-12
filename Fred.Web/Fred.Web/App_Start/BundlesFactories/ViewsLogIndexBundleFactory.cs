using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour ViewsLogIndex
  /// </summary>
  public class ViewsLogIndexBundleFactory : BundleFactory
  {
    public ViewsLogIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/ViewsLogIndexBundle.js");
      Js("~/Scripts/Log/order.js");

      CssBundleName("~/ViewsLogIndexBundle.css");
      Css("~/Content/View/log.css");
    }
  }
}

