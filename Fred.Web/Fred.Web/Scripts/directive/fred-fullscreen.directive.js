(function () {
  'use strict';

  angular.module('Fred').directive('fredFullscreen', fredFullscreen);

  fredFullscreen.$inject = ['$window'];

  function fredFullscreen($window) {
    return {
      link: link,
      restrict: 'A'
    };
    function link(scope, element, attrs) {
      var isFullscreen = JSON.parse(attrs.fredFullscreen);

      attrs.$observe('fredFullscreen', function (newval) {
        isFullscreen = JSON.parse(newval);
        toFullscreen();
      });

      /*
       * @description Gestion du mode plein écran
       */
      function toFullscreen() {
        // La div parent doit contenir la class css "container" ou "container-fluid"
        var parentDiv = angular.element(document.querySelector('div[class*="container"]'));
        var width;
        
        if (!parentDiv) {
          console.error("La div parent doit avoir la class css 'container' ou 'container-fluid'");
          return;
        }

        width = parentDiv.width();

        // En mode fullscreen, la hauteur et la largeur sont à 100%
        if (isFullscreen === true) {
          width = $window.innerWidth;
          //$(element).height($window.innerHeight);
        }

        $(element).width(width);
      }

      //  Rafraichissement de l'écran uniquement si la fonction n'a pas déjà été lancée.
      var throttleFn = throttle(toFullscreen, 500, this);

      scope.$on('$destroy', function () { angular.element($window).off('resize', throttleFn); });
      angular.element($window).on('resize', throttleFn);
      toFullscreen();
    }
  }
})();