(function () {
    'use strict';

    angular.module('Fred').service('NatureService', NatureService);

    NatureService.$inject = ['$http'];

    function NatureService($http) {

        return {
            /* Service de création d'une nouvelle instance de code nature */
            New: function (societeId) {
                return $http.get('/api/Nature/New/' + societeId, { cache: false });
            },
            /* Service de recherche par société*/
            SearchCodeNatureBySociete: function (filters, societeId, searchText) {
                return $http.post('/api/Nature/SearchAllBySocieteId/' + societeId + '/' + searchText, filters, { cache: false });
            },
            /* Service de test d'existence du code CodeNature */
            Exists: function (idCourant, codeCodeNature, societeId) {
                return $http.get('/api/Nature/CheckExistsCode/' + idCourant + '/' + codeCodeNature + '/' + societeId, idCourant, codeCodeNature, societeId, { cache: false });
            },
            /* Service Create */
            Create: function (model) {
                return $http.post('/api/Nature/', model, { cache: false });
            },
            /* Service Update */
            Update: function (model) {
                return $http.put('/api/Nature/', model, { cache: false });
            },
            /* Service Delete */
            Delete: function (model) {
                return $http.post('/api/Nature/Delete/', model, { cache: false });
            },
            GetNatureListBySocieteId: function (societeId) {
                return $http.get('/api/Nature/' + societeId, { cache: false });
            },
            isAlreadyUsed: function (id) {
                return $http.get('/api/Nature/IsAlreadyUsed/' + id);
            },
            /* Service Génération du Excel */
            GenerateExportExcel: function (exportexcelModel) {
                return $http.post("/api/Nature/GenerateExportExcel/", exportexcelModel);
            },
            /* Service Téléchargement du Excel */
            DownloadExportExcel: function (exportId) {
                window.location.href = "/api/Nature/DownloadExportExcel/" + exportId;
            },

            GetNatureFamilleOdBySocieteId: function (societeId) {
                return $http.get('/api/Nature/GetNatures/' + societeId);
            },
            /* Service Update d'une liste de natures */
            UpdateNatureFamilleOd: function (model) {
                return $http.put('/api/Nature/UpdateNatures/', model, { cache: false });
            },
            /* Service de récupération d'une Ressource par son identifiant */
            GetRessourceById: function (ressourceId) {
                return $http.get('/api/ReferentielFixes/GetRessource/' + ressourceId, { cache: false });
            }
        };
    }
})();