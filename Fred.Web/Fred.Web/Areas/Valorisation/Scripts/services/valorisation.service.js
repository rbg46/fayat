(function () {
  'use strict';

  angular.module('Fred').service('ValorisationService', ValorisationService);

  ValorisationService.$inject = ['$http', '$q', '$resource'];

  function ValorisationService($http, $q, $resource) {
    var vm = this;
    var resource = $resource;

    vm.GetList = function (periode, ciId) {
      return $http.get("/api/Valorisation/GetList?periode=" + periode + "&ciId=" + ciId);
    };

    angular.extend(vm, resource);
  }
})();