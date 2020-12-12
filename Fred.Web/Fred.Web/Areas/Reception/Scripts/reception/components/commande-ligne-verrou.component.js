(function () {
    'use strict';
    var commandeLigneVerrouComponent = {
        templateUrl: '/Areas/Reception/Scripts/reception/components/commande-ligne-verrou.tpl.html',
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
        //////////////////////////////////////////////////////////////////
        // Initialisation                                               //
        //////////////////////////////////////////////////////////////////
        $ctrl.$onInit = function () {

        };

        //////////////////////////////////////////////////////////////////
        // Chargement                                                   //
        //////////////////////////////////////////////////////////////////

        $ctrl.getToolTipWhenLineIsLocked = function () {
            return CommandeLigneLockService.getToolTipWhenLineIsLocked($ctrl.commandeLigne, resources);
        };

        $ctrl.getToolTipWhenLineIsUnlocked = function () {
            return CommandeLigneLockService.getToolTipWhenLineIsUnlocked($ctrl.commandeLigne, resources);
        };


        $ctrl.canShowLockButton = function () {
            return CommandeLigneLockService.canShowLockButton($ctrl.commandeLigne);
        };

        $ctrl.canShowUnLockButton = function () {
            return CommandeLigneLockService.canShowUnLockButton($ctrl.commandeLigne);
        };

        $ctrl.lock = async function () {
            CommandeLigneLockService.lock($ctrl.commandeLigne);
        };

        $ctrl.unLock = async function () {
            CommandeLigneLockService.unLock($ctrl.commandeLigne);
        };

    }
})();
