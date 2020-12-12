using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour EtablissementPaieIndex
  /// </summary>
  public class EtablissementPaieIndexBundleFactory : BundleFactory
  {
    public EtablissementPaieIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/EtablissementPaieIndexBundle.js");
      Js("~/vendor/bower_components/ngmap/build/scripts/ng-map.min.js",
           "~/Areas/EtablissementPaie/Scripts/etablissement-paie.service.js",
           "~/Areas/EtablissementPaie/Scripts/etablissement-paie.controller.js",
           "~/Scripts/module/Google/google.service.js",
           "~/Scripts/module/Google/adresse.model.js");

      CssBundleName("~/EtablissementPaieIndexBundle.css");
      Css("~/Content/FormPanel/style.css",
           "~/Content/BarreRecherche/barre-recherche-light.css",
           "~/Content/Referentials/style.css",
           "~/Areas/EtablissementPaie/Content/style.css",
           "~/Content/ReferentialPickList/ReferentialPickListCaller.css",
           "~/Content/Autocomplete/style.css");
    }
  }
}

