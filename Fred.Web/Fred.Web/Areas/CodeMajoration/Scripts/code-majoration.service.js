(function () {
  'use strict';

  angular.module('Fred').service('CodeMajorationService', CodeMajorationService);

  CodeMajorationService.$inject = ['$http'];

  function CodeMajorationService($http) {

    return {     
      GetNew: function () {
        return $http.get('/api/CodeMajoration/GetNew');
      },
      GetList: function (recherche) {
        return $http.get('/api/CodeMajoration/ListCodesMajoration/' + recherche);
      },
      New: function (codeMajoration) {
        return $http.post('/api/CodeMajoration/', codeMajoration);
      },
      Update: function (codeMajoration) {
        return $http.put('/api/CodeMajoration/', codeMajoration);
      },     
      Delete: function (model) {
        return $http.post('/api/CodeMajoration/Delete/', model, { cache: false });
      },
      /* Service de test d'existence du code du code majoration sur un groupe donné */
      Exists: function (idCourant, codeMajoration) {
        return $http.get('/api/CodeMajoration/CheckExistsCode/' + codeMajoration + '/' + idCourant + '/', { cache: false });
      },
      isAlreadyUsed : function (id) {
        return $http.get('/api/CodeMajoration/IsAlreadyUsed/' +id);
      }
  };
}

})();