using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour ReferentielFixesIndex
  /// </summary>
  public class ReferentielFixesIndexBundleFactory : BundleFactory
  {
    public ReferentielFixesIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/ReferentielFixesIndexBundle.js");
      Js("~/Areas/ReferentielFixes/Scripts/referentiel-fixe.service.js",
           "~/Areas/ReferentielFixes/Scripts/referentiel-fixe.controller.js",
           "~/Scripts/filter/code-libelle.filter.js",
           "~/Scripts/filter/truncate-text.filter.js");

      CssBundleName("~/ReferentielFixesIndexBundle.css");
      Css("~/Content/FormPanel/style.css",
           "~/Areas/ReferentielFixes/Content/style.css");
    }
  }
}

