(function () {
    'use strict';

    angular.module('Fred').service('CommandeHistoriqueService', CommandeHistoriqueService);

    CommandeHistoriqueService.$inject = ['$http'];

    /**
     * Service de gestion de l'historique d'une commande
     * @param {any} $http Identifiant d'une commande
     */
    function CommandeHistoriqueService($http) {
        var vm = this;

        /**
         * Récupérer tous les events sur une commande
         * @param {any} commandeId Identifiant d'une commande
         * @returns {any} Liste des Historiques
         */
        vm.GetHistorique = function (commandeId) {
            return $http.get(
                `/api/Commande/GetHistorique/${commandeId}`,
                {
                    cache: false
                }
            );
        };

    }
})();

