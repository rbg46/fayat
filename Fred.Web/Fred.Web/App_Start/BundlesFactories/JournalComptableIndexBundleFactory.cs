using System.Web.Optimization;

namespace Fred.Web.App_Start.BundlesFactories
{
  public class JournalComptableIndexBundleFactory : BundleFactory
  {
    public JournalComptableIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/JournalComptableIndexBundle.js");
      Js("~/Areas/JournalComptable/Scripts/journal-comptable.service.js",
           "~/Areas/JournalComptable/Scripts/journal-comptable.controller.js");

      CssBundleName("~/JournalComptableIndexBundle.css");
      Css("~/Content/FormPanel/style.css",
           "~/Content/BarreRecherche/barre-recherche-light.css",
           "~/Content/Referentials/style.css");
    }
  }
}