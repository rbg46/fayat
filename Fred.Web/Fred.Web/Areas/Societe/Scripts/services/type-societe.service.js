(function (angular) {
    'use strict';

    angular.module('Fred').service('TypeSocieteService', typeSocieteService);

    typeSocieteService.$inject = ['$http'];

    function typeSocieteService($http) {

        function GetAll() {
            return $http.get('/api/TypeSociete');
        }

        function GetByCode(code) {
            return $http.get(`/api/TypeSociete/${code}`);
        }

        function GetQueryParamTypeSocieteCodes(selectedTypeSocieteCodes) {
            var i;
            var params = '';

            for (i = 0; i < selectedTypeSocieteCodes.length; i++) {
                params += `typeSocieteCodes=${selectedTypeSocieteCodes[i]}&`;
            }

            return params;
        }

        var typeSocieteCodes = { INTERNE: 'INT', PARTENAIRE: 'PAR', SEP: 'SEP' };

        return {
            GetAll: GetAll,
            GetByCode: GetByCode,
            GetQueryParamTypeSocieteCodes: GetQueryParamTypeSocieteCodes,
            TypeSocieteCodes: typeSocieteCodes
        }
    }

})(angular);