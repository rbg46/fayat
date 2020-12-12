(function () {
    'use strict';

    angular
        .module('Fred')
        .component('datesClotureComptableRowComponent', {
            templateUrl: '/Areas/DatesClotureComptable/Scripts/dates-cloture-compable-row.component.html',
            bindings: {
                resources: '<',
                monthModel: '<',
                monthNumber: '<',
                readOnly: '<'
            },
            controller: 'DatesClotureComptableRowComponentController'
        });

    angular.module('Fred').controller('DatesClotureComptableRowComponentController', DatesClotureComptableRowComponentController);

    DatesClotureComptableRowComponentController.$inject = ['$scope',
        'ProgressBar',
        'confirmDialog',
        'Notify',
        '$timeout',
        'DatesClotureComptableHelperService',
        'DatesClotureComptableStatusService',
        'DatesClotureComptableDataService'];

    function DatesClotureComptableRowComponentController($scope,
        ProgressBar,
        confirmDialog,
        Notify,
        $timeout,
        DatesClotureComptableHelperService,
        DatesClotureComptableStatusService,
        DatesClotureComptableDataService) {

        var $ctrl = this;
        $ctrl.isOnClosing = false;

        $ctrl.dateOptions = {
            minDate: new Date(),
            showWeeks: false,
            startingDay: 1
        };
        $ctrl.popupOpened = false;
        $ctrl.oldDateCloture = null;

        $ctrl.isReadOnly = false;

        $ctrl.open = function () {
            if (!$ctrl.periodeClosed()) {
                $timeout(function myfunction() {
                    $ctrl.popupOpened = true;
                });
            }
        };

        /**
         * Initialization of the controller
         */
        $ctrl.$onInit = function () {
            $ctrl.monthDate = new Date($ctrl.monthModel.Annee, $ctrl.monthModel.Mois - 1, 15);
            if ($ctrl.monthModel !== null) {
                $ctrl.periodeClosed();
                $ctrl.oldDateCloture = angular.copy($ctrl.monthModel.DateCloture);
            }
        };

        /**
         * Close date is set
         */
        $ctrl.dateClotureChange = function () {
            var now = DatesClotureComptableHelperService.getNow();

            var isInPast = DatesClotureComptableHelperService.dateIsInPast($ctrl.monthModel.DateCloture, now);
            if (isInPast) {
                Notify.error(resources.DateCloture_is_in_past_message);
                $ctrl.monthModel.DateCloture = null;
                return;
            }

            var dateIsToday = DatesClotureComptableHelperService.dateIsToday($ctrl.monthModel.DateCloture, now);
            if (dateIsToday) {
                confirmCloture();
                return;
            }

            if (!dateIsToday && !isInPast) {
                $scope.$emit('datesClotureComptableRowComponent.dateChange', $ctrl.monthModel);
                $ctrl.oldDateCloture = angular.copy($ctrl.monthModel.DateCloture);
            }
        };

        $ctrl.cancelCloture = function () {
            confirmDialog
                .confirm($ctrl.resources, $ctrl.resources.Annulation_Cloture_lb, "fa fa-unlock")
                .then(onCancelClotureSuccess);
        };

        /**
         * Confirm the close date
         */
        $ctrl.confirmCloture = function () {
            confirmDialog
                .confirm($ctrl.resources, $ctrl.resources.Confirmation_Cloture_lb, "fa fa-lock")
                .then(onConfirmClotureSuccess)
                .catch(function () {
                    $ctrl.monthModel.DateCloture = $ctrl.oldDateCloture;
                });
        };

        function onCancelClotureSuccess() {
            var now = DatesClotureComptableHelperService.getNow();
            DatesClotureComptableDataService.unClosePeriod($ctrl.monthModel, now);
            $ctrl.isEditable = true;
            $scope.$emit('datesClotureComptableRowComponent.dateChange', $ctrl.monthModel);
        }

        function onConfirmClotureSuccess() {
            var now = DatesClotureComptableHelperService.getNow();
            DatesClotureComptableDataService.closePeriod($ctrl.monthModel, now);
            $scope.$emit('datesClotureComptableRowComponent.dateChange', $ctrl.monthModel);
        }

        $ctrl.periodeClosed = function () {
            var now = DatesClotureComptableHelperService.getNow();
            var result = DatesClotureComptableStatusService.periodeClosed($ctrl.monthModel, now);
            return result;
        };

        $ctrl.periodInactive = function () {
            var now = DatesClotureComptableHelperService.getNow();
            var result = DatesClotureComptableStatusService.periodInactive($ctrl.monthModel, now);
            return result;
        };

        $ctrl.periodInProgress = function () {
            var now = DatesClotureComptableHelperService.getNow();
            var result = DatesClotureComptableStatusService.periodInProgress($ctrl.monthModel, now);
            return result;
        };


        $ctrl.isAble = function () {
            $ctrl.isOnClosing = false;
        };

        $ctrl.isNotAble = function () {
            $ctrl.isOnClosing = true;
        };

        $scope.$on('startClosing', function (event, data) {
            $ctrl.isNotAble();
        });

        $scope.$on('endClosing', function (event, data) {
            $ctrl.isAble();
        });
    }
})();