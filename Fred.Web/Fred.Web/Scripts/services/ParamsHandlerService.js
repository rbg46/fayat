(function () {
    'use strict';

    angular.module('Fred').service('ParamsHandlerService', ParamsHandlerService);

    ParamsHandlerService.$inject = ['$http'];

    /**
     * Service de gestion des paramétres
     *  @param {any} $http http module
    */ 
    function ParamsHandlerService($http) {
        var vm = this;      
        vm.GetParamValue = function (organisationId,key) {
            return $http({
                method: 'GET',
                url: '/api/Params/GetParamValue/' + organisationId + '/' + key
            });
        };

        vm.GetParamValues = function (organisationId, key) {
            return $http({
                method: 'GET',
                url: '/api/Params/GetParamValues/' + organisationId + '/' + key
            });
        };
    }
})();

