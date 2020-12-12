(function (angular) {
    'use strict';

        angular.module('Fred').component('duplicateParametrageComponent', {
        templateUrl: '/Areas/FamilleOperationDiverse/Scripts/modals/duplicate-parametrage-modal.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: 'DuplicateParametrageController'
    });

    DuplicateParametrageController.$inject = [];

    function DuplicateParametrageController() {
        var $ctrl = this;

        $ctrl.$onInit = $onInit;
        $ctrl.close = close;
        $ctrl.handleClose = handleClose;

        function $onInit() {
            $ctrl.editFamille = $ctrl.resolve.editFamille;
            $ctrl.parametrageList = $ctrl.resolve.parametrageList;
            $ctrl.journalCodeList = $ctrl.resolve.journalCodeList;
            $ctrl.natureCodeList = $ctrl.resolve.natureCodeList;
        }

        /* 
         * @function handleClose()
         * @description Fermeture modal
         */
        function handleClose() {
            $ctrl.dismiss({ $value: 'cancel' });
        }

        function close() {
            $ctrl.dismiss({ $value: 'cancel' });
        }
    }

    angular.module('Fred').controller('DuplicateParametrageController', DuplicateParametrageController);

}(angular));