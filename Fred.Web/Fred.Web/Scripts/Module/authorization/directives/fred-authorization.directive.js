/// <reference path="../fonctionnalite-type-mode.model.js" />

(function () {
    'use strict';
    angular.module('Fred').directive('fredAuthorization', fredAuthorizationDirective);

    fredAuthorizationDirective.$inject = ['$window'];

    function fredAuthorizationDirective($window) {
        return {
            controller: 'FredAuthorizationController',
            restrict: 'E',
            transclude: true,
            replace: true,
            templateUrl: '/Scripts/module/authorization/directives/fred-authorization.tpl.html',
            controllerAs: '$ctrl',
            bindToController: true,
            scope: {
                key: '@',

                // N'affiche le contrôle que si l'autorisation n'est pas définie
                displayedIfUndefinedOnly: '<',

                //Attribut qui sert a supprimer l'overflow auto de la div "bloc-auth"
                isOverflowHidden: '<',

                //Ajuster les attributs de la div "bloc-auth"
                isToAdjustCss: '<',

                //Ajuster les attributs de la div "bloc-auth" en enlevant le margin
                isToAdjustCssNoMargin: '<'
            },
            link: function (scope) {
                scope.$on('refresh-fred-authorisation', function () {
                    scope.$ctrl.activate();
                });
            }
        };
    }

    angular.module('Fred').controller('FredAuthorizationController', FredAuthorizationController);

    FredAuthorizationController.$inject = ['authorizationService'];

    function FredAuthorizationController(authorizationService) {
        var $ctrl = this;

        //if (!displayIfUndefinedOnly) { displayIfUndefinedOnly = false; }
        $ctrl.isVisible = false;
        $ctrl.isReadOnly = true;
        $ctrl.isSuperAdmin = false;
        $ctrl.adminToolTip = "";
        $ctrl.activate = function activate() {
            var rights = authorizationService.getRights($ctrl.key);
            $ctrl.isVisible = !$ctrl.displayedIfUndefinedOnly && rights.isVisible;
            $ctrl.isReadOnly = rights.isReadOnly;
            $ctrl.isSuperAdmin = rights.isSuperAdmin;
        };

        $ctrl.activate();
    }
})();