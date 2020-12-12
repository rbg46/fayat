using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour FactureIndex
  /// </summary>
  public class FactureIndexBundleFactory : BundleFactory
  {
    public FactureIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/FactureIndexBundle.js");
      Js("~/Scripts/directive/searchFilter.js",
           "~/Scripts/filter/number.filter.js",
           "~/Areas/Facture/Scripts/facture.controller.js",
           "~/Areas/Facture/Scripts/facture.service.js",
           "~/Scripts/module/DateTimePicker/datetimepickerDirective.js",
           "~/Areas/Pointage/RapportListe/Scripts/Services/rapport.service.js"
        );

      CssBundleName("~/FactureIndexBundle.css");
      Css("~/Content/filtersBtn.css",
           "~/Areas/Facture/Content/style.css");
    }
  }
}

