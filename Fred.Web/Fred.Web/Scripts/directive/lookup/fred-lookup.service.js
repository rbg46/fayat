(function () {
  'use strict';

  angular.module('Fred').service('FredLookupService', FredLookupService);

  /**
   * @description Service Fred Lookup
   * @returns {any} Constructeur du service
   */
  function FredLookupService() {
      var service = {
          searchLightUrl: searchLightUrl
      };

    return service;

    /**
     * @description Création de l'url API correspondante à la liste d'éléments recherchés (ex: Ressource, Tâche, Personnel, etc.)
     * @param {any} params Liste des paramètres  
     * @returns {any} une URL correcte vers l'API permettant la recherche 
     */
    function searchLightUrl(params) {

      // Numéro de page par défaut = 1; Nombre d'éléments par défaut = 20;
      var url = "/api/" + params.model + "/SearchLight/?page=1&pageSize=20" +
        "&recherche=" + params.searchedText +
        "&societeId=" + params.societeId +
        "&ciId=" + params.ciId +
        "&groupeId=" + params.groupeId +
        "&organisationId=" + params.organisationId;

      return url;
    }

  }
})();