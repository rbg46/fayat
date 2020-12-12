using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour RapprochementFactureIndex
  /// </summary>
  public class RapprochementFactureIndexBundleFactory : BundleFactory
  {
    public RapprochementFactureIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/RapprochementFactureIndexBundle.js");
      Js("~/Scripts/expr-eval/dist/bundle.min.js",
           "~/Areas/RapprochementFacture/Scripts/ListeFacture/ListeFactureComponent.js",
           "~/Areas/RapprochementFacture/Scripts/RapprochementFactureController.js",
           "~/Scripts/module/BarreRecherche/barre-recherche.js");

      CssBundleName("~/RapprochementFactureIndexBundle.css");
      Css("~/Areas/RapprochementFacture/Content/Style.css",
           "~/Content/BarreRecherche/barre-recherche.css");
    }
  }
}

