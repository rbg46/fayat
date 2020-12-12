(function () {
    'use strict';

    var budgetDetailCustomizeDisplayPanelComponent = {
        templateUrl: '/Areas/Budget/Scripts/BudgetDetail/components/budget-detail-customize-display-panel.component.html',
        bindings: {
            resources: '<',
            onCustomizeDisplay: '&'
        },
        controller: budgetDetailCustomizeDisplayPanelController
    };

    angular.module('Fred').component('budgetDetailCustomizeDisplayPanelComponent', budgetDetailCustomizeDisplayPanelComponent);
    angular.module('Fred').controller('budgetDetailCustomizeDisplayPanelController', budgetDetailCustomizeDisplayPanelController);
    budgetDetailCustomizeDisplayPanelController.$inject = ['$rootScope', '$scope', 'BudgetService', '$timeout'];

    function budgetDetailCustomizeDisplayPanelController($rootScope, $scope, BudgetService, $timeout) {
        var $ctrl = this;

        //////////////////////////////////////////////////////////////////
        // Déclaration des membres publiques                            //
        //////////////////////////////////////////////////////////////////
        $ctrl.Hide = Hide;
        $ctrl.IsSelected = true;
        $ctrl.ChangeAxeSelection = ChangeAxeSelection;
        $ctrl.ChangeColumnSelection = ChangeColumnSelection;
        $ctrl.NotifyChanges = NotifyChanges;
        $ctrl.Validate = Validate;
        $ctrl.Cancel = Cancel;

        $ctrl.Axes = [{ Niveau: 1, Label: 'T1', IsSelected: true },
            {Niveau: 2, Label: 'T2', IsSelected: true },
            {Niveau: 3, Label: 'T3', IsSelected: true },
            {Niveau: 4, Label: 'T4', IsSelected: true }];

        $ctrl.Columns = [{ Code: BudgetService.DetailColumnEnum.Unite, Label: 'Unité', IsSelected: true },
            { Code: BudgetService.DetailColumnEnum.Quantite, Label: 'Quantité', IsSelected: true },
            { Code: BudgetService.DetailColumnEnum.PU, Label: 'PU', IsSelected: true }];


        //////////////////////////////////////////////////////////////////
        // Initialisation                                               //
        //////////////////////////////////////////////////////////////////
        Hide();
        $ctrl.$onInit = function () {
            if (sessionStorage.getItem('budgetDetailFilter') !== null) {
                $ctrl.filter = JSON.parse(sessionStorage.getItem('budgetDetailFilter'));
                $ctrl.Columns.forEach(column => column.IsSelected = $ctrl.filter.ColumnsVisible.includes(column.Code));
                $ctrl.Axes.forEach(axe => axe.IsSelected = $ctrl.filter.NiveauxVisible.includes(axe.Niveau));
                NotifyChanges(false);
            }
            $scope.$on(BudgetService.Events.OpenPanelCustomizeDisplay, function (event, arg) {
                Show();
            });
        };

        // Ouvre le panneau
        function Show() {
            $ctrl.PanelClass = "open";
        }

        // Ferme le panneau
        function Hide() {
            $ctrl.PanelClass = "close-right-panel";
        }

        function ChangeColumnSelection (column) {
            column.IsSelected = !column.IsSelected;
        }

        function ChangeAxeSelection(axe) {
            axe.IsSelected = !axe.IsSelected;
        }

        // validation des changements
        function Validate() {
            $ctrl.NotifyChanges(true);
            $ctrl.Hide();
        }

        // reset à l'état initial,tout est selectionné
        function Cancel() {
            $ctrl.Columns.forEach(column => column.IsSelected = true);
            $ctrl.Axes.forEach(axe => axe.IsSelected = true);
            $ctrl.NotifyChanges(true);
        }

        // Notifie les changements, events et callback methods
        function NotifyChanges(saveSession) {
            var columnsSelected = $ctrl.Columns.filter(column => column.IsSelected).map(column => column.Code);
            var niveauxSelected = $ctrl.Axes.filter(axe => axe.IsSelected).map(axe => axe.Niveau);
            
            $rootScope.$broadcast(BudgetService.Events.PanelCustomizeDisplayModified, {
                ColumnsVisible: columnsSelected,
                NiveauxVisible: niveauxSelected,
                IsDisplayCustomized: columnsSelected.length !== $ctrl.Columns.length
                    || niveauxSelected.length !== $ctrl.Axes.length,
                SaveSession: saveSession
            });
             
        }
    }
})();
