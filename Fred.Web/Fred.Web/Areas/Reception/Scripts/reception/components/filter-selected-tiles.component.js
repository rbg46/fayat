(function () {
    'use strict';
    var filterSelectedTilesComponent = {
        templateUrl: '/Areas/Reception/Scripts/reception/components/filter-selected-tiles.tpl.html',
        bindings: {
            filter: '='
        },
        controller: filterSelectedTilesController
    };

    angular.module('Fred').component('filterSelectedTilesComponent', filterSelectedTilesComponent);

    filterSelectedTilesController.$inject = ['FilterService'];

    function filterSelectedTilesController(FilterService) {

        var $ctrl = this;
        $ctrl.resources = resources;
        //////////////////////////////////////////////////////////////////
        // Initialisation                                               //
        //////////////////////////////////////////////////////////////////
        $ctrl.$onInit = function () {
            $ctrl.typeCodes = { fourniture: 'F', prestation: 'P', location: 'L' };
        };

        //////////////////////////////////////////////////////////////////
        // Chargement                                                   //
        //////////////////////////////////////////////////////////////////
        $ctrl.deleteAgence = function deleteDateFrom(filter) {
            filter.AgenceId = null;
            filter.Agence = null;
        };

        $ctrl.deleteFournisseur = function deleteFournisseur(filter) {
            filter.Fournisseur = null;
            filter.FournisseurId = null;
        };

        $ctrl.deleteCI = function deleteCI(filter) {
            filter.CI = null;
            filter.CiId = null;
            filter.RessourceId = null;
            filter.Ressource = null;
            filter.TacheId = null;
            filter.Tache = null;
        };

        $ctrl.deleteRessource = function deleteRessource(filter) {
            filter.Ressource = null;
            filter.RessourceId = null;
        };

        $ctrl.deleteTache = function deleteRessource(filter) {
            filter.TacheId = null;
            filter.Tache = null;
        };

        $ctrl.deleteDateTo = function deleteTache(filter) {
            filter.DateTo = null;
        };

        $ctrl.deleteDateFrom = function deleteDateFrom(filter) {
            filter.DateFrom = null;
        };

        $ctrl.deleteAuteurCreation = function deleteAuteurCreation(filter) {
            filter.AuteurCreation = null;
            filter.AuteurCreationId = null;
        };

        $ctrl.deleteFouniture = function deleteFouniture(filter) {
            var i = filter.TypeCodes.indexOf($ctrl.typeCodes.fourniture);
            if (i > -1) {
                filter.TypeCodes.splice(i, 1);
            }
        };

        $ctrl.deletePrestation = function deletePrestation(filter) {
            var i = filter.TypeCodes.indexOf($ctrl.typeCodes.prestation);
            if (i > -1) {
                filter.TypeCodes.splice(i, 1);
            }
        };

        $ctrl.deleteLocation = function deleteLocation(filter) {
            var i = filter.TypeCodes.indexOf($ctrl.typeCodes.location);
            if (i > -1) {
                filter.TypeCodes.splice(i, 1);
            }
        };

        $ctrl.deleteIsAbonnement = function deleteIsAbonnement(filter) {
            filter.IsAbonnement = false;
        };
        $ctrl.deleteIsMaterielAPointer = function deleteIsMaterielAPointer(filter) {
            filter.IsMaterielAPointer = false;
        };
        $ctrl.deleteIsEnergie = function deleteIsEnergie(filter) {
            filter.IsEnergie = false;
        };
        $ctrl.deleteIsSoldee = function deleteIsEnergie(filter) {
            filter.IsSoldee = false;
        };
        $ctrl.deleteOnlyCommandeWithAtLeastOneCommandeLigneLocked = function deleteIsEnergie(filter) {
            filter.OnlyCommandeWithAtLeastOneCommandeLigneLocked = false;
        };

    }
})();
