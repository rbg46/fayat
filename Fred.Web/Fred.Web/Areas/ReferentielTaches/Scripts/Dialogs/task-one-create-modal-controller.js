(function () {
  'use strict';

  angular
    .module('Fred')
    .controller('TaskOneCreateModalController', TaskOneCreateModalController);

  TaskOneCreateModalController.$inject = ['$scope', '$uibModalInstance', 'TachesService', 'Notification', 'TaskManagerService'];

  function TaskOneCreateModalController($scope, $uibModalInstance, TachesService, Notification, TaskManagerService) {

    activate();

    function activate() {
      $scope.isBusy = false;
      $scope.resources = $scope.$resolve.resources;
      $scope.action = resources.ReferencielTache_ModalTache1_Titre_Ajouter;

      $scope.clone = {
        TacheId: 0,
        Active: true,
        Code: '',
        Libelle: '',
        TachesEnfants: [],
        ParentId: null,
        Niveau: 1,
        CiId: $scope.$resolve.ciId
      };
    }

    $scope.save = function () {
      var message = $scope.$resolve.validateTask($scope.clone);
      if (message.length === 0) {
        $scope.isBusy = true;

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
        message: $scope.resources.Global_Notification_Enregistrement_Success,
        title: $scope.resources.Global_Notification_Titre
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