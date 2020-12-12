
(function () {
  'use strict';
  angular.module('Fred').directive('fredSuperAdminAuthorization', fredSuperAdminAuthorization);

  fredSuperAdminAuthorization.$inject = ['$window'];

  function fredSuperAdminAuthorization($window) {
    return {
      controller: 'FredAuthorizationController',     
      restrict: 'E',
      transclude: true,
      templateUrl: '/Scripts/module/authorization/directives/fred-super-admin-authorization.tpl.html',
      controllerAs: '$ctrl',
      bindToController: true,
      scope: {      
      },
    };
  }


  angular.module('Fred').controller('FredSuperAdminAuthorizationController', FredSuperAdminAuthorizationController);

  FredSuperAdminAuthorizationController.$inject = ['authorizationService'];

  function FredSuperAdminAuthorizationController(authorizationService) {
    
    var $ctrl = this;   
    $ctrl.isVisible = false;

    activate();

    function activate() {
      if (authorizationService.currentUserIsSuperAdmin()) {
        $ctrl.isVisible = true;
      }
    }    
  }

})();
