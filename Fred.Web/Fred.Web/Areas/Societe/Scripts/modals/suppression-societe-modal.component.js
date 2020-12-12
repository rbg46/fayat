(function (angular) {
    'use strict';

    angular.module('Fred').component('deleteSocieteComponent', {
        templateUrl: '/Areas/Societe/Scripts/modals/suppression-societe-modal.component.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: 'DeleteSocieteComponentController'
    });

    DeleteSocieteComponentController.$inject = [];

    function DeleteSocieteComponentController() {
        var $ctrl = this;

        $ctrl.$onInit = $onInit;
        $ctrl.handleValidate = handleValidate;
        $ctrl.handleCancel = handleCancel;

        function $onInit(){
            $ctrl.resources = $ctrl.resolve.resources;
        }

        function handleValidate() {
            $ctrl.close();
        }

        function handleCancel() {
            $ctrl.dismiss({ $value: 'cancel' });
        }
    }


    angular.module('Fred').controller('DeleteSocieteComponentController', DeleteSocieteComponentController);

}(angular));