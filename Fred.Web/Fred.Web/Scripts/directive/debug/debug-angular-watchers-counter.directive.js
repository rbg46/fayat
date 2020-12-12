(function () {
    'use strict';

    angular.module('Fred').directive('debugAngularWatchersCounter', DebugAngularWatchersCounterDirective);
    DebugAngularWatchersCounterDirective.$inject = ['$rootScope', '$timeout', 'StringFormat'];

    function DebugAngularWatchersCounterDirective($rootScope, $timeout, StringFormat) {
        return {
            restrict: 'E',
            transclude: false,
            replace: true,
            scope: {
                resources: '<'
            },
            templateUrl: '/Scripts/directive/debug/debug-angular-watchers-counter.directive.html',
            controller: function ($scope, $element) {
                $scope.Count = 0;
                $scope.Good = 2000;             // Good if count >= 0 & < 2000
                $scope.Warning = 5000;          // Warning if count >= 2000 & < 5000

                $scope.GetTooltip = function () {
                    return StringFormat.Format(
                        $scope.resources.Debug_Nombre_Watchers_Angular_Tooltip,
                        $scope.Good,
                        $scope.Warning
                    );
                };

                $scope.Refresh = function () {
                    $scope.Count = $rootScope.$$watchersCount;
                };

                function PostDigest(callback) {
                    var unregister = $rootScope.$watch(function () {
                        unregister();
                        $timeout(function () {
                            callback();
                            $scope.$apply();
                            PostDigest(callback);
                        }, 0, false);
                    });
                }

                PostDigest(function () {
                    $scope.Refresh();
                });
            }
        };
    }
})();
