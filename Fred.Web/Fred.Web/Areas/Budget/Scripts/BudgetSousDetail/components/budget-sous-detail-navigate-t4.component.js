(function () {
    'use strict';

    var budgetSousDetailNavigateT4Component = {
        templateUrl: '/Areas/Budget/Scripts/BudgetSousDetail/components/budget-sous-detail-navigate-t4.component.html',
        bindings: {
            resources: '<',
            budget: '<',
            sousDetail: '<'
        },
        controller: budgetSousDetailNavigateT4Controller
    };
    angular.module('Fred').component('budgetSousDetailNavigateT4Component', budgetSousDetailNavigateT4Component);
    angular.module('Fred').controller('budgetSousDetailNavigateT4Controller', budgetSousDetailNavigateT4Controller);

    budgetSousDetailNavigateT4Controller.$inject = ['$scope', '$rootScope' ,'BudgetService', 'BudgetDetailService'];

    function budgetSousDetailNavigateT4Controller($scope, $rootScope, BudgetService, BudgetDetailService) {
        var $ctrl = this;

        $ctrl.onNavigateT4 = onNavigateT4;
        $ctrl.isT4DisplayedFirstInList = isT4DisplayedFirstInList;
        $ctrl.isT4DisplayedLastInList = isT4DisplayedLastInList;
        $ctrl.previousT4ButtonMessage = previousT4ButtonMessage;
        $ctrl.nextT4ButtonMessage = nextT4ButtonMessage;
        $ctrl.showBandeauLateral = showBandeauLateral;

        $ctrl.NavigateT4Enum = BudgetDetailService.NavigateT4Enum;

        function onNavigateT4(navigateOption) {

            let displayedT4Index = GetDisplayedT4Index();
            let t4AsDisplayed = GetT4CodeAsDisplayed();
            let t4ToGoTo = null;
            if (navigateOption === $ctrl.NavigateT4Enum.PreviousT4) {
                if (displayedT4Index <= 0) {
                    return;
                }

                t4ToGoTo = t4AsDisplayed[displayedT4Index - 1];
            } else if (navigateOption === $ctrl.NavigateT4Enum.NextT4) {
                if (displayedT4Index === -1 || displayedT4Index + 1 === t4AsDisplayed.length) {
                    return;
                }

                t4ToGoTo = t4AsDisplayed[displayedT4Index + 1];
            }

            let allT4 = [...$ctrl.budget.GetTaches4()];
            let t4ToDisplay = allT4.find(t4 => {
                return t4.Code === t4ToGoTo;
            });

            $scope.$emit(BudgetService.Events.LoadSousDetail, { Tache4: t4ToDisplay });
        }

        function isT4DisplayedFirstInList() {
            let displayedT4Index = GetDisplayedT4Index();
            return displayedT4Index === 0;
        }

        function isT4DisplayedLastInList() {
            let displayedT4Index = GetDisplayedT4Index();
            let t4AsDisplayed = GetT4CodeAsDisplayed();
            return displayedT4Index === t4AsDisplayed.length - 1;
        }

        function previousT4ButtonMessage() {
            if ($ctrl.isT4DisplayedFirstInList()) {
                return $ctrl.resources.Budget_Sous_Detail_ToolBar_Titre_No_Previous_T4;
            } else {
                return $ctrl.resources.Budget_Sous_Detail_ToolBar_Titre_Visualize_Previous_T4;
            }
        }

        function nextT4ButtonMessage() {
            if ($ctrl.isT4DisplayedLastInList()) {
                return $ctrl.resources.Budget_Sous_Detail_ToolBar_Titre_No_Next_T4;
            } else {
                return $ctrl.resources.Budget_Sous_Detail_ToolBar_Titre_Visualize_Next_T4;
            }
        }

        function showBandeauLateral() {
            $rootScope.$broadcast(BudgetService.Events.DisplayPlanTacheLateral);
        }



        /**
         * Cette fonction retourne l'index du T4 dans la liste telle qu'affichée à l'écran (et non pas comme contenu par $ctrl.Budget)
         * @returns {any} un entier jamais null ni undefined
         * */
        function GetDisplayedT4Index() {
            let t4Affiche = $ctrl.sousDetail.BudgetT4;
            let codeTacheT4Affichee = t4Affiche.Tache.Code;

            let t4AsDisplayed = GetT4CodeAsDisplayed();
            let displayedT4Index = t4AsDisplayed.indexOf(codeTacheT4Affichee);
            return displayedT4Index;

        }

        /**
         * Renvoi sous forme de tableau de string, les codes de toutes les T4 telles qu'affichées à l'écran
         * @returns {any} un tableau de string
         * */
        function GetT4CodeAsDisplayed() {
            //le controlleur contient la liste des taches affichées mais ordonnées différemment, à l'écran les taches sont ordonnées par Code 
            //par angular (orderBy dans un ng-repeat), donc on ne peut pas utiliser la liste des taches présente dans le controlleur
            let t4AffichesDansDetail = $('.budget-detail:not(.bandeau-lateral) .level-4 .col-1').map((index, dom) => dom.innerText.trim()).toArray();
            return t4AffichesDansDetail;
        }


    }
})();