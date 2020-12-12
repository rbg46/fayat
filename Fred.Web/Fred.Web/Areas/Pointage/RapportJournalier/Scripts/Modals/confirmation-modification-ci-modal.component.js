(function (angular) {
    'use strict';

    angular.module('Fred').component('confirmationModificationCiComponent', {
        templateUrl: '/Areas/Pointage/RapportJournalier/Scripts/Modals/confirmation-modification-ci-modal.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: 'ConfirmationModificationCiComponentController'
    });

    ConfirmationModificationCiComponentController.$inject = [];

    function ConfirmationModificationCiComponentController() {
        var $ctrl = this;

        $ctrl.$onInit = $onInit;
        $ctrl.handleValidate = handleValidate;
        $ctrl.handleCancel = handleCancel;

        function $onInit() {
            $ctrl.resources = $ctrl.resolve.resources;
        }

        function handleValidate() {
            $ctrl.close();
        }

        function handleCancel() {
            $ctrl.dismiss({ $value: 'cancel' });
        }
    }

    angular.module('Fred').controller('ConfirmationModificationCiComponentController', ConfirmationModificationCiComponentController);

}(angular));