(function () {
    'use strict';

    angular.module('Fred').service('BudgetDateService', BudgetDateService);
    BudgetDateService.$inject = [];

    function BudgetDateService() {
        var service = this;


        service.formatPeriodeToApiYYYYMMFormat = function (date) {
            //Selon l'API PeriodeDebut et PeriodeFin sont des entiers au format YYYYMM
            //Donc on formatte la période saisie pour qu'elle soit à ce format
            var selectedYear = date.getFullYear();
            var selectedMonth = date.getMonth() + 1;
            if (selectedMonth < 10) {
                selectedYear = selectedYear * 10;
            }
            let selectedPeriode = parseInt(selectedYear.toString() + selectedMonth.toString());
            return selectedPeriode;
        };

        return service;
    }
})();