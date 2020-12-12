(function () {
  'use strict';

  angular
    .module('Fred')
    .controller('TaskFourEditModalController', TaskFourEditModalController);

  TaskFourEditModalController.$inject = ['$scope', '$uibModalInstance', 'TachesService', 'Notification', '$filter', 'TaskManagerService'];

  function TaskFourEditModalController($scope, $uibModalInstance, TachesService, Notification, $filter, TaskManagerService) {

    activate();

    function activate() {
      $scope.isBusy = false;
      $scope.resources = $scope.$resolve.resources;
      $scope.action = $scope.$resolve.resources.ReferencielTache_ModalTache4_Titre_Modifier;

      var options = TaskManagerService.getOptions($scope.$resolve.editedTask4);
      $scope.dropdownTache1Data = options.levelOne;
      $scope.dropdownTache2Data = options.levelTwo;
      $scope.dropdownTache3Data = options.levelThree;

      var clone = {};
      angular.copy($scope.$resolve.editedTask4, clone);
      $scope.clone = clone;
    }

    $scope.save = function () {
      var message = $scope.$resolve.validateTask($scope.clone, $scope.dropdownTache1Data, $scope.dropdownTache2Data, $scope.dropdownTache3Data);
      if (message.length === 0) {
        $scope.isBusy = true;
        TachesService.updateTache($scope.clone)
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
        Notification.error({ message: $scope.$resolve.resources.Global_Notification_Error, positionY: 'top', positionX: 'right' });
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