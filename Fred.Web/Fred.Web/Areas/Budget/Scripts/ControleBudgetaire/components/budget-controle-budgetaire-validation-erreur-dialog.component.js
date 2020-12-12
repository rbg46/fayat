(function () {
    'use strict';

    var budgetControleBudgetaireValidationErreurDialogComponent = {
        templateUrl: '/Areas/Budget/Scripts/ControleBudgetaire/components/budget-controle-budgetaire-validation-erreur-dialog.component.html',
        bindings: {
            resources: '<'
        },
        controller: budgetControleBudgetaireValidationErreurDialogController
    };
    angular.module('Fred').component('budgetControleBudgetaireValidationErreurDialogComponent', budgetControleBudgetaireValidationErreurDialogComponent);
    angular.module('Fred').controller('budgetControleBudgetaireValidationErreurDialogController', budgetControleBudgetaireValidationErreurDialogController);
    budgetControleBudgetaireValidationErreurDialogController.$inject = ['$scope', 'BudgetService', 'StringFormat'];

    function budgetControleBudgetaireValidationErreurDialogController($scope, BudgetService, StringFormat) {

        //////////////////////////////////////////////////////////////////
        // Membres publiques                                           //
        //////////////////////////////////////////////////////////////////
        var $ctrl = this;
        $ctrl.Hide = Hide;
        $ctrl.resources = resources;


        //////////////////////////////////////////////////////////////////
        // Initialisation                                               //
        //////////////////////////////////////////////////////////////////
        $ctrl.$onInit = function () {
            $scope.$on(BudgetService.Events.DisplayControleBudgetaireValidationErreurDialog, function (event, arg) { Show(arg); });
        };


        //////////////////////////////////////////////////////////////////
        // Evènements                                                   //
        //////////////////////////////////////////////////////////////////
        function Show(arg) {
            $ctrl.Message = StringFormat.Format($ctrl.resources.Budget_ControleBudgetaire_Validation_Erreur_Message, arg.Periode);
            $ctrl.AvancementValide = arg.AvancementValide;
            $ctrl.PeriodeComptableCloturee = arg.PeriodeComptableCloturee;
            var modal = $('#CONTROLE_BUDGETAIRE_VALIDATION_ERREUR_DIALOG_ID');
            modal.modal('show');
        }
        function Hide() {
            $('#CONTROLE_BUDGETAIRE_VALIDATION_ERREUR_DIALOG_ID').modal('hide');
        }
    }
})();
