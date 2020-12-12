using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour EtablissementComptableIndex
  /// </summary>
  public class EtablissementComptableIndexBundleFactory : BundleFactory
  {
    public EtablissementComptableIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/EtablissementComptableIndexBundle.js");
      Js(
           "~/Areas/EtablissementComptable/Scripts/etablissement-comptable.service.js",
           "~/Areas/EtablissementComptable/Scripts/etablissement-comptable.controller.js");

      CssBundleName("~/EtablissementComptableIndexBundle.css");
      Css("~/Content/FormPanel/style.css",
           "~/Content/BarreRecherche/barre-recherche-light.css",
           "~/Content/Referentials/style.css",
           "~/Areas/EtablissementComptable/Content/style.css");
    }
  }
}

