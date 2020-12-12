using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour DatesClotureComptableIndex
  /// </summary>
  public class DatesClotureComptableIndexBundleFactory : BundleFactory
  {
    public DatesClotureComptableIndexBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/DatesClotureComptableIndexBundle.js");
      Js("~/Scripts/Angularjs/1.5.8/i18n/angular-locale_fr-fr.js",
           "~/Scripts/services/permissions.service.js",
           "~/Areas/DatesClotureComptable/Scripts/dates-cloture-comptable.controller.js",
           "~/Areas/DatesClotureComptable/Scripts/dates-cloture-comptable.service.js",
           "~/Areas/DatesClotureComptable/Scripts/dates-cloture-comptable-helper.service.js",
           "~/Areas/DatesClotureComptable/Scripts/dates-cloture-comptable-data.service.js",
           "~/Areas/DatesClotureComptable/Scripts/dates-cloture-comptable-status.service.js",
           "~/Areas/DatesClotureComptable/Scripts/dates-cloture-compable-row.component.js");

      CssBundleName("~/DatesClotureComptableIndexBundle.css");
      Css("~/Areas/DatesClotureComptable/Content/Style.css",
           "~/Content/BarreRecherche/barre-recherche-light.css",
           "~/Content/Referentials/style.css",
           "~/Content/ReferentialPickList/ReferentialPickListCaller.css");
    }
  }
}

