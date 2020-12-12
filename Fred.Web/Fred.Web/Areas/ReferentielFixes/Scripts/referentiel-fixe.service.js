(function () {
  'use strict';

  angular.module('Fred').service('ReferentielFixeService', ReferentielFixeService);

  ReferentielFixeService.$inject = ['$http'];

  function ReferentielFixeService($http) {
    var baseUrl = '/api/ReferentielFixes';

    var service = {
      getChapitres: getChapitres,
      getChapitreById: getChapitreById,

      getSousChapitres: getSousChapitres,
      getSousChapitreById: getSousChapitreById,

      getResources: getResources,
      getRessourceById: getRessourceById,

      getListTypeRessource: getListTypeRessource,
      getListCarburants: getListCarburants,
      getNextRessourceCode: getNextRessourceCode,

      addChapitre: addChapitre,
      updateChapitre: updateChapitre,
      deleteChapitre: deleteChapitre,

      addSousChapitre: addSousChapitre,
      updateSousChapitre: updateSousChapitre,
      deleteSousChapitre: deleteSousChapitre,

      addRessource: addRessource,
      updateRessource: updateRessource,
      deleteRessource: deleteRessource,

      generateExcel: generateExcel
    };

    return service;

    function getChapitres() {
      return $http.get(baseUrl);
    }

    function getChapitreById(id) {
      return $http.get(baseUrl + '/GetChapitre/' + id);
    }

    function getSousChapitres(chapitreId) {
      return $http.get(baseUrl + '/SousChapitres/' + chapitreId);
    }

    function getSousChapitreById(id) {
      return $http.get(baseUrl + '/GetSousChapitre/' + id);
    }

    function getResources(souschapitreId) {
      return $http.get(baseUrl + '/Ressources/' + souschapitreId);
    }

    function getRessourceById(id) {
      return $http.get(baseUrl + '/GetRessource/' + id);
    }

    function addChapitre(chapitre) {
      var postData = JSON.stringify(chapitre);
      return $http.post(baseUrl + '/AddChapitre', postData);
    }

    function updateChapitre(chapitre) {
      var postData = JSON.stringify(chapitre);
      return $http.put(baseUrl + '/UpdateChapitre', postData);
    }

    function updateSousChapitre(souschapitre) {
      var postData = JSON.stringify(souschapitre);
      return $http.put(baseUrl + '/UpdateSousChapitre', postData);
    }

    function updateRessource(ressource) {
      var postData = JSON.stringify(ressource);
      return $http.put(baseUrl + '/UpdateRessource', postData);
    }

    function addSousChapitre(souschapitre) {
      var postData = JSON.stringify(souschapitre);
      return $http.post(baseUrl + '/AddSousChapitre/', postData);
    }

    function addRessource(ressource) {
      var postData = JSON.stringify(ressource);
      return $http.post(baseUrl + '/AddRessource/', postData);
    }

    function deleteChapitre(item) {
      var chapitreId = item.ChapitreId;
      return $http.delete(baseUrl + '/DeleteChapitre/' + chapitreId);
    }

    function deleteSousChapitre(item) {
      var sousChapitreId = item.SousChapitreId;
      return $http.delete(baseUrl + '/DeleteSousChapitre/' + sousChapitreId);
    }

    function deleteRessource(item) {
      var ressourceId = item.RessourceId;
      return $http.delete(baseUrl + '/DeleteRessource/' + ressourceId);
    }

    function getListTypeRessource() {
      return $http.get(baseUrl + '/GetRessourceTypes');
    }

    function getListCarburants() {
      return $http.get(baseUrl + '/GetListCarburants');
    }

    function getNextRessourceCode(item) {
      return $http.get(baseUrl + '/GetNextRessourceCode/' + item);
    }

    function generateExcel() {
      return $http.post(baseUrl + '/GenerateExcel');
    }
  }

})();