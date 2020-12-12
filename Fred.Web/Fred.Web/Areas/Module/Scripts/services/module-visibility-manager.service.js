/*
 * Service qui gere la visibilité(actif/inactif) des modules et des fonctionnalites
 * actif/inactif des modules sur la societe 
 * partiellement actif/inactif 
 */
(function () {
  "use strict";


  angular.module('Fred').service('moduleVisibilityManagerService', moduleVisibilityManagerService);

  moduleVisibilityManagerService.$inject = [];


  function moduleVisibilityManagerService() {

    /*
     * Marque le module comme etant actif ou inactif pour la societe courante
     */
    this.markModuleDisabledOrEnableForSociete = function (modules, moduleDesactivesIds) {
      for (var i = 0; i < modules.length; i++) {
        var module = modules[i];
        var isDesactivedForSociete = moduleDesactivesIds.filter(function (moduleId) {
          return moduleId === module.ModuleId;
        });
        if (isDesactivedForSociete.length && isDesactivedForSociete.length > 0) {
          module.isActivedForSociete = false;
        } else {
          module.isActivedForSociete = true;
        }
      }
    }

    /*
    * Marque le module comme etant partiellement inactif. (une societe a le module desactivé)
    */
    this.markModulePartiallyDisabledOrEnable = function (modules, idsOfModulesPartiallyDisabled) {
      for (var i = 0; i < modules.length; i++) {
        var module = modules[i];
        var isPartiAllyDesactived = idsOfModulesPartiallyDisabled.filter(function (moduleId) {
          return moduleId === module.ModuleId;
        });
        if (isPartiAllyDesactived.length && isPartiAllyDesactived.length > 0) {
          module.isPartiallyDesactived = true;
        } else {
          module.isPartiallyDesactived = false;
        }
      }
    }

    /*
   * Marque la fonctionnalite comme etant partiellement inactives. (une societe a la fonctionnalite desactivée)
   */
    this.markFonctionnalitePartiallyDisabledOrEnable = function (features, idsOfFonctionnalitesPartiallyDisabled) {
      if (features) {
        for (var i = 0; i < features.length; i++) {
          var feature = features[i];
          var isPartiAllyDesactived = idsOfFonctionnalitesPartiallyDisabled.filter(function (fonctionnaliteId) {
            return fonctionnaliteId === feature.FonctionnaliteId;
          });
          if (isPartiAllyDesactived.length && isPartiAllyDesactived.length > 0) {
            feature.isPartiallyDesactived = true;
          } else {
            feature.isPartiallyDesactived = false;
          }
        }
      }

    }

    
  };


})();