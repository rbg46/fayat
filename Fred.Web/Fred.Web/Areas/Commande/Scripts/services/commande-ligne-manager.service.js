(function () {
    'use strict';

    angular.module('Fred').service('CommandeLigneManagerService', CommandeLigneManagerService);

    CommandeLigneManagerService.$inject = ['$timeout', 'CommandeHelperService'];

    function CommandeLigneManagerService($timeout, CommandeHelperService) {

        function addLine(commande, defaultTache) {
            var newRow = angular.copy(commande.Lignes[0]);
            newRow.CommandeLigneId = 0;
            newRow.TacheId = defaultTache ? defaultTache.TacheId : null;
            newRow.Tache = defaultTache;
            newRow.RessourceId = null;
            newRow.Ressource = null;
            newRow.UniteId = null;
            newRow.Unite = null;
            newRow.DepensesReception = null;
            newRow.Libelle = "";
            newRow.Quantite = 0;
            newRow.PUHT = 0;
            newRow.MontantHT = 0;
            newRow.IsCreated = true;
            newRow.IsUpdated = false;
            newRow.IsDeleted = false;
            newRow.IsCommande = true;
            newRow.IsAvenantValide = false;
            newRow.IsAvenantNonValide = false;
            newRow.AvenantLigne = null;
            return newRow;
        }

        /*
        * @description Fonction ajout d'une ligne de commande
        */
        this.handleAddLigneCommande = function (commande, defaultTache) {
            var newRow = addLine(commande, defaultTache);
            commande.Lignes.push(newRow);
            return newRow;
        };

        /*
        * @description Fonction ajout d'une ligne d'avenant
        */
        this.handleAddLigneAvenantNonValide = function (commande, defaultTache, avenantViewId) {
            var newRow = addLine(commande, defaultTache);
            newRow.AvenantLigne = { IsDiminution: false };
            newRow.IsCommande = false;
            newRow.IsAvenantValide = false;
            newRow.IsAvenantNonValide = true;
            newRow.ViewId = avenantViewId;
            commande.Lignes.push(newRow);
            return newRow;
        };

        /*
         * @description Fonction de duplication d'une ligne de commande
         */
        this.handleDuplicateLigneCommande = function (commande, row) {
            var newRow = angular.copy(row);
            newRow.CommandeLigneId = 0;
            newRow.IsCreated = true;
            newRow.IsUpdated = false;
            newRow.IsDeleted = false;
            commande.Lignes.push(newRow);
            CommandeHelperService.actionCalculTotalCommandeLignes(commande);
            CommandeHelperService.actionCalculTotalCommande(commande);
            return newRow;
        };

        /*
        * @description Fonction de duplication d'une ligne d'avenant
        */
        this.handleDuplicateLigneAvenantNonValide = function (commande, row, avenantViewId) {
            var newRow = angular.copy(row);
            newRow.CommandeLigneId = 0;
            newRow.ViewId = avenantViewId;
            newRow.IsCreated = true;
            newRow.IsUpdated = false;
            newRow.IsDeleted = false;
            commande.Lignes.push(newRow);
            CommandeHelperService.actionCalculTotalAvenantNonValideLignes(commande);
            CommandeHelperService.actionCalculTotalCommande(commande);
            return newRow;
        };

        /*handleValidateCommande
        * @description Fonction de mise à jour d'une ligne de commande
        */
        this.handleUpdateLigneCommande = function (commande, ligne) {
            if (ligne) {
                ligne.IsUpdated = ligne.CommandeLigneId > 0 ? true : false;
            }

            CommandeHelperService.actionCalculLigneMontantHT(ligne);
            CommandeHelperService.actionCalculTotalCommandeLignes(commande);
            CommandeHelperService.actionCalculTotalCommande(commande);
        };

        /*
         * @description Fonction de mise à jour d'une ligne d'avenant
         */
        this.handleUpdateLigneAvenantNonValide = function (commande, ligne) {
            ligne.IsUpdated = !ligne.IsCreated;
            CommandeHelperService.actionCalculLigneMontantHT(ligne);
            CommandeHelperService.actionCalculTotalAvenantNonValideLignes(commande);
            CommandeHelperService.actionCalculTotalCommande(commande);
        };

        /*
         * @description Fonction Suppression d'une ligne de commande
         */
        this.handleDeleteLigneCommande = function (commande, row, index) {
            row.IsDeleted = true;
            // si id = 0 and IsDeleted +> suppression du tableau
            if (row.CommandeLigneId === 0 && row.IsDeleted) {
                commande.Lignes.splice(index, 1);
            }
            CommandeHelperService.actionCalculTotalCommandeLignes(commande);
            CommandeHelperService.actionCalculTotalCommande(commande);
        };

        /*
        * @description Fonction Suppression d'une ligne d'avenant
        */
        this.handleDeleteLigneAvenantNonValide = function (commande, row) {
            if (row.IsCreated) {
                for (var i = 0; i < commande.Lignes.length; i++) {
                    var ligne = commande.Lignes[i];
                    if (!ligne.IsDeleted && ligne.IsAvenantNonValide && ligne.ViewId === row.ViewId) {
                        commande.Lignes.splice(i, 1);
                        break;
                    }
                }
            }
            else {
                row.IsDeleted = true;
            }

            CommandeHelperService.actionCalculTotalAvenantNonValideLignes(commande);
            CommandeHelperService.actionCalculTotalCommande(commande);
        };

        /*
         * @description Fonction de comptage du nombre de ligne de commande
         */
        this.handleCountLigneCommande = function (commande) {
            var nbRows = 0;
            for (var n = 0; n < commande.Lignes.length; n++) {
                 var ligne = commande.Lignes[n];
                 if (!ligne.IsDeleted && ligne.IsCommande) {
                        nbRows++;
                 }
            }
            return nbRows;
        };

        /*
        * @description Fonction de comptage du nombre de ligne d'avenant non validé et non supprimé
        */
        this.handleCountLigneAvenantNonValide = function (commande) {
            var nbRows = 0;
            for (var n = 0; n < commande.Lignes.length; n++) {
                var ligne = commande.Lignes[n];
                if (!ligne.IsDeleted && ligne.IsAvenantNonValide) {
                    nbRows++;
                }
            }
            return nbRows;
        };


    }
})();

