(function (angular) {
    'use strict';
    
    angular.module('Fred').component('deleteContratInterimComponent', {
      templateUrl: '/Areas/Personnel/Scripts/modals/suppression-contratInterim-modal.html',
      bindings: {
        resolve: '<',
        close: '&',
        dismiss: '&'
      },
      controller: 'DeleteContratInterimComponentController'
    });
  
    function DeleteContratInterimComponentController($timeout) {
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
        $ctrl.ContratInterimModal =  angular.copy($ctrl.resolve.contratInterim);
        $ctrl.resources = $ctrl.resolve.resources;
      };
  
      /*
       * @function handleSave()
       * @description Enregistrement de la nouvelle delegation : Renvoie les valeurs au controller principal
       */
      function handleSave() {
        $ctrl.close({ $value: $ctrl.ContratInterimModal });
      };
  
      /* 
       * @function handleCancel()
       * @description Annulation de la création
       */
      function handleCancel() {
        $ctrl.dismiss({ $value: 'cancel' });
      };
    }
  
    DeleteContratInterimComponentController.$inject = ['$timeout'];
  
    angular.module('Fred').controller('DeleteContratInterimComponentController', DeleteContratInterimComponentController);
  
  }(angular));