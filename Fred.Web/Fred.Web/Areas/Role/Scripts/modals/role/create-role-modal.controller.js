(function () {
  "use strict";


  angular.module('Fred').controller("CreateRoleModalController", CreateRoleModalController);

  CreateRoleModalController.$inject = ['$scope', '$uibModalInstance', 'RoleService', 'Notification', 'ModelStateErrorManager', 'roleProviderService', 'ProgressBar'];

  function CreateRoleModalController($scope, $uibModalInstance, RoleService, Notification, ModelStateErrorManager, roleProviderService, ProgressBar) {



    /* -------------------------------------------------------------------------------------------------------------
    *                                            INIT
    * -------------------------------------------------------------------------------------------------------------
    */
    $scope.toggleRole = true;
    $scope.isBusy = false;
    $scope.resources = resources;
    $scope.action = $scope.resources.Role_Index_ModalRole_Titre_Ajouter;
    var clone = { RoleId: 0, Libelle: "", CodeNomFamilier: "", Code: "", Actif: true };
    $scope.clone = clone;
    $scope.clone.SocieteId = $scope.$resolve.societeId;
    $scope.roles = roleProviderService.getRoles();




    /* -------------------------------------------------------------------------------------------------------------
    *                                            SAUVEGARDE
    * -------------------------------------------------------------------------------------------------------------
    */

    $scope.save = function () {
      if ($scope.roleModalForm.$valid && !$scope.isBusy) {
        $scope.ErrorMessage = "";
        ProgressBar.start();
        $scope.isBusy = true;

        RoleService.addRole($scope.clone)
          .then(addRoleSucess)
          .catch(addRoleFail)
          .finally(addRoleFinally);
      }
    };

    function addRoleSucess(response) {
      $scope.$resolve.items.push(response.data);
      $uibModalInstance.close();
      Notification({ message: resources.Global_Notification_Enregistrement_Success, title: resources.Global_Notification_Titre });
    }

    function addRoleFail(error) {
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

    function addRoleFinally() {
      $scope.isBusy = false;
      ProgressBar.complete();

    }

    /* -------------------------------------------------------------------------------------------------------------
    *                                            SELECTION ROLE SYSTEME
    * -------------------------------------------------------------------------------------------------------------
    */

    $scope.roleChanged = function () {
      $scope.clone.Code = $scope.roleSelected;
    }


    /* -------------------------------------------------------------------------------------------------------------
    *                                            FERMETURE
    * -------------------------------------------------------------------------------------------------------------
    */
    $scope.close = function () {
      $uibModalInstance.dismiss("cancel");
    };

  }




})();