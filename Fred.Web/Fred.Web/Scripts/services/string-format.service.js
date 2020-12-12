(function () {
  'use strict';

  angular.module('Fred').service('StringFormat', StringFormat);

  function StringFormat() {
    var service = this;

    // Format une chaîne de caractère comme String.Format en .net
    service.Format = function (format) {
      var args = Array.prototype.slice.call(arguments, 1);
      return format.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] !== 'undefined'
          ? args[number]
          : match;
      });
    };
  }
})();
