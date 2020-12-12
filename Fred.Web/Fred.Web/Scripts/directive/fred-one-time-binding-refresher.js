(function (angular) {
    'use strict';

    // Détruit le scope et ses enfants (au digest) et le reconstruit là où la directive est déclarée lorsque
    //  l'expression passée en paramètre change.
    // Permet d'utiliser du one-time-binding afin de réduire le nombre de watcher et donc le temps du digest et de
    //  quand même pouvoir rafraichir le scope.
    // Exemple : <div fred-one-time-binding-refresher="expression"></div>
    // L'expression peut-être par exemple :
    //  - $ctrl.Test                           Si $ctrl.Test change, le scope est détruit puis recréé
    //  - [$ctrl.Test, $ctrl.Test2]            Si $ctrl.Test ou $ctrl.Test2 change, le scope est détruit puis recréé
    //  - $ctrl.Test !== null                  Si $ctrl.Test !== null change, ...
    //  - [$ctrl.Test, $ctrl.Test2 !== 2]      ...
    //  - ...
    //  Voir Mastering $watch in AngularJS : https://www.sitepoint.com/mastering-watch-angularjs/
    //
    // Autre :
    // Angular Performance: Updating bind-once elements : https://www.codelord.net/2016/04/21/angular-performance-updating-bind-once-elements/

    angular.module('Fred').directive('fredOneTimeBindingRefresher', fredOneTimeBindingRefresher);
    function fredOneTimeBindingRefresher() {
        return {
            scope: {
                fredOneTimeBindingRefresher: "="
            },
            transclude: true,
            controller: function ($scope, $transclude, $element) {
                var childScope;
                var element = $element[0];

                if ($scope.fredOneTimeBindingRefresher !== undefined) {
                    $scope.$watch('fredOneTimeBindingRefresher', function (newVal, oldVal) {
                        if (newVal !== oldVal) {
                            element.Refresh();
                        }
                    }, true);
                }

                Transclude();

                element.Refresh = function () {
                    $element.empty();
                    if (childScope) {
                        childScope.$destroy();
                        childScope = null;
                    }
                    Transclude();
                };

                element.GetWatchersCount = function () {
                    return $scope.$$watchersCount;
                };

                function Transclude() {
                    $transclude(function (clone, newScope) {
                        childScope = newScope;
                        $element.append(clone);
                    });
                }
            }
        };
    }
}(angular));
