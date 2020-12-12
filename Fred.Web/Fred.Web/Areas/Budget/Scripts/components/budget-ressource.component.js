(function () {
    'use strict';

    angular
        .module('Fred')
        .component('budgetRessourceComponent', {
            templateUrl: '/Areas/Budget/Scripts/components/budget-ressource.component.html',
            bindings: {
                resources: '<',
                budgetRessource: '<',
                chapitre: '<',
                sousChapitre: '<',
                deviseSelected: '<',
                budget: '<',
                searched: '<',
                bibliothequePrix: '<'

            },
            controller: 'budgetRessourceComponentController'
        });

    angular.module('Fred').controller('budgetRessourceComponentController', budgetRessourceComponentController);
    budgetRessourceComponentController.$inject = ['$scope', '$uibModal', '$log', '$timeout', 'ParametrageReferentielEtenduService', 'BudgetService'];

    function budgetRessourceComponentController($scope, $uibModal, $log, $timeout, ParametrageReferentielEtenduService, BudgetService) {

        var $ctrl = this;

        $ctrl.upperCaseArrayedSearch = [$ctrl.searched.toUpperCase()];

        $ctrl.isBusy = false;

        // permet de savoir si les enfants sont visible ou pas
        $ctrl.isOpen = true;

        //////////////////////////////////////////////////////////////////
        // Déclaration des fonctions publiques                          //
        //////////////////////////////////////////////////////////////////


        $ctrl.handleClickCreateNew = handleClickCreateNew;
        $ctrl.handleClickAddToTask = handleClickAddToTask;
        $ctrl.handleClickHeader = handleClickHeader;
        $ctrl.handleClickEditChild = handleClickEditChild;
        $ctrl.hasAnyBibliothequePrix = hasAnyBibliothequePrix;

        //////////////////////////////////////////////////////////////////
        // Init                                                         //
        //////////////////////////////////////////////////////////////////

        $ctrl.$onInit = function () {
            //$scope.$on('budgetCrtl.deviseChanged', function (event, deviseSelected) {
            //  onDeviseChanged(deviseSelected);
            //});
            //$scope.$on('budgetCrtl.ressourceChangedOnDetail', function (event, updatedRessource) {
            //  onRessourceChangedOnDetail(updatedRessource);
            //});
            manageAllDataAndVisibilityForDevise($ctrl.budgetRessource, $ctrl.deviseSelected);
        };

        //////////////////////////////////////////////////////////////////
        // Evenements                                                   //
        //////////////////////////////////////////////////////////////////


        function manageAllDataAndVisibilityForDevise(ressource, deviseSelected) {
            manageAllDataAndVisibilityForDeviseAndRessource(ressource, deviseSelected);

            if (ressource.RessourcesEnfants && ressource.RessourcesEnfants.length > 0) {
                for (var i = 0; i < ressource.RessourcesEnfants.length; i++) {
                    var child = ressource.RessourcesEnfants[i];
                    manageAllDataAndVisibilityForDeviseAndRessource(child, deviseSelected);
                }
            }

        }

        function manageAllDataAndVisibilityForDeviseAndRessource(ressource, deviseSelected) {

            if (ressource.BibliothequePrixMontant !== null) {
                ressource.symbole = deviseSelected.Symbole;
            } else {
                ressource.symbole = null;
            }
        }



        //////////////////////////////////////////////////////////////////
        // Handlers                                                     //
        //////////////////////////////////////////////////////////////////

        function handleClickHeader() {
            $ctrl.isOpen = !$ctrl.isOpen;
        }

        function handleClickCreateNew(ressourceParent) {
            $ctrl.isBusy = true;
            var modalInstance = openModal(false, ressourceParent, ressourceParent);

            modalInstance.result.then(function (newRessource) {
                ressourceParent.RessourcesEnfants.push(newRessource);
                manageAllDataAndVisibilityForDevise(newRessource, $ctrl.deviseSelected);
            }).finally(function () {
                $ctrl.isBusy = false;
            });
        }

        function handleClickAddToTask(newRessource) {
            $scope.$emit(BudgetService.Events.PanelRessourceAdded, { Chapitre: $ctrl.chapitre, SousChapitre: $ctrl.sousChapitre, Ressource: newRessource });
        }



        function handleClickEditChild(ressourceParent, existingRessource) {

            var modalInstance = openModal(true, existingRessource, ressourceParent);


            modalInstance.result.then(function (updatedRessource) {
                // Lorsque la mise a jour a reussit, nous remplacons la ressource de la liste par la ressource retournée du serveur. 
                var index = ressourceParent.RessourcesEnfants.map(function (e) { return e.RessourceId; }).indexOf(updatedRessource.RessourceId);
                if (index !== -1) {
                    ressourceParent.RessourcesEnfants[index] = updatedRessource;
                    manageAllDataAndVisibilityForDevise(updatedRessource, $ctrl.deviseSelected);
                    //$scope.$emit('budgetRessourceComponent.ressourceModifiedOnRessourceView', updatedRessource)
                } else {
                    // Normalement impossible, car la ressource mise a jour existe forcément.             
                    $log.warn("Une erreur est survenue lors de la mise a jour de la ressource.(budgetRessourceComponent)");
                }
            });
        }



        //////////////////////////////////////////////////////////////////
        // Actions                                                      //
        //////////////////////////////////////////////////////////////////

        /*
         * ouvre une popup pour creer ou mettre à jour une ressource
         */
        function openModal(isEditMode, ressource, ressourceParent) {
            var modalInstance = $uibModal.open({
                animation: $ctrl.animationsEnabled,
                component: 'budgetRessourceCreateComponent',
                appendTo: $("#SOUS_DETAIL_RESSOURCE_PANEL_MODAL"),    // Permet à la fenêtre de s'afficher au dessus, sinon on ne pourra rien modifier dedans.
                resolve: {
                    chapitre: function () {
                        return $ctrl.chapitre;
                    },
                    sousChapitre: function () {
                        return $ctrl.sousChapitre;
                    },
                    ressource: function () {
                        return ressource;
                    },
                    ressourceParent: function () {
                        return ressourceParent;
                    },
                    resources: function () {
                        return $ctrl.resources;
                    },
                    isEditMode: function () {
                        return isEditMode;
                    },
                    deviseSelected: function () {
                        return $ctrl.deviseSelected;
                    },
                    budget: function () {
                        return $ctrl.budget;
                    }
                }
            });

            return modalInstance;
        }

        function hasAnyBibliothequePrix(ressource) {
            let bibliothequePrixIndex = $ctrl.bibliothequePrix.findIndex((bp) => {
                return bp.RessourceId === ressource.RessourceId;
            });
            return bibliothequePrixIndex !== -1;
        }
    }

})();
