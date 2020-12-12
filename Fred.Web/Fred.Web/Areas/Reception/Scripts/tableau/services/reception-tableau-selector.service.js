/*
 * Ce service sert à la selection des réceptions à viser et enregistrer dans le tableau des réceptions
 */
(function () {
    'use strict';

    angular.module('Fred').service('ReceptionTableauSelectorService', ReceptionTableauSelectorService);

    ReceptionTableauSelectorService.$inject = ['ReceptionTableauIsVisableService'];

    function ReceptionTableauSelectorService() {

        var allVisableReceptionIds = [];

        this.Initialize = function (allVisableReceptionIdsData) {
            allVisableReceptionIds = allVisableReceptionIdsData;
        };


        /**
         * @description Retourne les nombre de receptions visables
         * @param {isAllSelected} isAllSelected indique si le boutton selectionner tout est activé
         * @param {receptionsOnScreen} receptionsOnScreen les receptions affichées a l'ecran      
         * @returns  {any}  les receptions ids visables
         */
        this.getVisablesReceptionIds = function (isAllSelected, receptionsOnScreen) {

            if (isAllSelected) {
                // Si tout est selectionné alors
                var result = allVisableReceptionIds.slice();//copie du tableau

                angular.forEach(receptionsOnScreen, function (receptionOnScreen) {

                    if (!receptionOnScreen.isReceptionSelected) {
                        const index = result.indexOf(receptionOnScreen.DepenseId);
                        if (index !== -1) {
                            result.splice(index, 1);
                        }
                    }
                });
                return result;
            }
            else {

                var receptionsSelecteds = receptionsOnScreen.filter(function (receptionOnScreen) {
                    return receptionOnScreen.isReceptionSelected === true;

                });

                var receptionIds = receptionsSelecteds.map(function (r) { return r.DepenseId; });

                return receptionIds;
            }
        };

        this.getVisablesReceptionCount = function (isAllSelected, receptionsOnScreen) {
            return this.getVisablesReceptionIds(isAllSelected, receptionsOnScreen).length;
        };


        this.hasReceptionsSelecteds = function (isAllSelected, receptionsOnScreen) {
            return this.getVisablesReceptionCount(isAllSelected, receptionsOnScreen) > 0;
        };

        /**
        * @description Retourne les réceptions sélectionnées
        * @param {any} receptions les receptions
        * @returns {any} selectedReceptions
        */
        this.getVisibleSelectedReceptionIds = function (receptions) {
            var selectedReceptions = [];
            angular.forEach(receptions, function (reception) {
                if (reception.isReceptionSelected) {
                    selectedReceptions.push(reception);
                }
            });
            return selectedReceptions.map(function (r) { return r.DepenseId; });
        };

        /**
        * @description Retourne les réceptions sélectionnées et modifiées, allégées pour le back
        * @param {any} receptions les receptions affichées
        * @returns  {selectedReceptions} receptions allégées
        */
        this.getModifiedReceptions = function (receptions) {
            var selectedReceptions = [];
            angular.forEach(receptions, function (reception) {
                if (reception.isReceptionSelected && reception.isModified) {
                    // Allègement de l'objet envoyé au back 
                    var receptionCopy = angular.copy(reception);
                    receptionCopy.CommandeLigne = null;
                    selectedReceptions.push(receptionCopy);
                }
            });
            return selectedReceptions;
        };

        /**
      * @description Retourne les réceptions IDs sélectionnées et modifiées
      * @param {any} receptions les receptions affichées
      * @returns  {selectedReceptions} receptions Ids
      */
        this.getModifiedReceptionsIds = function (receptions) {

            var selectedReceptions = this.getModifiedReceptions(receptions);

            return selectedReceptions.map(function (r) { return r.DepenseId; });
        };

    }
})();
