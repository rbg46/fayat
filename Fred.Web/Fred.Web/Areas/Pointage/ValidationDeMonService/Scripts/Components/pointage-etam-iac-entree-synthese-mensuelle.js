(function () {
    'use strict';

    var pointageEtamIacEntreeSyntheseMensuelleComponent = {
        templateUrl: '/Areas/Pointage/ValidationDeMonService/Scripts/Components/pointage-etam-iac-entree-synthese-mensuelle.html',
        bindings: {
            resources: '<'
        },
        controller: PointageEtamIacEntreeSyntheseMensuelleController,
        controllerAs: '$ctrl'
    };

    angular.module('Fred').component('pointageEtamIacEntreeSyntheseMensuelleComponent', pointageEtamIacEntreeSyntheseMensuelleComponent);

    angular.module('Fred').controller('PointageEtamIacEntreeSyntheseMensuelleController', PointageEtamIacEntreeSyntheseMensuelleController);

    PointageEtamIacEntreeSyntheseMensuelleController.$inject = ['$scope', '$q', 'PointageHedboService', 'Notify', 'ProgressBar', 'confirmDialog', 'UserService'];

    function PointageEtamIacEntreeSyntheseMensuelleController($scope, $q, PointageHedboService, Notify, ProgressBar, confirmDialog, UserService) {

        var $ctrl = this;
        $ctrl.resources = resources;

        UserService.getCurrentUser().then(function(user) {
            $ctrl.userId = user.Personnel.PersonnelId;
            $ctrl.personnelId = $ctrl.userId;
        });

        $ctrl.syntheseList = [];
        $ctrl.selectedDate = new Date();
        actionGetSyntheseData($ctrl.selectedDate);
        $ctrl.actionGetSyntheseData = actionGetSyntheseData;
        $ctrl.IsSelectAllPersonnel = false;
        $ctrl.selectedPersonnelIdList = [];
        $ctrl.validationWarningList = [];
        $ctrl.isValidToValidate = true;
        $ctrl.validationBlokingErrorList = [];
        $ctrl.Niveau = 1;
        $ctrl.handleSelectNiveau = handleSelectNiveau;
        $ctrl.unlockSelectPersonnel = false;
        $ctrl.getPeriode = getPeriode;
        $ctrl.handleChangePersonnel = handleChangePersonnel;
        $ctrl.handleDeletePersonnel = handleDeletePersonnel;
        initWatchers();
        $ctrl.PersonnelsFilter = "";
        $ctrl.ValueText = "";
        $ctrl.TypePersonnel =
            [
                { Libelle: "Ouvrier", Value: "1" },
                { Libelle: "ETAM", Value: "2" },
                { Libelle: "IAC", Value: "3" }
            ];

        $ctrl.PersonnelStatutValueList =
        {
            All: "",
            Ouvrier: "1",
            ETAM: "2",
            IAC: "3",
            ETAMBureau: "4",
            ETAMArticle36: "5"
        };


        //====================================================================================
        //==== Init methods
        //====================================================================================

        /*
         * @function inti watchers
         * @description intialisation des watchers
         */
        function initWatchers() {
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

            $scope.$on('event.validation.mensuelle.personnel.selected.entree.refresh', function () {
                actionGetSyntheseData($ctrl.selectedDate);
            });
        }

        /*
         * @function inti synthese data
         * @description intialisation des watchers
         */
        function actionGetSyntheseData(currentDate) {
            $ctrl.validationWarningList = [];
            $ctrl.validationBlokingErrorList = [];
            $ctrl.IsSelectAllPersonnel = false;
            $ctrl.selectedMonth = moment(currentDate).format("MM/YYYY");
            if ($ctrl.busy) {
                return;
            }

            $ctrl.busy = true;
            ProgressBar.start();
            disabledPersonnelFilter();
            $q.when().then(actionGetSyntheseMensuelle);
        }

        //====================================================================================
        //==== Actions
        //===================================================================================

        /*
         * @function actionGetSyntheseMensuelle()
         * @description Rafraichissement des données : synthese mensuelle pour l'utilisateur en cours
         */
        function actionGetSyntheseMensuelle() {
            var firstDateOfMonth = actionGetFirstDateOfMonth();

            PointageHedboService.GetSyntheseMensuelleEtamIac({ utilisateurId: $ctrl.personnelId, monthDate: firstDateOfMonth })
                .$promise
                .then(function (data) {
                    if (data) {
                        $ctrl.syntheseList = data;
                        $ctrl.syntheseListInitial = data;
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

        /*
         * @function actionGetFirstDateOfMonth()
         * @description Retourner la date du premier jour du mois séléctionné
         */
        function actionGetFirstDateOfMonth() {
            if ($ctrl.selectedDate) {
                var year = $ctrl.selectedDate.getFullYear();
                var dateFirstDayOfWeek = moment(new Date(year, moment($ctrl.selectedDate).month(), 1));
                return dateFirstDayOfWeek.format('YYYY-MM-DD');
            }
        }

        /*
         * @function is working week day
         * @description Vérifier si un jour est un jour ouvré (entre lundi et vendredi)
         */
        function isWorkingWeekday(year, month, d) {
            var momentDate = moment(new Date(year, month, d));
            return momentDate.isoWeekday() !== 6 && momentDate.isoWeekday() !== 7;
        }

        /*
         * @function Get working days in a month
         * @description Nombre de jours ouvrés dans un mois donné
         */
        function getWorkingDaysInMonth(month, year) {
            var days = daysInMonth(month + 1, year);
            var weekdays = 0;
            for (var i = 0; i < days; i++) {
                if (isWorkingWeekday(year, month, i + 1)) {
                    weekdays++;
                }
            }
            return weekdays;
        }

        /*
         * @function Get a list of days in a monthy
         * @description Retourne les jours du mois dans l'année 
         */
        function daysInMonth(month, year) {
            return new Date(year, month, 0).getDate();
        }

        /*
         * @function action get selected personnel
         * @description Retourne la liste des personnels séléctionnés dans la liste
         */
        function actionGetSelectedPersonnel() {
            if (!$ctrl.syntheseList) {
                return [];
            }
            return $ctrl.syntheseList.filter(function (a) { return a.Selected; });
        }

        /*
         * @function action qui s'applique pour valider le rapport
         * @description valider la synthése mensuelle
         */
        function actionValidateSynthese() {
            $q.when()
                .then(ProgressBar.start)
                .then(function () {
                    var personnelListSelected = actionGetSelectedPersonnel();
                    var selectedPersonnelList = personnelListSelected.map(function (m) { return m.Personnel.PersonnelId; });
                    var model = { PersonnelIdList: selectedPersonnelList, FirstDayInMonth: actionGetFirstDateOfMonth() };
                    PointageHedboService.ValiderSyntheseMensuelleEtamIac(model)
                        .$promise
                        .then(function () {
                            actionGetSyntheseData($ctrl.selectedDate);
                            Notify.message($ctrl.resources.Rapport_Hebdo_Validation_OK);
                        })
                        .catch(function (error) {
                            Notify.error(resources.Global_Notification_Error);
                        })
                        .finally(function () {
                            ProgressBar.complete();
                        });
                });
        }

        /*
         * @function Action create ul html element
         * @description Action create ul html element
         */
        function actionCreateUlHtmlElement(elements) {
            let ulEl = '';
            if (elements && elements.length > 0) {
                for (var i = 0; i < elements.length; i++) {
                    ulEl = ulEl + "<li>" + elements[i] + "</li>";
                }
            }
            return ulEl;
        }

        //====================================================================================
        //==== Handlers
        //===================================================================================

        /*
         * @function handlePersonnelSelection
         * @description handle la séléction d'un personnel
         */
        $ctrl.handlePersonnelSelection = function (personnel) {
            $scope.$emit('event.validation.mensuelle.personnel.selected.entree', { Personnel: personnel, FirstMonthDate: actionGetFirstDateOfMonth() });
        };

        /*
         * @function handleChangeCalendarDate()
         * @description Gère l'évènement de la modification de la date du calendrier
         */
        $ctrl.handleChangeCalendarDate = function () {
            handleChangeSelection();
        };

        /*
        * @function handleChangeCalendarDate()
        * @description Gère l'évènement de la modification de la date du calendrier
        */
        $ctrl.handleChangeMonth = function (isAddWeek) {
            handleChangeSelection(true, isAddWeek);
        };

        /*
         * @function handleChangeCalendarDate()
         * @description Gère l'évènement de la modification de la date du calendrier
         */
        function handleChangeSelection(isMonthChange, isAddMonth) {
            if (isMonthChange) {
                if (isAddMonth) {
                    let date = moment($ctrl.selectedDate).add(1, 'month').toDate();
                    actionGetSyntheseData(date);
                    $ctrl.selectedDate = date;
                } else {
                    let date = moment($ctrl.selectedDate).subtract(1, 'month').toDate();
                    actionGetSyntheseData(date);
                    $ctrl.selectedDate = date;
                }
            } else {
                actionGetSyntheseData($ctrl.selectedDate);
            }
        }

        /*
         * @function select all personnel
         * @description Gère l'évènement de la séléction de tous les personnels
         */
        $ctrl.handleSelectAllPersonnel = function (isSelectAll) {
            angular.forEach($ctrl.syntheseList, function (val) {
                val.Selected = isSelectAll;
            });
            var selectedPersonnelList = actionGetSelectedPersonnel();
            if (!selectedPersonnelList || selectedPersonnelList.length === 0) {
                $ctrl.validationWarningList = [];
                $ctrl.validationBlokingErrorList = [];
            }
        };

        /*
         * @function Handle validate
         * @description Gère l'évènement de la validation
         */
        $ctrl.handleValidate = function () {
            $ctrl.validationWarningList = [];
            $ctrl.validationBlokingErrorList = [];
            var selectedPersonnelList = actionGetSelectedPersonnel();
            if (!selectedPersonnelList || selectedPersonnelList.length === 0) {
                Notify.warning($ctrl.resources.RapportHebdo_Warning_Empty_Personnel);
                return;
            }

            var momentDate = moment($ctrl.selectedDate);
            var workingDaysInMonth = getWorkingDaysInMonth(momentDate.month(), momentDate.year());

            angular.forEach(selectedPersonnelList, function (person) {
                var errorMessage = $ctrl.resources.Rapport_Hebdo_Incomplet + ' ';
                var isWarning = person.NbreJoursPointes + person.NbreAbsences < workingDaysInMonth;
                var isBlockingError = person.NbreJoursPointes === 0 && person.NbreAbsences === 0;

                if (isBlockingError) {
                    $ctrl.validationBlokingErrorList.push(String.format($ctrl.resources.RapportHebdo_ErrorMessage_Personnel_HasNoPointage, person.Personnel.Prenom, person.Personnel.Nom));
                }

                if (isWarning) {
                    $ctrl.validationWarningList.push(errorMessage + person.Personnel.Prenom + '  ' + person.Personnel.Nom + ', confirmez-vous la validation ?');
                }
            });

            if ($ctrl.validationBlokingErrorList && $ctrl.validationBlokingErrorList.length > 0) {
                Notify.error($ctrl.resources.RapportHebdo_Validation_Error_Personnel_Without_Pointage);
            }
            else if ($ctrl.validationWarningList && $ctrl.validationWarningList.length > 0) {
                confirmDialog.confirm($ctrl.resources, actionCreateUlHtmlElement($ctrl.validationWarningList))
                    .then(function () {
                        actionValidateSynthese();
                    });
            }
            else {
                actionValidateSynthese();
            }
        };

        /*
         * @function Handle validate
         * @description Gère l'évènement de la validation
         */
        $ctrl.toggelePersonneSelection = function () {
            var selectedPersonnelList = actionGetSelectedPersonnel();
            if (!selectedPersonnelList || selectedPersonnelList.length === 0) {
                $ctrl.validationWarningList = [];
                $ctrl.validationBlokingErrorList = [];
            }
        };

        function handleChangePersonnel() {
            $ctrl.personnelId = $ctrl.personnel.PersonnelId;
        }

        function handleSelectNiveau() {
            // Mon Service
            if (JSON.parse($ctrl.Niveau) === 1) {
                $ctrl.unlockSelectPersonnel = false;
                $ctrl.personnelId = $ctrl.userId;
            }
            // Service de ...
            if (JSON.parse($ctrl.Niveau) === 2) {
                $ctrl.unlockSelectPersonnel = true;
            }
        }

        function disabledPersonnelFilter() {
            $ctrl.PersonnelsFilter = "";
        }

        function getPeriode() {
            if ($ctrl.selectedDate) {
                return moment($ctrl.selectedDate).format("YYYYMM");
            }
            else {
                return null;
            }
        }

        function handleDeletePersonnel() {
            $ctrl.personnel = null;
        }

        function filterPersonnelByStatut(statut, listPersonnel) {
            if (listPersonnel) {
                if (statut === $ctrl.PersonnelStatutValueList.All) {
                    $ctrl.syntheseList = listPersonnel;
                }
                else {
                    $ctrl.syntheseList = listPersonnel.filter(function (el) {
                        if (statut === $ctrl.PersonnelStatutValueList.ETAM) {
                            return el.PersonnelStatut === $ctrl.PersonnelStatutValueList.ETAM ||
                                el.PersonnelStatut === $ctrl.PersonnelStatutValueList.ETAMBureau ||
                                el.PersonnelStatut === $ctrl.PersonnelStatutValueList.ETAMArticle36;
                        }
                        if (statut === $ctrl.PersonnelStatutValueList.IAC) {
                            return el.PersonnelStatut === $ctrl.PersonnelStatutValueList.IAC;
                        }
                        if (statut === $ctrl.PersonnelStatutValueList.Ouvrier) {
                            return el.PersonnelStatut === $ctrl.PersonnelStatutValueList.Ouvrier;
                        }
                    });
                }
            }
        }

        $ctrl.filterPersonnelParNomPrenomStatut = function (nomPrenom, statut) {
            if ($ctrl.syntheseListInitial) {
                if (nomPrenom === undefined || nomPrenom.length === 0 || !nomPrenom.trim()) {
                    filterPersonnelByStatut(statut, $ctrl.syntheseListInitial);
                }
                else {
                    $ctrl.syntheseList = $ctrl.syntheseListInitial.filter(function (el) {
                        return el.Personnel.Prenom.includes(nomPrenom.toUpperCase()) || el.Personnel.Nom.includes(nomPrenom.toUpperCase());
                    });

                    filterPersonnelByStatut(statut, $ctrl.syntheseList);
                }
            }
        };
    }
})();