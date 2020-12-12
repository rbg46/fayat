(function (angular) {
    'use strict';

    angular.module('Fred').component('budgetCIComponent', {
        templateUrl: '/Areas/Budget/Scripts/ControleBudgetaire/components/budget-ci-modal.component.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: 'BudgetCIComponentController'
    });

    BudgetCIComponentController.$inject = ['Notify'];

    function BudgetCIComponentController(Notify) {
        var $ctrl = this;

        $ctrl.$onInit = $onInit;
        $ctrl.handleValidate = handleValidate;
        $ctrl.isMaximumCiSelected = isMaximumCiSelected;
        $ctrl.handleCancel = handleCancel;
        $ctrl.addCi = addCi;
        $ctrl.removeCi = removeCi;

        function $onInit() {
            $ctrl.resources = $ctrl.resolve.resources;
            $ctrl.periode = $ctrl.resolve.periode;
            $ctrl.ci = $ctrl.resolve.ci;
            $ctrl.ciSelectedlist = $ctrl.resolve.ciSelectedlist;
            if (!$ctrl.ciSelectedlist.find(x => x.CiId === $ctrl.ci.CiId)) {
                $ctrl.ciSelectedlist.push($ctrl.ci);
            }
        }

        function addCi() {
            if ($ctrl.isMaximumCiSelected()) {
                Notify.error($ctrl.resources.Global_Notification_MaxAtteint);
                return;
            }
            if ($ctrl.ciSelectedlist.findIndex(x => x.CiId === $ctrl.input.CiId) === -1)
                $ctrl.ciSelectedlist.push($ctrl.input);
        }

        function removeCi(index) {
            $ctrl.ciSelectedlist.splice(index, 1);
        }

        function handleValidate() {
            $ctrl.close({ $value: $ctrl.ciSelectedlist });
        }

        function handleCancel() {
            $ctrl.dismiss({ $value: 'cancel' });
        }

        function isMaximumCiSelected() {
            return $ctrl.ciSelectedlist.length > 5;
        }
    }

    angular.module('Fred').controller('BudgetCIComponentController', BudgetCIComponentController);

}(angular));