/*
 * Service qui execute les requettes vers le serveur pour le components module-arbo.
 */
(function () {
  "use strict";


  angular.module('Fred').service('moduleArboDataService', moduleArboDataService);

  moduleArboDataService.$inject = ['moduleArboService', 'moduleArboStoreService', '$q'];


  function moduleArboDataService(moduleArboService, moduleArboStoreService, $q) {

    var PAGE_SIZE = 25;

    this.getArbo = function (module, feature) {

      //recuperation des societes inactive pour un module donné
      var inactivesSocietesRequest = getInactivesSocietesRequest(module, feature);

      //recuperation de l'arbo
      var page = moduleArboStoreService.getNextPage();
      var societeId = moduleArboStoreService.get('societeId');
      var arboRequest = moduleArboService.getOrganisationTreeForSocieteId(page, PAGE_SIZE, societeId);

      return $q.all([inactivesSocietesRequest, arboRequest])
          .then(getArboOnSuccess);
    }

    /*
     * Retourne la liste des societes inactives pour un module ou pour une fonctionnalite
     */
    function getInactivesSocietesRequest(module, feature) {
      if (moduleArboStoreService.isModuleSelected()) {
        return moduleArboService.getInactivesSocietesForModuleId(module.ModuleId);
      } else {
        return moduleArboService.GetInactivesSocietesForFonctionnaliteId(feature.FonctionnaliteId);
      }
    }


    function getArboOnSuccess(responses) {
      moduleArboStoreService.saveOrganisationIdsOfSocietesInactives(responses[0].data);
      moduleArboStoreService.addOrganisations(responses[1].data);
    }

  };


})();