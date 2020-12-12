(function () {
    'use strict';
    angular.module('Fred').controller('ValidationAffairesOuvriersController', ValidationAffairesOuvriersController);

    ValidationAffairesOuvriersController.$inject = ['$scope'];

    function ValidationAffairesOuvriersController($scope) {

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                  Initialisation                                                                //
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        var $ctrl = this;
        $ctrl.resources = resources;
        $ctrl.isShowRapport = false;
        initWatchers();

        function initWatchers() {
            $scope.$on('event.show.rapport.hebdo.ci.forResponsableValidation', function (event, cIOuvriersList) {
                displayRapportHebdoOuvrierForSyntheseValidation(cIOuvriersList);
            });

            $scope.$on('event.change.affichage.entree.validation.affaire', function ($event) {
                $event.stopPropagation();
                $ctrl.isShowRapport = false;
                $scope.$broadcast('show.change.affichage.entree.validation.affaire');
            });
        }

        function displayRapportHebdoOuvrierForSyntheseValidation(cIOuvriersList) {
            if (!cIOuvriersList) {
                return;
            }

            $ctrl.isShowRapport = true;
            $scope.$broadcast('show.rapport.hebdo.ci.forResponsableValidation',  cIOuvriersList);
        }
    }
})();