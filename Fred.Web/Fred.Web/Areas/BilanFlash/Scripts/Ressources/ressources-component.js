(function () {
    'use strict';

    angular
        .module('Fred')
        .component('ofRessourcesComponent', {
            templateUrl: '/Areas/BilanFlash/Scripts/Ressources/ressources-template.html',
            bindings: {
                ci : '<',
                devise : '<',
                tache : '<',
                dateDebut: '<',
                resources: '<'
            },
            controller: 'ofRessourcesComponentController'
        });

    angular.module('Fred').controller('ofRessourcesComponentController', ofRessourcesComponentController);

    ofRessourcesComponentController.$inject = ['$scope', '$rootScope',
        'ProgressBar',
        'Notify',
        'ObjectifFlashService'];

    function ofRessourcesComponentController($scope, $rootScope,
        ProgressBar,
        Notify,
        ObjectifFlashService) {

        var $ctrl = this;
        var isBusy = false;
        var chapitrePage = 0;
        var chapitrePageSize = 25;
        var canLoadNextPage = true;

        $ctrl.clickAddToTask = clickAddToTask;
        $ctrl.actionUpdateRessourcesList = actionUpdateRessourcesList;
        $ctrl.handleChangeSearch = handleChangeSearch;
        $ctrl.clickChapitre = clickChapitre;
        $ctrl.clickSousChapitre = clickSousChapitre;
        $ctrl.chapitresFilter = chapitresFilter;
        $ctrl.sousChapitresFilter = sousChapitresFilter;
        $ctrl.ressourcesFilter = ressourcesFilter;

        //////////////////////////////////////////////////////////////////
        // Déclaration des propriétés publiques                         //
        //////////////////////////////////////////////////////////////////
        $ctrl.search = '';
        $ctrl.chapitres = [];
        $ctrl.ressourcesAreOpen = false;
        $ctrl.sousChapitreIsOpen = false;
        $ctrl.choiceBareme = 'BibliothequePrix';

        //////////////////////////////////////////////////////////////////
        // Init                                                         //
        //////////////////////////////////////////////////////////////////
        $ctrl.$onInit = function () {
            actionUpdateRessourcesList($ctrl.choiceBareme);

            FredToolBox.bindScrollEnd('#containerRessources', actionLoadMore);
        };

        function actionUpdateRessourcesList(type) {
            $ctrl.choiceBareme = type;
            clearRessources();
            canLoadNextPage = true;
            chapitrePage = 0;
            actionLoadRessources();
        }

        //////////////////////////////////////////////////////////////////
        // Public Methods                                               //
        //////////////////////////////////////////////////////////////////


        function clickChapitre(chapitre) {
            chapitre.isOpen = !chapitre.isOpen;
        };

        function clickSousChapitre(sousChapitre) {
            sousChapitre.isOpen = !sousChapitre.isOpen;
        };

        //////////////////////////////////////////////////////////////////
        // Actions                                                      //
        //////////////////////////////////////////////////////////////////
        function actionLoadRessources() {

            if (!isBusy && canLoadNextPage) {
                // controle des valeurs
                if (!$ctrl.ci) {
                    Notify.error('CI manquant');
                    return;
                }
                if (!$ctrl.devise) {
                    Notify.error('Devise manquante pour ce CI');
                    return;
                }
                if (!$ctrl.tache) {
                    Notify.error('Pas de tache selectionnée');
                    return;
                }
                if (!$ctrl.dateDebut) {
                    Notify.error('Pas de date de début définie');
                    return;
                }

                isBusy = true;
                ProgressBar.start();
                switch ($ctrl.choiceBareme) {
                    case 'BudgetValide':
                        ObjectifFlashService.GetRessourcesInBudgetEnApplicationByCiId($ctrl.ci.CiId, $ctrl.tache.TacheId)
                            .then(actionLoadRessourcesSuccess)
                            .catch(actionLoadError)
                            .finally(actionLoadEnd);
                        break;
                    case 'BibliothequePrix':
                        ObjectifFlashService.GetRessourcesInBibliothequePrix($ctrl.ci, $ctrl.devise, $ctrl.search, getNextPage(), chapitrePageSize)
                            .then(actionLoadRessourcesSuccess)
                            .catch(actionLoadError)
                            .finally(actionLoadEnd);
                        break;
                    case 'ExploitationCi':
                        ObjectifFlashService.GetRessourcesInBaremeExploitation($ctrl.ci, $ctrl.dateDebut)
                            .then(actionLoadRessourcesSuccess)
                            .catch(actionLoadError)
                            .finally(actionLoadEnd);
                        break;
                }

            }
        }

        function actionLoadRessourcesSuccess(result) {

            if (($ctrl.choiceBareme === 'BibliothequePrix' && result.data.length < chapitrePageSize) || $ctrl.choiceBareme !== 'BibliothequePrix') {
                canLoadNextPage = false;
            }

            result.data.forEach((chapitre) => {
                chapitre.isOpen = false;
                chapitre.SousChapitres.forEach((sousChapitre) => {
                    sousChapitre.isOpen = false;
                });
                $ctrl.chapitres.push(chapitre);
            });
        }

        function actionLoadError() {
            Notify.error($ctrl.resources.Global_Notification_Error);
        }

        function actionLoadEnd() {
            ProgressBar.complete();
            isBusy = false;
        }

        function actionLoadMore() {
            actionLoadRessources();
        }

        function clearRessources() {
            $ctrl.chapitres = [];
        }

        function getNextPage() {
            var newPage = chapitrePage;
            chapitrePage += 1;
            return newPage;
        }

        function clickAddToTask(newRessource) {
            $scope.$emit('objectifFlashRessourceSelected', { ressource: newRessource });
        };

        /*
        * @function chapitresFilter(searchText)
        * @description Filtre des Chapitres
        */
        function chapitresFilter(searchText) {
            return function (item) {
                for (var i = 0; i < item.SousChapitres.length; i++) {
                    for (var j = 0; j < item.SousChapitres[i].Ressources.length; j++) {
                        if (item.SousChapitres[i].Ressources[j].Code.toLowerCase().indexOf(searchText.toLowerCase()) >= 0 || item.SousChapitres[i].Ressources[j].Libelle.toLowerCase().indexOf(searchText.toLowerCase()) >= 0) {
                            return true;
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
                    if (item.Ressources[i].Code.toLowerCase().indexOf(searchText.toLowerCase()) >= 0 || item.Ressources[i].Libelle.toLowerCase().indexOf(searchText.toLowerCase()) >= 0) {
                        return true;
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
                return (item.Code.toLowerCase().indexOf(searchText.toLowerCase()) >= 0 || item.Libelle.toLowerCase().indexOf(searchText.toLowerCase()) >= 0);
            };
        }

        function handleChangeSearch() {
            if ($ctrl.search) {
                $ctrl.oldSearchText = $ctrl.search;
                $ctrl.chapitres.map(function (c) {
                    c.SousChapitres.map(s => s.isOpen = true);
                    c.isOpen = true;
                });
            } else if ($ctrl.oldSearchText && !$ctrl.search) {
                $ctrl.oldSearchText = "";
                $ctrl.chapitres.map(function (c) {
                    c.SousChapitres.map(s => s.isOpen = false);
                    c.isOpen = false;
                });
            }
        }


    }
})();