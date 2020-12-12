(function () {
  'use strict';

  angular
    .module('Fred')
    .controller('authentificationLogdetailController', authentificationLogdetailController);

  authentificationLogdetailController.$inject = ['$scope', '$uibModalInstance','authentificationLogService','Notify','ProgressBar'];

  function authentificationLogdetailController($scope, $uibModalInstance, authentificationLogService, Notify, ProgressBar) {

    var $ctrl = this;
    $scope.close = close;
    $scope.model = null;
    activate();

    function activate() {    
      $scope.resources = $scope.$resolve.resources;
    
      ProgressBar.start();
      authentificationLogService.getDetail($scope.$resolve.id)
        .then(getDetailSucess)
        .catch(getDetailError)
        .finally(getDetailFinally);
    }

    function getDetailSucess(response) {
      $scope.model = response.data;
    }

    function getDetailError() {
      Notify.error($ctrl.resources.Global_Notification_Chargement_Error);
    }

    function getDetailFinally() {
      $scope.isBusy = false;
      ProgressBar.complete();
    }

 
   function close() {
      $uibModalInstance.dismiss('cancel');
    };

  }
})();