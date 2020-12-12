using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour CodeMajorationIndex
  /// </summary>
  public class CodeMajorationIndexBundleFactory : BundleFactory
  {
    public CodeMajorationIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/CodeMajorationIndexBundle.js");
      Js("~/Areas/CodeMajoration/Scripts/code-majoration.service.js",
           "~/Areas/CodeMajoration/Scripts/code-majoration.controller.js");

      CssBundleName("~/CodeMajorationIndexBundle.css");
      Css("~/Content/FormPanel/style.css",
           "~/Content/BarreRecherche/barre-recherche-light.css",
           "~/Content/Referentials/style.css",
           "~/Areas/CodeMajoration/Content/style.css");
    }
  }
}

