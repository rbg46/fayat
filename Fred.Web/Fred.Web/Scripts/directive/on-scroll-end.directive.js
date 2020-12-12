(function () {
    'use strict';

    angular.module('Fred').directive('onScrollEnd', function ($timeout, $window) {
        return {
            scope: {
                onScrollEnd: "&"
            },
            restrict: 'A',
            link: function (scope, element, attrs) {

                var raw = element[0];
                var atBottom = false;
                element.bind('scroll', function () {

                    if (raw.scrollTop + raw.offsetHeight >= raw.scrollHeight) {
                        if (!atBottom) {

                            // Appeler la méthode définie sur le scrollEnd
                            scope.$apply(scope.onScrollEnd);

                            // Flag 
                            atBottom = true;
                        }
                    } else {
                        if (atBottom) {

                            // Flag 
                            atBottom = false;
                        }
                    }
                });
            }
        };
    });

})();