(function () {
  'use strict';

  angular.module('Fred').service('RemplacementTacheService', RemplacementTacheService);

  RemplacementTacheService.$inject = ['$http'];

  /*
   * @description Service des Remplacements de tâches
   */
  function RemplacementTacheService($http) {    
    var service =
    {
      AddRemplacementTache: AddRemplacementTache,
      DeleteRemplacementTache: DeleteRemplacementTache,
      GetRemplacementTacheHistory: GetRemplacementTacheHistory
        
    };

    /**
     * Requête d'ajout d'une tâche de remplacement
     *  @param {any} remplacementTache la tâche
     * @returns {any} requête http
     */
    function AddRemplacementTache(remplacementTache) {
      return $http.post("/api/RemplacementTache", remplacementTache);
    }

    /**
     * Requête de suppression d'une tâche de remplacement
     * @param {any} remplacementTacheId L'Id de la tâche     
     * @returns {any} requête http
     */
    function DeleteRemplacementTache(remplacementTacheId) {
      return $http.delete("/api/RemplacementTache/" + remplacementTacheId);
    }

    /**
    * Requête de récupération de l'historique des tâches associées à une dépense
    * @param {any} remplacementTacheId L'Id de la tâche
    * @returns {any} requête http
    */
    function GetRemplacementTacheHistory(remplacementTacheId) {
      return $http.get("/api/RemplacementTacheHistory/" + remplacementTacheId);
    }

    return service;
  }
})();