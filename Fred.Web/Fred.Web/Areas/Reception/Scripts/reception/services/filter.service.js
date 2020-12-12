
(function () {
    'use strict';

    angular.module('Fred').service('FilterService', FilterService);

    FilterService.$inject = [];

    function FilterService() {

        this.getIfOneFilterIsSelected = function getIfOneFilterIsSelected(filter) {
            if (!filter) {
                return false;
            }
            if (filter.AgenceId !== null) {
                return true;
            }
            if (filter.AuteurCreationId !== null) {
                return true;
            }
            if (filter.CiId !== null) {
                return true;
            }
            if (filter.DateFrom) {
                return true;
            }
            if (filter.DateTo) {
                return true;
            }
            if (filter.FournisseurId !== null) {
                return true;
            }
            if (filter.IsAbonnement !== false) {
                return true;
            }
            if (filter.IsEnergie !== false) {
                return true;
            }
            if (filter.IsMaterielAPointer !== false) {
                return true;
            }
            if (filter.IsSoldee !== false) {
                return true;
            }
            if (filter.IsMaterielAPointer !== false) {
                return true;
            }
            if (filter.IsSoldee !== false) {
                return true;
            }
            if (filter.OnlyCommandeWithAtLeastOneCommandeLigneLocked !== false) {
                return true;
            }
            if (filter.RessourceId !== null) {
                return true;
            }
            if (filter.TacheId !== null) {
                return true;
            }
            if (filter.TypeCodes.length !== 0) {
                return true;
            }
            if (filter.ValueText !== "") {
                return true;
            }
            return false;
        };

        this.GetInitialFilter = function GetInitialFilter() {
            return {
                "AgenceId": null,
                "Agence": null,
                "AuteurCreationId": null,
                "AuteurCreation": null,
                "CiId": null,
                "CI": null,
                "DateFrom": null,
                "DateTo": null,
                "FournisseurId": null,
                "Fournisseur": null,
                "IsAbonnement": false,
                "IsEnergie": false,
                "IsMaterielAPointer": false,
                "IsSoldee": false,
                "OnlyCommandeWithAtLeastOneCommandeLigneLocked": false,
                "RessourceId": null,
                "Ressource": null,
                "TacheId": null,
                "Tache": null,
                "TypeCodes": [],
                "ValueText": ""
            };
        };
    }
})();
