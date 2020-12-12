(function (angular) {
  'use strict';

  angular.module('Fred').controller('PersonnelEquipeController', PersonnelEquipeController);

  PersonnelEquipeController.$inject = ['$scope', '$q', '$timeout', 'PersonnelService', 'PersonnelEquipeService', 'OuvrierPickerService', 'Notify', 'ProgressBar', 'confirmDialog'];

  function PersonnelEquipeController($scope, $q, $timeout, PersonnelService, PersonnelEquipeService, OuvrierPickerService, Notify, ProgressBar, confirmDialog) {

    var $ctrl = this;
      $ctrl.hasChanges = false;
      $ctrl.listNotEmpty = true;
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //                                                  Initialisation                                                                      //
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    init();

    /**
     * Initialisation du controller.
     * 
     */
    function init() {
      $scope.$on('changeOuvrier', function () { actionAjouterOuvrier(); });
      $ctrl.resources = resources;
      $ctrl.initialOuvrierList = [];
      initHandlers();
      initData();
    }

    /**
     * Initialisation des listes des ouvriers.
     * 
     */
    function initData() {
      if ($ctrl.busy) {
        return;
      }

      $ctrl.busy = true;
      ProgressBar.start();
      $q.when().then(actionGetEquipeOuvrierList);
    }

    /**
     * Initialisation des listes des Ids
     * @param {any} firstLoad true au chargement de la page .
     */
    function initLists(firstLoad) {
      $ctrl.initialOuvrierList = firstLoad ? [] : $ctrl.initialOuvrierList;
      if (firstLoad) {
        $ctrl.initialOuvrierList = [];
        cloneAll($ctrl.ouvrierList, $ctrl.initialOuvrierList);
      } else {
        $ctrl.ouvrierList = [];
        cloneAll($ctrl.initialOuvrierList, $ctrl.ouvrierList);
      }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //                                                  Handlers                                                                      //
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    function initHandlers() {
      $ctrl.handleDeleteOuvrier = function (ouvrierId) {
        actionDeleteOuvrier(ouvrierId);
      };

      $ctrl.handleCancel = function() {
        actionCancel();
      };

      $ctrl.handleSave = function () {
        actionSave();
      }
    }

    return $ctrl;



    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //                                                  Actions                                                                      //
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /*
     * @function action de suppression d'ouvrier
     * @description action de suppression d'ouvrier
     */
    function actionDeleteOuvrier(ouvrierId) {
      confirmDialog.confirm($ctrl.resources, $ctrl.resources.Global_Modal_ConfirmationSuppression)
        .then(function () {
          actionDeleteOuvrierById(ouvrierId);
          $ctrl.hasChanges = true;
          $ctrl.listNotEmpty = $ctrl.ouvrierList.length !== 0;
        });
    }

    /*
     * @function actionGetEquipeOuvrierList()
     * @description Rafraichissement des données : liste des ouvriers
     */
    function actionGetEquipeOuvrierList() {
      PersonnelEquipeService.GetEquipePersonnelsByProprietaireId().$promise.then(function (response) {
        if (response) {
          $ctrl.ouvrierList = response;
          $ctrl.listNotEmpty = $ctrl.ouvrierList.length !== 0;
          $ctrl.busy = false;
          initLists(true);
        }
        ProgressBar.complete();
      })
      .catch(function (error) {
        Notify.error(resources.Global_Notification_Error)
      });
    }

    /*
     * @function actionUpdateEquipe()
     * @description Action de sauvegarde des éléments de l'équipe
     */
    function actionManageEquipe(viewModelEquipe) {
      PersonnelEquipeService.ManageEquipePersonnels(viewModelEquipe).$promise
        .then(function (response) {
          initLists(true);
          $ctrl.hasChanges = false;
          Notify.message(resources.Global_Notification_Enregistrement_Success);
        })
        .catch(function (error) {
          Notify.error(resources.Global_Notification_Error);
          console.log(error);
        });
    }

    /*
     * @function actionAjouterOuvrier()
     * @description Ajout d'un ouvrier
     */
    function actionAjouterOuvrier() {
      var addedOuvrier = OuvrierPickerService.getOuvrier();
      if (!addedOuvrier) {
        Notify.error(resources.Ouvrier_Error_Ajout);
        return;
      }

      if ($ctrl.ouvrierList.some(function (el) { return (el.PersonnelId === addedOuvrier.IdRef) })) {
        Notify.warning(resources.Equipe_Notification_Ouvrier_Existant);
        return;
      }

      if (addedOuvrier) {
        $ctrl.ouvrierList.push({
          PersonnelId: addedOuvrier.IdRef,
          Nom: addedOuvrier.Nom,
          Prenom: addedOuvrier.Prenom,
          Matricule: addedOuvrier.Matricule,
          IsInterimaire: addedOuvrier.IsInterimaire,
          IsInterne: addedOuvrier.IsInterne,
          InternStatus: addedOuvrier.IsInterimaire ? resources.Personnel_Interimaire : (addedOuvrier.IsInterne ? resources.Personnel_Interne : '')
        });
        $ctrl.hasChanges = true;
          $ctrl.listNotEmpty = $ctrl.ouvrierList.length !== 0;

      }
    }

    /*
     * @function delete ouvrier by id
     * @description Supprimer un ouvrier par son Id
     */
    function actionDeleteOuvrierById(ouvrierId) {
      if (ouvrierId && $ctrl.ouvrierList) {
        $ctrl.ouvrierList = $ctrl.ouvrierList.filter(function (el) {
          return el.PersonnelId !== ouvrierId;
        });
      }
    }

    /*
     * @function actionCancel
     * @description Action de d'annulation générique
     */
    function actionCancel() {
      if (!$ctrl.hasChanges) {
        return;
      }
      confirmDialog.confirm($ctrl.resources, $ctrl.resources.Global_Modal_ConfirmationAnnulation)
        .then(function () {
          initLists(false);
          $ctrl.hasChanges = false;
        });
    }

    /*
    * @function action save équipe
    * @description Action de la sauvegarde de l'équipe
    */
    function actionSave() {
      if (!$ctrl.hasChanges) {
        return;
      }

      var ouvrierListId = $ctrl.ouvrierList.map(function (value, index) { return Number(value.PersonnelId) });
      var initialOuvrierListId = $ctrl.initialOuvrierList.map(function (value, index) { return Number(value.PersonnelId) });

      var ouvrierListToAdd = ouvrierListId.filter(function (el) {
        return (initialOuvrierListId.indexOf(el) < 0);
      });

      var ouvrierListToDelete = initialOuvrierListId.filter(function (el) {
        return (ouvrierListId.indexOf(el) < 0);
      });

      var viewModelEquipe = {
        OuvrierListIdToAdd: ouvrierListToAdd,
        OuvrierListIdToDelete: ouvrierListToDelete
      }

      actionManageEquipe(viewModelEquipe);
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //                                                  General                                                                      //
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /*
     * @function cloneAll(source,target)
     * @description Recopie complète d'un objet
     * @param {any} source 
     * @param {any} target
     */
    function cloneAll(source, target) {
      for (var property in source) {
        if (source.hasOwnProperty(property)) {
          target[property] = angular.copy(source[property]);
        }
      }
    }
  }
}(angular));