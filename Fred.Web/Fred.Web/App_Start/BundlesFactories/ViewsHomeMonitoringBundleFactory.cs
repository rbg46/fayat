using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour ViewsHomeMonitoring
  /// </summary>
  public class ViewsHomeMonitoringBundleFactory : BundleFactory
  {
    public ViewsHomeMonitoringBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/ViewsHomeMonitoringBundle.js");
      Js("~/Scripts/Controllers/homeController.js");
    }
  }
}

