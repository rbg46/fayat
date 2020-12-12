using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour AreasReferentialViewsDeviseIndex
  /// </summary>
  public class AreasReferentialViewsDeviseIndexBundleFactory : BundleFactory
  {
    public AreasReferentialViewsDeviseIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/AreasReferentialViewsDeviseIndexBundle.js");
      Js("~/Scripts/Module/ProgressBar/module.js",
           "~/Scripts/Module/Notification/module.js",
           "~/Areas/Referential/Scripts/devise.service.js",
           "~/Areas/Referential/Scripts/devise.controller.js");

      CssBundleName("~/AreasReferentialViewsDeviseIndexBundle.css");
      Css("~/Content/module/ProgressBar/style.css",
           "~/Content/module/Notification/style.css",
           "~/Content/FormPanel/style.css",
           "~/Content/BarreRecherche/barre-recherche-light.css",
           "~/Content/Referentials/style.css",
           "~/Areas/Referential/Content/style.css");
    }
  }
}

