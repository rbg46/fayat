(function () {
  'use strict';

  angular.module('Fred').component('groupeComponent', {
    templateUrl: '/Areas/Module/Scripts/components/pole/groupe/groupe.tpl.html',
    bindings: {
      groupe: '<'    
    },
    controller: 'groupeComponentController'
  });

  angular.module('Fred').controller('groupeComponentController', groupeComponentController);

  groupeComponentController.$inject = ['$scope', 'ProgressBar', 'Notify', 'moduleArboStoreService'];

  function groupeComponentController($scope, ProgressBar, Notify, moduleArboStoreService) {

    var $ctrl = this;
   
    //////////////////////////////////////////////////////////////////
    // Init                                                         //
    //////////////////////////////////////////////////////////////////
    $ctrl.$onInit = function () {

    };
  
  }
})();