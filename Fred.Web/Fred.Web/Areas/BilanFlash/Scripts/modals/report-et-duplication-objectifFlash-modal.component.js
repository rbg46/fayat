(function (angular) {
    'use strict';

    angular.module('Fred').component('reportEtDuplicationObjectifFlashComponent', {
        templateUrl: '/Areas/BilanFlash/Scripts/modals/report-et-duplication-objectifFlash-modal.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: 'ReportEtDuplicationObjectifFlashController'
    });
    ReportEtDuplicationObjectifFlashController.$inject = [];

    function ReportEtDuplicationObjectifFlashController() {
        var $ctrl = this;

        $ctrl.dateMin = new Date();
        $ctrl.duplicate = $ctrl.resolve.duplicate;
        $ctrl.resources = $ctrl.resolve.resources;
        $ctrl.dateDebut = angular.copy($ctrl.resolve.dateDebut);
        $ctrl.dateFin = angular.copy($ctrl.resolve.dateFin);
        $ctrl.dateReport = null;
        $ctrl.handleCancel = handleCancel;
        $ctrl.handleValidate = handleValidate;

        function handleCancel() {
            $ctrl.dismiss({ $value: 'cancel' });
        }

        function handleValidate() {
            $ctrl.close({ $value: moment($ctrl.dateReport).format('YYYY-MM-DD') });
        }
    }

    angular.module('Fred').controller('ReportEtDuplicationObjectifFlashController', ReportEtDuplicationObjectifFlashController);


}(angular));