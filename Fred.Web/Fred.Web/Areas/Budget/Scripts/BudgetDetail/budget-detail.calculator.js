(function () {
    'use strict';

    angular.module('Fred').service('BudgetDetailCalculator', BudgetDetailCalculator);
    BudgetDetailCalculator.$inject = ['BudgetCalculator'];

    function BudgetDetailCalculator(BudgetCalculator) {

        return {
            Calculate: Calculate
        };

        // Calcule l'ensemble d'un détail.
        // - budget : le budget concerné.
        function Calculate(budget) {
            budget.Montant = 0;
            for (var tache1 of budget.Taches1) {
                tache1.Montant = null;
                for (let tache2 of tache1.Taches2) {
                    tache2.Montant = null;
                    for (let tache3 of tache2.Taches3) {
                        tache3.Montant = null;
                        for (let tache4 of tache3.Taches4) {
                            if (!tache4.Deleted && tache4.BudgetT4.View.MontantT4 !== null) {
                                tache3.Montant = tache3.Montant === null ? tache4.BudgetT4.View.MontantT4 : BudgetCalculator.Calculate(tache3.Montant + tache4.BudgetT4.View.MontantT4);
                            }
                        }
                        if (tache3.Montant !== null) {
                            tache2.Montant = tache2.Montant === null ? tache3.Montant : BudgetCalculator.Calculate(tache2.Montant + tache3.Montant);
                        }
                    }
                    if (tache2.Montant !== null) {
                        tache1.Montant = tache1.Montant === null ? tache2.Montant : BudgetCalculator.Calculate(tache1.Montant + tache2.Montant);
                    }
                }
                if (tache1.Montant !== null) {
                    budget.Montant = BudgetCalculator.Calculate(budget.Montant + tache1.Montant);
                }
            }
        }
    }
})();
