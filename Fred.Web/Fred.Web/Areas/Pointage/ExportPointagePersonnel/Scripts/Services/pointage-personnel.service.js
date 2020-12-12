(function () {
    'use strict';

    angular.module('Fred').service('PointagePersonnelService', PointagePersonnelService);

    PointagePersonnelService.$inject = ['$http', '$q', '$resource', '$filter'];

    function PointagePersonnelService($http, $q, $resource, $filter) {
        var vm = this;
        var uriBase = "api/PointagePersonnel/";
        vm.CodeDeplacementReadonly = false;
        vm.ShowSaisieManuelle = false;
        vm.ShowDeplacement = true;

        var resource = $resource(uriBase,
            {}, //parameters default
            {
                GetPointageByPersonnelIDAndInterval: {
                    method: "GET",
                    url: "/api/PointagePersonnel/GetPointageByPersonnelIDAndInterval",
                    params: {},
                    isArray: true
                }
            }
        );
        vm.GetPointagesByPersonnelIdAndPeriode = function (periode, personnelId) {
            return $http.get("/api/PointagePersonnel?periode=" + periode + "&personnelId=" + personnelId);
        };

        vm.GetDaysInMonth = function (periode, isWeekPeriode) {
            return $http.get("/api/PointagePersonnel/GetDaysInMonth?periode=" + periode + "&isWeekPeriode=" + isWeekPeriode);
        };

        vm.GetOrCreateIndemniteDeplacement = function (pointage) {
            return $http.post("/api/Rapport/GetOrCreateIndemniteDeplacement", pointage);
        };

        vm.RefreshIndemniteDeplacement = function (pointage) {
            return $http.post("/api/Rapport/RefreshIndemniteDeplacement", pointage);
        };

        vm.CheckPointage = function (pointage) {
            return $http.post("/api/PointagePersonnel/CheckPointage", pointage);
        };

        vm.CheckListePointages = function (listPointages) {
            return $http.post("/api/PointagePersonnel/CheckListePointages", listPointages);
        };

        vm.IsGSP = function (ciId) {
            return $http.get("/api/Utilisateur/IsGSP?ciId=" +ciId);
        };

        vm.GetRapportStatutVerrouille = function () {
            return $http.get("/api/PointagePersonnel/GetRapportStatutVerrouille");
        };

        vm.InitPointagePersonnel = function (ciId, year, month, day = 0, personnelId = 0, statut = "") {
            if (personnelId === 0) {
                return $http.post("/api/PointagePersonnel/InitPointagePersonnel?ciId=" + ciId + '&year=' + year + '&month=' + month);
            } else {
                return $http.post("/api/PointagePersonnel/InitPointagePersonnel?ciId=" + ciId + '&year=' + year + '&month=' + month + '&day=' + day + '&personnelId=' + personnelId + '&statut=' + statut);
            }

        };

        vm.GetAstreintesByPersonnelIdAndCiId = function (personnelId, ciId, year, month) {
            return $http.post("/api/PointagePersonnel/GetAstreintesByPersonnelIdAndCiId?personnelId=" + personnelId + "&ciId=" + ciId + '&year=' + year + '&month=' + month);
        };

        vm.GetNewObjectPointagePersonnelModel = function () {
            return $http.get("/api/PointagePersonnel/GetNewObjectPointagePersonnelModel");
        };

        vm.Save = function (listPointages) {
            return $http.post("/api/PointagePersonnel/Save", listPointages);
        };

        vm.GetPersonnel = function (personnelId) {
            return $http.get("/api/Personnel/" + personnelId);
        };

        vm.GetDatesContratInterimaire = function (personnelId, date) {
            return $http.get("/api/ContratInterimaire/GetDatesMax/" + personnelId + "/" + date);
        };

        vm.GetListOfDaysAvailable = function (personnelId, date) {
            return $http.get("/api/ContratInterimaire/GetListOfDaysAvailable/" + personnelId + "/" + date);
        };

        /*
         * @description Redirection vers l'url de téléchargement de l'export des pointages
         */
        vm.GetPointagePersonnelExport = function (personnelId, periode, typeExport) {
            window.location.href = "/api/PointagePersonnel/Export/" + personnelId + "/" + periode + "/" + typeExport;
        };

        this.PostPointageInterimaireExport = function (exportInterimaire) {
            return $http.post("/api/PointagePersonnel/Export/Interimaire", exportInterimaire);
        };

        this.GetPointageInterimaireExport = function (id, typeExport, dateDebut, dateFin) {
            dateDebut = getPeriode(dateDebut);
            dateFin = getPeriode(dateFin);
            window.location.href = "/api/PointagePersonnel/Export/Interimaire/" + id + "/" + typeExport + "/" + dateDebut + "/" + dateFin;
        };

        this.PostPointageHebdomadaireExport = function (exportInterimaire) {
            return $http.post("/api/PointagePersonnel/Export/Hebdomadaire", exportInterimaire);
        };

        this.GetPointageHebdomadaireExport = function (id, typeExport, dateComptable, dateDebut, dateFin) {
            dateComptable = getPeriode(dateComptable);
            dateDebut = getPeriode(dateDebut);
            dateFin = getPeriode(dateFin);
            window.location.href = "/api/PointagePersonnel/Export/Hebdomadaire/" + id + "/" + typeExport + "/" + dateComptable + "/" + dateDebut + "/" + dateFin;
        };

        function getPeriode(date) {
            return $filter('date')(date, 'MM-dd-yyyy');
        }

        vm.duplicate = function (listPointages) {
            return $http.put("/api/PointagePersonnel/Duplicate/", listPointages);
        };

        vm.GetDefaultCi = function (personnelId) {
            return $http.get("/api/Personnel/GetDefaultCi/" + personnelId);
        };

        vm.GetDateEntreeSortie = function (personnelId) {
            return $http.get("/api/Personnel/GetDateEntreeSortie/" + personnelId);
        };

        vm.PostPointageChallengeSecuriteExport = function (exportChallengeSecurite) {
            return $http.post("/api/PointagePersonnel/Export/ChallengeSecurite", exportChallengeSecurite);
        };

        vm.GetPointageChallengeSecuriteExport = function (id, dateComptable, dateDebut, dateFin) {
            dateComptable = getPeriode(dateComptable);
            dateDebut = getPeriode(dateDebut);
            dateFin = getPeriode(dateFin);
            window.location.href = "/api/PointagePersonnel/Export/ChallengeSecurite/" + id  + "/" + dateComptable + "/" + dateDebut + "/" + dateFin;
        };
           angular.extend(vm, resource);
    }
})();

