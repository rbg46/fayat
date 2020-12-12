(function () {
  "use strict";

  angular.module('Fred').controller('openCreateFeatureModalController', openCreateFeatureModalController);

  openCreateFeatureModalController.$inject = ['$scope', '$uibModalInstance', 'ModuleService', 'Notify'];

  function openCreateFeatureModalController($scope, $uibModalInstance, ModuleService, Notify) {
    $scope.isBusy = false;
    $scope.resources = resources;
    $scope.moduleForm = {};
    $scope.action = $scope.resources.Module_Service_ModalFonctionnalite_Titre_Ajouter;
    $scope.moduleList = $scope.$resolve.moduleList;
    $scope.modalSelectedModule = $scope.$resolve.selectedModule;
    $scope.clone = { FonctionnaliteId: 0, ModuleId: $scope.modalSelectedModule.ModuleId, Code: "", Libelle: "", HorsOrga: false };
    $scope.featureList = $scope.$resolve.featureList;
    $scope.save = function () {
      if ($scope.featureForm.$valid && !$scope.isBusy) {
        $scope.ErrorMessage = "";
        $scope.isBusy = true;

        ModuleService.addFeature($scope.clone)
          .then(function (response) {
            $uibModalInstance.close(response.data);
            Notify.message(resources.Global_Notification_Enregistrement_Success);
          })
          .catch(function (error) {
              if (error.data.ModelState && error.data.ModelState.Code.length > 0) {
                  for (var i = 0; i < error.data.ModelState.Code.length; i++) {
                      if (i > 0) {
                          $scope.ErrorMessage += '\n';
                      }
                      $scope.ErrorMessage += error.data.ModelState.Code[i];
                  }
              }
              else {
                  $scope.ErrorMessage = error.data.Message;
              }            
          })
          .finally(function () {
            $scope.isBusy = false;
          });
      }

    };


    $scope.getSelectedModule = function () {
      $scope.clone.ModuleId = $scope.modalSelectedModule.ModuleId;
    };

    $scope.close = function () {
      $uibModalInstance.dismiss("cancel");
    };
  }



})();