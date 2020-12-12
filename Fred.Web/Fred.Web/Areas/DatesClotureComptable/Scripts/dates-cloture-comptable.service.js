
(function () {
  'use strict';

  angular.module('Fred').service('DatesClotureComptableService', DatesClotureComptableService);

  DatesClotureComptableService.$inject = ['$http'];

    function DatesClotureComptableService($http) {

        return {
            GetYearAndPreviousNextMonths: function (ciId, year) {
                return $http.get("/api/DatesClotureComptable/GetYearAndPreviousNextMonths/" + ciId + '/' + year, { cache: false });
            },

            Add: function (model) {
                return $http.post('/api/DatesClotureComptable/Add', model);
            },

            Update: function (model) {
                return $http.put('/api/DatesClotureComptable/Update', model);
            },

            GetPreviousCurrentAndNextMonths: function (ciId) {
                return $http.get('/api/DatesClotureComptable/GePreviousCurrentAndNextMonths/' + ciId, { cache: false });
            },

            GetPeriodStatus: function (ciId, year, month) {
                return $http.get('/api/DatesClotureComptable/IsPeriodClosed/' + ciId + '/' + year + '/' + month);
            },

            GetPeriodStatusForRange: function (ciId, startDate, endDate) {
                return $http.get('/api/DatesClotureComptable/IsPeriodClosedForRange/' + ciId + '/' + startDate + '/' + endDate);
            },

            OpenPeriode: function (ciId, dateTimes) {
                return $http.post('/api/DatesClotureComptable/OpenPeriode?ciId=' + ciId, dateTimes , { cache: false });
            }


        };
    }
})();