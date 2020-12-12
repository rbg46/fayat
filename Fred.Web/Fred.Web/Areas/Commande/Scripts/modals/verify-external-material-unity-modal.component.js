(function (angular) {
    'use strict';

    angular.module('Fred').component('verifyExternalMaterialUnityModalComponent', {
        templateUrl: '/Areas/Commande/Scripts/modals/verify-external-material-unity-modal.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: 'VerifyExternalMaterialUnityModal'
    });

    angular.module('Fred').controller('VerifyExternalMaterialUnityModal', VerifyExternalMaterialUnityModal);

    VerifyExternalMaterialUnityModal.$inject = [];

    function VerifyExternalMaterialUnityModal() {
        var $ctrl = this;

        /*
         * Initilisation du controller de la modal
         */
        $ctrl.$onInit = function () {
            $ctrl.resources = $ctrl.resolve.resources;
            $ctrl.commande = $ctrl.resolve.commande;
            $ctrl.handleCancel = handleCancel; 
            $ctrl.handleConfirm = handleConfirm;          
        };

        /* 
         * @function handleCancel ()
         * @description Quitte la modal
         */
        function handleCancel() {
            $ctrl.dismiss({ $value: 'cancel' });
        }

        function handleConfirm(){
            $ctrl.close({ $value: $ctrl.commande });
        }

    }
}(angular));