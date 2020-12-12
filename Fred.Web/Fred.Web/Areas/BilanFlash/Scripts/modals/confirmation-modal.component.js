(function (angular) {
    'use strict';

    angular.module('Fred').component('confirmationComponent', {
        templateUrl: '/Areas/BilanFlash/Scripts/modals/confirmation-modal.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: 'confirmationComponentController'
    });

    confirmationComponentController.$inject = [];

    function confirmationComponentController() {
        var $ctrl = this;

        $ctrl.$onInit = $onInit;
        $ctrl.handleValidate = handleValidate;
        $ctrl.handleCancel = handleCancel;

        function $onInit(){
            $ctrl.resources = $ctrl.resolve.resources;
            $ctrl.type = $ctrl.resolve.type;
        }

        function handleValidate() {
            $ctrl.close({ $value: $ctrl.type });
        }

        
        function handleCancel() {
            $ctrl.dismiss({ $value: 'cancel' });
        }

    }

    angular.module('Fred').controller('confirmationComponentController', confirmationComponentController);

}(angular));