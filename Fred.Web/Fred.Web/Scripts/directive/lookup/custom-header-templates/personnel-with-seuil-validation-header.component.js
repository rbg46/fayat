(function (angular) {
    'use strict';

    angular.module('Fred').component('personnelWithSeuilValidationHeaderComponent', {
        templateUrl: '/Scripts/directive/lookup/custom-header-templates/personnel-with-seuil-validation-header.template.html',
        bindings: {
            context: '=',
            reloadAction: '&'
        },
        controller: PersonnelWithSeuilValidationHeaderComponentController
    });

    PersonnelWithSeuilValidationHeaderComponentController.$inject = ['$timeout'];

    function PersonnelWithSeuilValidationHeaderComponentController($timeout) {

        var $ctrl = this;

        $ctrl.includePersonnelWithoutSeuil = true;

        $ctrl.$onInit = function () {
            $ctrl.context.authorizedOnlyUrl = buildExtraQueryUrl();
        };

        $ctrl.onChange = function () {

            var queryExtraUrl = buildExtraQueryUrl();
            $ctrl.context.authorizedOnlyUrl = queryExtraUrl;
            $timeout(() => {
                $ctrl.reloadAction();
            });
        };

        function buildExtraQueryUrl() {
            return '&authorizedOnly=' + $ctrl.includePersonnelWithoutSeuil;
        }

    }

}(angular));
