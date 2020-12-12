using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour CodeDeplacementIndex
  /// </summary>
  public class CodeDeplacementIndexBundleFactory : BundleFactory
  {
    public CodeDeplacementIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/CodeDeplacementIndexBundle.js");
      Js("~/Areas/CodeDeplacement/Scripts/code-deplacement.service.js",
           "~/Areas/CodeDeplacement/Scripts/code-deplacement.controller.js");

      CssBundleName("~/CodeDeplacementIndexBundle.css");
      Css("~/Content/FormPanel/style.css",
           "~/Content/BarreRecherche/barre-recherche-light.css",
           "~/Content/Referentials/style.css",
           "~/Areas/CodeDeplacement/Content/Style.css");
    }
  }
}

