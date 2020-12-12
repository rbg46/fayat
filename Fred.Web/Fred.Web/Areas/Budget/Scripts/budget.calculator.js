(function () {
    'use strict';

    angular.module('Fred').service('BudgetCalculator', BudgetCalculator);

    function BudgetCalculator() {

        return {
            GetValue: GetValue,
            Calculate: Calculate,
            EqualsRounded: EqualsRounded 
        };

        // Parse une valeur en Number.
        // - value : la valeur concernée.
        // - return : la bonne valeur.
        function GetValue(value) {
            if (value === 0) {
                return 0;
            }
            if (!value || value === '') {
                return null;
            }
            return Number(value);
        }

        // compare 2 valeurs arrondies
        // - value1 : la 1ere valeur à comparer.
        // - value2 : la 2eme valeur à comparer.
        // - decimals : le nombre de décimales à arrondir.
        // - return : true si equal sinon false.
        function EqualsRounded(value1, value2, decimals) {
            var compareValue1 = value1;
            var compareValue2 = value2;
            if (!compareValue1 || compareValue1 === '') {
                compareValue1 = Number.MIN_SAFE_INTEGER;
            }
            if (!compareValue2 || compareValue2 === '') {
                compareValue2 = Number.MIN_SAFE_INTEGER;
            }
            compareValue1 = Number(compareValue1);
            compareValue2 = Number(compareValue2);

            return compareValue1.toFixed(decimals) === compareValue2.toFixed(decimals);
        }

        // Corrige le problème d'arrondi sur les float.
        // - value : la valeur concernée.
        // - return : la valeur corrigée.
        function Calculate(value) {
            return value === null ? null : Number(value.toFixed(12));
        }
    }
})();
