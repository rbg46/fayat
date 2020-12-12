(function () {
    'use strict';

    angular.module('Fred').service('TypeEnergieService', TypeEnergieService);

    TypeEnergieService.$inject = ['$http'];

    function TypeEnergieService($http) {

        var service =
        {
            GetAll: GetAll            
        };

        return service;

        function GetAll() {
            return $http.get(`/api/TypeEnergie`);
        }      
    }
})();
