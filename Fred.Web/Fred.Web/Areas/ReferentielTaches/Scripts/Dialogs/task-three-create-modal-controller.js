(function () {
  'use strict';

  angular
    .module('Fred')
    .controller('TaskThreeCreateModalController', TaskThreeCreateModalController);

  TaskThreeCreateModalController.$inject = ['$scope', '$uibModalInstance', 'TachesService', 'Notification', '$filter', 'TaskManagerService', 'confirmDialog'];

  function TaskThreeCreateModalController($scope, $uibModalInstance, TachesService, Notification, $filter, TaskManagerService, confirmDialog) {

    activate();

    function activate() {
      $scope.isBusy = false;
      $scope.isCreate = true;

      $scope.resources = $scope.$resolve.resources;
      $scope.action = $scope.$resolve.resources.ReferencielTache_ModalTache3_Titre_Ajouter;

      $scope.clone = {
        TacheId: 0,
        Active: true,
        Libelle: '',
        Niveau: 3,
        CiId: $scope.$resolve.ciId,
        TachesEnfants: []
      };
      $scope.clone.ParentId = $scope.$resolve.selectedTache2.ParentId;
      $scope.clone.TacheParDefaut = false;
      selectTask($scope.$resolve.selectedTache2);

    }

    // Evénement OnChange de la dropdownlist Tâche 1 : Recalcule du nouveau code
    $scope.getSelectedTask1 = function () {
      selectTask($scope.dropdownTache1Data.selected);
    };

    // Evénement OnChange de la dropdownlist Tâche 2 : Recalcule le nouveau code
    $scope.getSelectedTask2 = function () {
      selectTask($scope.dropdownTache2Data.selected);
    };

    function selectTask(taskSelected) {
      var options = TaskManagerService.getOptions(taskSelected);
      $scope.dropdownTache1Data = options.levelOne;
      $scope.dropdownTache2Data = options.levelTwo;

      if ($scope.dropdownTache2Data.selected !== null) {
        TachesService.getNextTaskCode($scope.dropdownTache2Data.selected)
          .then(function (response) {
            $scope.clone.Code = response.data;
          })
          .catch(function (error) {
            Notification.error({
              message: error.data.Message,
              positionY: 'bottom', positionX: 'right'
            });
          });
      }
    }

    $scope.save = function () {
      var message = $scope.$resolve.validateTask($scope.clone, $scope.dropdownTache1Data, $scope.dropdownTache2Data);
      if (message.length === 0) {
        // Si la tâche est devenue celle par défaut il faut faire valider par l'utilisateur
        if ($scope.clone.TacheParDefaut) {
          var currentDefaultTask = TaskManagerService.getDefaultTask(null);
          if (currentDefaultTask) {
            confirmDialog.confirm(resources, format($scope.resources.ReferencielTache_ModalTache3_ConfirmDefaultTacheChange, currentDefaultTask.Code, currentDefaultTask.Libelle, $scope.clone.Code, $scope.clone.Libelle)).then(function () {
              reallySave();
            });
          }
          else {
            reallySave();
          }
        }
        else {
          reallySave();
        }
      }
      else {
        $scope.ErrorMessage = message;
      }
    };

    function reallySave() {
      $scope.isBusy = true;
      $scope.clone.ParentId = $scope.dropdownTache2Data.selected.TacheId;
      TachesService.addTache($scope.clone)
        .then(saveSuccess)
        .catch(saveError)
        .finally(saveFinally);
    }

    /*
    * Action executée lorsque la sauvegarde reussie.
    */
    function saveSuccess(response) {
      $uibModalInstance.close(response.data);
      Notification({
        message: $scope.$resolve.resources.Global_Notification_Enregistrement_Success,
        title: $scope.$resolve.resources.Global_Notification_Titre
      });
    }

    /*
    * Action executée lorsque la sauvegarde echoue.
    */
    function saveError(error) {
      if (error.status === 409) {
        $scope.ErrorMessage = $scope.$resolve.resources.Global_Control_Code_Exist_Erreur;
      }
      else {
        Notification.error({
          message: $scope.$resolve.resources.Global_Notification_Error,
          positionY: 'bottom', positionX: 'right'
        });
      }
    }

    /*
    * Action executée à la finde la sauvegarde.
    */
    function saveFinally() {
      $scope.isBusy = false;
    }

    $scope.close = function () {
      $uibModalInstance.dismiss('cancel');
    };


    // NPI : à mettre dans un service...
    function format(format) {
      var args = Array.prototype.slice.call(arguments, 1);
      return format.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] !== 'undefined'
          ? args[number]
          : match
          ;
      });
    }
  }
})();