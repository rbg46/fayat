(function () {
  'use strict';

  angular
    .module('Fred')
    .component('fredBackButton', {
      templateUrl: '/Scripts/Controllers/menu/fred-back-button.html',
      bindings: {
        resources: '<'
      },
      controller: 'fredBackButtonController'
    });

  angular.module('Fred').controller('fredBackButtonController', fredBackButtonController);

  fredBackButtonController.$inject = ['fredSubscribeService'];

  function fredBackButtonController(fredSubscribeService) {
    var $ctrl = this;
    $ctrl.resources = resources;
    $ctrl.isVisible = false;

    //////////////////////////////////////////////////////////////////
    // Init                                                         //
    //////////////////////////////////////////////////////////////////
    $ctrl.$onInit = function () {
      if (fredSubscribeService.hasSubscriberFor('goBack')) {
        $ctrl.tooltip = fredSubscribeService.getTooltip('goBack');
        $ctrl.isVisible = true;
      }
    };

    //////////////////////////////////////////////////////////////////
    // Handlers                                                     //
    //////////////////////////////////////////////////////////////////
    $ctrl.goBack = function () {
      fredSubscribeService.raiseEvent('goBack');
    };
  }
})();