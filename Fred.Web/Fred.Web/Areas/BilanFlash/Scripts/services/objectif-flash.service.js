
(function () {
    'use strict';

    angular.module('Fred').service('ObjectifFlashService', ObjectifFlashService);

    ObjectifFlashService.$inject = ['$http'];

    function ObjectifFlashService($http) {

        // Récupération des filtres de recherche pour la liste des objectifs flash
        this.GetNewObjectifFlashFilter = function () {
            return $http.get("/api/ObjectifFlash/Filter");
        };

        // Récupération de la liste des Objectifs Flash
        this.getObjectifFlashList = function (filters, paging) {
            return $http.post("/api/ObjectifFlash/Search/" + paging.page + "/" + paging.pageSize, filters);
        };

        // Récupération d'un Object Objectif Flash Vide
        this.getNewObjectifFlash = function () {
            return $http.get("/api/ObjectifFlash/New/");
        };

        // Récupération d'un Objectif Flash par identifiant unique 
        this.getObjectifFlash = function (id) {
            return $http.get("/api/ObjectifFlash/" + id);
        };

        // Ajout d'un nouvel Objectif Flash
        this.addNewObjectifFlash = function (objectifFlashModel) {
            return $http.post("/api/ObjectifFlash", objectifFlashModel, { cache: false });
        };

        // Mise à jour d'un Objectif Flash
        this.updateObjectifFlash = function (objectifFlashModel) {
            return $http.put("/api/ObjectifFlash", objectifFlashModel, { cache: false });
        };

        // Activation d'un Objectif Flash
        this.activateObjectifFlash = function (objectifFlashId) {
            return $http.put("/api/ObjectifFlash/Activate/" + objectifFlashId);
        };

        // Suppression logique d'un objectif flash
        this.deleteObjectifFlash = function (objectifId) {
            return $http.put("/api/ObjectifFlash/DeleteObjectifFlash/" + objectifId);
        };

        // Duplication d'un objectif flash
        this.duplicateObjectifFlash = function (date, objectifId) {
            return $http.post("/api/ObjectifFlash/Duplicate/" + objectifId + "/" + date);
        };

        //Récupération des ressources du budget en application du CI
        this.GetRessourcesInBudgetEnApplicationByCiId = function (ciId, tacheId) {
            return $http.get("/api/ObjectifFlash/GetRessourcesInBudgetEnApplicationByCiId/" + ciId + "/" + tacheId);
        };

        // Récupération devise de référence d'un CI
        this.getDeviseRefForCi = function (ciId) {
            return $http.get("/api/CI/DeviseRef/" + ciId + "/");
        };

        //Récupération de la bibliotheque des prix
        this.GetRessourcesInBibliothequePrix = function (ciSelected, deviseSelected, searchText, page, pageSize) {
            return $http.get("/api/ObjectifFlash/GetRessourcesInBibliothequePrix?ciId=" + ciSelected.CiId + "&deviseId=" + deviseSelected.DeviseId + "&filter=" + searchText + "&page=" + page + "&pageSize=" + pageSize);
        };

        //Récupération du Barème exploitation du CI
        this.GetRessourcesInBaremeExploitation = function (ciSelected, dateDebut) {
            return $http.get("/api/ObjectifFlash/GetRessourcesInBaremeExploitation?ciId=" + ciSelected.CiId + "&periode=" + (dateDebut.getMonth() + 1) +"-"+ dateDebut.getFullYear());
        };

        // Récupération d'un Objectif flash journalisé
        this.GetNewJournalisation = function (objectifFlashModel) {
            return $http.post("/api/ObjectifFlash/NewJournalisation", objectifFlashModel, { cache: false });
        };

        // Récupération d'un Objectif flash journalisé
        this.GetReportedJournalisation = function (date, objectifFlashModel) {
            return $http.post("/api/ObjectifFlash/ReportJournalisation/" + date + "/", objectifFlashModel, { cache: false });
        };

        // Export d'un bilan flash
        this.ExportBilanFlash = function (templateName, objectifFlashId, startDate, endDate, isPdf) {
            if (templateName === 'BilanFlash') {
                return $http.get("/api/ObjectifFlash/ExportBilanFlash/" + objectifFlashId + "/" + startDate + "/" + endDate + "/" + isPdf, { cache: false });
            }
            else {
                return $http.get("/api/ObjectifFlash/ExportBilanFlashSynthese/" + objectifFlashId + "/" + startDate + "/" + endDate + "/" + isPdf, { cache: false });
            }
        };

        //  Download de l'Export d'un bilan flash
        this.ExportBilanFlashDownload = function (guidIdExport, fileName) {
            window.location.href = '/api/ObjectifFlash/ExportBilanFlashDownload/' + guidIdExport + '/' + fileName;
        };
    }
})();