/*
 * Cette directive change la taille de l'element sur lequelle elle est placée.
 * Exemple d'utilisation
 *   <div class="rapports-scroll"
             change-height-on-resize='{"size":480,"isActive":{{rapportsContainerSpreadsheet===false}}}'>
    Elle est active seulement si l'object passé en paramettre a l'attribut 'isActive' = true
    On soustrairera la taille de l'ecran moins l'attribut 'size' pour donner la nouvelle taille de l'element.
 */
(function () {
  'use strict';
  angular.module('Fred').directive('changeHeightOnResize', changeHeightOnResizeDirective);

  changeHeightOnResizeDirective.$inject = ['$window'];

  function changeHeightOnResizeDirective($window) {

    return {
      link: link,
      restrict: 'A',
      scope: {}
    };
    function link(scope, element, attrs) {

      const LIMIT_HEIGHT = 700;

      attrs.$observe('changeHeightOnResize', function (newval) {
        var attributValue = parseAttribut(newval);
        var isActive = attributValue.isActive;
        if (isActive === true) {
          subscribe();
        }
        if (isActive === false) {
          unsubscribe();
        }
      });

      function onResize() {

        var attributValue = parseAttribut(attrs.changeHeightOnResize);

        attributValue.size -= $window.innerHeight < LIMIT_HEIGHT ? 100 : 0;
        $(element).height($window.innerHeight - attributValue.size);

      };

      function parseAttribut(stringValue) {
        return JSON.parse(stringValue);
      }

      function unsubscribe() {
        angular.element($window).off('resize', throttleFn);
      }

      function subscribe() {
        angular.element($window).on('resize', throttleFn);
      }

      var throttleFn = throttle(function () { onResize() }, 500, this);

      scope.$on('$destroy', unsubscribe);
      onResize();
    }

  }

})();
