(function () {
  'use strict';

  angular.module('Fred').service('PrimeService', PrimeService);

  PrimeService.$inject = ['$http'];

  function PrimeService($http) { 

    return {
      /* Service de création d'une nouvelle instance d'établissement de paie */
      New: function (societeId) {
        return $http.get('/api/Prime/New/' + societeId, { cache: false });
      },     
      Search: function (model, societeId, searchText) {
        return $http.post('/api/Prime/SearchAll/' + societeId + '/' + searchText, model, societeId, searchText, { cache: false });
      },    
      Exists: function (idCourant, code, societeId) {
        return $http.get('/api/Prime/CheckExistsCode/' + code + '/' + idCourant + '/' + societeId, code, idCourant, societeId, { cache: false });
      },   
      Get: function () {
        return $http.get('/api/Prime/All/', { cache: false });
      },    
      Create: function (model) {
        return $http.post('/api/Prime/', model, { cache: false });
      },   
      Update: function (model) {
        return $http.put('/api/Prime/', model, { cache: false });
      },     
      Delete: function (model) {
        return $http.post('/api/Prime/' + 'Delete/', model, { cache: false });
      },
      isAlreadyUsed: function (id) {
        return $http.get('/api/Prime/IsAlreadyUsed/' + id);
      }
    };
  }

})();