(function () {
    'use strict';

    angular.module('Fred').service('BudgetSousDetailCalculator', BudgetSousDetailCalculator);
    BudgetSousDetailCalculator.$inject = ['BudgetCalculator'];

    function BudgetSousDetailCalculator(BudgetCalculator) {

        return {
            CalculateBudgetMontant: CalculateBudgetMontant
        };

        // Calcule le montant d'un budget en fonction du sous-détail en cours.
        // - sousDetail : le sous-détail en cours.
        function CalculateBudgetMontant(sousDetail) {
            if (sousDetail.BudgetT4.View.MontantT4 !== null) {
                sousDetail.Budget.Montant = BudgetCalculator.Calculate(sousDetail.MontantBudgetSansCeT4 + sousDetail.BudgetT4.View.MontantT4);
            }
            else {
                sousDetail.Budget.Montant = sousDetail.MontantBudgetSansCeT4;
            }
        }
    }
})();
