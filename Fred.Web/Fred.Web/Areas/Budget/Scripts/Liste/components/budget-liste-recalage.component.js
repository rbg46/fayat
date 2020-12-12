(function () {
    'use strict';

    var budgetListeRecalageComponent = {
        templateUrl: '/Areas/Budget/Scripts/Liste/components/budget-liste-recalage.component.html',
        bindings: {
            budgetId: '<',
            listPeriodes: '<',
            resources: '<'
        },
        controller: budgetListeRecalageController
    };
    angular.module('Fred').component('budgetListeRecalageComponent', budgetListeRecalageComponent);
    angular.module('Fred').controller('budgetListeRecalageController', budgetListeRecalageController);
    budgetListeRecalageController.$inject = ['$scope', 'BudgetService', 'ProgressBar', 'Notify'];

    function budgetListeRecalageController($scope, BudgetService, ProgressBar, Notify) {

        //////////////////////////////////////////////////////////////////
        // Membres publiques                                            //
        //////////////////////////////////////////////////////////////////
        var $ctrl = this;

        //////////////////////////////////////////////////////////////////
        // Fonctions publiques                                          //
        //////////////////////////////////////////////////////////////////
        $ctrl.RecalageBudgetaire = RecalageBudgetaire;

        //////////////////////////////////////////////////////////////////
        // Initialisation                                               //
        //////////////////////////////////////////////////////////////////
        $ctrl.$onInit = function () {
            $ctrl.resources = resources;
            $scope.$on(BudgetService.Events.DisplayRecalage, function (event, arg) { ShowRecalage(arg); });
        };

        //////////////////////////////////////////////////////////////////
        // Evènements                                                   //
        //////////////////////////////////////////////////////////////////
        function ShowRecalage(arg) {
            Show();
        }

        //////////////////////////////////////////////////////////////////
        // Validation du budget                                         //
        //////////////////////////////////////////////////////////////////
        function RecalageBudgetaire() {
            ProgressBar.start();
            BudgetService.RecalageBudgetaire($ctrl.budgetId, ParsePeriode($ctrl.periodeFin))
                .then(RecalageBudgetaireThen)
                .catch(RecalageBudgetaireCatch)
                .finally(ProgressBarComplete);
        }


        function RecalageBudgetaireThen(response) {
            RefreshListBudget(response.data);
            Notify.message("Recalage terminé, une nouvelle version du budget à été généré");
        }

        function RecalageBudgetaireCatch(res) {
            if (res && res.data && res.data.ExceptionMessage) {
                var exception = res.data.ExceptionMessage;
                Notify.error("Echec du recalage budgétaire : " + exception);
            }
            else {
                Notify.error("Echec du recalage budgétaire");
            }
        }

        function ProgressBarComplete() {
            Hide();
            ProgressBar.complete();
        }

        //////////////////////////////////////////////////////////////////
        // Autre                                                        //
        //////////////////////////////////////////////////////////////////

        function RefreshListBudget(nouveauBudgetId) {
            $scope.$emit(BudgetService.Events.RefreshListBudget, { budgetId: nouveauBudgetId });
        }

        function ParsePeriode(periode) {
            var periodeTab = periode.split('/');
            return periodeTab[1] + periodeTab[0];
        }

        function Show() {
            $('#BUDGET_LISTE_RECALAGE_ID').modal();
        }

        function Hide() {
            $('#BUDGET_LISTE_RECALAGE_ID').modal('hide');
        }
    }
})();
