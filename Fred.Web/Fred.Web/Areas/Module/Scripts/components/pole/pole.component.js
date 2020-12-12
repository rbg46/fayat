(function () {
  'use strict';

  angular.module('Fred').component('poleComponent', {
    templateUrl: '/Areas/Module/Scripts/components/pole/pole.tpl.html',
    bindings: {
      resources: '<',
      pole: '<'
    },
    controller: 'poleComponentController'
  });

  angular.module('Fred').controller('poleComponentController', poleComponentController);

  poleComponentController.$inject = ['$scope', 'ProgressBar', 'Notify', 'moduleArboStoreService'];

  function poleComponentController($scope, ProgressBar, Notify, moduleArboStoreService) {

    var $ctrl = this;

  
    //////////////////////////////////////////////////////////////////
    // Init                                                         //
    //////////////////////////////////////////////////////////////////
    $ctrl.$onInit = function () {

    };

  }
})();