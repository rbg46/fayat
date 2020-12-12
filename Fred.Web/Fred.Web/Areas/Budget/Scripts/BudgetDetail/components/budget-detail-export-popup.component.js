(function () {
    'use strict';

    var budgetDetailExportPopupComponent = {
        templateUrl: '/Areas/Budget/Scripts/BudgetDetail/components/budget-detail-export-popup.component.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: budgetDetailExportPopupController
    };
    angular.module('Fred').component('budgetDetailExportPopupComponent', budgetDetailExportPopupComponent);
    angular.module('Fred').controller('budgetDetailExportPopupController', budgetDetailExportPopupController);
    budgetDetailExportPopupController.$inject = ['$scope', 'BudgetService', 'ProgressBar', 'Notify'];

    function budgetDetailExportPopupController($scope, BudgetService, ProgressBar, Notify) {

        var $ctrl = this;

        $ctrl.$onInit = function () {
            $ctrl.resources = $ctrl.resolve.resources;
        };

        $ctrl.SelectReport = function (report) {
            unselectAll();
            if (report === 1)
                $ctrl.IsReportAnalyseSelected = true;
            else
                $ctrl.IsReportEditableSelected = true;
        };

        $ctrl.NoReportSelected = function () {
            return !$ctrl.IsReportAnalyseSelected && !$ctrl.IsReportEditableSelected;
        };

        $ctrl.Export = function () {
            $ctrl.close({ $value: $ctrl.IsReportAnalyseSelected? 'Analyse' : 'Editable' });
        };

        $ctrl.Cancel = function () {
            $ctrl.dismiss({ $value: 'cancel' });
        };

        function unselectAll() {
            $ctrl.IsReportAnalyseSelected = false;
            $ctrl.IsReportEditableSelected = false;
        }
    }

})();