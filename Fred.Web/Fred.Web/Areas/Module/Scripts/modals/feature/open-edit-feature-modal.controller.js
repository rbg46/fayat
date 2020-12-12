(function () {
  "use strict";

  angular.module('Fred').controller('openEditFeatureModalController', openEditFeatureModalController);

  openEditFeatureModalController.$inject = ['$scope', '$uibModalInstance', 'ModuleService', 'Notify'];

  function openEditFeatureModalController($scope, $uibModalInstance, ModuleService, Notify) {
    $scope.isBusy = false;
    $scope.resources = resources;
    $scope.action = $scope.resources.Module_Service_ModalFonctionnalite_Titre_Modifier;
    $scope.moduleList = [$scope.$resolve.selectedModule];
    $scope.modalSelectedModule = $scope.$resolve.selectedModule;
    var clone = {};
    angular.copy($scope.$resolve.feature, clone);
    $scope.clone = clone;

    $scope.save = function () {
      if ($scope.featureForm.$valid && !$scope.isBusy) {
        $scope.ErrorMessage = "";
        $scope.isBusy = true;

        ModuleService.updateFeature($scope.clone)
          .then(function (response) {
            $uibModalInstance.close(response.data);
            Notify.message(resources.Global_Notification_Enregistrement_Success);
          })
          .catch(function (error) {
            $scope.ErrorMessage = error.data.Message;
          })
         .finally(function () {
           $scope.isBusy = false;
         });
      }
    };

    $scope.close = function () {
      $uibModalInstance.dismiss("cancel");
    };
  }


})();