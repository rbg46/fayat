(function (angular) {
    'use strict';
    
    angular.module('Fred').component('deleteDelegationComponent', {
      templateUrl: '/Areas/Personnel/Scripts/modals/suppression-delegation-modal.html',
      bindings: {
        resolve: '<',
        close: '&',
        dismiss: '&'
      },
      controller: 'DeleteDelegationComponentController'
    });
  
    function DeleteDelegationComponentController() {
      var $ctrl = this;
  
      angular.extend($ctrl, {
        // Fonctions
        handleSave: handleSave,
        handleCancel: handleCancel
      });
  
      /*
       * Initilisation du controller de la modal
       */
      $ctrl.$onInit = function () {
        $ctrl.delegationModal =  angular.copy($ctrl.resolve.delegation);
        $ctrl.resources = $ctrl.resolve.resources;
      };
  
      /*
       * @function handleSave()
       * @description Enregistrement de la nouvelle delegation : Renvoie les valeurs au controller principal
       */
      function handleSave() {
        $ctrl.close({ $value: $ctrl.delegationModal });
      };
  
      /* 
       * @function handleCancel()
       * @description Annulation de la création
       */
      function handleCancel() {
        $ctrl.dismiss({ $value: 'cancel' });
      };
    }  
    angular.module('Fred').controller('DeleteDelegationComponentController', DeleteDelegationComponentController);
  
  }(angular));