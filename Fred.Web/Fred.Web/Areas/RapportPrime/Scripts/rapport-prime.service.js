(function () {
    'use strict';

    angular.module('Fred').service('RapportPrimeService', RapportPrimeService);

    RapportPrimeService.$inject = ['$http'];

    function RapportPrimeService($http) {
        var service =
        {
            GetRapportPrime: GetRapportPrime,
            AddRapportPrime: AddRapportPrime,
            UpdateRapportPrime: UpdateRapportPrime
        };

        function GetRapportPrime(dateRapportPrime) {
            return $http.get("/api/RapportPrime/date/" + dateRapportPrime);
        }

        function AddRapportPrime() {
            return $http.post("/api/RapportPrime");
        }

        function UpdateRapportPrime(rapportPrimeId, rapportPrimeUpdateModel) {
            return $http.put(`/api/RapportPrime/${rapportPrimeId}`, rapportPrimeUpdateModel);
        }

        return service;
    }
})();