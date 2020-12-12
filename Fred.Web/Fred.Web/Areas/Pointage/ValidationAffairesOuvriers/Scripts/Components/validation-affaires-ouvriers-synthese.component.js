(function () {
    'use strict';

    var validationAffairesOuvriersSyntheseComponent = {
        templateUrl: '/Areas/Pointage/ValidationAffairesOuvriers/Scripts/Components/validation-affaires-ouvriers-synthese.componenet.html',
        bindings: {
            resources: '<'
        },
        controller: ValidationAffairesOuvriersSyntheseController,
        controllerAs: '$ctrl'
    };

    angular.module('Fred').component('validationAffairesOuvriersSyntheseComponent', validationAffairesOuvriersSyntheseComponent);
    angular.module('Fred').controller('ValidationAffairesOuvriersSyntheseController', ValidationAffairesOuvriersSyntheseController);

    ValidationAffairesOuvriersSyntheseController.$inject = ['$scope', '$q', 'PointageHedboService', 'Notify', 'ProgressBar'];

    function ValidationAffairesOuvriersSyntheseController($scope, $q, PointageHedboService, Notify, ProgressBar) {

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                  Initialisation                                                                //
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        var $ctrl = this;
        $ctrl.resources = resources;
        $ctrl.listCi = [];
        $ctrl.IsAllCiSelected = false;
        $ctrl.dateDebut;
        $ctrl.rapportHebdoEntreeList = [];
        $ctrl.isShowRapport = false;
        $ctrl.selectedDate = new Date();
        initWatchers();
        initData();

        function initWatchers() {
            $scope.$on('show.change.affichage.entree.validation.affaire', function ($event) {
                initData();
            });
        }

        /**
         * Initialisation des listes des ouvriers.
         */
        function initData() {
            if ($ctrl.busy) {
                return;
            }

            $q.when().then(generateSelectedWeekLabel)
                .then(GetValidationAffairesbyResponsable);
        }

        /*
         * @function GetValidationAffairesbyResponsable()
         * @description Rafraichissement des données : liste des Cis
         */
        function GetValidationAffairesbyResponsable() {
            $ctrl.IsAllCiSelected = false;
            $ctrl.busy = true;
            ProgressBar.start();
            $ctrl.dateDebut = moment($ctrl.selectedDate).startOf('isoWeek').format("YYYY-MM-DD");
            PointageHedboService.GetValidationAffairesbyResponsableAsync({ dateDebut: $ctrl.dateDebut })
                .$promise
                .then(function (response) {
                    if (response) {
                        $ctrl.listCi = response;
                    }
                })
                .catch(function (error) {
                    Notify.error(resources.Global_Notification_Error);
                    console.log(error);
                })
                .finally(function () {
                    $ctrl.busy = false;
                    ProgressBar.complete();
                });
        }

        $ctrl.ValidateAndGetAffairesbyResponsable = function () {
            var ouvriersList = [];
            var ciList = [];
            if ($ctrl.listCi) {
                angular.forEach($ctrl.listCi, function (ci) {
                    if (ci.IsSelected) {
                        ciList.push(ci.CiId);
                        ouvriersList.push.apply(ouvriersList, ci.AffectedPersonnelsIds);
                    }
                });
                var model = { DateDebut: $ctrl.dateDebut, PersonnelIdsList: ouvriersList, CiIdsList: ciList };
                if (ouvriersList && ouvriersList.length > 0 && ciList && ciList.length > 0) {
                    PointageHedboService.ValidateAffairesbyResponsableAsync(model)
                        .$promise
                        .then(initData)
                        .catch(function (error) {
                            Notify.error(resources.Global_Notification_Error);
                            console.log(error);
                        })
                        .finally(function () {
                            $ctrl.busy = false;
                            ProgressBar.complete();
                        });
                }
            }
        };

        $ctrl.handleSelectAllCi = function (isAllCiSelected) {
            angular.forEach($ctrl.listCi, function (val) {
                val.IsSelected = isAllCiSelected;
            });
        };

        $ctrl.ViewRapportHebdoForCi = function (ciAndOuvriersList) {
            if (ciAndOuvriersList) {
                $ctrl.rapportHebdoEntreeList = [];
                var rapportHebdoEntree = {
                    CiId: ciAndOuvriersList.CiId,
                    OuvrierListId: ciAndOuvriersList.AffectedPersonnelsIds
                };
                $ctrl.rapportHebdoEntreeList.push(rapportHebdoEntree);
                $scope.$emit('event.show.rapport.hebdo.ci.forResponsableValidation', { rapportHebdoEntreeList: $ctrl.rapportHebdoEntreeList, date: $ctrl.dateDebut});
            }
        };

        $ctrl.ViewRapportHebdoForAllCi = function () {
            $ctrl.rapportHebdoEntreeList = [];
            if ($ctrl.listCi) {
                angular.forEach($ctrl.listCi, function (ci) {
                    if (ci.IsSelected && ci.AffectedPersonnelsIds && ci.AffectedPersonnelsIds.length > 0) {
                        var rapportHebdoEntree = {
                            CiId: ci.CiId,
                            OuvrierListId: ci.AffectedPersonnelsIds
                        };
                        $ctrl.rapportHebdoEntreeList.push(rapportHebdoEntree);
                    }
                });

                if ($ctrl.rapportHebdoEntreeList && $ctrl.rapportHebdoEntreeList.length > 0) {
                    $scope.$emit('event.show.rapport.hebdo.ci.forResponsableValidation', { rapportHebdoEntreeList: $ctrl.rapportHebdoEntreeList, date: $ctrl.dateDebut});
                }
            }
        };


        $ctrl.handleChangeWeek = function (isAddWeek) {
            if (isAddWeek) {
                $ctrl.selectedDate = moment($ctrl.selectedDate).add(1, 'week').toDate();
            } else {
                $ctrl.selectedDate = moment($ctrl.selectedDate).subtract(1, 'week').toDate();
            }

            initData();
        };

        $ctrl.handleChangeCalendarDate = function () {
            initData();
        };

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
    }
})();