(function () {
  "use strict";
  

  angular.module('Fred').component('fredTreeStateToggleComponent', {
    templateUrl: '/Areas/Role/Scripts/components/fred-tree-state-toggle.tpl.html',
    bindings: {
      change: '&',    
      currentValue: '<',
      isBusy: '=',
      stepOne: '<',
      stepTwo: '<',
      stepTree: '<',
      resources: '<'
    },
    controller: 'fredTreeStateToggleComponentController'
  });

  angular.module('Fred').controller('fredTreeStateToggleComponentController', fredTreeStateToggleComponentController);

  fredTreeStateToggleComponentController.$inject = ['$timeout'];

  function fredTreeStateToggleComponentController($timeout) {

    var $ctrl = this;
    
    $ctrl.toggle = function () {
      if (!$ctrl.isBusy) {
        if ($ctrl.currentValue === $ctrl.stepOne.value) {
          $ctrl.currentValue = $ctrl.stepTwo.value;
        }
        else if ($ctrl.currentValue === $ctrl.stepTwo.value) {
          $ctrl.currentValue = $ctrl.stepTree.value;
        }
        else {
          $ctrl.currentValue = $ctrl.stepOne.value;
        }
        if (typeof $ctrl.change != 'undefined') {
          $timeout(function () {
            $ctrl.change({ currentValue: $ctrl.currentValue });
          });
        }
      }

    };

  }


})();