(function (angular) {
    'use strict';

    angular.module('Fred').component('validateAvancementComponent', {
        templateUrl: '/Areas/Budget/Scripts/Avancement/components/validation-avancement-modal.component.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: 'ValidateAvancementComponentController'
    });

    ValidateAvancementComponentController.$inject = [];

    function ValidateAvancementComponentController() {
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


    angular.module('Fred').controller('ValidateAvancementComponentController', ValidateAvancementComponentController);

}(angular));