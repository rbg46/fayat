(function (angular) {
    'use strict';

    angular.module('Fred').service('SocieteService', SocieteService);

    SocieteService.$inject = ['$http', '$q', '$resource'];

    function SocieteService($http, $q, $resource) {
        var vm = this;
        var uriBase = "/api/Societe/";

        var resource = $resource(uriBase + ':cmd/:societeId/:organisationId/:deviseId/:imageId',
            {}, //parameters default
            {
                New: {
                    method: "GET",
                    params: { cmd: 'New' },
                    cache: false
                },
                Search: {
                    method: "POST",
                    url: uriBase + 'SearchAll/:page/:pageSize/',
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
                GetById: {
                    method: "GET",
                    params: { cmd: 'GetSocieteById', societeId: 0 },
                    cache: false
                },
                Update: {
                    method: "PUT",
                    params: {}
                },
                Create: {
                    method: "POST",
                    params: {}
                },
                Delete: {
                    method: "DELETE",
                    params: { societeId: 0 }
                },
                GetSocieteDeviseList: {
                    method: "GET",
                    params: { cmd: "GetSocieteDeviseList", societeId: 0 },
                    isArray: true,
                    cache: false
                },
                ManageSocieteDeviseList: {
                    method: "POST",
                    params: { cmd: "ManageSocieteDeviseList", societeId: 0 },
                    isArray: true,
                    cache: false
                },
                GetSocieteUniteList: {
                    method: "GET",
                    params: { cmd: "GetSocieteUniteList", societeId: 0 },
                    isArray: true,
                    cache: false
                },
                ManageSocieteUniteList: {
                    method: "POST",
                    params: { cmd: "ManageSocieteUniteList", societeId: 0 },
                    isArray: true,
                    cache: false
                },
                GetJournalList: {
                    method: "GET",
                    params: { cmd: "GetJournalList", societeId: 0 },
                    isArray: true,
                    cache: false
                },
                ManageJournalList: {
                    method: "POST",
                    params: { cmd: "ManageJournalList", societeId: 0 },
                    isArray: true,
                    cache: false
                },
                GetLoginImage: {
                    method: "GET",
                    url: uriBase + ':societeId/GetLoginImage',
                    isArray: false,
                    cache: false
                },
                GetLogoImage: {
                    method: "GET",
                    url: uriBase + ':societeId/GetLogoImage',
                    isArray: false,
                    cache: false
                },
                GetLoginImages: {
                    method: "GET",
                    params: { cmd: "LoginImages" },
                    isArray: true,
                    cache: false
                },
                GetLogoImages: {
                    method: "GET",
                    params: { cmd: "LogoImages" },
                    isArray: true,
                    cache: false
                },
                UpdateLoginImage: {
                    method: "PUT",
                    url: uriBase + ':societeId/UpdateLoginImage/:imageId',
                    cache: false
                },
                UpdateLogoImage: {
                    method: "PUT",
                    url: uriBase + ':societeId/UpdateLogoImage/:imageId',
                    cache: false
                },
                GetIndemniteDeplacementCalculTypes: {
                    method: "GET",
                    params: { cmd: "GetIndemniteDeplacementCalculTypes" },
                    isArray: true,
                    cache: false
                }
            }
        );

        /**
         * Vérifie si un Code société n'existe pas déjà en base de données
         *      
         * @param {any} params paramètre
         * @param {any} callback callback de succès
         * @param {any} errorCallback callback d'erreur
         * @returns {any} le nouveau code
         */
        vm.IsCodeSocieteExist = function (params) {
            var res = {};
            res.$promise = $http({
                method: 'GET',
                url: uriBase + 'CheckSocieteExists/' + params.codeSociete + '/' + params.libelle + '/' + params.societeId,
                cache: false
            }).then(function (response) {
                res.value = response.data;
                return res;
            });
            return res;
        };

        /********************** Service pour Associés SEP ***********************/

        vm.GetAllAssocieSep = function (societeId) {
            return $http.get(`/api/Societe/${societeId}/AssocieSep`);
        }

        vm.CreateOrUpdateAssocieSepRange = function (societeId, associeSeps) {
            return $http.post(`/api/Societe/${societeId}/AssocieSep`, associeSeps);
        }

        vm.DeleteAssocieSepRange = function (societeId, associeSepIds) {
            return $http.delete(`/api/Societe/${societeId}/AssocieSep?associeSepIds=${associeSepIds}`);
        }
        
        this.GetDefaultSocieteInterim = function () {
            return $http.get("/api/Societe/GetDefaultSocieteInterim");
        };
    

        angular.extend(vm, resource);
    }

})(angular);