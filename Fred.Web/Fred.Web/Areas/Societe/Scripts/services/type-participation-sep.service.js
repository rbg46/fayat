(function (angular) {
    'use strict';

    angular.module('Fred').service('TypeParticipationSepService', typeParticipationSepService);

    typeParticipationSepService.$inject = ['$http'];

    function typeParticipationSepService($http) {
        function GetAll() {
            return $http.get('/api/TypeParticipationSep');
        }

        return {
            GetAll: GetAll
        }
    }

})(angular);