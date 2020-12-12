(function (angular) {
  'use strict';

  angular.module('Fred').controller('OrganisationController', OrganisationController);

  OrganisationController.$inject = ['$q', '$filter', '$uibModal', 'Notify', 'ProgressBar', 'OrganisationService', 'confirmDialog'];

  function OrganisationController($q, $filter, $uibModal, Notify, ProgressBar, OrganisationService, confirmDialog) {
    /*
     * -------------------------------------------------------------------------------------------------------------
     *                                            INIT
     * -------------------------------------------------------------------------------------------------------------
     */
    var $ctrl = this;

    // méthodes exposées
    angular.extend($ctrl, {
      handleSelectRole: handleSelectRole,
      handleSelectCurrency: handleSelectCurrency,
      handleAddOrUpdateSeuilValidation: handleAddOrUpdateSeuilValidation,
      handleDeleteSeuilValidation: handleDeleteSeuilValidation,
      handleShowPickList: handleShowPickList,
      handlePickListSelectedItem: handlePickListSelectedItem,

    });

    init();

    /**
     * Initialisation du controller.
     * 
     */
    function init() {
      ProgressBar.start();

      angular.extend($ctrl, {
        // Instanciation Objet Ressources
        resources: resources,

        // Initialisation des données                           
        searchRole: "",
        searchCurrency: "",

        selectedOrganisation: null,
        selectedRole: null,
        selectedCurrency: null,

        organisationList: [],
        roleList: [],
        roleOrganisationCurrencyList: []
      });

      // Chargement des données du personnel          
      ProgressBar.complete();
    }

    return $ctrl;

    /*
     * -------------------------------------------------------------------------------------------------------------
     *                                            HANDLERS
     * -------------------------------------------------------------------------------------------------------------
     */

    function handleSelectRole(role) {
      $ctrl.selectedRole = role;
      actionLoadroleOrganisationCurrencyList($ctrl.selectedRole);
    }

    function handleSelectCurrency(selectedCurrency) {
      $ctrl.selectedCurrency = selectedCurrency;
    }

    function handleAddOrUpdateSeuilValidation(roleOrgaDevise) {
      actionAddOrUpdateSeuilValidation(roleOrgaDevise);
    }

    function handleDeleteSeuilValidation(roleOrgaDevise, index) {
      actionDeleteThresholdOrga(roleOrgaDevise, index);
    }

    function handleShowPickList(referential) {
      switch (referential) {
        case "Devise":
          return "/api/Devise/SearchLight/?organisationId=" + $ctrl.selectedOrganisation.OrganisationId;          
        default:
          break;
      }
    }

    function handlePickListSelectedItem(referential, item) {
      $ctrl.selectedOrganisation = item;

      switch (referential) {
        case "Organisation":
          actionOnOrganisationChange();
          break;
        default:
          break;
      }
    }


    /*
     * -------------------------------------------------------------------------------------------------------------
     *                                            ACTIONS
     * -------------------------------------------------------------------------------------------------------------
     */

    /*
     * @description Chargement des rôles après avoir choisit une Organisation
     */
    function actionOnOrganisationChange() {
      if ($ctrl.selectedOrganisation !== null) {        
        $q.when()
          .then(ProgressBar.start)
          .then(actionLoadRoleList)
          .then(actionLoadAllRoleOrganisationDeviseList)
          .then(ProgressBar.complete);
      }
    }

    /*
     * @function actionLoadRoleList
     * @description Chargement de la liste des rôles d'une Organisation
     */
    function actionLoadRoleList() {
      return OrganisationService.getRoleListByOrganisationId($ctrl.selectedOrganisation.OrganisationId).then(function (response) {
        $ctrl.roleList = response.data;
        $ctrl.selectedRole = null;
        $ctrl.roleOrganisationCurrencyList = [];
        return $ctrl.roleList;
      }).catch(function (error) { Notify.error(error.data.Message); });
    }

    /*
     * @function actionLoadAllRoleOrganisationDeviseList(roleList) 
     * @description Chargement des associations RoleOrganisationDevise     
     */
    function actionLoadAllRoleOrganisationDeviseList(roleList) {
      angular.forEach(roleList, function (val, key) {
        OrganisationService.getThresholdByOrganisationId($ctrl.selectedOrganisation.OrganisationId, val.RoleId).then(function (response) {
          val.roleOrganisationDeviseCount = response.data.length;

          //actionSetLibelleDeviseCount(val);

        }).catch(function (error) { Notify.error(error.data.Message); });
      });
    }

    ///*
    // * @description Rempli le champ LibelleDeviseCount pour chaque Rôle. (ex: 1 Devise, x Devises, Aucune Devise)
    // */
    //function actionSetLibelleDeviseCount(role){
    //  if (role.roleOrganisationDeviseCount === 0) {
    //    role.LibelleDeviseCount = resources.Organisation_Controller_AucuneDevise;
    //  }
    //  else {
    //    role.LibelleDeviseCount = (role.roleOrganisationDeviseCount === 1) ? role.roleOrganisationDeviseCount + " " + resources.Devise_lb : role.roleOrganisationDeviseCount + " " + resources.Devises_lb;
    //  }
    //}

    /*
     * @description Action Chargement de la liste des devises surchargées en fonction du rôle et de l'organisation
     */
    function actionLoadroleOrganisationCurrencyList(selectedRole) {
      $ctrl.roleOrganisationCurrencyList = [];

      OrganisationService.getThresholdByOrganisationId($ctrl.selectedOrganisation.OrganisationId, selectedRole.RoleId).then(function (response) {
        angular.forEach(response.data, function (val, key) {
          $ctrl.roleOrganisationCurrencyList.push(val);
        });

        $ctrl.selectedRole.roleOrganisationDeviseCount = response.data.length;
        //actionSetLibelleDeviseCount($ctrl.selectedRole);

      }).catch(function (error) { Notify.error(error.data.Message); });
    }

    /*
     * @description Action ouverture de la modal d'ajout ou de mise à jour d'une surcharge d'un seuil de validation
     */
    function actionAddOrUpdateSeuilValidation(roleOrgaDevise) {
      var item = null, modalTitle = "";
      if (roleOrgaDevise) {
        item = roleOrgaDevise;
        modalTitle = resources.Organisation_Controller_Modal_ModifierSeuil;
      }
      else {
        item = { SeuilRoleOrgaId: 0, OrganisationId: $ctrl.selectedOrganisation.OrganisationId, RoleId: $ctrl.selectedRole.RoleId, DeviseId: 0, Seuil: '' };
        modalTitle = resources.Organisation_Controller_Modal_AjouterSeuil;
      }

      var modalInstance = $uibModal.open({
        animation: true,
        component: 'deviseSeuilValidationComponent',
        size: 'md',
        resolve: {
          resources: function () { return resources; },
          roleOrganisationCurrencyList: function () { return $ctrl.roleOrganisationCurrencyList; },
          organisation: function () { return $ctrl.selectedOrganisation; },
          handleShowPickList: function () { return handleShowPickList; },
          role: function () { return $ctrl.selectedRole; },
          modalTitle: function () { return modalTitle; },
          item: function () { return item; }
        }
      });

      modalInstance.result.then(function (createdOverloadedCurrency) {
        handleSelectRole($ctrl.selectedRole);
        handleSelectCurrency(createdOverloadedCurrency);
      });
    }

    /*
     * @description Action ouverture de la modal de confirmation de suppression d'une surcharge de validation
     */
    function actionDeleteThresholdOrga(threshold, index) {

      confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationSuppression).then(function () {

        OrganisationService.deleteThresholdOrganisation(threshold.SeuilRoleOrgaId).then(function (response) {
          $ctrl.roleOrganisationCurrencyList.splice(index, 1);

          // Mise à jour du nombre de devises paramétrées pour le Rôle sélectionné.
          $ctrl.selectedRole.roleOrganisationDeviseCount--;
          //actionSetLibelleDeviseCount($ctrl.selectedRole);

          $ctrl.selectedCurrency = null;
          Notify.message(resources.Global_Notification_Suppression_Success);

        }).catch(function (error) { Notify.error(error.data.Message); });
      });
    }

  };
  

  angular.module('Fred').filter('deviseCodeLibelleFilter', function () {
    return function (items, searchText) {
      if (searchText !== undefined && searchText !== null) {
        var filtered = [];
        for (var i = 0; i < items.length; i++) {
          var item = items[i];
          if (item.Devise.IsoCode.toLowerCase().indexOf(searchText.toLowerCase()) >= 0 || item.Devise.Libelle.toLowerCase().indexOf(searchText.toLowerCase()) >= 0) {
            filtered.push(item);
          }
        }
        return filtered;
      }
      return items;
    };
  });

})(angular);