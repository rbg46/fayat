(function () {
    'use strict';

    angular.module('Fred').factory('focus', function ($rootScope, $timeout) {

        return (name) => {

            $timeout(function () {

                // Notifier la demande de focus
                $rootScope.$broadcast('focusOn', name);

            });
        };

    });

})();