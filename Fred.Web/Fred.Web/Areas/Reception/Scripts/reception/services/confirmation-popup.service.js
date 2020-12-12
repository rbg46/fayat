(function () {
    'use strict';

    angular.module('Fred').service('ConfirmationPopupService', ConfirmationPopupService);

    ConfirmationPopupService.$inject = [
        '$filter',
        'ReceptionService',
        'confirmDialog',
        'fredDialog'
    ];

    function ConfirmationPopupService($filter,

        ReceptionService,
        confirmDialog,
        fredDialog) {

        this.rejectOrConfirmSaveIfNecessaryAsync = async function (reception, commandeLigne, resources) {

            var result = {
                isBlocked: true
            };

            try {
                var isBlockedResult = await ReceptionService.IsCiBlockedInReception(reception.CiId, $filter('date')(reception.Date, 'MM-dd-yyyy'));
            } catch (e) {
                Notify.defaultError();
                result.isBlocked = true;
                return result;
            }

            var isBlockedInReception = isBlockedResult.data;

            if (!isBlockedInReception) {
                result.isBlocked = false;
                return result;
            }

            if (Number(reception.Quantite) < 0) {
                //dans tout les cas je considere que je ne peux pas allez plus loin.
                try {
                    await fredDialog.erreur(resources.Reception_Index_Ligne_Avenant_Negative_Not_Modifiable_On_Closed_Peride);
                } catch (e) {
                    // je ne fait rien, considere comme bloqué
                }
                result.isBlocked = true;
                return result;
            }

            try {
                await confirmDialog.confirm(resources, resources.Reception_Index_ModalConfirmation);
            } catch (e) {
                result.isBlocked = true;
                return result;
            }

            result.isBlocked = false;
            return result;

        };
    }
})();
