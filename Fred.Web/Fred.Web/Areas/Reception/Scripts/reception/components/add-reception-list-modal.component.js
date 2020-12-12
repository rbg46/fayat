(function (angular) {
    'use strict';

    var addReceptionListComponent = {
        templateUrl: '/Areas/Reception/Scripts/reception/components/add-reception-list-modal.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: 'AddReceptionListModalController'
    };

    angular.module('Fred').controller('AddReceptionListModalController', AddReceptionListModalController);

    angular.module('Fred').component('addReceptionListComponent', addReceptionListComponent);

    AddReceptionListModalController.$inject = ['$scope',
        '$filter',
        '$timeout',
        'ReceptionService',
        'Notify',
        'ProgressBar',
        '$q',
        'PieceJointeService',
        'PieceJointeValidatorService',
        'AttachementManagerService',
        'MontantQuantitePourcentageCalculatorService',
        'ModelStateErrorManager',
        'CommandeLigneLockService',
        'ConfirmationPopupService'
    ];

    function AddReceptionListModalController($scope,
        $filter,
        $timeout,
        ReceptionService,
        Notify,
        ProgressBar,
        $q,
        PieceJointeService,
        PieceJointeValidatorService,
        AttachementManagerService,
        MontantQuantitePourcentageCalculatorService,
        ModelStateErrorManager,
        CommandeLigneLockService,
        ConfirmationPopupService) {

        var $ctrl = this;

        // Paramètres globaux : Pièces jointes
        $ctrl.attachmentAcceptedFormats = "image/jpeg, image/png, image/jpg, application/pdf, application/msword, application/vnd.openxmlformats-officedocument.wordprocessingml.document, application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, text/plain";
        $ctrl.attachmentMaxSizeForOneFile = 10000;
        $ctrl.attachmentMaxSizeForAllFiles = 10000;
        $ctrl.attachmentMaxNbFiles = 10;
        $ctrl.attachments = null;

        angular.extend($ctrl, {
            // Fonctions publiques
            handleSave: handleSave,
            //handleSaveAndNext: handleSaveAndNext,
            handleCancel: handleCancel,
            handleCancelAndPrevious: handleCancelAndPrevious,
            handleCancelAndNext: handleCancelAndNext,
            handleQuantiteChange: handleQuantiteChange,
            handleLookupSelection: handleLookupSelection,
            handleLookupDeletion: handleLookupDeletion,
            handleChangeDate: handleChangeDate,
            handleParseFloat: handleParseFloat,

            // Pièces jointes
            handleSelectFile: handleSelectFile,
            handleDeleteAttachment: handleDeleteAttachment,
            handleDownloadAttachment: handleDownloadAttachment,

            // Variables                
            receptionForm: {},
            lastReception: null,
            busy: false
        });


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////            INIT         ///////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        $ctrl.$onInit = function () {

            $q.when().then(init);
        };

        async function init() {
            $ctrl.index = 0;
            $ctrl.selectedCommandeLignes = $ctrl.resolve.selectedCommandeLignes;
            $ctrl.resources = $ctrl.resolve.resources;
            $ctrl.data = $ctrl.resolve.data;
            $ctrl.handleShowLookup = $ctrl.resolve.handleShowLookup;

            onStart();

            var commandeLigne = $ctrl.selectedCommandeLignes[$ctrl.index];

            var newReception = await addNewReceptionAsync(commandeLigne);

            $ctrl.reception = newReception;
            $ctrl.reception.numeroCommande = !commandeLigne.numeroCommandeExterne ? commandeLigne.numeroCommande : commandeLigne.numeroCommandeExterne;
            $ctrl.reception.codeLibelleFournisseur = commandeLigne.codeLibelleFournisseur;
            $ctrl.reception.Date = null;

            onFinally();
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////     AJOUT RECEPTION    ///////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        $ctrl.saveButtonIsDisabled = function () {
            return !$ctrl.reception || !$ctrl.reception.Quantite || $ctrl.handleParseFloat($ctrl.reception.Quantite) === 0 || $ctrl.busy;
        };

        function canSave() {
            if ($ctrl.receptionForm.$valid) {
                return true;
            }
            if ($ctrl.receptionForm.$error.required) {
                return false;
            }
            if ($ctrl.receptionForm.$invalid && $ctrl.receptionForm.$error && $ctrl.receptionForm.Quantite.$error.Quantite) {
                return true;
            }
            if ($ctrl.busy) {
                return false;
            }
            return false;
        }

        async function handleSave(saveAndStop) {

            if (!canSave()) {
                return;
            }

            onStart();

            var commandeLigne = $ctrl.selectedCommandeLignes[$ctrl.index];

            var isBlockedResult = await ConfirmationPopupService.rejectOrConfirmSaveIfNecessaryAsync($ctrl.reception, commandeLigne, $ctrl.resources);

            if (isBlockedResult.isBlocked) {
                onFinally();
                return;
            }

            var savedReceptionResult = null;

            try {

                savedReceptionResult = await ReceptionService.Save($ctrl.reception);

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

            actionAddReceptionFront(commandeLigne, savedReception);

            openCommandLine(savedReception.CommandeLigneId);

            await CommandeLigneLockService.updateVerrouOnCommandeLigne(commandeLigne);

            MontantQuantitePourcentageCalculatorService.actionUpdateFigures($ctrl.data);

            if (saveAndStop) {
                $ctrl.close({
                    $value: null
                });
            }
            else {

                $ctrl.lastReception = angular.copy($ctrl.reception);
                $ctrl.reception = null;
                $ctrl.receptionForm.$setPristine();
                $ctrl.Quantite_Error = null;

                $ctrl.index++;

                commandeLigne = $ctrl.selectedCommandeLignes[$ctrl.index];

                var newReception = await addNewReceptionAsync(commandeLigne);

                actionOnGetNewSuccess({
                    CommandeLigne: commandeLigne,
                    Reception: newReception
                });
            }

            //si busy => pas d'erreur sinon addNewReceptionAsync a mis isbusy a false
            if ($ctrl.busy) {
                Notify.message(resources.Global_Notification_Enregistrement_Success);
            }

            onFinally();
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
            else if (error && error.ExceptionMessage) {
                Notify.error(error.ExceptionMessage);
            }
            else {
                Notify.defaultError();
            }
        }

        function actionAddReceptionFront(commandeLigne, reception) {
            commandeLigne.DepensesReception.push(reception);
        }

        function openCommandLine(commandeLigneId) {
            var e = document.querySelector('#TB_LIGNE_' + commandeLigneId);
            angular.element(e).collapse("show");
        }

        async function addNewReceptionAsync(commandeLigne) {

            var newReception = null;

            try {

                newReception = await ReceptionService.New(commandeLigne.CommandeLigneId);
                newReception.Date = $filter('toLocaleDate')(newReception.Date);
            } catch (err) {
                onFinally();
                onSaveError(err);//j'ai eu un prob cote back OU une erreur de validation => je sort
                return null;
            }

            return newReception;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////// SAUVEGARDE  ET CONTINUE ///////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        $ctrl.saveAndNextButtonIsDisabled = function () {
            //
            return !$ctrl.reception || ($ctrl.index === $ctrl.selectedCommandeLignes.length - 1) || !$ctrl.reception.Quantite || $ctrl.handleParseFloat($ctrl.reception.Quantite) === 0 || $ctrl.busy;
        };


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////// ANNULATION ////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /* 
         * @function handleCancel()
         * @description Annulation de la création
         */
        function handleCancel() {
            $ctrl.dismiss({ $value: 'cancel' });
        }

        async function handleCancelAndPrevious() {

            if ($ctrl.busy) {
                return;
            }
            onStart();

            $ctrl.lastReception = angular.copy($ctrl.reception);

            $ctrl.index--;

            var commandeLigne = $ctrl.selectedCommandeLignes[$ctrl.index];

            var newReception = await addNewReceptionAsync(commandeLigne);

            actionOnGetNewSuccess({
                CommandeLigne: commandeLigne,
                Reception: newReception
            });

            onFinally();
        }


        /**
         * Gestion du bouton Annuler et Suivant
         */
        async function handleCancelAndNext() {

            if ($ctrl.busy) {
                return;
            }
            onStart();

            $ctrl.lastReception = angular.copy($ctrl.reception);

            $ctrl.index++;

            var commandeLigne = $ctrl.selectedCommandeLignes[$ctrl.index];

            var newReception = await addNewReceptionAsync(commandeLigne);

            actionOnGetNewSuccess({
                CommandeLigne: commandeLigne,
                Reception: newReception
            });

            onFinally();

        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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

        function handleParseFloat(value) {
            return parseFloat(value);
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
         * @param {any} pieceJointeId : Identifiant de la piece jointe
         * @param {any} libelle Libelle de la pièce jointe
         */
        function handleDownloadAttachment(pieceJointeId, libelle) {
            if (pieceJointeId) {
                PieceJointeService.Download(pieceJointeId);

            } else {
                Notify.message(resources.PieceJointe_Error_FichierNonTeleverse.replace('{0}', libelle));
            }
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            COMMON
         * -------------------------------------------------------------------------------------------------------------
         */


        function onStart() {
            ProgressBar.start();
            $ctrl.busy = true;
        }

        function onFinally() {
            ProgressBar.complete();
            $ctrl.busy = false;
        }

        /**
         * Init la nouvelle réception
         * @param {any} data Données CommandeLigne et Réception
         */
        function actionOnGetNewSuccess(data) {
            $ctrl.reception = data.Reception;
            $ctrl.reception.numeroCommande = !data.CommandeLigne.numeroCommandeExterne ? data.CommandeLigne.numeroCommande : data.CommandeLigne.numeroCommandeExterne;
            $ctrl.reception.codeLibelleFournisseur = data.CommandeLigne.codeLibelleFournisseur;
            $ctrl.attachment = { isReplaced: false, isDeleted: false, file: null, isNew: false };

            if ($ctrl.lastReception.numeroCommande !== $ctrl.reception.numeroCommande) {
                $ctrl.reception.Date = null;
                $ctrl.reception.NumeroBL = null;
            }
            else {
                $ctrl.reception.Date = $ctrl.lastReception.Date;
                $ctrl.reception.NumeroBL = $ctrl.lastReception.NumeroBL;
            }
        }
    }
}(angular));
