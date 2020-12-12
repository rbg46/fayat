(function () {
    'use strict';

    var pointageHebdoEntreeParAffaireComponent = {
        templateUrl: '/Areas/Pointage/RapportHebdo/Scripts/Components/pointage-hebdo-entree-par-affaire.component.html',
        bindings: {
            resources: '<',
            ciList: '='
        },
        controller: PointageHebdoEntreeParAffaireController,
        controllerAs: '$ctrl'
    };

    angular.module('Fred').component('pointageHebdoEntreeParAffaireComponent', pointageHebdoEntreeParAffaireComponent);

    angular.module('Fred').controller('PointageHebdoEntreeParAffaireController', PointageHebdoEntreeParAffaireController);

    PointageHebdoEntreeParAffaireController.$inject = ['$scope', 'PointageHedboService'];

    function PointageHebdoEntreeParAffaireController($scope, PointageHedboService) {
        var $ctrl = this;
        $ctrl.selectedCi = [];

        //////////////////////////////////////////////////////////////////
        // Handlers                                                     //
        //////////////////////////////////////////////////////////////////

        /*
         * Handle select all ci 
        */
        $ctrl.selectAllCi = function (change) {
            angular.forEach($ctrl.ciList, function (val) {
                val.Selected = change;
            });

            $ctrl.selectedCi = $ctrl.ciList
                .filter(function (a) { return a.Selected; })
                .map(function (ev) { return ev.CiId; });
            $scope.$emit('event.change.ci', $ctrl.selectedCi);
            PointageHedboService.updateSelectedEntreeCiList($ctrl.selectedCi);
        };

        /*
         * Handle select a ci 
        */
        $ctrl.toggleCiSelection = function (ciChanged) {
            if (ciChanged.checkState) {
                $ctrl.selectedCi.push(ciChanged.CiId);
            } else {
                $ctrl.selectedCi = $ctrl.selectedCi.filter(function (i) {
                    return i !== ciChanged.CiId;
                });
            }
            selectOrUnSelectGlobalCiSelectionButton();
            $scope.$emit('event.change.ci', $ctrl.selectedCi);
            PointageHedboService.updateSelectedEntreeCiList($ctrl.selectedCi);
        };

        function selectOrUnSelectGlobalCiSelectionButton() {
            if (allCisAreSelected()) {
                $ctrl.SelectAllCi = true;
            } else {
                $ctrl.SelectAllCi = false;
            }
        }

        function allCisAreSelected() {
            return $ctrl.selectedCi.length === $ctrl.ciList.length;
        }

        // Watcher pour détécter le changement du mode d'affichage
        $scope.$on('event.change.mode', function (evt, data) {
            $ctrl.selectedCi = [];
            $ctrl.SelectAllCi = false;
            angular.forEach($ctrl.ciList, function (val) {
                val.Selected = false;
            });
        });
    }
})();
