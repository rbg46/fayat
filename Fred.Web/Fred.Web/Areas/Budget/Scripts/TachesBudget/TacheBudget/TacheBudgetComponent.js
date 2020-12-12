
(function () {
  'use strict';

  angular
    .module('Fred')
    .component('tacheBudgetComponent', {
      templateUrl: '/Areas/Budget/Scripts/TachesBudget/TacheBudget/TacheBudgetTemplate.html',
      bindings: {
        resources: '<',
        tache: '<',
        isLargeList: '<',
        taskSelected: '<',
        deviseSelected: '<',
        bugetRevisionStatut: '<'
      },
      controller: 'tacheBudgetComponentController'
    });

  angular.module('Fred').controller('tacheBudgetComponentController', tacheBudgetComponentController);

  tacheBudgetComponentController.$inject = ['$scope',
                                            'Notify',
                                            'BudgetDataService',
                                            'confirmDialog',
                                            'TacheBudgetCodeLibelleFilterService',
                                            'TacheCalculatorService',
                                            'TacheBudgetRecetteService',
                                            '$sessionStorage',
                                            'TacheValidatorService'];

  function tacheBudgetComponentController ($scope,
                                            Notify,
                                            BudgetDataService,
                                            confirmDialog,
                                            TacheBudgetCodeLibelleFilterService,
                                            TacheCalculatorService,
                                            TacheBudgetRecetteService,
                                            $sessionStorage,                         
                                            TacheValidatorService) {

    var $ctrl = this;

    var validationChangeRecetteOnTask = false;
    var savedRecette = null;
    var dialogIsOpen = false;
    $ctrl.selectedTypeAvancement = null;
    $ctrl.typeAvancements = [
        { id: '1', name: '%' },//todo I18N
        { id: '2', name: 'Quantite' }
    ];

    //$ctrl.taskMachWithFilter = true;
    $ctrl.errorInfo = null;


    //////////////////////////////////////////////////////////////////
    // Déclaration des fonctions publiques                          //
    //////////////////////////////////////////////////////////////////

    $ctrl.handleClickHeader = handleClickHeader;
    $ctrl.handleGetLevel = handleGetLevel;
    $ctrl.handleSelect = handleSelect;
    $ctrl.handlecanShowArrow = handlecanShowArrow;
    $ctrl.handleOpenChild = handleOpenChild;
    $ctrl.handleQuantiteChanged = handleQuantiteChanged;
    $ctrl.handleRecetteChanged = handleRecetteChanged;
    $ctrl.handleAvancementChanged = handleAvancementChanged;
    $ctrl.handleTypeAvancementChanged = handleTypeAvancementChanged;
    $ctrl.handleSaveRecette = handleSaveRecette;
    $ctrl.handleBlurRecette = handleBlurRecette;
    $ctrl.handleGetTaskVisibility = handleGetTaskVisibility;



    //////////////////////////////////////////////////////////////////
    // Init                                                         //
    //////////////////////////////////////////////////////////////////

    $ctrl.$onInit = function () {        
      $ctrl.selectedTypeAvancement = getSelectedtypeAvancement($ctrl.tache.TypeAvancement);
      if ($ctrl.selectedTypeAvancement === null) {
        $ctrl.selectedTypeAvancement = $ctrl.typeAvancements[0];
      }
      $ctrl.errorInfo = TacheValidatorService.getTasksErrorInfo($ctrl.tache);

      $scope.$on('tachesBudgetComponent.taskSaved', function (event) {
        $ctrl.tache.isModified = true;
        $ctrl.errorInfo = TacheValidatorService.getTasksErrorInfo($ctrl.tache);
      });

      $scope.$on('budgetCrtl.deviseChanged', function (event, deviseSelected) {
        onDeviseChanged(deviseSelected);
      });

      $ctrl.tache.Recette = TacheBudgetRecetteService.getMontantRecetteByDevise($ctrl.tache, $ctrl.deviseSelected);

    };


    //////////////////////////////////////////////////////////////////
    // Evenements                                                   //
    //////////////////////////////////////////////////////////////////


    function onDeviseChanged(deviseSelected) {

      $ctrl.tache.Recette = TacheBudgetRecetteService.getMontantRecetteByDevise($ctrl.tache, deviseSelected);

    }


    //////////////////////////////////////////////////////////////////
    // Handlers                                                     //
    //////////////////////////////////////////////////////////////////


    function handlecanShowArrow(tache) {
      if (tache.TachesEnfants && tache.TachesEnfants.length > 0) {
        return true;
      } else {
        return false;
      }
    }

    function handleOpenChild() {
      if ($ctrl.tache.TachesEnfants && $ctrl.tache.TachesEnfants.length > 0) {
        $ctrl.isOpen = !$ctrl.isOpen;
      } else {
        $ctrl.isOpen = false;
      }
    }

    function handleClickHeader() {
      $scope.$emit('tacheBudgetComponent.taskSelectedChanged', $ctrl.tache);
    }

    /*
     * Lance un event pour ouvrir un budget. C'est le controlleur principal qui gere les vues.
     */
    function handleSelect($event) {
      if ($ctrl.tache.Niveau === 4) {
        $scope.$emit('tacheBudgetComponent.openTask', $ctrl.tache);
      }
    }


    function handleGetLevel() {
      return "level" + $ctrl.tache.Niveau;
    }

    /*
     * Determine si la ligne correspondant a une tache est visible
     * en se basant sur le filtre.
     */
    function handleGetTaskVisibility() {
      return TacheBudgetCodeLibelleFilterService.taskMachWithFilter($ctrl.tache.TacheId);
    }


    //////////////////////////////////////////////////////////////////
    // Handlers  GESTION DES CHANGEMENTS SUR UNE LIGNE              //
    //////////////////////////////////////////////////////////////////


    function handleQuantiteChanged(tache, formTacheBudget) {
      $ctrl.tache.isModified = true;
      TacheCalculatorService.manageTaskQuantityChanged(tache, $ctrl.deviseSelected);
      $scope.$emit('tacheBudgetComponent.quantityChanged', { isValid: !formTacheBudget.quantiteARealise.$invalid, tache: tache });
    }


    function handleSaveRecette(tache) {
      //sauvegarde de la recette, pour rollback
      if (!dialogIsOpen) {
        savedRecette = TacheBudgetRecetteService.getMontantRecetteByDevise(tache, $ctrl.deviseSelected);
      }
    }


    function handleRecetteChanged(tache, formTacheBudget) {
      $ctrl.tache.isModified = true;
      TacheBudgetRecetteService.setMontantRecetteByDevise(tache, $ctrl.deviseSelected, tache.Recette);
      if (!$sessionStorage.dontShowAgainPopup && validationChangeRecetteOnTask === false) {         
        dialogIsOpen = true;
        confirmDialog
           .confirmWithOption($ctrl.resources, $ctrl.resources.TacheBudgetComponent_message_dialog, "Valider et ne plus afficher")
           .then(function (response) {
             if (response.option) {
               $sessionStorage.dontShowAgainPopup = true;                 
             }                
             validationChangeRecetteOnTask = true;
                 

             $scope.$emit('tacheBudgetComponent.recetteChanged', { isValid: !formTacheBudget.$invalid, tache: tache });
             dialogIsOpen = false;
           }).catch(function () {
             validationChangeRecetteOnTask = false;
             $sessionStorage.dontShowAgainPopup = false;                
             //rollback de la recette suite a un refus de l'utilisateur de la modifier.
             tache.Recette = savedRecette;
             TacheBudgetRecetteService.setMontantRecetteByDevise(tache, $ctrl.deviseSelected, savedRecette);
             $scope.$emit('tacheBudgetComponent.recetteChanged', { isValid: !formTacheBudget.$invalid, tache: tache });
             savedRecette = null;
             dialogIsOpen = false;
           });
      } else {
        $scope.$emit('tacheBudgetComponent.recetteChanged', { isValid: !formTacheBudget.$invalid, tache: tache });
      }

    }

    function handleBlurRecette() {
      if (!dialogIsOpen) {
        validationChangeRecetteOnTask = false;
        savedRecette = null;
      }
    }


    function handleAvancementChanged(tache) {
      $ctrl.tache.isModified = true;
      $scope.$emit('tacheBudgetComponent.avanvementChanged', tache);
    }

    //Au changement de valeur du select on place la bonne valeur dans le model
    function handleTypeAvancementChanged(tache) {
      $ctrl.tache.isModified = true;
      tache.TypeAvancement = parseInt($ctrl.selectedTypeAvancement.id, 10);
    }

    //////////////////////////////////////////////////////////////////
    // Actions                                                      //
    //////////////////////////////////////////////////////////////////


    //gestion de la selection du type d'avancement
    function getSelectedtypeAvancement(idTypeAvancement) {
      var selectedOption = $ctrl.typeAvancements.filter(function (option) {
        return option.id === '' + idTypeAvancement;
      });
      if (selectedOption.length > 0) {
        return selectedOption[0];
      }
      return null;
    }

  }

})();