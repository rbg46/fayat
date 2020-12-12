(function () {
    'use strict';

    angular.module('Fred').service('CommandeLigneCommonService', CommandeLigneCommonService);

    CommandeLigneCommonService.$inject = [];

    function CommandeLigneCommonService() {

        this.isAvenantLigne = function (commandeLigne) {
            if (commandeLigne.AvenantLigne !== null) {
                return true;
            }
            return false;
        };

        this.isClassicCommandeLigne = function (commandeLigne) {
            if (commandeLigne.AvenantLigne === null) {
                return true;
            }
            return false;
        };

        this.isAvenantLigneValide = function (commandeLigne) {
            if (this.isAvenantLigne(commandeLigne) && commandeLigne.AvenantLigne.Avenant.DateValidation !== null) {
                return true;
            }
            return false;
        };

        this.isAvenantLigneNonValide = function (commandeLigne) {
            if (this.isAvenantLigne(commandeLigne) && commandeLigne.AvenantLigne.Avenant.DateValidation === null) {
                return true;
            }
            return false;
        };

    }
})();
