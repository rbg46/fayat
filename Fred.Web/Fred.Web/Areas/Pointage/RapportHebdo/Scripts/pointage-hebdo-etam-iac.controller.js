(function (angular) {
    'use strict';

    angular.module('Fred').controller('PointageHebdoEtamIacController', PointageHebdoEtamIacController);

    PointageHebdoEtamIacController.$inject = ['$scope', '$q', 'PointageHedboService', 'UtilisateurService', 'Notify', 'ProgressBar', '$timeout'];

    function PointageHebdoEtamIacController($scope, $q, PointageHedboService, UtilisateurService, Notify, ProgressBar, $timeout) {

        var $ctrl = this;
        $ctrl.hasChanges = false;
        $ctrl.busy = false;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                  Initialisation                                                                //
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        init();

        /**
         * Initialisation du controller.
         * 
         */
        function init() {
            $ctrl.resources = resources;
            $ctrl.selectedDate = new Date();
            $ctrl.isShowRapport = false;
            $ctrl.isUserEtamOrIac = false;

            initWatchers();
            initData();
        }

        /**
         * Initialisation des listes des ouvriers.
         */
        function initData() {
            if ($ctrl.busy) {
                return;
            }

            $ctrl.busy = true;
            ProgressBar.start();
            $q.when().then(actionGetRapportHebdoSummaryForUser);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                  Handlers                                                                      //
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        $ctrl.handleChangeWeek = function (isAddWeek) {
            actionChangeWeek(isAddWeek);
        };

        $ctrl.handleShowRapport = function () {
            $ctrl.isShowRapport = true;
            $scope.$broadcast('event.show.rapport.refresh', { date: $ctrl.selectedDate });
        };


        /*
         * Initialisation des watchers qui vont détécté les siganux lancés par les sous composants
        */
        function initWatchers() {
            $scope.$on('event.change.ci', function ($event, selectedCi) {
                $event.stopPropagation();
                $scope.$broadcast('event.change.ci.ouvrier', selectedCi);
            });

            $scope.$on('event.change.select.by.ouvrier', function ($event, data) {
                $event.stopPropagation();
                PointageHedboService.setSelectedOuvrierId(data.personnelId);
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
         * Ajouter une semaine à la semaine courante .
         * @param {boolean} isAddWeek Booléan indique s'il faut ajouter une semaine
         */
        function actionChangeWeek(isAddWeek) {
            if (isAddWeek) {
                $ctrl.selectedDate = moment($ctrl.selectedDate).add(1, 'week').toDate();
            } else {
                $ctrl.selectedDate = moment($ctrl.selectedDate).subtract(1, 'week').toDate();
            }

            generateSelectedWeekLabel();
            actionGetRapportHebdoSummaryForUser();
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
        }

        /*
         * @function actionGetRapportHebdoSummaryForUser()
         * @description Rafraichissement des données : liste des ouvriers et listes des Cis
         * @description La liste des ouvriers pour un affichage par ouvrier doit étre groupé .
         */
        function actionGetRapportHebdoSummaryForUser() {
            UtilisateurService.GetCurrentUser().$promise
                .then(function (utilisateur) {
                    if (utilisateur) {
                        if (utilisateur.Personnel.Statut === "3" || utilisateur.Personnel.Statut === "2") {
                            $ctrl.isUserEtamOrIac = true;
                            PointageHedboService.setSelectedOuvrierId(utilisateur.UtilisateurId);
                            $ctrl.isShowRapport = true;
                            $timeout(function () {
                                $scope.$broadcast('event.show.rapport.refresh', { date: $ctrl.selectedDate, isAffichageParOuvrier: true });
                            }, 0, false);
                        }
                        $ctrl.busy = false;
                    }
                })
                .catch(function (error) {
                    Notify.error($ctrl.resources.Global_Notification_Error);
                })
                .finally(function () {
                    ProgressBar.complete();
                });
        }
    }
}(angular));