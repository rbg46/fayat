(function (angular) {
  'use strict';

  angular.module('Fred').component('exportDepenseComponent', {
    templateUrl: '/Areas/Depense/Scripts/modals/export-depense-modal.html',
    bindings: {
      resolve: '<',
      close: '&',
      dismiss: '&'
    },
    controller: 'ExportDepenseComponentController'
  });

  angular.module('Fred').controller('ExportDepenseComponentController', ExportDepenseComponentController);
  
  function ExportDepenseComponentController() {
    var $ctrl = this;

    angular.extend($ctrl, {
      // Fonctions            
      handleCancel: handleCancel,
      handleValidate: handleValidate,

      // Variables                
      exportOption: 1,
      hasSelectedDepenses: false
    });

    /*
     * Initilisation du controller de la modal
     */
    $ctrl.$onInit = function () {
      $ctrl.resources = $ctrl.resolve.resources;
      $ctrl.hasSelectedDepenses = $ctrl.resolve.hasSelectedDepenses;
    };

    /**
     * validation d'export
     */
    function handleValidate() {
      $ctrl.close({ $value: $ctrl.exportOption });
    }

    /* 
     * @function handleCancel ()
     * @description Quitte la modal commande buyer
     */
    function handleCancel() {
      $ctrl.dismiss({ $value: 'cancel' });
    }

  }

}(angular));