(function () {
    'use strict';

    angular.module('Fred').service('AttachementManagerService', AttachementManagerService);

    AttachementManagerService.$inject = ['PieceJointeService', 'Enums'];

    function AttachementManagerService(PieceJointeService, Enums) {

        /**
         * Gestion de la pièce jointe         
         * @param {any} data data
         * @returns {any} data
         */
        this.actionSaveOrDeleteAttachment = function (data) {

            (data.attachments || []).forEach((currentAttachement) => {

                // Cas d'un ajout
                if (currentAttachement.IsNew) {

                    // Ajouter la pièce jointe
                    PieceJointeService.AddAttachment(
                        Enums.EnumTypeEntite.Reception.Value,
                        data.Reception.DepenseId,
                        currentAttachement.Libelle,
                        currentAttachement.File
                    )
                        .success(function (result) {

                            // Maj réception correpondante
                            if (data.Reception.PiecesJointesReception === null) {
                                data.Reception.PiecesJointesReception = [];
                            }

                            // Ajout de la pièce jointe
                            data.Reception.PiecesJointesReception.push({
                                PieceJointeId: result.PieceJointeId,
                                ReceptionId: data.Reception.DepenseId
                            });

                        })
                        .catch(function (error) {

                            Notify.error(resources.PieceJointe_Error_AjoutFichier.replace('{0}', currentAttachement.Libelle));

                        });
                }

                // Cas d'une suppresssion
                if (currentAttachement.IsDeleted) {

                    // Supprimer la pièce jointe
                    PieceJointeService.DeleteAttachment(currentAttachement.PieceJointeId)
                        .success(function (result) {

                            // Mettre à jour les données du controller
                            data.Reception.PiecesJointesReception = data.Reception.PiecesJointesReception.filter(function (depenseReception) {
                                if (depenseReception.PieceJointeId === currentAttachement.PieceJointeId) {
                                    return false;
                                }
                                return true;
                            });

                        })
                        .catch(function (error) {

                            // Afin de réafficher si erreur
                            currentAttachement.IsDeleted = false;

                            Notify.error(resources.PieceJointe_Error_SuppressionFichier.replace('{0}', currentAttachement.Libelle));
                        });
                }
            });

            return data;
        };
    }
})();
