using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
    /// <summary>
    /// Bundle pour PersonnelEdit
    /// </summary>
    public class PersonnelEditBundleFactory : BundleFactory
    {
        public PersonnelEditBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/PersonnelEditBundle.js");
            Js("~/Areas/Personnel/Scripts/services/personnel-edit-carto.service.js",
                 "~/Areas/Personnel/Scripts/services/personnel-edit-contact.service.js",
                 "~/Areas/Personnel/Scripts/services/personnel-edit-manage-lookup.service.js",
                 "~/Areas/Personnel/Scripts/services/personnel-edit-fields-cleaner.service.js",
                 "~/Areas/Personnel/Scripts/services/personnel-edit-persiste-state.service.js",
                 "~/Areas/Personnel/Scripts/services/delegation.service.js",
                 "~/Areas/Personnel/Scripts/services/contratInterim.service.js",
                 "~/Areas/Personnel/Scripts/services/zoneDeTravail.service.js",
                 "~/Areas/Personnel/Scripts/services/matriculeExterne.service.js",
                 "~/Areas/Pointage/RapportListe/Scripts/Services/rapport.service.js",
                 "~/Areas/Personnel/Scripts/personnel.service.js",
                 "~/Areas/Personnel/Scripts/personnel-edit.controller.js",
                 "~/Areas/Utilisateur/Scripts/utilisateur.service.js",
                 "~/Areas/Societe/Scripts/services/societe.service.js",
                 "~/Scripts/module/DateTimePicker/datetimepickerDirective.js",
                  "~/Areas/Personnel/Scripts/modals/formulaire-contratInterim-modal.component.js",
                 "~/Areas/Personnel/Scripts/modals/formulaire-delegation-modal.component.js",
                 "~/Areas/Personnel/Scripts/modals/suppression-delegation-modal.component.js",
                 "~/Areas/Personnel/Scripts/modals/suppression-contratInterim-modal.component.js",
                 "~/Areas/Personnel/Scripts/modals/validation-contratInterim-modal.component.js",
                 "~/Scripts/module/Google/google.service.js",
                 "~/Scripts/module/Google/adresse.model.js");

            CssBundleName("~/PersonnelEditBundle.css");
            Css("~/Content/jquery-ui-1.10.2.css",
                 "~/Areas/Personnel/Content/style.css",
                 "~/Areas/Personnel/Content/edit.css",
                 "~/Content/Autocomplete/style.css",
                 "~/Content/ReferentialPickList/ReferentialPickListCaller.css");
        }
    }
}

