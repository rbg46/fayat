using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour MaterielIndex
  /// </summary>
  public class MaterielIndexBundleFactory : BundleFactory
  {
    public MaterielIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/MaterielIndexBundle.js");
      Js("~/Areas/Materiel/Scripts/materiel.service.js",
           "~/Areas/Materiel/Scripts/materiel.controller.js",
           "~/Scripts/Angularjs/1.5.8/i18n/angular-locale_fr-fr.js");

      CssBundleName("~/MaterielIndexBundle.css");
      Css("~/Content/FormPanel/style.css",
           "~/Content/BarreRecherche/barre-recherche-light.css",
           "~/Content/Referentials/style.css",
           "~/Areas/Materiel/Content/Style.css",
           "~/Content/ReferentialPickList/ReferentialPickListCaller.css");
    }
  }
}

