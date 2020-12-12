(function () {
  "use strict";


  angular.module('Fred').controller('openEditModuleModalController', openEditModuleModalController);

  openEditModuleModalController.$inject = ['$scope', '$uibModalInstance', 'ModuleService', 'Notify', '$q','ProgressBar'];

  function openEditModuleModalController($scope, $uibModalInstance, ModuleService, Notify, $q, ProgressBar) {

    $scope.resources = resources;
    var societeId = null;
    $scope.clone = {};
    $scope.ErrorMessage = "";
    $scope.isBusy = false;
    $scope.isEditionMode = true;

    init();


    /* -------------------------------------------------------------------------------------------------------------
    *                                            INIT
    * -------------------------------------------------------------------------------------------------------------
    */

    function init() {
      $scope.action = $scope.resources.Module_Service_ModalModule_Titre_Modifier;
      societeId = $scope.$resolve.societeSelectedId;
      angular.copy($scope.$resolve.selectedModule, $scope.clone);
    }    

    /* -------------------------------------------------------------------------------------------------------------
    *                                            SAUVEGARDE DU MODULE
    * -------------------------------------------------------------------------------------------------------------
    */

    $scope.save = function () {
      if ($scope.moduleForm.$valid && !$scope.isBusy) {  
        $scope.ErrorMessage = "";
        $scope.isBusy = true;

        // Validation
        if (!$scope.clone.Libelle || $scope.clone.Libelle.length === 0) {
          $scope.ErrorMessage += $scope.resources.Module_Service_Control_LibelleRequis_Erreur + ". ";
          return;
        }
        ProgressBar.start();
        var resquests = [];
        // Enregistrement en base
        var resquestUpdateModule = ModuleService.updateModule($scope.clone)
                                                .then(updateModuleSuccess);
        resquests.push(resquestUpdateModule);
        if ($scope.clone.isActivedForSociete !== $scope.$resolve.selectedModule.isActivedForSociete) {
          var requestForEnableOrDisableModule = createRequestForEnableOrDisableModuleForSociete();
          resquests.push(requestForEnableOrDisableModule);
        }

        $q.all(resquests)
          .then(allRequestSuccess)
          .catch(allRequestFail)
          .finally(allRequestFinally);
      }
    
    };

    /*
     * Creer une promise de desactivation ou d'activation d'un module pour une societe
     */
    function createRequestForEnableOrDisableModuleForSociete() {
      var request = null;
      if ($scope.clone.isActivedForSociete) {
        request = ModuleService.enableModuleForSocieteId($scope.clone.ModuleId, societeId);
      } else {
        request = ModuleService.disableModuleForSoceiteId($scope.clone.ModuleId, societeId);
      }
      return request;
    }

    function updateModuleSuccess(response) {
      angular.extend($scope.$resolve.selectedModule, response.data);
      return response.data;
    }

    function allRequestSuccess(response) {
      var updatedModule = response[0];    
      Notify.message(resources.Global_Notification_Enregistrement_Success);
      $uibModalInstance.close(updatedModule);
    }

    function allRequestFail(error) {     
      $scope.ErrorMessage = error.data.Message;
    }

    function allRequestFinally() {
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
  }



})();