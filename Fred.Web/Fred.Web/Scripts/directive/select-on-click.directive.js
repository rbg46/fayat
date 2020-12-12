(function (angular) {
  'use strict';

  angular.module('Fred').directive('selectOnClick', function () {
    return {
      restrict: 'A',
      link: function (scope, element) {
        var focusedElement;
        element.on('click', function () {
          if (focusedElement !== this) {
            this.select();
            focusedElement = this;
          }
        });
        element.on('blur', function () {
          focusedElement = null;
        });
      }
    };
  });

}(angular));