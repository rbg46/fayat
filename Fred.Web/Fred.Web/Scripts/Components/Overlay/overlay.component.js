(function () {
    'use strict';
    angular.module('Fred').component('overlay', {
        templateUrl: '/Scripts/Components/Overlay/overlay.template.html',
        transclude: true,
        bindings: {

            // Class du parent
            class: "@",

            isVisible: "<",

            onRequestClose : '&'

        },
        controller: function () {

            var $ctrl = this;

            // ################# HANDLERS ###################

            $ctrl.handleHide = function () {

                // Request close
                $ctrl.onRequestClose();
            }
        }
    });
})();