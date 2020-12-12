/*
 * Ce service helper pour commande énergie
 */
(function () {
    'use strict';

    angular.module('Fred').service('CommandeEnergieHelperService', CommandeEnergieHelperService);

    CommandeEnergieHelperService.$inject = ['$filter'];

    function CommandeEnergieHelperService($filter) {

        var service = {
            toLocaleDate: toLocaleDate,
            commandeCalculation: commandeCalculation,
            getErrors: getErrors,
            onCommandeLigneLookupSelection: onCommandeLigneLookupSelection,
            onCommandeLigneLookupDeletion: onCommandeLigneLookupDeletion
        };

        return service;

        /* -------------------------------------------------------------------------------------------------------------
         *                                            PUBLIC
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
          * @function actionToLocaleDate(data)
          * @description Transforme les dates UTC issues du serveur en dates locales
          * @param {Commande} data Commande 
          */
        function toLocaleDate(data) {
            if (data) {
                data.Date = $filter('toLocaleDate')(data.Date);
                data.DateCreation = $filter('toLocaleDate')(data.DateCreation);
                data.DateModification = $filter('toLocaleDate')(data.DateModification);
                data.DateValidation = $filter('toLocaleDate')(data.DateValidation);
                data.DateSuppression = $filter('toLocaleDate')(data.DateSuppression);
                data.DateCloture = $filter('toLocaleDate')(data.DateCloture);
                angular.forEach(data.Lignes, function (val) {
                    val.DateModification = $filter('toLocaleDate')(val.DateModification);
                    val.DateCreation = $filter('toLocaleDate')(val.DateCreation);
                    val.DateSuppression = $filter('toLocaleDate')(val.DateSuppression);
                });
            }
        }

        /*
        * @description Fonction de mise à jour du total de la commande
        */
        function commandeCalculation(commande) {
            if (commande) {
                commande.MontantHT = totalCommandeCalculation(commande);
            }
        }

        function getErrors(modelState) {
            if (modelState) {
                var errors = [];

                for (var key in modelState) {
                    var cle = key.substring(0, 6);

                    if (cle !== "Lignes") {
                        var value = modelState[key][0];
                        errors.push(value);
                    }
                    else {
                        for (var numError in modelState[key]) {
                            if (!errors.includes(modelState[key][numError]))
                                errors.push(modelState[key][numError]);
                        }
                    }
                }

                return errors;
            }

            return [];
        }

        function onCommandeLigneLookupSelection(type, item, ligne) {
            ligne.IsUpdated = true;
            switch (type) {
                case 'Ressource':
                    ligne.RessourceId = item.IdRef;
                    break;
                case 'Tache':
                    ligne.TacheId = item.IdRef;
                    break;
                case 'Unite':
                    ligne.UniteId = item.IdRef;
                    break;
                default: break;
            }
        }

        function onCommandeLigneLookupDeletion(type, ligne) {
            ligne.IsUpdated = true;
            switch (type) {
                case 'Ressource':
                    ligne.RessourceId = null;
                    ligne.Ressource = null;
                    break;
                case 'Tache':
                    ligne.TacheId = null;
                    ligne.Tache = null;
                    break;
                case 'Unite':
                    ligne.UniteId = null;
                    ligne.Unite = null;
                    break;
                default: break;
            }
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            PRIVATE
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
        * @description Calcule le total de la commande
        */
        function totalCommandeCalculation(commande) {
            var total = 0;
            var lignes = commande.Lignes;

            if (lignes && lignes.length > 0) {
                for (var i = 0; i < lignes.length; i++) {
                    var ligne = lignes[i];

                    commandeEnergieLigneCalculation(ligne);
                    allCommandeEnergieLigneCalculation(ligne);

                    if (!ligne.IsDeleted) {
                        total += ligne.MontantHT;
                    }
                }
            }

            return total;
        }

        /**
         * Calcul du montant HT de ligne de commande
         * @param {any} ligne ligne de commande
         */
        function allCommandeEnergieLigneCalculation(ligne) {
            if (ligne.PUHT < 0) { ligne.PUHT = 0; }
            if (ligne.Quantite < 0) { ligne.Quantite = 0; }

            ligne.MontantHT = ligne.PUHT * ligne.Quantite;
        }

        function commandeEnergieLigneCalculation(ligne) {
            commandeEnergieLigneEcartQuantite(ligne);
            commandeEnergieLigneEcartPu(ligne);
            commandeEnergieLigneEcartMontant(ligne);
        }

        function commandeEnergieLigneEcartQuantite(ligne) {
            ligne.EcartQuantite = ligne.Quantite - ligne.QuantiteConvertie;
        }

        function commandeEnergieLigneEcartPu(ligne) {
            ligne.EcartPu = ligne.PUHT - ligne.Bareme;
        }

        function commandeEnergieLigneEcartMontant(ligne) {
            ligne.EcartMontant = (ligne.Quantite * ligne.PUHT) - ligne.MontantValorise;
        }
    }
})();

