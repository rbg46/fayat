using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
    /// <summary>
    /// Bundle pour PointagePersonnelExport
    /// </summary>
    public class ExportPointagePersonnelBundleFactory : BundleFactory
    {
        public ExportPointagePersonnelBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/ExportPointagePersonnelBundle.js");
            Js(
                 "~/Areas/Pointage/ExportPointagePersonnel/Scripts/Services/pointage-personnel.service.js",
                 "~/Areas/Pointage/ExportPointagePersonnel/Scripts/export.controller.js",
                 "~/Scripts/module/DateTimePicker/datetimepickerDirective.js");

            CssBundleName("~/ExportPointagePersonnelBundle.css");
            Css("~/Areas/Pointage/ExportPointagePersonnel/Content/style.css");
        }
    }
}

