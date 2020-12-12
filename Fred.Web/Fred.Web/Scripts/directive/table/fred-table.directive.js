(function () {
    'use strict';
    /*
      @description Tableau FRED
      Utilisation :             
    */
    angular.module('Fred').directive('fredTable', fredTable);

    fredTable.$inject = ['$window', '$timeout'];

    function fredTable($window, $timeout) {
        return {
            restrict: 'A',
            link: link,
            scope: {
                rowCount: '=',
                'onWheelUp': '&onwheelup',
                'onWheelDown': '&onwheeldown'
            }
        };

        function link(scope, element, attrs) {
            var thead = angular.element(element[0].querySelector('thead'))[0];
            var tbody = angular.element(element[0].querySelector('tbody'))[0];
            var tfoot = angular.element(element[0].querySelector('tfoot'))[0];

            attrs.$observe('fredTableResizeTrigger', function (newval) {
                var toTrigger = (newval === 'true');

                if (toTrigger) { subscribe(); }
                else { unsubscribe(); }
            });



            /* On attache un callback à tous les événements de scrolling  */
            //*cet événement est déclenché seulement si les lignes de pagination ne dépasse pas le contenaire du tableau
            //*et quand a d'autre lignes à ramener
            //* Exemple d'utilisation
            // *<table fred-table  onwheeldown="ctrl.actionLoadMore()">

            element.bind('mousewheel wheel', function (e) {

                /* On vérifie si l'utilisateur scroll up ou down */
                if (e.originalEvent) {
                    e = e.originalEvent;
                }
                var delta = (e.wheelDelta) ? e.wheelDelta : -e.deltaY;
                var isScrollingUp = (e.detail || delta > 0);

                if (!hasVerticalScrollbarVisible(element)) {  /* On appelle le bon callback utilisateur */
                    if (isScrollingUp) {
                        scope.$apply(scope.onWheelUp());
                    } else {
                        scope.$apply(scope.onWheelDown());
                    }
                    /* On évite que la page scrolle */
                    e.preventDefault();
                }
            });


            scope.$watch(
                function () {
                    var scrollbarWidth = 0, parentWidth;

                    if (tbody.scrollHeight > $(tbody).height()) {
                        scrollbarWidth = tbody.offsetWidth - tbody.clientWidth;
                    }

                    parentWidth = element.parent()[0].clientWidth - scrollbarWidth;

                    $(thead).width(parentWidth);

                    if (tfoot) {
                        $(tfoot).width(parentWidth);
                    }
                   

                    return scope.rowCount;
                }
            );

            /* -------------------------------------------------------------------------------------------------------------
             *                                            RESPONSIVE TABLEAU
             * -------------------------------------------------------------------------------------------------------------
             */
            function toResize() {
                var tableBodyWidth = element.parent()[0].clientWidth;
                var headFootWidth = tableBodyWidth;
                var bodyHeight;

                /* ----------------------------------------- LARGEUR ----------------------------------------------- */

                /* Resize largeur du tableau en fonction la largeur de la page */
                $(element).width(tableBodyWidth);

                // Resize largeur du body du tableau en fonction la largeur de la page
                $(tbody).width(tableBodyWidth);

                if (tbody.scrollHeight > $(tbody).height()) {
                    var scrollbarWidth = tbody.offsetWidth - tbody.clientWidth;
                    headFootWidth -= scrollbarWidth;
                }

                $(thead).width(headFootWidth);

                if (tfoot) {
                    $(tfoot).width(headFootWidth);
                }

                /* ----------------------------------------- HAUTEUR ----------------------------------------------- */

                if (tfoot) {
                    bodyHeight = $window.innerHeight - element.parent().offset().top - $(thead).height() - $(tfoot).height() - 1;
                }
                else {
                    bodyHeight = $window.innerHeight - $(tbody).offset().top - 10;
                }

                $(tbody).height(bodyHeight);
            }

            function unsubscribe() {
                angular.element($window).off('resize', throttleFn);
            }

            function subscribe() {
                $timeout(function () { onScroll(); });
                $timeout(function () {
                    toResize();
                    angular.element($window).on('resize', throttleFn);
                }, 1000);
            }

            var throttleFn = throttle(toResize, 500, this);

            scope.$on('$destroy', unsubscribe);

            subscribe();

            /* -------------------------------------------------------------------------------------------------------------
             *                                            GESTION COLONNES FIXES
             * -------------------------------------------------------------------------------------------------------------
             */

            var selectedColumns = attrs.fredFixedCol ? attrs.fredFixedCol.split(',') : null;

            if (selectedColumns && selectedColumns.length > 0) {

                $(tbody).scroll(onScroll);

            }
            else {
                $(tbody).scroll(function () {
                    var tbodyScrollLeft = $(tbody).scrollLeft();
                    $(thead).css('left', -tbodyScrollLeft);
                    if (tfoot) {
                        $(tfoot).css('left', -tbodyScrollLeft);
                    }
                });
            }

            function onScroll() {
                var tbodyScrollLeft = $(tbody).scrollLeft();
                $(thead).css('left', -tbodyScrollLeft);
                if (tfoot) {
                    $(tfoot).css('left', -tbodyScrollLeft);
                }

                angular.forEach(selectedColumns, function (val) {
                    $(thead).find('th:nth-child(' + val + ')').css('left', tbodyScrollLeft);
                    $(tbody).find('td:nth-child(' + val + ')').css('left', tbodyScrollLeft);

                    if (tfoot) {
                        $(tfoot).find('td:nth-child(' + val + ')').css('left', tbodyScrollLeft);
                    }
                });
            }

            function hasVerticalScrollbarVisible(element) {
                if (!element) {
                    return false;
                }
                var ele = element[0];
                if (!ele || !ele.scrollHeight || !ele.clientHeight) {
                    return false;
                }
                return ele.scrollHeight >= ele.clientHeight;
            };
        }
    }
})();
