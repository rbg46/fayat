(function () {
    'use strict';

    angular.module('Fred').service('ValidationPointageService', ValidationPointageService);

    ValidationPointageService.$inject = ['$http', '$q', '$resource'];

    function ValidationPointageService($http, $q, $resource) {
        var vm = this;
        var uriBase = "/api/ValidationPointage/";

        var resource = $resource(uriBase + ':cmd/:periode/:lotPointageId/:typeControle',
            {}, //parameters default
            {
                Get: {
                    method: "GET",
                    params: { periode: '01/01/1900' },
                    isArray: true,
                    cache: false
                },
                GetFilter: {
                    method: "GET",
                    params: { cmd: 'Filter', typeControle: 0 }
                },
                GetControlePointageErreurList: {
                    method: "GET",
                    url: uriBase + "ControlePointageErreur/:controlePointageId/:page/:pageSize/:searchText",
                    params: { controlePointageId: 0, page: 1, pageSize: 20, searchText: "" }
                },
                GetRemonteeVracErreurList: {
                    method: "GET",
                    url: uriBase + "RemonteeVracErreur/:remonteeVracId/:page/:pageSize/:searchText",
                    params: { remonteeVracId: 0, page: 1, pageSize: 20, searchText: "" }
                },
                GetRemonteeVrac: {
                    method: "GET",
                    url: uriBase + "GetLastRemonteeVrac/:periode/:utilisateurId",
                    params: { periode: "01/01/1900", utilisateurId: 0 }
                },
                ExecuteControleChantier: {
                    method: "POST",
                    url: uriBase + "ControleChantier/:lotPointageId",
                    params: { lotPointageId: 0 }
                },
                ExecuteControleVrac: {
                    method: "POST",
                    url: uriBase + "ControleVrac/:lotPointageId",
                    params: { lotPointageId: 0 }
                },
                ExecuteVisa: {
                    method: "POST",
                    params: { cmd: "Visa" }
                },
                ExecuteRemonteeVrac: {
                    method: "POST",
                    url: uriBase + "RemonteeVrac/:periode",
                    params: { periode: null }
                },
                ExecuteRemonteePrimes: {
                    method: "POST",
                    url: uriBase + "RemonteePrimes/:periode",
                    params: { periode: null }
                }
            }
        );

        /**
         * Récupère le nombre de pointages non verrouillés
         *      
         * @param {any} params paramètre
         * @param {any} callback callback de succès
         * @param {any} errorCallback callback d'erreur
         * @returns {any} le nouveau matricule intérimaire
         */
        vm.GetAucunVerrouillageCount = function (params) {
            var res = {};
            res.$promise = $http({
                method: 'GET',
                url: uriBase + 'AucunVerrouillageCount/' + params.periode,
                cache: false
            }).then(function (response) {
                res.value = response.data;
                return res;
            });
            return res;
        };

        /*
         * @description Redirige vers l'url d'export des erreurs de contrôle pointage (vrac/chantier) au format PDF
         */
        vm.ExportPdfControlePointageErreur = function (controlePointageId) {
            window.location.href = uriBase + "ControlePointageErreur/Export/" + controlePointageId;
        };

        /*
         * @description Redirige vers l'url d'export des erreurs de remontée vrac au format PDF
         */
        vm.ExportPdfRemonteeVracErreur = function (remonteeVracId) {
            if (!remonteeVracId) {
                remonteeVracId = 0;
            }
            window.location.href = uriBase + "RemonteeVracErreur/Export/" + remonteeVracId;
        };

        vm.VerificationCiSepControleVrac = function(lotPointageId, filter){
            return $http.post("/api/ValidationPointage/ControleVrac/VerificationCiSep/" + lotPointageId, filter)
        }

        vm.VerificationCiSepRemonteeVrac = function(periode, filter){
            return $http.post("/api/ValidationPointage/RemonteeVrac/VerificationCiSep/" + periode, filter)
        }
        angular.extend(vm, resource);
    }

})();