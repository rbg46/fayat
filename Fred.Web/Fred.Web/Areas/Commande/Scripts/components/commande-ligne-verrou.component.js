(function () {
    'use strict';
    var commandeLigneVerrouComponent = {
        templateUrl: '/Areas/Commande/Scripts/components/commande-ligne-verrou.tpl.html',
        bindings: {
            commandeLigne: '='
        },
        controller: commandeLigneVerrouController
    };
    angular.module('Fred').component('commandeLigneVerrouComponent', commandeLigneVerrouComponent);

    commandeLigneVerrouController.$inject = ['CommandeLigneLockService'];

    function commandeLigneVerrouController(CommandeLigneLockService) {

        var $ctrl = this;

        $ctrl.resources = resources;

        $ctrl.canShowLockButton = function () {
            return CommandeLigneLockService.canShowLockButton($ctrl.commandeLigne);
        };
    }
})();
