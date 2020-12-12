(function (angular) {
    'use strict';

    angular.module('Fred').controller('PointageHebdoOuvrierController', PointageHebdoOuvrierController);

    PointageHebdoOuvrierController.$inject = ['$scope', '$q', 'PointageHedboService', 'Notify', 'ProgressBar'];

    function PointageHebdoOuvrierController($scope, $q, PointageHedboService, Notify, ProgressBar) {
        var $ctrl = this;
        $ctrl.hasChanges = false;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                  Initialisation                                                                //
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        init();

        /**
         * Initialisation du controller.
         */
        function init() {
            $ctrl.resources = resources;
            $ctrl.personnelStatut = "1";
            $ctrl.isAffichageCi = false;
            $ctrl.selectedDate = new Date();
            $ctrl.ouvrierList = [];
            $ctrl.ciList = [];
            $ctrl.isShowRapport = false;

            initWatchers();
            initHandlers();
            initData();
        }

        /**
         * Initialisation des listes des ouvriers.
         */
        function initData() {
            if ($ctrl.busy) {
                return;
            }
            $q.when().then(actionGetRapportHebdoSummaryForUser);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                  Handlers                                                                      //
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        function initHandlers() {
            $ctrl.handleChangeCalendarDate = function () {
                actionCalendarChangeDate();
            };

            $ctrl.handleChangeWeek = function (isAddWeek) {
                actionChangeWeek(isAddWeek);
            };

            $ctrl.handleToggleAffichage = function (isAffichageCi) {
                $ctrl.isAffichageCi = isAffichageCi;
                // Evénement à utiliser pour informer de du changement du mode d'affichage
                $scope.$broadcast('event.change.mode', { isAffichageCi: $ctrl.isAffichageCi });
            };

            $ctrl.handleShowRapport = function () {
                $ctrl.isShowRapport = true;
                $scope.$broadcast('event.show.rapport.refresh', { date: $ctrl.selectedDate });
            };
        }

        /**
         * Initialisation des watchers qui vont détécté les siganux lancés par les sous composants
        */
        function initWatchers() {
            $scope.$on('event.change.ci', function ($event, selectedCi) {
                $event.stopPropagation();
                $scope.$broadcast('event.change.ci.ouvrier', selectedCi);
            });

            $scope.$on('event.change.affichage.entree', function ($event, selectedCi) {
                $event.stopPropagation();
                $ctrl.isShowRapport = false;
                ProgressBar.start();
                initData();
                ProgressBar.complete();
                $scope.$broadcast('event.change.mode', { isAffichageCi: $ctrl.isAffichageCi });
            });

            $scope.$on('event.change.select.by.ouvrier', function ($event, personnelId) {
                $event.stopPropagation();
                PointageHedboService.setSelectedOuvrierId(personnelId);
                $ctrl.isShowRapport = true;
                $scope.$broadcast('event.show.rapport.refresh', { date: $ctrl.selectedDate, isAffichageParOuvrier: true });
            });

            $scope.$on('event.change.save.state', function ($event, isSaveActif) {
                $event.stopPropagation();
                $scope.$broadcast('event.change.save.state', isSaveActif);
            });

            $scope.$on('event.change.validate.state', function ($event, isSaveActif) {
                $event.stopPropagation();
                $scope.$broadcast('event.change.validate.state', isSaveActif);
            });

            $scope.$on('event.change.totals', function ($event, obj) {
                $scope.$broadcast('event.change.totals.refresh', { item: obj });
            });
        }

        return $ctrl;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                  Actions                                                                      //
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /**
         * Gérer l'action du changement du calendrier et générer le label à afficher .
         */
        function actionCalendarChangeDate() {
            generateSelectedWeekLabel();
            initData();
        }

        /**
         * Charge le rapport lors du changement de semaine
         * @param {boolean} isAddWeek Si True on passe à la semaine suivante, sinon à la précédente.
         */
        function actionChangeWeek(isAddWeek) {
            if (isAddWeek) {
                $ctrl.selectedDate = moment($ctrl.selectedDate).add(1, 'week').toDate();
            } else {
                $ctrl.selectedDate = moment($ctrl.selectedDate).subtract(1, 'week').toDate();
            }

            generateSelectedWeekLabel();
            ProgressBar.start();
            initData();
            ProgressBar.complete();
        }

        /*
         * @function actionGenerateSelectedWeekLabel()
         * @description Générer libelle pour la semaine selectionné
         */
        function generateSelectedWeekLabel() {
            var momentDate = moment($ctrl.selectedDate);
            var mondayDayNumber = momentDate.startOf('isoWeek').format('DD');
            var sundayDayNumber = momentDate.isoWeekday(7).format('DD');
            var mondayMonth = momentDate.startOf('isoWeek').format('MM');
            var sundayMonth = momentDate.isoWeekday(7).format('MM');
            var mondayYear = momentDate.startOf('isoWeek').format('YYYY');
            var sundayYear = momentDate.isoWeekday(7).format('YYYY');

            var mondayMonthLabel = mondayMonth === sundayMonth ? '' : $ctrl.resources['Global_Month_' + mondayMonth];
            var sundayMonthLabel = $ctrl.resources['Global_Month_' + sundayMonth];
            var mondayYearLabel = mondayYear === sundayYear ? '' : mondayYear;

            $ctrl.selectedWeek = $ctrl.resources.Global_From + " " + mondayDayNumber + " " + mondayMonthLabel + " " + mondayYearLabel + " " +
                                 $ctrl.resources.Global_To + " " + sundayDayNumber + " " + sundayMonthLabel + " " + sundayYear;
            $ctrl.selectedWeek = $ctrl.selectedWeek[0].toUpperCase() + $ctrl.selectedWeek.slice(1).toLowerCase();
        }

        /*
         * @function actionGetRapportHebdoSummaryForUser()
         * @description Rafraichissement des données : liste des ouvriers et listes des Cis
         * @description La liste des ouvriers pour un affichage par ouvrier doit étre groupé .
         */
        function actionGetRapportHebdoSummaryForUser() {
            $ctrl.busy = true;
            ProgressBar.start();
            var mondayDate = moment($ctrl.selectedDate).startOf('isoWeek').format("YYYY-MM-DD");
            PointageHedboService.GetUserPointageHebdoSummary({ personnelStatut: $ctrl.personnelStatut, mondayDate: mondayDate })
                .$promise
                .then(function (response) {
                    if (response) {
                        $ctrl.ouvrierList = response.PersonnelSummaryList;
                        $ctrl.ouvrierListGrouped = getGroupOuvrierListByOuvrierId(response.PersonnelSummaryList);
                        $ctrl.ouvrierListByMode = [];
                        cloneAll($ctrl.ouvrierListGrouped, $ctrl.ouvrierListByMode);
                        $ctrl.ciList = response.CiPointageSummaryList;
                    }
                })
                .catch(function (error) {
                    Notify.error(resources.Global_Notification_Error);
                })
                .finally(function () {
                    $ctrl.busy = false;
                    ProgressBar.complete();
                });
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                  General                                                                       //
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /**
         * Dans le cas d'un affichage par ouvrier il faudra bien faire un group by utilisant le PersonnelId . 
         * Car un ouvrier peut éventuellement étre afféctés à dans deux Cis différents
         * @param {any} ouvrierList list a grouper
         * @returns {any} Liste des ouvriers
        */
        function getGroupOuvrierListByOuvrierId(ouvrierList) {
            var result = [];
            ouvrierList.reduce(function (res, value) {
                if (!res[value.PersonnelId]) {
                    res[value.PersonnelId] = {
                        PersonnelId: value.PersonnelId,
                        SocieteCode: value.SocieteCode,
                        Matricule: value.Matricule,
                        Nom: value.Nom,
                        Prenom: value.Prenom,
                        IsInFavouriteTeam: value.IsInFavouriteTeam,
                        TotalHeure: 0,
                        TotalHeureAbsence: 0,
                        TotalHeureSup: 0,
                        CiId: value.CiId,
                        CiCode: value.CiCode,
                        Selected: value.Selected
                    };
                    result.push(res[value.PersonnelId]);
                }
                res[value.PersonnelId].TotalHeure += getValueOrDefaultNumber(value.TotalHeure);
                res[value.PersonnelId].TotalHeureAbsence += getValueOrDefaultNumber(value.TotalHeureAbsence);
                res[value.PersonnelId].TotalHeureSup += getValueOrDefaultNumber(value.TotalHeureSup);
                return res;
            }, {});
            return result;
        }

        /*
         * Dans le cas d'un affichage par ouvrier il faudra bien faire un group by utilisant le PersonnelId .
        */
        function getValueOrDefaultNumber(value) {
            return isNumeric(value) ? value : 0;
        }

        /* Check if a value is a numeric value 
        */
        function isNumeric(n) {
            return !isNaN(parseFloat(n)) && isFinite(n);
        }

        /*
         * @function cloneAll(source,target)
         * @description Recopie complète d'un objet
         * @param {any} source 
         * @param {any} target
         */
        function cloneAll(source, target) {
            for (var property in source) {
                if (source.hasOwnProperty(property)) {
                    target[property] = angular.copy(source[property]);
                }
            }
        }
    }
}(angular));