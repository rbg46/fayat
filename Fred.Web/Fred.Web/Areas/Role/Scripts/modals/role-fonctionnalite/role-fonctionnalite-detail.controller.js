
(function () {
  "use strict";


  angular.module('Fred').controller("roleFonctionnaliteDetailController", roleFonctionnaliteDetailController);

  roleFonctionnaliteDetailController.$inject = ['$scope', '$uibModalInstance', 'RoleService', 'Notify', 'ProgressBar', 'fonctionnaliteModeService'];

  function roleFonctionnaliteDetailController($scope, $uibModalInstance, RoleService, Notify, ProgressBar, fonctionnaliteModeService) {

    var $ctrl = this;
    $ctrl.model = null;
    $ctrl.resources = resources;

    init();

    /* -------------------------------------------------------------------------------------------------------------
    *                                            INIT
    * -------------------------------------------------------------------------------------------------------------
    */


    function init() {
      ProgressBar.start();
      RoleService.getRoleFonctionnaliteDetail($scope.$resolve.roleFonctionnaliteId)
                  .then(getRoleFonctionnaliteDetailSuccessed)
                  .catch(getRoleFonctionnaliteDetailFail)
                  .finally(getRoleFonctionnaliteDetailFinally);
    }


    function getRoleFonctionnaliteDetailSuccessed(response) {
      $ctrl.model = Object.assign({}, response.data);
      $ctrl.mode = fonctionnaliteModeService.getModeText(response.data.Mode);
    }

    function getRoleFonctionnaliteDetailFail(error) {
      Notify.error($ctrl.resources.Global_Notification_Error);
    }

    function getRoleFonctionnaliteDetailFinally() {
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