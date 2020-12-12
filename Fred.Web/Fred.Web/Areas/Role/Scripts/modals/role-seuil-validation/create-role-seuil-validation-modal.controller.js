(function () {
  "use strict";



  angular.module('Fred').controller("CreateRoleSeuilValidationModalController", CreateRoleSeuilValidationModalController);

  CreateRoleSeuilValidationModalController.$inject = ['$scope', '$uibModalInstance', 'RoleService', 'Notification', 'thresholdValidator'];

  function CreateRoleSeuilValidationModalController($scope, $uibModalInstance, RoleService, Notification, thresholdValidator) {
    $scope.resources = resources;
    $scope.action = $scope.resources.Role_Index_ModalSeuil_Titre_Ajouter;
    $scope.modalSelectedRole = null;
    $scope.parent = $scope.$resolve.parentScope;


    $scope.showPickList = function () {
      return "/api/Devise/SearchLight/";
    };

    $scope.loadDataRef = function (item) {    
      $scope.deviseRef = item;
    };

    $scope.handleDeletePickListItemDeviseRef = function () {
      $scope.deviseRef = null;
    };

    $scope.roleList = [$scope.$resolve.selectedRole];
    $scope.modalSelectedRole = $scope.$resolve.selectedRole;
   
    RoleService.getDevisesList().then(function (t) {
      $scope.devises = t.data;
      var associatedDevisesIds = [];
      angular.forEach($scope.$resolve.items, function (value, key) {
        associatedDevisesIds.push(value.DeviseId);
      });

      $scope.devises = $scope.devises.filter(function (val) {
        return $.inArray(val.DeviseId, associatedDevisesIds) === -1;
      });
    });

    var clone = { SeuilValidationId: 0, RoleId: $scope.$resolve.roleId, DeviseId: 0, Montant: "" };
    $scope.clone = clone;

    $scope.close = function () {
      $uibModalInstance.dismiss("cancel");
    };


    $scope.save = function (formSeuil) {

      cleanErrors();
      //Preparation des parametres pour la validation du formulaire
      var threshold = $scope.clone.Montant;
      var currency = $scope.deviseRef;
      var seuilValidations = $scope.parent.seuilValidationList;
      // verification du formulaire
      var validatorResult = thresholdValidator.valid($scope.clone, formSeuil, threshold, currency, seuilValidations, resources);

      if (validatorResult.isValid) {
        var seuilValidationModel = { DeviseId: $scope.deviseRef.DeviseId, RoleId: $scope.clone.RoleId, Montant: $scope.clone.Montant };
        //Creation sur le serveur 
        RoleService.addSeuilValidation(seuilValidationModel)
          .then(onUpdateSuccess)
          .catch(onUpdateFail);
      } else {
        //Affichage des erreurs de validations       
        $scope.hasErrors = true;
        $scope.ErrorMessage = validatorResult.message;
      }
    };

    function cleanErrors() {
      $scope.hasErrors = false;
      $scope.ErrorMessage = "";
    }

    function onUpdateSuccess(response) {
      $scope.$resolve.items.push(response.data);
      $uibModalInstance.close();
      Notification({ message: resources.Global_Notification_Enregistrement_Success, title: resources.Global_Notification_Titre });
    }

    function onUpdateFail(response) {
      $scope.hasErrors = true;
      if (response.status === 400) {
        $scope.ErrorMessage = response.data.Message;
      } else {
        $scope.ErrorMessage = resources.Global_Notification_Error;
      }
    }

    $scope.getSelectedRole = function () {
      var selectedRole = $("#ddlRoles :selected").val();
      $scope.modalSelectedRole = selectedRole;
      $scope.clone.RoleId = selectedRole;
      //$scope.modules = RoleService.getModules(selectedRole);
    };

    $scope.getSelectedModule = function () {
      var selectedModule = $("#ddlModules :selected").val();
      $scope.modalSelectedModule = selectedModule;    
    };
  }



})();