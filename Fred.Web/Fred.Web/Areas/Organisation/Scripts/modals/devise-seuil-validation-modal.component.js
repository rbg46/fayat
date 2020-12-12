(function (angular) {
  'use strict';

  angular.module('Fred').component('deviseSeuilValidationComponent', {
    templateUrl: '/Areas/Organisation/Scripts/modals/devise-seuil-validation-modal.html',
    bindings: {
      resolve: '<',
      close: '&',
      dismiss: '&'
    },
    controller: 'DeviseSeuilValidationComponentController'
  });

  angular.module('Fred').controller('DeviseSeuilValidationComponentController', DeviseSeuilValidationComponentController);

  DeviseSeuilValidationComponentController.$inject = ['$timeout', '$filter', 'OrganisationService', 'Notify'];


  function DeviseSeuilValidationComponentController($timeout, $filter, OrganisationService, Notify) {
    var $ctrl = this;

    angular.extend($ctrl, {
      // Fonctions
      handleSave: handleSave,
      handleCancel: handleCancel,
      handleDelete: handleDelete,
      handleSelectedItem: handleSelectedItem
    });

    /*
     * Initilisation du controller de la modal
     */
    $ctrl.$onInit = function () {
      $ctrl.roleOrganisationCurrencyList = $ctrl.resolve.roleOrganisationCurrencyList;
      $ctrl.resources = $ctrl.resolve.resources;
      $ctrl.handleShowPickList = $ctrl.resolve.handleShowPickList;
      $ctrl.role = $ctrl.resolve.role;
      $ctrl.organisation = $ctrl.resolve.organisation;
      $ctrl.modalTitle = $ctrl.resolve.modalTitle;
      $ctrl.clone = $ctrl.resolve.item;
    };

    /*
     * @function handleSave()
     * @description Enregistrement de la nouvelle affection : Renvoie les valeurs au controller principal
     */
    function handleSave() {
      cleanError();

      // verification du formulaire      
      var validatorResult = {};

      if ($ctrl.clone.SeuilRoleOrgaId > 0) {
        validatorResult = OrganisationService.thresholdValidator().validThreshold($ctrl.formSeuil, $ctrl.clone.Seuil);
      }
      else {
        validatorResult = OrganisationService.thresholdValidator().valid($ctrl.roleOrganisationCurrencyList, $ctrl.formSeuil, $ctrl.clone.Seuil, $ctrl.clone.Devise, $ctrl.organisation, $ctrl.role);
      }

      if (validatorResult.isValid) {

        if ($ctrl.clone.SeuilRoleOrgaId > 0) {
          OrganisationService.updateThresholdOrganisation($ctrl.clone)
           .then(function (response) {
             $ctrl.close({ $value: response.data });
             Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
           })
           .catch(function (error) { Notify.error(error.data.Message); });
        }
        else {
          OrganisationService.addThresholdOrganisation($ctrl.clone)
            .then(function (response) {
              $ctrl.close({ $value: response.data });
              Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
            })
            .catch(function (error) { Notify.error(error.data.Message); });
        }
      }
      else {
        setError(validatorResult.message);
      }
    };

    /*
     * @description Set les erreurs
     */
    function setError(message) {
      $ctrl.hasErrors = true;
      $ctrl.ErrorMessage = message;
    };

    /*
     * @description Efface les erreurs
     */
    function cleanError() {
      $ctrl.hasErrors = false;
      $ctrl.ErrorMessage = '';
    };

    /* 
     * @function handleCancel()
     * @description Annulation de la création
     */
    function handleCancel() {
      $ctrl.dismiss({ $value: 'cancel' });
    };

    /*
     * -------------------------------------------------------------------------------------------------------------
     *                                            GESTION DES PICKLIST
     * -------------------------------------------------------------------------------------------------------------
     */

    /*
     * @description Gère la suppression de l'élément courant de la lookup
     */
    function handleDelete(type) {
      cleanError();
      $ctrl.clone.Devise = null;
      $ctrl.clone.DeviseId = 0;
    };

    /*
     * @description Gère la sélection d'une devise
     */
    function handleSelectedItem(item) {
      cleanError();
      $ctrl.clone.Devise = item;
      $ctrl.clone.DeviseId = item.DeviseId;

      // Chargement du montant du seuil par défaut (si existant)
      OrganisationService.getValidationThresholdByRoleId($ctrl.role.RoleId)
        .then(function (response) {
          var currency = $filter('filter')(response.data, { DeviseId: $ctrl.clone.DeviseId }, true)[0];
          $ctrl.clone.Seuil = (currency !== undefined && currency !== null) ? currency.Montant : null;
        })
        .catch(function (error) { Notify.error(error.data.Message); });
    };

  }

}(angular));