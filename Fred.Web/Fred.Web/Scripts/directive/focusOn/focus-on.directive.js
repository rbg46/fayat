(function () {
    'use strict';

    angular.module('Fred').directive('focusOn', function () {

        return (scope, elem, attr) => {

            scope.$on('focusOn', function (e, name) {

                if (name === attr.focusOn) {
                    elem[0].focus();
                }

            });
        };
    });

})();