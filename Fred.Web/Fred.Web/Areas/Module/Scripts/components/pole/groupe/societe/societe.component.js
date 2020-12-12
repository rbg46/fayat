(function () {
  'use strict';

  angular.module('Fred').component('societeComponent', {
    templateUrl: '/Areas/Module/Scripts/components/pole/groupe/societe/societe.tpl.html',
    bindings: {     
      societe: '<'
    },
    controller: 'societeComponentController'
  });

  angular.module('Fred').controller('societeComponentController', societeComponentController);

  societeComponentController.$inject = ['$scope', 'ProgressBar', 'Notify', 'moduleArboStoreService'];

  function societeComponentController($scope, ProgressBar, Notify, moduleArboStoreService) {

    var $ctrl = this;

    //////////////////////////////////////////////////////////////////
    // Init                                                         //
    //////////////////////////////////////////////////////////////////
    $ctrl.$onInit = function () {

    };   

  }
})();