(function () {
  'use strict';

  angular.module('Fred').service('FactureService', FactureService);

  FactureService.$inject = ['$http', '$resource'];

  function FactureService($http, $resource) {
    var vm = this;

    var uriBase = "/api/Facture/";

    vm.ScanURL = function (factureId) {
      return $http.get("/api/Facture/URLFacture/" + factureId);
    };

    var resource = $resource(uriBase + ':cmd/:factureId',
        {}, //parameters default
        {
          Search: {
            method: "POST",
            url: uriBase + 'SearchWithFilters/:page/:pageSize',
            //params: { cmd: 'Filter' },
            isArray: true
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

          Update: {
            method: "PUT",
            params: {}
          }//,
          //GetById: {
          //  url: uriBase + ':factureId',
          //  method: "GET",
          //  params: { factureiId: 0 },
          //  cache: false
          //},
          //ScanURL: {
          //  url: uriBase + 'URLFacture/:factureId',
          //  method: "GET",
          //  params: { factureId: 0 },
          //  cache: false,
          //}
        }
    );

    angular.extend(vm, resource);
  }
})();