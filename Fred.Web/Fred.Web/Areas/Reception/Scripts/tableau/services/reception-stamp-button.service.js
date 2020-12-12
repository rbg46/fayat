/*
 * Ce service sert a savoir si on peut afficher le boutton pour visée une reception
 */
(function () {
    'use strict';

    angular.module('Fred').service('ReceptionStampButtonService', ReceptionStampButtonService);

    ReceptionStampButtonService.$inject = ['ReceptionTableauIsVisableService'];

    function ReceptionStampButtonService(ReceptionTableauIsVisableService) {

        var service = {
            canShowButtonStamp: canShowButtonStamp,
            getStyleDisplay: getStyleDisplay
        };
        return service;

        /**
         * @description sert a savoir si on peut afficher le boutton pour visée une reception
         * @param {reception}  reception sur laquelle on cherche a savoir si on peut afficher le boutton pour visée
         * @returns {boolean}  true si on peut afficher le boutton pour visée une reception
         */
        function canShowButtonStamp(reception) {
            return ReceptionTableauIsVisableService.isVisable(reception);
        }

        /**
       * @description sert a savoir le style display a applique sur la cellule
       * @param {reception}  reception sur laquelle on cherche a appliqué le style
       * @returns {string}  flex si on peut afficher,  table-cell si on peut afficher
       */
        function getStyleDisplay(reception) {
            var canShowButton = canShowButtonStamp(reception);
            return canShowButton ? 'flex' : 'table-cell';
        }

    }
})();
