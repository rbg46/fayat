/*
 * Ce service helper pour commande
 */
(function () {
    'use strict';

    angular
        .module('Fred')
        .service('CommandeHelperService', CommandeHelperService);

    CommandeHelperService.$inject = ['$filter'];

    function CommandeHelperService($filter) {

        var service = {
            actionToLocaleDate: actionToLocaleDate,
            actionCalculTotalCommande: actionCalculTotalCommande,
            actionCalculTotalCommandeLignes: actionCalculTotalCommandeLignes,
            actionCalculTotalAvenantValideLignes: actionCalculTotalAvenantValideLignes,
            actionCalculTotalAvenantNonValideLignes: actionCalculTotalAvenantNonValideLignes,
            actionCalculLigneMontantHT: actionCalculLigneMontantHT,
            updateTotalCommande: updateTotalCommande
        };
        return service;

        /*
          * @function actionToLocaleDate(data)
          * @description Transforme les dates UTC issues du serveur en dates locales
          * @param {Commande} data Commande 
          */
        function actionToLocaleDate(data) {
            if (data) {
                data.Date = $filter('toLocaleDate')(data.Date);
                data.DateMiseADispo = $filter('toLocaleDate')(data.DateMiseADispo);
                data.DateCreation = $filter('toLocaleDate')(data.DateCreation);
                data.DateModification = $filter('toLocaleDate')(data.DateModification);
                data.DateValidation = $filter('toLocaleDate')(data.DateValidation);
                data.DateSuppression = $filter('toLocaleDate')(data.DateSuppression);
                data.DateCloture = $filter('toLocaleDate')(data.DateCloture);
                data.DateProchaineReception = $filter('toLocaleDate')(data.DateProchaineReception);
                data.DatePremiereReception = $filter('toLocaleDate')(data.DatePremiereReception);

                angular.forEach(data.Lignes, function (val) {
                    val.DateModification = $filter('toLocaleDate')(val.DateModification);
                    val.DateCreation = $filter('toLocaleDate')(val.DateCreation);
                });
            }
        }

        /*
        * @description Fonction de mise à jour du total de la commande
        */
        function actionCalculTotalCommande(commande) {
            if (commande !== null) {
                commande.TotalCommande = commande.TotalCommandeLignes + commande.TotalAvenantValideLignes + commande.TotalAvenantNonValideLignes;
                commande.MontantHT = commande.TotalCommande;
            }
        }

        /*
        * @description Calcule le montant total des lignes de commande en excluant les avenants
        */
        function actionCalculTotalCommandeLignes(commande) {
            commande.TotalCommandeLignes = actionCalculTotalLignes(commande, function (ligne) { return ligne.IsCommande; });
        }

        /*
        * @description Calcule le montant total des lignes d'avenant validées
        */
        function actionCalculTotalAvenantValideLignes(commande) {
            commande.TotalAvenantValideLignes = actionCalculTotalLignes(commande, function (ligne) { return ligne.IsAvenantValide; });
        }

        /*
        * @description Calcule le montant total des lignes d'avenant non validées
        */
        function actionCalculTotalAvenantNonValideLignes(commande) {
            commande.TotalAvenantNonValideLignes = actionCalculTotalLignes(commande, function (ligne) { return ligne.IsAvenantNonValide; });
        }

        /*
        * @description Calcule
        */
        function actionCalculTotalLignes(commande, predicate) {
            var total = 0;
            var lignes = commande.Lignes;
            if (lignes && lignes.length > 0) {
                for (var i = 0; i < lignes.length; i++) {
                    var ligne = lignes[i];
                    if (!ligne.IsDeleted && predicate(ligne)) {
                        total += ligne.MontantHT;
                    }
                }
            }
            return total;
        }

        /**
         * Calcul du montant HT
         * @param {any} ligne ligne de commande ou d'avenant
         */
        function actionCalculLigneMontantHT(ligne) {
            if (ligne.PUHT < 0) { ligne.PUHT = 0; }
            if (ligne.Quantite < 0) { ligne.Quantite = 0; }

            if (ligne.AvenantLigne !== null && ligne.AvenantLigne.IsDiminution) {
                ligne.MontantHT = -ligne.PUHT * ligne.Quantite;
            }
            else {
                ligne.MontantHT = ligne.PUHT * ligne.Quantite;
            }
        }

        /*
         * @description Mise à jour du montant total de la commande
        */
        function updateTotalCommande(commande) {
            actionCalculTotalCommandeLignes(commande);
            actionCalculTotalAvenantValideLignes(commande);
            actionCalculTotalAvenantNonValideLignes(commande);
            actionCalculTotalCommande(commande);
        }

    }
})();

