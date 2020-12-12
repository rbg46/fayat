(function () {
    'use strict';

    angular.module('Fred').service('PersonnelService', PersonnelService);

    PersonnelService.$inject = ['$http', '$q', '$resource', '$rootScope'];

    function PersonnelService($http, $q, $resource, $rootScope) {
        var vm = this;
        var uriBase = "/api/Personnel/";

        var resource = $resource(uriBase + ':cmd/:personnelId',
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
                GetTypesRattachement: {
                    method: "GET",
                    params: { cmd: "GetTypesRattachement" },
                    isArray: true,
                    cache: false
                },
                Search: {
                    method: "POST",
                    url: uriBase + 'SearchPersonnelWithFilters/:page/:pageSize/:recherche',
                    isArray: false
                },
                SearchOptimized: {
                    method: "POST",
                    url: uriBase + 'SearchPersonnelWithFiltersOptimzed/:page/:pageSize/:recherche',
                    isArray: false
                },
                GetFilter: {
                    method: "GET",
                    params: { cmd: 'Filter' },
                    cache: false
                },
                Get: {
                    method: "GET",
                    params: { cmd: "All" },
                    isArray: true,
                    cache: false
                },
                GetById: {
                    url: uriBase + ':personnelId',
                    method: "GET",
                    params: { personnelId: 0 },
                    cache: false
                },
                GetDefaultCi: {
                    url: uriBase + 'GetDefaultCi/:personnelId',
                    method: "GET",
                    params: { personnelId: 0 },
                    cache: false
                },
                GetByNomPrenom: {
                    url: uriBase + "GetByNomPrenom/" + ':nom/:prenom/:groupeId',
                    method: "GET",
                    params: { nom: "", prenom: "", groupeId: 0 },
                    cache: false
                },
                AddPersonnelAsUtilisateur: {
                    method: "POST",
                    params: { cmd: 'AddPersonnelAsUtilisateur' },
                    cache: false
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
                    url: uriBase + ':personnelId',
                    method: "DELETE",
                    params: { personnelId: 0 }
                },
                GetAffectationInterimaire: {
                    method: "GET",
                    url: uriBase + "AffectationInterimaire/:personnelId/:page/:pageSize",
                    params: { personnelId: 0, page: 1, pageSize: 20 },
                    isArray: true,
                    cache: false
                },
                AddAffectationInterimaire: {
                    method: "POST",
                    params: { cmd: "AffectationInterimaire" }
                },
                UpdateAffectationInterimaire: {
                    method: "PUT",
                    params: { cmd: "AffectationInterimaire" }
                },
                GetBySocieteMatricule: {
                    url: uriBase + "GetBySocieteMatricule/" + ':societeId/:matricule',
                    method: "GET",
                    params: { societeId: 0, matricule: "" },
                    cache: false
                },
                GetPersonnelImage: {
                    url: uriBase + "Image/" + ':personnelId',
                    method: "GET",
                    params: { personnelId: 0 },
                    cache: true
                },
                AddOrUpdatePersonnelImage: {
                    method: "POST",
                    params: { cmd: "Image" }
                }
            }
        );

        /**
         * Récupère un nouveau matricule intérimaire généré par le serveur
         *      
         * @param {any} params paramètre
         * @param {any} callback callback de succès
         * @param {any} errorCallback callback d'erreur
         * @returns {any} le nouveau matricule intérimaire
         */
        vm.GetNextMatriculeInterimaire = function (params) {
            var res = {};
            res.$promise = $http({
                method: 'GET',
                url: uriBase + 'GetNextMatriculeInterimaire',
                cache: false
            }).then(function (response) {
                res.value = response.data;
                return res;
            });
            return res;
        };

        vm.getExportExcel = function (exportexcelModel, haveHabilitation) {
            return $http.post("/api/Personnel/Excel?haveHabilitation=" + haveHabilitation, exportexcelModel);
        };

        vm.downloadExcel = function (exportId) {
            window.location.href = "/api/Personnel/Excel/" + exportId;
        };

        vm.getExportExcelUserHabilitations = function (utilisateurId) {
            return $http.get("/api/Personnel/CreateExcelHabilitationsUtilisateurs/"+ utilisateurId);
        };

        vm.downloadExcelUserHabilitations = function (exportId,utilisateurId) {
            window.location.href = String.format("/api/Personnel/DownloadExcelHabilitationUtilisateurs/{0}/{1}", exportId, utilisateurId);
        };

        vm.hasSubscribeToEmailSummary = function (personnelId) {
            return $http.get("/api/Personnel/HasSubscribeToEmailSummary/" + personnelId);
        };

        vm.activeEmailSummary = function (personnelId) {
            return $http.post("/api/Personnel/ActiveEmailSummary", personnelId);
        };

        vm.disableEmailSummary = function (personnelId) {
            return $http.delete("/api/Personnel/DisableEmailSummary/" + personnelId);
        };

        vm.SearchSocietes = () => $http.get("/api/Societe/GetSocietesGroupesByUserHabibilitation");

        vm.exportReceptionInterim = s => $http.post("/api/Personnel/ExportReceptionInterimaires", s);

        angular.extend(vm, resource);
    }
})();
