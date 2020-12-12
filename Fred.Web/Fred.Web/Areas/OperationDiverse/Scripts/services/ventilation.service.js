(function () {
  'use strict';

  angular.module('Fred').service('VentilationService', VentilationService);

  VentilationService.$inject = ['$http'];

  function VentilationService($http) {
    return {
      GetOdListByFamilyAndPeriod: function (ciId, familyId, date) {
        return $http.get('/api/OperationDiverse/GetOdListByFamilyAndPeriod/'+ ciId + '/' + familyId + '/' + moment(date).format('YYYY-MM-DD'));
      }
    };
  }
})();