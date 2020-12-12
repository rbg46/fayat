(function (angular) {
  'use strict';

  angular.module('Fred').component('replaceTaskComponent', {
    templateUrl: '/Areas/Depense/Scripts/modals/replace-task-modal.html',
    bindings: {
      resolve: '<',
      close: '&',
      dismiss: '&'
    },
    controller: 'ReplaceTaskComponentController'
  });

  angular.module('Fred').controller('ReplaceTaskComponentController', ReplaceTaskComponentController);

    ReplaceTaskComponentController.$inject = ['RemplacementTacheService', 'Notify'];

    function ReplaceTaskComponentController(RemplacementTacheService,Notify) {
    var $ctrl = this;

    angular.extend($ctrl, {
      // Fonctions            
      handleCancel: handleCancel,
      handleValidate: handleValidate
    });

    /*
     * Initilisation du controller de la modal
     */
    $ctrl.$onInit = function () {
      $ctrl.resources = $ctrl.resolve.resources;      
      $ctrl.selectedDepenses = $ctrl.resolve.selectedDepenses;
      $ctrl.parent = $ctrl.resolve.scope;
      $ctrl.ciId = $ctrl.resolve.CI; 
      $ctrl.btnReplaceDisabled = true;

      // Modele Remplacement Tache
      $ctrl.RemplacementTache = {
        DepensesLiees: [],
        TacheId: 0
      };
    };

    /**
     * validation d'export
     */
    function handleValidate() {

      $ctrl.RemplacementTache.DepensesLiees = $ctrl.selectedDepenses;

      $ctrl.RemplacementTache.TacheId = $ctrl.Tache.TacheId;
      RemplacementTacheService.AddRemplacementTache($ctrl.RemplacementTache)
          .then(function () {
              $ctrl.close({ $value: $ctrl.exportOption });
              Notify.message($ctrl.resources.RT_Notification_Replacement_Success);          
              $ctrl.parent.selectedDepenses = [];
              $ctrl.parent.handleSearch();          
        });
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