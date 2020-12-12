(function () {
    'use strict';

    angular.module('Fred').service('EtablissementComptableService', EtablissementComptableService);

    EtablissementComptableService.$inject = ['$http', '$q'];

    function EtablissementComptableService($http, $q) {

    var uriBase = "/api/EtablissementComptable/";
    const url = "/api/PieceJointe/DownloadCGA/";

        return {

            /* Service de création d'une nouvelle instance de code absence */
            New: function (societeId) {
                return $q(function (resolve, reject) {
                    $http.get(uriBase + 'New/' + societeId, { cache: false })
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

            /* Service de test d'existence du code EtablissementComptable */
            Exists: function (idCourant, codeEtablissementComptable, societeId) {
                return $q(function (resolve, reject) {
                    $http.get(uriBase + 'CheckExistsCode/' + codeEtablissementComptable + '/' + idCourant + '/' + societeId + '/', { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service GetSocieteAll */
            Get: function (societeId) {
                return $q(function (resolve, reject) {
                    $http.get(uriBase + 'All/' + societeId, { cache: false })
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

            /* Service Download CGA */
            DownloadCGA: function (CGAName) {
                // Url avec le CGA correspondant
                var urlCGA = `${url}${CGAName}`;
                // Télécharger le fichier
                if (CGAName != null)
                    window.open(urlCGA);
            }

        };
    }
})();