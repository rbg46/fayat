
(function () {
  'use strict';

  angular.module('Fred').component('tacheDetailComponent', {
    templateUrl: '/Areas/Budget/Scripts/TacheDetail/TacheDetailTemplate.html',
    bindings: {
      resources: '<',
      devises: '<',
      deviseSelected: '<',
      tasks: '<',
      bugetRevisionStatut: '<',
      ciSelected: '<'
    },
    controller: 'tacheDetailComponentController'
  });

  angular.module('Fred').controller('tacheDetailComponentController', tacheDetailComponentController);

  tacheDetailComponentController.$inject = ['$scope',
                                            '$q',
                                            'ProgressBar',
                                            'Notify',
                                            'BudgetDataService',
                                            'TacheCalculatorService',
                                            'TacheValidatorService',
                                            '$uibModal',
                                            'ModelStateErrorManager',
                                            'RessourceUpdaterService',
                                            'TachePriceManagerService',
                                            'CiManagerService',
                                            'ParametrageReferentielEtenduService',
                                            'BudgetCopyPasteService'];

  function tacheDetailComponentController($scope,
                            $q,
                            ProgressBar,
                            Notify,
                            BudgetDataService,
                            TacheCalculatorService,
                            TacheValidatorService,
                            $uibModal,
                            ModelStateErrorManager,
                            RessourceUpdaterService,
                            TachePriceManagerService,
                            CiManagerService,
                            ParametrageReferentielEtenduService,
                            BudgetCopyPasteService) {

    var $ctrl = this;
    var savedTask = null;
    $ctrl.selectedRessourceTache = null;
    $ctrl.serverError = '';


    //////////////////////////////////////////////////////////////////
    // Déclaration des fonctions publiques                          //
    //////////////////////////////////////////////////////////////

    $ctrl.handleTacheQuantitesChange = handleTacheQuantitesChange;
    $ctrl.handleRessourceQuantiteBaseChange = handleRessourceQuantiteBaseChange;
    $ctrl.handleRessourceFormuleChange = handleRessourceFormuleChange;
    $ctrl.handleDeleteRessource = handleDeleteRessource;
    $ctrl.handleSelectionRessourceTask = handleSelectionRessourceTask;

    $ctrl.handleCanCopyTask = handleCanCopyTask;
    $ctrl.handleCopyTask = handleCopyTask;
    $ctrl.handleCanPasteTask = handleCanPasteTask;
    $ctrl.handlePasteTask = handlePasteTask;

    $ctrl.handleSaveTask = handleSaveTask;
    $ctrl.handleCancelTask = handleCancelTask;
    $ctrl.handleCheckFormuleError = handleCheckFormuleError;


    $ctrl.handlePriceOfParamChange = handlePriceOfParamChange;
    $ctrl.handlePriceRessourceTaskChange = handlePriceRessourceTaskChange;

    $ctrl.handleSelectUnite = handleSelectUnite;
    $ctrl.handleDeleteUnite = handleDeleteUnite;


    //////////////////////////////////////////////////////////////////
    // Init                                                         //
    //////////////////////////////////////////////////////////////////

    $ctrl.$onInit = function () {

      $scope.$on('budgetCrtl.openTask', function (event, newTaskSelected) {
        onOpenTask(newTaskSelected);
      });


      $scope.$on('budgetCrtl.addRessourceToTask', function (event, newRessource) {
        onAddRessourceToTask(newRessource);
      });

      $scope.$on('budgetCrtl.deviseChanged', function (event, deviseSelected) {
        onDeviseChanged(deviseSelected);
      });

      $scope.$on('budgetCrtl.ressourceModifiedOnRessourceView2', function (event, info) {
        TacheCalculatorService.calculateAllForEachRessourceTask($ctrl.taskSelected, $ctrl.deviseSelected);
        TacheCalculatorService.calculTotalsForTask($ctrl.taskSelected, $ctrl.deviseSelected);
        changePrixUnitaire($ctrl.deviseSelected);
        changeUnite($ctrl.deviseSelected);
      });


    };


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Evenements                                                   //
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    function onDeviseChanged(deviseSelected) {
      changePrixUnitaire(deviseSelected);
      changeUnite(deviseSelected);
    }

    /*
    *  Ajout d'une ressource tache
    */
    function onAddRessourceToTask(newRessource) {
      markTaskAsModified(true);

      var ressourceCopy = {};
      angular.copy(newRessource, ressourceCopy);

      var ressourceTache = {
        RessourceTacheId: 0,
        TacheId: $ctrl.taskSelected.TacheId,
        RessourceId: ressourceCopy.RessourceId,
        Ressource: ressourceCopy,
        QuantiteBase: 1,
        Quantite: null,
        Formule: null,
        Montant: null,
        MontantTotal: null,
        RessourceTacheDevises: [],
        TypeRessourceId: ressourceCopy.TypeRessourceId
      };

      //on rensaigne le prix et l'unite.        
      changesPricesOfRessourceTask(ressourceTache, $ctrl.deviseSelected);
      var paramRefEtendu = ParametrageReferentielEtenduService.getLastParametrageReferentielEtenduForDevise(ressourceTache, $ctrl.deviseSelected);
      if (paramRefEtendu !== null) {
        ressourceTache.unite = paramRefEtendu.Unite;
      }
      else {
        ressourceTache.unite = ParametrageReferentielEtenduService.getDefaultUnite($ctrl.taskSelected, ressourceTache.Ressource, $ctrl.deviseSelected);
      }


      //on rajoute la ressourceTache a la tache
      $ctrl.taskSelected.RessourceTaches.push(ressourceTache);

      TacheCalculatorService.calculateAllForEachRessourceTask($ctrl.taskSelected, $ctrl.deviseSelected);
      TacheCalculatorService.calculTotalsForTask($ctrl.taskSelected, $ctrl.deviseSelected);

    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // HANDLERS  VERIFICATION DES ERREURS                                                                                             //
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    function handleCheckFormuleError() {
      return TacheValidatorService.hasAnyFormuleErrorInTask($ctrl.taskSelected);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // HANDLERS  OUVERTURE FERMETURE                                                   //
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    function onOpenTask(newTaskSelected) {

      $ctrl.serverError = '';

      //sauvegarde pour remise a zero sur l'appui sur cancel
      savedTask = angular.copy(newTaskSelected, savedTask);
      //ici je modifie la tache de la liste (partagé entre detail et la liste)
      $ctrl.taskSelected = newTaskSelected;

      TacheCalculatorService.initializeQuantityBaseIfUndefined($ctrl.taskSelected);
      TacheCalculatorService.initializeQuantityToRealizedIfUndefined($ctrl.taskSelected);
      TacheCalculatorService.calculateAllForEachRessourceTask($ctrl.taskSelected, $ctrl.deviseSelected);
      TacheCalculatorService.calculTotalsForTask($ctrl.taskSelected, $ctrl.deviseSelected);
      changePrixUnitaire($ctrl.deviseSelected);
      changeUnite($ctrl.deviseSelected);
    }


    function handleCancelTask() {

      BudgetCopyPasteService.clear();

      markTaskAsModified(false);
      // je remet les valeurs de la tache 
      angular.merge($ctrl.taskSelected, savedTask);
      // je  vide les ressources taches, car le merge n'a pas supprimer les ressourcesTaches.
      $ctrl.taskSelected.RessourceTaches = [];
      // Remise des anciennes ressourcesTache avec les ressourcesTaches sauvegardées.
      angular.merge($ctrl.taskSelected.RessourceTaches, savedTask.RessourceTaches);

      RessourceUpdaterService.rollbackRessourcesModification();

      TacheCalculatorService.calculateAllForEachRessourceTask($ctrl.taskSelected, $ctrl.deviseSelected);
      TacheCalculatorService.calculTotalsForTask($ctrl.taskSelected, $ctrl.deviseSelected);

      $ctrl.taskSelected = null;

      $scope.$emit('changeView', 'listingView');

      $ctrl.serverError = '';
    }



    function handleSaveTask() {
      updateRessources().then(function () {
        onSaveTask();
      });
    }

    //////////////////////////////////////////////////////////////////
    // Handlers COPIER COLLER                                       //
    //////////////////////////////////////////////////////////////////


    function handleCopyTask() {
      BudgetCopyPasteService.copy($ctrl.taskSelected);
    }

    function handleCanCopyTask() {
      return BudgetCopyPasteService.canCopy($ctrl.taskSelected);
    }

    function handleCanPasteTask() {
      return BudgetCopyPasteService.canPaste();
    }

    //todo verifier que le copier collé marche apres modif referentiel
    function handlePasteTask() {

      markTaskAsModified(true);
      BudgetCopyPasteService.paste($ctrl.taskSelected);
      //re-calcul tout sur la tache
      TacheCalculatorService.calculateAllForEachRessourceTask($ctrl.taskSelected, $ctrl.deviseSelected);
      TacheCalculatorService.calculTotalsForTask($ctrl.taskSelected, $ctrl.deviseSelected);
    }


    //////////////////////////////////////////////////////////////////
    // Handlers QUANTITES                                           //
    //////////////////////////////////////////////////////////////////


    function handleTacheQuantitesChange() {
      markTaskAsModified(true);
      TacheCalculatorService.manageTaskQuantityChanged($ctrl.taskSelected, $ctrl.deviseSelected);
    }

    function handleRessourceQuantiteBaseChange(ressourceTache) {
      markTaskAsModified(true);

      ressourceTache.Formule = null;
      ressourceTache.hasFormuleError = false;

      TacheCalculatorService.calculateAllForOneRessourceTask($ctrl.taskSelected, ressourceTache, $ctrl.deviseSelected);
      TacheCalculatorService.calculTotalsForTask($ctrl.taskSelected, $ctrl.deviseSelected);
    }


    //////////////////////////////////////////////////////////////////
    // Handlers PRIX UNITAIRE                                       //
    //////////////////////////////////////////////////////////////////


    function handlePriceOfParamChange(ressourceTache) {
      markTaskAsModified(true);
      RessourceUpdaterService.saveRessourceForRollback(ressourceTache.Ressource);
      TachePriceManagerService.setPriceOfParam($ctrl.taskSelected, ressourceTache, ressourceTache.priceOfParam, $ctrl.deviseSelected);
      RessourceUpdaterService.modifyRessourceInRessourcesView(ressourceTache.Ressource);
      RessourceUpdaterService.modifyRessourceInTasksView(ressourceTache.Ressource);
      $scope.$emit('tacheDetailComponent.ressourceChangedOnDetail', ressourceTache.Ressource);
      changePrixUnitaire($ctrl.deviseSelected);
      TacheCalculatorService.calculateAllForEachRessourceTask($ctrl.taskSelected, $ctrl.deviseSelected);
      TacheCalculatorService.calculTotalsForTask($ctrl.taskSelected, $ctrl.deviseSelected);

    }

    function handlePriceRessourceTaskChange(ressourceTache) {
      markTaskAsModified(true);
      TachePriceManagerService.setPriceOfRessourceTask(ressourceTache, ressourceTache.priceOfRessourceTask, $ctrl.deviseSelected);
      TacheCalculatorService.calculateAllForOneRessourceTask($ctrl.taskSelected, ressourceTache, $ctrl.deviseSelected);
      TacheCalculatorService.calculTotalsForTask($ctrl.taskSelected, $ctrl.deviseSelected);
    }




    //////////////////////////////////////////////////////////////////
    // Handlers FORMULE                                             //
    //////////////////////////////////////////////////////////////////

    function handleRessourceFormuleChange(ressourceTache) {
      markTaskAsModified(true);

      if (ressourceTache.Formule === '') {
        ressourceTache.hasFormuleError = false;
        TacheCalculatorService.calculateAllForOneRessourceTask($ctrl.taskSelected, ressourceTache, $ctrl.deviseSelected);
        return;
      }
      if (TacheCalculatorService.canCalculQuantiteBaseWithFormule($ctrl.taskSelected, ressourceTache)) {
        ressourceTache.QuantiteBase = TacheCalculatorService.calculQuantiteBaseWithFormule($ctrl.taskSelected, ressourceTache);
        TacheCalculatorService.calculateAllForOneRessourceTask($ctrl.taskSelected, ressourceTache, $ctrl.deviseSelected);
      } else {
        TacheCalculatorService.resetRessourceTask($ctrl.taskSelected, ressourceTache);
      }
      TacheCalculatorService.calculTotalsForTask($ctrl.taskSelected, $ctrl.deviseSelected);
    }

    //////////////////////////////////////////////////////////////////
    // Handlers SUPPRESSION                                         //
    //////////////////////////////////////////////////////////////////

    function handleDeleteRessource(ressourceTache) {
      markTaskAsModified(true);
      deleteRessource(ressourceTache);
      TacheCalculatorService.calculTotalsForTask($ctrl.taskSelected, $ctrl.deviseSelected);
    }

    //////////////////////////////////////////////////////////////////
    // Handlers SELECTION                                           //
    //////////////////////////////////////////////////////////////////

    /*
     * manage la selection d'une ressource tache
     */
    function handleSelectionRessourceTask(ressourceTache) {
      $ctrl.selectedRessourceTache = ressourceTache;
    }

    //////////////////////////////////////////////////////////////////
    // Handlers unite de la tache                                   //
    //////////////////////////////////////////////////////////////////

    function handleSelectUnite(newUnite) {
      $ctrl.taskSelected.Unite = newUnite;
      $ctrl.taskSelected.UniteId = newUnite.UniteId;
    }

    function handleDeleteUnite() {
      $ctrl.taskSelected.Unite = null;
      $ctrl.taskSelected.UniteId = null;
    }




    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Actions                                                                                                                        //
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////
    // Actions - Mise a jour du prix d'une ressource                //
    //////////////////////////////////////////////////////////////////

    function updateRessources() {
      var ressources = [];

      if (!$ctrl.isBusy) {
        ProgressBar.start();
        BudgetCopyPasteService.clear();
        $ctrl.isBusy = true;
        var ciId = CiManagerService.getCi().CiId;
        var modifiedRessources = RessourceUpdaterService.getRessourcesModified($ctrl.taskSelected);
        for (var i = 0; i < modifiedRessources.length; i++) {
          var ressource = modifiedRessources[i];
          var promise = BudgetDataService
                        .UpdateRessource(ressource, ciId)
                        .then(updateRessourceSuccess);
          ressources.push(promise);
        }

        return $q.all(ressources)
                  .then(updateRessourcesSuccess)
                  .catch(updateRessourcesError)
                  .finally(updateRessourcesEnd);
      }
      return $q.resolve(null);

    }

    function updateRessourceSuccess(response) {

      RessourceUpdaterService.modifyRessourceInRessourcesView(response.data);
      RessourceUpdaterService.modifyRessourceInTasksView(response.data);

      changePrixUnitaire($ctrl.deviseSelected);
      changeUnite($ctrl.deviseSelected);

      TacheCalculatorService.calculateAllForEachRessourceTask($ctrl.taskSelected, $ctrl.deviseSelected);
      TacheCalculatorService.calculTotalsForTask($ctrl.taskSelected, $ctrl.deviseSelected);


      $scope.$emit('tacheDetailComponent.ressourceChangedOnDetail', response.data);

    }

    function updateRessourcesSuccess() {
      RessourceUpdaterService.clear();
      Notify.message($ctrl.resources.TacheDetailComponent_createParameterSuccess);
    }

    function updateRessourcesError() {
      Notify.error($ctrl.resources.TacheDetailComponent_updateRessourceError);
      return $q.reject(false);
    }

    function updateRessourcesEnd() {
      $ctrl.isBusy = false;
      ProgressBar.complete();
    }




    //////////////////////////////////////////////////////////////////
    // Actions - Sauvegarde                                         //
    //////////////////////////////////////////////////////////////////

    function onSaveTask() {
      ProgressBar.start();

      $ctrl.isBusy = true;
      $ctrl.serverError = '';
      var taskToSend = {};
      angular.copy($ctrl.taskSelected, taskToSend);
      for (var i = 0; i < taskToSend.RessourceTaches.length; i++) {
        var ressourceTask = taskToSend.RessourceTaches[i];
        for (var j = 0; j < ressourceTask.RessourceTacheDevises.length; j++) {
          var ressourceTacheDevise = ressourceTask.RessourceTacheDevises[j];
          ressourceTacheDevise.Devise = null;
        }
      }

      return BudgetDataService
           .UpdateTacheWithRessourceTaches($ctrl.ciSelected.CiId, taskToSend)
           .then(saveTaskSuccess)
           .catch(saveTaskError)
           .finally(saveTaskEnd);
    }

    function saveTaskSuccess(response) {
      markTaskAsModified(false);

      Notify.message($ctrl.resources.TacheDetailComponent_saveTaskSuccess);
      $scope.$emit('changeView', 'listingView');

      $scope.$emit('tacheDetailComponent.taskSaved', response.data);
      $ctrl.isBusy = false;
    }

    function saveTaskError(error) {
      var validationError = ModelStateErrorManager.getErrors(error);
      if (validationError) {
        $ctrl.serverError = validationError;
      }
      else if (error.data && error.data.Message) {
        $ctrl.serverError = error.data.Message;
      }
      else {
        $ctrl.serverError = $ctrl.resources.TacheDetailComponent_saveTaskError;
      }

      $scope.$emit('tacheDetailComponent.taskSaved', null);
      $ctrl.isBusy = false;
    }

    //permet de mettre l'overlay sur la liste
    function markTaskAsModified(isModified) {
      $scope.$emit('tacheDetailComponent.selectedTaskIsModified', { isModified: isModified });
    }


    function saveTaskEnd() {
      $ctrl.isBusy = false;
      ProgressBar.complete();
    }


    //////////////////////////////////////////////////////////////////
    // Helpers                                                      //
    //////////////////////////////////////////////////////////////////

    /*
   * Change le prix sur toutes les ressources taches.
   */
    function changePrixUnitaire(deviseSelected) {
      if ($ctrl.taskSelected && $ctrl.taskSelected.RessourceTaches) {
        for (var i = 0; i < $ctrl.taskSelected.RessourceTaches.length; i++) {
          var ressourceTask = $ctrl.taskSelected.RessourceTaches[i];
          changesPricesOfRessourceTask(ressourceTask, deviseSelected);
        }
      }

    }

    function changesPricesOfRessourceTask(ressourceTache, deviseSelected) {
      var priceOfParam = TachePriceManagerService.getPriceOfParam(ressourceTache, deviseSelected);
      ressourceTache.priceOfParam = priceOfParam;
      var priceOfRessourceTask = TachePriceManagerService.getPriceOfRessourceTask(ressourceTache, deviseSelected);
      ressourceTache.priceOfRessourceTask = priceOfRessourceTask;
      if (ressourceTache.priceOfParam === null) {
        ressourceTache.priceOfParam = 0;
      }

    }


    /*
    * Change l'unite sur toutes les ressources taches.
    */
    function changeUnite(deviseSelected) {
      if ($ctrl.taskSelected && $ctrl.taskSelected.RessourceTaches) {
        for (var i = 0; i < $ctrl.taskSelected.RessourceTaches.length; i++) {
          var ressourceTask = $ctrl.taskSelected.RessourceTaches[i];
          var paramRefEtendu = ParametrageReferentielEtenduService.getLastParametrageReferentielEtenduForDevise(ressourceTask, deviseSelected);
          if (paramRefEtendu) {
            ressourceTask.unite = paramRefEtendu.Unite;
          }
          else {
            ressourceTask.unite = ParametrageReferentielEtenduService.getDefaultUnite($ctrl.taskSelected, ressourceTask.Ressource, $ctrl.deviseSelected);
          }

        }
      }
    }

    function deleteRessource(ressourceTache) {
      var index = $ctrl.taskSelected.RessourceTaches.indexOf(ressourceTache);
      $ctrl.taskSelected.RessourceTaches.splice(index, 1);
    }


  }

})();