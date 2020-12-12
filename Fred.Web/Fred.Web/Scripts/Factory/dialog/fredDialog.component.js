(function () {
    'use strict';

    angular.module('Fred').component('fredDialogComponent', {
        templateUrl: '/Scripts/Factory/dialog/fredDialog.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: function () {
            var $ctrl = this;
            $ctrl.$onInit = function () {
                $ctrl.message = $ctrl.resolve.message;
                $ctrl.title = $ctrl.resolve.title;
                $ctrl.titleIcon = $ctrl.resolve.titleIcon;

                $ctrl.leftTextButon = $ctrl.resolve.leftTextButon;
                $ctrl.rightTextButon = $ctrl.resolve.rightTextButon;

                $ctrl.leftActionButon = $ctrl.resolve.leftActionButon ? $ctrl.resolve.leftActionButon : $ctrl.privateOk;
                $ctrl.rightActionButon = $ctrl.resolve.rightActionButon ? $ctrl.resolve.rightActionButon : $ctrl.privateCancel;

                $ctrl.cancel = $ctrl.privateCancel;

                $ctrl.optionTextButon = $ctrl.resolve.optionTextButon;
                $ctrl.optionActionButon = $ctrl.resolve.optionActionButon ? $ctrl.resolve.optionActionButon : $ctrl.privateOption;

                $ctrl.bodyContentStyle = $ctrl.resolve.maxHeight === "0" ? "" : "overflow-y: scroll;max-height: " + $ctrl.resolve.maxHeight + "px;";
                $ctrl.divContentStyle = $ctrl.resolve.divContentStyle === "" ? "modal-body" : $ctrl.resolve.divContentStyle;
            };

            $ctrl.test = function () {
                $ctrl.leftActionButon();
                $ctrl.close({ $value: true });
            };

            $ctrl.privateOk = function () {
                $ctrl.close({ $value: true });
            };

            $ctrl.privateOption = function () {
                $ctrl.close({ $value: { option: true } });
            };

            $ctrl.privateCancel = function () {
                $ctrl.dismiss({ $value: false });
            };
        }
    });

    angular.module('Fred').directive('bindUnsafeHtml', ['$compile', function ($compile) {
        return function (scope, element, attrs) {
            scope.$watch(
                function (scope) {
                    // watch the 'bindUnsafeHtml' expression for changes
                    return scope.$eval(attrs.bindUnsafeHtml);
                },
                function (value) {
                    // when the 'bindUnsafeHtml' expression changes
                    // assign it into the current DOM
                    element.html(value);

                    // compile the new DOM and link it to the current
                    // scope.
                    // NOTE: we only compile .childNodes so that
                    // we don't get into infinite loop compiling ourselves
                    $compile(element.contents())(scope);
                }
            );
        };
    }]);
})();