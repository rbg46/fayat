(function () {
  "use strict";

  angular.module('Fred').controller("EditRoleModalController", EditRoleModalController);

  EditRoleModalController.$inject = ['$scope', '$uibModalInstance', 'RoleService', 'Notification', 'roleProviderService', 'ProgressBar'];

  function EditRoleModalController($scope, $uibModalInstance, RoleService, Notification, roleProviderService, ProgressBar) {

    /* -------------------------------------------------------------------------------------------------------------
    *                                            INIT
    * -------------------------------------------------------------------------------------------------------------
    */
    $scope.resources = resources;
    $scope.action = $scope.resources.Role_Index_ModalRole_Titre_Modifier;
    var clone = {};

    init();

    function init() {

      angular.copy($scope.$resolve.item, clone);
      $scope.clone = clone;
      $scope.clone.SocieteId = $scope.$resolve.societeId;
      $scope.roles = roleProviderService.getRoles();
      var codesFilter = $scope.roles.filter(function (code) {
        return code === $scope.clone.Code;
      });
      $scope.roleSelected = codesFilter && codesFilter.length > 0 ? codesFilter[0] : '';
    }

    /* -------------------------------------------------------------------------------------------------------------
    *                                            SAUVEGARDE
    * -------------------------------------------------------------------------------------------------------------
    */
    $scope.save = function () {
      if ($scope.roleModalForm.$valid && !$scope.isBusy) {
        $scope.ErrorMessage = "";
        ProgressBar.start();
        $scope.isBusy = true;
        RoleService.updateRole($scope.clone)
          .then(updateRoleSucess)
          .catch(updateRoleFail)
         .finally(updateRoleFinally);
      }
    }


    function updateRoleSucess(response) {
      angular.extend($scope.$resolve.item, clone);
      $uibModalInstance.close($scope.$resolve.item);
      Notification({ message: resources.Global_Notification_Enregistrement_Success, title: resources.Global_Notification_Titre });
    }

    function updateRoleFail(restriction) {
      if (restriction.status === 404) {
        $scope.ErrorMessage = "Code n'existe pas!";
      }
      else if (restriction.status === 400) {
        $scope.ErrorMessage = resources.Role_Service_Control_Alphanumeric_Erreur;
      }
      else {
        $scope.ErrorMessage = restriction.data.Message;
      }
    }


    function updateRoleFinally() {
      $scope.isBusy = false;
      ProgressBar.complete();
    }




    /* -------------------------------------------------------------------------------------------------------------
    *                                            FERMETURE
    * -------------------------------------------------------------------------------------------------------------
    */
    $scope.close = function () {
      $uibModalInstance.dismiss("cancel");
    };


  };


})();