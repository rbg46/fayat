using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
    /// <summary>
    /// Bundle pour AreasPointageViewsRapportDetail
    /// </summary>
    public class RapportJournalierBundleFactory : BundleFactory
    {
        public RapportJournalierBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/RapportJournalierBundle.js");
            Js("~/Scripts/module/LocalStorage/ngStorage.js",
                 "~/Scripts/Module/Notification/module.js",
                 "~/Scripts/Module/ProgressBar/module.js",
                 "~/Scripts/module/Fayat/Toolbox.js",
                 "~/Scripts/helpers/throttle.js",
                 "~/Scripts/directive/fred-fullscreen.directive.js",


                 "~/Areas/Pointage/RapportJournalier/Scripts/Components/custom-rapport-detail-table.directive.js",
                 "~/Areas/Pointage/RapportJournalier/Scripts/Services/rapport.service.js",
                 "~/Areas/Pointage/RapportJournalier/Scripts/Services/rapport-change-detector.service.js",
                 "~/Areas/Pointage/RapportJournalier/Scripts/Services/rapport-duplicator-proccess.service.js",
                 "~/Areas/Pointage/RapportJournalier/Scripts/rapport-detail.controller.js",
                 "~/Areas/Pointage/RapportJournalier/Scripts/Modals/avancement-bilanFlash-modal.component.js",
                 "~/Areas/Pointage/RapportJournalier/Scripts/Modals/confirmation-modification-ci-modal.component.js",
                 "~/Areas/Pointage/RapportJournalier/Scripts/Services/bilan-flash.service.js",


                 "~/Scripts/directive/changeHeightOnResize/change-height-on-resize.js",
                 "~/Scripts/module/DateTimePicker/datetimepickerDirective.js",

                 
                 "~/Scripts/Angularjs/1.5.8/i18n/angular-locale_fr-fr.js",
                 "~/Scripts/module/duplication/duplicator-rapport-pointage-modal.component.js",
                 "~/Scripts/module/duplication/duplicator-time.service.js",
                 "~/Scripts/services/permissions.service.js");

            CssBundleName("~/RapportJournalierBundle.css");
            Css("~/Content/module/ProgressBar/style.css",
                 "~/Content/FormPanel/Style.css",
                 "~/Content/module/Notification/style.css",
                 "~/Areas/Pointage/RapportJournalier/Content/RapportDetail.css",
                 "~/Content/ReferentialPickList/ReferentialPickListCaller.css");
        }
    }
}
