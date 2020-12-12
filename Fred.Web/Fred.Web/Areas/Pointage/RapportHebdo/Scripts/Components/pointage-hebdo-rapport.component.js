(function () {
    'use strict';

    var pointageHebdoRapportComponent = {
        templateUrl: '/Areas/Pointage/RapportHebdo/Scripts/Components/pointage-hebdo-rapport.component.html',
        bindings: {
            resources: '<',
            isEtamIac: '<',
            isManagerPointing: '<',
            personnelToShowIsIac: '<'
        },
        controller: PointageHebdoRapportController,
        controllerAs: '$ctrl'
    };

    angular.module('Fred').component('pointageHebdoRapportComponent', pointageHebdoRapportComponent);

    angular.module('Fred').controller('PointageHebdoRapportController', PointageHebdoRapportController);

    PointageHebdoRapportController.$inject = ['$scope', 'PointageHedboService', '$q', 'ProgressBar', 'Notify', 'confirmDialog', 'UserService'];

    function PointageHebdoRapportController($scope, PointageHedboService, $q, ProgressBar, Notify, confirmDialog, UserService) {
        var $ctrl = this;
        generateSelectedWeekLabel();
        $ctrl.tabItems =
            [{ Id: 1, Libelle: "Pointage", cssTag: "1 onglet" },
            { Libelle: "", cssTag: "sep" },
            { Id: 2, Libelle: "Majorations", cssTag: "2 onglet" },
            { Libelle: "", cssTag: "sep" },
            { Id: 3, Libelle: "Primes", cssTag: "3 onglet" },
            { Libelle: "", cssTag: "sep" },
            { Id: 4, Libelle: "Astreintes", cssTag: "4 onglet" }];

        $ctrl.selectedTabItem = $ctrl.tabItems[0];
        $ctrl.isAffichageParOuvrier = false;
        $ctrl.mondayDate;
        $ctrl.rapportHebdoMajorationList = [];
        $ctrl.isAbsenceColumnHidden = false;
        $ctrl.isIac = false;
        $ctrl.groupeId;
        $ctrl.isValidatedByManager = false;
        $ctrl.isValidAstreinteToSave = true;
        $ctrl.isValidToSave = true;
        $ctrl.disableSaveWhenLoad = false;
        $ctrl.isValidToValidate = true;
        $ctrl.isForValidationAffaires = false;
        $ctrl.isForValidationService = false;

        UserService.getCurrentUser().then(function(user) {
            $scope.currentUser = user.Personnel;
        });

        $ctrl.TotalHours = 0;
        $ctrl.TotalHoursSup = 0;
        // Watcher pour détécter le rafraichissement si les dates changent.
        $scope.$on('event.show.rapport.refresh', function (evt, data) {
            eventRefreshEcranRapport(data);
        });

        $scope.$on('show.rapport.hebdo.ci.forValidationService', function (evt, data) {
            $ctrl.isForValidationService = true;
            eventRefreshEcranRapport(data);
        });

        $scope.$on('event.refresh.visible.commentaire', function (event, data) {
            $scope.$broadcast('event.refresh.visible.commentaire.panel.pointage', data);
        });

        function eventRefreshEcranRapport(data) {
            $ctrl.selectedDate = data.date;
            $ctrl.isAffichageParOuvrier = data.isAffichageParOuvrier;
            $ctrl.counterDate = 0;
            refresh();
        }

        function fillRedirectionUrl() {
            if ($ctrl.isForValidationService) {
                return '/Pointage/Rapport/PointageEtamIacSyntheseMensuelle';
            } else {
                return '/Pointage/Rapport/ValidationAffairesOuvriers';
            }
        }

        // Rafraichissement du contenu de la page
        function refresh() {
            generateWeekDateAndLabel();
            actionLoadData();
        }

        function generateWeekDateAndLabel() {
            generateSelectedWeekLabel();
            var startDate = moment($ctrl.selectedDate).startOf('isoWeek');
            $ctrl.mondayDate = startDate.format('YYYY-MM-DD');

            // Refresh des dates de semaines
            $ctrl.mondayFormattedDate = startDate.isoWeekday(1).format('DD-MM');
            $ctrl.tuesdayFormattedDate = startDate.isoWeekday(2, 'days').format('DD-MM');
            $ctrl.wednesdayFormattedDate = startDate.isoWeekday(3, 'days').format('DD-MM');
            $ctrl.thursdayFormattedDate = startDate.isoWeekday(4, 'days').format('DD-MM');
            $ctrl.fridaydayFormattedDate = startDate.isoWeekday(5, 'days').format('DD-MM');
            $ctrl.saturdayFormattedDate = startDate.isoWeekday(6, 'days').format('DD-MM');
            $ctrl.sundayFormattedDate = startDate.isoWeekday(7, 'days').format('DD-MM');
        }

        $scope.$on('event.change.save.state', function ($event, isSaveActif) {
            $event.stopPropagation();
            $ctrl.isValidToSave = isSaveActif;
        });

        $scope.$on('event.change.astreinte.save.state', function ($event, isSaveActif) {
            $event.stopPropagation();
            $ctrl.isValidAstreinteToSave = isSaveActif;
        });

        $scope.$on('event.change.validate.state', function ($event, isSaveActif) {
            $event.stopPropagation();
            $ctrl.isValidToValidate = isSaveActif;
        });

        $scope.$on('event.change.totals.refresh', function ($event, obj) {
            var total = obj.item.total;
            var supHour = obj.item.supHour;
            $ctrl.TotalHours = total;
            $ctrl.TotalHoursSup = supHour;
        });

        $scope.$on('event.change.totals', function ($event, obj) {
            $ctrl.TotalHours = obj.total;
            $ctrl.TotalHoursSup = obj.supHour;
        });


        function actionLoadData() {
            UserService.getCurrentUser().then(function(user) {
                $scope.currentUser = user.Personnel;
            });

            if ($ctrl.isAffichageParOuvrier) {
                actionGetDataAffichageParOuvrier($ctrl.mondayDate);
            } else {
                actionGetDataAffichageParCi($ctrl.mondayDate);
            }

            if ($ctrl.selectedTabItem.Id === 3) {
                refreshPrimePersonnelAffected();
            }
            if ($ctrl.selectedTabItem.Id === 2) {
                refreshPrimePersonnelAffected();
            }
        }

        function actionActualizeInputScreen() {
            $scope.$emit('event.rapport.hebdo.actualize.input.screen');
        }

        $scope.$on('show.rapport.hebdo.ci.forResponsableValidation', function (event, obj) {
            displayRapportHebdoOuvrierForSyntheseValidationAffaires(obj);
        });

        //////////////////////////////////////////////////////////////////
        // Handlers                                                     //
        //////////////////////////////////////////////////////////////////

        /*
         * Hanlde change tab
        */
        $ctrl.handleChangeTab = function (tab) {
            $ctrl.selectedTabItem = tab;

            // Afin de considérer les majorations dans les pointages il faudra rafaichir les totaux de l'onglet pointage
            if ($ctrl.selectedTabItem.Id === 1) {
                $scope.$broadcast('event.refresh.pointage.hebdo.task.panel', {
                    rapportTaskAbsencePanelData: $ctrl.rapportTaskAbsencePanelData,
                    rapportMajorationPanelData: $ctrl.rapportMajorationPanelData,
                    isAffichageParOuvrier: $ctrl.isAffichageParOuvrier
                });
            }
            if ($ctrl.selectedTabItem.Id === 2) {
                GetOrganisationIdbyPersonnelId($scope.currentUser.PersonnelId);
                refreshPrimePersonnelAffected();
            }
            if ($ctrl.selectedTabItem.Id === 3) {
                refreshPrimePersonnelAffected();
                GetOrganisationIdbyPersonnelId($scope.currentUser.PersonnelId);
            }

            if ($ctrl.selectedTabItem.Id === 4) {
                refreshAstreintes();
            }
        };

        /*
         * Hanlde change calendar week
        */
        $ctrl.handleChangeWeekRapport = function (isAddWeek) {
            $ctrl.TotalHours = 0;
            $ctrl.TotalHoursSup = 0;
            handleChangeDateNotification(true, isAddWeek);
        };

        /*
        * Hanlde pour réafficher les écrans d'entrée .
        */
        $ctrl.handleShowEcranEntree = function () {
            if ($ctrl.isForValidationAffaires) {
                $scope.$emit('event.change.affichage.entree.validation.affaire');
            }
            else {
                $scope.$emit('event.change.affichage.entree', $ctrl.selectedCi);
                $ctrl.validation = {};
            }
        };

        $ctrl.handleSave = function () {
            $q.when()
                .then(actionSaveLoadStart)
                .then(handleSaveMajoration)
                .then(handleSavePrime)
                .then(function () {
                    var mondayDate = moment($ctrl.selectedDate).isoWeekday(1).format('YYYY-MM-DD');
                    var model = {
                        mondayDate: mondayDate,
                        astreintePanelViewModel: $ctrl.rapportAstreintePanelData,
                        majorationPanelViewModel: $ctrl.rapportHebdoMajorationList,
                        PointagePanelViewModel: $ctrl.rapportTaskAbsencePanelData,
                        primePanelViewModel: $ctrl.rapportHebdoPrimeList
                    };

                    PointageHedboService.SaveRapportHebdo(model)
                        .$promise
                        .then(function (result) {
                            if (result.Errors.length > 0) {
                                angular.forEach(result.Errors, function (val, key) {
                                    Notify.error(val);
                                });
                            }
                            else {
                                actionLoadData();
                                actionActualizeInputScreen();
                                $ctrl.validation = {};
                                Notify.message(resources.Global_Notification_Enregistrement_Success);
                            }
                            angular.forEach(result.Warnings, function (val, key) {
                                Notify.warning(val);
                            });
                        })
                        .catch(function (error) {
                            Notify.error(resources.Global_Notification_Error);
                        })
                        .finally(actionSaveLoadEnd);
                });
        };

        $ctrl.handleValidate = function () {
            if ($ctrl.isForValidationAffaires) {
                handleValidateAffichageOuvrierForValidationAffaires();
            }
            else {
                handleValidateAffichageOuvrier();
            }
        };

        function handleValidateAffichageOuvrier() {
            $q.when()
                .then(ProgressBar.start)
                .then(handleSaveMajoration)
                .then(handleSavePrime)
                .then(function () {
                    var mondayDate = moment($ctrl.selectedDate).isoWeekday(1).format('YYYY-MM-DD');
                    var rapportHebdoSaveViewModel = {
                        mondayDate: mondayDate,
                        astreintePanelViewModel: $ctrl.rapportAstreintePanelData,
                        majorationPanelViewModel: $ctrl.rapportHebdoMajorationList,
                        PointagePanelViewModel: $ctrl.rapportTaskAbsencePanelData,
                        primePanelViewModel: $ctrl.rapportHebdoPrimeList
                    };

                    var isEtamIac = $ctrl.isEtamIac;

                    if ($ctrl.isEtamIac === undefined) {
                        isEtamIac = false;
                    }

                    PointageHedboService.CheckAndValidateRapportHebdo({ isEtamIac: isEtamIac }, rapportHebdoSaveViewModel)
                        .$promise
                        .then(function (value) {
                            angular.forEach(value.Warnings, function (val, key) {
                                Notify.warning(val);
                            });
                            if (value.Errors.length > 0) {
                                angular.forEach(value.Errors, function (val, key) {
                                    Notify.error(val);
                                });
                            }
                            else {
                                $ctrl.validation = value;
                                if ($ctrl.validation && $ctrl.validation.IsValidated === true) {
                                    Notify.message($ctrl.resources.RapportHebdo_SucessValidationMessage);
                                }
                                else {
                                    Notify.error($ctrl.resources.RapportHebdo_FailureValidationMessage);
                                }

                                if ($ctrl.validation && angular.equals({}, $ctrl.validation.PersonnelErrorList)) {
                                    $ctrl.validation.PersonnelErrorList = undefined;
                                }
                                if ($ctrl.validation && angular.equals({}, $ctrl.validation.PersonnelWarningList)) {
                                    $ctrl.validation.PersonnelWarningList = undefined;
                                }
                                if ($ctrl.validation && angular.equals({}, $ctrl.validation.DailyRapportErrorList)) {
                                    $ctrl.validation.DailyRapportErrorList = undefined;
                                }

                                actionLoadData();
                            }
                        })
                        .catch(function (error) {
                            $ctrl.validation = {};
                            console.log(error);
                            Notify.error(resources.Global_Notification_Error);
                        })
                        .finally(ProgressBar.complete);
                });
        }

        function handleValidateAffichageOuvrierForValidationAffaires() {
            var ciListOuvriersList = PointageHedboService.getSelectedEntree();
            if (ciListOuvriersList && ciListOuvriersList.length > 0) {
                var ouvriersList = [];
                var ciList = [];
                angular.forEach(ciListOuvriersList, function (obj) {
                    ciList.push(obj.CiId);
                    ouvriersList.push.apply(ouvriersList, obj.OuvrierListId);
                });
                if (ouvriersList && ouvriersList.length > 0 && ciList && ciList.length > 0) {
                    var model = { DateDebut: $ctrl.mondayDate, PersonnelIdsList: ouvriersList, CiIdsList: ciList };
                    actionSaveLoadStart();
                    PointageHedboService.ValidateAffairesbyResponsableAsync(model)
                        .$promise
                        .then(actionLoadData)
                        .catch(function (error) {
                            Notify.error(resources.Global_Notification_Error);
                            console.log(error);
                        })
                        .finally(function () {
                            $ctrl.busy = false;
                            actionSaveLoadEnd();
                        });
                }
            }
        }

        $ctrl.handleCancel = function () {
            confirmDialog.confirm($ctrl.resources, $ctrl.resources.Global_Modal_ConfirmationAnnulation)
                .then(function () {
                    actionLoadData();
                });
        };

        function GetPointageByPersonnelIDAndIntervalParCi(mondayDate) {
            $ctrl.weekPointageStatutCode = null;
            $ctrl.weekPointageStatutLibelle = null;
            var model = { Mondaydate: mondayDate, PersonnelIds: $ctrl.personnelIdList, isForWeek: false };
            return PointageHedboService.GetPointageByPersonnelIDAndInterval(model)
                .$promise
                .then(function (value) {
                    $ctrl.listPointagePersonnelForWeekParCi = value;
                    if ($ctrl.listPointagePersonnelForWeekParCi && $ctrl.listPointagePersonnelForWeekParCi.length === 1) {
                        $ctrl.weekPointageStatutCode = $ctrl.listPointagePersonnelForWeekParCi[0].PointageStatutCode;
                        $ctrl.weekPointageStatutLibelle = $ctrl.listPointagePersonnelForWeekParCi[0].PointageStatutLibelle;
                    }
                });
        }

        //////////////////////////////////////////////////////////////////
        // Actions                                                     //
        //////////////////////////////////////////////////////////////////

        /**
         * Générer le label de la semaine à afficher . Même si la date est undefined (ca sera considérée date du jour courant)
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

        function getSpecifiquePanelData(rapportData, nodeTyleLibelleToKeep) {
            var panelData = [];
            cloneAll(rapportData, panelData);
            angular.forEach(panelData, function (firstLevelElement) {
                angular.forEach(firstLevelElement.SubNodeList, function (secondeLevelElement) {
                    var panelDataSubNodes = [];
                    angular.forEach(secondeLevelElement.SubNodeList, function (thirdLevelEelemnt) {
                        if (thirdLevelEelemnt.NodeTypeLibelle === nodeTyleLibelleToKeep)
                            panelDataSubNodes.push(thirdLevelEelemnt);
                    });
                    secondeLevelElement.SubNodeList = panelDataSubNodes;
                });
            });
            return panelData;
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

        /**
        * Action get data en mode affichage par Ci.
        * @param {Date} mondayDate Date du lundi
        */
        function actionGetDataAffichageParCi(mondayDate) {
            var model = { Mondaydate: mondayDate, RapportHebdoEntreeList: PointageHedboService.getSelectedEntree() };
            getPersonnelIdList(PointageHedboService.getSelectedEntree());
            if (model) {
                GetRapportHebdoByCi(model);
            }
        }

        function GetRapportHebdoByCi(model) {
            ProgressBar.start();
            PointageHedboService.GetRapportHebdoByCi(model)
                .$promise
                .then(function (value) {
                    $ctrl.rapportData = value;
                    getDisplayByCiPointageStatut($ctrl.rapportData);
                    $ctrl.rapportAstreintePanelData = getSpecifiquePanelData($ctrl.rapportData, 'Astreinte').filter(r => r.NodeTypeLibelle === "Affaire");
                    $scope.$broadcast('event.refresh.pointage.hebdo.astreinte.panel', { rapportAstreintePanelData: $ctrl.rapportAstreintePanelData, isAffichageParOuvrier: false });

                    $ctrl.rapportMajorationPanelData = getSpecifiquePanelData($ctrl.rapportData, 'Majoration');
                    $scope.$broadcast('event.refresh.pointage.hebdo.majoration.panel', { rapportMajorationPanelData: $ctrl.rapportMajorationPanelData, isAffichageParOuvrier: false });

                    $ctrl.rapportTaskAbsencePanelData = getSpecifiquePanelData($ctrl.rapportData, 'Task');
                    $scope.$broadcast('event.refresh.pointage.hebdo.task.panel', {
                        rapportTaskAbsencePanelData: $ctrl.rapportTaskAbsencePanelData,
                        rapportMajorationPanelData: $ctrl.rapportMajorationPanelData,
                        isAffichageParOuvrier: $ctrl.isAffichageParOuvrier
                    });

                    $ctrl.rapportPrimePanelData = getSpecifiquePanelData($ctrl.rapportData, 'Prime');
                    $scope.$broadcast('event.refresh.pointage.hebdo.prime.panel', { rapportPrimePanelData: $ctrl.rapportPrimePanelData, isAffichageParOuvrier: $ctrl.isAffichageParOuvrier });

                    var validatedByManager = false;
                    var tooltipSave = "";
                    for (var i = 0; i < $ctrl.rapportData.length; i++) {
                        if ($ctrl.rapportData[i].ApprovedBySuperior) {
                            validatedByManager = true;
                            tooltipSave = $ctrl.resources.RapportHebdo_Already_Validated;
                            break;
                        }
                    }

                    $ctrl.isValidatedByManager = validatedByManager;
                    $ctrl.tooltipSave = tooltipSave;
                })
                .finally(ProgressBar.complete);
        }
        /**
        * Action get data en mode affichage par ouvrier.
        * @param {Date} mondayDate Date du lundi
        */
        function actionGetDataAffichageParOuvrier(mondayDate) {
            var personnelId = PointageHedboService.getSelectedOuvrierId();
            $ctrl.personnelIdList = [];

            $ctrl.personnelIdList.push(personnelId);

            // Appel web service

            GetPointageByPersonnelIDAndIntervalParCi(mondayDate);
            ProgressBar.start();
            var allCi = $ctrl.isEtamIac !== undefined && $ctrl.isEtamIac === true;
            PointageHedboService.GetRapportHebdoByWorker({ personnelId: personnelId, mondayDate: mondayDate, allCi: allCi })
                .$promise
                .then(function (value) {
                    $ctrl.rapportData = value;
                    $ctrl.rapportAstreintePanelData = getSpecifiquePanelData($ctrl.rapportData, 'Astreinte');
                    $ctrl.rapportAstreintePanelData[0].SubNodeList = $ctrl.rapportAstreintePanelData[0].SubNodeList.filter(r => r.NodeTypeLibelle === "Affaire");
                    $scope.$broadcast('event.refresh.pointage.hebdo.astreinte.panel', { rapportAstreintePanelData: $ctrl.rapportAstreintePanelData, isAffichageParOuvrier: true });

                    $ctrl.rapportMajorationPanelData = getSpecifiquePanelData($ctrl.rapportData, 'Majoration');
                    $scope.$broadcast('event.refresh.pointage.hebdo.majoration.panel', { rapportMajorationPanelData: $ctrl.rapportMajorationPanelData, isAffichageParOuvrier: true });

                    $ctrl.rapportTaskAbsencePanelData = getSpecifiquePanelData($ctrl.rapportData, 'Task');
                    $scope.$broadcast('event.refresh.pointage.hebdo.task.panel', {
                        rapportTaskAbsencePanelData: $ctrl.rapportTaskAbsencePanelData,
                        rapportMajorationPanelData: $ctrl.rapportMajorationPanelData,
                        isAffichageParOuvrier: $ctrl.isAffichageParOuvrier
                    });

                    $ctrl.rapportPrimePanelData = getSpecifiquePanelData($ctrl.rapportData, 'Prime');
                    $scope.$broadcast('event.refresh.pointage.hebdo.prime.panel', { rapportPrimePanelData: $ctrl.rapportPrimePanelData, isAffichageParOuvrier: $ctrl.isAffichageParOuvrier });

                    if ($ctrl.isEtamIac && $ctrl.rapportTaskAbsencePanelData && $ctrl.rapportTaskAbsencePanelData[0] && $ctrl.rapportTaskAbsencePanelData[0].Statut === 'IAC') {
                        $ctrl.isAbsenceColumnHidden = true;
                        $ctrl.isIac = true;
                    }
                    else if ($ctrl.isEtamIac && $ctrl.isManagerPointing === false) {
                        $ctrl.isAbsenceColumnHidden = true;
                        $ctrl.isIac = false;
                    }
                    else {
                        $ctrl.isAbsenceColumnHidden = false;
                        $ctrl.isIac = false;
                    }

                    $ctrl.isValidatedByManager = $ctrl.rapportData[0].ApprovedBySuperior;
                    $ctrl.tooltipSave = $ctrl.isValidatedByManager ? $ctrl.resources.RapportHebdo_Already_Validated : '';
                })
                .finally(ProgressBar.complete);
        }

        /**
         * Function Handle save rapport hebdo majoration ligne 
         **/
        function handleSaveMajoration() {
            $ctrl.rapportHebdoMajorationList = [];
            if ($ctrl.isAffichageParOuvrier) {
                saveMajorationRapportHebdoWorker();
            }
            else {
                saveMajorationRapportHebdoCi();
            }
        }

        /** 
         * Function handle save majoration for worker
         **/
        function saveMajorationRapportHebdoWorker() {
            var ouvrierId = $ctrl.rapportMajorationPanelData[0].NodeId;
            for (var i = 0; i < $ctrl.rapportMajorationPanelData[0].SubNodeList.length; i++) {
                var ciMajorationNode = $ctrl.rapportMajorationPanelData[0].SubNodeList[i];
                if (ciMajorationNode !== undefined && !ciMajorationNode.IsPersonnelCiDesaffected && !ciMajorationNode.IsAbsence) {
                    for (var j = 0; j < ciMajorationNode.SubNodeList[0].Items.length; j++) {
                        var majorationItems = ciMajorationNode.SubNodeList[0].Items[j];
                        for (var k = 0; k < majorationItems.MajorationHeurePerDayList.length; k++) {
                            var majorationLigne = majorationItems.MajorationHeurePerDayList[k];
                            if (majorationLigne.IsCreated || majorationLigne.IsUpdated) {
                                var majorationRapportLigne = InitmajorationRapportLigne(majorationLigne, ciMajorationNode.NodeId, ouvrierId, majorationItems.CodeMajorationId);
                                $ctrl.rapportHebdoMajorationList.push(majorationRapportLigne);
                            }
                        }
                    }
                }
            }
        }

        /**
         * Function handle save majoration for CI
         **/
        function saveMajorationRapportHebdoCi() {
            for (var i = 0; i < $ctrl.rapportMajorationPanelData.length; i++) {
                var ciId = $ctrl.rapportMajorationPanelData[i].NodeId;
                for (var j = 0; j < $ctrl.rapportMajorationPanelData[i].SubNodeList.length; j++) {
                    var personnelMajorationNode = $ctrl.rapportMajorationPanelData[i].SubNodeList[j];
                    if (personnelMajorationNode !== undefined && !personnelMajorationNode.IsPersonnelCiDesaffected && !personnelMajorationNode.IsAbsence) {
                        for (var k = 0; k < personnelMajorationNode.SubNodeList[0].Items.length; k++) {
                            var majorationItems = personnelMajorationNode.SubNodeList[0].Items[k];
                            for (var l = 0; l < majorationItems.MajorationHeurePerDayList.length; l++) {
                                var majorationLigne = majorationItems.MajorationHeurePerDayList[l];
                                if (majorationLigne.IsCreated || majorationLigne.IsUpdated) {
                                    var majorationRapportLigne = InitmajorationRapportLigne(majorationLigne, ciId, personnelMajorationNode.NodeId, majorationItems.CodeMajorationId);
                                    $ctrl.rapportHebdoMajorationList.push(majorationRapportLigne);
                                }
                            }
                        }
                    }
                }
            }
        }

        /** 
         * Function initial majoration rapport hebdo object 
         * @param {any} majorationLigne Ligne de majoration
         * @param {number} ciId Identifiant du CI
         * @param {number} personnelId Identifiant du personnel pour la ligne de majoration
         * @param {number} majorationCodeId Identifiant du code de majoration
         * @returns {any} Ligne de majoration initialisée
         */
        function InitmajorationRapportLigne(majorationLigne, ciId, personnelId, majorationCodeId) {
            var majorationRapportLigne = {
                rapportId: majorationLigne.rapportId,
                rapportLigneId: majorationLigne.RapportLigneId,
                personnelId: personnelId,
                ciId: ciId,
                majorationCodeId: majorationCodeId,
                heureMajoration: majorationLigne.HeureMajoration,
                dayOfWeek: majorationLigne.DayOfWeek
            };
            return majorationRapportLigne;
        }

        /**
         * Function Handle save rapport hebdo prime ligne 
         **/
        function handleSavePrime() {
            $ctrl.rapportHebdoPrimeList = [];
            if ($ctrl.isAffichageParOuvrier) {
                savePrimeRapportHebdoWorker();
            }
            else {
                savePrimeRapportHebdoCi();
            }
        }

        /** 
         * Function handle save prime for worker
         **/
        function savePrimeRapportHebdoWorker() {
            var ouvrierId = $ctrl.rapportPrimePanelData[0].NodeId;
            for (var i = 0; i < $ctrl.rapportPrimePanelData[0].SubNodeList.length; i++) {
                var ciPrimeNode = $ctrl.rapportPrimePanelData[0].SubNodeList[i];
                if (ciPrimeNode !== undefined && !ciPrimeNode.IsPersonnelCiDesaffected && !ciPrimeNode.IsAbsence) {
                    for (var j = 0; j < ciPrimeNode.SubNodeList[0].Items.length; j++) {
                        var primeItems = ciPrimeNode.SubNodeList[0].Items[j];
                        for (var k = 0; k < primeItems.RapportHebdoPrimePerDayList.length; k++) {
                            var primeLigne = primeItems.RapportHebdoPrimePerDayList[k];
                            if (primeLigne.IsCreated || primeLigne.IsUpdated) {
                                var primeRapportLigne = InitPrimeRapportLigne(primeLigne, ciPrimeNode.NodeId, ouvrierId, primeItems);
                                $ctrl.rapportHebdoPrimeList.push(primeRapportLigne);
                            }
                        }
                    }
                }
            }
        }

        /**
         * Function handle save majoration for CI
         **/
        function savePrimeRapportHebdoCi() {
            for (var i = 0; i < $ctrl.rapportPrimePanelData.length; i++) {
                var ciId = $ctrl.rapportPrimePanelData[i].NodeId;
                for (var j = 0; j < $ctrl.rapportPrimePanelData[i].SubNodeList.length; j++) {
                    var personnelPrimeNode = $ctrl.rapportPrimePanelData[i].SubNodeList[j];
                    if (personnelPrimeNode !== undefined && !personnelPrimeNode.IsPersonnelCiDesaffected && !personnelPrimeNode.IsAbsence) {
                        for (var k = 0; k < personnelPrimeNode.SubNodeList[0].Items.length; k++) {
                            var primeItems = personnelPrimeNode.SubNodeList[0].Items[k];
                            for (var l = 0; l < primeItems.RapportHebdoPrimePerDayList.length; l++) {
                                var primeLigne = primeItems.RapportHebdoPrimePerDayList[l];
                                if (primeLigne.IsCreated || primeLigne.IsUpdated) {
                                    var primeRapportLigne = InitPrimeRapportLigne(primeLigne, ciId, personnelPrimeNode.NodeId, primeItems);
                                    $ctrl.rapportHebdoPrimeList.push(primeRapportLigne);
                                }
                            }
                        }
                    }
                }
            }
        }

        /** 
         * Function initial prime rapport hebdo object 
         * @param {any} primeLigne Ligne de prime
         * @param {number} ciId Identifiant du CI
         * @param {number} personnelId Identifiant du personnel pour la ligne de majoration
         * @param {any} PrimeItem Prime sélectionnée
         * @returns {any} Ligne de prime initialisée
         */
        function InitPrimeRapportLigne(primeLigne, ciId, personnelId, PrimeItem) {
            var primeRapportLigne = {
                rapportId: primeLigne.RapportId,
                rapportLigneId: primeLigne.RapportLigneId,
                personnelId: personnelId,
                ciId: ciId,
                primeId: PrimeItem.PrimeId,
                isPrimeJournaliere: PrimeItem.IsPrimeJournaliere,
                isChecked: primeLigne.IsChecked,
                heurePrime: primeLigne.HeurePrime,
                dayOfWeek: primeLigne.DayOfWeek
            };
            return primeRapportLigne;
        }

        /*
         * @function handleChangeCalendarDate()
         * @description Gère l'évènement de la modification de la date du calendrier
         */
        function handleChangeDateNotification(isWeekChange, isAddWeek) {
            if (isWeekChange) {
                if (isAddWeek) {
                    $ctrl.selectedDate = moment($ctrl.selectedDate).add(1, 'week').toDate();
                    refresh();
                } else {
                    $ctrl.selectedDate = moment($ctrl.selectedDate).subtract(1, 'week').toDate();
                    refresh();
                }
            } else {
                refresh();
            }
        }

        /*
         * Hanlde change calendar date
        */
        $ctrl.handleChangeCalendarDate = function () {
            // NPI : corrige le bug du app-date-picker (en mode pompier, désolé...), ça évite :
            // - de charger 2 fois les mêmes données au chargement de l'écran
            // - de charger les données au clic (sans même changer la date...)
            // - de charger les données lorsque le jour change mais que c'est la même semaine

            var startDate = moment($ctrl.selectedDate).startOf('isoWeek');
            var newMondayDate = startDate.format('YYYY-MM-DD');
            if (newMondayDate === $ctrl.mondayDate) {
                generateSelectedWeekLabel();
            }
            else {
                handleChangeDateNotification();
            }
        };

        function GetOrganisationIdbyPersonnelId(personnelId) {
            return PointageHedboService.GetPersonnelGroupebyId({ personnelId: personnelId })
                .$promise
                .then(function (value) {
                    $ctrl.groupeId = value.GroupeId;
                }).catch(function (error) {
                    console.log(error);
                    Notify.error($ctrl.resources.Global_Notification_Error);
                });
        }

        function getPersonnelIdList(model) {
            $ctrl.personnelIdList = [];
            for (var i = 0; i < model.length; i++) {
                $ctrl.personnelIdList.push.apply($ctrl.personnelIdList, model[i].OuvrierListId);
            }

            $ctrl.personnelIdList = $ctrl.personnelIdList.filter(function (item, pos) {
                return $ctrl.personnelIdList.indexOf(item) === pos;
            });
        }

        function refreshPrimePersonnelAffected() {
            $scope.$broadcast('event.refresh.prime.personnel.affected', {
                personnelIdList: $ctrl.personnelIdList,
                data: $ctrl.rapportTaskAbsencePanelData
            });
        }

        function refreshAstreintes() {
            $scope.$broadcast('event.refresh.astreintes.affected', {
                data: $ctrl.rapportTaskAbsencePanelData
            });
        }

        function disableSaveWhenLoad(disable) {
            $ctrl.disableSaveWhenLoad = disable;
        }

        function actionSaveLoadEnd() {
            ProgressBar.complete();
            disableSaveWhenLoad(false);
        }

        function actionSaveLoadStart() {
            ProgressBar.start();
            disableSaveWhenLoad(true);
        }

        function displayRapportHebdoOuvrierForSyntheseValidationAffaires(obj) {
            if (obj) {
                $ctrl.isForValidationAffaires = true;
                $ctrl.personnelIdList = [];
                var selectedEntreeOuvrierList = [];
                var model = { Mondaydate: obj.date, RapportHebdoEntreeList: obj.rapportHebdoEntreeList };
                angular.forEach(obj.rapportHebdoEntreeList, function (obj) {
                    $ctrl.personnelIdList.push.apply($ctrl.personnelIdList, obj.OuvrierListId);
                    if (obj.OuvrierListId && obj.OuvrierListId.length > 0) {
                        angular.forEach(obj.OuvrierListId, function (ouvrierId) {
                            selectedEntreeOuvrierList.push({ PersonnelId: ouvrierId, CiId: obj.CiId });
                        });
                    }
                });

                const distinct = (value, index, self) => { return self.indexOf(value) === index; };
                $ctrl.personnelIdList = $ctrl.personnelIdList.filter(distinct);
                PointageHedboService.updateSelectedEntreeOuvrierList(selectedEntreeOuvrierList);
                $ctrl.selectedDate = obj.date;
                generateWeekDateAndLabel();
                GetRapportHebdoByCi(model);
            }
        }

        function getDisplayByCiPointageStatut(data) {
            if (data) {
                data = data.filter(ci => !ci.IsAbsence);
                if (checkStatut(data, 'V2')) {
                    $ctrl.weekPointageStatutCode = 'V2';
                    $ctrl.weekPointageStatutLibelle = 'Validé 2';
                    return;
                }
                if (checkStatut(data, 'VE')) {
                    $ctrl.weekPointageStatutCode = 'VE';
                    $ctrl.weekPointageStatutLibelle = 'Verrouillé';
                    return;
                }

                $ctrl.weekPointageStatutCode = 'EC';
                $ctrl.weekPointageStatutLibelle = 'En cours';
            }
        }

        function checkStatut(data, statut) {
            return data.every(function (item) {
                return item.Statut === statut;
            });
        }
    }
})();