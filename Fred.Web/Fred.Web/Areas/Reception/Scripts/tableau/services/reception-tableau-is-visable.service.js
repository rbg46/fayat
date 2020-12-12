/*
 * Ce service sert a savoir si reception est visable
 */
(function () {
    'use strict';

    angular.module('Fred').service('ReceptionTableauIsVisableService', ReceptionTableauIsVisableService);

    function ReceptionTableauIsVisableService() {

        var service = {
            isVisable: isVisable
        };
        return service;

        /**
         * @description sert a savoir si une reception est visable
         * @param {reception}  reception sur laquelle on cherche a savoir si on peut viser une reception
         * @returns {boolean}  true si on peut viser une reception
         */
        function isVisable(reception) {
            // faire en sorte que les cases à cocher ne soit jamais affichée dans le cas où "IsReceptionInterimaire" = 1 dans la table "FRED_DEPENSE_ACHAT"
            if (reception.IsReceptionInterimaire === true) {
                return false;
            }

            if (!reception.DateVisaReception) {
                return true;
            }

            if (reception.DateVisaReception && !reception.HangfireJobId) {
                return true;
            }

            return false;
        }

    }
})();
