/*
 * Cette directive change la taille de l'element sur lequelle elle est placée (en hauteur et largeur)
 * par défaut, on ajoute la class 'fred-scroll'
 * Exemple d'utilisation
 *   <div fred-resize></div>    
 */
(function () {
  'use strict';
  angular.module('Fred').directive('fredResize', fredResize);

  fredResize.$inject = ['$window', '$timeout'];

  function fredResize($window, $timeout) {

    return {
      link: link,
      restrict: 'A',
        scope: {
            'onWheelUp': '&onwheelup',
            'onWheelDown': '&onwheeldown'
        },
    };

    function link(scope, element, attrs) {

      var throttleFn = throttle(function () { onResize() }, 500, this);

      attrs.$observe('fredResizeTrigger', function (newval) {
        var toTrigger = (newval == 'true');

        if (toTrigger) { subscribe(); }
        else { unsubscribe(); }
        });

        /* On attache un callback à tous les événements de scrolling  */
        //*cet événement est déclenché seulement si les lignes de pagination ne dépasse pas le contenaire des éléments
        //*et quand a d'autre lignes à ramener
         //* Exemple d'utilisation
         //* <div fred-resize onwheeldown="ctrl.actionLoadMore()"></div>  

        element.bind('mousewheel wheel', function (e) {

            /* On vérifie si l'utilisateur scroll up ou down */
            if (e.originalEvent) {
                e = e.originalEvent;
            }
            var delta = (e.wheelDelta) ? e.wheelDelta : -e.deltaY;
            var isScrollingUp = (e.detail || delta > 0);

            if (!hasVerticalScrollbarVisible(element) )
            {  /* On appelle le bon callback utilisateur */
                if (isScrollingUp) {
                    scope.$apply(scope.onWheelUp());
                } else {
                    scope.$apply(scope.onWheelDown());
                }
                /* On évite que la page scrolle */
                e.preventDefault();
            }
        });

      /**
       * Redimensionnement de l'élément en hauteur
       */
      function onResize() {
        var divHeight = $window.innerHeight - $(element).offset().top - 10;

        if (!$(element).hasClass('fred-scroll')) {
          $(element).addClass('fred-scroll');
        }
        $(element).height(divHeight);
      }
     
      function unsubscribe() {
        angular.element($window).off('resize', throttleFn);
      }

      function subscribe() {
        $timeout(function () { onResize(); angular.element($window).on('resize', throttleFn); }, 1000);
      }

      function hasVerticalScrollbarVisible(element) {
            if (!element) {
                return false;
            }
            var ele = element[0];
            if (!ele || !ele.scrollHeight || !ele.clientHeight) {
                return false;
            }
          return ele.scrollHeight > ele.clientHeight;
        };

      scope.$on('$destroy', unsubscribe);
      subscribe();

    }

  }

})();
