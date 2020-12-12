using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour LookupIndex
  /// </summary>
  public class LookupIndexBundleFactory : BundleFactory
  {
    public LookupIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/LookupIndexBundle.js");
      Js("~/Areas/Lookup/Scripts/LookupController.js",
           "~/Areas/Lookup/Scripts/LookupPopupComponent.js");
    }
  }
}

