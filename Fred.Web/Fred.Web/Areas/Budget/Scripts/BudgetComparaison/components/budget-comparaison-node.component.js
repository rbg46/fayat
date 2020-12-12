(function () {
    'use strict';

    var budgetComparaisonNodeComponent = {
        templateUrl: '/Areas/Budget/Scripts/BudgetComparaison/components/budget-comparaison-node.component.html',
        bindings: {
            index: '<',
            node: '<'
        },
        controller: budgetComparaisonNodeController
    };
    angular.module('Fred').component('budgetComparaisonNodeComponent', budgetComparaisonNodeComponent);
    angular.module('Fred').controller('budgetComparaisonNodeController', budgetComparaisonNodeController);
    budgetComparaisonNodeController.$inject = ['BudgetComparaisonService'];
    function budgetComparaisonNodeController(BudgetComparaisonService) {

        //////////////////////////////////////////////////////////////////
        // Membres                                                      //
        //////////////////////////////////////////////////////////////////
        var $ctrl = this;
        $ctrl.resources = resources;
        $ctrl.Colonnes = BudgetComparaisonService.Filter.Colonnes;
        $ctrl.BudgetComparaisonService = BudgetComparaisonService;

        var currentNodeType = BudgetComparaisonService.Filter.AxeAnalytique.NodeTypes[$ctrl.index];
        $ctrl.HasChildren = $ctrl.index < BudgetComparaisonService.Filter.AxeAnalytique.NodeTypes.length - 1;
        $ctrl.NodeId = $ctrl.node.ViewId;

        $ctrl.GetAxeClass = GetAxeClass;
        $ctrl.GetAxePadding = GetAxePadding;
        $ctrl.GetAxeIconClass = GetAxeIconClass;
        $ctrl.GetAxeTooltipHeader = GetAxeTooltipHeader;
        return $ctrl;


        //////////////////////////////////////////////////////////////////
        // Vue                                                          //
        //////////////////////////////////////////////////////////////////

        // Retourne la classe css à utiliser pour l'axe.
        function GetAxeClass() {
            switch (currentNodeType) {
                case BudgetComparaisonService.AxeAnalytique.NodeType.Tache1:
                    return "axe-tache-1";
                case BudgetComparaisonService.AxeAnalytique.NodeType.Tache2:
                    return "axe-tache-2";
                case BudgetComparaisonService.AxeAnalytique.NodeType.Tache3:
                    return "axe-tache-3";
                case BudgetComparaisonService.AxeAnalytique.NodeType.Tache4:
                    return "axe-tache-4";
                case BudgetComparaisonService.AxeAnalytique.NodeType.Chapitre:
                    return "axe-chapitre";
                case BudgetComparaisonService.AxeAnalytique.NodeType.SousChapitre:
                    return "axe-sous-chapitre";
                case BudgetComparaisonService.AxeAnalytique.NodeType.Ressource:
                    return "axe-ressource";
            }
            return "";
        }

        // Retourne le padding à utiliser pour l'axe.
        function GetAxePadding() {
            return ($ctrl.index + 1) * 15;
        }

        // Retourne la classe css à utiliser pour l'icone de l'axe.
        function GetAxeIconClass() {
            switch (currentNodeType) {
                case BudgetComparaisonService.AxeAnalytique.NodeType.Tache1:
                    return "icon-tache-1";
                case BudgetComparaisonService.AxeAnalytique.NodeType.Tache2:
                    return "icon-tache-2";
                case BudgetComparaisonService.AxeAnalytique.NodeType.Tache3:
                    return "icon-tache-3";
                case BudgetComparaisonService.AxeAnalytique.NodeType.Tache4:
                    return "icon-tache-4";
                case BudgetComparaisonService.AxeAnalytique.NodeType.Chapitre:
                    return "icon-chapitre";
                case BudgetComparaisonService.AxeAnalytique.NodeType.SousChapitre:
                    return "icon-sous-chapitre";
                case BudgetComparaisonService.AxeAnalytique.NodeType.Ressource:
                    return "icon-ressource";
            }
            return "";
        }

        // Retourne le header du tooltip à utiliser pour l'axe.
        function GetAxeTooltipHeader() {
            switch (currentNodeType) {
                case BudgetComparaisonService.AxeAnalytique.NodeType.Tache1:
                    return $ctrl.resources.BudgetComparaison_AxeTooltipHeader_Tache1;
                case BudgetComparaisonService.AxeAnalytique.NodeType.Tache2:
                    return $ctrl.resources.BudgetComparaison_AxeTooltipHeader_Tache2;
                case BudgetComparaisonService.AxeAnalytique.NodeType.Tache3:
                    return $ctrl.resources.BudgetComparaison_AxeTooltipHeader_Tache3;
                case BudgetComparaisonService.AxeAnalytique.NodeType.Tache4:
                    return $ctrl.resources.BudgetComparaison_AxeTooltipHeader_Tache4;
                case BudgetComparaisonService.AxeAnalytique.NodeType.Chapitre:
                    return $ctrl.resources.BudgetComparaison_AxeTooltipHeader_Chapitre;
                case BudgetComparaisonService.AxeAnalytique.NodeType.SousChapitre:
                    return $ctrl.resources.BudgetComparaison_AxeTooltipHeader_SousChapitre;
                case BudgetComparaisonService.AxeAnalytique.NodeType.Ressource:
                    return $ctrl.resources.BudgetComparaison_AxeTooltipHeader_Ressource;
            }
            return "";
        }
    }
})();
