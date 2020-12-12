(function (angular, undefined) {
    'use strict';

    angular.module('Fred').directive('organisationRelatedFeature', ['OrganisationRelatedFeatureService', '$rootScope', function (OrganisationRelatedFeatureService, $rootScope) {
            return {
                restrict: 'E',
                compile: function () {

                    return function ($scope, element, attrs) {
                        OrganisationRelatedFeatureService.IsEnabledForCurrentUser(attrs.key, attrs.defaultValue).then(function (result) {
                            if (result.data === false) {
                                element[0].remove();
                            }
                        }).catch();
                    };
                }
            };
        }]);
})(angular);
