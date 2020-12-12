(function () {
    'use strict';

    angular.module('Fred').service('PointageHedboService', PointageHedboService);

    PointageHedboService.$inject = ['$resource'];

    function PointageHedboService($resource) {
        var uriBase = "api/RapportHebdo/";
        var vm = this;
        var selectedEntree = {ciList : [], ouvrierList: [], isAffichageCi: false};
        var selectedOuvrier;

        // Update de la sélection obtenue des écrans d'entrée : Pour la séléction des Cis
        vm.updateSelectedEntreeCiList = function (items) {
            selectedEntree.isAffichageCi = true;
            var ciList = Array.from(items);
            selectedEntree.ouvrierList = [];
            cloneAll(ciList, selectedEntree.ciList);
        };

        // Update de la séléction obtenue des écrans d'entrée : Pour la séléction des ouvriers
        vm.updateSelectedEntreeOuvrierList = function (items) {
            var ouvrierList = Array.from(items);
            selectedEntree.ouvrierList = [];
            cloneAll(ouvrierList, selectedEntree.ouvrierList);
        };

        // Get the selected entree
        vm.getSelectedEntree = function () {
            var groupedEntry = getGroupOuvrierListByCiId();
            return groupedEntry;
        };

        /**
        * Group the list of workers by CiId
        * @returns {any} List of workers
        */
        function getGroupOuvrierListByCiId() {
            var result = [];
            selectedEntree.ouvrierList.reduce(function (res, value) {
                if (!res[value.CiId]) {
                    res[value.CiId] = {
                        CiId: value.CiId,
                        OuvrierListId: []
                    };
                    result.push(res[value.CiId]);
                }
                res[value.CiId].OuvrierListId.push(value.PersonnelId);
                return res;
            }, {});
            return result;
        }

        /*
         * Séléction par ouvrier 
        */
        vm.setSelectedOuvrierId = function (ouvrierId) {
            selectedOuvrier = ouvrierId;
        };

        /*
         * Séléction par ouvrier 
        */
        vm.getSelectedOuvrierId = function () {
            return selectedOuvrier;
        };

        /**
         * Toggle selected entree selection
         * @param {boolean} isAffichageCi True si on affiche le rapport par CI
         */
        vm.toggleSelectedEntreeSelection = function (isAffichageCi) {
            selectedEntree.isAffichageCi = isAffichageCi;
            selectedEntree.isAffichageCi = !isAffichageCi;
        };

        /*
         * @function cloneAll(source,target)
         * @description Recopie complète d'un objet
         * @param {any} source 
         * @param {any} target
         */
        function cloneAll(source, target) {
            for (var property in source) {
                if (source.hasOwnProperty(property)) {
                    target[property] = angular.copy(source[property]);
                }
            }
        }

        //////////////////////////////////////////////////////////////////
        // HTTP METHODES                                                //
        //////////////////////////////////////////////////////////////////
        var resource = $resource(uriBase,
            {}, //parameters default
            {
                // Retourner la liste des ouvriers et des Cis éligible au pointage par l'utilisateur
                GetUserPointageHebdoSummary: {
                    method: "GET",
                    url: "/api/RapportHebdo/GetUserPointageHebdoSummary/:personnelStatut/:mondayDate",
                    params: {},
                    isArray: false,
                    cache: false
                },
                GetRapportHebdoByCi: {
                    method: "POST",
                    url: "/api/RapportHebdo/GetRapportHebdoByCi",
                    params: {},
                    isArray: true
                },
                GetRapportHebdoByWorker: {
                    method: "GET",
                    url: "/api/RapportHebdo/GetRapportHebdoByWorker/:personnelId/:mondayDate/:allCi",
                    params: {},
                    isArray: true
                },
                SaveRapportHebdo: {
                    method: "POST",
                    url: "/api/RapportHebdo/SaveRapportHebdo",
                    params: {},
                    isArray: false
                },
                CheckAndValidateRapportHebdo: {
                    method: "POST",
                    url: "/api/RapportHebdo/CheckAndValidateRapportHebdo/:isEtamIac",
                    params: { isEtamIac: false},
                    isArray: false,
                    cache: false   
                },
                GetSyntheseMensuelleEtamIac: {
                    method: "GET",
                    url: "/api/RapportHebdo/GetSyntheseMensuelleEtamIac/:utilisateurId/:monthDate",
                    params: {},
                    isArray: true
                },
                ValiderSyntheseMensuelleEtamIac: {
                    method: "POST",
                    url: "/api/RapportHebdo/ValiderSyntheseMensuelleEtamIac",
                    params: {},
                    isArray: false
                },
                GetPersonnelGroupebyId: {
                    method: "GET",
                    url: "/api/Personnel/GetPersonnelGroupebyId/:personnelId",
                    params: { personnelId: 0 },
                    isArray: false
                },
                PrimePersonnelAffected: {
                    method: "GET",
                    url: "/api/RapportHebdo/PrimePersonnelAffected",
                    params: {},
                    isArray: true
                },
                GetPointageByPersonnelIDAndInterval: {
                    method: "GET",
                    url: "/api/RapportHebdo/GetPointageByPersonnelIDAndInterval",
                    params: {},
                    isArray: true
                },
                MajorationPersonnelAffected: {
                    method: "GET",
                    url: "/api/Rapport/MajorationPersonnelAffected",
                    params: {},
                    isArray: true
                },
                GetRapportLigneStatutForNewPointage: {
                    method: "GET",
                    url: "/api/RapportHebdo/GetRapportLigneStatutForNewPointage/:personnelId/:ciId/:mondayDate",
                    params: {},
                    isArray: true
                },
                GetSortie: {
                    method: "GET",
                    url: "/api/RapportHebdo/GetSortie/:personnelId/:ciId/:mondayDate",
                    params: {},
                    isArray: false
                },
                GetValidationAffairesbyResponsableAsync: {
                    method: "GET",
                    url: "/api/RapportHebdo/GetValidationAffairesByResponsableAsync/:dateDebut",
                    params: {},
                    isArray: true
                },
                ValidateAffairesbyResponsableAsync: {
                    method: "GET",
                    url: "/api/RapportHebdo/ValidateAffairesByResponsableAsync",
                    params: {},
                    isArray: true
                }
            }
        );

    angular.extend(vm, resource);
  }
})();