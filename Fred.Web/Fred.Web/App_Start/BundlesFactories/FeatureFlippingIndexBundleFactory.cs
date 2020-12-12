using System.Web.Optimization;

namespace Fred.Web.App_Start.BundlesFactories
{
    public class FeatureFlippingIndexBundleFactory : BundleFactory
    {
        public FeatureFlippingIndexBundleFactory(BundleCollection bundle)
          : base(bundle)
        {
            JsBundleName("~/FeatureFlippingIndexBundleFactory.js");
            Js("~/Areas/FeatureFlipping/Scripts/feature-flipping.service.js",
               "~/Areas/FeatureFlipping/Scripts/feature-flipping.controller.js");

            CssBundleName("~/FeatureFlippingIndexBundleFactory.css");
            Css("~/Content/module/ProgressBar/style.css",
                 "~/Content/module/Notification/style.css",
                 "~/Areas/FeatureFlipping/Content/style.css",
                 "~/Content/Referentials/style.css",
                 "~/Areas/Referential/Content/style.css");
        }
    }
}
