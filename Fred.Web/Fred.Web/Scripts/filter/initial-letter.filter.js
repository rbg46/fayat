(function (angular) {
  'use strict';

  angular.module('Fred').filter('initialLetter', function () {
    return function (input) {
      if (input) {
        input.toUpperCase().replace(/-/g, ' ');
        var result = "";
        var prenoms = input.split(' ');

        if (prenoms.length > 1) {
          angular.forEach(prenoms, function (val) {
            result += val.charAt(0);
          });
        }
        else {
          result = prenoms[0].charAt(0);
        }
        return result;
      }
      return "";
    }
  });
}(angular));