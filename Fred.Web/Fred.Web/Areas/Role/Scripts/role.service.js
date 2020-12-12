(function () {
  "use strict";
  /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  /////////////////////////////////////////////////////////////// ES CE QUE CETTE DIRECTIVE EST ENCORE UTILISEE ? /////////////////////////////////////////
  /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


  angular.module('Fred').service('RoleService', RoleService);

  RoleService.$inject = ['$http'];

  function RoleService($http) {

    this.getRolesBySocieteId = function (societeId) {
      return $http.get("/api/Role/GetRolesBySocieteId/" + societeId);
    };

    this.duplicateRole = function (role, copythreshold, copyFeature) {
      return $http.post("/api/Role/DuplicateRole/" + copythreshold + "/" + copyFeature, role);
    };

    this.GetRoleFonctionnalitesByRoleId = function (roleId) {
      return $http.get("/api/Role/GetRoleFonctionnalitesByRoleId/" + roleId);
    };


    this.getModulesAvailablesForSocieteId = function (societeId) {
      return $http.get("/api/Role/GetModulesAvailablesForSocieteId/" + societeId);
    };
    this.getFonctionnaliteAvailablesForSocieteIdAndModuleId = function (societeId, moduleId) {
      return $http.get("/api/Role/GetFonctionnaliteAvailablesForSocieteIdAndModuleId/" + societeId + "/" + moduleId);
    };


    this.addRoleFonctionnalite = function (roleId, fonctionnaliteId, mode) {
      return $http.post("/api/Role/AddRoleFonctionnalite", { roleId: roleId, fonctionnaliteId: fonctionnaliteId, mode: mode });
    };

    this.deleteRoleFonctionnaliteById = function (rolefonctionnaliteId) {
      return $http.delete("/api/Role/DeleteRoleFonctionnaliteById/" + rolefonctionnaliteId);
    };

    this.getDevisesList = function () {
      return $http.get("/api/Devise");
    };

    this.getSeuilValidations = function (roleId) {
      return $http.get("/api/Role/SeuilValidationListByRoleId/" + roleId);
    };

    this.getSeuilValidationById = function (id) {
      return $http.get("/api/Role/GetSeuilValidation/" + id);
    };

    this.addRole = function (model) {
      var data = JSON.stringify(model);
      return $http.post("/api/Role", data);
    };

    this.updateRole = function (model) {
      var data = JSON.stringify(model);
      return $http.put("/api/Role", data);
    };

    this.deleteRole = function (roleId) {
      return $http.delete("/api/Role/" + roleId);
    };

    this.addSeuilValidation = function (seuilValidationModel) {
      var data = JSON.stringify(seuilValidationModel);
      return $http.post("/api/Role/AddSeuilValidation", data);
    };

    this.updateSeuilValidation = function (item) {
      return $http.put("/api/Role/UpdateSeuilValidation", item);
    };

    this.deleteSeuilValidationById = function (seuilValidationId) {
      return $http.delete("/api/Role/DeleteSeuilValidation/" + seuilValidationId);
    };

    this.cloneRoles = function (societeSourceId, societeTargetId, copyfeatures, copythreshold) {
      return $http.get("/api/Role/cloneRoles/" + societeSourceId + "/" + societeTargetId + "/" + copyfeatures + "/" + copythreshold);
    };

    this.changeMode = function (roleFonctionnaliteId, mode) {
      return $http.put("/api/Role/ChangeMode/" + roleFonctionnaliteId + "/" + mode);
    };

    this.getRoleFonctionnaliteDetail = function (roleFonctionnaliteId) {
      return $http.get("/api/Role/GetRoleFonctionnaliteDetail/" + roleFonctionnaliteId);
    };

  }



  angular.module('Fred').service('thresholdValidator', thresholdValidator);

  function thresholdValidator() {
    return {
      valid: valid,
      validCurrency: validCurrency,
      validThreshold: validThreshold
    };

    function valid(item, formSeuil, threshold, currency, seuilValidations, resources) {

      var result = { isValid: false, messages: [], message: "" };

      var currencyValidation = validCurrency(item, currency, seuilValidations, resources);
      var thresholdValidation = validThreshold(formSeuil, threshold);

      if (currencyValidation.isValid && thresholdValidation.isValid) {
        result.isValid = true;
        return result;
      } else {
        result.isValid = false;
        var messages = [];

        for (var i = 0; i < currencyValidation.messages.length; i++) {
          result.messages.push(currencyValidation.messages[i]);
        }

        for (var j = 0; j < thresholdValidation.messages.length; j++) {
          result.messages.push(thresholdValidation.messages[j]);
        }

        for (var k = 0; k < result.messages.length; k++) {
          result.message += result.messages[k] + "\n";
        }
        return result;
      }

    }

    function validCurrency(item, currency, seuilValidations, resources) {
      var result = { isValid: true, messages: [] };
      if (!currency) {
        result.isValid = false;
        result.messages.push(resources.Role_Service_Control_DeviseRequis_Erreur);
        return result;
      }

      var seuilValidationsWithoutMe = seuilValidations.filter(function (seuilvalidation) {
        return seuilvalidation.SeuilValidationId !== item.SeuilValidationId;
      });
      var hasSameDevise = seuilValidationsWithoutMe.filter(function (seuilvalidation) {
        return seuilvalidation.DeviseId === currency.DeviseId;
      });
      if (hasSameDevise.length > 0) {
        result.isValid = false;
        result.messages.push(resources.Role_Service_Control_DeviseDejaAffecte_Erreur);
      }
      return result;
    }

    function validThreshold(formSeuil, threshold) {
      var result = { isValid: true, messages: [] };
      if (formSeuil.Montant.$error.required) {
        result.isValid = false;
        result.messages.push(resources.Role_Service_Control_SeuilRequis_Erreur);
      }
      if (formSeuil.Montant.$error.min) {
        result.isValid = false;
        result.messages.push(resources.Role_Service_Control_SeuilMin_Erreur);
      }
      if (formSeuil.Montant.$error.max) {
        result.isValid = false;
        result.messages.push(resources.Role_Service_Control_SeuilMax_Erreur);
      }
      if (formSeuil.Montant.$error.pattern) {
        result.isValid = false;
        result.messages.push(resources.Role_Service_Control_SeuilFormat_Erreur);
      }
      if (formSeuil.Montant.$error.number) {
        result.isValid = false;
        result.messages.push(resources.Role_Service_Control_SeuilFormat_Erreur);
      }

      return result;
    }

  }




})();