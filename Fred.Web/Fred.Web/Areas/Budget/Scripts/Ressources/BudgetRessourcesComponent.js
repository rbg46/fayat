(function () {
  'use strict';

  angular
    .module('Fred')
    .component('budgetRessourcesComponent', {
      templateUrl: '/Areas/Budget/Scripts/Ressources/BudgetRessourcesTemplate.html',
      bindings: {
        resources: '<',
        ciSelected: '<',
        deviseSelected: '<'
      },
      controller: 'budgetRessourcesComponentController'
    });

  angular.module('Fred').controller('budgetRessourcesComponentController', budgetRessourcesComponentController);

  budgetRessourcesComponentController.$inject = ['$scope', 'ProgressBar', 'BudgetDataService', 'Notify', 'RessourceUpdaterService'];

  function budgetRessourcesComponentController($scope, ProgressBar, BudgetDataService, Notify, RessourceUpdaterService) {

    var $ctrl = this;
    var isBusy = false;
    var chapitrePage = 0;
    var chapitrePageSize = 25;
    var canLoadNextPage = true;

    //////////////////////////////////////////////////////////////////
    // Déclaration des propriétés publiques                         //
    //////////////////////////////////////////////////////////////////
    $ctrl.search = '';
    $ctrl.chapitres = [];
    $ctrl.ressourcesAreOpen = false;
    $ctrl.sousChapitreIsOpen = false;

    //////////////////////////////////////////////////////////////////
    // Déclaration des fonctions publiques                          //
    //////////////////////////////////////////////////////////////////
    $ctrl.handleSearchTextChanged = handleSearchTextChanged;
    $ctrl.handleClickChapitre = handleClickChapitre;
    $ctrl.handleClickSousChapitre = handleClickSousChapitre;

    //////////////////////////////////////////////////////////////////
    // Init                                                         //
    //////////////////////////////////////////////////////////////////
    $ctrl.$onInit = function () {
      RessourceUpdaterService.setChapitres($ctrl.chapitres);
      $scope.$on('budgetCrtl.ciSelectedChanged', function (event, ciSelected) {
        clearbudgetRessources();
        canLoadNextPage = true;
        chapitrePage = 0;
        actionLoad(ciSelected);
      });

      FredToolBox.bindScrollEnd('#containerRessources', actionLoadMore);
    };

    //////////////////////////////////////////////////////////////////
    // Handlers                                                     //
    //////////////////////////////////////////////////////////////////
      function handleSearchTextChanged() {
          console.log($ctrl.chapitres);
          clearbudgetRessources();
          canLoadNextPage = true;
          chapitrePage = 0;
          actionLoad($ctrl.ciSelected);
      }

    function handleClickChapitre(chapitre) {
      chapitre.isOpen = !chapitre.isOpen;
    }

    function handleClickSousChapitre(sousChapitre) {
      sousChapitre.isOpen = !sousChapitre.isOpen;
    }

    //////////////////////////////////////////////////////////////////
    // Actions                                                      //
    //////////////////////////////////////////////////////////////////
    function actionLoad(ciSelected) {
      if (!isBusy && ciSelected !== null && canLoadNextPage) {
        isBusy = true;
        ProgressBar.start();
        BudgetDataService
              .GetChapitres(ciSelected, $ctrl.search, getNextPage(), chapitrePageSize)
              .then(actionLoadSuccess)
              .catch(actionLoadError)
              .finally(actionLoadEnd);
      }
    }

    function actionLoadSuccess(result) {
      if (result.data.length < chapitrePageSize) {
        canLoadNextPage = false;
      }
      for (var i = 0; i < result.data.length; i++) {
        var chapitre = result.data[i];
        for (var j = 0; j < chapitre.SousChapitres.length; j++) {
          var sousChapitres = chapitre.SousChapitres[j];
          sousChapitres.isOpen = false;
        }
        chapitre.isOpen = false;
        $ctrl.chapitres.push(chapitre);
      }
      RessourceUpdaterService.setChapitres($ctrl.chapitres);
    }

    function actionLoadError() {
      Notify.error($ctrl.resources.BudgetRessourcesComponent_actionLoadError);
    }

    function actionLoadEnd() {
      ProgressBar.complete();
      isBusy = false;
    }

    function actionLoadMore() {
      actionLoad($ctrl.ciSelected);
    }

    function clearbudgetRessources() {
      $ctrl.chapitres = [];
      RessourceUpdaterService.setChapitres($ctrl.chapitres);
    }

    function getNextPage() {
      var newPage = chapitrePage;
      chapitrePage += 1;
      return newPage;
    }


  }
})();