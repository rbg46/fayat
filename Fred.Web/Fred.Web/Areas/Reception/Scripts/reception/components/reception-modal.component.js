(function (angular) {
    'use strict';

    var receptionComponent = {
        templateUrl: '/Areas/Reception/Scripts/reception/components/reception-modal.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: 'ReceptionModalController'
    };

    angular.module('Fred').controller('ReceptionModalController', ReceptionModalController);

    angular.module('Fred').component('receptionModalComponent', receptionComponent);

    ReceptionModalController.$inject = ['$scope',
        '$timeout',
        'ReceptionService',
        'Notify',
        'ProgressBar',
        '$filter',
        'PieceJointeService',
        'PieceJointeValidatorService',
        'Enums',
        'CurrentBonLivraisonManagerService',
        'ModelStateErrorManager',
        'AttachementManagerService',
        'CommandeLigneLockService',
        'ConfirmationPopupService'
    ];

    function ReceptionModalController($scope,
        $timeout,
        ReceptionService,
        Notify,
        ProgressBar,
        $filter,
        PieceJointeService,
        PieceJointeValidatorService,
        Enums,
        CurrentBonLivraisonManagerService,
        ModelStateErrorManager,
        AttachementManagerService,
        CommandeLigneLockService,
        ConfirmationPopupService) {

        var $ctrl = this;

        // Paramètres globaux : Pièces jointes
        $ctrl.attachmentAcceptedFormats = "image/jpeg, image/png, image/jpg, application/pdf, application/msword, application/vnd.openxmlformats-officedocument.wordprocessingml.document, application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, text/plain";
        $ctrl.attachmentMaxSizeForOneFile = 10000;
        $ctrl.attachmentMaxSizeForAllFiles = 10000;
        $ctrl.attachmentMaxNbFiles = 10;
        $ctrl.attachments = null;
        $ctrl.Quantite_Error = '';
        var commandeLigne = null;
        var mode = receptionModalModeEnum.ADD;
        angular.extend($ctrl, {
            // Fonctions
            handleSave: handleSave,
            handleCancel: handleCancel,
            handleQuantiteChange: handleQuantiteChange,
            handleLookupSelection: handleLookupSelection,
            handleLookupDeletion: handleLookupDeletion,
            handleChangeDate: handleChangeDate,

            // Pièces jointes
            handleSelectFile: handleSelectFile,
            handleDeleteAttachment: handleDeleteAttachment,
            handleDownloadAttachment: handleDownloadAttachment,

            // Variables                
            receptionForm: {},
            busy: false
        });

        $ctrl.IsModificationRessourceReceptions = false;

        /*
         * Initilisation du controller de la modal
         */
        $ctrl.$onInit = function () {
            mode = $ctrl.resolve.mode;
            $ctrl.reception = $ctrl.resolve.reception;
            $ctrl.resources = $ctrl.resolve.resources;
            $ctrl.currentBL = $ctrl.resolve.currentBL;
            $ctrl.commande = $ctrl.resolve.commande;

            $ctrl.handleShowLookup = $ctrl.resolve.handleShowLookup;
            commandeLigne = $ctrl.resolve.commandeLigne;

            setTitle(mode);

            $ctrl.reception.NumeroBL = CurrentBonLivraisonManagerService.getCurrentBonLivraison(commandeLigne, $ctrl.reception);

            if (!$ctrl.reception.DepenseId) {
                $ctrl.reception.Date = null;
            } else {
                // Charger le pièces jointes si DepenseId existe
                loadPieceJointes($ctrl.reception.DepenseId);
            }

            setUrlRessourcesRecommandeesEnabled();

            ReceptionService.IsLimitationUnitesRessource($ctrl.commande.CI.SocieteId)
                .then(initModificationRessourceReceptions)
                .catch(Notify.defaultError);
        };

        function setTitle(mode) {
            if (mode === receptionModalModeEnum.ADD) {
                $ctrl.modalTitle = $ctrl.resources.Reception_Index_ModalAjout_TitreAjout;
            }
            if (mode === receptionModalModeEnum.UPDATE) {
                $ctrl.modalTitle = $ctrl.resources.Reception_Index_ModalAjout_TitreModification;
            }
        }


        function loadPieceJointes(depenseId) {

            // Charger les pieces jointes
            PieceJointeService.GetAttachements(Enums.EnumTypeEntite.Reception.Value, depenseId)
                .then(function (result) {
                    $ctrl.attachments = result.data;
                })
                .catch(function (error) {
                    Notify.error(resources.PieceJointe_Error_ChargementFichiers);
                });
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////     AJOUT + MODIFICATION RECEPTION    ////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        function canSave() {
            if ($ctrl.receptionForm.$valid) {
                return true;
            }
            if ($ctrl.receptionForm.$invalid && $ctrl.receptionForm.$error && $ctrl.receptionForm.Quantite.$error.Quantite) {
                return true;
            }
            if ($ctrl.busy) {
                return false;
            }
            return false;
        }


        async function handleSave() {

            if (!canSave()) {
                return;
            }
            //evite les problemes techniques
            if (mode !== receptionModalModeEnum.ADD && mode !== receptionModalModeEnum.UPDATE) {
                return;
            }
            onStart();

            CurrentBonLivraisonManagerService.setCurrentBonLivraison($ctrl.reception.NumeroBL);

            CurrentBonLivraisonManagerService.setCurrentNumeroCommande(commandeLigne);

            var isBlockedResult = await ConfirmationPopupService.rejectOrConfirmSaveIfNecessaryAsync($ctrl.reception, commandeLigne, $ctrl.resources);

            if (isBlockedResult.isBlocked) {
                onFinally();
                return;
            }

            var savedReceptionResult = null;

            try {
                if (mode === receptionModalModeEnum.ADD) {
                    savedReceptionResult = await ReceptionService.Save($ctrl.reception);
                }
                if (mode === receptionModalModeEnum.UPDATE) {
                    savedReceptionResult = await ReceptionService.Update($ctrl.reception);
                }

            } catch (err) {
                onFinally();
                onSaveError(err);//j'ai eu un prob cote back OU une erreur de validation => je sort
                return;
            }

            var savedReception = savedReceptionResult.data;

            AttachementManagerService.actionSaveOrDeleteAttachment({
                Reception: savedReception,
                attachments: $ctrl.attachments
            });

            if (mode === receptionModalModeEnum.ADD) {
                actionAddReceptionFront(commandeLigne, savedReception);
                openCommandLine(savedReception.CommandeLigneId);
            }
            if (mode === receptionModalModeEnum.UPDATE) {
                replaceReceptionFront(commandeLigne, savedReception);
                udpdateDate(savedReception);
            }

            await CommandeLigneLockService.updateVerrouOnCommandeLigne(commandeLigne);

            Notify.message(resources.Global_Notification_Enregistrement_Success);

            onFinally();

            $ctrl.close({
                $value: null
            });
        }

        function onSaveError(error) {
            var receptionQuantiteNegativeErrorName = 'Quantite_' + $ctrl.reception.DepenseId;
            var receptionQuantiteNegativeError = ModelStateErrorManager.getError(error, receptionQuantiteNegativeErrorName);
            if (receptionQuantiteNegativeError.hasThisError) {
                $ctrl.receptionForm.Quantite.$setValidity("Quantite", false);
                $ctrl.Quantite_Error = receptionQuantiteNegativeError.firstError;
                return;
            }
            var oneReceptionIsBlockedError = ModelStateErrorManager.getError(error, "Receptions");
            if (oneReceptionIsBlockedError.hasThisError) {
                Notify.error(oneReceptionIsBlockedError.firstError);
            }
            else {
                Notify.defaultError();
            }
        }

        function openCommandLine(commandeLigneId) {
            var e = document.querySelector('#TB_LIGNE_' + commandeLigneId);
            angular.element(e).collapse("show");
        }

        function onStart() {
            ProgressBar.start();
            $ctrl.busy = true;
        }

        function onFinally() {
            ProgressBar.complete();
            $ctrl.busy = false;
        }

        function actionAddReceptionFront(commandeLigne, reception) {
            commandeLigne.DepensesReception.push(reception);
        }

        function replaceReceptionFront(commandeLigne, savedReception) {
            var rcpt = $filter('filter')(commandeLigne.DepensesReception, { DepenseId: savedReception.DepenseId }, true)[0];
            var index = commandeLigne.DepensesReception.indexOf(rcpt);
            commandeLigne.DepensesReception[index] = savedReception;
        }
        function udpdateDate(rcpt) {
            rcpt.Date = $filter('toLocaleDate')(rcpt.Date);
            rcpt.DateRapprochement = $filter('toLocaleDate')(rcpt.DateRapprochement);
            rcpt.DateCreation = $filter('toLocaleDate')(rcpt.DateCreation);
            rcpt.DateModification = $filter('toLocaleDate')(rcpt.DateModification);
            rcpt.DateSuppression = $filter('toLocaleDate')(rcpt.DateSuppression);
            rcpt.DateComptable = $filter('toLocaleDate')(rcpt.DateComptable);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////     
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /* 
         * @function handleCancel()
         * @description Annulation de la création
         */
        function handleCancel() {
            $ctrl.dismiss({ $value: 'cancel' });
        }

        /*
         * @function handleDateValidation
         * @description Valide les dates d'entrée et sortie
         */
        function handleQuantiteChange() {
            if ($ctrl.reception.Quantite !== null) {
                $ctrl.reception.MontantHT = $ctrl.reception.Quantite * $ctrl.reception.PUHT;
            }
        }

        /*
         * @description Gestion de la sélection par la Lookup
         */
        function handleLookupSelection(type, item) {

            switch (type) {
                case "Tache":
                    if (item !== null) {
                        $ctrl.reception.Tache = item;
                        $ctrl.reception.TacheId = item.IdRef;
                    }
                    break;
                case "Ressource":
                    if (item !== null) {
                        $ctrl.reception.Ressource = item;
                        $ctrl.reception.RessourceId = item.IdRef;
                    }
                    break;
            }
        }

        /*
         * @description Fonction suppression d'un item dans la lookup en fonction du type de lookup
         */
        function handleLookupDeletion(type) {

            switch (type) {
                case "Tache":
                    $ctrl.reception.TacheId = null;
                    $ctrl.reception.Tache = null;
                    break;
                case "Ressource":
                    $ctrl.reception.RessourceId = null;
                    $ctrl.reception.Ressource = null;
                    break;
            }
        }

        /*
         * @description Permet de forcer le rafraîchissement de l'input Date
         */
        function handleChangeDate() {
            $timeout(angular.noop);
        }

        function initModificationRessourceReceptions(result) {
            $ctrl.IsModificationRessourceReceptions = result.data;
        }



        function setUrlRessourcesRecommandeesEnabled() {
            $ctrl.resssourcesRecommandeesOnly = 0;
            if ($ctrl.reception.CI && $ctrl.reception.CI.EtablissementComptable && $ctrl.reception.CI.EtablissementComptable.RessourcesRecommandeesEnabled) {
                $ctrl.resssourcesRecommandeesOnly = 1;
            }
        }

        /**
         * Gérer l'ajout de nouvelles pièces jointes
         * @param {any} event Event renvoyé par l'input file
         */
        function handleSelectFile(event) {
            if (event.target.files) {

                for (var i = 0; i < event.target.files.length; i++) {

                    let file = event.target.files[i];

                    // Vérifier la validité de la pièce jointe
                    if (PieceJointeValidatorService.isAttachmentValid(
                        $ctrl.attachments,
                        file,
                        $ctrl.attachmentAcceptedFormats,
                        $ctrl.attachmentMaxSizeForOneFile,
                        $ctrl.attachmentMaxSizeForAllFiles,
                        $ctrl.attachmentMaxNbFiles)
                    ) {

                        let fileToAdd = {
                            IsDeleted: false,
                            IsNew: true,
                            Libelle: file.name,
                            File: file,
                            SizeInKo: PieceJointeValidatorService.getFileSize(file)
                        };

                        // Mettre à jour la liste des pièces jointes
                        if ($ctrl.attachments === null) {
                            $ctrl.attachments = [];
                        }

                        // Ajouter pièce jointe
                        $ctrl.attachments.push(fileToAdd);
                    }
                }
                // Update view
                $timeout(angular.noop);
            }
        }

        /**
         * Gérer la suppression d'une pièce jointe
         * @param {any} $index Index correspondant à l'id dans le tableau
         */
        function handleDeleteAttachment($index) {
            let attachmentTmp = $ctrl.attachments[$index];
            if (attachmentTmp.IsNew) {
                // Si attachement non sauvegardé alors juste le supprimer dans la liste
                $ctrl.attachments.splice($index, 1);
            } else {
                // Sinon le marquer à supprimer pour le back-end
                attachmentTmp.IsDeleted = true;
            }
        }


        /**
         * Gérer le téléchargment de tous les fichiers
         * @param {any} pieceJointeId Identifiant de la pièce jointe
         * @param {any} libelle Le libellé de la pièce jointe
         */
        function handleDownloadAttachment(pieceJointeId, libelle) {
            if (pieceJointeId) {
                PieceJointeService.Download(pieceJointeId);

            } else {
                Notify.message(resources.PieceJointe_Error_FichierNonTeleverse.replace('{0}', libelle));
            }
        }
    }
}(angular));
