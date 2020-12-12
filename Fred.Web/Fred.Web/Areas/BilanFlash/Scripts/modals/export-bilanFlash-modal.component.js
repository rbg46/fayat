(function (angular) {
    'use strict';

    angular.module('Fred').component('exportBilanFlashComponent', {
        templateUrl: '/Areas/BilanFlash/Scripts/modals/export-bilanFlash-modal.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: 'ExportBilanFlashComponentController'
    });

    ExportBilanFlashComponentController.$inject = [];

    function ExportBilanFlashComponentController() {
        var $ctrl = this;

        $ctrl.exportStep = 0;
        $ctrl.errorDate = false;
        $ctrl.resources = $ctrl.resolve.resources;
        $ctrl.dateDebut = angular.copy($ctrl.resolve.dateDebut);
        $ctrl.dateMin = new Date($ctrl.resolve.dateDebut);
        $ctrl.dateFin = angular.copy($ctrl.resolve.dateFin);
        $ctrl.dateMax = new Date($ctrl.resolve.dateDebut);
        $ctrl.dateMax.setMonth($ctrl.dateMin.getMonth() + 7);
        $ctrl.handleCancel = handleCancel;
        $ctrl.handleExport = handleExport;
        $ctrl.handleCanExport = handleCanExport;

        function handleExport() {
            $ctrl.close({
                $value: {
                    dateDebut: moment($ctrl.dateDebut).format('YYYY-MM-DD'),
                    dateFin: moment($ctrl.dateFin).format('YYYY-MM-DD'),
                    templateName: $ctrl.templateName,
                    isPdf: $ctrl.exportPdf
                }
            });
        }

        function handleCanExport(exportPdf) {
            $ctrl.exportPdf = exportPdf;
            if ($ctrl.exportPdf === null || $ctrl.exportPdf === undefined ||
                $ctrl.templateName === null || $ctrl.templateName === undefined || $ctrl.dateDebut > $ctrl.dateFin) {
                $ctrl.canExport = false;
            } else {
                $ctrl.canExport = true;
            }
        }

        function handleCancel() {
            $ctrl.dismiss({ $value: 'cancel' });
        }
    }

    angular.module('Fred').controller('ExportBilanFlashComponentController', ExportBilanFlashComponentController);

}(angular));
