using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
    /// <summary>
    /// Bundle pour PointagePersonnelIndex
    /// </summary>
    public class ListePointagePersonnelBundleFactory : BundleFactory
    {
        public ListePointagePersonnelBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/ListePointagePersonnelBundle.js");
            Js(
                "~/Areas/Pointage/ListePointagePersonnel/Scripts/pointage-personnel.controller.js",


                "~/Areas/Pointage/ListePointagePersonnel/Scripts/Components/sidebar-detail.component.js",
                "~/Areas/Pointage/ListePointagePersonnel/Scripts/Components/pointage-list.component.js",
                "~/Areas/Pointage/ListePointagePersonnel/Scripts/Components/date-picker.component.js",
                "~/Areas/Pointage/ListePointagePersonnel/Scripts/Components/personnel-picker.component.js",


                "~/Areas/Pointage/ListePointagePersonnel/Scripts/Services/personnel-picker.service.js",
                "~/Areas/Pointage/ListePointagePersonnel/Scripts/Services/date-picker.service.js",
                "~/Areas/Pointage/ListePointagePersonnel/Scripts/Services/pointage-helper.service.js",
                "~/Areas/Pointage/ListePointagePersonnel/Scripts/Services/pointage-personnel.service.js",
                "~/Areas/Pointage/ListePointagePersonnel/Scripts/Services/pointage-list-change-detector.service.js",
                "~/Areas/Pointage/ListePointagePersonnel/Scripts/Services/pointage-duplicator-proccess.service.js",
                "~/Areas/Pointage/ListePointagePersonnel/Scripts/Services/pointage-duplicator-time.service.js",

                "~/Areas/DatesClotureComptable/Scripts/dates-cloture-comptable.service.js",
                
                "~/Areas/Pointage/ListePointagePersonnel/Scripts/Directives/stepper.directive.js",
                
                
                "~/Scripts/Angularjs/1.5.8/i18n/angular-locale_fr-fr.js",
                
                
                "~/Scripts/module/duplication/duplicator-rapport-pointage-modal.component.js",
                "~/Scripts/module/duplication/duplicator-time.service.js",
                "~/Scripts/module/duplication/interimaire-duplication-state.js",

                "~/Scripts/module/DateTimePicker/datetimepickerDirective.js"
            );

            CssBundleName("~/ListePointagePersonnelBundle.css");
            Css("~/Areas/Pointage/ListePointagePersonnel/Content/style.css",
                 "~/Content/BarreRecherche/barre-recherche-light.css");
        }
    }
}
