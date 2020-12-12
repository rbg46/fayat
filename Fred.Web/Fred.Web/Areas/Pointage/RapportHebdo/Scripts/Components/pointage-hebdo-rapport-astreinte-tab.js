(function () {
    'use strict';

    var pointageHebdoRapportAstreinteTabComponent = {
        templateUrl: '/Areas/Pointage/RapportHebdo/Scripts/Components/pointage-hebdo-rapport-astreinte-tab.html',
        bindings: {
            resources: '<',
            isManagerPointing: '<',
            mondayDate: '<'
        },
        controller: PointageHebdoRapportAstreinteTabController,
        controllerAs: '$ctrl'
    };

    angular.module('Fred').component('pointageHebdoRapportAstreinteTabComponent', pointageHebdoRapportAstreinteTabComponent);

    angular.module('Fred').controller('PointageHebdoRapportAstreinteTabController', PointageHebdoRapportAstreinteTabController);

    PointageHebdoRapportAstreinteTabController.$inject = ['$scope', '$filter', 'Notify', 'PointageHedboService'];

    function PointageHebdoRapportAstreinteTabController($scope, $filter, Notify, PointageHedboService) {
        // assignation de la valeur du scope au controller pour les templates non mis à jour
        var $ctrl = this;
        $ctrl.isAffichageParOuvrier = true;
        $ctrl.resources = resources;
        $ctrl.IsShevalError = false;
        $ctrl.IsSuperieureError = false;
        $ctrl.IsTotalHoursError = false;
        $ctrl.subNodeList = [];
        init();

        // méthodes exposées
        angular.extend($ctrl, {
            addSortie: addSortie,
            handleAddSortieAstreinte: handleAddSortieAstreinte,
            handleDeleteSortieAstreinte: handleDeleteSortieAstreinte,
            handleGetAstreinteTime: handleGetAstreinteTime,
            handleChangeAstreinteDates: handleChangeAstreinteDates,
            handleChangeDateFinAstreinte: handleChangeDateFinAstreinte,
            handleChangeDateDebutAstreinte: handleChangeDateDebutAstreinte,
            handleGetDebutAstreinteMinDate: handleGetDebutAstreinteMinDate,
            handleGetFinAstreinteMinDate: handleGetFinAstreinteMinDate,
            handleGetDebutAstreinteMaxDate: handleGetDebutAstreinteMaxDate,
            handleGetFinAstreinteMaxDate: handleGetFinAstreinteMaxDate,
            handleCheckAstreinte: handleCheckAstreinte,
            handleSuperieureCondition: handleSuperieureCondition,
            handleShevalCondition: handleShevalCondition,
            checkAllAstreintes: checkAllAstreintes,
            TestTotalHoursByDay: TestTotalHoursByDay,
            displayNoAstreinte: displayNoAstreinte,
            displayButtonAddAstreinte: displayButtonAddAstreinte
        });


        $scope.$on('event.refresh.pointage.hebdo.astreinte.panel', function (evt, data) {
            $ctrl.isAffichageParOuvrier = data.isAffichageParOuvrier;
            $ctrl.data = data.rapportAstreintePanelData;
            actionConvertToLocaleDate();
            actionCalculateTotalHeureAstreintes();
        });

        $scope.$on('event.change.mode', function (evt, data) {
            $ctrl.isAffichageCi = data.isAffichageCi;
        });

        /**
         * Initialisation du controller.     
         */
        function init() {
            angular.extend($ctrl, {
                isAffichageCi: false
            });
        }

        //////////////////////////////////////////////////////////////////
        // Handlers                                                     //
        //////////////////////////////////////////////////////////////////

        /*
         * @description Permet de gérer l'evenement ajout d'une sortie astreinte
         */
        function handleAddSortieAstreinte(subSubNodeItem) {
            var todayDate = moment(subSubNodeItem.Date).set({ hour: 0, minute: 0 });
            var sortieAstreinte = {
                RapportLigneAstreinteId: 0,
                RapportLigneId: subSubNodeItem.RapportLigneId ? 0 : subSubNodeItem.RapportLigneId,
                AstreinteId: subSubNodeItem.AstreinteId,
                DateDebutAstreinte: todayDate.utc(true),
                DateFinAstreinte: moment(todayDate).add(15, "minutes").utc(true)
            };

            if (subSubNodeItem.ListRapportLigneAstreintes === null) {
                subSubNodeItem.ListRapportLigneAstreintes = [];
            }

            subSubNodeItem.ListRapportLigneAstreintes.push(sortieAstreinte);
            actionCalculateTotalHeureAstreintes();
            checkAllAstreintes();

        }

        function handleGetAstreinteTime(astreinteDate) {
            if (astreinteDate) {
                return moment(astreinteDate).format('HH:mm');
            }
            return "";
        }

        /*
         * @description Permet de gérer l'evenement supression sortie astreinte
         */
        function handleDeleteSortieAstreinte(subNode, indexSubSubNode, subSubNodeItem, ligneAstreinte) {
            var indexOfSortieAstreinteToDelete = subSubNodeItem.ListRapportLigneAstreintes.indexOf(ligneAstreinte);
            if (indexOfSortieAstreinteToDelete !== -1) {
                subSubNodeItem.ListRapportLigneAstreintes.splice(indexOfSortieAstreinteToDelete, 1);
            }
            actionCalculateTotalHeureAstreintes();
            checkAllAstreintes();
            checkSortie(subNode, indexSubSubNode);
        }

        /*
         * @description Permet de gérer l'evenement changement de date d'astreinte
         */
        function handleChangeAstreinteDates(firstLevelItem, secondeLevelItem, thirdLevelItem, dayOfWeek, item, ligneAstreinte) {

            // Récupérer les index de l'element à modifier
            $ctrl.FirstLevelItemIndex = $ctrl.data.indexOf(firstLevelItem);
            $ctrl.SecondeLevelItemIndex = $ctrl.data[$ctrl.FirstLevelItemIndex].SubNodeList.indexOf(secondeLevelItem);
            $ctrl.ThirdLevelItemIndex = $ctrl.data[$ctrl.FirstLevelItemIndex].SubNodeList[$ctrl.SecondeLevelItemIndex].SubNodeList.indexOf(thirdLevelItem);
            $ctrl.AstreinteItemIndex = $ctrl.data[$ctrl.FirstLevelItemIndex].SubNodeList[$ctrl.SecondeLevelItemIndex].SubNodeList[$ctrl.ThirdLevelItemIndex].Items.indexOf(item);
            $ctrl.AstreinteIndexToUpdate = $ctrl.data[$ctrl.FirstLevelItemIndex].SubNodeList[$ctrl.SecondeLevelItemIndex].SubNodeList[$ctrl.ThirdLevelItemIndex].Items[$ctrl.AstreinteItemIndex].ListRapportLigneAstreintes.indexOf(ligneAstreinte);

            $("#changeAstreinteDateModalDayOfWeek" + dayOfWeek).modal('toggle');
        }


        function TestTotalHoursByDay(i) {
            if (typeof $ctrl.FirstLevelItemIndex === 'undefined' || typeof $ctrl.SecondeLevelItemIndex === 'undefined') {
                return true;
            }
            var TotalHoursItems = $ctrl.isAffichageParOuvrier ? $ctrl.data[$ctrl.FirstLevelItemIndex].TotalHoursItems[i] : $ctrl.data[$ctrl.FirstLevelItemIndex].SubNodeList[$ctrl.SecondeLevelItemIndex].TotalHoursItems[i];
            if (TotalHoursItems > 24) {
                return false;
            }
            return true;
        }

        /*
         * @description Gérer l'evenement qui actualise les heures totales lors de changement de la date d'une sortie astreinte
         */
        function handleChangeDateFinAstreinte(i) {
            if (!$("#changeAstreinteDateModalDayOfWeek" + i).hasClass('in')) return;
            actionCalculateTotalHeureAstreintes();
            TestTotalHoursByDay(i);
            checkAllAstreintes();
        }

        function handleChangeDateDebutAstreinte(i) {
            if (!$("#changeAstreinteDateModalDayOfWeek" + i).hasClass('in')) return;
            actionCalculateTotalHeureAstreintes();
            TestTotalHoursByDay(i);
            checkAllAstreintes();
        }

        /*
         * @description Permet de récupérer la date min de la début d'une sortie astreinte
         */
        function handleGetDebutAstreinteMinDate(date) {
            if (date) {
                return moment(date).set({ hour: 0, minute: 0 }).format("DD/MM/YYYY HH:mm");
            }
            return "";
        }

        /*
         * @description Permet de récupérer la date min de la fin d'une sortie astreinte
         */
        function handleGetFinAstreinteMinDate(date) {
            if (date) {
                return moment(date).set({ hour: 0, minute: 0 }).format("DD/MM/YYYY HH:mm");
            }
            return "";
        }

        /*
         * @description Permet de récupérer la date max de la début d'une sortie astreinte
         */
        function handleGetDebutAstreinteMaxDate(date) {
            if (date) {
                return moment(date).set({ hour: 23, minute: 45 }).format("DD/MM/YYYY HH:mm");
            }
            return "";
        }

        /*
         * @description Permet de récupérer la date max de la fin d'une sortie astreinte
         */
        function handleGetFinAstreinteMaxDate(date) {
            if (date) {
                return moment(date).add(1, 'days').set({ hour: 23, minute: 59 }).format("DD/MM/YYYY HH:mm");
            }
            return "";
        }

        function checkAllAstreintes() {
            var SuperieureError = false;
            var ShevalError = false;
            var totalHoursError = false;
            if ($ctrl.data === undefined) return $ctrl.IsSuperieureError || $ctrl.IsShevalError;
            $ctrl.data.forEach(function (node) {
                node.SubNodeList.forEach(function (subNode) {
                    subNode.SubNodeList.forEach(function (subSubNode) {
                        subSubNode.Items.forEach(function (item, index) {
                            var TotalHoursItems = $ctrl.isAffichageParOuvrier ? node.TotalHoursItems[index] : subSubNode.TotalHoursItems[index];
                            if (TotalHoursItems > 24) {
                                totalHoursError = true;
                            }

                            item.ListRapportLigneAstreintes.forEach(function (ligneAstreinte) {
                                var superieureCondition = handleSuperieureCondition(ligneAstreinte.DateDebutAstreinte, ligneAstreinte.DateFinAstreinte);
                                var shevalCondition = handleShevalCondition(ligneAstreinte.DateDebutAstreinte, ligneAstreinte.DateFinAstreinte);

                                if (!superieureCondition) {
                                    SuperieureError = true;
                                }
                                if (!shevalCondition) {
                                    ShevalError = true;
                                }
                                if (!superieureCondition && !shevalCondition && totalHoursError) {
                                    $ctrl.IsSuperieureError = true;
                                    $ctrl.IsShevalError = true;
                                    $ctrl.IsTotalHoursError = true;
                                    $scope.$emit('event.change.astreinte.save.state', !($ctrl.IsSuperieureError || $ctrl.IsShevalError || $ctrl.IsTotalHoursError));
                                    return true;
                                }
                            });
                        });
                    });

                });
            });
            $ctrl.IsSuperieureError = SuperieureError;
            $ctrl.IsShevalError = ShevalError;
            $ctrl.IsTotalHoursError = totalHoursError;
            $scope.$emit('event.change.astreinte.save.state', !($ctrl.IsSuperieureError || $ctrl.IsShevalError || $ctrl.IsTotalHoursError));
            return $ctrl.IsSuperieureError || $ctrl.IsShevalError || $ctrl.IsTotalHoursError;
        }

        function checkSortie(subNode, index) {
            var listAstreintesCells = subNode.SubNodeList[index].Items.filter(node => node.HasAstreinte === true && node.ListRapportLigneAstreintes !== null && node.ListRapportLigneAstreintes.length > 0);
            if (!listAstreintesCells || listAstreintesCells.length === 0) {
                subNode.SubNodeList.splice(index, 1);
            }
            actionCalculateTotalHeureAstreintes();
        }

        function handleCheckAstreinte(datedebut, datefin) {
            var superieureCondition = handleSuperieureCondition(datedebut, datefin);
            var shevalCondition = handleShevalCondition(datedebut, datefin);

            return superieureCondition && shevalCondition;
        }

        function handleSuperieureCondition(datedebut, datefin) {
            datedebut = moment(datedebut);
            datefin = moment(datefin);
            return datedebut < datefin;
        }

        function handleShevalCondition(datedebut, datefin) {
            datedebut = moment(datedebut);
            datefin = moment(datefin);
            var heurDebutNuit = moment().set({ hour: 21, minute: 0 }).format('HH:mm');
            var heureMinuit = moment().set({ hour: 0, minute: 0 }).format('HH:mm');
            var minuteBeforeMinuit = moment().set({ hour: 23, minute: 59 }).format('HH:mm');
            var heureFinNuit = moment().set({ hour: 6, minute: 0 }).format('HH:mm');

            var dateDebutAstreinte = datedebut;
            var heureDateDebutAstreinte = moment(dateDebutAstreinte).format('HH:mm');
            var dateFinAstreinte = datefin;
            var heureDateFinAstreinte = moment(dateFinAstreinte).format('HH:mm');
            if (heureDateFinAstreinte !== heureDateDebutAstreinte) {
                if (((heureDateDebutAstreinte < heurDebutNuit && heureDateDebutAstreinte >= heureFinNuit) &&
                    ((heureDateFinAstreinte > heurDebutNuit && heureDateFinAstreinte <= minuteBeforeMinuit) || (heureDateFinAstreinte >= heureMinuit && heureDateFinAstreinte <= heureFinNuit)))
                    || (((heureDateDebutAstreinte >= heurDebutNuit && heureDateDebutAstreinte <= minuteBeforeMinuit) || (heureDateDebutAstreinte >= heureMinuit && heureDateDebutAstreinte < heureFinNuit)) &&
                        (heureDateFinAstreinte > heureFinNuit && heureDateFinAstreinte <= heurDebutNuit))) {
                    return false;
                }
            }
            return true;
        }



        /*
         * @description Convertir les dates en local
         */
        function actionConvertToLocaleDate() {
            var isChrome = !!window.chrome && (!!window.chrome.webstore || !!window.chrome.runtime);
            if ($ctrl.data) {
                angular.forEach($ctrl.data, function (firstLevelElement) {
                    angular.forEach(firstLevelElement.SubNodeList, function (secondeLevelElement) {
                        angular.forEach(secondeLevelElement.SubNodeList, function (thirdLevelElement) {
                            angular.forEach(thirdLevelElement.Items, function (item) {
                                angular.forEach(item.ListRapportLigneAstreintes, function (astreinte) {
                                    if (isChrome) {
                                        astreinte.DateDebutAstreinte = moment(astreinte.DateDebutAstreinte).utc(false);
                                        astreinte.DateFinAstreinte = moment(astreinte.DateFinAstreinte).utc(false);
                                    } else {
                                        astreinte.DateDebutAstreinte = moment($filter('toLocaleDate')(astreinte.DateDebutAstreinte)).utc(false);
                                        astreinte.DateFinAstreinte = moment($filter('toLocaleDate')(astreinte.DateFinAstreinte)).utc(false);
                                    }
                                });
                            });
                        });
                    });
                });
            }
        }

        /*
         * @description Calculer les heures totales lors de changement de la date d'une sortie astreinte
         */
        function actionCalculateTotalHeureAstreintes() {
            if ($ctrl.data) {
                angular.forEach($ctrl.data, function (firstLevelElement) {
                    firstLevelElement.TotalHoursItems = [0, 0, 0, 0, 0, 0, 0];
                    firstLevelElement.TotalHoursPerWeek = 0;
                    angular.forEach(firstLevelElement.SubNodeList, function (secondeLevelElement) {
                        secondeLevelElement.TotalHoursItems = [0, 0, 0, 0, 0, 0, 0];
                        secondeLevelElement.TotalHoursPerWeek = 0;
                        angular.forEach(secondeLevelElement.SubNodeList, function (thirdLevelElement) {
                            var thirdLeveltotalHoursPerWeek = 0;
                            angular.forEach(thirdLevelElement.Items, function (item, $index) {
                                var secondeLevelTotalHoursPerDay = 0;
                                angular.forEach(item.ListRapportLigneAstreintes, function (astreinte) {
                                    var startTime = astreinte.DateDebutAstreinte ? moment(astreinte.DateDebutAstreinte) : 0;
                                    if (startTime) {
                                        var endTime = astreinte.DateFinAstreinte ? moment(astreinte.DateFinAstreinte) : 0;
                                        if (endTime) {
                                            var duration = moment.duration(endTime.diff(startTime));
                                            if (duration) {
                                                secondeLevelTotalHoursPerDay += duration.asHours();
                                            }
                                        }
                                    }
                                });

                                secondeLevelElement.TotalHoursItems[$index] += secondeLevelTotalHoursPerDay;
                                firstLevelElement.TotalHoursItems[$index] += secondeLevelTotalHoursPerDay;
                                thirdLeveltotalHoursPerWeek += secondeLevelTotalHoursPerDay;
                            });
                            thirdLevelElement.TotalHoursPerWeek = thirdLeveltotalHoursPerWeek;
                            secondeLevelElement.TotalHoursPerWeek += thirdLeveltotalHoursPerWeek;
                            firstLevelElement.TotalHoursPerWeek += thirdLeveltotalHoursPerWeek;
                        });
                    });
                });
            }
        }

        function addSortie(subNode) {
            var personnelId = $ctrl.isAffichageParOuvrier ? $ctrl.data[0].NodeId : subNode.NodeId;
            var ciId = $ctrl.isAffichageParOuvrier ? subNode.NodeId : $ctrl.data[0].NodeId;
            PointageHedboService.GetSortie({ personnelId: personnelId, ciId: ciId, mondayDate: $ctrl.mondayDate }).$promise
                .then(function (value) {
                    var sortie = value;
                    subNode.SubNodeList.push(sortie);
                });
        }

        function displayNoAstreinte(dayCell) {
            var result = false;
            if (dayCell) {
                result = dayCell.HasAstreinte || dayCell.ListRapportLigneAstreintes.length > 0;
            }
            return result;
        }

        function displayButtonAddAstreinte(dayCell) {
            var result = false;
            if (dayCell) {
                result = dayCell.HasAstreinte && dayCell.ListRapportLigneAstreintes.length === 0;
            }
            return result;
        }

        $scope.$on('event.refresh.astreintes.affected', function (evt, data) {
            if (data.data !== undefined) {
                $ctrl.subNodeList = data.data[0].SubNodeList;
            }
        });
        //////////////////////////////////////////////////////////////////
    }
})();