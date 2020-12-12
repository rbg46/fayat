(function () {
    'use strict';

    var budgetComparaisonFilterComponent = {
        templateUrl: '/Areas/Budget/Scripts/BudgetComparaison/components/budget-comparaison-filter.component.html',
        bindings: {},
        controller: budgetComparaisonFilterController
    };
    angular.module('Fred').component('budgetComparaisonFilterComponent', budgetComparaisonFilterComponent);
    angular.module('Fred').controller('budgetComparaisonFilterController', budgetComparaisonFilterController);
    budgetComparaisonFilterController.$inject = ['$scope', 'BudgetComparaisonService'];
    function budgetComparaisonFilterController($scope, BudgetComparaisonService) {

        //////////////////////////////////////////////////////////////////
        // Membres                                                      //
        //////////////////////////////////////////////////////////////////
        var $ctrl = this;
        $ctrl.resources = resources;
        $ctrl.BudgetComparaisonService = BudgetComparaisonService;
        $ctrl.Visible = false;
        $ctrl.Filter = null;

        $ctrl.OnAxeTacheRessourceButtonClick = OnAxeTacheRessourceButtonClick;
        $ctrl.OnAxeRessourceTacheButtonClick = OnAxeRessourceTacheButtonClick;
        $ctrl.OnToggleAxeAnalytiqueNodeType = OnToggleAxeAnalytiqueNodeType;
        $ctrl.OnSaveButtonClick = OnSaveButtonClick;
        $ctrl.OnCancelButtonClick = OnCancelButtonClick;

        $ctrl.IsAxeAnalytiqueSelected = IsAxeAnalytiqueSelected;
        $ctrl.Hide = Hide;
        $ctrl.CanSave = CanSave;

        $scope.$on(BudgetComparaisonService.Events.DisplayFilterPanel, function (event, args) { Show(args); });


        //////////////////////////////////////////////////////////////////
        // Evènements internes                                          //
        //////////////////////////////////////////////////////////////////

        // Appelé sur le clic du bouton tâche / ressource
        function OnAxeTacheRessourceButtonClick() {
            $ctrl.Filter.AxeAnalytique.Type = BudgetComparaisonService.AxeAnalytique.Type.TacheRessource;
        }

        // Appelé sur le clic du bouton ressource / tâche
        function OnAxeRessourceTacheButtonClick() {
            $ctrl.Filter.AxeAnalytique.Type = BudgetComparaisonService.AxeAnalytique.Type.RessourceTache;
        }

        // Appelé sur le clic d'un noeud d'axe analytique
        function OnToggleAxeAnalytiqueNodeType(nodeType) {
            let index = GetAxeAnalytiqueNodeTypeIndex(nodeType);
            if (index === -1) {
                $ctrl.Filter.AxeAnalytique.NodeTypes.push(nodeType);
            }
            else {
                $ctrl.Filter.AxeAnalytique.NodeTypes.splice(index, 1);
            }
        }

        // Appelé sur le clic du bouton enregistrer
        function OnSaveButtonClick() {
            if ($ctrl.OnValidate) {
                // Le tableau des types de noeud peut-être désordonnée ici, il faut le remettre dans l'ordre original
                $ctrl.Filter.AxeAnalytique.Update();
                let axeAnalytiqueChanged = $ctrl.Filter.AxeAnalytique.Type !== BudgetComparaisonService.Filter.AxeAnalytique.Type
                    || !angular.equals(BudgetComparaisonService.Filter.AxeAnalytique.NodeTypes, $ctrl.Filter.AxeAnalytique.NodeTypes);
                BudgetComparaisonService.Filter = $ctrl.Filter;
                $ctrl.OnValidate(axeAnalytiqueChanged);
            }
            $ctrl.Hide();
        }

        // Appelé sur le clic du bouton annuler
        function OnCancelButtonClick() {
            $ctrl.Filter = angular.copy(BudgetComparaisonService.Filter);
        }


        //////////////////////////////////////////////////////////////////
        // View                                                         //
        //////////////////////////////////////////////////////////////////
        function IsAxeAnalytiqueSelected(nodeType) {
            if (!$ctrl.Filter) {
                return false;
            }
            return GetAxeAnalytiqueNodeTypeIndex(nodeType) !== -1;
        }


        //////////////////////////////////////////////////////////////////
        // Autre                                                        //
        //////////////////////////////////////////////////////////////////

        function Show(args) {
            $ctrl.Filter = angular.copy(BudgetComparaisonService.Filter);
            $ctrl.OnValidate = args.OnValidate;
            $ctrl.Visible = true;
        }

        function Hide() {
            $ctrl.Visible = false;
        }

        function CanSave() {
            if (!$ctrl.Filter) {
                return false;
            }
            return $ctrl.Filter.AxeAnalytique.NodeTypes.length > 0;
        }

        function GetAxeAnalytiqueNodeTypeIndex(nodeType) {
            if (!$ctrl.Filter) {
                return null;
            }
            return $ctrl.Filter.AxeAnalytique.NodeTypes.findIndex((n) => n === nodeType);
        }
    }
})();
