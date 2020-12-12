
(function () {
  'use strict';

  angular.module('Fred').service('DeviseService', DeviseService);

  DeviseService.$inject = ['$http'];

  function DeviseService($http) {   

    return {
      /* Service de recherche */
      Search: function (model, searchText) {
        return $http.post('/api/Devise/SearchAll/' + searchText, model, { cache: false });
      },     
      /* Service de création d'une nouvelle instance de devise */
      New: function () {
        return $http.get('/api/Devise/New/', { cache: false });
      },
    
      Create: function (model) {
        return $http.post('/api/Devise/', model, { cache: false });
      },
    
      Update: function (model) {
        return $http.put('/api/Devise/', model, { cache: false });
      },
     
      Delete: function (model) {
        return $http.post('/api/Devise/Delete/', model, { cache: false });
      },
      /* Service de test d'existence du code Devise */
      Exists: function (idCourant, codeDevise) {
        return $http.get('/api/Devise/CheckExistsCode/' + idCourant + '/' + codeDevise, { cache: false });
      },
      isAlreadyUsed: function (id) {
        return $http.get('/api/Devise/IsAlreadyUsed/' + id);
      }
    };
  }
})();