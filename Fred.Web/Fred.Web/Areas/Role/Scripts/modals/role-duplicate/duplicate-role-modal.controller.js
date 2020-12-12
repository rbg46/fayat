(function () {
  "use strict";


  angular.module('Fred').controller("DuplicateRoleModalController", DuplicateRoleModalController);

  DuplicateRoleModalController.$inject = ['$scope', '$uibModalInstance', 'RoleService', 'Notification', 'ModelStateErrorManager','Notify'];

  function DuplicateRoleModalController($scope, $uibModalInstance, RoleService, Notification, ModelStateErrorManager, Notify) {

    $scope.copythreshold = true;
    $scope.copyFeature = true;
    $scope.resources = resources;
    $scope.action = $scope.resources.Role_Index_ModalRole_Titre_Ajouter;
    var clone = { RoleId: 0, CodeNomFamilier: "", Code: "" };
    angular.copy($scope.$resolve.role, clone);
    $scope.clone = clone;
    $scope.clone.Libelle = $scope.clone.Libelle + ' (Copie)';
    $scope.clone.SocieteId = $scope.$resolve.societeId;

    $scope.close = function () {
      $uibModalInstance.dismiss("cancel");
    };

    $scope.save = function () {
      if ($scope.roleModalForm.$valid && !$scope.isBusy) {
        $scope.ErrorMessage = "";
        RoleService.duplicateRole(clone, $scope.copythreshold, $scope.copyFeature)
          .then(duplicateRoleSucess)
          .catch(duplicateRoleFail);
      }
    
    };

    function duplicateRoleSucess(response) {
      $scope.$resolve.roles.push(response.data);
      Notify.message(resources.Global_Notification_Enregistrement_Success);
      $uibModalInstance.dismiss("cancel");
    }

    function duplicateRoleFail(error) {
      var validationError = ModelStateErrorManager.getErrors(error);
      if (validationError) {
        $scope.ErrorMessage = validationError;
      }
      else if (error.data && error.data.Message) {
        $scope.ErrorMessage = error.data.Message;
      }
      else {
        $scope.serverError = resources.Global_Notification_Error;
      }

    }
  }




})();