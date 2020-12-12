(function (angular) {
    'use strict';
    
    angular.module('Fred').component('verificationCiSepComponent', {
      templateUrl: '/Areas/ValidationPointage/Scripts/Modals/verification-ci-sep-modal.html',
      bindings: {
        resolve: '<',
        close: '&',
        dismiss: '&'
      },
      controller: 'VerificationCiSepComponentController'
    });
  
    function VerificationCiSepComponentController() {
      var $ctrl = this;
  
      angular.extend($ctrl, {
        // Fonctions
        handleClose: handleClose,
      });
  
      /*
       * Initilisation du controller de la modal
       */
      $ctrl.$onInit = function () {
        $ctrl.ciSepList =  angular.copy($ctrl.resolve.ciSepList);
        $ctrl.resources = $ctrl.resolve.resources;
        $ctrl.isControleVrac = $ctrl.resolve.controleVrac;
        $ctrl.isRemonteeVrac = $ctrl.resolve.remonteeVrac;
      };
  
      function handleClose() {
        $ctrl.close({ $value: $ctrl.ciSepList });
      };
  
    }  
    angular.module('Fred').controller('VerificationCiSepComponentController', VerificationCiSepComponentController);
  
  }(angular));