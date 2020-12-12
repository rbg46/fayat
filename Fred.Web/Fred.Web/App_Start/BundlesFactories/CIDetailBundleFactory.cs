using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
  /// <summary>
  /// Bundle pour CIDetail
  /// </summary>
  public class CIDetailBundleFactory : BundleFactory
  {
    public CIDetailBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/CIDetailBundle.js");
      Js("~/Scripts/module/Google/google.service.js",
           "~/Scripts/module/Google/adresse.model.js",
           "~/Areas/CI/Scripts/ci-detail.controller.js",
           "~/Areas/CI/Scripts/services/ci.service.js",
           "~/Areas/CI/Scripts/services/affectation.service.js",
           "~/Scripts/module/DateTimePicker/datetimepickerDirective.js",
           "~/Areas/ReferentielEtendu/Scripts/referentiel-etendu-filter.service.js",
           "~/Areas/DatesClotureComptable/Scripts/dates-cloture-comptable.service.js",
           "~/Scripts/filter/check-empty.filter.js");

      CssBundleName("~/CIDetailBundle.css");
      Css("~/Content/jquery-ui-1.10.2.css",
           "~/Content/FormPanel/style.css",
           "~/Areas/CI/Content/style.css",
           "~/Content/ReferentialPickList/ReferentialPickListCaller.css",
           "~/Content/Autocomplete/style.css");
    }
  }
}

