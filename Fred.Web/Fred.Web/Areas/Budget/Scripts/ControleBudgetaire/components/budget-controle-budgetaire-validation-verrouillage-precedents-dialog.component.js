(function () {
    'use strict';

    var budgetControleBudgetaireValidationVerrouillagePrecedentsDialogComponent = {
        templateUrl: '/Areas/Budget/Scripts/ControleBudgetaire/components/budget-controle-budgetaire-validation-verrouillage-precedents-dialog.component.html',
        bindings: {
            resources: '<'
        },
        controller: budgetControleBudgetaireValidationVerrouillagePrecedentsDialogController
    };
    angular.module('Fred').component('budgetControleBudgetaireValidationVerrouillagePrecedentsDialogComponent', budgetControleBudgetaireValidationVerrouillagePrecedentsDialogComponent);
    angular.module('Fred').controller('budgetControleBudgetaireValidationVerrouillagePrecedentsDialogController', budgetControleBudgetaireValidationVerrouillagePrecedentsDialogController);
    budgetControleBudgetaireValidationVerrouillagePrecedentsDialogController.$inject = ['$scope', 'BudgetService', 'StringFormat'];

    function budgetControleBudgetaireValidationVerrouillagePrecedentsDialogController($scope, BudgetService, StringFormat) {

        //////////////////////////////////////////////////////////////////
        // Membres publiques                                           //
        //////////////////////////////////////////////////////////////////
        var $ctrl = this;
        $ctrl.resources = resources;
        $ctrl.Hide = Hide;
        $ctrl.Validate = Validate;


        //////////////////////////////////////////////////////////////////
        // Initialisation                                               //
        //////////////////////////////////////////////////////////////////
        $ctrl.$onInit = function () {
            $scope.$on(BudgetService.Events.DisplayControleBudgetaireValidationVerrouillagePrecedentsDialog, function (event, arg) { Show(arg); });
        };


        //////////////////////////////////////////////////////////////////
        // Evènements                                                   //
        //////////////////////////////////////////////////////////////////
        function Show(arg) {
            $ctrl.Periodes = [];
            for (var i = 0; i < arg.Periodes.length; i++) {
                $ctrl.Periodes.push(StringFormat.Format($ctrl.resources.Budget_ControleBudgetaire_Validation_Verrouillage_Precedents_Periode, arg.Periodes[i]));
            }
            var modal = $('#CONTROLE_BUDGETAIRE_VALIDATION_VERROUILLAGE_PRECEDENTS_DIALOG_ID');
            modal.modal('show');
        }
        function Hide() {
            $('#CONTROLE_BUDGETAIRE_VALIDATION_VERROUILLAGE_PRECEDENTS_DIALOG_ID').modal('hide');
        }
        function Validate() {
            $scope.$emit(BudgetService.Events.DisplayControleBudgetaireValidationVerrouillagePrecedentsDialogValidated);
            Hide();
        }
    }
})();
