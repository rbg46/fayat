(function () {
    'use strict';

    angular.module('Fred').directive('fredSelectMeOn', function ($timeout) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {

                scope.$watch(attrs.fredSelectMeOn, function (value) {
                    var val = value || null;
                    if (val) {
                        $timeout(function () {
                            if (element.length > 0) {
                                element[0].focus();
                                element[0].select();
                            }
                        }, 1000);
                    }
                }, true);
            }

        };
    });
})();
