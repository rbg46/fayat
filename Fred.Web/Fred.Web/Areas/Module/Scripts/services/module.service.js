(function () {
  "use strict";

  /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  /////////////////////////////////////////////////////////////// ES CE QUE CETTE DIRECTIVE EST ENCORE UTILISEE ? /////////////////////////////////////////
  /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


  angular.module('Fred').service('ModuleService', ModuleService);

  ModuleService.$inject = ['$http'];

  function ModuleService($http) {

    var service = {
      // Gestion des Modules
      getModuleList: getModuleList,
      getModuleById: getModuleById,
      addModule: addModule,
      updateModule: updateModule,
      deleteModule: deleteModule,

      // Gestion des Fonctionnalités
      getFeatureList: getFeatureList,
      getFeatureById: getFeatureById,
      addFeature: addFeature,
      updateFeature: updateFeature,
      deleteFeature: deleteFeature,
      //Permissions
      CanAddPermissionFonctionnalite: CanAddPermissionFonctionnalite,
      getPermissionFonctionnalites: getPermissionFonctionnalites,
      AddPermissionFonctionnalite: AddPermissionFonctionnalite,
      deletePermissionFonctionnalite: deletePermissionFonctionnalite,
      //ModuleDesactive
      getInactifModulesForSocieteId: getInactifModulesForSocieteId,
      disableModuleForSoceiteId: disableModuleForSoceiteId,
      enableModuleForSocieteId: enableModuleForSocieteId,

      getModulesAndFonctionnalitesPartiallyDisabled: getModulesAndFonctionnalitesPartiallyDisabled
    };



    return service;

    function getModuleList() {
      return $http.get("/api/Module");
    }
    function getModuleById(id) {
      return $http.get("/api/Module/" + id);
    }
    function addModule(module) {
      return $http.post("/api/Module", module);
    }
    function updateModule(module) {
      return $http.put("/api/Module", module);
    }
    function deleteModule(moduleId) {
      return $http.delete("/api/Module/" + moduleId);
    }
    function getFeatureList(moduleId) {
      return $http.get("/api/Module/GetFeatureListByModuleId/" + moduleId);
    }
    function getFeatureById(id) {
      return $http.get("/api/Module/GetFeatureById/" + id);
    }
    function addFeature(feature) {
      return $http.post("/api/Module/AddFeature", feature);
    }
    function updateFeature(feature) {
      return $http.put("/api/Module/UpdateFeature", feature);
    }
    function deleteFeature(featureId) {
      return $http.delete("/api/Module/DeleteFeatureById/" + featureId);
    }
    function getPermissionFonctionnalites(fonctionnaliteId) {
      return $http.get("/api/Module/GetPermissionFonctionnalites/" + fonctionnaliteId);
    }
    function AddPermissionFonctionnalite(info) {
      return $http.post("/api/Module/AddPermissionFonctionnalite/" + info.permissionId + "/" + info.fonctionnaliteId);
    }
    function deletePermissionFonctionnalite(permissionFonctionnaliteId) {
      return $http.delete("/api/Module/DeletePermissionFonctionnalite/" + permissionFonctionnaliteId);
    }
    function CanAddPermissionFonctionnalite(permissionId) {
      return $http.get("/api/Module/CanAddPermissionFonctionnalite/" + permissionId);
    }

    function getInactifModulesForSocieteId(societeId) {
      return $http.get("/api/Module/GetInactifModulesForSocieteId/" + societeId);
    }
    function disableModuleForSoceiteId(moduleId, societeId) {
      return $http.post("/api/Module/DisableModuleForSocieteId/" + moduleId + "/" + societeId, {});
    }
    function enableModuleForSocieteId(moduleId, societeId) {
      return $http.delete("/api/Module/EnableModuleForSoceiteId/" + moduleId + "/" + societeId);
    }
   
    function getModulesAndFonctionnalitesPartiallyDisabled() {
      return $http.get("/api/Module/GetModulesAndFonctionnalitesPartiallyDisabled");
    }
    

  }

})();