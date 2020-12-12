(function () {
    'use strict';

    var bibliothequePrixEnregistrementSurCiComponent = {
        templateUrl: '/Areas/Budget/Scripts/BibliothequePrix/components/bibliotheque-prix-enregistrement-sur-ci.component.html',
        bindings: {},
        controller: bibliothequePrixEnregistrementSurCiController
    };

    angular.module('Fred').component('bibliothequePrixEnregistrementSurCiComponent', bibliothequePrixEnregistrementSurCiComponent);

    angular.module('Fred').controller('bibliothequePrixEnregistrementSurCiController', bibliothequePrixEnregistrementSurCiController);

    bibliothequePrixEnregistrementSurCiController.$inject = ['$scope', 'BudgetService'];

    function bibliothequePrixEnregistrementSurCiController($scope, BudgetService) {

        var $ctrl = this;
        $ctrl.resources = resources;
        $ctrl.BudgetsBrouillons = null;
        $ctrl.OnContinue = null;

        $ctrl.UpdateValeursSurLignesEnException = false;
        $ctrl.SelectAllValue = false;

        $ctrl.IsSaveAllowed = IsSaveAllowed;
        $ctrl.SelectAllCbChange = SelectAllCbChange;
        $ctrl.EnregistrerSurCi = EnregistrerSurCi;

        $ctrl.Identifiers = {
            Dialog: "BIBLIOTHEQUE_PRIX_ENREGISTREMENT_SUR_CI_DIALOG_ID"
        };

        $scope.$on(BudgetService.Events.BibliothequePrixEnregistrementSurCiDialog, function (event, arg) { Show(arg); });

        function Show(arg) {
            $ctrl.BudgetsBrouillons = arg.BudgetsBrouillons;
            $ctrl.OnContinue = arg.OnContinue;
            $ctrl.SelectAllValue = true;
            SelectAllCbChange();
            ShowDialog();
        }

        function IsSaveAllowed() {
            return $ctrl.BudgetsBrouillons && $ctrl.BudgetsBrouillons.some((budget) => {
                return budget.PropageSurBudget;
            });
        }

        function EnregistrerSurCi() {
            let budgetIdAEnregistrer = $ctrl.BudgetsBrouillons
                .filter(BudgetBrouillonAPropager)
                .map(BudgetBrouillonMapId);

            if ($ctrl.OnContinue) {
                $ctrl.OnContinue({
                    BudgetIdAEnregistrer: budgetIdAEnregistrer,
                    UpdateValeursSurLignesEnException: $ctrl.UpdateValeursSurLignesEnException
                });

            }
        }

        function BudgetBrouillonAPropager(budget) {
            return budget.PropageSurBudget;
        }

        function BudgetBrouillonMapId(budget) {
            return budget.BudgetId;
        }

        function SelectAllCbChange() {
            $ctrl.BudgetsBrouillons.forEach((budget) => {
                budget.PropageSurBudget = $ctrl.SelectAllValue;
            });
        }

        function ShowDialog() {
            $('#' + $ctrl.Identifiers.Dialog).modal('show');
        }
    }
})();
