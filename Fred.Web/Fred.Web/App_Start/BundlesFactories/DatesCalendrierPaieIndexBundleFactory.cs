using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour DatesCalendrierPaieIndex
  /// </summary>
  public class DatesCalendrierPaieIndexBundleFactory : BundleFactory
  {
    public DatesCalendrierPaieIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/DatesCalendrierPaieIndexBundle.js");
      Js("~/Areas/DatesCalendrierPaie/Scripts/dates-calendrier-paie.service.js",
           "~/Areas/DatesCalendrierPaie/Scripts/dates-calendrier-paie.controller.js");

      CssBundleName("~/DatesCalendrierPaieIndexBundle.css");
      Css("~/Content/module/ProgressBar/style.css",
           "~/Content/module/Notification/style.css",
           "~/Areas/DatesCalendrierPaie/Content/style.css",
           "~/Content/ReferentialPickList/ReferentialPickListCaller.css");
    }
  }
}

