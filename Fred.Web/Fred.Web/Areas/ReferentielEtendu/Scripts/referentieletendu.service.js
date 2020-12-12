
(function () {
  'use strict';
  angular.module('Fred').service('ReferentielEtenduService', ReferentielEtenduService);

  ReferentielEtenduService.$inject = ['$http'];

  function ReferentielEtenduService($http) {
    var uriBase = "/api/ReferentielEtendu/";

    return {
      manageReferentielEtenduList: function (list) {
        return $http.post(uriBase + 'ManageReferentielEtenduList', list);
      },
      get: function (societeId) {
        return $http.get(uriBase + societeId);
      },
      exportToExcel: function (societeId) {
        return $http.get(uriBase + 'GenerateExcel/' + societeId);
      },
      isLimitationUnitesRessource : function (societeId) {
        return $http.get('/api/Societe/IsLimitationUnitesRessource/' + societeId);
      }
    };
  }
})();
