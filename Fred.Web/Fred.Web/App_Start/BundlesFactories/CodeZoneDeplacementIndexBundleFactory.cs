using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour CodeZoneDeplacementIndex
  /// </summary>
  public class CodeZoneDeplacementIndexBundleFactory : BundleFactory
  {
    public CodeZoneDeplacementIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/CodeZoneDeplacementIndexBundle.js");
      Js("~/Areas/CodeZoneDeplacement/Scripts/code-zone-deplacement.service.js",
           "~/Areas/CodeZoneDeplacement/Scripts/code-zone-deplacement.controller.js");

      CssBundleName("~/CodeZoneDeplacementIndexBundle.css");
      Css("~/Content/FormPanel/style.css",
           "~/Content/BarreRecherche/barre-recherche-light.css",
           "~/Content/Referentials/style.css",
           "~/Areas/CodeZoneDeplacement/Content/style.css");
    }
  }
}

