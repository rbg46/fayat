(function () {
  'use strict';

  angular.module('Fred').service('CodeDeplacementService', CodeDeplacementService);

  CodeDeplacementService.$inject = ['$http', '$q', '$resource'];

  function CodeDeplacementService($http, $q, $resource) {
    var vm = this;
    var uriBase = "/api/CodeDeplacement/";

    var resource = $resource(uriBase + ':cmd/:societeId',
        {}, //parameters default
        {
          New: {
            method: "GET",
            params: { cmd: "New" },
            cache: false
          },
          Search: {
            method: "POST",
            url: uriBase + 'SearchAll/:societeId/:searchText',
            isArray: true
          },
          GetBySocieteIdAndCode: {
            method: "GET",
            url: uriBase + 'GetBySocieteIdAndCode',
            cache: false

          },
          GetFilter: {
            method: "GET",
            params: { cmd: 'Filter' },
            cache: false
          },
          Get: {
            method: "GET",
            params: { cmd: "All" },
            isArray: true,
            cache: false
          },
          Create: {
            method: "POST",
            params: {}
          },
          Update: {
            method: "PUT",
            params: {}
          },
          Delete: {
            url: uriBase + ':codeDeplacementId',
            method: "DELETE",
            params: { codeDeplacementId: 0 }
          }
        }
    );

    vm.isAlreadyUsed = function (codeDeplacementId) {
      return $http.get('/api/CodeDeplacement/IsAlreadyUsed/' + codeDeplacementId);
    };

    angular.extend(vm, resource);
  }
})();

