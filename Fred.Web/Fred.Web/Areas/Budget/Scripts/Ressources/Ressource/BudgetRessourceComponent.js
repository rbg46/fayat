(function () {
  'use strict';

  angular
    .module('Fred')
    .component('budgetRessourceComponent', {
      templateUrl: '/Areas/Budget/Scripts/Ressources/Ressource/BudgetRessourceTemplate.html',
      bindings: {
        resources: '<',
        budgetRessource: '<',
        chapitre: '<',
        sousChapitre: '<',
        deviseSelected: '<',
        ciSelected: '<'
      },
      controller: 'budgetRessourceComponentController'
    });

  angular.module('Fred').controller('budgetRessourceComponentController', budgetRessourceComponentController);

  budgetRessourceComponentController.$inject = ['$scope', '$uibModal', 'ProgressBar', 'BudgetDataService', 'TachePriceManagerService', '$log', '$timeout', 'ParametrageReferentielEtenduService'];

  function budgetRessourceComponentController($scope, $uibModal, ProgressBar, BudgetDataService, TachePriceManagerService, $log, $timeout, ParametrageReferentielEtenduService) {

    var $ctrl = this;

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

    //////////////////////////////////////////////////////////////////
    // Init                                                         //
    //////////////////////////////////////////////////////////////////

    $ctrl.$onInit = function () {
      $scope.$on('budgetCrtl.deviseChanged', function (event, deviseSelected) {
        onDeviseChanged(deviseSelected);
      });
      $scope.$on('budgetCrtl.ressourceChangedOnDetail', function (event, updatedRessource) {
        onRessourceChangedOnDetail(updatedRessource);
      });
      manageAllDataAndVisibilityForDevise($ctrl.budgetRessource, $ctrl.deviseSelected);
    };

    //////////////////////////////////////////////////////////////////
    // Evenements                                                   //
    //////////////////////////////////////////////////////////////////


    /*
    *Action executé apres la mise a jour d'une ressource dans l'ecran de detail.
    *Affiche le prix correctement.
    */
    function onRessourceChangedOnDetail(updatedRessource) {
      if (updatedRessource.RessourceId === $ctrl.budgetRessource.RessourceId) {
        $timeout(function () {
          manageAllDataAndVisibilityForDevise($ctrl.budgetRessource, $ctrl.deviseSelected);
        });
      }
      for (var l = 0; l < $ctrl.budgetRessource.RessourcesEnfants.length; l++) {
        var childRessource = $ctrl.budgetRessource.RessourcesEnfants[l];
        if (childRessource.RessourceId === updatedRessource.RessourceId) {
          manageAllDataAndVisibilityForDevise($ctrl.budgetRessource, $ctrl.deviseSelected);
        }
      }
    }

    /*
   * action executée sur la reception d'un event budgetCrtl.deviseChanged.
   * Recalcule de tous les totaux pour toutes les taches.
   */
    function onDeviseChanged(deviseSelected) {
      manageAllDataAndVisibilityForDevise($ctrl.budgetRessource, deviseSelected);
    }

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
      var hasAnyParametrage = ParametrageReferentielEtenduService.hasAnyParametrageReferentielEtendu(ressource);
      var param = ParametrageReferentielEtenduService.getLastParametrageReferentielEtenduForDeviseByRessource(ressource, deviseSelected);
      if (param) {
        var montant = param.Montant;
        var symbole = param.Devise.Symbole;
        var unite = param.Unite;

        ressource.montant = montant;
        ressource.symbole = symbole;
        ressource.unite = unite;
        ressource.hasAnyParametrages = hasAnyParametrage;
      } else {
        ressource.montant = 0;
        ressource.symbole = null;
        ressource.hasAnyParametrages = hasAnyParametrage;
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
      $scope.$emit('budgetRessourceComponent.addRessourceToTask', newRessource);
    }



    function handleClickEditChild(ressourceParent, existingRessource) {

      var modalInstance = openModal(true, existingRessource, ressourceParent);


      modalInstance.result.then(function (updatedRessource) {
        // Lorsque la mise a jour a reussit, nous remplacons la ressource de la liste par la ressource retournée du serveur. 
        var index = ressourceParent.RessourcesEnfants.map(function (e) { return e.RessourceId; }).indexOf(updatedRessource.RessourceId);
        if (index !== -1) {
          ressourceParent.RessourcesEnfants[index] = updatedRessource;
          manageAllDataAndVisibilityForDevise(updatedRessource, $ctrl.deviseSelected);
          $scope.$emit('budgetRessourceComponent.ressourceModifiedOnRessourceView', updatedRessource);
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
        component: 'createBudgetRessourceComponent',
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
          }
        }
      });

      return modalInstance;
    }



  }

})();