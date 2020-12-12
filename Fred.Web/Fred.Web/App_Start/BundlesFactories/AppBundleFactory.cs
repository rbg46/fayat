using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
    public class AppBundleFactory : BundleFactory
    {
        /*
           * Instanciation de l'application Fred dans Angular avec l'ordre suivant :
           * 
           * 1/ Chargement du fichier de configuration
           * 2/ LocalStorageModule > Recherche dans le Storage interne pour persistance du token
           * 3/ App.js qui appellent les modules instanciés au préalable 
           * 3/ authInterceptorService > Intercepteur des Appels Api
           * 4/ Chargement des outils communs
           * 5/ Chargement fichier opérant les action d'authentification
           */
        public AppBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/app/js");
            Js("~/Scripts/App/modules-config.js",
                "~/Scripts/App/config.js",
                "~/Scripts/App/app.js",
                "~/Scripts/Module/Notification/module.js",
                "~/Scripts/Module/ProgressBar/module.js",

                 // Factories
                 "~/Scripts/Factory/focus/focus.factory.js",

                "~/Scripts/Controllers/main/main.controller.js",
                "~/Scripts/Controllers/main/services/user.service.js",
                "~/Scripts/Controllers/main/services/server-notifications.service.js",
                "~/Scripts/Controllers/Header/fred-application-header.component.js",
                "~/Scripts/Controllers/Header/modal/suppression-notification-modal.component.js",
                "~/Scripts/Controllers/Header/fred-application-header-mobile.component.js",
                "~/Scripts/Controllers/userPanel/fred-user-panel.component.js",
                "~/Scripts/Controllers/menu/fred-menu.component.js",
                "~/Scripts/Controllers/menu/fred-menu-home.component.js",
                "~/Scripts/Controllers/menu/fred-back-button.component.js",
                "~/Scripts/Factory/subscribe/fred-subscribe.service.js",
                "~/Scripts/Controllers/Horloge/fred-horloge.component.js",
                "~/Scripts/module/Fayat/Toolbox.js",
                "~/Scripts/Controllers/Notification/fred-notification.component.js",
                // confirmDialog et fredDialog
                "~/Scripts/Factory/dialog/confirmDialog.Service.js",
                "~/Scripts/Factory/dialog/confirmDialog.component.js",
                "~/Scripts/Factory/dialog/fredDialog.Service.js",
                "~/Scripts/Factory/dialog/fredDialog.component.js",
                "~/Scripts/interceptors/unauthorizedInterceptor.js",
                //gestion des erreurs du models suite a une validation du serveur.
                "~/Scripts/Factory/errors/model-state-error-manager.service.js",
                "~/Scripts/directive/lookup/fred-lookup.directive.js",
                "~/Scripts/directive/lookup/fred-lookup-standalone.component.js",

                // Components internes
                "~/Scripts/Components/Notify/Notify.js",
                "~/Scripts/Components/ProgressBar/ProgressBar.js",
                "~/Scripts/Components/InputSelect/input-select.component.js",
                "~/Scripts/Components/LookupPanel/lookup-panel.component.js",
                "~/Scripts/Components/LookupPanel/lookup-panel-filter.component.js",

                 // Overlay panel
                 "~/Scripts/Components/Overlay/overlay.component.js",

                //directive qui permet de simuler le comportement des input type time sur firefox 52
                "~/Scripts/directive/fred-input-time.directive.js",
                "~/Scripts/module/authorization/authorization.service.js",
                "~/Scripts/module/authorization/menu-authorization.service.js",
                "~/Scripts/module/authorization/directives/fred-authorization.directive.js",
                "~/Scripts/module/authorization/directives/fred-super-admin-authorization.directive.js",
                "~/Scripts/module/authorization/permissionKeys.js",
                "~/Scripts/module/authorization/fonctionnalite-type-mode.model.js",
                // Provider et Directive featureFlags
                "~/Scripts/module/feature-flipping/feature-flipping.module.js",

                // Directives
                "~/Scripts/directive/UI/fred-ui-controls.js",
                "~/Scripts/directive/fred-date-picker/fred-date-picker.directive.js",
                 "~/Scripts/directive/select-on-click.directive.js",
                 "~/Scripts/directive/fred-on-enter.directive.js",
                 "~/Scripts/directive/fred-input-number.directive.js",
                 "~/Scripts/directive/fred-resize.directive.js",
                 "~/Scripts/directive/fred-wrap-in-tag.directive.js",
                 "~/Scripts/directive/fred-display-handler.directive.js",
                 "~/Scripts/directive/focusOn/focus-on.directive.js",
                 "~/Scripts/directive/on-scroll-end.directive.js",
                 "~/Scripts/directive/organisation-related-feature.directive.js",

                 // Énumérations
                 "~/Scripts/enums/enums.js",

                 // Services
                 "~/Scripts/services/pays.service.js",
                 "~/Scripts/services/ParamsHandlerService.js",
                 "~/Areas/Utilisateur/Scripts/utilisateur.service.js",
                 "~/Scripts/services/piece-jointe.service.js",
                 "~/Scripts/services/piece-jointe-validator.service.js",
                 "~/Areas/Societe/Scripts/services/type-societe.service.js",
                 "~/Scripts/services/organisation-related-feature.service.js",


                 // Référentiel des données
                 "~/Scripts/services/referential/fournisseur.service.js",

                 // Filters
                 "~/Scripts/filter/initial-letter.filter.js",
                 "~/Scripts/filter/to-locale-date.filter.js",
                 "~/Scripts/filter/number.filter.js",
                 "~/Scripts/filter/truncate-text.filter.js",
                 "~/Scripts/filter/format-text.filter.js",
                 "~/Scripts/Services/string-format.service.js",

                 // CropperJS (for images)
                 "~/Scripts/module/Cropperjs/cropper.min.js",

                 // Helpers
                 "~/Scripts/helpers/throttle.js",

                //Favoris
                "~/Scripts/module/Favoris/Service.js",
                "~/Scripts/Factory/favori/favori.component.js",
                "~/Scripts/Factory/favori/deleteFavori.component.js",
                "~/Scripts/Factory/favori/favori.service.js"
                );

            CssBundleName("~/app/css");
            Css("~/Content/custom.bootstrap.css",
                  "~/Content/Site.css",
                  //"~/Content/Fonts.css",
                  // ReferentialPickList
                  "~/Content/ReferentialPickList/ReferentialPickList.css",
                  // Menu
                  "~/Content/menu.css",
                  "~/Scripts/directive/lookup/lookup.css",
                  "~/Content/BlocUtilisateur.css",
                  "~/Content/closing.css",
                  "~/Content/FredMenuHome.css",

                  // Gestion des tableaux Fixed
                  "~/Content/tableFixed/tableFixed.css",

                  // Composants internes
                  "~/Content/Components/input-select.css",
                  "~/Content/Components/lookup-panel.css",
                  "~/Content/Components/overlay.css",

                  // Composants externes
                  "~/Content/module/Notification/style.css",
                  "~/Content/module/ProgressBar/style.css",

                 // CropperJS (for images)
                 "~/Scripts/module/Cropperjs/cropper.min.css"
                  );

#if DEBUG
            AddJs("~/Scripts/directive/debug/debug-enabled.js");
            AddJs("~/Scripts/directive/debug/debug-angular-watchers-counter.directive.js");
            AddCss("~/Scripts/directive/debug/debug-angular-watchers-counter.directive.css");
#else
            AddJs("~/Scripts/directive/debug/debug-disabled.js");
#endif

        }
    }
}
