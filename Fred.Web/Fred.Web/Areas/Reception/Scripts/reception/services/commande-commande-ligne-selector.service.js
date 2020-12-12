
(function () {
    'use strict';

    angular.module('Fred').service('CommandeCommandeLigneSelectorService', CommandeCommandeLigneSelectorService);

    CommandeCommandeLigneSelectorService.$inject = ['$filter', 'CommandeLigneLockService'];

    function CommandeCommandeLigneSelectorService($filter, CommandeLigneLockService) {

        var that = this;

        var selectedCommandeLignes = [];

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////// SELECTION DESELECTION COMMANDE LIGNES/ ///////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        this.getSelecteLignes = function () {
            return selectedCommandeLignes;
        };

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////// SELECTION DESELECTION COMMANDE ///////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        this.canSelectCommande = function (cmd) {
            for (var i = 0, len = cmd.Lignes.length; i < len; i++) {
                if (that.canSelectCommandeLigne(cmd.Lignes[i])) {
                    return true;
                }
            }
            return false;
        };

        this.handleSelectCommande = function (cmd) {
            if (cmd.isSelected) {
                selectAllCommandeLignes(cmd);
            }
            else {
                unSelectAllCommandeLignes(cmd);
            }
            actionOrderSelectedCommandeLignes();
        };


        function selectAllCommandeLignes(cmd, ) {
            angular.forEach(cmd.Lignes, function (ligne) {
                if (that.canSelectCommandeLigne(ligne)) {
                    ligne.isSelected = true;
                    selectUnSelectCommandeLigne(ligne);
                }
            });

            cmd.isSelected = getIfIsAllCommandeLigneAreSelected(cmd);
        }

        function unSelectAllCommandeLignes(cmd) {
            angular.forEach(cmd.Lignes, function (ligne) {
                if (that.canSelectCommandeLigne(ligne)) {
                    ligne.isSelected = false;
                    selectUnSelectCommandeLigne(ligne);
                }
            });
            cmd.isSelected = getIfIsAllCommandeLigneAreSelected(cmd);
        }

        /**
        * Vérifie si toutes les lignes de commandes d'une commande sont sélectionnées
        * @param {any} cmd Commande en cours
        * @returns {any} true or false
        */
        function getIfIsAllCommandeLigneAreSelected(cmd) {
            for (var i = 0, len = cmd.Lignes.length; i < len; i++) {
                var ligne = cmd.Lignes[i];
                if (ligne.IsVisible && !ligne.isSelected) {
                    return false;
                }
            }
            return true;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////// SELECTION DESELECTION COMMANDE LIGNES/ ///////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        this.canSelectCommandeLigne = function (commandeLigne) {

            if (!CommandeLigneLockService.getIfUserIsInSocieteWithThisFonctionnalite()) {
                return true;
            }
            if (commandeLigne.IsVerrou) {
                return false;
            }
            return true;
        };


        this.handleSelectCommandeLigne = function (cmd, cmdLigne) {
            selectUnSelectCommandeLigne(cmdLigne);
            cmd.isSelected = getIfIsAllCommandeLigneAreSelected(cmd);
            actionOrderSelectedCommandeLignes();
        };


        function selectUnSelectCommandeLigne(cmdLigne) {
            if (!cmdLigne.IsVisible) {
                return;
            }
            if (cmdLigne.isSelected) {
                selectCommandeLigne(cmdLigne);
            }
            else {
                unSelectCommandeLigne(cmdLigne);
            }
        }

        function selectCommandeLigne(cmdLigne) {
            var selectedCmdLigne = $filter('filter')(selectedCommandeLignes, { CommandeLigneId: cmdLigne.CommandeLigneId }, true)[0];
            if (!selectedCmdLigne) {
                selectedCommandeLignes.push(cmdLigne);
            }
        }

        function unSelectCommandeLigne(cmdLigne) {
            var selectedCmdLigne = $filter('filter')(selectedCommandeLignes, { CommandeLigneId: cmdLigne.CommandeLigneId }, true)[0];
            if (selectedCmdLigne) {
                var index = selectedCommandeLignes.indexOf(selectedCmdLigne);
                if (index > -1) {
                    selectedCommandeLignes.splice(index, 1);
                }
            }
        }

        /**
         * Tri des réceptions (comme affichée à l'écran)     
         */
        function actionOrderSelectedCommandeLignes() {
            selectedCommandeLignes.sort(function actionSort(a, b) { return a.CommandeLigneId - b.CommandeLigneId; });
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////// SELECTION DESELECTION COMMANDE LIGNES/ ///////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /**
        * Désélection des lignes de commande
         * @param {data} data data les commandes
        */
        this.actionDeselectAll = function (data) {
            angular.forEach(data, function (cmd) {
                cmd.isSelected = false;
                angular.forEach(cmd.Lignes, function (cmdLigne) {
                    cmdLigne.isSelected = false;
                });
            });
            that.clearSelectedLignes();
        };

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////// SELECTION DESELECTION COMMANDE LIGNES/ ///////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        this.clearSelectedLignes = function () {
            selectedCommandeLignes = [];
        };
    }
})();
