(function () {
    'use strict';

    angular.module('Fred').component('confirmDialogComponent', {
        templateUrl: '/Scripts/Factory/dialog/confirmDialog.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: function () {
            var $ctrl = this;

            $ctrl.$onInit = function () {
                $ctrl.resources = $ctrl.resolve.resources;
                $ctrl.message = $ctrl.resolve.message;
                $ctrl.titleIcon = $ctrl.resolve.titleIcon;
                $ctrl.bodyContentStyle = $ctrl.resolve.maxHeight === "0" ? "" : "overflow-y: scroll;max-height: " + $ctrl.resolve.maxHeight + "px;";
            };

            $ctrl.ok = function () {
                $ctrl.close({ $value: true });
            };

            $ctrl.handleOption = function () {
                $ctrl.close({ $value: { option: true } });
            };

            $ctrl.cancel = function () {
                $ctrl.dismiss({ $value: false });
            };
        }
    });

})();