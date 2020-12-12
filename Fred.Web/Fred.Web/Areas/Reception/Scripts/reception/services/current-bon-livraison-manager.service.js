(function () {
    'use strict';

    angular.module('Fred').service('CurrentBonLivraisonManagerService', CurrentBonLivraisonManagerService);

    CurrentBonLivraisonManagerService.$inject = [];

    function CurrentBonLivraisonManagerService() {

        var lastNumeroCommande = '';
        var currentBL = null;

        this.setCurrentBonLivraison = function (bonLivraison) {
            currentBL = bonLivraison;
        };

        this.setCurrentNumeroCommande = function (commandeLigne) {
            lastNumeroCommande = !commandeLigne.numeroCommandeExterne ? angular.copy(commandeLigne.numeroCommande) : angular.copy(commandeLigne.numeroCommandeExterne);
        };

        this.getCurrentBonLivraison = function (commandeLigne, reception) {
            if (reception.NumeroBL !== null && reception.NumeroBL !== "") {
                return reception.NumeroBL;
            }
            if ((commandeLigne.numeroCommandeExterne && lastNumeroCommande !== commandeLigne.numeroCommandeExterne)
                || lastNumeroCommande !== commandeLigne.numeroCommande) {
                currentBL = null;
            }
            return currentBL;
        };

    }
})();
