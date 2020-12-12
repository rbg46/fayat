(function () {
  "use strict";

  angular.module('Fred').controller('openCreateModuleModalController', openCreateModuleModalController);

  openCreateModuleModalController.$inject = ['$scope', '$uibModalInstance', '$sce', 'ModuleService', 'Notify'];

  function openCreateModuleModalController($scope, $uibModalInstance, $sce, ModuleService, Notify) {
    $scope.isBusy = false;
    $scope.resources = resources;
    $scope.moduleList = $scope.$resolve.moduleList;
    $scope.action = $scope.resources.Module_Service_ModalModule_Titre_Ajouter;
    $scope.clone = { ModuleId: 0, Libelle: "", Code: "" };

    $scope.save = function () {
      if ($scope.moduleForm.$valid && !$scope.isBusy) {
        $scope.ErrorMessage = "";
        $scope.isBusy = true;

        // Enregistrement en base
        if ($scope.ErrorMessage.length === 0) {
          ModuleService.addModule($scope.clone)
            .then(function (response) {
              var newModule = response.data;             
              $scope.moduleList.push(newModule);
              $uibModalInstance.close(newModule);
              Notify.message(resources.Global_Notification_Enregistrement_Success);
            })
            .catch(function (error) {
              $scope.ErrorMessage = error.data.Message;
            }).finally(function () {
              $scope.isBusy = false;
            });
        }
      }
    };


    $scope.close = function () {
      $uibModalInstance.dismiss("cancel");
    };
  }

})();