(function () {
  'use strict';

  angular.module('Fred').service('MaterielService', MaterielService);

  MaterielService.$inject = ['$http'];

  function MaterielService($http) {

    return {
      /* Service de création d'une nouvelle instance de code absence */
      GetMaterielById: function (MaterielId) {
        return $http.get('/api/Materiel/GetMaterielById/' + MaterielId, { cache: false });
      },
      /* Service de création d'une nouvelle instance de code absence */
      New: function (societeId) {
        return $http.get('/api/Materiel/New/' + societeId, { cache: false });
      },
      /* Service de recherche */
        Search: function(societeId, searchText, pageSize, pageIndex) {
            return $http.post('/api/Materiel/SearchMateriels/' + societeId + '/' + pageSize + '/' + pageIndex + '/' + searchText, { cache: false });
        },
      /* Service de test d'existence du code Materiel */
      Exists: function (idCourant, codeMateriel, societeId) {
        return $http.get('/api/Materiel/CheckExistsCode/' + codeMateriel + '/' + idCourant + '/' + societeId, codeMateriel, idCourant, societeId, { cache: false });
      },
      /* Service Create */
      Create: function (model) {
        return $http.post("/api/Materiel/", model, { cache: false });
      },
      /* Service Update */
      Update: function (model) {
        return $http.put("/api/Materiel/", model, { cache: false });
      },
      /* Service Delete */
      Delete: function (model) {
        return $http.post('/api/Materiel/Delete/', model, { cache: false });
      },
      isAlreadyUsed: function (id) {
        return $http.get('/api/Materiel/IsAlreadyUsed/' + id);
      }
    };

  }

})();