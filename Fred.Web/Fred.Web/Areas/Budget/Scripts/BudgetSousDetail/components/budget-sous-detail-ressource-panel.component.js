(function () {
    'use strict';

    var budgetSousDetailRessourcePanelComponent = {
        templateUrl: '/Areas/Budget/Scripts/BudgetSousDetail/components/budget-sous-detail-ressource-panel.component.html',
        bindings: {
            budget: '<',
            resources: '<'
        },
        controller: budgetSousDetailRessourcePanelController
    };
    angular.module('Fred').component('budgetSousDetailRessourcePanelComponent', budgetSousDetailRessourcePanelComponent);
    angular.module('Fred').controller('budgetSousDetailRessourcePanelController', budgetSousDetailRessourcePanelController);
    budgetSousDetailRessourcePanelController.$inject = ['$scope', 'BudgetService', 'ProgressBar', 'Notify', '$timeout'];

    function budgetSousDetailRessourcePanelController($scope, BudgetService, ProgressBar, Notify, $timeout) {
        var $ctrl = this;
        var isBusy = false;
        var chapitrePage = 0;
        var chapitrePageSize = 25;
        var canLoadNextPage = true;

        //////////////////////////////////////////////////////////////////
        // Déclaration des membres  publiques                           //
        //////////////////////////////////////////////////////////////////
        $ctrl.Search = '';
        $ctrl.View = { Search: "" };
        $ctrl.Chapitres = [];
        $ctrl.ressourcesAreOpen = false;
        $ctrl.sousChapitreIsOpen = false;
        $ctrl.oldSearchText = "";
        $ctrl.bibliothequePrix = [];
        $ctrl.GereRessourcesRecommandees = false;
        $ctrl.RecommandeesUniquement = false;

        $ctrl.Hide = Hide;
        $ctrl.handleClickChapitre = handleClickChapitre;
        $ctrl.handleClickSousChapitre = handleClickSousChapitre;
        $ctrl.ressourcesFilter = ressourcesFilter;
        $ctrl.sousChapitresFilter = sousChapitresFilter;
        $ctrl.chapitresFilter = chapitresFilter;
        $ctrl.handleChangeSearch = handleChangeSearch;

        //////////////////////////////////////////////////////////////////
        // Initialisation                                               //
        //////////////////////////////////////////////////////////////////
        Hide();
        Load();

        $ctrl.$onInit = function () {
            $scope.$on(BudgetService.Events.OpenPanelRessource, function (event) { Show(); });
        };


        //////////////////////////////////////////////////////////////////
        // Chargement                                                   //
        //////////////////////////////////////////////////////////////////

        function Load() {
            if (!isBusy && canLoadNextPage) {
                isBusy = true;
                ProgressBar.start();
                BudgetService.GereRessourcesRecommandees($ctrl.budget.CI.CiId)
                    .then(GereRessourcesRecommandeesThen)
                    .catch(GereRessourcesRecommandeesCatch)
                    .finally(GereRessourcesRecommandeesFinally);
            }
        }

        function GereRessourcesRecommandeesThen(result) {
            $ctrl.GereRessourcesRecommandees = result.data;
            $ctrl.RecommandeesUniquement = result.data;
        }
        function GereRessourcesRecommandeesCatch() {
            Notify.error($ctrl.resources._Budget_Erreur_Chargement);
        }
        function GereRessourcesRecommandeesFinally() {
            BudgetService.GetChapitres($ctrl.budget.CI.CiId, $ctrl.budget.Devise.DeviseId, $ctrl.Search, getNextPage(), chapitrePageSize)
                .then(GetChapitresThen)
                .catch(GetChapitresCatch);
        }

        function GetChapitresThen(result) {

            let chapitres = result.data;

            result.data.forEach((chapitre) => {
                chapitre.SousChapitres.forEach((sousChapitre) => {
                    sousChapitre.Ressources.forEach((ressource) => {
                        if (ressource.BibliothequePrixMontant !== null) {
                            $ctrl.bibliothequePrix.push(ressource);
                        }
                    });
                });
            });

            if (chapitres.length < chapitrePageSize) {
                canLoadNextPage = false;
            }
            for (var i = 0; i < chapitres.length; i++) {
                var chapitre = chapitres[i];
                for (var j = 0; j < chapitre.SousChapitres.length; j++) {
                    var sousChapitres = chapitre.SousChapitres[j];
                    sousChapitres.isOpen = false;
                }
                chapitre.isOpen = false;
                $ctrl.Chapitres.push(chapitre);
                $ctrl.ChapitresCopy = angular.copy($ctrl.Chapitres);
            }

            GetChapitresFinally();
        }

        function GetChapitresCatch() {
            Notify.error($ctrl.resources._Budget_Erreur_Chargement);
            GetChapitresFinally();
        }

        function GetChapitresFinally() {
            ProgressBar.complete();
            isBusy = false;
            if ($ctrl.Search !== $ctrl.View.Search)
                handleSearchTextChanged();
        }


        //////////////////////////////////////////////////////////////////
        // Actions                                                      //
        //////////////////////////////////////////////////////////////////

        /*
     * @function chapitresFilter(searchText)
     * @description Filtre des Chapitres
     */
        function chapitresFilter(searchText) {
            return function (item) {
                for (var i = 0; i < item.SousChapitres.length; i++) {
                    var sousChapitre = item.SousChapitres[i];
                    for (var j = 0; j < sousChapitre.Ressources.length; j++) {
                        var ressource = sousChapitre.Ressources[j];
                        if (!$ctrl.RecommandeesUniquement || ressource.IsRecommandee) {
                            if (ressource.RessourceCode.toLowerCase().indexOf(searchText.toLowerCase()) >= 0 || ressource.RessourceLibelle.toLowerCase().indexOf(searchText.toLowerCase()) >= 0) {
                                return true;
                            }
                        }
                    }
                }
                return false;
            };
        }

        /*
         * @function sousChapitresFilter(searchText)
         * @description Filtre des Sous Chapitres
         */
        function sousChapitresFilter(searchText) {
            return function (item) {
                for (var i = 0; i < item.Ressources.length; i++) {
                    var ressource = item.Ressources[i];
                    if (!$ctrl.RecommandeesUniquement || ressource.IsRecommandee) {
                        if (ressource.RessourceCode.toLowerCase().indexOf(searchText.toLowerCase()) >= 0 || ressource.RessourceLibelle.toLowerCase().indexOf(searchText.toLowerCase()) >= 0) {
                            return true;
                        }
                    }
                }
                return false;
            };
        }

        /*
         * @function ressourcesFilter(searchText)
         * @description Filtre des Ressources
         */
        function ressourcesFilter(searchText) {
            return function (item) {
                if ($ctrl.RecommandeesUniquement && !item.IsRecommandee) {
                    return false;
                }
                return item.RessourceCode.toLowerCase().indexOf(searchText.toLowerCase()) >= 0 || item.RessourceLibelle.toLowerCase().indexOf(searchText.toLowerCase()) >= 0;
            };
        }

        function handleChangeSearch() {
            if ($ctrl.View.Search) {
                $ctrl.oldSearchText = $ctrl.View.Search;
                $ctrl.Chapitres.map(function (c) {
                    c.SousChapitres.map(s => s.isOpen = true);
                    c.isOpen = true;
                });
            } else if ($ctrl.oldSearchText && !$ctrl.View.Search) {
                $ctrl.oldSearchText = "";
                $ctrl.Chapitres.map(function (c) {
                    c.SousChapitres.map(s => s.isOpen = false);
                    c.isOpen = false;
                });
            }
        }

        function handleClickChapitre(chapitre) {
            chapitre.isOpen = !chapitre.isOpen;
        }

        function handleClickSousChapitre(sousChapitre) {
            sousChapitre.isOpen = !sousChapitre.isOpen;
        }


        //////////////////////////////////////////////////////////////////
        // Autre                                                        //
        //////////////////////////////////////////////////////////////////

        function getNextPage() {
            var newPage = chapitrePage;
            chapitrePage += 1;
            return newPage;
        }

        // Ouvre le panneau
        function Show() {
            $ctrl.PanelClass = "open";
            $timeout(function () {
                var selector = '#searched_text';
                var search = angular.element.find(selector)[0];
                if (search.value.length > 0) {
                    search.focus();
                    search.select();
                }
            }, 100);
        }

        // Ferme le panneau
        function Hide() {
            $ctrl.PanelClass = "close-right-panel";
        }
    }
})();
