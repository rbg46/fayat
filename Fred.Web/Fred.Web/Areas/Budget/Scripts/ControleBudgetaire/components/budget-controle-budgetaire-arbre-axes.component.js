(function () {
    'use strict';

    var budgetControleBudgetaireArbreAxesComponent = {
        templateUrl: '/Areas/Budget/Scripts/ControleBudgetaire/components/budget-controle-budgetaire-arbre-axes.component.html',
        controller: budgetControleBudgetaireArbreAxesController,
        bindings: {
            tree: '<',
            axePrincipal: '<',
            locked: '<',
            readonly: '<',
            colonnesAffichees: '<',
            symboleDevise: '<',
            cumul: '<',
            axeAffichees: '<'
        }
    };
    angular.module('Fred').component('budgetControleBudgetaireArbreAxesComponent', budgetControleBudgetaireArbreAxesComponent);

    budgetControleBudgetaireArbreAxesController.$inject = ['$scope', 'ControleBudgetaireCalculService'];

    function budgetControleBudgetaireArbreAxesController($scope, ControleBudgetaireCalculService) {
        var $ctrl = this;

        $ctrl.resources = resources;
        $ctrl.getAxeLevel = getAxeLevel;
        $ctrl.getLevelIcon = getLevelIcon;
        $ctrl.montantAjustementChanged = montantAjustementChanged;
        $ctrl.isAjustementEditable = isAjustementEditable;
        $ctrl.ouvreOngletDepense = ouvreOngletDepense;
        $ctrl.setHiddenChildren = setHiddenChildren;

        $ctrl.getUniteQuantiteEcart = getUniteQuantiteEcart;
        $ctrl.getUniteQuantiteRad = getUniteQuantiteRad;
        $ctrl.getPfa = ControleBudgetaireCalculService.calculPfa;

        $ctrl.AxePrincipalTaches = "TacheRessource";
        $ctrl.AxePrincipalRessource = "RessourceTache";

        return $ctrl;

        function isAjustementEditable(axe) {

            if ($ctrl.locked || $ctrl.readonly) {
                return false;
            }

            if ($ctrl.axeAffichees.includes("T3") || $ctrl.axeAffichees.includes("Ressource")) {
                return axe.AxeType === 'Ressource';
            }
        }

        function getUniteQuantiteEcart(axe) {

            if (axe.Valeurs.QuantiteDepense &&
                axe.Valeurs.QuantiteBudget) {

                if (axe.Valeurs.QuantiteDepense.Unite && axe.Valeurs.QuantiteBudget.Unite) {

                    if (axe.Valeurs.QuantiteDepense.Unite === axe.Valeurs.QuantiteBudget.Unite) {
                        return axe.Valeurs.QuantiteDepense.Unite;
                    }
                    return '#';
                }
            }

            if (!axe.Valeurs.QuantiteDepense &&
                !axe.Valeurs.QuantiteBudget) {
                return null;
            }

            if (axe.Valeurs.QuantiteDepense && axe.Valeurs.QuantiteDepense.Unite) {
                return axe.Valeurs.QuantiteDepense.Unite;
            }

            if (axe.Valeurs.QuantiteBudget && axe.Valeurs.QuantiteBudget.Unite) {
                return axe.Valeurs.QuantiteBudget.Unite;
            }



            return '#';
        }

        function getUniteQuantiteRad(axe) {

            if (axe.Valeurs.QuantiteBudget) {
                if (axe.Valeurs.QuantiteBudget.Unite) {
                    return axe.Valeurs.QuantiteBudget.Unite;
                } else if (axe.Valeurs.QuantiteDepense.Unite) {
                    return axe.Valeurs.QuantiteDepense.Unite;
                } else {
                    return '#';
                }
            }
        }

        function montantAjustementChanged(axe) {

            if (axe.SousAxe !== null) {
                resetMontantAjustement(axe.SousAxe);
            }

            $scope.$emit('MontantAjustementChanged');            
        }

        function ouvreOngletDepense(axe) {

            $scope.$emit('ShowOngletDepenses', axe);
        }

        function setHiddenChildren(axe) {
            axe.hiddenChildren = !axe.hiddenChildren;
        }

        function resetMontantAjustement(axes) {
            axes.forEach((axe) => {
                axe.Valeurs.MontantAjustement = null;
                axe.Valeurs.CommentaireAjustement = null;
                axe.Valeurs.PourcentageDepense = null;
                if (axe.SousAxe !== null) {
                    resetMontantAjustement(axe.SousAxe);
                }
            });

        }

        function getAxeLevel(axe) {
            if ($ctrl.axePrincipal === $ctrl.AxePrincipalTaches) {
                return getAxeLevelAxePrincipalTache(axe);
            } else {
                return getAxeLevelAxePrincipalRessource(axe);
            }

        }

        function getAxeLevelAxePrincipalTache(axe) {

            switch (axe.AxeType) {
                case "T1":
                    return "level-1 tache-1";
                case "T2":
                    return "level-2 tache-2";
                case "T3":
                    return "level-3 tache-3";
                case "Chapitre":
                    return "level-4 chapitre";
                case "SousChapitre":
                    return "level-5 sous-chapitre";
                case "Ressource":
                    return "level-6 ressource";
            }
        }

        function getAxeLevelAxePrincipalRessource(axe) {

            switch (axe.AxeType) {
                case "T1":
                    return "level-4 tache-1";
                case "T2":
                    return "level-5 tache-2";
                case "T3":
                    return "level-6 tache-3";
                case "Chapitre":
                    return "level-1 chapitre";
                case "SousChapitre":
                    return "level-2 sous-chapitre";
                case "Ressource":
                    return "level-3 ressource";
            }
        }

        function getLevelIcon(axe) {
            switch (axe.AxeType) {
                case "T1":
                    return "icon-T1";
                case "T2":
                    return "icon-T2";
                case "T3":
                    return "icon-T3";
                case "Chapitre":
                    return "icon-chapitre";
                case "SousChapitre":
                    return "icon-sous-chapitre";
                case "Ressource":
                    return "icon-ressource-mo";
            }
        }
    }
})();
