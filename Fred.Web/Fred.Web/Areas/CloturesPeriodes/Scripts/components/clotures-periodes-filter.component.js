(function (angular) {
    'use strict';

    var CloturesPeriodesFilterComponent = {
        templateUrl: '/Areas/CloturesPeriodes/Scripts/components/clotures-periodes-filter.component.html',
        bindings: {
            resources: '<',
            childFilter: '='
        },
        controller: CloturesPeriodesFilterController, 
        controllerAs: 'filterCtrl'
    };

    angular.module('Fred').component('cloturesPeriodesFilterComponent', CloturesPeriodesFilterComponent);

    function CloturesPeriodesFilterController() {
        var filterCtrl = this;

        filterCtrl.FredToolBox = FredToolBox;
        filterCtrl.resources = resources;

        filterCtrl.reinitTransfertFar = function () {
            filterCtrl.childFilter.transfertFar = null;
        };
        filterCtrl.reinitClotureDepenses = function () {
            filterCtrl.childFilter.clotureDepenses = null;
        };
        filterCtrl.reinitValidationAvancement = function () {
            filterCtrl.childFilter.validationAvancement = null;
        };
        filterCtrl.reinitValidationControleBudgetaire = function () {
            filterCtrl.childFilter.validationControleBudgetaire = null;
        };
        filterCtrl.reinitClotureSurLaPeriode = function () {
            filterCtrl.childFilter.clotureSurLaPeriode = null;
        };
        filterCtrl.reinitDejaTermine = function () {
            filterCtrl.childFilter.dejaTermine = false;
        };
        filterCtrl.getTransfertFar = function () {
            return angular.fromJson(filterCtrl.childFilter.transfertFar);
        };
        filterCtrl.getClotureDepenses = function () {
            return angular.fromJson(filterCtrl.childFilter.clotureDepenses);
        };
        filterCtrl.getValidationAvancement = function () {
            return angular.fromJson(filterCtrl.childFilter.validationAvancement);
        };
        filterCtrl.getValidationControleBudgetaire = function () {
            return angular.fromJson(filterCtrl.childFilter.validationControleBudgetaire);
        };
        filterCtrl.getClotureSurLaPeriode = function () {
            return angular.fromJson(filterCtrl.childFilter.clotureSurLaPeriode);
        };

        init();

        return filterCtrl;

        /**
         * Initialisation du controller.     
         */
        function init() {
        }
    }
}(angular));
