(function () {
  "use strict";



  angular.module('Fred').controller("EditRoleSeuilValidationModalController", EditRoleSeuilValidationModalController);

  EditRoleSeuilValidationModalController.$inject = ['$scope', '$uibModalInstance', 'RoleService', 'Notification', 'thresholdValidator'];

  function EditRoleSeuilValidationModalController($scope, $uibModalInstance, RoleService, Notification, thresholdValidator) {
    $scope.resources = resources;
    $scope.action = $scope.resources.Role_Index_ModalSeuil_Titre_Modifier;
    $scope.modalSelectedRole = null;
    $scope.parent = $scope.$resolve.parentScope;

    $scope.roleList = [$scope.$resolve.selectedRole];
    $scope.modalSelectedRole = $scope.$resolve.selectedRole;   

    RoleService.getDevisesList().then(function (t) {
      $scope.devises = t.data;
    
      angular.forEach($scope.parent.seuilValidationList, function (value, key) {
        if (value.DeviseId === $scope.$resolve.item.DeviseId)
          $scope.loadDataRef(value.Devise);       
      });

    });

    $scope.showPickList = function () {
      // {text} / {societeId} / {ciId} / {groupeId} / {materielId}
      return "/api/Devise/SearchLight/";
    };

    $scope.loadDataRef = function (item) {     
      $scope.deviseRef = item;
    };

    $scope.handleDeletePickListItemDeviseRef = function () {
      $scope.deviseRef = null;
    };


    var clone = {};
    angular.copy($scope.$resolve.item, clone);
    $scope.clone = clone;
    $scope.modalSelectedDeviseId = $scope.$resolve.item.DeviseId;

    $scope.close = function () {
      $uibModalInstance.dismiss("cancel");
    };

    $scope.save = function (formSeuil) {

      //nettoyage anciens messages d'erreurs
      cleanErrors();

      //Preparation des parametres pour la validation du formulaire
      var threshold = $scope.clone.Montant;
      var currency = $scope.deviseRef;
      var seuilValidations = $scope.parent.seuilValidationList;

      // verification du formulaire
      var validatorResult = thresholdValidator.valid($scope.clone, formSeuil, threshold, currency, seuilValidations, resources);

      if (validatorResult.isValid) {
        //Sauvegarde sur le serveur 
        var updatedThreshold = $scope.clone;
        updatedThreshold.DeviseId = $scope.deviseRef.DeviseId;
        updatedThreshold.Devise = $scope.deviseRef;
        RoleService.updateSeuilValidation(updatedThreshold)
          .then(onUpdateSuccess)
          .catch(onUpdateFail);
      }
      else {
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
      angular.extend($scope.$resolve.item, response.data);
      $uibModalInstance.close();
      Notification({ message: resources.Global_Notification_Enregistrement_Success, title: resources.Global_Notification_Titre });
    }

    function onUpdateFail(response) {
      $scope.hasErrors = true;
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
     
    };

  }


})();