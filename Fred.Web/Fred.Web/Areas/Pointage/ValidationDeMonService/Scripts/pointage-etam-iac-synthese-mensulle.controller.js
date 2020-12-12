(function (angular) {
    'use strict';

    angular.module('Fred').controller('PointageEtamIacSyntheseMensuelleController', PointageEtamIacSyntheseMensuelleController);

    PointageEtamIacSyntheseMensuelleController.$inject = ['$scope', 'PointageHedboService'];

    function PointageEtamIacSyntheseMensuelleController($scope, PointageHedboService) {
        var $ctrl = this;
        $ctrl.isShowRapport = false;
        $ctrl.personnelToShowIsIac = false;
        $ctrl.resources = resources;
        initWatchers();

        /*
         * @function inti watchers
         * @description intialisation des watchers
         */
        function initWatchers() {
            $scope.$on('event.change.save.state', function ($event, isSaveActif) {
                $event.stopPropagation();
                $scope.$broadcast('event.change.save.state', isSaveActif);
            });

            $scope.$on('event.change.validate.state', function ($event, isSaveActif) {
                $event.stopPropagation();
                $scope.$broadcast('event.change.validate.state', isSaveActif);
            });

            $scope.$on('event.change.totals', function ($event, obj) {
                $scope.$broadcast('event.change.totals.refresh', { item: obj });
            });

            $scope.$on('event.validation.mensuelle.personnel.selected.entree', function ($event, obj) {
                $event.stopPropagation();
                actionRefreshPersonnelSelection(obj);
            });

            $scope.$on('event.change.affichage.entree', function ($event, selectedCi) {
                $event.stopPropagation();
                $ctrl.isShowRapport = false;
                $scope.$broadcast('event.validation.mensuelle.personnel.selected.entree.refresh');
            });
        }


        //====================================================================================
        //==== Handlers
        //===================================================================================

        /*
         * @function handlePersonnelSelection(personnelId)
         * @description Handle de la séléction du personnel
         * @param {any} personnel id
         */
        function actionRefreshPersonnelSelection(obj) {
            if (!obj) {
                return;
            }
            PointageHedboService.setSelectedOuvrierId(obj.Personnel.PersonnelId);
            checkStatut(obj);
            $ctrl.isShowRapport = true;
            $scope.$broadcast('show.rapport.hebdo.ci.forValidationService', { date: obj.FirstMonthDate, isAffichageParOuvrier: true });
        }

        function checkStatut(obj) {
            if (obj.Personnel.Statut === 2) {
                $ctrl.personnelToShowIsIac = true;
            }
            else {
                $ctrl.personnelToShowIsIac = false;
            }
        }
    }
}(angular));