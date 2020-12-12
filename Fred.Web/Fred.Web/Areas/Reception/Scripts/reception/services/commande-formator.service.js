
(function () {
    'use strict';

    angular.module('Fred').service('CommandeFormatorService', CommandeFormatorService);

    CommandeFormatorService.$inject = [
        '$filter',
        'CommandeLigneVisibilityService',
        'CommandeLigneIsReceptionnableService',
        'CommandeLigneCommonService'];

    function CommandeFormatorService($filter,
        CommandeLigneVisibilityService,
        CommandeLigneIsReceptionnableService,
        CommandeLigneCommonService ) {

        this.formatCommandeAndChildren = function (cmd, filterOnlyCommandeWithAtLeastOneCommandeLigneLockedIsSelected) {

            cmd.isSelected = false;

            // Parcours des lignes de commande
            angular.forEach(cmd.Lignes, function (cmdLigne) {
                cmdLigne.isSelected = false;
                cmdLigne.numeroCommande = cmd.Numero;
                cmdLigne.numeroCommandeExterne = cmd.NumeroCommandeExterne;
                cmdLigne.codeLibelleFournisseur = cmd.Fournisseur.CodeLibelle;

                // Type de ligne
                cmdLigne.IsReceptionnable = CommandeLigneIsReceptionnableService.isReceptionnable(cmdLigne);   
                cmdLigne.IsCommande = CommandeLigneCommonService.isClassicCommandeLigne(cmdLigne);
                cmdLigne.IsAvenantValide = CommandeLigneCommonService.isAvenantLigneValide(cmdLigne);
                cmdLigne.IsAvenantNonValide = CommandeLigneCommonService.isAvenantLigneNonValide(cmdLigne);

                cmdLigne.IsVisible = CommandeLigneVisibilityService.getIfCommandeLigneIsVisible(cmdLigne, filterOnlyCommandeWithAtLeastOneCommandeLigneLockedIsSelected);


                // Parcours des réceptions des lignes de commande
                angular.forEach(cmdLigne.DepensesReception, function (rcpt) {
                    rcpt.Date = $filter('toLocaleDate')(rcpt.Date);
                    rcpt.DateRapprochement = $filter('toLocaleDate')(rcpt.DateRapprochement);
                    rcpt.DateCreation = $filter('toLocaleDate')(rcpt.DateCreation);
                    rcpt.DateModification = $filter('toLocaleDate')(rcpt.DateModification);
                    rcpt.DateSuppression = $filter('toLocaleDate')(rcpt.DateSuppression);
                });

            });

        };

    }
})();
