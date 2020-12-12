(function () {
    'use strict';

    angular.module('Fred').service('CommandeLigneLockService', CommandeLigneLockService);

    CommandeLigneLockService.$inject = [
        'ReceptionService',
        'OrganisationRelatedFeatureService',
        'authorizationService',
        'ModelStateErrorManager',
        'confirmDialog',
        'Notify'];

    function CommandeLigneLockService(ReceptionService,
        OrganisationRelatedFeatureService,
        authorizationService,
        ModelStateErrorManager,
        confirmDialog,
        Notify) {

        var that = this;
        var userCanLockAndUnlock = false;
        var userHasPermissionToLockAndUnLockCommandeLigne = false;
        var userIsInSocieteWithThisFonctionnalite = false;


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////INIT AU CHARGEMENT DE LA PAGE////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        this.initializeIfUserCanShowLockUnLockButton = async function () {
            //je verifie d'abord que j'ai les droit car c'est le plus rapide et que c'est aussi le plus restrictif
            var permission = authorizationService.getPermission(PERMISSION_KEYS.VerrouillageDeverouillageCommandeLigne);
            if (permission !== null && permission.Mode === FONCTIONNALITE_TYPE_MODE.WRITE) {
                userHasPermissionToLockAndUnLockCommandeLigne = true;
            }

            var isEnabledForCurrentUserResult = await OrganisationRelatedFeatureService.IsEnabledForCurrentUser('lock.unlock.commandeLigne', false);
            if (isEnabledForCurrentUserResult && isEnabledForCurrentUserResult.data) {
                userIsInSocieteWithThisFonctionnalite = true;
            }

            if (userHasPermissionToLockAndUnLockCommandeLigne === true && userIsInSocieteWithThisFonctionnalite === true) {
                userCanLockAndUnlock = true;
            }

        };

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////// TOOLTIP  ///////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        this.getToolTipWhenLineIsLocked = function (commandeLigne, resources) {
            if (!userCanLockAndUnlock) {
                return '';
            }
            return commandeLigne.IsVerrou ? resources.Reception_Index_Deverrouiller : "";
        };

        this.getToolTipWhenLineIsUnlocked = function (commandeLigne, resources) {
            if (!userCanLockAndUnlock) {
                return '';
            }
            return !commandeLigne.IsVerrou ? resources.Reception_Index_Verrouiller : "";
        };

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////// DETERMINE SI L UTILISATEUR FAIT PARTIE D UNE SOCIETE AVEC LA FONCTIONNALI////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        this.getIfUserIsInSocieteWithThisFonctionnalite = function (cmd) {
            return userIsInSocieteWithThisFonctionnalite;
        };

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////  AFFICHAGE VERROU      /////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        this.canShowLockButton = function (commandeLigne) {
            return commandeLigne.IsVerrou && userIsInSocieteWithThisFonctionnalite;
        };

        this.canShowUnLockButton = function (commandeLigne) {
            return userCanLockAndUnlock && !commandeLigne.IsVerrou;
        };

        this.canShowFilter = function () {
            return userIsInSocieteWithThisFonctionnalite;
        };

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////  ACTIVATION/DESACTIVATION BOUTTONS D ACTIONS ///////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        this.getIfActionButtonsAreDisables = function (commandeLigne) {
            if (!userIsInSocieteWithThisFonctionnalite) {
                return false;
            }
            return commandeLigne.IsVerrou && userIsInSocieteWithThisFonctionnalite;
        };


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////  AFFICHAGE COMMANDE LIGNE  /////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        this.getIfCommandeLigneIsVisible = function (commandeLigne, filTerOnlyCommandeWithAtLeastOneCommandeLigneLockedIsSelected) {
            if (!userIsInSocieteWithThisFonctionnalite) {
                return true;
            }

            if (filTerOnlyCommandeWithAtLeastOneCommandeLigneLockedIsSelected) {
                return commandeLigne.IsVerrou;
            } else {
                return !commandeLigne.IsVerrou;
            }
        };


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////// ACTION DE VERROUILLAGE DEVERROUILLAGE  ///////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        this.lock = async function (cmdLigne) {

            if (!userCanLockAndUnlock) {
                return;
            }
            var confirDialogResult = await confirmDialog.confirm(resources, resources.Reception_Index_Confirmez_Vous_Le_Verrouillage);

            if (!confirDialogResult) {
                return;
            }

            try {

                await ReceptionService.Lock(cmdLigne.CommandeLigneId);

                cmdLigne.IsVerrou = true;

                Notify.message(resources.Global_Notification_Enregistrement_Success);
            } catch (error) {
                manageError(error);
            }
        };


        this.unLock = async function (cmdLigne) {
            if (!userCanLockAndUnlock) {
                return;
            }
            var confirDialogResult = await confirmDialog.confirm(resources, resources.Reception_Index_Confirmez_Vous_Le_Deverrouillage);

            if (!confirDialogResult) {
                return;
            }

            try {
                await ReceptionService.UnLock(cmdLigne.CommandeLigneId);

                cmdLigne.IsVerrou = false;

                Notify.message(resources.Global_Notification_Enregistrement_Success);
            } catch (error) {
                manageError(error);
            }
        };

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////// MISE A JOUR DE LA PROPRIETE ISVERROU SUR LA LIGNE DE COMMANDE     //////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        this.updateVerrouOnCommandeLigne = async function (commandeLigne) {

            var commandeLigneIsLockedResult = await ReceptionService.IsLocked(commandeLigne.CommandeLigneId);

            var commandeLigneIsLocked = commandeLigneIsLockedResult.data;

            commandeLigne.IsVerrou = commandeLigneIsLocked;
        };

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////// COMMON               ////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        var manageError = function (error) {
            var isVerrouError = ModelStateErrorManager.getError(error, "IsVerrou");
            if (isVerrouError.hasThisError) {
                Notify.error(isVerrouError.firstError);
            }
            else {
                Notify.defaultError();
            }
        };

    }
})();
