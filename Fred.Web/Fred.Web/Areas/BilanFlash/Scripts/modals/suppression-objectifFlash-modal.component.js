(function (angular) {
    'use strict';

    angular.module('Fred').component('deleteObjectifFlashComponent', {
        templateUrl: '/Areas/BilanFlash/Scripts/modals/suppression-objectifFlash-modal.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: 'DeleteObjectifFlashComponentController'
    });

    DeleteObjectifFlashComponentController.$inject = [];

    function DeleteObjectifFlashComponentController() {
        var $ctrl = this;

        $ctrl.$onInit = $onInit;
        $ctrl.handleSave = handleSave;
        $ctrl.handleCancel = handleCancel;

        function $onInit(){
            $ctrl.objectifFlashModal = angular.copy($ctrl.resolve.objectifFlash);
            $ctrl.resources = $ctrl.resolve.resources;
            $ctrl.modalText = String.format($ctrl.resources.BilanFlash_Modal_Suppresion_Confirmation, $ctrl.objectifFlashModal.Libelle, $ctrl.objectifFlashModal.ObjectifFlashId)
        }

        function handleSave() {
            $ctrl.close({ $value: $ctrl.objectifFlashModal });
        }

        function handleCancel() {
            $ctrl.dismiss({ $value: 'cancel' });
        }
    }


    angular.module('Fred').controller('DeleteObjectifFlashComponentController', DeleteObjectifFlashComponentController);

}(angular));