(function () {
  'use strict';

  angular.module('Fred').service('PaysService', PaysService);

  PaysService.$inject = ['$http'];

  /**
   * Service des PAYS Fred
   * @param {any} $http injection http
   */
  function PaysService($http) {
    var vm = this;

    vm.GetByLibelle = function (libelle) {
      return $http.get("/api/Pays/" + libelle);
      };

      vm.GetByCode = function (code) {
          return $http.get("/api/Pays/Code/" + code);
      };
  }
})();

