(function () {
  'use strict';

  angular.module('Fred').service('IndemniteDeplacementService', IndemniteDeplacementService);

  IndemniteDeplacementService.$inject = ['$http', '$q'];

  function IndemniteDeplacementService($http, $q) {
    var uriBase = "/api/IndemniteDeplacement/";

    return {

      /* Service Create */
      Create: function (model) {
        return $q(function (resolve, reject) {
          $http.post(uriBase, model, { cache: false })
            .success(function (data) { resolve(data); })
            .error(function (data) { reject(data); });
        });
      },

      /* Service Update */
      Update: function (model) {
        return $q(function (resolve, reject) {
          $http.put(uriBase, model, { cache: false })
            .success(function (data) { resolve(data); })
            .error(function (data) { reject(data); });
        });
      },

      /* Service Delete */
      Delete: function (modelId) {
        return $q(function (resolve, reject) {
          $http.delete(uriBase + modelId, { cache: false })
            .success(function (data) { resolve(data); })
            .error(function (data) { reject(data); });
        });
      },

      /* Service de création d'une nouvelle instance d'indemnité de déplacement */
      New: function () {
        return $q(function (resolve, reject) {
          $http.get(uriBase + 'New/', { cache: false })
            .success(function (data) { resolve(data); })
            .error(function (data) { reject(data); });
        });
      },

      /* Service GetIndemniteDeplacementByCi */
      GetIndemniteDeplacementByCi: function (idPersonnel, idCi) {
        return $q(function (resolve, reject) {
          $http.get(uriBase + 'GetIndemniteDeplacementByCi/' + idPersonnel + "/" + idCi, { cache: false })
            .success(function (data) { resolve(data); })
            .error(function (data) { reject(data); });
        });
      },

      /* Service GetIndemniteDeplacementByPersonnel */
      GetIndemniteDeplacementByPersonnel: function (idPersonnel) {
        return $q(function (resolve, reject) {
          $http.get(uriBase + 'GetIndemniteDeplacementByPersonnel/' + idPersonnel, { cache: false })
            .success(function (data) { resolve(data); })
            .error(function (data) { reject(data); });
        });
      },

      /* Service Get objet de recherche */
      GetFilter: function () {
        return $q(function (resolve, reject) {
          $http.get(uriBase + 'Filter/', { cache: false })
            .success(function (data) { resolve(data); })
            .error(function (data) { reject(data); });
        });
      },

      /* Service de recherche */
      Search: function (idCi, idPersonnel) {
        return $q(function (resolve, reject) {
          $http.get(uriBase + 'SearchIndemniteDeplacementByCiId/' + idPersonnel + "/" + idCi, { cache: false })
            .success(function (data) { resolve(data); })
            .error(function (data) { reject(data); });
        });
      },

      SearchByPersonnel: function (model, idPersonnel, searchText) {
        return $q(function (resolve, reject) {
          $http.post(uriBase + 'SearchByPersonnel/' + idPersonnel + "/" + searchText, model, { cache: false })
            .success(function (data) { resolve(data); })
            .error(function (data) { reject(data); });
        });
      },

      CalculKm: function (model) {
        return $q(function (resolve, reject) {
          $http.post(uriBase + 'CalculKm/', model, { cache: false })
            .success(function (data) { resolve(data); })
            .error(function (data) { reject(data); });
        });
      },

      IsRolePaie: function () {
        return $q(function (resolve, reject) {
          $http.get('/api/Utilisateur/IsRolePaie', { cache: false })
            .success(function (data) { resolve(data); })
            .error(function (data) { reject(data); });
        });
      }
    };
  }

})();