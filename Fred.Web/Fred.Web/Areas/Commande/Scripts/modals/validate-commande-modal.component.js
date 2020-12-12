(function (angular) {
    'use strict';

    angular.module('Fred').component('validateCommandeModalComponent', {
        templateUrl: '/Areas/Commande/Scripts/modals/validate-commande-modal.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: 'ValidateCommandeModalController'
    });

    angular.module('Fred').controller('ValidateCommandeModalController', ValidateCommandeModalController);

    ValidateCommandeModalController.$inject = ['UtilisateurService','UserService', 'Notify', 'Enums'];

    function ValidateCommandeModalController(UtilisateurService, UserService, Notify, Enums) {
        var $ctrl = this;

        // contexte nécessaire à la picklist des destinataires
        $ctrl.customPicklistContext = {
            authorizedOnlyUrl: ''
        };

        /**
         * Initilisation du controller de la modal
         */
        $ctrl.$onInit = function () {
            // constants
            $ctrl.validateActionValueValidate = "validate";
            $ctrl.validateActionValueSend = "send";
            $ctrl.typeAvisValueAgreement = Enums.EnumTypeAvis.Accord.Value;
            $ctrl.typeAvisValueRefusal = Enums.EnumTypeAvis.Refus.Value;
            $ctrl.typeAvisValueWithoutOpinion = Enums.EnumTypeAvis.SansAvis.Value;

            $ctrl.resources = $ctrl.resolve.resources;
            $ctrl.insufficientThresholdMessage = "";

            $ctrl.validateButtonLibelle = $ctrl.resources.Commande_ValidateCommande_Validate;

            $ctrl.isAvenant = $ctrl.resolve.isAvenant;

            $ctrl.hasValidationRights = false;

            UserService.getCurrentUser().then(function(user) {
                $ctrl.model = {
                    senderId: user.UtilisateurId,
                    commande: $ctrl.resolve.commande,
                    recipient: null,
                    directValidation: false,
                    typeAvis: $ctrl.typeAvisValueWithoutOpinion,
                    comment: null,
                    typeAction: null,
                    validationNotification: null
                };

                $ctrl.isLoading = true;

                UtilisateurService.GetSeuilUtilisateur(user.UtilisateurId, $ctrl.resolve.commande.CiId, $ctrl.resolve.commande.DeviseId)
                    .then(function (result) {
                        var seuilUtilisateur = result.data;
                        $ctrl.hasValidationRights = seuilUtilisateur > $ctrl.resolve.commande.MontantHT;
                        $ctrl.validationAction = $ctrl.hasValidationRights ? $ctrl.validateActionValueValidate : $ctrl.validateActionValueSend;
                        if ($ctrl.hasValidationRights) {
                            $ctrl.handleClickAction(1);
                        } else {
                            $ctrl.handleClickAction(2);
                        }
                        $ctrl.insufficientThresholdMessage = $ctrl.resolve.resources.Commande_ValidateCommande_InsufficientThreshold.replace("{0}", seuilUtilisateur);
                    }).catch(function () {
                        Notify.error($ctrl.resources.Global_Notification_Error);
                    }).finally(function () { $ctrl.isLoading = false;});
            });
        };

        $ctrl.handleClickAction = function (choice) {

            $ctrl.typeAction = choice;
            if (choice === 1) {
                $ctrl.validateButtonLibelle = $ctrl.resources.Commande_ValidateCommande_Validate;
            }
            else {
                $ctrl.validateButtonLibelle = $ctrl.resources.Commande_ValidateCommande_Save;
            }
        };

        $ctrl.isCommentRequired = function() {
            return $ctrl.validationAction === $ctrl.validateActionValueSend && parseInt($ctrl.model.typeAvis) === $ctrl.typeAvisValueRefusal;
        };

        $ctrl.isRecipientRequired = function () {
            return $ctrl.validationAction === $ctrl.validateActionValueSend;
        };

        /**
         * Sélection du destinataire
         * @param {any} response réponse de la picklist
         */
        $ctrl.handleSelectPersonnelForValidation = function (response) {
            $ctrl.model.recipient = response.item;
        };

        /**
        * Désélection du destinataire
        */
        $ctrl.handleDeletePersonnelForValidation = function () {
            $ctrl.model.recipient = null;
        };

        /**
         * Annulation de la modal
         */
        $ctrl.handleCancel = function () {
            $ctrl.dismiss();
        };

        /**
         * Validation de la modal
         */
        $ctrl.handleConfirm = function () {
            if ($ctrl.form.$valid) {
                $ctrl.model.directValidation = $ctrl.hasValidationRights && $ctrl.validationAction === $ctrl.validateActionValueValidate;
                $ctrl.close({ $value: $ctrl.model });
            }
        };
    }
}(angular));