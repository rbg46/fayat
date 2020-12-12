(function (angular) {
    'use strict';

    angular.module('Fred').filter('thousandSeparator', function () {
        return function (number) {
            if (!angular.isUndefined(number)) {
                var parts = number.split(".");
                var str = parts[0].toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1 ");
                if (parts[1] !== undefined) {
                    str += "." + parts[1];
                }
                return str;
            }
        };
    });
    
    angular.module('Fred').filter('customDecimal', function () {
        return function (number, minDecimal, maxDecimal) {
            if (!angular.isUndefined(number) && minDecimal && maxDecimal) {

                // TFS 6295
                if (typeof number === 'string') {
                    number = Number(number);
                }

                var n = number.round(maxDecimal);
                var array = n.toString().split(".");

                if (array[1]) {
                    if (array[1].length < maxDecimal) {
                        return n.toFixed(minDecimal);
                    }
                }
                else {
                    return n.toFixed(minDecimal);
                }

                return n;
            }
        };
    });

})(angular);