using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour ReferentielEtenduIndex
  /// </summary>
  public class ReferentielEtenduIndexBundleFactory : BundleFactory
  {
    public ReferentielEtenduIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/ReferentielEtenduIndexBundle.js");
      Js("~/Areas/ReferentielEtendu/Scripts/referentieletendu.service.js",
           "~/Areas/ReferentielEtendu/Scripts/referentieletendu.controller.js",
           "~/Areas/ReferentielEtendu/Scripts/referentiel-etendu-filter.service.js",
           "~/Areas/Nature/Scripts/nature.service.js",
           "~/Scripts/filter/code-libelle.filter.js");

      CssBundleName("~/ReferentielEtenduIndexBundle.css");
      Css("~/Areas/ReferentielEtendu/Content/style.css",
           "~/Content/FormPanel/style.css",
           "~/Content/ReferentialPickList/ReferentialPickList.css");
    }
  }
}

