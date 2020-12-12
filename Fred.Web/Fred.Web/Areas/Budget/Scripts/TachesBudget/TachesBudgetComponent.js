(function () {
  'use strict';

  angular
    .module('Fred')
    .component('tachesBudgetComponent', {
      templateUrl: '/Areas/Budget/Scripts/TachesBudget/TachesBudgetTemplate.html',
      bindings: {
        resources: '<',
        deviseSelected: '=',
        bugdetInfoChanged: '&',
        tasksLoaded: '&',
        view: '<',
        bugetRevisionStatut: '<'
      },
      controller: 'tachesBudgetComponentController'
    });

  angular.module('Fred').controller('tachesBudgetComponentController', tachesBudgetComponentController);

  tachesBudgetComponentController.$inject = ['$scope',
                                      '$q',
                                      'ProgressBar',
                                      'BudgetDataService',
                                      'Notify',
                                      '$log',
                                      'RessourceUpdaterService',
                                      'TacheBudgetAvancementService',
                                      'TacheBudgetComparerService',
                                      'TacheBudgetRecetteService',
                                      'TacheBudgetService',
                                      'TacheBudgetCodeLibelleFilterService',
                                      'TacheValidatorService',
                                      'TacheCalculatorService',
                                      'TacheBudgetManagerService',
                                      'ModelStateErrorManager',
                                      'TacheBudgetMontantService'];

  function tachesBudgetComponentController($scope,
                      $q,
                      ProgressBar,
                      BudgetDataService,
                      Notify,
                      $log,
                      RessourceUpdaterService,
                      TacheBudgetAvancementService,
                      TacheBudgetComparerService,
                      TacheBudgetRecetteService,
                      TacheBudgetService,
                      TacheBudgetCodeLibelleFilterService,
                      TacheValidatorService,
                      TacheCalculatorService,
                      TacheBudgetManagerService,
                      ModelStateErrorManager,
                      TacheBudgetMontantService) {

    var $ctrl = this;
    $ctrl.isBusy = false;
    var ciSelected = null;
    var LastBudgetState = null;
    //////////////////////////////////////////////////////////////////
    // Déclaration des propriétés publiques                         //
    //////////////////////////////////////////////////////////////////


    $ctrl.taches = [];
    $ctrl.taskSelected = null;
    $ctrl.isLargeList = true; //par defaut , affichage en mode large liste
    $ctrl.taskSearchText = '';
    //////////////////////////////////////////////////////////////////
    // Déclaration des fonctions publiques                          //
    //////////////////////////////////////////////////////////////////
    $ctrl.handleShowTasksPlan = handleShowTasksPlan;
    //$ctrl.handleShowBudget = handleShowBudget;
    $ctrl.handleDoVadidBudget = handleDoVadidBudget;
    $ctrl.handleVadidBudget = handleVadidBudget;
    $ctrl.handleRejectBudget = handleRejectBudget;
    $ctrl.handleSaveTasks = handleSaveTasks;


    //////////////////////////////////////////////////////////////////
    // Init                                                         //
    //////////////////////////////////////////////////////////////////

    $ctrl.$onInit = function () {
      //ici je m'abonne a l'evenement qui est envoyé lors d'un changement de Ci         
      $scope.$on('budgetCrtl.loadTasks', function (event, data) {
        $ctrl.serverError = '';
        $ctrl.taches = [];
        RessourceUpdaterService.setTasks($ctrl.taches);
        $ctrl.taskSelected = null;
        ciSelected = data;
        actionLoad();
      });

      $scope.$on('changedTaskListView', function (event, data) {
        $ctrl.isLargeList = "LargeList" === data ? true : false;
      });

      $scope.$on('tacheBudgetComponent.taskSelectedChanged', function (event, tache) {
        onTaskSelectedChanged(tache);
      });

      $scope.$on('tacheBudgetComponent.avanvementChanged', function (event, tacheAvancementChanged) {
        onAvancementChanged(tacheAvancementChanged);
      });

      $scope.$on('tacheBudgetComponent.recetteChanged', function (event, infoTask) {
        onRecetteChanged(infoTask);
      });

      $scope.$on('budgetCrtl.taskSaved', function (event, info) {
        $ctrl.serverError = '';
        onTaskSaved(info);
      });

      $scope.$on('tacheBudgetComponent.quantityChanged', function (event, infoTask) {
        onQuantityChanged(infoTask);
      });

      $scope.$on('budgetCrtl.taskSearchTextChanged', function (event, taskSearchText) {
        onTaskSearchTextChanged(taskSearchText);
      });

      $scope.$on('budgetCrtl.deviseChanged', function (event, deviseSelected) {
        onDeviseChanged(deviseSelected);
      });

      $scope.$on('budgetCrtl.onTacksChanged', function (event, info) {
        onTacksChanged(info);
      });


    };



    //////////////////////////////////////////////////////////////////
    // Handlers                                                     //
    //////////////////////////////////////////////////////////////////

    function handleSaveTasks() {
      saveTasks();
    }

    /*
    * Affiche le plan taches
    */
    function handleShowTasksPlan() {
      $scope.$emit('changeView', 'taskPlanView');
    }

    function handleDoVadidBudget() {
      //recuperation des taches modifiés mis a plat.
      var tasksModified = TacheBudgetComparerService.getModifiedTasks($ctrl.taches);
      if (tasksModified.length > 0) {
        saveTasks().then(function () {
          doVadidBudgetRevision();
        });
      } else {
        doVadidBudgetRevision();
      }
    }

    function handleVadidBudget() {
      //recuperation des taches modifiés mis a plat.
      var tasksModified = TacheBudgetComparerService.getModifiedTasks($ctrl.taches);
      if (tasksModified.length > 0) {
        saveTasks().then(function () {
          vadidBudgetRevision();
        });
      } else {
        vadidBudgetRevision();
      }
    }

    function handleRejectBudget() {
      rejectBudgetRevision();
    }

    //////////////////////////////////////////////////////////////////
    // Evenements                                                   //
    //////////////////////////////////////////////////////////////////


    function onTaskSelectedChanged(tache) {
      $ctrl.taskSelected = tache;
    }

    function onAvancementChanged(tacheAvancementChanged) {
      if (tacheAvancementChanged !== null) {
        TacheBudgetAvancementService.modifyParentAndChildrenAvancement($ctrl.taches, tacheAvancementChanged);
      }
    }

    function onRecetteChanged(infoTask) {

      if (infoTask.isValid) {
        TacheBudgetRecetteService.modifyParentAndChildrenRecette($ctrl.taches, infoTask.tache, $ctrl.deviseSelected);
        var recetteTotal = TacheBudgetRecetteService.calculRecetteTotal($ctrl.taches, $ctrl.deviseSelected);
        $scope.$emit('tachesBudgetComponent.recetteTotalChanged', { isValid: true, recetteTotal: recetteTotal });
      } else {
        $scope.$emit('tachesBudgetComponent.recetteTotalChanged', { isValid: false, recetteTotal: null });
      }

    }


    function onTaskSaved(info) {

      TacheBudgetService.replaceTaskT4InParent($ctrl.taches, info);
      //permet de calculer les taches en erreurs.

      TacheValidatorService.calculateTasksErrors($ctrl.taches);

      TacheCalculatorService.calculateTotalsForAllTasks($ctrl.taches, $ctrl.deviseSelected);
      TacheBudgetMontantService.calculateAndModifyTasks($ctrl.taches, $ctrl.deviseSelected);
      $scope.$broadcast('tachesBudgetComponent.taskSaved');

      reloadInfo();
    }


    function onQuantityChanged(infoTask) {
      if (infoTask.isValid) {
        //TacheBudgetMontantService.calculateAndModifyTasks($ctrl.taches, $ctrl.deviseSelected);
        $scope.$emit('tachesBudgetComponent.quantityChanged', { isValid: true });
      } else {
        $scope.$emit('tachesBudgetComponent.quantityChanged', { isValid: false });
      }
    }

    function onTaskSearchTextChanged(taskSearchText) {
      $ctrl.taskSearchText = taskSearchText;
      TacheBudgetCodeLibelleFilterService.setFilterForTasks($ctrl.taches, taskSearchText);
    }

    /*
     * action executée sur la reception d'un event budgetCrtl.deviseChanged.
     * Recalcule de tous les totaux pour toutes les taches.
     */
    function onDeviseChanged(deviseSelected) {

      TacheCalculatorService.calculateTotalsForAllTasks($ctrl.taches, deviseSelected);

      TacheBudgetMontantService.calculateAndModifyTasks($ctrl.taches, deviseSelected);

    }

    function onTacksChanged(info) {
      $ctrl.serverError = '';
      info.isModified = true;
      TacheBudgetManagerService.manage($ctrl.taches, info);
      TacheBudgetCodeLibelleFilterService.setFilterForTasks($ctrl.taches, $ctrl.taskSearchText);
    }





    //////////////////////////////////////////////////////////////////
    // Actions                                                      //
    //////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////
    // Actions - Chargement du budget                               //
    //////////////////////////////////////////////////////////////////
    function actionLoad() {

      if (ciSelected) {
        $ctrl.isBusy = true;
        $ctrl.serverError = '';
        ProgressBar.start();
        BudgetDataService
               .GetBudget(ciSelected.CiId)
               .then(getBudgetSuccess)
               .catch(getBudgetError)
               .then(getBudgetRevisionTachesSuccess)
               .catch(getBudgetRevisionTachesError)
               .finally(actionLoadEnd);
      }
    }

    function getBudgetSuccess(response) {

      var revision = response.data.BudgetRevisions[0];
      //on previent le parent que de nouvelles info sur le budget
      $ctrl.bugdetInfoChanged({ bugdetInfo: revision });

      $ctrl.bugetRevision = revision;

      return BudgetDataService.GetBudgetRevisionTaches(revision.BudgetRevisionId, ciSelected.CiId);
    }


    function getBudgetError() {
      Notify.error($ctrl.resources.TachesBudgetComponent_getBudgetError);
      return $q.reject({ errorAlReadyHandled: true });
    }


    function getBudgetRevisionTachesSuccess(response) {

      $ctrl.taches = response.data;

      RessourceUpdaterService.setTasks($ctrl.taches);
      //initialize le filtre des taches
      TacheBudgetCodeLibelleFilterService.setFilterForTasks($ctrl.taches, '');

      //permet de calculer les taches en erreurs.
      TacheValidatorService.calculateTasksErrors($ctrl.taches);

      //recalcule tous les totaux pour toutes les taches.
      TacheCalculatorService.calculateTotalsForAllTasks($ctrl.taches, $ctrl.deviseSelected);

      //recalcule les montants 'totalt4' pour chaque tache
      TacheBudgetMontantService.calculateAndModifyTasks($ctrl.taches, $ctrl.deviseSelected);

      $ctrl.tasksLoaded({ tasksLoadedInfo: $ctrl.taches });
    }

    function getBudgetRevisionTachesError(error) {
      if (!error.errorAlReadyHandled) {
        Notify.error($ctrl.resources.TachesBudgetComponent_getBudgetRevisionTachesError);
      }
    }

    function actionLoadEnd() {
      ProgressBar.complete();
      $ctrl.isBusy = false;
    }


    //////////////////////////////////////////////////////////////////
    // Actions - Chargement du budget                               //
    //////////////////////////////////////////////////////////////////
    function reloadInfo() {

      if (ciSelected) {
        $ctrl.isBusy = true;
        ProgressBar.start();
        BudgetDataService
               .GetBudget(ciSelected.CiId)
               .then(reloadInfoSuccess)
               .catch(reloadInfoError)
               .finally(reloadInfoEnd);
      }
    }

    function reloadInfoSuccess(response) {

      var revision = response.data.BudgetRevisions[0];
      //on previent le parent que de nouvelles info sur le budget
      $ctrl.bugdetInfoChanged({ bugdetInfo: revision });

      $ctrl.bugetRevision = revision;

    }


    function reloadInfoError() {

    }

    function reloadInfoEnd() {
      ProgressBar.complete();
      $ctrl.isBusy = false;
    }



    //////////////////////////////////////////////////////////////////
    // Actions - Sauvegarde la liste des taches                     //
    //////////////////////////////////////////////////////////////////

    function saveTasks() {

      //recuperation des taches modifiés met a plat les taches.
      var tasksModified = TacheBudgetComparerService.getModifiedTasks($ctrl.taches);

      if (tasksModified.length > 0) {
        $ctrl.serverError = '';
        $ctrl.isBusy = true;
        ProgressBar.start();
        var promise = BudgetDataService
             .UpdateBudgetRevisionTaches(tasksModified)
             .then(updateBudgetRevisionTachesSuccess)
             .catch(updateBudgetRevisionTachesError)
             .finally(updateBudgetRevisionTachesEnd);
        return promise;
      } else {
        $log.warn("Aucune tache n'a été modifiée.");
        $scope.$emit('tachesBudgetComponent.tasksSaved');
        return $q.resolve(null);
      }

    }


    function updateBudgetRevisionTachesSuccess() {
      TacheBudgetComparerService.clearModifiedTasks($ctrl.taches);
      Notify.message($ctrl.resources.TachesBudgetComponent_updateBudgetRevisionTachesSuccess);

      return $q.resolve(true);
    }

    function updateBudgetRevisionTachesError(error) {

      showServerErrorWithDefaultMessage(error, $ctrl.resources.TachesBudgetComponent_updateBudgetRevisionForValidationError);

      return $q.reject(false);
    }

    function updateBudgetRevisionTachesEnd() {
      $scope.$emit('tachesBudgetComponent.tasksSaved');
      ProgressBar.complete();
      $ctrl.isBusy = false;
    }


    function showServerErrorWithDefaultMessage(error, defaultMessageError) {
      var validationError = ModelStateErrorManager.getErrors(error);
      if (validationError) {
        $ctrl.serverError = validationError;
      }
      else if (error.data.Message) {
        $ctrl.serverError = error.data.Message;
      }
      else {
        $ctrl.serverError = defaultMessageError;
      }
    }

    //////////////////////////////////////////////////////////////////
    // Action -  soumettre le budgetRevision pour validation        //
    //////////////////////////////////////////////////////////////////

    function doVadidBudgetRevision() {
      $ctrl.serverError = '';
      ProgressBar.start();
      saveLastBudgetState();
      $ctrl.bugetRevision.Statut = 1;
      $ctrl.isBusy = true;
      BudgetDataService
          .UpdateBudgetRevision($ctrl.bugetRevision)
          .then(updateBudgetRevisionForDoValidationSuccess)
          .catch(updateBudgetRevisionForDoValidationError)
          .finally(updateBudgetRevisionForDoValidationEnd);
    }

    function updateBudgetRevisionForDoValidationSuccess(response) {
      $ctrl.bugdetInfoChanged({ bugdetInfo: response.data });
      Notify.message($ctrl.resources.TachesBudgetComponent_updateBudgetRevisionForDoValidationSuccess);
    }

    function updateBudgetRevisionForDoValidationError(error) {
      backToLastBudgetState();
      showServerErrorWithDefaultMessage(error, $ctrl.resources.TachesBudgetComponent_updateBudgetRevisionForDoValidationError);

    }

    function updateBudgetRevisionForDoValidationEnd() {
      ProgressBar.complete();
      $ctrl.isBusy = false;
    }

    //////////////////////////////////////////////////////////////////
    // Action - Validation du budgetRevision                        //
    //////////////////////////////////////////////////////////////////

    function vadidBudgetRevision() {
      $ctrl.isBusy = true;
      ProgressBar.start();
      saveLastBudgetState();
      $ctrl.bugetRevision.Statut = 2;
      BudgetDataService
          .UpdateBudgetRevision($ctrl.bugetRevision)
          .then(updateBudgetRevisionForValidationSuccess)
          .catch(updateBudgetRevisionForValidationError)
          .finally(updateBudgetRevisionForValidationEnd);
    }

    function updateBudgetRevisionForValidationSuccess(response) {
      $ctrl.bugdetInfoChanged({ bugdetInfo: response.data });
      Notify.message($ctrl.resources.TachesBudgetComponent_updateBudgetRevisionForValidationSuccess);
    }

    function updateBudgetRevisionForValidationError(error) {
      backToLastBudgetState();
      showServerErrorWithDefaultMessage(error, $ctrl.resources.TachesBudgetComponent_updateBudgetRevisionForValidationError);

    }

    function updateBudgetRevisionForValidationEnd() {
      ProgressBar.complete();
      $ctrl.isBusy = false;
    }

    //////////////////////////////////////////////////////////////////
    // Action - reject du budgetRevision                            //
    //////////////////////////////////////////////////////////////////



    function rejectBudgetRevision() {
      $ctrl.isBusy = true;
      ProgressBar.start();
      saveLastBudgetState();
      $ctrl.bugetRevision.Statut = 0;
      BudgetDataService
          .UpdateBudgetRevision($ctrl.bugetRevision)
          .then(updateBudgetRevisionForDaftSuccess)
          .catch(updateBudgetRevisionForDaftError)
          .finally(updateBudgetRevisionForDaftEnd);
    }

    function updateBudgetRevisionForDaftSuccess(response) {
      $ctrl.bugdetInfoChanged({ bugdetInfo: response.data });
      Notify.message($ctrl.resources.TachesBudgetComponent_updateBudgetRevisionForDaftSuccess);
    }

    function updateBudgetRevisionForDaftError(error) {
      backToLastBudgetState();
      showServerErrorWithDefaultMessage(error, $ctrl.resources.TachesBudgetComponent_updateBudgetRevisionForDaftError);
    }

    function updateBudgetRevisionForDaftEnd() {
      ProgressBar.complete();
      $ctrl.isBusy = false;
    }

    //////////////////////////////////////////////////////////////////
    // HELPERS                           //
    //////////////////////////////////////////////////////////////////

    function saveLastBudgetState() {
      LastBudgetState = $ctrl.bugetRevision.Statut;
    }

    function backToLastBudgetState() {
      $ctrl.bugetRevision.Statut = LastBudgetState;
      LastBudgetState = null;
    }
  }

})();