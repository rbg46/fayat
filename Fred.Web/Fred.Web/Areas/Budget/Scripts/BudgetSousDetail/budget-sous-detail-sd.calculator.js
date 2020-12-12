(function () {
    'use strict';

    angular.module('Fred').service('BudgetSousDetailSdCalculator', BudgetSousDetailSdCalculator);
    BudgetSousDetailSdCalculator.$inject = ['BudgetCalculator', 'BudgetSousDetailCalculator'];

    function BudgetSousDetailSdCalculator(BudgetCalculator, BudgetSousDetailCalculator) {

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
            CalculateTotalHeuresMo(item.SousDetail);
            CalculateHeuresMoParUnite(item.SousDetail);
            CalculateTotalHeureMoT4(item.SousDetail);
        }

        // A appeler lorsqu'un élément du sous détail change.
        // - item : élement du sous-détail concerné.
        function ItemChanged(item) {
            CalculateItemSD(item);
            CalculateItemT4(item);
            CalculatePrixTotalQb(item.SousDetail);
            CalculateTotalHeuresMo(item.SousDetail);
            CalculatePrixUnitaireQb(item.SousDetail);
            CalculateHeuresMoParUnite(item.SousDetail);
            CalculatePrixTotalEtPrixUnitaireT4(item.SousDetail);
            CalculateTotalHeureMoT4(item.SousDetail);
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
            item.View.QuantiteSD = 1;
            item.Current.QuantiteSD = 1;

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
            CalculatePrixTotalQb(item.SousDetail);
            CalculateTotalHeuresMo(item.SousDetail);
            CalculatePrixUnitaireQb(item.SousDetail);
            CalculateHeuresMoParUnite(item.SousDetail);
            CalculatePrixTotalEtPrixUnitaireT4(item.SousDetail);
            CalculateTotalHeureMoT4(item.SousDetail);
            BudgetSousDetailCalculator.CalculateBudgetMontant(item.SousDetail);
        }

        // A appeler lorsque la quantité de base change.
        // - sousDetail : le sous-détail concerné.
        function QuantiteDeBaseChanged(sousDetail) {
            for (let item of sousDetail.Items) {
                CalculateItemT4(item);
            }
            CalculatePrixUnitaireQb(sousDetail);
            CalculateHeuresMoParUnite(sousDetail);
            CalculatePrixTotalEtPrixUnitaireT4(sousDetail);
            CalculateTotalHeureMoT4(sousDetail);
            BudgetSousDetailCalculator.CalculateBudgetMontant(sousDetail);
        }

        // A appeler lorsque la quantité à réaliser change.
        // - sousDetail : le sous-détail concerné.
        function QuantiteARealiserChanged(sousDetail) {
            for (let item of sousDetail.Items) {
                CalculateItemT4(item);
            }
            CalculatePrixTotalEtPrixUnitaireT4(sousDetail);
            CalculateTotalHeureMoT4(sousDetail);
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
                CalculateItemSD(item);
                CalculateItemT4(item);
            }

            CalculatePrixTotalQb(sousDetail);
            CalculateTotalHeuresMo(sousDetail);
            CalculatePrixUnitaireQb(sousDetail);
            CalculateHeuresMoParUnite(sousDetail);
            CalculatePrixTotalEtPrixUnitaireT4(sousDetail);
            CalculateTotalHeureMoT4(sousDetail);
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
                        if (item.View.QuantiteSD !== null) {
                            update = false;
                            break;
                        }
                    }
                    if (update) {
                        for (let item of sousDetail.Items) {
                            item.View.QuantiteSD = item.Current.Quantite;
                            item.View.QuantiteSDFormule = null;
                            item.Current.QuantiteSD = item.Current.Quantite;
                            item.Current.QuantiteSDFormule = null;
                        }
                    }
                }
            }
        }

        // Calcule la partie SD d'un élément de sous-détail.
        // - item : élément concerné.
        function CalculateItemSD(item) {
            // Le montant SD est calculé via le prix unitaire et la quantité SD
            if (item.Current.PrixUnitaire === null || item.Current.QuantiteSD === null) {
                item.View.MontantSD = null;
            }
            else {
                item.View.MontantSD = BudgetCalculator.Calculate(item.Current.PrixUnitaire * item.Current.QuantiteSD);
            }
        }

        // Calcule la partie T4 d'un élément de sous-détail.
        // - item : élément concerné.
        function CalculateItemT4(item) {
            // La quantité et le montant T4 sont calculés via le SD
            var quantite = null;
            var montant = null;

            if (item.Current.QuantiteSD !== null) {
                var budgetT4 = item.SousDetail.BudgetT4;
                var quantiteDeBase = budgetT4.Current.QuantiteDeBase;
                var quantiteARealiser = budgetT4.Current.QuantiteARealiser;
                if (quantiteDeBase !== null && quantiteDeBase !== 0 && quantiteARealiser !== null && quantiteARealiser !== 0) {
                    quantite = BudgetCalculator.Calculate(item.Current.QuantiteSD * quantiteARealiser / quantiteDeBase);
                    if (item.Current.PrixUnitaire !== null) {
                        montant = BudgetCalculator.Calculate(quantite * item.Current.PrixUnitaire);
                    }
                }
            }

            if (!BudgetCalculator.EqualsRounded(item.View.Quantite, quantite, 3)) {
                item.View.Quantite = quantite;
                item.View.QuantiteFormule = null;
            }
            if (!BudgetCalculator.EqualsRounded(item.Current.Quantite, quantite, 3)) {
                item.Current.Quantite = quantite;
                item.Current.QuantiteFormule = null;
            }
            item.View.Montant = montant;
        }

        // Calcule le prix total QB d'un sous-détail.
        // - sousDetail : le sous-détail concerné.
        function CalculatePrixTotalQb(sousDetail) {
            sousDetail.BudgetT4.View.MontantSD = null;
            if (sousDetail.Items.length > 0) {
                var prixTotalQb = null;
                for (let item of sousDetail.Items) {
                    if (!item.Deleted && item.View.MontantSD !== null) {
                        prixTotalQb = prixTotalQb === null ? item.View.MontantSD : prixTotalQb + item.View.MontantSD;
                    }
                }
                sousDetail.BudgetT4.View.MontantSD = BudgetCalculator.Calculate(prixTotalQb);
            }
        }

        // Calcule le total heures MO d'un sous-détail.
        // - sousDetail : le sous-détail concerné.
        function CalculateTotalHeuresMo(sousDetail) {
            sousDetail.TotalHeuresMo = null;
            if (sousDetail.Items.length > 0) {
                var totalHeuresMo = null;
                for (let item of sousDetail.Items) {
                    if (!item.Deleted && item.Current.QuantiteSD !== null && item.Current.Unite !== null && item.Ressource.TypeRessourceId === 2) {
                        if (item.Current.Unite.UniteId === 4) {                   // 4 -> Heure
                            totalHeuresMo = totalHeuresMo === null ? item.Current.QuantiteSD : totalHeuresMo + item.Current.QuantiteSD;
                        }
                        else if (item.Current.Unite.UniteId === 5) {              // 5 -> Jour
                            var quantiteSD = 8 * item.Current.QuantiteSD;
                            totalHeuresMo = totalHeuresMo === null ? quantiteSD : totalHeuresMo + quantiteSD;
                        }
                    }
                }
                sousDetail.TotalHeuresMo = BudgetCalculator.Calculate(totalHeuresMo);
            }
        }

        // Calcule le prix unitaire QB d'un sous-détail.
        // - sousDetail : le sous-détail concerné.
        function CalculatePrixUnitaireQb(sousDetail) {
            sousDetail.PrixUnitaireQb = null;
            var quantiteDeBase = sousDetail.BudgetT4.Current.QuantiteDeBase;
            if (quantiteDeBase !== null && quantiteDeBase !== 0 && sousDetail.BudgetT4.View.MontantSD !== null) {
                sousDetail.PrixUnitaireQb = BudgetCalculator.Calculate(sousDetail.BudgetT4.View.MontantSD / quantiteDeBase);
            }
        }

        // Calcule les heures MO par unité d'un sous-détail.
        // - sousDetail : le sous-détail concerné.
        function CalculateHeuresMoParUnite(sousDetail) {
            sousDetail.HeuresMoParUnite = null;
            var quantiteDeBase = sousDetail.BudgetT4.Current.QuantiteDeBase;
            if (quantiteDeBase !== null && quantiteDeBase !== 0 && sousDetail.TotalHeuresMo !== null) {
                sousDetail.HeuresMoParUnite = BudgetCalculator.Calculate(sousDetail.TotalHeuresMo / quantiteDeBase);
            }
        }

        // Calcule le prix total et le prix unitaire T4 d'un sous-détail
        // - sousDetail : le sous-détail concerné.
        function CalculatePrixTotalEtPrixUnitaireT4(sousDetail) {
            // Les éléments sont calculé relativement au SD
            sousDetail.BudgetT4.View.MontantT4 = null;
            sousDetail.BudgetT4.View.PU = null;
            if (sousDetail.BudgetT4.View.MontantSD !== null) {
                var quantiteDeBase = sousDetail.BudgetT4.Current.QuantiteDeBase;
                if (quantiteDeBase !== null && quantiteDeBase !== 0) {
                    sousDetail.BudgetT4.View.PU = BudgetCalculator.Calculate(sousDetail.BudgetT4.View.MontantSD / quantiteDeBase);
                    var quantiteARealiser = sousDetail.BudgetT4.Current.QuantiteARealiser;
                    if (quantiteARealiser !== null && quantiteARealiser !== 0) {
                        sousDetail.BudgetT4.View.MontantT4 = BudgetCalculator.Calculate(sousDetail.BudgetT4.View.PU * quantiteARealiser);
                    }
                }
            }
        }

        // Calcule le total des heures MO T4 d'un sous-détail
        // - sousDetail : le sous-détail concerné.
        function CalculateTotalHeureMoT4(sousDetail) {
            sousDetail.TotalHeuresMoT4 = null;
            var quantiteARealiser = sousDetail.BudgetT4.Current.QuantiteARealiser;
            if (sousDetail.HeuresMoParUnite !== null && quantiteARealiser !== null && quantiteARealiser !== 0) {
                sousDetail.TotalHeuresMoT4 = BudgetCalculator.Calculate(sousDetail.HeuresMoParUnite * quantiteARealiser);
            }
        }
    }
})();
