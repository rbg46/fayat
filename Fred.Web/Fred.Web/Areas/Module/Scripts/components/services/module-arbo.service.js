/*
 * Service qui execute les requette http avec le serveur pour le components module-arbo.
 */
(function () {
  "use strict";


  angular.module('Fred').service('moduleArboService', moduleArboService);

  moduleArboService.$inject = ['$http'];


  function moduleArboService($http) {

    /*
     * Recupere les societes inactives pour un module donné
     */
    this.getInactivesSocietesForModuleId = function (moduleId) {
      return $http.get("/api/Module/GetInactivesSocietesForModuleId/" + moduleId);
    }  
    /*
     * Recupere les societes inactives pour un fonctionnalité donnée
     */
    this.GetInactivesSocietesForFonctionnaliteId = function (fonctionnaliteId) {
      return $http.get("/api/Module/GetInactivesSocietesForFonctionnaliteId/" + fonctionnaliteId);
    }

    /*
     * Recupere l'arbo
     */
    this.getOrganisationTreeForSocieteId = function (page, pageSize, societeId) {
      return $http.get("/api/Module/GetOrganisationTreeForSocieteId/" + page + "/" + pageSize + "/" + societeId);
    }

    /*
     * Active et desactive les societes pour un module donné.
     */
    this.enableOrDisableModuleByOrganisationIdsOfSocietesAndModuleId = function (moduleId, organisationIdsOfSocietesToEnable, organisationIdsOfSocietesToDisable) {
      return $http.post("/api/Module/EnableOrDisableModuleByOrganisationIdsOfSocietesAndModuleId/" + moduleId, {
        organisationIdsOfSocietesToEnable: organisationIdsOfSocietesToEnable,
        organisationIdsOfSocietesToDisable: organisationIdsOfSocietesToDisable
      });
    }

    /*
     * Active et desactive les societes pour une fonctionnalite donnée.
     */
    this.enableOrDisableFonctionnaliteByOrganisationIdsOfSocietesAndFonctionnaliteId = function (fonctionnaliteId, organisationIdsOfSocietesToEnable, organisationIdsOfSocietesToDisable) {
      return $http.post("/api/Module/EnableOrDisableFonctionnaliteByOrganisationIdsOfSocietesAndFonctionnaliteId/" + fonctionnaliteId, {
        organisationIdsOfSocietesToEnable: organisationIdsOfSocietesToEnable,
        organisationIdsOfSocietesToDisable: organisationIdsOfSocietesToDisable
      });
    }

  };


})();