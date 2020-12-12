
(function () {
  "use strict";


  angular.module('Fred').controller("CreateRoleFonctionnaliteModalController", CreateRoleFonctionnaliteModalController);

  CreateRoleFonctionnaliteModalController.$inject = ['$scope', '$uibModalInstance', 'RoleService', 'Notify', 'ModelStateErrorManager', 'ProgressBar','fonctionnaliteModeService'];

  function CreateRoleFonctionnaliteModalController($scope, $uibModalInstance, RoleService, Notify, ModelStateErrorManager, ProgressBar, fonctionnaliteModeService) {

    var $ctrl = this;
    $ctrl.resources = resources;
    $ctrl.modules = [];
    $ctrl.moduleSelected = null;
    $ctrl.features = [];
    $ctrl.featureSelected = null;
    $ctrl.modeSelected = null;

    $ctrl.modes = fonctionnaliteModeService.getModes();

    var societeId = $scope.$resolve.societeId;
    var roleId = $scope.$resolve.selectedRoleId;

    init();

    /* -------------------------------------------------------------------------------------------------------------
    *                                            INIT
    * -------------------------------------------------------------------------------------------------------------
    */


    function init() {
      $ctrl.modeSelected = $ctrl.modes[0];
      ProgressBar.start();
      RoleService.getModulesAvailablesForSocieteId(societeId)
                  .then(getModulesAvailablesForSocieteIdSuccessed)
                  .catch(getModulesAvailablesForSocieteIdFail)
                  .finally(getModulesAvailablesForSocieteIdFinally);


    }


    function getModulesAvailablesForSocieteIdSuccessed(response) {
      $ctrl.modules = response.data;
    }

    function getModulesAvailablesForSocieteIdFail(error) {
      Notify.error($ctrl.resources.Global_Notification_Error);
    }

    function getModulesAvailablesForSocieteIdFinally() {
      ProgressBar.complete();
    }

    /* -------------------------------------------------------------------------------------------------------------
   *                                            SELECTION D UN MODULE
   * -------------------------------------------------------------------------------------------------------------
   */

    $ctrl.selectModule = function () {
      ProgressBar.start();
      RoleService.getFonctionnaliteAvailablesForSocieteIdAndModuleId(societeId, $ctrl.moduleSelected.ModuleId)
                 .then(getFonctionnaliteAvailablesForSocieteIdAndModuleIdSuccessed)
                 .catch(getFonctionnaliteAvailablesForSocieteIdAndModuleIdFail)
                 .finally(getFonctionnaliteAvailablesForSocieteIdAndModuleIdFinally)

    }

    function getFonctionnaliteAvailablesForSocieteIdAndModuleIdSuccessed(response) {
      $ctrl.features = response.data;
      if ($ctrl.features.length > 0) {
        $ctrl.featureSelected = $ctrl.features[0];
      }
    }

    function getFonctionnaliteAvailablesForSocieteIdAndModuleIdFail(error) {
      Notify.error($ctrl.resources.Global_Notification_Error);
    }

    function getFonctionnaliteAvailablesForSocieteIdAndModuleIdFinally(error) {
      ProgressBar.complete();
    }

    /* -------------------------------------------------------------------------------------------------------------
    *                                            SAUVEGARDE
    * -------------------------------------------------------------------------------------------------------------
    */

    $ctrl.canSave = function () {
      if ($ctrl.featureSelected !== null && $ctrl.moduleSelected !== null) {
        return true;
      }
      return false;
    }

    $ctrl.save = function () {
      if ($ctrl.featureSelected !== null && $ctrl.moduleSelected !== null) {
        ProgressBar.start();
        RoleService.addRoleFonctionnalite(roleId, $ctrl.featureSelected.FonctionnaliteId, $ctrl.modeSelected.value)
                    .then(addRoleFonctionnaliteSuccess)
                    .catch(addRoleFonctionnaliteFail)
                    .finally(addRoleFonctionnaliteFinally);
      }
    };

    function addRoleFonctionnaliteSuccess(response) {
      $uibModalInstance.close(response.data);
    }


    function addRoleFonctionnaliteFail(error) {

      if (error && error.status == 409) {//conflit
        $ctrl.ErrorMessage = $ctrl.resources.Role_Index_ModalFonctionnalite_Error_Conflict_AlreadyExist;
        return;
      }

      var validationError = ModelStateErrorManager.getErrors(error);

      if (validationError) {
        $ctrl.ErrorMessage = validationError;
      }
      else if (error.data && error.data.Message) {
        $ctrl.ErrorMessage = error.data.Message;
      }
      else {
        $ctrl.ErrorMessage = $ctrl.resources.Global_Notification_Error;
      }
    }

    function addRoleFonctionnaliteFinally() {
      ProgressBar.complete();
    }

    /* -------------------------------------------------------------------------------------------------------------
   *                                            FERMETURE
   * -------------------------------------------------------------------------------------------------------------
   */

    $ctrl.close = function () {
      $uibModalInstance.dismiss("cancel");
    };

  }
})();