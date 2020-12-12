using System.Web.Optimization;

namespace Fred.Web.App_Start.BundlesFactories
{
  public class ObjectifFlashListBundleFactory : BundleFactory
  {
    public ObjectifFlashListBundleFactory(BundleCollection bundles)
      : base(bundles)
    {
      JsBundleName("~/ObjectifFlashListBundleFactory.js");
      Js(
        //DateTimePicker
        "~/Scripts/module/DateTimePicker/datetimepickerDirective.js",

        //ObjectifFlash
        "~/Areas/BilanFlash/Scripts/services/objectif-flash.service.js",
        "~/Areas/BilanFlash/Scripts/objectif-flash-list.controller.js",

        //Suppression Objectif Flash Modal
        "~/Areas/BilanFlash/Scripts/modals/suppression-objectifFlash-modal.component.js",

        //Export Bilan Flash Modal
        "~/Areas/BilanFlash/Scripts/modals/export-bilanFlash-modal.component.js", 
        
        //Duplication Objectif Flash Modal
        "~/Areas/BilanFlash/Scripts/modals/report-et-duplication-objectifFlash-modal.component.js");

      CssBundleName("~/ObjectifFlashListBundleFactory.css");
      Css("~/Areas/BilanFlash/Content/objectifFlash-List.css",
          "~/Areas/BilanFlash/Content/common.css");
    }
  }
}

