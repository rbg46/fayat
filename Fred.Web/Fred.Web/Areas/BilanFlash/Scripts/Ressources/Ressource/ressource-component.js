(function () {
  'use strict';

  angular
    .module('Fred')
    .component('ofRessourceComponent', {
      templateUrl: '/Areas/BilanFlash/Scripts/Ressources/Ressource/ressource-template.html',
      bindings: {
        resources: '<',
        currentRessource: '<',
        deviseSelected: '<'
      },
      controller: 'ofRessourceComponentController'
    });

  angular.module('Fred').controller('ofRessourceComponentController', ofRessourceComponentController);

  ofRessourceComponentController.$inject = ['$scope'];

  function ofRessourceComponentController($scope) {

    var $ctrl = this;

    $ctrl.isBusy = false;
    $ctrl.isOpen = true;

    $ctrl.ressourcesFilter = ressourcesFilter;

    //////////////////////////////////////////////////////////////////
    // Init                                                         //
    //////////////////////////////////////////////////////////////////

    $ctrl.$onInit = function () {
      manageAllDataAndVisibilityForDevise();
    };

    //////////////////////////////////////////////////////////////////
    // Evenements                                                   //
    //////////////////////////////////////////////////////////////////

    function manageAllDataAndVisibilityForDevise() {
      manageAllDataAndVisibilityForDeviseAndRessource($ctrl.currentRessource);

      if ($ctrl.currentRessource.RessourcesEnfants && $ctrl.currentRessource.RessourcesEnfants.length > 0) {
        for (var i = 0; i < $ctrl.currentRessource.RessourcesEnfants.length; i++) {
          var child = $ctrl.currentRessource.RessourcesEnfants[i];
          manageAllDataAndVisibilityForDeviseAndRessource(child);
        }
      }
    }

    function manageAllDataAndVisibilityForDeviseAndRessource(ressource) {
      if (ressource.PuHT !== 0) {
        ressource.Symbole = $ctrl.deviseSelected.Symbole;
      } else {
        ressource.PuHT = 0;
        ressource.Symbole = null;
      }
    }

    //////////////////////////////////////////////////////////////////
    // Actions                                                     //
    //////////////////////////////////////////////////////////////////

    /*
    * @function ressourcesFilter(searchText)
    * @description Filtre des Ressources
    */
    function ressourcesFilter(searchText) {
      return function (item) {
          return (item.Code.toLowerCase().indexOf(searchText.toLowerCase()) >= 0 || item.Libelle.toLowerCase().indexOf(searchText.toLowerCase()) >= 0);
      };
    }

    $ctrl.clickHeader = function clickHeader() {
      $ctrl.isOpen = !$ctrl.isOpen;
    };

    $ctrl.clickAddToTask = function clickAddToTask(newRessource) {
          $scope.$emit('objectifFlashRessourceSelected', {ressource: newRessource});
    };

  }

})();