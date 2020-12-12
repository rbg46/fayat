(function () {
    'use strict';

    angular.module('Fred').service('GroupeFeatureService', GroupeFeatureService);

    GroupeFeatureService.$inject = ['$http', '$q'];

    function GroupeFeatureService($http, $q) {

        this.loadGroupeFeatures = function () {

            var url = '/api/GroupeFeature';

            return loadContextFor(url);
        };

        function loadContextFor(url) {

            var deferred = $q.defer();

            var storageContextName = url;

            var sessionStorageItem = sessionStorage.getItem(storageContextName);

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
                        sessionStorage.setItem(storageContextName, sessionStorageItem);
                        deferred.resolve({
                            data: response.data
                        });
                    })
                    .catch(function (error) {
                        deferred.reject(error);
                    });
            }

            return deferred.promise;
        }

        this.getFeaturesForGroupe = function () {

            var url = '/api/GroupeFeature';

            return getContextFor(url);
        };

        function getContextFor(url) {

            var storageContextName = url;

            var sessionStorageItem = sessionStorage.getItem(storageContextName);

            var result = JSON.parse(sessionStorageItem);

            return result;
        }
    }
})();
