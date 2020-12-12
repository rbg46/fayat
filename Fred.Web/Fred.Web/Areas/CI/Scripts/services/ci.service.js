(function () {
    'use strict';

    angular.module('Fred').service('CIService', CIService);

    CIService.$inject = ['$http', '$q', '$resource'];

    function CIService($http, $q, $resource) {
        var vm = this;
        var uriBase = "/api/CI/";

        var resource = $resource(uriBase + ':cmd/:ciId/:societeId/:organisationId/:deviseId',
            {}, //parameters default
            {
                Search: {
                    method: "POST",
                    url: uriBase + 'SearchWithFilters/:page/:pageSize/:recherche',
                    isArray: true
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
                GetCIListByOrganisationIdAndPeriod: {
                    method: "GET",
                    url: uriBase + "GetCIListByOrganisationIdAndPeriod/:organisationId/:period/:page/:pageSize",
                    params: { organisationId: 0, period: 0, page: 1, pageSize: 10 },
                    isArray: true,
                    cache: false
                },
                GetById: {
                    url: uriBase + ':ciId',
                    method: "GET",
                    params: { ciId: 0 },
                    cache: false
                },
                GetByOrganisationId: {
                    method: "GET",
                    params: { cmd: 'Organisation', organisationId: 0 },
                    cache: false
                },
                Update: {
                    method: "PUT",
                    params: {}
                },
                GetDeviseRef: {
                    method: "GET",
                    params: { cmd: "DeviseRef", ciId: 0 },
                    cache: false
                },
                GetDeviseSecList: {
                    method: "GET",
                    params: { cmd: "DeviseSec" },
                    isArray: true,
                    cache: false
                },
                GetCIDeviseList: {
                    method: "GET",
                    params: { cmd: "Devise", ciId: 0 },
                    isArray: true,
                    cache: false
                },
                // TODO : à modifier
                GetCICodePrimeList: {
                    url: '/api/Prime/GetPrimesListForCI/:ciId',
                    method: "GET",
                    params: { ciId: 0 },
                    isArray: true,
                    cache: false
                },
                // TODO : à modifier
                GetCICodeMajorationList: {
                    url: '/api/CodeMajoration/ListActifsCodesMajorationBySocieteId/:groupeId/:ciId',
                    method: "GET",
                    params: { groupeId: 0, ciId: 0 },
                    isArray: true,
                    cache: false
                },
                GetCIRessourceList: {
                    method: "GET",
                    params: { cmd: "Ressource", ciId: 0, societeId: 0 },
                    isArray: true,
                    cache: false
                },
                GetParametrageCarburantList: {
                    method: "GET",
                    params: { cmd: "ParametrageCarburant", organisationId: 0, deviseId: 0 },
                    isArray: true,
                    cache: false
                },
                // Ajout ou modification d'une association CI/Devise
                ManageCIDevise: {
                    method: "POST",
                    params: { cmd: "ManageCIDevise", ciId: 0 },
                    isArray: true,
                    cache: false
                },
                // Ajout ou modification d'une association CI/Code Majoration
                ManageCICodeMajoration: {
                    method: "POST",
                    url: '/api/CodeMajoration/ManageCICodeMajoration/:ciId',
                    params: { ciId: 0 },
                    isArray: true,
                    cache: false
                },
                // Ajout ou modification d'une association CI/Prime
                ManageCIPrime: {
                    method: "POST",
                    url: '/api/Prime/ManageCIPrime/:ciId',
                    params: { ciId: 0 },
                    isArray: true,
                    cache: false
                },
                // Ajout ou modification d'une association CI/Ressource
                ManageCIRessource: {
                    method: "POST",
                    params: { cmd: "ManageCIRessource" },
                    isArray: true,
                    cache: false
                },
                // Ajout ou modification des paramétrages carburant (prix)
                ManageParametrageCarburant: {
                    method: "POST",
                    params: { cmd: "ManageParametrageCarburant" },
                    isArray: true,
                    cache: false
                }
            }
        );

        vm.IsRolePaie = function (ciId) {
            var res = {};
            res.$promise = $http({
                method: 'GET',
                url: '/api/Utilisateur/IsRolePaie/' + ciId,
                cache: false
            }).then(function (response) {
                res.value = response.data;
                return res;
            });
            return res;
        };

        angular.extend(vm, resource);
    }
})();