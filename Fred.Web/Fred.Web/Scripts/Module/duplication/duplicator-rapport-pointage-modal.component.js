(function (angular) {
    'use strict';

    angular.module('Fred').component('duplicatorRapportPointageModalComponent', {
        templateUrl: '/Scripts/module/duplication/duplicator-rapport-pointage-modal.tpl.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: 'DuplicatorRapportPointageModalController'
    });

    angular.module('Fred').controller('DuplicatorRapportPointageModalController', DuplicatorRapportPointageModalController);

    DuplicatorRapportPointageModalController.$inject = ['$timeout'];

    function DuplicatorRapportPointageModalController($timeout) {

        var $ctrl = this;
        var duplicateRapport = null;
        var duplicatePointage = null;
        var dateDuplication = null;
        /*
         * Initilisation du controller de la modal
         */
        $ctrl.$onInit = function () {
            $ctrl.resources = $ctrl.resolve.resources;
            $ctrl.startDate = new Date($ctrl.resolve.data.startDate);
            $ctrl.endDate = new Date($ctrl.resolve.data.endDate);
            $ctrl.pointageDuplication = $ctrl.resolve.data.pointageDuplication;
            $ctrl.rapportDuplication = $ctrl.resolve.data.rapportDuplication;
            $ctrl.ciDuplication = $ctrl.resolve.data.ciDuplication;
            $ctrl.title = $ctrl.resolve.data.title;
            duplicateRapport = $ctrl.resolve.data.rapportDuplication !== undefined;
            duplicatePointage = $ctrl.resolve.data.pointageDuplication !== undefined;
            dateDuplication = duplicatePointage ? new Date($ctrl.pointageDuplication.DatePointage) : new Date($ctrl.rapportDuplication.DateChantier);

        };

        $ctrl.$onDestroy = function () {

        };

        $ctrl.turnOff = function () {
            $ctrl.dismiss({
                $value: {
                    duplicatedDialogClosed: true
                }
            });
        };

        $ctrl.canDuplicate = function () {
            return $ctrl.startDate <= $ctrl.endDate && $ctrl.ciDuplication !== null;
        };

        $ctrl.duplicate = function () {
            $ctrl.close({
                $value: {
                    startDate: moment($ctrl.startDate.setUTCHours(0, 0, 0, 0)),
                    endDate: moment($ctrl.endDate.setUTCHours(0, 0, 0, 0)),
                    ciId: $ctrl.ciDuplication.CiId,
                    RapportLigneId: duplicatePointage ? $ctrl.pointageDuplication.PointageId : null,
                    RapportId: duplicateRapport ? $ctrl.rapportDuplication.RapportId : null,
                    DatePointage: dateDuplication
                }
            });
        };

        /**
         * Open the calendar for start date criteria
         */
        $ctrl.openCalendarStartDate = function () {
            $timeout(function () {
                $ctrl.popupOpenedStartDate = true;
            });
        };

        /**
         * Open the calendar for end date criteria
         */
        $ctrl.openCalendarEndDate = function () {
            $timeout(function () {
                $ctrl.popupOpenedEndDate = true;
            });
        };

        $ctrl.lookupCiIsDisabled = function () {
            return !$ctrl.resolve.data.canChangeCi;

        };


    }
}(angular));
