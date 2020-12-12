(function () {
  'use strict';

  // Action à l'appui sur la touche Entrée
  angular.module('Fred').directive('onEnter', onEnter);

  function onEnter() {
    return {
      restrict: 'A',
      link: function (scope, element, attrs, ngModel) {
        element.bind("keydown keypress", onAction);

        function onAction(event) {
          if (event.which === 13) {
            scope.$apply(function () {
              scope.$eval(attrs.onEnter, { 'event': event });
            });
            event.preventDefault();
          }
        }
      }
    };  
  }
})();