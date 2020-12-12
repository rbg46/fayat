using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour AuthentificationLogIndex
  /// </summary>
  public class AuthentificationLogIndexBundleFactory : BundleFactory
  {
    public AuthentificationLogIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/AuthentificationLogIndexBundle.js");
      Js("~/Areas/AuthentificationLog/Scripts/Services/authentification-log.service.js",
           "~/Areas/AuthentificationLog/Scripts/authentification-log.controller.js",
           "~/Areas/AuthentificationLog/Scripts/Dialogs/authentification-log-detail.controller.js",
           "~/Scripts/Module/Notification/module.js");

      CssBundleName("~/AuthentificationLogIndexBundle.css");
      Css("~/Content/BarreRecherche/barre-recherche.css",
           "~/Areas/AuthentificationLog/Content/style.css",
           "~/Content/module/Notification/style.css");
    }
  }
}

