using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour CodeAbsenceIndex
  /// </summary>
  public class CodeAbsenceIndexBundleFactory : BundleFactory
  {
    public CodeAbsenceIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/CodeAbsenceIndexBundle.js");
      Js("~/Scripts/Module/ProgressBar/module.js",
           "~/Scripts/Module/Notification/module.js",
           "~/Areas/CodeAbsence/Scripts/code-absence.controller.js",
           "~/Areas/CodeAbsence/Scripts/code-absence.service.js");

      CssBundleName("~/CodeAbsenceIndexBundle.css");
      Css("~/Content/module/ProgressBar/style.css",
           "~/Content/FormPanel/style.css",
           "~/Content/BarreRecherche/barre-recherche-light.css",
           "~/Content/Referentials/style.css",
           "~/Content/module/Notification/style.css",
           "~/Areas/CodeAbsence/Content/Style.css");
    }
  }
}

