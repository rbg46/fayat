(function () {
    'use strict';

    angular.module('Fred').service('EtablissementPaieService', EtablissementPaieService);

    EtablissementPaieService.$inject = ['$http', '$q'];

    function EtablissementPaieService($http, $q) {
        var uriBase = "/api/EtablissementPaie/";

        return {

            /* Service de création d'une nouvelle instance d'établissement de paie */
            New: function (societeId) {
                return $q(function (resolve, reject) {
                    $http.get(uriBase + 'New/' + societeId, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service de test d'existence de l'établissement de paie */
            Exists: function (idCourant, code, libelle) {
                return $q(function (resolve, reject) {
                    $http.get(uriBase + 'CheckExistsCodeLibelle/' + code + '/' + libelle + '/' + idCourant, code, libelle, idCourant, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service de test d'existence du code déplacement */
            CodeExists: function (idCourant, code, societeId) {
                return $q(function (resolve, reject) {
                    $http.get(uriBase + 'CheckExistsCode/' + idCourant + '/' + code + '/' + societeId, code, idCourant, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service Get objet de recherche */
            GetFilter: function () {
                return $q(function (resolve, reject) {
                    $http.get(uriBase + 'Filter/', { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service GetEtablissementsPaie */
            Get: function () {
                return $q(function (resolve, reject) {
                    $http.get(uriBase + 'All/', { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service de recherche */
            Search: function (model, societeId, searchText) {
                return $q(function (resolve, reject) {
                    $http.post(uriBase + 'SearchAll/' + societeId + '/' + searchText, model, searchText, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service GetEtablissementsPaie */
            GetBySocieteId: function (id) {
                return $q(function (resolve, reject) {
                    $http.get(uriBase + 'BySocieteID/' + id, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service Create */
            Create: function (model) {
                return $q(function (resolve, reject) {
                    $http.post(uriBase, model, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service Update */
            Update: function (model) {
                return $q(function (resolve, reject) {
                    $http.put(uriBase, model, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service Delete */
            Delete: function (model) {
                return $q(function (resolve, reject) {
                    $http.post(uriBase + 'Delete/', model, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            UpdatePersonnelsPointableByEtatPaieId: function (etabPaieId, isPersonnelsNonPointables) {
                return $q(function (resolve, reject) {
                    $http.post("/api/Personnel/" + 'UpdatePersonnelsPointableByEtatPaieId/' + etabPaieId + '/' + isPersonnelsNonPointables, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            }

        };
    }

})();