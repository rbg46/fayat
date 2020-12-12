(function () {
    'use strict';

    angular.module('Fred').service('CommandeDiversService', CommandeDiversService);

    CommandeDiversService.$inject = ['$http', '$q'];

    function CommandeDiversService($http, $q) {

        var service =
        {
            GetAll: GetAll,
            GetData: GetData
        };

        return service;

        function GetAll() {
            return $q(function (resolve, reject) {
                $http.get("/api/StatutCommande")
                    .success(function (data) { resolve(data); })
                    .error(function (data) { reject(data); });
            });
        }
        function GetData(url) {
            return $q(function (resolve, reject) {
                $http.get(url)
                    .success(function (data) { resolve(data); })
                    .error(function (data) { reject(data); });
            });
        }


    }
})();