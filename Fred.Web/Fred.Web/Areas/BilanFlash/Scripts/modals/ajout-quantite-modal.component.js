(function (angular) {
    'use strict';

    angular.module('Fred').component('ajoutQuantiteComponent', {
        templateUrl: '/Areas/BilanFlash/Scripts/modals/ajout-quantite-modal.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: 'AjoutQuantiteComponentController'
    });

    AjoutQuantiteComponentController.$inject = [];

    function AjoutQuantiteComponentController() {
        var $ctrl = this;

        $ctrl.$onInit = $onInit;
        $ctrl.handleValidate = handleValidate;

        function $onInit(){
            $ctrl.item = angular.copy($ctrl.resolve.item);
            $ctrl.resources = $ctrl.resolve.resources;
            $ctrl.tache = $ctrl.resolve.tache;
            $ctrl.dateDebut = $ctrl.resolve.dateDebut;
            $ctrl.dateFin = $ctrl.resolve.dateFin;
        }

        function handleValidate() {
            if(!$ctrl.Quantite || $ctrl.Quantite == ""){
                $ctrl.Quantite = 0;
            }

            $ctrl.close({ $value: $ctrl.Quantite });
        }

    }

    angular.module('Fred').controller('AjoutQuantiteComponentController', AjoutQuantiteComponentController);

}(angular));