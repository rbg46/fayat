(function () {
    'use strict';

    angular.module('Fred').service('OrganisationRelatedFeatureService', OrganisationRelatedFeatureService);

    OrganisationRelatedFeatureService.$inject = ['$http', '$q'];

    /**
     * Service de gestion des feature liées au société
     *  @param {any} $http http module
     *  @param {$q} $q $q module

    */
    function OrganisationRelatedFeatureService($http, $q) {
        var service = this;

        service.IsEnabledForCurrentUser = function (featureKey, defaultValue) {

            var deferred = $q.defer();

            var url = '/api/OrganisationRelatedFeature/IsEnabledForCurrentUser/' + featureKey + '/' + defaultValue;

            var storageFeatcureName = url;

            var sessionStorageItem = sessionStorage.getItem(storageFeatcureName);

            if (sessionStorageItem !== null) {

                var result = JSON.parse(sessionStorageItem);

                deferred.resolve({
                    data: result
                });

            } else {

                var callPromise = $http({
                    method: 'GET',
                    url: url
                });

                callPromise
                    .then(function (response) {
                        var sessionStorageItem = JSON.stringify(response.data);
                        sessionStorage.setItem(storageFeatcureName, sessionStorageItem);
                        deferred.resolve({
                            data: response.data
                        });
                    })
                    .catch(function (error) {
                        deferred.reject(error);
                    });
            }

            return deferred.promise;
        };
    }
})();
