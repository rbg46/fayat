(function () {
    'use strict';

    angular.module('Fred').service('FournisseurService', FournisseurService);

    FournisseurService.$inject = ['$http', '$q', '$resource'];

    function FournisseurService($http, $q, $resource) {
        var vm = this;

        var resource = $resource('/api/Fournisseur/:cmd/:groupeId/:fournisseurId/',
            {}, //parameters default
            {
                Get: {
                    method: "GET",
                    params: {},
                    isArray: true,
                    cache: false
                },
                GetById: {
                    url: '/api/Fournisseur/:fournisseurId',
                    method: "GET",
                    params: { fournisseurId: 0 },
                    cache: false
                },
                GetPersonnelInterimaire:
                {
                    method: "GET",
                    url: "/api/Fournisseur/GetPersonnelInterimaireList/:fournisseurId",
                    params: { fournisseurId: 0 },
                    cache: false,
                    isArray: true
                },
                GetFilter: {
                    method: "GET",
                    params: { cmd: "Filter" },
                    cache: false
                },
                Search: {
                    method: "POST",
                    url: "/api/Fournisseur/SearchFournisseurWithFilter/:page/:pageSize",
                    params: { page: 1, pageSize: 20 },
                    cache: false,
                    isArray: true
                },
                Create: {
                    method: "POST",
                    params: {}
                },
                Update: {
                    method: "PUT",
                    params: {}
                },
                Delete: {
                    url: '/api/Fournisseur/:utilisateurId',
                    method: "DELETE",
                    params: { fournisseurId: 0 }
                }
            }
        );

        /*
         * Récupère le nombre de personnel intérimaire lié à un fournisseur
         *      
         * @param {any} params paramètre
         * @param {any} callback  callback de succès
         * @param {any} errorCallback callback d'erreur
         * @returns {any} un objet contenant le résultat dans value si succès, ou l'erreur dans error.
         */
        vm.GetCountPersonnelInterimaire = function (params) {
            var res = {};
            res.$promise = $http({
                method: 'GET',
                url: '/api/Fournisseur/GetCountPersonnelInterimaire/' + params.fournisseurId,
                cache: false
            }).then(function (response) {
                res.value = response.data;
                return res;
            });
            return res;
        };

        /*
        * Exécution de l'import des Fournisseurs ANAEL vers FRED (sans paramètre: par défaut, on utilise le flux société RZB)
        * @param {any} params paramètre
        * @returns {any} vrai si l'opération s'est bien lancée.
        */
        vm.ExecuteImportFournisseur = function () {
            return $http.get('/api/Fournisseur/ImportFournisseur/');
        };

        angular.extend(vm, resource);
    }
})();