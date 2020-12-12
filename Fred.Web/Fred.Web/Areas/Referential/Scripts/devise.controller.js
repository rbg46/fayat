(function (angular) {
  'use strict';

  angular.module('Fred').controller('DeviseController', DeviseController);

  DeviseController.$inject = ['$scope', 'Notify', 'DeviseService', 'ProgressBar', 'confirmDialog'];

  function DeviseController($scope, Notify, DeviseService, ProgressBar, confirmDialog) {
      
    // Instanciation Objet Ressources
    $scope.resources = resources;

    // Instanciation de la recherche
    $scope.recherche = "";


    // Attribut d'affichage de la liste
    $scope.checkDisplayOptions = "close-right-panel";
    $scope.checkFormatOptions = "small";

    $scope.isAlreadyUsed = false;
    $scope.isBusy = false;

    init();
    
    function init() {
      // Chargement des données
      actionInitSearch();
      actionLoad(true);
      actionNewDevise();
    }
  

   

    //////////////////////////////////////////////////////////////////
    // Handlers                                                     //
    //////////////////////////////////////////////////////////////////

    // Handler de sélection d'une ligne de le repeater Angular
    $scope.handleSelect = function (item) {
      $scope.devise = angular.copy(item);
      $scope.checkDisplayOptions = "open";
      $scope.changeFormModel = false;
      $scope.isAlreadyUsed = false;
      ProgressBar.start();
      $scope.isBusy = true;
      DeviseService.isAlreadyUsed($scope.devise.DeviseId)
                            .then((response) => {
                              $scope.isAlreadyUsed = response.data.isAlreadyUsed;
                            }).catch((error) => {
                              Notify.defaultError();
                            }).finally(() => {
                              $scope.isBusy = false;
                              ProgressBar.complete();
                            });
    };

    // Handler de click sur le bouton ajouter
    $scope.handleClickCreateNew = function () {
      $scope.isAlreadyUsed = false;
      $scope.formDevise.$setPristine();
      $scope.formDevise.IsoCode.$setValidity('exist', true);
      actionNewDevise();
      $scope.checkDisplayOptions = "open";
      $scope.changeFormModel = false;
    };

    // Handler de click sur le bouton Enregistrer
    $scope.handleClickAddOrUpdate = function () {
      if ($scope.isAlreadyUsed) {
        confirmDialog.confirm($scope.resources, $scope.resources.Global_Modal_Item_Already_Used)
                     .then(function () {
                       $scope.actionAddOrUpdate(false, true);
                     });
      } else {
        $scope.actionAddOrUpdate(false, true);
      }

    };

    // Handler de click sur le bouton Enregistrer et Nouveau
    $scope.handleClickAddOrUpdateAndNew = function () {
      if ($scope.isAlreadyUsed) {
        confirmDialog.confirm($scope.resources, $scope.resources.Global_Modal_Item_Already_Used)
                     .then(function () {
                       $scope.actionAddOrUpdate(true, true);
                     });
      }
      else {
        $scope.actionAddOrUpdate(true, true);
      }

    };
    // Handler de click sur le bouton Cancel
    $scope.handleClickCancel = function () {
      $scope.actionCancel();
    };

    // Handler de frappe clavier dans le champs recherche
    $scope.handleSearch = function (recherche) {
      $scope.recherche = recherche;
      actionLoad();
    };
    // Handler de frappe clavier dans le champs code
    $scope.handleChangeCode = function () {
      if (!$scope.formDevise.IsoCode.$error.pattern) {
        var idCourant;

        if ($scope.devise.DeviseId !== undefined)
          idCourant = $scope.devise.DeviseId;
        else
          idCourant = 0;

        $scope.existCodeDevise(idCourant, $scope.devise.IsoCode);
      }
    };

    //////////////////////////////////////////////////////////////////
    // Actions                                                      //
    //////////////////////////////////////////////////////////////////

    // Action click sur les boutons Enregistrer
    $scope.actionAddOrUpdate = function (newItem, withNotif) {
      if ($scope.formDevise.$invalid)
        return;
      if ($scope.devise.DeviseId === 0)
        $scope.actionCreate(newItem, withNotif);
      else
        $scope.actionUpdate(newItem, withNotif);
    };

    // Action Create
    $scope.actionCreate = function (newItem, withNotif) {
      ProgressBar.start();
      DeviseService.Create($scope.devise).then(function () {
        actionLoad(false);
        if (newItem) {
          $scope.handleClickCreateNew();
        }
        else {
          $scope.actionCancel();
        }
        ProgressBar.complete();
        if (withNotif) $scope.sendNotification(resources.Global_Notification_Enregistrement_Success);
      }, function (reason) {
        ProgressBar.complete();
        if (withNotif) $scope.sendNotificationError(resources.Global_Notification_Error);
      });
    };

    // Action Update
    $scope.actionUpdate = function (newItem, withNotif) {
      ProgressBar.start();
      DeviseService.Update($scope.devise).then(function () {
        actionLoad(false);
        if (newItem) {
          $scope.handleClickCreateNew();
        }
        else {
          $scope.actionCancel();
        }
        ProgressBar.complete();
        if (withNotif) $scope.sendNotification(resources.Global_Notification_Enregistrement_Success);
      }, function (reason) {
        ProgressBar.complete();
        if (withNotif) $scope.sendNotificationError(resources.Global_Notification_Error);
      });
    };

    // Action Cancel
    $scope.actionCancel = function () {
      $scope.checkDisplayOptions = "close-right-panel";
      $scope.formDevise.$setPristine();
      $scope.formDevise.IsoCode.$setValidity('exist', true);
    };

    // Action Delete
    $scope.handleClickDelete = function (devise) {
      $scope.checkDisplayOptions = "close-right-panel";
      confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationSuppression).then(function () {
        DeviseService.Delete(devise).then(function () {
          actionLoad(false);
          $scope.actionCancel(true);
        }, function (reason) {
          $scope.sendNotificationError(resources.Global_Notification_Suppression_Error);
        });
      });
    };

    // Action initalisation d'une nouvelle devise
    function actionNewDevise() {
      DeviseService.New().then(function (response) {
        $scope.devise = response.data;
      }, function (reason) {
      });
    }

    // Action Load
    function actionLoad(withNotif) {
      ProgressBar.start();
      DeviseService.Search($scope.filters, $scope.recherche).then(function (response) {
        $scope.buildGrid(response.data);
        if (response && response.data && response.length === 0) {
          $scope.sendNotificationError(resources.Global_Notification_AucuneDonnees);
        }
      }, function (reason) {
        if (withNotif) $scope.sendNotificationError(resources.Global_Notification_Error);
      });
      ProgressBar.complete();
    }

    // Action d'initialisation de la recherche muli-critère des Devises
    function actionInitSearch() {
      $scope.filters = { IsoCode: true, Libelle: true };
    }

    // Action de test d'existence de code nature
    $scope.existCodeDevise = function (idCourant, codeDevise) {
      DeviseService.Exists(idCourant, codeDevise).then(function (response) {
        if (response.data) {
          $scope.formDevise.IsoCode.$setValidity('exist', false);
        } else {
          $scope.formDevise.IsoCode.$setValidity('exist', true);
        }

      }, function (reason) {

      });
    };

    //////////////////////////////////////////////////////////////////
    // Gestion diverses                                             //
    //////////////////////////////////////////////////////////////////


    // Construction de la grille
    $scope.buildGrid = function (itemsCollection) {
      $scope.items = itemsCollection;
    };

    // Gestion des notifications de succes
    $scope.sendNotification = function (message) {
      Notify.message(message);
    };

    // Gestion des notifications d'erreur
    $scope.sendNotificationError = function (message) {
      Notify.error(message);
    };
  }



})(angular);