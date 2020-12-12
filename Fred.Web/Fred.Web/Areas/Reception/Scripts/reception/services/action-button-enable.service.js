(function () {
    'use strict';

    angular.module('Fred').service('ActionButtonEnableService', ActionButtonEnableService);

    ActionButtonEnableService.$inject = ['CommandeLigneLockService'];

    function ActionButtonEnableService(CommandeLigneLockService) {

        this.getIfCommandeLigneAddButtonIsDisabled = function (commandeLigne) {
            return CommandeLigneLockService.getIfActionButtonsAreDisables(commandeLigne);
        };

        this.getIfReceptionUpdateButtonsIsDisabled = function (commandeLigne, reception) {
            return reception.DateVisaReception || CommandeLigneLockService.getIfActionButtonsAreDisables(commandeLigne);
        };

        this.getIfReceptionDeleteButtonsIsDisabled = function (commandeLigne, reception) {
            return reception.DateVisaReception || CommandeLigneLockService.getIfActionButtonsAreDisables(commandeLigne);
        };

        this.getIfReceptionDuplicateButtonsIsDisabled = function (commandeLigne) {
            return CommandeLigneLockService.getIfActionButtonsAreDisables(commandeLigne);
        };
    }
})();
