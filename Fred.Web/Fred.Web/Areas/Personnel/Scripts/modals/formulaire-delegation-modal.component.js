(function (angular) {
    'use strict';
    
    angular.module('Fred').component('formulaireDelegationComponent', {
      templateUrl: '/Areas/Personnel/Scripts/modals/formulaire-delegation-modal.html',
      bindings: {
        resolve: '<',
        close: '&',
        dismiss: '&'
      },
      controller: 'FormulaireDelegationComponentController'
    });
  
    function FormulaireDelegationComponentController(DelegationService) {
      var $ctrl = this;
  
      angular.extend($ctrl, {
        // Fonctions
        handleSave: handleSave,
        handleCancel: handleCancel,
        handleDelete: handleDelete,
        handleSelectedItem: handleSelectedItem,
        handleDateValidation: handleDateValidation,
        showPick: showPick,
        // Variables
        selectedRef: null,
        delegationForm: {},
        errorValidation: false
      });
  
      /*
       * Initilisation du controller de la modal
       */
      $ctrl.$onInit = function () {
        $ctrl.delegationModal =  angular.copy($ctrl.resolve.delegation);
        $ctrl.societeId = $ctrl.resolve.societeId;
        $ctrl.resources = $ctrl.resolve.resources;
      };
  
      /*
       * @function handleSave()
       * @description Enregistrement de la nouvelle delegation : Renvoie les valeurs au controller principal
       */
      function handleSave() {
        if (!$ctrl.delegationForm.$invalid) {
          DelegationService.GetDelegationAlreadyActive($ctrl.delegationModal).then(function(value){
            if(value.data === 0){
              $ctrl.close({ $value: $ctrl.delegationModal });
            }
            else {
              $ctrl.errorValidation = true
            }
          }).catch(function(error){console.log(error)});
        }
      };
  
      /* 
       * @function handleCancel()
       * @description Annulation de la création
       */
      function handleCancel() {
        $ctrl.dismiss({ $value: 'cancel' });
      };
  
      /*
       * @function handleDateValidation
       * @description Valide les dates de début et de fin de délégation
       */
      function handleDateValidation() {
        $ctrl.delegationForm.DateDeDebut.$setValidity("RangeError", ($ctrl.delegationModal.DateDeDebut <= $ctrl.delegationModal.DateDeFin));
      };
  
      /*
       * -------------------------------------------------------------------------------------------------------------
       *                                            GESTION DE LA PICKLIST
       * -------------------------------------------------------------------------------------------------------------
       */
      function handleDelete() {
        $ctrl.delegationModal.PersonnelDelegueId = null;
        $ctrl.delegationModal.PersonnelDelegue = null;
      };
  
      function handleSelectedItem(item) {
        $ctrl.delegationModal.PersonnelDelegueId = item.IdRef;
        $ctrl.delegationModal.PersonnelDelegue = item;
      };

      function showPick() {

        var searchLightUrl = '/api/Delegation/Delegue/'+$ctrl.delegationModal.PersonnelDelegantId+'/?recherche={0}&page={1}&ciId={2}&pageSize={3}&societeId={4}';
  
        searchLightUrl = String.format(searchLightUrl, null , null, null, $ctrl.societeId);

        return searchLightUrl;
      }
    }
  
    FormulaireDelegationComponentController.$inject = ['DelegationService'];
  
    angular.module('Fred').controller('FormulaireDelegationComponentController', FormulaireDelegationComponentController);
  
  }(angular));