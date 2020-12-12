(function () {
  'use strict';

    angular.module('Fred').service('FamilleOperationDiverseService', FamilleOperationDiverseService);

    FamilleOperationDiverseService.$inject = ['$http'];

    function FamilleOperationDiverseService($http) {

        return {

            /* Service de recherche par société */
            SearchFamilleOperationDiverseBySociete: function (societeId) {
                return $http.post('/api/FamilleOperationDiverse/SearchAllBySocieteId/' + societeId, { cache: false });
            },

            /* Service de mise à jour des familles OD */
            UpdateFamilleOperationDiverse: function (famille) {
                return $http.put('/api/FamilleOperationDiverse/UpdateFamilleOperationDiverse', famille, { cache: false });
            },

            /* Service de création du Excel des erreurs de paramétrage */
            GenerateExportExcelErreursParametrage: function (exportExcelModel) {
                return $http.post("/api/FamilleOperationDiverse/GenerateExportExcelErreursParametrage/", exportExcelModel);
            },

            /* Service de Téléchargement du Excel des erreurs de paramétrage */
            DownloadExportExcelErreursParametrage: function (exportId, societeId) {
                window.location.href = "/api/FamilleOperationDiverse/ExportErreursParametrage/" + exportId + '/' + societeId;
            },

            /* Service de contrôle du paramétrage pour les journaux */
            LaunchControleParametrageForJournal: function (societeId) {
                return $http.post('/api/FamilleOperationDiverse/LaunchControleParametrageForJournal/' + societeId);
            },

            /* Service de contrôle du paramétrage pour les natures */
            LaunchControleParametrageForNature: function (societeId) {
                return $http.post('/api/FamilleOperationDiverse/LaunchControleParametrageForNature/' + societeId);
            },

            GetFamilleOD: function (societeId) {
                return $http.get('api/FamilleOperationDiverse/Get/' + societeId);
            },

            /* Service de création du Excel des familles d'OD et journaux */
            GenerateExportExcelForJournal: function (societeId, typeDonnee) {
                return $http.post("/api/FamilleOperationDiverse/GenerateExportExcelForJournal/" + societeId + '/' + typeDonnee);
            },

            /* Service de création du Excel des familles d'OD et natures */
            GenerateExportExcelForNature: function (societeId, typeDonnee) {
                return $http.post("/api/FamilleOperationDiverse/GenerateExportExcelForNature/" + societeId + '/' + typeDonnee);
            },

            /* Service de Téléchargement du Excel des familles d'OD */
            DownloadExportExcel: function (exportId, societeId, typeDonnee) {
                window.location.href = '/api/FamilleOperationDiverse/Export/' + exportId + '/' + societeId + '/' + typeDonnee;
            },

            /* Service de recherche d'une societe */
            GetSociete: function (societeId) {
                return $http.get('api/FamilleOperationDiverse/GetSociete/' + societeId);
            },

            GetAllParametrages: function (societeId) {
                return $http.get('/api/FamilleOperationDiverse/ParametrageNaturesJournaux/' + societeId);
            },

            SetParametrageNaturesJournaux: function (famille) {
                return $http.post('/api/FamilleOperationDiverse/ParametrageNaturesJournaux', famille, { cache: false });
            },

            /* Service de recherche du filtre type OD de l'explorateur de dépenses */
            GetOdTypesByCompanyId: function (societeId) {
                return $http.get('/api/FamilleOperationDiverse/GetOdTypesByCompanyId/' + societeId);
            }
        };
    }
})();