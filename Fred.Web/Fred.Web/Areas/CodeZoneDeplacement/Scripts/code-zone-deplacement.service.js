(function () {
  'use strict';

  angular.module('Fred').service('CodeZoneDeplacementService', CodeZoneDeplacementService);

  CodeZoneDeplacementService.$inject = ['$http', '$q'];

  function CodeZoneDeplacementService($http, $q) {

    var vm = this;
    /* Service de création d'une nouvelle instance de code ZoneDeplacement */
    vm.New = function (societeId) {
      return $http.get('/api/CodeZoneDeplacement/New/' + societeId, { cache: false });
    };
    /* Service de recherche */
    vm.Search = function (model, societeId, searchText) {
      return $http.post('/api/CodeZoneDeplacement/SearchCodeZoneDeplacementAllBySocieteId/' + societeId + '/' + searchText, model, { cache: false });
    };
    /* Service de test d'existence du code CodeZoneDeplacement */
    vm.Exists = function (idCourant, code, societeId) {
      var u = '/api/CodeZoneDeplacement/CheckExistsCode/' + code + '/' + idCourant + '/' + societeId + '/';
      return $http.get(u, { cache: false });
    };
    /* Service Create */
    vm.Create = function (model) {
      return $http.post('/api/CodeZoneDeplacement/', model, { cache: false });
    };
    /* Service Update */
    vm.Update = function (model) {
      return $http.put('/api/CodeZoneDeplacement/', model, { cache: false });
    };
    /* Service Delete */
    vm.Delete = function (model) {
      return $http.post('/api/CodeZoneDeplacement/Delete/', model, { cache: false });
    };
    vm.isAlreadyUsed = function (id) {
      return $http.get('/api/CodeZoneDeplacement/IsAlreadyUsed/' + id);
    };

  }

})();