(function () {
  'use strict';

  angular
    .module('Fred')
    .filter('truncateText', function () {
      return function (input, chars) {
        if (isNaN(chars)) return input;
        if (chars <= 0) return '';
        if (input && input.length >= chars) {
          input = input.substring(0, chars);

          while (input.charAt(input.length - 1) === ' ') {
            input = input.substr(0, input.length - 1);
          }
          return input + '...';
        }
        return input;
      };
    });

})();