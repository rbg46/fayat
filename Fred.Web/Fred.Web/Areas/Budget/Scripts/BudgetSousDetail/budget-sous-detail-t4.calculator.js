(function () {
    'use strict';

    angular.module('Fred').service('BudgetSousDetailT4Calculator', BudgetSousDetailT4Calculator);
    BudgetSousDetailT4Calculator.$inject = ['BudgetCalculator', 'BudgetSousDetailCalculator'];

    function BudgetSousDetailT4Calculator(BudgetCalculator, BudgetSousDetailCalculator) {

        return {
            UniteChanged: UniteChanged,
            ItemChanged: ItemChanged,
            ItemAdded: ItemAdded,
            ItemDeleted: ItemDeleted,
            QuantiteDeBaseChanged: QuantiteDeBaseChanged,
            QuantiteARealiserChanged: QuantiteARealiserChanged,

            Calculate: Calculate
        };

        // A appeler lorsque l'unité d'un élément du sous détail change.
        // - item : élement du sous-détail concerné.
        function UniteChanged(item) {
            CalculateTotalHeuresMoT4(item.SousDetail);
        }

        // A appeler lorsqu'un élément du sous détail change.
        // - item : élement du sous-détail concerné.
        function ItemChanged(item) {
            CalculateItemT4(item);
            CalculateItemSD(item);
            CalculatePrixTotalEtPrixUnitaireT4(item.SousDetail);
            CalculateTotalHeuresMoT4(item.SousDetail);
            BudgetSousDetailCalculator.CalculateBudgetMontant(item.SousDetail);
        }

        // A appeler lorsqu'un élément du sous détail est ajouté.
        // - item : élement du sous-détail concerné.
        // - sousDetail : le sous-détail correspondant.
        // - montant : le montant à utiliser.
        function ItemAdded(item, sousDetail, montant) {
            montant = BudgetCalculator.GetValue(montant);

            item.View.PrixUnitaire = montant;
            item.Current.PrixUnitaire = montant;
            item.View.Quantite = 1;
            item.Current.Quantite = 1;

            //Etant donné qu'on vient d'ajouter l'élément, les valeurs actuelles sont celles de la bibliotheques des prix
            //Puisqu'elles ne peuvent pas être modifiées dans l'écran de sélection des ressources
            item.PuBibliothequePrix = item.Current.PrixUnitaire;
            if (item.Unite !== null) {
                item.UniteIdBibliothequePrix = item.Unite.UniteId;
            }
            else {
                item.UniteIdBibliothequePrix = null;
            }

            ItemChanged(item);
        }

        // A appeler lorsqu'un élément du sous détail est supprimé.
        // - item : élement du sous-détail concerné.
        function ItemDeleted(item) {
            CalculatePrixTotalEtPrixUnitaireT4(item.SousDetail);
            CalculateTotalHeuresMoT4(item.SousDetail);
            BudgetSousDetailCalculator.CalculateBudgetMontant(item.SousDetail);
        }

        // A appeler lorsque la quantité de base change.
        // - sousDetail : le sous-détail concerné.
        function QuantiteDeBaseChanged(sousDetail) {
            QuantiteDeBaseOuARealiserChanged(sousDetail);
        }

        // A appeler lorsque la quantité à réaliser change.
        // - sousDetail : le sous-détail concerné.
        function QuantiteARealiserChanged(sousDetail) {
            QuantiteDeBaseOuARealiserChanged(sousDetail);
        }

        // A appeler lorsque la quantité de base ou à réaliser change.
        // - sousDetail : le sous-détail concerné.
        function QuantiteDeBaseOuARealiserChanged(sousDetail) {
            for (let item of sousDetail.Items) {
                CalculateItemSD(item);
            }
            CalculatePrixTotalEtPrixUnitaireT4(sousDetail);
            CalculateTotalHeuresMoT4(sousDetail);
            BudgetSousDetailCalculator.CalculateBudgetMontant(sousDetail);
        }

        // Calcule l'ensemble d'un sous-détail.
        // - sousDetail : le sous-détail concerné.
        // - updateQuantite : indique si les quantités doivent être swapées d'une vue à l'autre au besoin.
        function Calculate(sousDetail, updateQuantite) {
            if (updateQuantite) {
                UpdateQuantite(sousDetail);
            }

            sousDetail.BudgetT4.View.MontantSD = null;
            sousDetail.PrixUnitaireQb = null;
            sousDetail.TotalHeuresMo = null;
            sousDetail.HeuresMoParUnite = null;
            sousDetail.BudgetT4.View.MontantT4 = null;
            sousDetail.BudgetT4.View.PU = null;
            sousDetail.TotalHeuresMoT4 = null;

            for (let item of sousDetail.Items) {
                CalculateItemT4(item);
                CalculateItemSD(item);
                item.MontantSD = null;
            }

            CalculatePrixTotalEtPrixUnitaireT4(sousDetail);
            CalculateTotalHeuresMoT4(sousDetail);
            BudgetSousDetailCalculator.CalculateBudgetMontant(sousDetail);
        }

        // Met à jour les quantités avant les calculs.
        // Si la quantité de base ou à réaliser est null, il n'est pas possible de calculer les quantités SD via les quantités T4 et inversement.
        // De ce fait, en vue T4, si les quantités sont renseignées mais pas la quantité à réaliser, il n'est pas possible de calculer les
        //  quantités SD, qui seront donc à null après les calculs. Lorsque l'utilisateur passe en vue SD, comme les quantités SD et la quantité
        //  à réaliser sont null, les quantité T4 vont donc après calcul repasser à null. Les quantités SD et T4 seront donc perdues.
        // Le problème se produit également lorsque l'utilisateur est en vue SD et passe en vue T4, ou s'il recherge un sous-détail, qui par
        //  défaut sera en vue SD alors qu'il l'a enregistré en vue T4.
        // Pour corriger ce problème, les quantités sont swapées d'une vue à l'autre si les quantités cible sont null.
        // - sousDetail : le sous-détail concerné.
        function UpdateQuantite(sousDetail) {
            if (sousDetail.Items.length > 0) {
                var budgetT4 = sousDetail.BudgetT4;
                var quantiteDeBase = budgetT4.Current.QuantiteDeBase;
                var quantiteARealiser = budgetT4.Current.QuantiteARealiser;
                if (quantiteDeBase === null || quantiteDeBase === 0 || quantiteARealiser === null || quantiteARealiser === 0) {
                    var update = true;
                    for (let item of sousDetail.Items) {
                        if (item.View.Quantite !== null) {
                            update = false;
                            break;
                        }
                    }
                    if (update) {
                        for (let item of sousDetail.Items) {
                            item.View.Quantite = item.Current.QuantiteSD;
                            item.View.QuantiteFormule = null;
                            item.Current.Quantite = item.Current.QuantiteSD;
                            item.Current.QuantiteFormule = null;
                        }
                    }
                }
            }
        }

        // Calcule la partie SD d'un élément de sous-détail.
        // - item : élément concerné.
        function CalculateItemSD(item) {
            // La quantité SD est calculé via le T4
            var quantiteSDCalculee = null;
            if (item.Current.Quantite !== null) {
                var budgetT4 = item.SousDetail.BudgetT4;
                var quantiteDeBase = budgetT4.Current.QuantiteDeBase;
                var quantiteARealiser = budgetT4.Current.QuantiteARealiser;
                if (quantiteDeBase !== null && quantiteDeBase !== 0 && quantiteARealiser !== null && quantiteARealiser !== 0) {
                    quantiteSDCalculee = BudgetCalculator.Calculate(item.Current.Quantite * quantiteDeBase / quantiteARealiser).toFixed(3);
                }
            }
            if (!BudgetCalculator.EqualsRounded(item.View.QuantiteSD, quantiteSDCalculee, 3)) {
                item.View.QuantiteSD = quantiteSDCalculee;
                item.View.QuantiteSDFormule = null;
            }

            if (!BudgetCalculator.EqualsRounded(item.Current.QuantiteSD, quantiteSDCalculee, 3)) {
                item.Current.QuantiteSD = quantiteSDCalculee;
                item.Current.QuantiteSDFormule = null;
            }

        }

        // Calcule la partie T4 d'un élément de sous-détail.
        // - item : élément concerné.
        function CalculateItemT4(item) {
            // Le montant T4 est calculé via le prix unitaire et la quantité T4
            item.View.Montant = null;
            if (item.Current.PrixUnitaire === null || item.Current.Quantite === null) {
                item.View.Montant = null;
            }
            else {
                item.View.Montant = BudgetCalculator.Calculate(item.Current.PrixUnitaire * item.Current.Quantite);
            }
        }

        // Calcule le prix total et le prix unitaire T4 d'un sous-détail
        // - sousDetail : le sous-détail concerné.
        function CalculatePrixTotalEtPrixUnitaireT4(sousDetail) {
            // Les éléments sont calculé relativement au T4
            sousDetail.BudgetT4.View.MontantT4 = null;
            sousDetail.BudgetT4.View.PU = null;
            var montantT4 = null;
            for (let item of sousDetail.Items) {
                if (!item.Deleted && item.View.Montant !== null) {
                    montantT4 = montantT4 === null ? item.View.Montant : montantT4 + item.View.Montant;
                }
            }

            sousDetail.BudgetT4.View.MontantT4 = BudgetCalculator.Calculate(montantT4);

            var quantiteARealiser = sousDetail.BudgetT4.Current.QuantiteARealiser;
            if (montantT4 !== null && quantiteARealiser !== null && quantiteARealiser !== 0) {
                sousDetail.BudgetT4.View.PU = BudgetCalculator.Calculate(montantT4 / quantiteARealiser);
            }
        }

        // Calcule le total des heures MO T4 d'un sous-détail
        // - sousDetail : le sous-détail concerné.
        function CalculateTotalHeuresMoT4(sousDetail) {
            // Les éléments sont calculé relativement au T4
            sousDetail.TotalHeuresMoT4 = null;
            var totalHeuresMoT4 = null;
            for (let item of sousDetail.Items) {
                if (!item.Deleted && item.Current.Quantite !== null && item.Current.Unite !== null && item.Ressource.TypeRessourceId === 2) {
                    if (item.Current.Unite.UniteId === 4) {                   // 4 -> Heure
                        totalHeuresMoT4 = totalHeuresMoT4 === null ? item.Current.Quantite : totalHeuresMoT4 + item.Current.Quantite;
                    }
                    else if (item.Current.Unite.UniteId === 5) {              // 5 -> Jour
                        var quantite = 8 * item.Current.Quantite;
                        totalHeuresMoT4 = totalHeuresMoT4 === null ? quantite : totalHeuresMoT4 + quantite;
                    }
                }
            }

            sousDetail.TotalHeuresMoT4 = BudgetCalculator.Calculate(totalHeuresMoT4);
        }
    }
})();
