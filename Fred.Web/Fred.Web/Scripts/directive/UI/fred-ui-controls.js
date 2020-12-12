
(function () {
    'use strict';

    /*A mettre sur le body au retour de vacances ...*/





    angular.module('Fred').directive('escKey', function () {
        return function (scope, element, attrs) {

            element.bind("keydown keypress keyup", function (event) {

                if (event.which === 27) { // 27 = esc key

                    /**
                     * Suppression des controles FRED SIDE
                     */
                    var fred_side = angular.element("Fred-Side");
                    fred_side.removeClass('open');


                    /**
                     * Suppression des controles FRED OVERLAY
                     */
                    var fred_overlay = angular.element("Fred-Overlay");
                    fred_overlay.removeClass('open');


                    scope.$apply(function () {
                        scope.$eval(attrs.escKey);
                    });

                    event.preventDefault();
                }
            });
        };
    });


    /**
     * @author Fayat IT 2018
     * @summary Gestion des tableaux avec une colonne Fixe
     * @todo Revue de code par Alexandre
     * @example <table id="tableFixed" fred-ui-table-fixed>
     */

    angular.module('Fred').directive('fredUiTableFixed', fredUiTableFixed);

    fredUiTableFixed.$inject = ['$http', '$document', '$window', '$timeout'];

    function fredUiTableFixed($http, $document, $window, $timeout) {

        return {
            restrict: 'A',
            transclude: false,

            link: function (scope, iElement, iAttrs) {

                /*Gestion du tableau pour figer la premiere colonne*/
                FredToolBox.manageTable(iElement);

                /*HACK > Force le resizing après un timeout*/
                $timeout(function () {
                    FredToolBox.manageTableResize(iElement, $window);
                }, 100);

                /*redimensionnement si resizing de la page*/
                $($window).resize(function () {
                    FredToolBox.manageTableResize($('#tableFixed'), $window);
                });

            }
            //controller: 'fredUiTableFixedController'
        };

    }

    /**
    * @author Fayat IT 2018
    * @summary Message d'information pour informer qu'il n'y a pas de données
    * @todo Revue de code par Alexandre
    * @example <fred-ui-no-datas message="{{ressources.messages}}"></fred-ui-no-datas>
    */
    angular.module('Fred').directive("fredUiNoDatas", function () {
        return {
            restrict: 'EA',
            replace: true,
            transclude: true,
            scope: {
                message: '@'

            },
            templateUrl: '/Scripts/directive/UI/FredUiNoDatas.tpl.html'
        };
    });


    /**
     * @author Fayat IT 2018
     * @summary Gestion des tableaux avec une colonne Fixe
     * @todo Revue de code par Alexandre
     * @example <fred-ui-detail ng-click="factureCtrl.handleSelectFacture(facture)" title="{{factureCtrl.resources.Facture_Index_Bouton_Detail_Tooltip}} {{facture.NoFMFI}}"></fred-ui-detail>
     */
    angular.module('Fred').directive("fredUiDetail", function () {
        return {
            restrict: 'EA',
            replace: true,
            transclude: true,
            scope: {},
            template: '<i class="action-icone material-icons" aria-hidden="true">more_vert</i>'
        };
    });

    /**
    * @author Fayat IT 2018
    * @summary Génère un bouton Icon Print
    * @todo Revue de code par Alexandre
    * @example <fred-ui-print></fred-ui-print>
    */
    angular.module('Fred').directive("fredUiPrint", function () {
        return {
            restrict: 'EA',
            replace: true,
            transclude: true,
            scope: {},
            template: '<i class="action-icone material-icons" aria-hidden="true">print</i>'
        };
    });


    /**
     * @author Fayat IT 2018
     * @summary Génère un bouton Icon Close
     * @todo Revue de code par Alexandre
     * @example <table id="tableFixed" fred-ui-table-fixed>
     */
    angular.module('Fred').directive("fredUiClose", function () {
        return {
            restrict: 'EA',
            replace: true,
            transclude: true,
            scope: {
                title: '@'

            },
            template: '<i class="action-icone material-icons" aria-hidden="true">close</i>'
        };
    });


    /**
  * @author Fayat IT 2018
  * @summary Génère un bouton Icon
  * @example <fred-ui-icon></fred-ui-icon>
  */
    angular.module('Fred').directive("fredUiIcon", function () {
        return {
            restrict: 'EA',
            replace: true,
            transclude: true,
            scope: {
                icon: '@'

            },
            template: '<i class="action-icone material-icons" aria-hidden="true">{{icon}}</i>'
        };
    });


    /**
    * Directive fredUiTitreDirective Génère le titre de page avec  la gestion de l'affichage de la barre de recherche'
    *      
    * @param {any} title titre de la page
    */
    angular.module('Fred').directive('fredUiTitre', fredUiTitreDirective);

    fredUiTitreDirective.$inject = ['$http', '$document'];

    function fredUiTitreDirective($http, $document) {

        return {
            restrict: 'E',
            transclude: true,
            scope: {
                title: '@'

            },
            template: '<h1 ng-click="toggleState()" class="{{style}}"> {{title}}</h1>',

            link: function (scope, iElement, iAttrs) {
                /*Evènement permettant de changer la variable displaySearch*/
                scope.toggleState = function () {
                    scope.$parent.displaySearch = !scope.$parent.displaySearch;
                    if (!scope.$parent.displaySearch) {
                        scope.style = "reduice";
                    } else {
                        scope.style = "expanded";
                    }
                };
            },
            controller: 'fredUiTitreController'
        };
    }


    /**
    * Controller fredUiTitreController de la directive fredUiTitreDirective
    **/
    angular.module('Fred').controller('fredUiTitreController', fredUiTitreController);

    fredUiTitreController.$inject = ['$scope'];

    function fredUiTitreController($scope) {
        $scope.$parent.displaySearch = true;
        $scope.style = "expanded";

    }

    /**
  * @author Fayat IT 2018
  * @summary Gestion des tables flex avec des headers ligne et de colonne fixes, permet également d'ancrer la table en bas de page.
  * @example <fred-flex-table>
  * @remarks
    - En cas d'ajout dynamique d'une ligne dans la table il faut rafraichir comme ceci :
          document.getElementById("ID DE LA TABLE FLEX").refresh();
    - les headers ligne doivent posséder la class "fixed-line-header"
    - les headers colonne doivent posséder la class "fixed-col-header"
    - si "anchor-bottom" est définit, ancre la table en bas de page avec une marge définie par la valeur de cet attribut (en pixel)
          <fred-flex-table anchor-bottom="50">
    - Pour exemple, les barèmes exploitations utilisent cette table
  */

    angular.module('Fred').directive('fredFlexTable', fredFlexTable);
    fredFlexTable.$inject = ['$window', '$timeout'];
    function fredFlexTable($window, $timeout) {
        return {
            transclude: true,
            replace: true,
            template: '<div ng-transclude></div>',
            scope: {
                anchorBottom: '<?'
            },
            link: function ($scope, $element) {
                var refreshOnScroll = false;
                var dOMSubtreeModified = false;
                var $lineHeaders = $element.find('.fixed-line-header');
                var $lineFooters = $element.find('.fixed-line-footer');
                var $fakeFooter = null;

                // Rafraîchit la table en recallant les headers ligne et colonne à leur bonne position
                // Ceci lors d'un scroll (gestion automatisé) ou d'ajout d'une ligne dans la table (à la charge du développeur)
                var refresh = function () {
                    if (!refreshOnScroll) {
                        return;
                    }
                    let scrollLeft = $element.scrollLeft();
                    let scrollTop = $element.scrollTop();
                    $lineHeaders.css('top', scrollTop);
                    $lineHeaders.find('.fixed-col-header').css('left', scrollLeft);
                    $element.find('.flex-line').find('.fixed-col-header').css('left', scrollLeft);
                    $lineFooters.find('.fixed-col-header').css('left', scrollLeft);
                    if ($fakeFooter !== null) {
                        $fakeFooter.css('left', scrollLeft);
                    }
                    if ($lineFooters.length > 0) {
                        let top = 0;
                        for (let i = $lineFooters.length; i-- > 0;) {
                            top += $lineFooters[i].clientHeight;
                            $($lineFooters[i]).css('top', scrollTop + $element[0].clientHeight - top);
                        }
                    }
                };

                // Ancre la table par rapport au bas de page, si l'attribut "anchor-bottom" est définit
                var anchor = function () {
                    let height = $window.innerHeight - $element.offset().top - $scope.anchorBottom;
                    if (height >= 0) {
                        $element.css('height', height);
                        refresh();
                    }
                };

                // Gère l'évènement "scroll" de la table flex
                $element.on('scroll', function (e) {
                    refresh();
                });

                // Déclenché lors d'une modification du DOM dans la table
                // Cet évènement est obsolète et doit être remplacé par un MutationObserver
                // Le problème est que dans ce cas on observe des sauts, le temps de recalage des éléments est visible
                $element.on('DOMSubtreeModified', function (e) {
                    if (!dOMSubtreeModified) {
                        dOMSubtreeModified = true;
                        $timeout(function () {
                            dOMSubtreeModified = false;
                            refresh();
                        }, 0, false);
                    }
                });

                // Gestion particulière pour Firefox
                // Sous Firefox, le scroll fonctionne très bien avec les touches de navigation et avec l'assenceur de la scroll bar.
                // En revanche, avec la molette de la souris on constate que les headers ligne et colonne scrolle avec le reste du tableau
                //  puis se recalle après... ça saute en gros...
                // Pour éviter celà, on gère l'évènement "DOMMouseScroll" qui est spécifique à Firefox.
                // On scroll alors manuellement puis on annule l'évènement.
                $element.on('DOMMouseScroll', function (e) {
                    // Si la souris se trouve sur un enfant de la table, c'est cet enfant qui doit scroller
                    let target = $(e.target);
                    while (target.get(0) !== this) {
                        let overflow = target.css('overflow');
                        if (overflow && (overflow === 'auto' || overflow === 'scroll')) {
                            return true;
                        }
                        target = target.parent();
                    }

                    if (!e.altKey && !e.ctrlKey && !e.shiftKey) {
                        if (e.originalEvent.axis === 1) {
                            // Scrolling horizontal
                            ($(this)).scrollLeft(($(this)).scrollLeft() + (e.originalEvent.detail * 45));
                        }
                        else if (e.originalEvent.axis === 2) {
                            // Scrolling vertical
                            ($(this)).scrollTop(($(this)).scrollTop() + (e.originalEvent.detail * 45));
                        }
                        return false;
                    }
                });

                // Ajoute la fonction "refresh" à la table
                $element[0].refresh = function () {
                    // https://stackoverflow.com/questions/22733422/angularjs-rootscopeinprog-inprogress-error?utm_medium=organic&utm_source=google_rich_qa&utm_campaign=google_rich_qa
                    let phase = this.scope.$root.$$phase;
                    if (phase === '$apply' || phase === '$digest') {
                        refresh();
                    } else {
                        this.scope.$apply();
                        refresh();
                    }
                };

                // Calcule la hauteur minimum de la table
                // NPI : le calcul est bon sauf si la scrollbar horizontale est affichée
                let minHeight = 0;
                for (let i = 0; i < $lineHeaders.length; i++) {
                    minHeight += $lineHeaders[i].offsetHeight;
                }
                for (let i = 0; i < $lineFooters.length; i++) {
                    minHeight += $lineFooters[i].offsetHeight;
                }
                $element.css('min-height', minHeight + 50);

                // Gère les footers
                if ($lineFooters.length > 0) {
                    let footersHeight = 0;
                    for (let i = 0; i < $lineFooters.length; i++) {
                        footersHeight += $lineFooters[i].clientHeight;
                    }
                    $element.append('<div class="flex-line flex-fake-footer" style="height:' + footersHeight + 'px"></div>');
                    $fakeFooter = $element.find('.flex-fake-footer');
                    $element.on('hidden.bs.collapse', function (e) {
                        var scrollTop = $element.scrollTop();
                        var scrollPosition = $element.scrollTop() + $element[0].clientHeight;
                        if (scrollPosition > $fakeFooter[0].offsetTop) {
                            $element.animate({
                                scrollTop: scrollTop - $lineFooters[0].offsetTop + $fakeFooter[0].offsetTop
                            }, 0);
                            refresh();
                        }
                    });
                }

                // Gère l'ancre par rapport au bas de page
                let elementTop = $element.offset().top;
                if ($scope.anchorBottom) {
                    $scope.$watch(function () { return $element.offset().top !== elementTop; }, function () { elementTop = $element.offset().top; anchor(); });
                    $timeout(function () { anchor(); }, 0);
                    $window.onresize = function () { anchor(); };
                }

                // Rafraîchit la table
                $timeout(function () {
                    refreshOnScroll = true;
                    refresh();
                }, 0, false);
            }
        };
    }
})();
