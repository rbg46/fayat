(function () {
  'use strict';

  angular
    .module('Fred')
    .controller('TaskFourCreateModalController', TaskFourCreateModalController);

  TaskFourCreateModalController.$inject = ['$scope', '$uibModalInstance', 'TachesService', 'Notification', '$filter', 'TaskManagerService'];

  function TaskFourCreateModalController($scope, $uibModalInstance, TachesService, Notification, $filter, TaskManagerService) {

    activate();

    function activate() {
      $scope.isBusy = false;
      $scope.resources = $scope.$resolve.resources;
      $scope.action = $scope.$resolve.resources.ReferencielTache_ModalTache4_Titre_Ajouter;

      $scope.clone = { TacheId: 0, Active: true, Libelle: '', Niveau: 4, CiId: $scope.$resolve.ciId, TachesEnfants: [] };
      $scope.clone.ParentId = $scope.$resolve.selectedTache3.ParentId;
      selectTask($scope.$resolve.selectedTache3);
    }

    // Evénement OnChange de la dropdownlist Tâche 1 : Recalcule du nouveau code
    $scope.getSelectedTask1 = function () {
      selectTask($scope.dropdownTache1Data.selected);
    };

    // Evénement OnChange de la dropdownlist Tâche 2 : Recalcule le nouveau code
    $scope.getSelectedTask2 = function () {
      selectTask($scope.dropdownTache2Data.selected);
    };

    // Evénement OnChange de la dropdownlist Tâche 3 : Recalcule le nouveau code
    $scope.getSelectedTask3 = function () {
      selectTask($scope.dropdownTache3Data.selected);
    };

    function selectTask(taskSelected) {
      var options = TaskManagerService.getOptions(taskSelected);
      $scope.dropdownTache1Data = options.levelOne;
      $scope.dropdownTache2Data = options.levelTwo;
      $scope.dropdownTache3Data = options.levelThree;
      if ($scope.dropdownTache3Data.selected !== null) {
        TachesService.getNextTaskCode($scope.dropdownTache3Data.selected)
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
      var message = $scope.$resolve.validateTask($scope.clone, $scope.dropdownTache1Data, $scope.dropdownTache2Data, $scope.dropdownTache3Data);
      if (message.length === 0 && $scope.dropdownTache3Data.selected !== null) {
        $scope.isBusy = true;
        $scope.clone.ParentId = $scope.dropdownTache3Data.selected.TacheId;
        TachesService.addTache($scope.clone)
          .then(saveSuccess)
          .catch(saveError)
          .finally(saveFinally);
      }
      else {
        $scope.ErrorMessage = message;
      }
    };

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

  }
})();