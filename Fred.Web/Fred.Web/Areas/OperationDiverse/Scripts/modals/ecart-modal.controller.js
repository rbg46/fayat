(function (angular) {
    'use strict';

    angular.module('Fred').controller('EcartModalController', EcartModalController);

    EcartModalController.$inject = ['$scope', '$uibModalInstance', 'authorizationService', 'DatesClotureComptableService', 'CloturesPeriodesService', 'ProgressBar', 'StringFormat'];

    function EcartModalController($scope, $uibModalInstance, authorizationService, DatesClotureComptableService, CloturesPeriodesService, ProgressBar, StringFormat) {

        var $ctrl = this;

        angular.extend($ctrl, {
            resources: resources,
            handleValider: handleValider,
            handleCancel: handleCancel,
            canClosePeriode: canClosePeriode
        });

        $ctrl.listAuto = [];
        $ctrl.listManuel = [];
        $ctrl.listPeriodClosed = $scope.$resolve.listPeriodClosed;
        $ctrl.ciSelected = $scope.$resolve.ciSelected;
        $ctrl.message = $scope.$resolve.message;

        $ctrl.periodeDebut = moment($scope.$resolve.periodeDebut).format('MM/YYYY');
        $ctrl.periodeFin = moment($scope.$resolve.periodeFin).format('MM/YYYY');

        $ctrl.monthNames = ["", "Janvier", "Février", "Mars", "Avril", "Mai", "Juin", "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre"];

        if (isValidPeriodeDebut()) {
            $ctrl.EcartModal_GapOnPeriod = StringFormat.Format($ctrl.resources.EcartModal_GapOnPeriod, $ctrl.periodeDebut, $ctrl.periodeFin);
        } else {
            $ctrl.EcartModal_GapOnPeriod = StringFormat.Format($ctrl.resources.EcartModal_GapOnPeriodCumulee, $ctrl.periodeFin);
        }

        function isValidPeriodeDebut() { 
            // Vu le comportement non maitrisé de "moment", le test est fait aussi sur $ctrl.periodeDebut pour blinder le code
            return $scope.$resolve.periodeDebut && $ctrl.periodeDebut;
        }

        function handleCancel() {
            $uibModalInstance.dismiss("cancel");
        }

        function canClosePeriode() {
            var permission = authorizationService.getPermission('button.enabled.close.period.index');
            var isAffected = authorizationService.getRights('button.enabled.close.period.index');
            if (permission !== null && isAffected.isVisible) {
                return true;
            }
            return false;
        }

        function handleValider() {

            ProgressBar.start();
            CloturesPeriodesService.getFromList($ctrl.listPeriodClosed.filter(item => item.Option === 'odauto')).then((response) => {
                var model = {};
                angular.forEach(response.data, function (cloture) {
                    model = {
                        DatesClotureComptableId: cloture.DatesClotureComptableId,
                        CiId: $ctrl.ciSelected.CiId,
                        Annee: cloture.Annee,
                        Mois: cloture.Mois,
                        DateArretSaisie: cloture.DateArretSaisie,
                        DateTransfertFar: cloture.DateTransfertFAR,
                        DateCloture: moment(new Date()).format('YYYY-MM-DD'),
                        Historique: cloture.Historique,
                        AuteurSap: cloture.AuteurSap,
                        AuteurCreationId: cloture.AuteurCreationId,
                        DateCreation: cloture.DateCreation,
                        AuteurModificationId: cloture.AuteurModificationId,
                        DateModification: moment(new Date()).format('YYYY-MM-DD')
                    };
                    $ctrl.listAuto.push(model);
                });
            })
                .then(processOdAuto)
                .then(processOdManuel)
                .then(function () { $uibModalInstance.dismiss("cancel"); })
                .finally(function () { ProgressBar.complete(); });

        }

        function processOdAuto() {
            CloturesPeriodesService.OpenAndCloseDatesClotureComptableFromList($ctrl.listAuto);
        }

        function processOdManuel() {
            DatesClotureComptableService.OpenPeriode($ctrl.ciSelected.CiId, $ctrl.listPeriodClosed.filter(item => item.Option !== "odauto").map(x => moment(new Date(x.Annee, x.Mois - 1, 15)).format('YYYY-MM-DD')));
        }

    }
})(angular);
