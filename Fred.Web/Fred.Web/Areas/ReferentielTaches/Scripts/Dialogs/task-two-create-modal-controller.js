(function () {
  'use strict';

  angular
    .module('Fred')
    .controller('taskTwoCreateModalController', taskTwoCreateModalController);

  taskTwoCreateModalController.$inject = ['$scope', '$uibModalInstance', 'TachesService', 'Notification', 'TaskManagerService'];

  function taskTwoCreateModalController($scope, $uibModalInstance, TachesService, Notification, TaskManagerService) {

    activate();

    function activate() {
      $scope.isBusy = false;

      $scope.resources = $scope.$resolve.resources;
      $scope.action = $scope.$resolve.resources.ReferencielTache_ModalTache2_Titre_Ajouter;

      var options = TaskManagerService.getOptions($scope.$resolve.selectedTask1);
      $scope.dropdownTache1Data = options.levelOne;

      $scope.clone = { TacheId: 0, Active: true, Libelle: '', Code: '', Niveau: 2, CiId: $scope.$resolve.ciId, TachesEnfants: [] };
      $scope.clone.ParentId = $scope.dropdownTache1Data.selected.TacheId;

      selectTask($scope.$resolve.selectedTask1);
    }

    // Evénement OnChange de la dropdownlist Tâche 1 : Recalcule du nouveau code
    $scope.getSelectedTask1 = function () {
      selectTask($scope.dropdownTache1Data.selected);
    };

    function selectTask(taskSelected) {
      var options = TaskManagerService.getOptions(taskSelected);
      $scope.dropdownTache1Data = options.levelOne;

      if ($scope.dropdownTache1Data.selected !== null) {

        TachesService.getNextTaskCode($scope.dropdownTache1Data.selected)
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
      var message = $scope.$resolve.validateTask($scope.clone, $scope.dropdownTache1Data);

      if (message.length === 0) {
        $scope.isBusy = true;
        $scope.clone.ParentId = $scope.dropdownTache1Data.selected.TacheId;
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
        message: resources.Global_Notification_Enregistrement_Success,
        title: resources.Global_Notification_Titre
      });
    }

    /*
    * Action executée lorsque la sauvegarde echoue.
    */
    function saveError(error) {
      if (error.status === 409) {
        $scope.ErrorMessage = resources.Global_Control_Code_Exist_Erreur;
      }
      else {
        Notification.error({
          message: resources.Global_Notification_Error,
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