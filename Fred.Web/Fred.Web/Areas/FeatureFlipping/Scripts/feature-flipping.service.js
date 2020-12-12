(function () {
    'use strict';

    angular.module('Fred').service('FeatureFlippingService', FeatureFlippingService);

    FeatureFlippingService.$inject = ['$http'];

    function FeatureFlippingService($http) {
        return {

            List: function () {
                return $http.post('/api/FeatureFlipping/List');
            },

            Update: function (model) {
                return $http.put('/api/FeatureFlipping/', model, { cache: false });
            }
        };
    }
})();