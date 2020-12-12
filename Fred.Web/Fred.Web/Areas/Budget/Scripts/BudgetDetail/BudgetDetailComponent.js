(function () {
    'use strict';

    angular
        .module('Fred')
        .component('budgetDetailComponent', {
            templateUrl: '/Areas/Budget/Scripts/BudgetDetail/BudgetDetailTemplate.html',
            bindings: {
                resources: '<',
                devises: '<'
            },
            controller: 'budgetDetailComponentController'
        });

    angular.module('Fred').controller('budgetDetailComponentController', budgetDetailComponentController);

    budgetDetailComponentController.$inject = ['$scope', 'ProgressBar', 'BudgetDataService', 'TacheBudgetRecetteService', '$filter', 'TacheBudgetMontantService'];

    function budgetDetailComponentController($scope,
        ProgressBar,
        BudgetDataService,
        TacheBudgetRecetteService,
        $filter,
        TacheBudgetMontantService) {

        var tasks = [];
        var $ctrl = this;

        //////////////////////////////////////////////////////////////////
        // Déclaration des propriétés publiques                         //
        //////////////////////////////////////////////////////////////////
        $ctrl.devisesModels = [];
        $ctrl.DateCreation = '';
        $ctrl.AuteurCreation = '';
        $ctrl.DateModification = '';
        $ctrl.AuteurModification = '';
        $ctrl.Valideur = '';

        //////////////////////////////////////////////////////////////////
        // Déclaration des fonctions publiques                          //
        //////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////
        // Init                                                         //
        //////////////////////////////////////////////////////////////////
        $ctrl.$onInit = function () {

            $scope.$on('budgetCrtl.bugdetInfoChanged', function (event, bugdetInfo) {
                if (bugdetInfo.DateCreation) {
                    $ctrl.DateCreation = $scope.today = $filter('date')(new Date(bugdetInfo.DateCreation), 'dd/MM/yyyy');
                }
                if (bugdetInfo.AuteurCreation) {
                    $ctrl.AuteurCreation = bugdetInfo.AuteurCreation.Personnel.Nom + ' ' + bugdetInfo.AuteurCreation.Personnel.Prenom;
                }
                if (bugdetInfo.DateModification) {
                    $ctrl.DateModification = $scope.today = $filter('date')(new Date(bugdetInfo.DateModification), 'dd/MM/yyyy');
                }
                if (bugdetInfo.AuteurModification) {
                    $ctrl.AuteurModification = bugdetInfo.AuteurModification.Personnel.Nom + ' ' + bugdetInfo.AuteurModification.Personnel.Prenom;
                }
                if (bugdetInfo.AuteurValidation) {
                    $ctrl.Valideur = bugdetInfo.AuteurValidation.Personnel.Nom + ' ' + bugdetInfo.AuteurValidation.Personnel.Prenom;
                }
            });

            $scope.$on('budgetCrtl.tasksLoaded', function (event, tasksLoadedInfo) {
                $ctrl.devisesModels = [];
                tasks = tasksLoadedInfo;
                createDeviseModels();
                calculateMontants(true);
                calculateRecettes(true);
            });

            $scope.$on('budgetCrtl.taskSaved', function (event, tasksLoadedInfo) {
                calculateMontants(true);
                calculateRecettes(true);
            });

            $scope.$on('budgetCrtl.recetteTotalChanged', function (event, info) {
                calculateRecettes(info.isValid);
            });

            $scope.$on('budgetCrtl.quantityChanged', function (event, info) {
                calculateMontants(info.isValid);
            });
        };

        //////////////////////////////////////////////////////////////////
        // Handlers                                                     //
        //////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////
        // Actions                                                      //
        //////////////////////////////////////////////////////////////////
        function createDeviseModels() {
            $ctrl.devisesModels = [];
            for (var i = 0; i < $ctrl.devises.length; i++) {
                var devise = $ctrl.devises[i];
                var deviseModel = {
                    DeviseId: devise.DeviseId,
                    devise: devise,
                    total: null
                };
                $ctrl.devisesModels.push(deviseModel);
            }
        }

        function calculateRecettes(isValid) {
            for (var k = 0; k < $ctrl.devisesModels.length; k++) {
                var deviseModelSelected = $ctrl.devisesModels[k];
                var deviseSelected = deviseModelSelected.devise;
                var recetteTotal = TacheBudgetRecetteService.calculRecetteTotal(tasks, deviseSelected);

                if (isValid) {
                    deviseModelSelected.recette = recetteTotal.recette;
                } else {
                    deviseModelSelected.recette = null;
                }
            }
        }

        function calculateMontants(isValid) {
            var totalsByDevises = TacheBudgetMontantService.calculateTotalsForBudget(tasks, $ctrl.devises);

            for (var k = 0; k < $ctrl.devisesModels.length; k++) {
                var deviseModelSelected = $ctrl.devisesModels[k];
                var deviseSelected = deviseModelSelected.devise;

                if (isValid) {
                    deviseModelSelected.total = totalsByDevises.find(function (t) {
                        return deviseSelected.DeviseId === t.DeviseId;
                    }).total;
                } else {
                    deviseModelSelected.total = null;
                }
            }
        }

    }




})();