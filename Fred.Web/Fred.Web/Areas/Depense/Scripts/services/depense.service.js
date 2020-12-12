(function () {
    'use strict';

    angular.module('Fred').service('DepenseService', DepenseService);

    DepenseService.$inject = ['$http', '$q'];

    /*
     * @description Service des Dépenses
     */
    function DepenseService($http, $q) {
        var service =
        {
            GetExplorateurDepenseTree: GetExplorateurDepenseTree,
            GetDepensesByAxe: GetDepensesByAxe,
            GetDepensesByAxeForFayatTP: GetDepensesByAxeForFayatTP,
            GetNewFilter: GetNewFilter,
            GenerateExport: GenerateExport,
            GenerateExportForFayatTP: GenerateExportForFayatTP,
            GetExport: GetExport,
            IsSep: IsSep
        };

        /**
         * Requête de récupération de l'arbre d'exploration
         * @param {any} filter filtre searchExplorateurDepense
         * @returns {any} requête http
         */
        function GetExplorateurDepenseTree(filter) {
            return $http.post("/api/Depense/ExplorateurDepense/Tree", filter);
        }

        /**
         * Requête de récupération des dépenses en fonction des axes sélectionnés
         * @param {any} filter filtre searchExplorateurDepense
         * @param {any} page numéro de page
         * @param {any} pageSize taille de page
         * @returns {any} requête http
         */
        function GetDepensesByAxe(filter, page, pageSize) {
            return $http.post("/api/Depense/ExplorateurDepense/List/" + page + "/" + pageSize, filter);
        }

        /**
         * Requête de récupération des dépenses en fonction des axes sélectionnés pour Fayat TP
         * @param {any} filter filtre searchExplorateurDepense
         * @param {any} page numéro de page
         * @param {any} pageSize taille de page
         * @returns {any} requête http
         */
        function GetDepensesByAxeForFayatTP(filter, page, pageSize) {
            return $http.post("/api/Depense/ExplorateurDepenseFayatTP/List/" + page + "/" + pageSize, filter);
        }

        /**
         * Requête de récupération d'un nouveau filtre
         * @returns {any} requête http
         */
        function GetNewFilter() {
            return $http.get("/api/Depense/ExplorateurDepense/Filter");
        }

        /**
         * Génération du fichier excel
         * @param {any} filter filtre
         * @returns {any} requête http
         */
        function GenerateExport(filter) {
            return $http.post("/api/Depense/ExplorateurDepense/Export/", filter);
        }

        /**
         * Génération du fichier excel pour Fayat TP
         * @param {any} filter filtre
         * @returns {any} requête http
         */
        function GenerateExportForFayatTP(filter) {
            return $http.post("/api/Depense/ExplorateurDepenseFayatTP/Export/", filter);
        }

        /**
         * Redirection vers le lien du fichier excel
         * @param {any} id guid du cache où se trouve le fichier excel
         * @param {any} codeCi code du ci
         */
        function GetExport(id, codeCi) {
            window.location.href = '/api/Depense/ExplorateurDepense/Export/' + id + '/' + codeCi;
        }

        /**
        * Génération du fichier excel
        * @param {any} ciId id du CI
        * @returns {any} requête http
        */
        function IsSep(ciId) {
            return $http.get("/api/Depense/ExplorateurDepense/IsSep/" + ciId);
        }

        return service;
    }
})();