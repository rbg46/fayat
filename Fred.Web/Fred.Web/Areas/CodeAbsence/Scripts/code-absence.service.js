(function () {
  'use strict';

  angular.module('Fred').service('CodeAbsenceService', CodeAbsenceService);

  CodeAbsenceService.$inject = ['$http'];

  function CodeAbsenceService($http) {  
    return {
      /* Service de création d'une nouvelle instance de code absence */
      New: function (societeId) {
        return $http.get('/api/CodeAbsence/New/' + societeId, { cache: false });
      },
      /* Service de recherche */
      Search: function (filters, searchText, societeId) {
        return $http.post('/api/CodeAbsence/SearchCodeAbsenceAllBySocieteId/' + societeId + "/" + searchText, filters, { cache: false });
      },
      /* Service de test d'existence du code CodeAbsence */
      Exists: function (idCourant, codeCodeAbsence, societeId) {
        return $http.get('/api/CodeAbsence/CheckExistsCode/' + codeCodeAbsence + '/' + idCourant + '/' + societeId + '/', codeCodeAbsence, idCourant, societeId, { cache: false });
      },         
      /* Service Create */
      Create: function (model) {
        return $http.post('/api/CodeAbsence/', model, { cache: false });
      },
      /* Service Update */
      Update: function (model) {
        return $http.put('/api/CodeAbsence/', model, { cache: false });
      },    
      /* Service Delete */
      Delete: function (model) {
        return $http.post('/api/CodeAbsence/Delete/', model, { cache: false });
      },      
      isAlreadyUsed : function (id) {
        return $http.get('/api/CodeAbsence/IsAlreadyUsed/' + id);
      }
    };
  }

})();