using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
    /// <summary>
    /// Bundle pour ViewsAuthenticationConnect
    /// </summary>
    public class ViewsAuthenticationConnectBundleFactory : BundleFactory
    {
        public ViewsAuthenticationConnectBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/ViewsAuthenticationConnectBundle.js");
            Js("~/Scripts/Controllers/Authentication/connect.controller.js",
                "~/Scripts/Controllers/Authentication/resetPassword.controller.js",
                "~/Scripts/Controllers/Authentication/newPassword.controller.js");
        }
    }
}

