
(function () {
    'use strict';

    angular.module('Fred').service('CommandeLigneVisibilityService', CommandeLigneVisibilityService);

    CommandeLigneVisibilityService.$inject = ['CommandeLigneLockService'];

    function CommandeLigneVisibilityService(CommandeLigneLockService) {

        this.getIfCommandeLigneIsVisible = function (commandeLigne, filTerOnlyCommandeWithAtLeastOneCommandeLigneLockedIsSelected) {

            if (!commandeLigne.IsCommande && !commandeLigne.IsAvenantValide) {
                return false;
            }

            if (CommandeLigneLockService.getIfCommandeLigneIsVisible(commandeLigne, filTerOnlyCommandeWithAtLeastOneCommandeLigneLockedIsSelected)) {
                return true;
            }

            return false;
        };

    }
})();
