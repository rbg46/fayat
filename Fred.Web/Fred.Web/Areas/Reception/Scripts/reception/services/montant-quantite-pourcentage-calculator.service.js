
(function () {
    'use strict';

    angular.module('Fred').service('MontantQuantitePourcentageCalculatorService', MontantQuantitePourcentageCalculatorService);

    MontantQuantitePourcentageCalculatorService.$inject = [];

    function MontantQuantitePourcentageCalculatorService() {

        this.actionUpdateFigures = function (data) {
            angular.forEach(data, function (cmd) {

                var mtntReceptionneCmdLigne = 0;

                angular.forEach(cmd.Lignes, function (cmdLigne) {

                    var qteReceptionnee = 0;
                    var mtntReceptionne = 0;

                    angular.forEach(cmdLigne.DepensesReception, function (rcpt) {

                        qteReceptionnee += rcpt.Quantite;
                        mtntReceptionne += rcpt.MontantHT;

                    });

                    cmdLigne.QuantiteReceptionnee = qteReceptionnee;
                    cmdLigne.MontantHTReceptionne = mtntReceptionne;
                    mtntReceptionneCmdLigne += cmdLigne.MontantHTReceptionne;

                });

                cmd.MontantHTReceptionne = mtntReceptionneCmdLigne;
                cmd.PourcentageReceptionne = cmd.MontantHT > 0 ? cmd.MontantHTReceptionne / cmd.MontantHT * 100 : 100;

            });
        };

    }
})();
