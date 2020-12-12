using System.Web.Optimization;

namespace Fred.Web.App_Start.BundlesFactories
{
    public class CacheBundleFactory : BundleFactory
    {
        public CacheBundleFactory(BundleCollection bundle)
          : base(bundle)
        {
            JsBundleName("~/CacheBundleFactory.js");
            Js("~/Areas/Cache/Scripts/cache.service.js",
               "~/Areas/Cache/Scripts/cache.controller.js");

            CssBundleName("~/CacheBundleFactory.css");
            Css("~/Content/module/ProgressBar/style.css",
                 "~/Content/module/Notification/style.css",
                 "~/Areas/Cache/Content/style.css",
                 "~/Content/Referentials/style.css",
                 "~/Areas/Referential/Content/style.css");
        }
    }
}
