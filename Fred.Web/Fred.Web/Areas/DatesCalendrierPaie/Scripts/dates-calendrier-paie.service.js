
(function () {
  'use strict';

  angular
      .module('Fred')
      .service('DatesCalendrierPaieService', DatesCalendrierPaieService);

  DatesCalendrierPaieService.$inject = ['$http', '$q'];

  function DatesCalendrierPaieService($http, $q) {
    var uriBase = "/api/DatesCalendrierPaie/";

    return {
      GetBySocieteAndYear: function (societeId, year) {
        return $q(function (resolve, reject) {
          $http.get(uriBase + societeId + '/' + year, { cache: false })
          .success(function (data) { resolve(data); })
          .error(function (data) { reject(data); });
        });
      },

      AddOrUpdate: function (model) {
        return $q(function (resolve, reject) {
          $http.post(uriBase + 'AddOrUpdate', model, { cache: false })
          .success(function (data) { resolve(data); })
          .error(function (data) { reject(data); });
        });
      }
    };

  }


})();