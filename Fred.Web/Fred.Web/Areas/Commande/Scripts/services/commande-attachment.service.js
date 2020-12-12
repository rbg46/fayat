(function () {
    'use strict';

    angular.module('Fred').service('CommandeAttachementService', CommandeAttachementService);

    CommandeAttachementService.$inject = ['$timeout',
        'PieceJointeValidatorService',
        'Enums',
        'Notify',
        'PieceJointeService'];

    function CommandeAttachementService($timeout,
        PieceJointeValidatorService,
        Enums,
        Notify,
        PieceJointeService) {

        // Paramètres globaux : Pièces jointes
        var attachmentAcceptedFormats = "image/jpeg, image/png, image/jpg, application/pdf, application/msword, application/vnd.openxmlformats-officedocument.wordprocessingml.document, application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, text/plain";
        var attachmentMaxSizeForOneFile = 10000;
        var attachmentMaxSizeForAllFiles = 10000;
        var attachmentMaxNbFiles = 10;

        ///**
        //* Vérifier si on peut ajouter une pièce jointe
        //* @param {any} event Event de click sur le bouton ajouter une pièce jointe
        //* @param {any} element Bouton ajout pièce jointe
        //*/
        this.handleCheckBeforeSelectFile = function (event, element, commande, resources) {

            // Réinitialiser la sélection des fichiers
            element.val(null);

            // Si la commande est clôturée : Alors afficher un message d'erreur et annuler l'action
            if (commande.IsStatutCloturee) {
                Notify.message(resources.Commande_Details_Controller_MessageErreurAjoutPj);
                event.preventDefault();
                return;
            }
        };


        ///**
        //* Gérer l'ajout de nouvelles pièces jointes
        //* @param {any} event Event de sélection des fichiers
        //*/
        this.handleSelectFile = function (event, attachments) {
            if (event.target.files) {

                for (var i = 0; i < event.target.files.length; i++) {

                    let file = event.target.files[i];

                    // Vérifier la validité de la pièce jointe
                    if (PieceJointeValidatorService.isAttachmentValid(
                        attachments,
                        file,
                        attachmentAcceptedFormats,
                        attachmentMaxSizeForOneFile,
                        attachmentMaxSizeForAllFiles,
                        attachmentMaxNbFiles)
                    ) {

                        let fileToAdd = {
                            IsDeleted: false,
                            IsNew: true,
                            Libelle: file.name,
                            File: file,
                            SizeInKo: PieceJointeValidatorService.getFileSize(file)
                        };

                        // Ajouter pièce jointe
                        attachments.push(fileToAdd);

                    }
                }
                // Maj vue
                $timeout(angular.noop);
            }
        };


        // /**
        //* Gérer la suppression d'une pièce jointe
        //* @param {any} $index Index correspondant à l'id dans le tableau
        //*/
        this.handleDeleteAttachment = function ($index, attachments) {
            let attachmentTmp = attachments[$index];
            if (attachmentTmp.IsNew) {
                // Si attachement non sauvegardé alors juste le supprimer dans la liste
                attachments.splice($index, 1);
            } else {
                // Sinon le marquer à supprimer pour le back-end
                attachmentTmp.IsDeleted = true;
            }
        };

        ///**
        // * Gérer le téléchargment d'un fichier
        // */
        this.handleDownloadAllAttachments = function (attachments, resources) {
            for (var i = 0; i < attachments.length; i++) {
                this.handleDownloadAttachment(attachments[i].PieceJointeId, attachments[i].Libelle, resources);
            }
        };


        ///**
        // * Gérer le téléchargment de tous les fichiers
        // * @param {any} pieceJointeId Identifiant de la pièce jointe
        // * @param {any} libelle Libellé de la pièce jointe
        // */
        this.handleDownloadAttachment = function (pieceJointeId, libelle, resources) {
            if (pieceJointeId) {
                PieceJointeService.Download(pieceJointeId);

            } else {
                Notify.message(resources.PieceJointe_Error_FichierNonTeleverse.replace('{0}', libelle));
            }
        };


        ///**
        //* Gestion de la pièce jointe
        //* NB: cette action est lancée après la sauvegarde de la commande
        //* @param {any} commandeId identifiant de la commande
        //* @returns {any} commandeId
        //*/
        this.actionSaveOrDeleteAttachment = function (commandeId, attachments) {

            let promises = [];

            (attachments || []).forEach((currentAttachement) => {

                // Cas d'un ajout
                if (currentAttachement.IsNew) {

                    // Ajouter la pièce jointe
                    let promise = PieceJointeService.AddAttachment(
                        Enums.EnumTypeEntite.Commande.Value,
                        commandeId,
                        currentAttachement.Libelle,
                        currentAttachement.File
                    )
                        .success(function (result) {

                            // Maj PieceJointeId de l'entité
                            currentAttachement.IsDeleted = false;
                            currentAttachement.IsNew = false;
                            currentAttachement.PieceJointeId = result.PieceJointeId;

                        })
                        .catch(function () {

                            Notify.error(resources.PieceJointe_Error_AjoutFichier.replace('{0}', currentAttachement.Libelle));

                        });

                    // Add promise
                    promises.push(promise);
                }

                // Cas d'une suppresssion
                if (currentAttachement.IsDeleted) {

                    // Supprimer la pièce jointe
                    let promise = PieceJointeService.DeleteAttachment(currentAttachement.PieceJointeId)
                        .success(function (result) {

                            // Mettre à jour les données du controller
                            attachments = attachments.filter(function (attachmentTmp) {
                                if (attachmentTmp.PieceJointeId === currentAttachement.PieceJointeId) {
                                    return false;
                                }
                                return true;
                            });
                        })
                        .catch(function () {

                            // Afin de réafficher si erreur
                            currentAttachement.IsDeleted = false;
                            Notify.error(resources.PieceJointe_Error_SuppressionFichier.replace('{0}', currentAttachement.Libelle));
                        });

                    // Add promise
                    promises.push(promise);
                }
            });

            // Attendre toutes les promises
            return Promise.all(promises).then(function () {
                // Result
                return { 
                    commandId: commandeId, 
                    attachments : attachments
                };
            });
        };

    }
})();

