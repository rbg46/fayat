(function () {
    'use strict';

    var planTacheLateralComponent = {
        templateUrl: '/Areas/Budget/Scripts/BudgetSousDetail/components/budget-plan-tache-lateral.component.html',
        bindings: {
            taches: '<',
            resources: '<'
        },
        controller: budgetPlanTacheController
    };

    angular.module('Fred').component('planTacheLateralComponent', planTacheLateralComponent);
    angular.module('Fred').controller('budgetPlanTacheController', budgetPlanTacheController);
    budgetPlanTacheController.$inject = ['$scope', '$timeout', 'BudgetService'];

    function budgetPlanTacheController($scope, $timeout, BudgetService) {
        var $ctrl = this;
        $ctrl.showBandeauLateral = false;
        $ctrl.displayThisT4 = displayThisT4;

        $scope.$on(BudgetService.Events.DisplayPlanTacheLateral, DisplayPlanTacheLateral);

        return $ctrl;

        function displayThisT4(tache4) {
            $ctrl.showBandeauLateral = false;
            $scope.$emit(BudgetService.Events.LoadSousDetail, { Tache4: tache4 });
        }

        function DisplayPlanTacheLateral() {
            $ctrl.showBandeauLateral = true;
            // Un rafraichissement est requis pour firefox
            $timeout(() => {
                document.getElementById("FLEX_TABLE_PLAN_TACHE_LATERAL").refresh();
            });
        }
    }
})();
