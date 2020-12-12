(function() {
    'use strict';

    angular.module('Fred').service('UtilisateurService', UtilisateurService);

    UtilisateurService.$inject = ['$http', '$q', '$resource'];

    function UtilisateurService($http, $q, $resource) {
        var vm = this;
        var uriBase = "/api/Utilisateur/";

        var resource = $resource(uriBase + ':cmd/:utilisateurId/:login',
            {}, //parameters default
            {
                New: {
                    method: "GET",
                    params: { cmd: "New" },
                    cache: false
                },
                NewUtilisateur: {
                    method: "GET",
                    params: { cmd: "NewUtilisateur" },
                    cache: false
                },
                Get: {
                    method: "GET",
                    params: { cmd: "All" },
                    isArray: true,
                    cache: false
                },
                GetCurrentUser: {
                    method: "GET",
                    params: { cmd: "CurrentUser" }
                },
                GetById: {
                    url: uriBase + ':utilisateurId',
                    method: "GET",
                    params: { utilisateurId: 0 },
                    cache: false
                },
                GetAffectationRole: {
                    method: "GET",
                    params: { cmd: "GetAffectationRole", utilisateurId: 0 },
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
                UpdateRole: {
                    method: "POST",
                    params: { cmd: "UpdateRole", utilisateurId: 0 },
                    isArray: true,
                },
                Delete: {
                    url: uriBase + ':utilisateurId',
                    method: "DELETE",
                    params: { utilisateurId: 0 }
                },
                IsLoginADExist: {
                    method: "POST",
                    params: { cmd: "IsLoginADExist" },
                    isArray: true,
                    cache: false
                }
            }
        );

        /**
         * Vérifie si un login existe déjà sur le serveur.
         * La paramètre idCourant permet de ne pas prendre en compte l'objet courant lors de la recherche et renvoyer 
         * "false" si le propriétaire du login a idCourant comme id.
         *      
         * @param {any} params paramètre
         * @param {any} callback  callback de succès
         * @param {any} errorCallback callback d'erreur
         * @returns {any} un objet contenant le résultat dans value si succès, ou l'erreur dans error.
         */
        vm.IsLoginExist = function(params) {
            var res = {};
            res.$promise = $http({
                method: 'POST',
                url: uriBase + 'CheckExistLogin/',
                cache: false,
                data: params
            }).then(function(response) {
                res.value = response.data;
                return res;
            });
            return res;
        };

        /**
         * Vérifie si un folio/trigramme existe déjà sur le serveur.
         * La paramètre idCourant permet de ne pas prendre en compte l'objet courant lors de la recherche et renvoyer 
         * "false" si le propriétaire du login a idCourant comme id.
         *      
         * @param {any} params paramètre
         * @param {any} callback  callback de succès
         * @param {any} errorCallback callback d'erreur
         * @returns {any} un objet contenant le résultat dans value si succès, ou l'erreur dans error.
         */
        vm.IsFolioExist = function(params) {
            var res = {};
            res.$promise = $http({
                method: 'GET',
                url: uriBase + 'CheckExistFolio/' + params.idCourant + '/' + params.folio + '/' + params.societeId,
                cache: false
            }).then(function(response) {
                res.value = response.data;
                return res;
            });
            return res;
        };

        this.GetSeuilUtilisateur = function(utilisateurId, ciId, deviseId) {
            return $http.get("/api/Utilisateur/Seuil/" + utilisateurId + "/" + ciId + "/" + deviseId);
        };

        this.GetRightPersonnelManagement = function(roleId) {
            return $http.get("/api/Utilisateur/RightPersonnelManagement/" + roleId);
        };

        this.GetAffectationSeuilUtilisateurByUtilisateurId = function(utilisateurId) {
            return $http.get("/api/Utilisateur/AffectationSeuil/" + utilisateurId);
        };

        angular.extend(vm, resource);
    }
})();

