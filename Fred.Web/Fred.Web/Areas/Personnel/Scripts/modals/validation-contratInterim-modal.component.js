(function (angular) {
    'use strict';

    angular.module('Fred').component('validateContratInterimComponent', {
        templateUrl: '/Areas/Personnel/Scripts/modals/validation-contratInterim-modal.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: 'ValidateContratInterimComponentController'
    });

    angular.module('Fred').controller('ValidateContratInterimComponentController', ValidateContratInterimComponentController);

    function ValidateContratInterimComponentController() {
        var $ctrl = this;

        angular.extend($ctrl, {
            // Fonctions
            handleClose: handleClose
        });

        /*
         * Initilisation du controller de la modal
         */
        $ctrl.$onInit = function () {
            $ctrl.ContratInterimModal = angular.copy($ctrl.resolve.ContratInterimModal);
            $ctrl.resources = $ctrl.resolve.resources;
            $ctrl.displayDateDebut = convertDate($ctrl.ContratInterimModal.DateDebut);
            $ctrl.displayDateFin = convertDate($ctrl.ContratInterimModal.DateFin);
        };

        /*
         * @function handleSave()
         * @description Enregistrement de la nouvelle delegation : Renvoie les valeurs au controller principal
         */
        function handleClose(IsValidate) {
            var result = {
                validate: IsValidate,
                contratInterim: $ctrl.ContratInterimModal
            };
            $ctrl.close({ $value: result });
        }

        function convertDate(inputFormat) {
            function pad(s) { return (s < 10) ? '0' + s : s; }
            var d = new Date(inputFormat);
            return [pad(d.getDate()), pad(d.getMonth() + 1), d.getFullYear()].join('/');
        }
    }
}(angular));