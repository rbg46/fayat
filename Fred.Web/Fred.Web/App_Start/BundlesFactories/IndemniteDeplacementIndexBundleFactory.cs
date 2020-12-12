using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour IndemniteDeplacementIndex
  /// </summary>
  public class IndemniteDeplacementIndexBundleFactory : BundleFactory
  {
    public IndemniteDeplacementIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/IndemniteDeplacementIndexBundle.js");
      Js("~/Areas/IndemniteDeplacement/Scripts/indemnite-deplacement.service.js",
           "~/Areas/IndemniteDeplacement/Scripts/indemnite-deplacement.controller.js",
           "~/Areas/Personnel/Scripts/personnel.service.js");

      CssBundleName("~/IndemniteDeplacementIndexBundle.css");
      Css("~/Content/FormPanel/style.css",
           "~/Content/BarreRecherche/barre-recherche-light.css",
           "~/Content/Referentials/style.css",
           "~/Areas/IndemniteDeplacement/Content/style.css",
           "~/Content/ReferentialPickList/ReferentialPickListCaller.css");
    }
  }
}

