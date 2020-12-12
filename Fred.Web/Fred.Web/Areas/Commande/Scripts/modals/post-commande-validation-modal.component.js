(function (angular) {
    'use strict';

    angular.module('Fred').component('postCommandeValidationModalComponent', {
        templateUrl: '/Areas/Commande/Scripts/modals/post-commande-validation-modal.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: 'PostCommandeValidationModal'
    });

    angular.module('Fred').controller('PostCommandeValidationModal', PostCommandeValidationModal);

    PostCommandeValidationModal.$inject = ['CommandeService', 'Notify', 'ProgressBar', '$q', 'PieceJointeService'];

    function PostCommandeValidationModal(CommandeService, Notify, ProgressBar, $q, PieceJointeService) {
        var $ctrl = this;

        /*
         * Initilisation du controller de la modal
         */
        $ctrl.$onInit = function () {
            $ctrl.resources = $ctrl.resolve.resources;
            $ctrl.validatedCommande = $ctrl.resolve.commande;
            $ctrl.handleCancel = handleCancel;
            $ctrl.handlePrint = handlePrint;
            $ctrl.newCommandeUrl = '/Commande/Commande/Detail';
            $ctrl.duplicateCommandeUrl = '/Commande/Commande/Detail/' + $ctrl.validatedCommande.CommandeId + '/true';
            $ctrl.commandeListUrl = '/Commande/Commande/Index';
            $ctrl.receptionUrl = '/Reception/Reception/Search?id=' + $ctrl.validatedCommande.Numero + '&commandeSoldee=true';
            $ctrl.headermessage =  $ctrl.resolve.params.firstlabel;
            $ctrl.secondlabel = $ctrl.resolve.params.secondlabel;
        };

        /* 
         * @function handleCancel ()
         * @description Quitte la modal
         */
        function handleCancel() {
            $ctrl.dismiss({ $value: 'cancel' });
        }

        function handlePrint() {
            $q.when()
                .then(ProgressBar.start)
                .then(actionCommandeFormat)
                .then(actionExtractPDFBonDeCommandeEtPieceJointe)
                .then(ProgressBar.complete)
                .finally(function () { $ctrl.close({ $value: 'print done' }); });
        }

        function actionExtractPDFBonDeCommandeEtPieceJointe(commande) {
            // NPI : passer l'id de la commande suffirait ici, mais il y a une histoire avec du cache (a analyser avant modification)
            return CommandeService.ExtractPDFBonDeCommande(commande)
                .then(function (response) {
                    if ((commande.PiecesJointesCommande!==undefined ? commande.PiecesJointesCommande.length : 0) > 0) {
                        for (var i = 0; i < commande.PiecesJointesCommande.length; i++) {
                            PieceJointeService.Download(commande.PiecesJointesCommande[i].PieceJointeId);
                        }
                    }
                    window.location.href = '/api/Commande/ExtractPdfBonDeCommande/' + response.id + "/" + commande.Numero;
                  
                })
                .catch(Notify.defaultError);
        }

        function actionCommandeFormat() {
            var commande = angular.copy($ctrl.validatedCommande);
            commande.Lignes = [];
            for (var i = 0; i < $ctrl.validatedCommande.Lignes.length; i++) {
                var ligne = $ctrl.validatedCommande.Lignes[i];
                if (!ligne.IsAvenantNonValide) {
                    commande.Lignes.push(ligne);
                }
                else {
                    commande.MontantHT -= ligne.MontantHT;
                }
            }
            commande.TotalCommande = commande.MontantHT.toFixed(2);

            return commande;
        }
    }
}(angular));