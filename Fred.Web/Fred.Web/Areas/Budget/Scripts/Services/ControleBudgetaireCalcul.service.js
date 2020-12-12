(function () {
    'use strict';

    angular.module('Fred').service('ControleBudgetaireCalculService', ControleBudgetaireCalculService);

    function ControleBudgetaireCalculService() {
        var service = this;

        service.calculMontantAjustementEnfant = calculMontantAjustementEnfant;
        service.calculPfa = calculPfa;
        service.calculPourcentageDepense = calculPourcentageDepense;
        service.calculPuDepense = calculPuDepense;
        service.calculPourcentageDad = calculPourcentageDad;
        service.calculMontantRadTheorique = calculMontantRadTheorique;
        service.calculQuantiteEcart = calculQuantiteEcart;
        service.calculMontantEcart = calculMontantEcart;
        service.calculPuEcart = calculPuEcart;
        service.calculQuantiteRad = calculQuantiteRad;
        service.calculTotalMontantBudget = calculTotalMontantBudget;
        service.calculTotalDaD = calculTotalDaD;
        service.calculTotalDepense = calculTotalDepense;
        service.calculTotalEcart = calculTotalEcart;
        service.calculTotalRad = calculTotalRad;
        service.calculTotalAjustement = calculTotalAjustement;
        service.calculTotalPfa = calculTotalPfa;
        service.calculTotalPfaMoisPrecedent = calculTotalPfaMoisPrecedent;
        service.calculTotalProjectionLineaireDepenses = calculTotalProjectionLineaireDepenses;
        service.calculQuantiteDad = calculQuantiteDad;
        service.calculPuDad = calculPuDad;
    }

    function calculTotalMontantBudget(tree) {
        if (!tree) {
            return 0;
        }

        let total = 0;
        //Les montants du budgets des niveaux les plus hauts sont calculés comme étant la somme des montants descendants.
        //Donc pour calculer le total, il suffit de faire la somme des montants du niveau le plus haut
        tree.forEach((axePrincipal) => {
            total += axePrincipal.Valeurs.MontantBudget;
        });

        return total;
    }

    function calculTotalDaD(tree) {
        if (!tree) {
            return 0;
        }

        let total = 0;
        tree.forEach((axePrincipal) => {
            total += axePrincipal.Valeurs.MontantDad;
        });

        return total;
    }

    function calculTotalDepense(tree) {
        if (!tree) {
            return 0;
        }

        let total = 0;
        tree.forEach((axePrincipal) => {
            total += axePrincipal.Valeurs.MontantDepense;
        });

        return total;
    }

    function calculTotalEcart(tree) {
        if (!tree) {
            return 0;
        }

        //Rappel :  l'écart est égale au Dad - Depense, donc le total de l'ecart c'est le total du dad - total depense
        let total = 0;
        tree.forEach((axePrincipal) => {
            total += calculMontantEcart(axePrincipal);
        });
        return total;
    }

    function calculTotalRad(tree) {
        if (!tree) {
            return 0;
        }

        let total = 0;
        tree.forEach((axePrincipal) => {
            total += calculMontantRadTheorique(axePrincipal);
        });

        return total;
    }

    function calculTotalAjustement(tree) {
        if (!tree) {
            return 0;
        }

        let total = 0;
        tree.forEach((axePrincipal) => {
            total += axePrincipal.Valeurs.MontantAjustement;
        });

        return total;
    }

    function calculTotalPfa(tree) {
        if (!tree) {
            return 0;
        }

        let total = 0;
        tree.forEach((axePrincipal) => {
            total += calculPfa(axePrincipal);
        });

        return total;
    }

    function calculTotalPfaMoisPrecedent(tree) {
        if (!tree) {
            return 0;
        }

        let total = 0;
        tree.forEach((axePrincipal) => {
            total += axePrincipal.Valeurs.PfaMoisPrecedent;
        });

        return total;
    }

    function calculTotalProjectionLineaireDepenses(tree) {
        if (!tree) {
            return 0;
        }

        let total = 0;
        tree.forEach((axePrincipal) => {
            total += axePrincipal.Valeurs.ProjectionLineaireDepenses;
        });

        return total;
    }

    function calculMontantAjustementEnfant(axe) {
        var total = null;

        axe.SousAxe.forEach(function (sousAxe) {
            //Chaque branche est identique, donc il est impossible que sousAxe n'ait pas de SousAxe
            //et que le prochain (puisqu'on est dans un foreach) en ait.
            //Si on est sur une feuille, il ne faut pas toucher la valeur du MontantAjustement
            //puisqu'elle est déjà spécifiée (par l'utilisateur). Modifier cette valeur déclencherait un évenement
            //Dans le Fred-input-number qui rajouterait automatiquement les décimales et placerait le curseur à la fin du texte,
            //gênant la saisie (suffisemment pour qu'un bug soit ouvert)
            if (sousAxe.SousAxe) {
                sousAxe.Valeurs.MontantAjustement = calculMontantAjustementEnfant(sousAxe);
            }

            total += parseFloat(sousAxe.Valeurs.MontantAjustement);
        });

        return total;
    }

    function calculPfa(axe) {
        var pfa = parseFloat(axe.Valeurs.MontantAjustement) + calculMontantRadTheorique(axe) + axe.Valeurs.MontantDepense;
        return pfa;
    }

    function calculPourcentageDepense(axe) {
        var pfa = calculPfa(axe);
        var pourcentage = axe.Valeurs.MontantDepense / pfa * 100;
        return pourcentage;
    }

    function calculPuDepense(axe) {
        if (axe.Valeurs.QuantiteDepense !== null) {
            var quantiteDepense = parseFloat(axe.Valeurs.QuantiteDepense.Quantite);
            if (quantiteDepense !== 0) {
                var puDepense = axe.Valeurs.MontantDepense / quantiteDepense;
                return puDepense;
            }
        }
        return null;
    }

    function calculPourcentageDad(axe) {
        if (axe.Valeurs.MontantBudget === 0) {
            return 0;
        }

        var pourcentageDad = axe.Valeurs.MontantDad / axe.Valeurs.MontantBudget * 100;
        return pourcentageDad;
    }

    function calculQuantiteDad(axe) {
        if (axe.SousAxe == null) {
            return calculQuantiteDadLeaf(axe);
        }

        return sumSousAxes(axe.SousAxe);
    }

    function calculQuantiteDadLeaf(axe) {
        if (!axe.Valeurs.QuantiteBudget) {
            return null;
        }

        var pourcentageDad = calculPourcentageDad(axe);
        if (pourcentageDad === 0) {
            return 0;
        }

        return axe.Valeurs.QuantiteBudget.Quantite * pourcentageDad / 100;
    }

    function sumSousAxes(axes) {
        var quantiteDad = 0;

        axes.forEach(function (axe) {
            if (axe.SousAxe != null)
                quantiteDad += sumSousAxes(axe.SousAxe);
            else 
                quantiteDad += axe.Valeurs.QuantiteDad ? axe.Valeurs.QuantiteDad : calculQuantiteDadLeaf(axe);
        });

        return quantiteDad;
    }

    function calculPuDad(axe) {
        if (axe.SousAxe == null || !axe.Valeurs.MontantDad)
            return axe.Valeurs.PuBudget;

        return axe.Valeurs.MontantDad / axe.Valeurs.QuantiteDad;
    }

    function calculMontantRadTheorique(axe) {
        return axe.Valeurs.MontantBudget - axe.Valeurs.MontantDad;
    }

    function calculQuantiteEcart(axe) {
        let quantiteDad = 0;

        //La fonction calculQuantiteDad ne renvoi rien si la quantité budget n'est pas renseigné
        //Sauf que dans notre cas, il nous faut 0 et pas undefined
        if (axe.Valeurs.QuantiteBudget) {
            quantiteDad = calculQuantiteDad(axe);
        }

        if (axe.Valeurs.QuantiteDepense) {
            return quantiteDad - axe.Valeurs.QuantiteDepense.Quantite;
        } else if (axe.Valeurs.QuantiteBudget) {
            //On vérifie qu'on a une quantité budgété avant, car l'écart ne peut s'afficher que sur un axe ayant une quantité budgétée
            return quantiteDad;
        }
    }

    function calculMontantEcart(axe) {
        return axe.Valeurs.MontantDad - axe.Valeurs.MontantDepense;
    }

    function calculPuEcart(axe) {
        let montantEcart = calculMontantEcart(axe);
        let quantiteEcart = calculQuantiteEcart(axe);
        if (quantiteEcart === undefined || montantEcart === undefined) {
            return null;
        }
        return quantiteEcart === 0 ? 0 : montantEcart / quantiteEcart;
    }

    function calculQuantiteRad(axe) {
        if (axe.Valeurs.QuantiteBudget) {
            let quantiteDad = calculQuantiteDad(axe);
            let quantiteRad = axe.Valeurs.QuantiteBudget.Quantite - quantiteDad;
            return quantiteRad;
        }
        return null;
    }
})();