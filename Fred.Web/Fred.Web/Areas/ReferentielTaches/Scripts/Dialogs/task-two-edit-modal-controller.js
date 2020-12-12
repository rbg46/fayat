(function () {
  'use strict';

  angular
    .module('Fred')
    .controller('taskTwoEditModalController', taskTwoEditModalController);

  taskTwoEditModalController.$inject = ['$scope', '$uibModalInstance', 'TachesService', 'Notification', 'TaskManagerService'];

  function taskTwoEditModalController($scope, $uibModalInstance, TachesService, Notification, TaskManagerService) {

    activate();

    function activate() {
      $scope.isBusy = false;
      $scope.resources = $scope.$resolve.resources;
      $scope.action = $scope.$resolve.resources.ReferencielTache_ModalTache2_Titre_Modifier;

      var options = TaskManagerService.getOptions($scope.$resolve.editedTask2);
      $scope.dropdownTache1Data = options.levelOne;

      var clone = {};
      angular.copy($scope.$resolve.editedTask2, clone);
      $scope.clone = clone;
    }

    $scope.save = function () {
      var message = $scope.$resolve.validateTask($scope.clone, $scope.dropdownTache1Data);

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
      Notification({ message: resources.Global_Notification_Enregistrement_Success, title: resources.Global_Notification_Titre });
    }

    /*
    * Action executée lorsque la sauvegarde echoue.
    */
    function saveError(error) {
      if (error.status === 409) {
        $scope.ErrorMessage = resources.Global_Control_Code_Exist_Erreur;
      }
      else {
        Notification.error({ message: resources.Global_Notification_Error, positionY: 'bottom', positionX: 'right' });
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