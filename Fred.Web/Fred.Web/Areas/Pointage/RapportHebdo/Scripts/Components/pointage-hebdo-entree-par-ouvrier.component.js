(function () {
    'use strict';

    var pointageHebdoEntreeParOuvrierComponent = {
        templateUrl: '/Areas/Pointage/RapportHebdo/Scripts/Components/pointage-hebdo-entree-par-ouvrier.component.html',
        bindings: {
            resources: '<',
            ouvrierList: '=',
            ouvrierListGrouped: '=',
            ouvrierListByMode: '=',
            isAffichageCi: '<'
        },
        controller: PointageHebdoEntreeParOuvrierController,
        controllerAs: '$ctrl'
    };

    angular.module('Fred').component('pointageHebdoEntreeParOuvrierComponent', pointageHebdoEntreeParOuvrierComponent);

    angular.module('Fred').controller('PointageHebdoEntreeParOuvrierController', PointageHebdoEntreeParOuvrierController);

    /*
     * Custom filter pour récupérer tous les éléments si le isInTeam est false ou undéfini 
    */
    angular.module('Fred').filter('equipeFilter', function () {
        return function (items, isInTeam) {
            if (!isInTeam || '' === isInTeam) {
                return items;
            }

            return items.filter(function (element, index, array) {
                return element.IsInFavouriteTeam === isInTeam;
            });

        };
    });

    PointageHebdoEntreeParOuvrierController.$inject = ['$scope', 'PointageHedboService', '$filter'];

    function PointageHebdoEntreeParOuvrierController($scope, PointageHedboService, $filter) {
        var $ctrl = this;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                  Initialisation                                                                      //
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        init();
        $ctrl.ishide = true;
        /*
         * Init component
        */
        function init() {
            $ctrl.filterEquipeFavorite = false;
            toggleOuvrierListToShow();
            $ctrl.selectedCi = [];
            $ctrl.selectedOuvrierList = [];
            $ctrl.favoriteTeamList = [];

            $ctrl.filterCi = function (el) {
                if (!$ctrl.isAffichageCi) {
                    return true;
                }

                return isOuvrierOfSelectedCiList(el);
            };

            // Watcher pour détécter le changement du mode d'affichage
            $scope.$on('event.change.mode', function (evt, data) {
                toggleOuvrierListToShow();
            });

            // Watcher pour détécter le changement de séléction d'un ouvrier
            $scope.$on('event.change.ci.ouvrier', function (evt, selectedCi) {
                $ctrl.equipeFavoriteSelected = false;
                angular.forEach($ctrl.ouvrierListByMode, function (val) {
                    val.Selected = false;
                });
                $ctrl.selectedCi = selectedCi;
                
                if ($ctrl.selectedCi.length !== 0) {
                    $ctrl.favoriteTeamList = $ctrl.ouvrierListByMode.filter(function (s) { return s.IsInFavouriteTeam; });
                }
                else {
                    $ctrl.favoriteTeamList = [];
                }

                selectOrUnSelectGlobalPersonnelSelectionButton();
            });

            initHandlers();
        }

        /*
         * Init handlers
        */
        function initHandlers() {
            $ctrl.handleSelectionEquipeFavorite = function () {
                $ctrl.filterEquipeFavorite = !$ctrl.filterEquipeFavorite;
            };

            $ctrl.handleChangeWeek = function (isAddWeek) {
                actionChangeWeek(isAddWeek);
            };

            $ctrl.equipeFavoriteSelected = false;
            $ctrl.handleCheckEquipeFavoriteMembers = function (change) {
                $ctrl.equipeFavoriteSelected = !$ctrl.equipeFavoriteSelected;
                angular.forEach($ctrl.ouvrierListByMode.filter(function (s) { return s.IsInFavouriteTeam; }),
                    function (val) {
                        val.Selected = $ctrl.equipeFavoriteSelected;
                    });

                var filtredList = $ctrl.ouvrierListByMode.filter(function (i) {
                    return i.Selected;
                });

                $ctrl.selectedOuvrierList = [];
                angular.forEach(filtredList, function (ouvrier) {
                    $ctrl.selectedOuvrierList.push({ PersonnelId: ouvrier.PersonnelId });
                });

                updateSelection();
            };
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                  General                                                                      //
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /*
         * Handle select all ouvrier 
        */
        $ctrl.selectAllOuvrier = function (change) {
            angular.forEach($ctrl.ouvrierListByMode, function (val) {
                val.Selected = change;
            });

            selectAllOuvrier(change);
        };

        $ctrl.check = function () {
            if ($ctrl.favoriteTeamList.length === 0) {
                return true;
            }
            else {
                return false;
            }
        };

        function selectAllOuvrier(change) {
            if (!$ctrl.ouvrierListByMode || !$ctrl.isAffichageCi) {
                return;
            }

            angular.forEach($ctrl.ouvrierListByMode, function (ouvrier) {
                var isOuvrierOfCurrentCi = isOuvrierOfSelectedCiList(ouvrier);
                if (isOuvrierOfCurrentCi) {
                    if (change) {
                        $ctrl.selectedOuvrierList.push({ PersonnelId: ouvrier.PersonnelId });
                    } else {
                        $ctrl.selectedOuvrierList = $ctrl.selectedOuvrierList.filter(function (i) {
                            return i.PersonnelId !== ouvrier.PersonnelId;
                        });
                    }
                }
            });

            updateSelection();
        }

        /*
         * Séléction d'un seul ouvrier 
        */
        $ctrl.handleSelectOuvrier = function (personnelId) {
            if ($ctrl.isAffichageCi) {
                return;
            }
            $scope.$emit('event.change.select.by.ouvrier', personnelId);
        };

        /*
        * Toggle ouvrier list to show
        */
        function toggleOuvrierListToShow() {
            $ctrl.ouvrierListByMode = $ctrl.ouvrierListGrouped;
        }

        /*
         * Handle select a ouvrier 
        */
        $ctrl.toggleOuvrierSelection = function (ouvrier) {
            if (ouvrier.checkState) {
                $ctrl.selectedOuvrierList.push({ PersonnelId: ouvrier.PersonnelId });
            } else {
                var filtredList = $ctrl.selectedOuvrierList.filter(function (i) {
                    return (i.PersonnelId !== ouvrier.PersonnelId);
                });
                $ctrl.selectedOuvrierList = filtredList;
            }

            updateSelection();
            selectOrUnSelectGlobalPersonnelSelectionButton();
        };

        // Watcher pour détécter le changement du mode d'affichage
        $scope.$on('event.change.mode', function (evt, data) {
            $ctrl.selectedOuvrierList = [];
            $ctrl.selectedCi = [];
            $ctrl.filterEquipeFavorite = false;
            $ctrl.SelectAllOuvrier = false;
            angular.forEach($ctrl.ouvrierListByMode, function (val) {
                val.Selected = false;
            });
        });

        /*
         * Update de la séléction au niveau des écrans d'entrée
        */
        function updateSelection() {
            if (!$ctrl.selectedCi || $ctrl.selectedCi.length === 0 || !$ctrl.selectedOuvrierList || $ctrl.selectedOuvrierList.length === 0) {
                return;
            }

            var selectedPersonnelIdList = $ctrl.selectedOuvrierList.map(function (r) { return r.PersonnelId; });
            var selection = [];
            angular.forEach($ctrl.selectedCi, function (ciId) {
                var ouvrierList = $ctrl.ouvrierList
                    .filter(function (e) { return e.CiId === ciId && selectedPersonnelIdList && selectedPersonnelIdList.indexOf(e.PersonnelId) !== -1; })
                    .map(function (o) { return o.PersonnelId; });


                angular.forEach(ouvrierList, function (ouvrierId) {
                    selection.push({ PersonnelId: ouvrierId, CiId: ciId });
                });
            });

            PointageHedboService.updateSelectedEntreeOuvrierList(selection);
        }

        /*
         * Méthode pour voir si un ouvrier est dans la liste des Ci séléctionnés
        */
        function isOuvrierOfSelectedCiList(el) {
            return $ctrl.ouvrierList
                && $ctrl.ouvrierList.length > 0
                && $ctrl.ouvrierList.filter(function (m) { return m.PersonnelId === el.PersonnelId && $ctrl.selectedCi.length !== 0 && $ctrl.selectedCi.indexOf(m.CiId) !== -1; })
                    .length > 0;
        }

        function selectOrUnSelectGlobalPersonnelSelectionButton() {
            if (allPersonnelsAreSelected()) {
                $ctrl.SelectAllOuvrier = true;
            } else {
                $ctrl.SelectAllOuvrier = false;
            }
        }

        function allPersonnelsAreSelected() {
            var listAfterApplyEquipeFilter = $filter('equipeFilter')($ctrl.ouvrierListByMode, $ctrl.filterEquipeFavorite, true);
            var applyEquipeFilter = $filter('filter')(listAfterApplyEquipeFilter, $ctrl.filterCi, true);
            var listUnSelected = applyEquipeFilter.filter(function (o) {
                return o.Selected === false;
            });
            return listUnSelected.length > 0 ? false : true;
        }


    }
})();
