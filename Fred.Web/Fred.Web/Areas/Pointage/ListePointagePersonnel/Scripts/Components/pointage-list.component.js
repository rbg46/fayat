(function () {
    'use strict';

    var pointageListComponent = {
        templateUrl: '/Areas/Pointage/ListePointagePersonnel/Scripts/Components/pointage-list.component.html',
        bindings: {
            resources: '<',
            readOnly: '<',
            personnel: '<',
            periode: '<',
            displayBanner: '=',
            pointagesList: '=',
            isGsp: '<',
            isSomopa: '<'
        },
        require: {
            parentCtrl: '^ngController'
        },
        controller: pointageListController
    };

    angular.module('Fred').component('pointageListComponent', pointageListComponent);

    angular.module('Fred').controller('pointageListController', pointageListController);

    pointageListController.$inject = ['UserService',
        '$scope',
        '$filter',
        'ProgressBar',
        'PointagePersonnelService',
        'PersonnelPickerService',
        'DatePickerService',
        'PointageHelperService',
        'Notify',
        'PointageDuplicatorProccesService',
        'DatesClotureComptableService',
        'fredSubscribeService'];

    function pointageListController(UserService,
        $scope,
        $filter,
        ProgressBar,
        PointagePersonnelService,
        PersonnelPickerService,
        DatePickerService,
        PointageHelperService,
        Notify,
        PointageDuplicatorProccesService,
        DatesClotureComptableService,
        fredSubscribeService) {

        var listDays = [];
        var listPointages = [];
        var jourErrone = [];
        var weekErrone = [];
        //////////////////////////////////////////////////////////////////
        // Déclaration des propriétés publiques                         //
        //////////////////////////////////////////////////////////////////
        var $ctrl = this;
        $ctrl.lstErreurs = [];
        $ctrl.pointagesList = [];
        $ctrl.ciNewPointage = null;
        $ctrl.TotalHeureNormale = 0;
        $ctrl.TotalHeureAbsence = 0;
        $ctrl.TotalHeureMajoration = 0;
        $ctrl.element = null;
        $ctrl.userOrganizationId = null;
        $ctrl.userOrganisationType = 0;
        $ctrl.isUserFes = false;
        $ctrl.saveEnable = true;
        $ctrl.isDefaultPointage = false;
        $ctrl.selectedRaportLigneAstreinte = null;
        $ctrl.isIac = false;
        $ctrl.datePintageSelected = "";
        $ctrl.PointagePersonnelService = PointagePersonnelService;
        $ctrl.selectedRaportLigneIndex = 0;
        $ctrl.selectedRaportLigneAstreinteIndex = 0;
        $ctrl.permissionKeys = PERMISSION_KEYS;
        $ctrl.personnel = null;
        $ctrl.chargeListPointagesInProgress = false;
        $ctrl.CI_Search_CIType_Etude = "CI_Search_CIType_Etude";
        $ctrl.IsShevalError = false;
        $ctrl.IsSuperieureError = false;
        const maxHourFes = 10;

        //////////////////////////////////////////////////////////////////
        // Déclaration des fonctions publiques                          //
        //////////////////////////////////////////////////////////////////
        $ctrl.checkTotalTacheMoreThanOne = checkTotalTacheMoreThanOne;
        $ctrl.GetURLForCISearch = GetURLForCISearch;
        $ctrl.handleChangeHeureNormale = handleChangeHeureNormale;
        $ctrl.handleChangeHeureAbsence = handleChangeHeureAbsence;
        $ctrl.handleChangeHeureAbsenceIac = handleChangeHeureAbsenceIac;
        $ctrl.handleChangeIVD = handleChangeIVD;
        $ctrl.handleCancelNewPointage = handleCancelNewPointage;
        $ctrl.handleClickDelete = handleClickDelete;
        $ctrl.handleClickDeleteCI = handleClickDeleteCI;
        $ctrl.handleClickDeleteAbsence = handleClickDeleteAbsence;
        $ctrl.handleClickDeleteDeplacement = handleClickDeleteDeplacement;
        $ctrl.handleClickDeleteZoneDeplacement = handleClickDeleteZoneDeplacement;
        $ctrl.handleClickIndemnites = handleClickIndemnites;
        $ctrl.handleClickNewPointage = handleClickNewPointage;
        $ctrl.handleDisplayTotalHeureMajoration = handleDisplayTotalHeureMajoration;
        $ctrl.handleSelectCI = handleSelectCI;
        $ctrl.handleSelectCodeAbsence = handleSelectCodeAbsence;
        $ctrl.handleSelectCodeDeplacement = handleSelectCodeDeplacement;
        $ctrl.handleSelectCodeZoneDeplacement = handleSelectCodeZoneDeplacement;
        $ctrl.handleSetNewPointage = handleSetNewPointage;
        $ctrl.handleShowDeletePopIn = handleShowDeletePopIn;
        $ctrl.handleShowDetail = handleShowDetail;
        $ctrl.handleShowDuplicatePopIn = handleShowDuplicatePopIn;
        $ctrl.heuresAbsenceMin = heuresAbsenceMin;
        $ctrl.heuresAbsenceMax = heuresAbsenceMax;
        $ctrl.loadErreurs = loadErreurs;
        $ctrl.showPickList = showPickList;
        $ctrl.handleAddAstreinte = handleAddAstreinte;
        $ctrl.handleDeleteSortieAstreinte = handleDeleteSortieAstreinte;
        $ctrl.handleChangeAstreinteDates = handleChangeAstreinteDates;
        $ctrl.handleChangeDateDebutAstreinte = handleChangeDateDebutAstreinte;
        $ctrl.handleChangeDateFinAstreinte = handleChangeDateFinAstreinte;
        $ctrl.handleMinDateFinAstreinte = handleMinDateFinAstreinte;
        $ctrl.handleMaxDateFinAstreinte = handleMaxDateFinAstreinte;
        $ctrl.handleMinDateDebutAstreinte = handleMinDateDebutAstreinte;
        $ctrl.handleMaxDateDebutAstreinte = handleMaxDateDebutAstreinte;
        $ctrl.handleCheckAstreinte = handleCheckAstreinte;
        $ctrl.handleSuperieureCondition = handleSuperieureCondition;
        $ctrl.handleShevalCondition = handleShevalCondition;
        $ctrl.getDateTime = getDateTime;
        $ctrl.anyErreur = anyErreur;
        $ctrl.hasVericalScrollbarVisible = hasVericalScrollbarVisible;
        $ctrl.astreinteDetailIsVisible = astreinteDetailIsVisible;
        $ctrl.getReferentielRhUrl = getReferentielRhUrl;

        //////////////////////////////////////////////////////////////////
        // Init                                                         //
        //////////////////////////////////////////////////////////////////

        $ctrl.$onInit = function () {
            fredSubscribeService.subscribe({ eventName: 'pointage-duplicator-process-ask-load', callback: actionChargeListPointages });

            UserService.getCurrentUser().then(function (user) {
                $ctrl.userOrganizationId = user.Personnel.Societe.Organisation.OrganisationId;
                $ctrl.userOrganisationType = user.Personnel.Societe.Organisation.TypeOrganisationId;
                $ctrl.isUserFes = user.Personnel.Societe.Groupe.Code.trim() === 'GFES' ? true : false;
                $ctrl.isUserRZB = user.Personnel.Societe.Groupe.Code.trim() === 'GRZB' ? true : false;
            });

            $scope.$on('chargeListPointages', function () { actionChargeListPointages(); });
            $scope.$on('updatePointage', function () {
                onUpdatePointage();
            });
            if (sessionStorage.getItem('pointagePersonnelFilter') !== null) {
                actionChargeListPointages();
            }

            $scope.$on('handleDefaultPointage', function () { actionPointageParDefaut(); });
            GetRapportStatutVerrouille();
        };

        //////////////////////////////////////////////////////////////////
        // Handlers                                                     //
        //////////////////////////////////////////////////////////////////

        function getReferentielRhUrl() {
            if ($ctrl.isUserFes) {
                if (PersonnelPickerService.IsETAM())
                    return "isETAM=true&";
                if (PersonnelPickerService.IsOuvrier())
                    return "isOuvrier=true&";
                if (PersonnelPickerService.IsCadre())
                    return "isCadre=true&";
                else
                    return "";
            }
            return "";
        }

        function handleShowDetail(pointage, index) {
            actionDisplayDetail(pointage, index);
            $scope.$emit('openPointage');
            $scope.$emit('list.total.ci', { pointagelist: $ctrl.pointagesList });
        }

        function IsRapportVerrouille(pointage) {
            return (pointage.Rapport.RapportStatutId !== null && pointage.Rapport.RapportStatutId === $ctrl.rapportStatutVerrouilleId) ||
                (pointage.RapportLigneStatutId !== null && pointage.RapportLigneStatutId === $ctrl.rapportStatutVerrouilleId);
        }

        function GetRapportStatutVerrouille() {
            return PointagePersonnelService.GetRapportStatutVerrouille()
                .then(function (value) {
                    $ctrl.rapportStatutVerrouilleId = value.data;
                });
        }

        function handleClickNewPointage(pointage, index) {
            PointageHelperService.setPointageIndex(pointage, index);
            $("#addNewPointage").modal();
        }

        function handleSetNewPointage() {
            actionSetNewPointage();
        }

        function handleCancelNewPointage() {
            actionCancelNewPointage();
        }

        function handleSelectCI(item, pointage, index, list) {
            $ctrl.wait = true;
            var date = new Date(DatePickerService.getDate());
            var year = date.getFullYear();
            var month = date.getMonth() + 1;
            if ($ctrl.isUserFes) {
                for (var i = 0; i < list.length; i++) {
                    if (!list[i].IsDeleted && list[i].Day.Date === pointage.Day.Date && list[i].CiId === item.CiId) {
                        Notify.error($ctrl.resources.PointagePersonnel_Ci_Existe);
                        return;
                    }
                }
            }

            DatesClotureComptableService.GetPeriodStatus(item.CiId, year, month)
                .then(function (response) {
                    var isClosed = response.data;
                    if (isClosed) {
                        $ctrl.wait = false;
                        Notify.error($ctrl.resources.Pointage_Error_CI_Periode_Cloturee);
                        return;
                    }
                    PointageHelperService.setPointageIndex(pointage, index);
                    actionSelectCiNewPointage(item);
                })
                .catch(function (error) {
                    $ctrl.wait = false;
                    Notify.error(error);
                });
        }

        function handleClickIndemnites(pointage) {
            if (!$ctrl.readOnly && !pointage.IsLocked) {
                actionRefreshIndemnites(pointage);
            }
        }

        function handleShowDeletePopIn(pointage, index) {
            if (!$ctrl.readOnly && !pointage.IsLocked) {
                PointageHelperService.setPointageIndex(pointage, index);
                $("#confirmationDelete").modal();
            }
        }

        function checkStatut() {
            $ctrl.isIac = PersonnelPickerService.getIsIac();
        }

        function handleClickDelete() {
            actionDelete();
            refreshTotalHeureAbsence();
            refreshTotalHeureNormale();
            refreshTotalHeureMajoration();
        }

        function handleClickDeleteAbsence(pointage) {
            pointage.CodeAbsence = null;
            pointage.CodeAbsenceId = null;
            pointage.HeureAbsence = null;
            pointage.NumSemaineIntemperieAbsence = null;
            refreshTotalHeureAbsence();
            PointageHelperService.actionIsUpdated(pointage);
            $ctrl.parentCtrl.saveEnable = true;
        }

        function handleClickDeleteDeplacement(pointage) {
            pointage.CodeDeplacement = null;
            pointage.CodeDeplacementId = null;
            PointageHelperService.actionIsUpdated(pointage);
        }

        function handleClickDeleteZoneDeplacement(pointage) {
            pointage.CodeZoneDeplacement = null;
            pointage.CodeZoneDeplacementId = null;
            PointageHelperService.actionIsUpdated(pointage);
        }

        function handleClickDeleteCI() {
            $ctrl.ciNewPointage = null;
        }

        function handleSelectCodeAbsence(item, pointage) {
            var previousSomme = 0;
            pointage.CodeAbsenceId = item.CodeAbsenceId;
            pointage.CodeAbsence = item;
            PointageHelperService.refreshHeuresAbsenceDefaut(pointage);
            PointageHelperService.setNumSemaineIntemperie(pointage);
            PointageHelperService.actionIsUpdated(pointage);

            var pointages = $ctrl.pointagesList.filter(x => x.DatePointage === pointage.DatePointage && x.CiId !== pointage.CiId && x.RapportLigneId !== pointage.RapportLigneId);
            pointages.forEach(function (node) {
                previousSomme += parseFloat(node.HeureNormale) + parseFloat(node.HeureAbsence);
            });

            var somme = pointage.HeureNormale + pointage.HeureAbsence;
            if ($ctrl.isUserFes && somme + previousSomme > maxHourFes) {
                Notify.error($ctrl.resources.PointagePersonnel_Depassement_journee_FES);
                $ctrl.parentCtrl.saveEnable = false;
            }
            else {
                $ctrl.parentCtrl.saveEnable = true;
            }

            $ctrl.element.focus();
            refreshTotalHeureAbsence();
        }

        function handleSelectCodeDeplacement(item, pointage) {
            pointage.CodeDeplacementId = item.CodeDeplacementId;
            pointage.CodeDeplacement = item;
            PointageHelperService.actionIsUpdated(pointage);
            $ctrl.element.focus();
        }

        function handleSelectCodeZoneDeplacement(item, pointage) {
            pointage.CodeZoneDeplacementSaisiManuellement = true;
            pointage.CodeZoneDeplacementId = item.CodeZoneDeplacementId;
            pointage.CodeZoneDeplacement = item;
            PointageHelperService.actionIsUpdated(pointage);
            $ctrl.element.focus();
        }

        function handleShowDuplicatePopIn(pointage) {
            if (!$ctrl.readOnly) {
                var duplicateInfo = {
                    pointage: pointage,
                    pointagesList: $ctrl.pointagesList
                };
                PointageDuplicatorProccesService.startDuplicationProcess(duplicateInfo);
            }
        }

        function handleChangeHeureNormale(pointage) {
            if (!pointage.HeureNormale) {
                pointage.HeureNormale = 0;
            }
            actionChangeHeureNormale(pointage);
            refreshTotalHeureNormale();
            PointageHelperService.refreshTotalHeure(pointage);
            PointageHelperService.actionIsUpdated(pointage);
        }

        function handleChangeHeureAbsenceIac(pointage) {
            if ($ctrl.isIac && $ctrl.isUserFes) {
                if (parseFloat(pointage.HeureAbsenceIac) !== 1 && parseFloat(pointage.HeureAbsenceIac) !== 0.5 && parseFloat(pointage.HeureAbsenceIac) !== 0) {
                    pointage.HeureAbsenceIac = 1;
                    Notify.error($ctrl.resources.valeur_admises_error);
                }
                else {
                    pointage.HeureAbsence = pointage.HeureAbsenceIac ? pointage.HeureAbsenceIac * 7 : 0;
                    pointage.IsUpdated = true;
                    var totalHourAbsence = 0;
                    var pointageList = $ctrl.pointagesList.filter(x => !x.IsDeleted && x.DatePointage === pointage.DatePointage);
                    if (pointageList) {
                        for (var p = 0; p < pointageList.length; p++) {
                            if (!isNaN(parseFloat(pointageList[p].HeureAbsence))) {
                                totalHourAbsence += parseFloat(pointageList[p].HeureAbsence);
                            }
                        }

                        if ($ctrl.isUserFes && totalHourAbsence > maxHourFes) {
                            Notify.error($ctrl.resources.PointagePersonnel_Depassement_journee_FES_IAC);
                            $ctrl.parentCtrl.saveEnable = false;
                        }
                        else {
                            $ctrl.parentCtrl.saveEnable = true;
                        }
                    }
                }
            }
        }

        function handleChangeHeureAbsence(pointage) {
            if ($ctrl.isUserFes && pointage.Personnel.Statut === "3") {
                if (parseFloat(pointage.HeureAbsence) !== 1 && parseFloat(pointage.HeureAbsence) !== 0.5 && parseFloat(pointage.HeureAbsence) !== 0) {
                    pointage.HeureAbsence = 1;
                    Notify.error($ctrl.resources.valeur_admises_error);
                }
            }
            if ($ctrl.isUserFes) {
                checkHeureJournalier(pointage.DatePointage);
                checkHeureByWeek(pointage.DatePointage);
            }
            refreshTotalHeureAbsence();
            PointageHelperService.actionIsUpdated(pointage);
        }

        function handleChangeIVD(pointage) {
            PointageHelperService.actionIsUpdated(pointage);
        }

        //////////////////////////////////////////////////////////////////
        // Actions                                                      //
        //////////////////////////////////////////////////////////////////


        function onUpdatePointage() {
            var pointageIndex = PointageHelperService.getPointageIndex();
            if (pointageIndex) {
                if ($ctrl.isIac && $ctrl.isUserFes) {
                    UpdateHeureNormalesAndAbsenceIac(pointageIndex.pointage);
                }
                actionConvertToLocaleDatePointage(pointageIndex.pointage);
                $ctrl.pointagesList[pointageIndex.index] = pointageIndex.pointage;
            }
            else {
                Notify.error('Une erreur est survenue, la mise à jour du pointage à échouée');
            }
            reloadTotaux();
        }

        function actionChargeListPointages() {
            var periode = DatePickerService.getPeriode();
            $ctrl.personnel = PersonnelPickerService.getPersonnel();
            if (periode && $ctrl.personnel && !$ctrl.chargeListPointagesInProgress) {
                $ctrl.chargeListPointagesInProgress = true;
                PointagePersonnelService.GetDateEntreeSortie($ctrl.personnel.PersonnelId).then(function (value) {
                    $ctrl.personnel.DateEntree = value.data.DateEntree;
                    $ctrl.personnel.DateSortie = value.data.DateSortie;

                    if (periode && $ctrl.personnel) {
                        sessionStorage.setItem('pointagePersonnelFilter', JSON.stringify({ Personnel: $ctrl.personnel, Periode: convertDate(periode) }));
                        $ctrl.pointagesList = [];
                        ProgressBar.start();
                        disableChangeCalendar(true);
                        PointagePersonnelService.GetPointagesByPersonnelIdAndPeriode(periode, $ctrl.personnel.PersonnelId)
                            .then(actionLoadSuccess)
                            .catch(actionLoadError)
                            .finally(actionLoadEnd);
                    }
                })
                    .catch(actionLoadError);

            }
        }

        // Pour la compatibilité du format de date sur Firefox
        function convertDate(date) {
            var dateArray = date.split("-");
            return dateArray[0] + "/" + dateArray[1] + "/" + dateArray[2].substring(2, 4);
        }

        function actionDisplayDetail(pointage, index) {
            PointageHelperService.setPointageIndex(pointage, index);
            $ctrl.displayBanner = !$ctrl.displayBanner;
            $ctrl.parentCtrl.actionToggleSidebar($ctrl.displayBanner);
        }

        function actionLoadSuccess(result) {
            listPointages = result.data.Pointages;
            PointagePersonnelService.CodeDeplacementReadonly = result.data.CodeDeplacementReadonly;
            PointagePersonnelService.ShowSaisieManuelle = result.data.ShowSaisieManuelle;
            PointagePersonnelService.ShowDeplacement = result.data.ShowDeplacement;

            actionConvertToLocaleDate(listPointages);

            angular.forEach(listPointages, function (val, key) {
                val.View = {
                    CodeDeplacementPlusFavorable: val.CodeDeplacementPlusFavorable,
                    CodeZoneDeplacementSaisiManuellement: val.CodeZoneDeplacementSaisiManuellement
                };
                val.IsRapportLocked = IsRapportVerrouille(val);
            });

            return PointagePersonnelService.GetDaysInMonth(DatePickerService.getPeriode(), DatePickerService.getIsPeriodeWeek()).then(function (value) {
                listDays = value.data;
                initEvenDays();
                initLibPrimeAbrege();
                generateListPointageAndDaysAndPointages();
                lockPointage();
                reloadTotaux();
                checkStatut();
                if ($ctrl.isIac && $ctrl.isUserFes) {
                    ChangeHeurNormalAndAbsenceIac();
                }
                reloadTotaux();
                fredSubscribeService.raiseEvent('list-finish-to-load', $ctrl.pointagesList);
            });
        }

        function hasVericalScrollbarVisible() {
            if (!FredToolBox.hasVerticalScrollbarVisible('#bodyPointagePersonnel')) {
                return false;
            }
            else {
                return true;
            }
        }

        function astreinteDetailIsVisible() {
            if ($('#astreinte-detail').is(':visible')) {
                return true;
            }
            else {
                return false;
            }
        }

        /*
       * @description Convertir les dates en local
       */
        function actionConvertToLocaleDate(listPointages) {
            var isChrome = !!window.chrome && (!!window.chrome.webstore || !!window.chrome.runtime);
            if (listPointages) {
                angular.forEach(listPointages, function (pointage) {
                    actionConvertToLocaleDatePointage(pointage, isChrome);
                });
            }
        }

        function actionConvertToLocaleDatePointage(pointage, isChrome) {
            angular.forEach(pointage.ListRapportLigneAstreintes, function (rapportLigneAstreintes) {
                if (isChrome) {
                    rapportLigneAstreintes.DateDebutAstreinte = moment(rapportLigneAstreintes.DateDebutAstreinte).utc(false);
                    rapportLigneAstreintes.DateFinAstreinte = moment(rapportLigneAstreintes.DateFinAstreinte).utc(false);
                } else {
                    rapportLigneAstreintes.DateDebutAstreinte = moment($filter('toLocaleDate')(rapportLigneAstreintes.DateDebutAstreinte)).utc(false);
                    rapportLigneAstreintes.DateFinAstreinte = moment($filter('toLocaleDate')(rapportLigneAstreintes.DateFinAstreinte)).utc(false);
                }
            });
        }

        function initEvenDays() {
            listDays.forEach(x => x.IsEven = dayIsEven(x.Date));
        }

        //Factorisation des méthodes ChangeHeurNormalIac & ChangeHeurAbsenceIac
        function ChangeHeurNormalAndAbsenceIac() {
            if ($ctrl.pointagesList && $ctrl.pointagesList.length > 0) {
                for (var i = 0; i < $ctrl.pointagesList.length; i++) {
                    let pointage = $ctrl.pointagesList[i];
                    if (pointage) {
                        UpdateHeureNormalesAndAbsenceIac(pointage);
                    }
                }
            }
        }

        function UpdateHeureNormalesAndAbsenceIac(pointage) {
            pointage.HeureNormaleIac = pointage.HeureNormale ? pointage.HeureNormale / 7 : 0;
            pointage.HeureAbsenceIac = pointage.HeureAbsence ? pointage.HeureAbsence / 7 : 0;
        }

        function getDateTime(dateStr) {
            if (dateStr) {
                return moment(dateStr).format('HH:mm');
            }
            return "";
        }

        function dayIsEven(date) {
            return new Date(date).getDate() % 2 === 0;
        }

        function actionChangeHeureNormale(pointage) {
            for (var i = 0; i < pointage.ListRapportLigneTaches.length; i++) {
                if (!pointage.ListRapportLigneTaches[i].IsDeleted) {
                    if ($ctrl.isIac && $ctrl.isUserFes) {
                        pointage.HeureNormale = pointage.HeureNormaleIac ? parseFloat(pointage.HeureNormaleIac) * 7 : 0;
                    }

                    pointage.ListRapportLigneTaches[i].HeureTache = pointage.HeureMajoration && pointage.HeureNormale ? parseFloat(pointage.HeureNormale) + parseFloat(pointage.HeureMajoration) : pointage.HeureNormale ? parseFloat(pointage.HeureNormale) : 0;
                    break;
                }
            }
            if ($ctrl.isUserFes) {
                if (pointage.Personnel.Statut === 3) {
                    if (pointage.HeureNormale > 1) {
                        pointage.HeureNormale = 1;
                    }
                }
                checkHeureJournalier(pointage.DatePointage);
                checkHeureByWeek(pointage.DatePointage);
            }
        }

        function actionLoadError() {
            Notify.error($ctrl.resources.Global_Notification_Error);
        }

        function actionLoadEnd() {
            ProgressBar.complete();
            $ctrl.chargeListPointagesInProgress = false;
            disableChangeCalendar(false);
        }

        function GetURLForCISearch(date) {
            var perso = PersonnelPickerService.getPersonnel();
            if (perso) {
                if (perso.IsInterimaire) {
                    var dateFormat = $filter('date')(date, 'MM-dd-yyyy');
                    return `/api/CI/SearchLightForInterimaire/?interimaireId=${perso.PersonnelId}&date=${dateFormat}&`;
                }
                else {
                    if ($ctrl.isUserFes) {
                        return `/api/CI/SearchLightByPersonnel/${perso.PersonnelId}/`;
                    }
                    if ($ctrl.isUserRZB) {
                        return `/api/CI/SearchLightByPersonnel/?personnelId=${perso.PersonnelId}&societeId=${perso.SocieteId}&`;
                    }
                    else {
                        return `/api/CI/SearchLight?personnelSocieteId=${perso.SocieteId}&`;
                    }
                }
            }
        }

        function checkHeureJournalier(date) {
            var listPointageThisDaye = $ctrl.pointagesList.filter(p => p.DatePointage === date && !p.IsDeleted);
            var sommeAbsence = listPointageThisDaye.reduce(function (s, p) {
                if (p.HeureAbsence !== null) {
                    return s + parseFloat(p.HeureAbsence);
                }
                return s + 0;
            }, 0);
            var sommeNormal = listPointageThisDaye.reduce(function (s, p) {
                return s + parseFloat(p.HeureNormale);
            }, 0);
            var sommeMaj = listPointageThisDaye.reduce(function (s, p) {
                if (p.HeureMajoration) {
                    return s + parseFloat(p.HeureMajoration);
                } else {
                    return s;
                }
            }, 0);
            if ($ctrl.isUserFes && sommeAbsence + sommeNormal - sommeMaj > maxHourFes) {
                if (jourErrone.indexOf(date, 0) === -1) {
                    jourErrone.push(date);
                }

            } else {
                var index = jourErrone.indexOf(date, 0);
                if (index !== -1) {
                    jourErrone.splice(index, 1);
                }
            }
            if (weekErrone.length === 0 && jourErrone.length === 0) {
                $ctrl.parentCtrl.saveEnable = true;

            } else if (jourErrone.length !== 0) {
                $ctrl.parentCtrl.saveEnable = false;
                if (document.getElementsByClassName("ui-notification ng-scope error").length === 0)
                    Notify.error($ctrl.resources.error_pointage_Journalier);
            }
        }

        function checkHeureByWeek(date) {
            var wekNumber = moment(date).isoWeek();
            var listPointageThisWeek = $ctrl.pointagesList.filter(p => p.PointageId !== undefined && moment(p.DatePointage).isoWeek() === wekNumber && !p.IsDeleted);
            var sommeAbsence = listPointageThisWeek.reduce(function (s, p) {
                if (p.HeureAbsence !== null) {
                    return s + parseFloat(p.HeureAbsence);
                }
                return s + 0;
            }, 0);
            var sommeNormal = listPointageThisWeek.reduce(function (s, p) {
                return s + parseFloat(p.HeureNormale);
            }, 0);
            var sommeMaj = listPointageThisWeek.reduce(function (s, p) {
                if (p.HeureMajoration) {
                    return s + parseFloat(p.HeureMajoration);
                } else {
                    return s;
                }

            }, 0);
            if (sommeAbsence + sommeNormal - sommeMaj > 48) {
                if (weekErrone.indexOf(wekNumber, 0) === -1) {
                    weekErrone.push(wekNumber);
                }
                if (document.getElementsByClassName("ui-notification ng-scope error").length === 0) {
                    Notify.error($ctrl.resources.error_message_pointage_hebdo);
                }

            } else if (sommeAbsence + sommeNormal - sommeMaj <= 48 && sommeAbsence + sommeNormal - sommeMaj > 35) {
                if (weekErrone.indexOf(wekNumber, 0) === -1) {
                    weekErrone.push(wekNumber);
                }
                if (document.getElementsByClassName("ui-notification ng-scope error").length === 0)
                    Notify.error($ctrl.resources.error_message_pointage_implique_heur_sup);

            } else {
                var index = weekErrone.indexOf(wekNumber, 0);
                if (index !== -1) {
                    weekErrone.splice(index, 1);
                }
            }
            if (weekErrone.length === 0 && jourErrone.length === 0) {
                $ctrl.parentCtrl.saveEnable = true;
            } else {
                $ctrl.parentCtrl.saveEnable = false;
            }
        }

        function heuresAbsenceMin(pointage) {
            PointageHelperService.setIsUserFes($ctrl.isUserFes);
            return PointageHelperService.heuresAbsenceMin(pointage);
        }

        function heuresAbsenceMax(pointage) {
            PointageHelperService.setIsUserFes($ctrl.isUserFes);
            return PointageHelperService.heuresAbsenceMax(pointage);
        }

        // Initialise pour chaque pointage le libellé abrégé de sa liste de prime
        function initLibPrimeAbrege() {
            for (var i = 0; i < listPointages.length; i++) {
                PointageHelperService.refreshLibelleAbregePrime(listPointages[i]);
            }
        }

        // Complète les pointages avec les dates à affichées
        function generateListPointageAndDaysAndPointages() {
            for (var i = 0; i < listDays.length; i++) {
                var day = angular.copy(listDays[i]);
                var first = true;
                var any = true;

                for (var j = 0; j < listPointages.length; j++) {
                    var pointage = listPointages[j];
                    if (moment(day.Date).format('L') === moment(new Date(pointage.DatePointage)).format('L')) {
                        var dayClone = angular.copy(day);
                        if (first) {
                            dayClone.Display = true;
                            listPointages[j].Day = dayClone;
                            first = false;
                        }
                        else {
                            dayClone.Display = false;
                            listPointages[j].Day = dayClone;
                        }
                        $ctrl.pointagesList.push(pointage);
                        any = false;
                    }
                }
                if (any) {
                    day.Display = true;
                    var emptyDay = { Day: day, ListRapportLignePrimes: [], ListRapportLigneTaches: [], ListRapportLigneAstreintes: [], IsCreated: true, IsLocked: false, HasAstreinte: false };
                    $ctrl.pointagesList.push(emptyDay);
                }
            }
        }

        function lockPointage() {
            var perso = PersonnelPickerService.getPersonnel();
            for (var i = 0; i < $ctrl.pointagesList.length; i++) {
                var pointage = $ctrl.pointagesList[i];
                PointageHelperService.initLock(pointage, perso);
            }

            lockDaysForInterimaire();
        }

        function lockDaysForInterimaire() {
            var perso = PersonnelPickerService.getPersonnel();
            if (perso.IsInterimaire) {
                PointagePersonnelService.GetListOfDaysAvailable(PersonnelPickerService.getPersonnel().PersonnelId, DatePickerService.getPeriode())
                    .then(updateListPointagesForInterimaireLock)
                    .catch(actionFailLoadServer);
            }
        }

        function updateListPointagesForInterimaireLock(result) {
            var listDays = result.data;
            for (var i = 0; i < $ctrl.pointagesList.length; i++) {
                var pointage = $ctrl.pointagesList[i];
                pointage.InterimaireLock = false;
                if (listDays.indexOf(new Date(pointage.Day.Date).getDate()) === -1) {
                    pointage.NewPointageLocked = true;
                    pointage.InterimaireLock = true;
                }
            }
        }

        function actionPointageParDefaut() {
            ProgressBar.start();
            var personnel = PersonnelPickerService.getPersonnel();
            if (personnel) {
                PointagePersonnelService.GetDefaultCi(personnel.PersonnelId).then(function (result) {
                    pointageParDefault(result, personnel);
                }).catch(actionFailLoadServer).finally(ProgressBar.complete());
            }
        }

        async function pointageParDefault(result, personnel) {
            $ctrl.defaultCi = result.data;
            if ($ctrl.defaultCi && $ctrl.defaultCi.Taches && $ctrl.defaultCi.Taches.length > 0) {
                var tacheParDefault = $ctrl.defaultCi.Taches.find(function (element) {
                    return element.TacheParDefaut === true;
                });
                if (tacheParDefault) {
                    $ctrl.isDefaultPointage = true;
                    var listEmptyPointage = [];
                    for (var i = 0; i < $ctrl.pointagesList.length; i++) {
                        if (!$ctrl.pointagesList[i].Ci && !$ctrl.pointagesList[i].Day.IsWeekend) {
                            listEmptyPointage.push({
                                index: i,
                                pointage: $ctrl.pointagesList[i]
                            });
                        }
                    }
                    var dateMonth = new Date(DatePickerService.getPeriode().replace(/-/g, '/'));
                    var astreintes = await PointagePersonnelService.GetAstreintesByPersonnelIdAndCiId(personnel.PersonnelId, $ctrl.defaultCi.CiId, dateMonth.getFullYear(), dateMonth.getMonth() + 1);
                    var PointageModel = await PointagePersonnelService.GetNewObjectPointagePersonnelModel();

                    for (var j = 0; j < listEmptyPointage.length; j++) {
                        var pointage = listEmptyPointage[j];
                        PointageHelperService.setPointageIndex(pointage.pointage, pointage.index);
                        var pointageIndex = PointageHelperService.getPointageIndex();
                        if (!checkRapportLigne(pointageIndex.pointage)) continue;
                        var date = new Date(DatePickerService.getDate());
                        var year = date.getFullYear();
                        var month = date.getMonth() + 1; // l'index du premier mois est 0
                        var day = new Date(pointage.pointage.Day.Date).getUTCDate();
                        var astreinte = astreintes.data.filter(a => (new Date(a.DateAstreinte)).getUTCDate() === day);
                        pointage = InitPointagePersonnel(PointageModel.data, year, month, day, personnel, tacheParDefault, $ctrl.defaultCi, astreinte);
                        initNewPointage(pointage, pointageIndex);
                    }
                    $ctrl.isDefaultPointage = false;
                    refreshTotalHeureNormale();
                    refreshTotalHeureAbsence();
                    refreshTotalHeureMajoration();
                }
                else {
                    var message = String.format($ctrl.resources.Pointage_Default_Error_Default_CI_Taches,
                        !$ctrl.defaultCi.Societe ? "" : $ctrl.defaultCi.Societe.Code,
                        !$ctrl.defaultCi.EtablissementComptable ? "" : $ctrl.defaultCi.EtablissementComptable.Code, $ctrl.defaultCi.Code);
                    Notify.error(message);
                }
            }
            else {
                var messageError = String.format(
                    $ctrl.resources.Pointage_Default_Error_Default_CI,
                    personnel.Societe ? personnel.Societe.Code : "",
                    personnel.Matricule, personnel.Nom, personnel.Prenom);
                Notify.error(messageError);
            }
        }

        //check RapportLigne to create par default
        function checkRapportLigne(pointage) {
            return (!pointage.ListRapportLigneMajorations || pointage.ListRapportLigneMajorations.every(x => x.HeureMajoration === 0))
                && (!pointage.ListRapportLigneTaches || pointage.ListRapportLigneTaches.every(x => x.HeureTache === 0))
                && (!pointage.ListRapportLigneAstreintes || pointage.ListRapportLigneAstreintes.length === 0)
                && (!pointage.ListRapportLignePrimes || pointage.ListRapportLignePrimes.every(x => !x.IsChecked))
                && (!pointage.HeureAbsence || pointage.HeureAbsence === 0);
        }

        // Init Pointage Personnel
        function InitPointagePersonnel(PointageModel, year, month, day = 0, personnel, tacheParDefault, defaultCi, astreinte) {
            var pointage = PointageModel;

            pointage.CiId = defaultCi.CiId;
            pointage.Ci = defaultCi;
            pointage.HeureNormaleIac = 1;
            pointage.Personnel = personnel;

            var rapportLigneTache = {
                RapportLigneTacheId: 0,
                RapportLigneId: 0,
                TacheId: tacheParDefault.TacheId,
                Tache: tacheParDefault,
                HeureTache: 7,
                IsCreated: true,
                IsUpdated: false,
                IsDeleted: false,
                IsTemp: true
            };
            pointage.ListRapportLigneTaches = [];
            pointage.ListRapportLigneTaches.push(rapportLigneTache);
            if (astreinte && astreinte.length > 0) {
                pointage.HasAstreinte = true;
                pointage.AstreinteId = astreinte[0].AstreintId;
            }

            return pointage;
        }

        // ajout un nouveau pointage
        function actionSetNewPointage() {
            var pointageIndex = PointageHelperService.getPointageIndex();
            var pointage = PointageHelperService.getPointage();
            var date = new Date(DatePickerService.getDate());
            var year = date.getFullYear();
            var month = date.getMonth() + 1; // l'index du premier mois est 0
            var personnelId = PersonnelPickerService.getPersonnel().PersonnelId;
            var day = new Date(pointage.Day.Date).getUTCDate();
            if ($ctrl.isUserFes) {
                PointagePersonnelService.InitPointagePersonnel($ctrl.ciNewPointage.CiId, year, month, day, personnelId).then(function (result) {
                    initNewPointage(result.data, pointageIndex);
                })
                    .catch(actionFailLoadServer);
            } else {
                PointagePersonnelService.InitPointagePersonnel($ctrl.ciNewPointage.CiId, year, month).then(function (result) {
                    initNewPointage(result.data, pointageIndex);
                })
                    .catch(actionFailLoadServer);
            }
        }

        function actionCancelNewPointage() {
            $ctrl.ciNewPointage = null;
        }

        function initNewPointage(newPointage, pointageIndexOrigin) {
            var personnel = PersonnelPickerService.getPersonnel();
            var dayClone = angular.copy(pointageIndexOrigin.pointage.Day);
            newPointage.Personnel = personnel;
            newPointage.PersonnelId = personnel.PersonnelId;
            newPointage.PrenomNomTemporaire = personnel.LibelleRef;
            newPointage.DatePointage = dayClone.Date;
            newPointage.Day = dayClone;
            if ($ctrl.isDefaultPointage) {
                newPointage.HeureNormale = 7;
            }
            if (pointageIndexOrigin.pointage.PointageId === undefined) {
                $ctrl.pointagesList[pointageIndexOrigin.index] = angular.copy(newPointage);
                PointageHelperService.refreshLibelleAbregePrime($ctrl.pointagesList[pointageIndexOrigin.index]);
            }
            else {
                newPointage.Day.Display = false;
                var index = getPointageMaxIndex(dayClone.Date);
                $ctrl.pointagesList.splice(index + 1, 0, newPointage);
            }
            $ctrl.wait = false;
        }

        // Récupère l'index maximum d'un pointage pour une date donnée
        function getPointageMaxIndex(date) {
            var index = null;
            var i = 0;
            while (i < $ctrl.pointagesList.length && ($ctrl.pointagesList[i].Day.Date !== date || index === null)) {
                if ($ctrl.pointagesList[i].Day.Date === date) {
                    index = i;
                }
                i++;
            }
            return index;
        }

        function actionSelectCiNewPointage(ciNewPointage) {
            var pointageIndex = PointageHelperService.getPointageIndex();
            var pointage = PointageHelperService.getPointage();
            var date = new Date(DatePickerService.getDate());
            var year = date.getFullYear();
            var month = date.getMonth() + 1; // l'index du premier mois est 0
            var day = new Date(pointage.Day.Date).getUTCDate();
            var personnelId = PersonnelPickerService.getPersonnel().PersonnelId;
            if ($ctrl.isUserFes) {
                PointagePersonnelService.InitPointagePersonnel(ciNewPointage.CiId, year, month, day, personnelId).then(function (result) {
                    initNewPointage(result.data, pointageIndex);
                })
                    .catch(actionFailLoadServer);
            } else {
                PointagePersonnelService.InitPointagePersonnel(ciNewPointage.CiId, year, month, 0, personnelId).then(function (result) {
                    initNewPointage(result.data, pointageIndex);
                })
                    .catch(actionFailLoadServer);
            }
        }

        function actionFailLoadServer(reason) {
            Notify.error(reason.data.ExceptionMessage);
        }

        // Recharge les informations de déplacements pour un pointage donné
        function actionRefreshIndemnites(pointage) {
            ProgressBar.start();
            PointagePersonnelService.RefreshIndemniteDeplacement(pointage)
                .then(function (value) {
                    var result = value.data;
                    pointage.CodeDeplacement = result.CodeDeplacement;
                    pointage.CodeDeplacementId = result.CodeDeplacementId;
                    pointage.CodeZoneDeplacement = result.CodeZoneDeplacement;
                    pointage.CodeZoneDeplacementId = result.CodeZoneDeplacementId;
                    pointage.DeplacementIV = result.DeplacementIV;
                    pointage.CodeZoneDeplacementSaisiManuellement = result.CodeZoneDeplacementSaisiManuellement;
                    if (result.Warnings) {
                        angular.forEach(result.Warnings, function (val, key) {
                            Notify.warning(val);
                        });
                    }
                    PointageHelperService.actionIsUpdated(pointage);
                })
                .catch(function () { Notify.error($ctrl.resources.Global_Notification_Error); })
                .finally(ProgressBar.complete);
        }

        function handleAddAstreinte(pointage) {
            var todayDate = moment(pointage.DatePointage).set({ hour: 0, minute: 0 });
            if (pointage.PointageId > 0) {
                pointage.IsUpdated = true;
            }
            else {
                pointage.IsCreated = true;
            }
            pointage.ListRapportLigneAstreintes.push({
                AstreinteId: pointage.AstreinteId,
                DateDebutAstreinte: todayDate.utc(true),
                DateFinAstreinte: moment(todayDate).add(15, "minutes").utc(true),
                RapportLigneAstreinteId: 0,
                RapportLigneId: pointage.PointageId,
                IsDeleted: false

            });
            checkAllAstreintes();
        }

        function handleMinDateFinAstreinte() {
            if ($ctrl.datePintageSelected) {
                return moment($ctrl.datePintageSelected).set({ hour: 0, minute: 0 }).format("DD/MM/YYYY HH:mm");
            }
            return "";
        }

        function handleMaxDateFinAstreinte() {
            if ($ctrl.datePintageSelected) {
                return moment($ctrl.datePintageSelected).add(1, 'days').set({ hour: 23, minute: 59 }).format("DD/MM/YYYY HH:mm");
            }
            return "";
        }

        function handleMinDateDebutAstreinte() {
            if ($ctrl.datePintageSelected) {
                return moment($ctrl.datePintageSelected).set({ hour: 0, minute: 0 }).format("DD/MM/YYYY HH:mm");
            }
            return "";
        }

        function handleMaxDateDebutAstreinte() {
            if ($ctrl.datePintageSelected) {
                return moment($ctrl.datePintageSelected).set({ hour: 23, minute: 59 }).format("DD/MM/YYYY HH:mm");
            }
            return "";
        }

        function handleChangeDateDebutAstreinte(index) {
            checkAllAstreintes();
        }

        function handleChangeDateFinAstreinte(index) {
            checkAllAstreintes();
        }


        function handleChangeAstreinteDates(raportligneAstreinte, day, pointage) {
            $ctrl.datePintageSelected = "";
            $ctrl.selectedRaportLigneAstreinte = raportligneAstreinte;
            var date = new Date(day);
            var y = date.getFullYear();
            var m = date.getMonth() + 1;
            var d = date.getDate();
            var datestring = m + "/" + d + "/" + y;
            $ctrl.datePintageSelected = datestring;
            raportligneAstreinte.DateDebutAstreinte = moment(raportligneAstreinte.DateDebutAstreinte, "DD/MM/YYYY HH:mm").utc(true);
            raportligneAstreinte.DateFinAstreinte = moment(raportligneAstreinte.DateFinAstreinte, "DD/MM/YYYY HH:mm").utc(true);

            if (pointage) {
                $ctrl.selectedRaportLigneIndex = $ctrl.pointagesList.indexOf(pointage);
                $ctrl.selectedRaportLigneAstreinteIndex = $ctrl.pointagesList[$ctrl.selectedRaportLigneIndex].ListRapportLigneAstreintes.indexOf(raportligneAstreinte);
            }

            $("#changeAstreinteDateModal" + $ctrl.selectedRaportLigneIndex).modal('toggle');
            if ($ctrl.selectedRaportLigneAstreinte.RapportLigneAstreinteId >= 0) {
                pointage.IsUpdated = true;
            }
        }

        function handleDeleteSortieAstreinte(index, pointage) {
            if (pointage.RapportLigneId !== 0) {
                pointage.IsUpdated = true;
            }
            if (pointage.ListRapportLigneAstreintes[index]) {
                pointage.ListRapportLigneAstreintes[index].IsDeleted = true;
            }
            checkAllAstreintes();
        }

        function actionDelete() {
            var pointageIndex = PointageHelperService.getPointageIndex();
            if (pointageIndex.pointage.Day.Display) {
                var cloneDay = angular.copy(pointageIndex.pointage.Day);
                var emptyDay = { Day: cloneDay };
                if (pointageIndex.index + 1 < $ctrl.pointagesList.length) {
                    for (var i = pointageIndex.index + 1; i < $ctrl.pointagesList.length; i++) {
                        if (!$ctrl.pointagesList[i].IsDeleted) {
                            if (!$ctrl.pointagesList[i].Day.Display) {
                                $ctrl.pointagesList[i].Day = cloneDay;
                                if (!pointageIndex.pointage.IsCreated) {
                                    pointageIndex.pointage.IsUpdated = false;
                                    pointageIndex.pointage.IsDeleted = true;
                                }
                                else {
                                    $ctrl.pointagesList.splice(pointageIndex.index, 1);
                                }
                            }
                            else {
                                if (!pointageIndex.pointage.IsCreated) {
                                    $ctrl.pointagesList.splice(pointageIndex.index, 0, emptyDay);
                                    pointageIndex.pointage.IsUpdated = false;
                                    pointageIndex.pointage.IsDeleted = true;
                                }
                                else {
                                    $ctrl.pointagesList.splice(pointageIndex.index, 1, emptyDay);
                                }
                            }
                            break;
                        }
                    }
                }
                else {
                    if (!pointageIndex.pointage.IsCreated) {
                        $ctrl.pointagesList.splice(pointageIndex.index, 0, emptyDay);
                        pointageIndex.pointage.IsUpdated = false;
                        pointageIndex.pointage.IsDeleted = true;
                    }
                    else {
                        $ctrl.pointagesList.splice(pointageIndex.index, 1, emptyDay);
                    }
                }
            }
            else {
                if (!pointageIndex.pointage.IsCreated) {
                    pointageIndex.pointage.IsUpdated = false;
                    pointageIndex.pointage.IsDeleted = true;
                }
                else {
                    $ctrl.pointagesList.splice(pointageIndex.index, 1);
                }
            }
            if ($ctrl.isUserFes) {
                var wekNumber = moment(pointageIndex.pointage.Day.Date).isoWeek();
                var indexWeek = weekErrone.indexOf(wekNumber, 0);
                if (indexWeek !== -1) {
                    weekErrone.splice(indexWeek, 1);
                }
                var indexDay = jourErrone.indexOf(pointageIndex.pointage.Day.Date, 0);
                if (indexDay !== -1) {
                    jourErrone.splice(indexDay, 1);
                }
            }
        }

        function reloadTotaux() {
            refreshTotalHeureNormale();
            refreshTotalHeureAbsence();
            refreshTotalHeureMajoration();
        }

        function handleDisplayTotalHeureMajoration(ListMajorations) {
            var list = ListMajorations.filter(x => !x.IsDeleted);
            return list.length > 0;
        }

        $ctrl.totalMajorationCi = function (ListMajorations) {
            var list = ListMajorations.filter(x => !x.IsDeleted);
            var total = 0;
            list.forEach(function (maj) { total += maj.HeureMajoration; });
            return total;
        };

        function refreshTotalHeureNormale() {
            var listPointage = $ctrl.pointagesList.filter(p => !p.IsDeleted);
            var sumNormale = 0;
            if ($ctrl.isIac && $ctrl.isUserFes) {
                for (var i = 0; i < listPointage.length; i++) {
                    if (listPointage[i].HeureNormaleIac) {
                        sumNormale += isNaN(parseFloat(listPointage[i].HeureNormaleIac)) ? 0 : parseFloat(listPointage[i].HeureNormaleIac);
                    }
                }
                $ctrl.TotalHeureNormale = Math.trunc(sumNormale * 100) / 100;
            }
            else {
                for (var j = 0; j < listPointage.length; j++) {
                    if (listPointage[j].HeureNormale) {
                        sumNormale += isNaN(parseFloat(listPointage[j].HeureNormale)) ? 0 : parseFloat(listPointage[j].HeureNormale);
                    }
                }
                $ctrl.TotalHeureNormale = Math.trunc(sumNormale * 100) / 100;
            }
        }

        function refreshTotalHeureAbsence() {
            var sumAbsence = 0;
            var listPointage = $ctrl.pointagesList.filter(p => !p.IsDeleted);
            if ($ctrl.isIac && $ctrl.isUserFes) {
                for (var i = 0; i < listPointage.length; i++) {
                    if (listPointage[i].HeureAbsenceIac) {
                        sumAbsence += isNaN(parseFloat(listPointage[i].HeureAbsenceIac)) ? 0 : parseFloat(listPointage[i].HeureAbsenceIac);
                    }
                }
                $ctrl.TotalHeureAbsence = Math.trunc(sumAbsence * 100) / 100;
            }
            else {
                for (var j = 0; j < listPointage.length; j++) {
                    if (listPointage[j].HeureAbsence) {
                        sumAbsence += isNaN(parseFloat(listPointage[j].HeureAbsence)) ? 0 : parseFloat(listPointage[j].HeureAbsence);
                    }
                }
                $ctrl.TotalHeureAbsence = Math.trunc(sumAbsence * 100) / 100;
            }
        }

        function refreshTotalHeureMajoration() {
            var sumMajoration = 0;
            var listPointage = $ctrl.pointagesList.filter(p => !p.IsDeleted);
            if (!$ctrl.isUserFes) {
                for (var i = 0; i < listPointage.length; i++) {
                    if (listPointage[i].HeureMajoration) {
                        sumMajoration += isNaN(parseFloat(listPointage[i].HeureMajoration)) ? 0 : parseFloat(listPointage[i].HeureMajoration);
                    }
                }
                $ctrl.TotalHeureMajoration = Math.trunc(sumMajoration * 100) / 100;
            }
            else {
                for (var j = 0; j < listPointage.length; j++) {
                    if (listPointage[j].ListRapportLigneMajorations) {
                        var list = listPointage[j].ListRapportLigneMajorations.filter(x => !x.IsDeleted);
                        for (var k = 0; k < list.length; k++)
                            if (list[k].HeureMajoration) {
                                sumMajoration += isNaN(parseFloat(list[k].HeureMajoration)) ? 0 : parseFloat(list[k].HeureMajoration);
                            }
                    }
                }
                $ctrl.TotalHeureMajoration = Math.trunc(sumMajoration * 100) / 100;
            }
        }

        function loadErreurs(pointage) {
            $ctrl.lstErreurs = pointage.ListErreurs;
        }

        function checkTotalTacheMoreThanOne(pointage) {
            var cpt = 0;
            for (var i = 0; i < pointage.ListRapportLigneTaches.length; i++) {
                if (!pointage.ListRapportLigneTaches[i].IsDeleted) {
                    cpt++;
                }
            }
            return cpt > 1;
        }

        /**
         * Retourne true si un des pointages gérés par ce controller contient une erreur ou plus
         * @returns{any} un booléen
         * */
        function anyErreur() {
            if ($ctrl.pointagesList.length === 0) {
                return false;
            }

            let anyErreur = $ctrl.pointagesList.some(pointage => {
                if (!pointage.ListErreurs) {
                    return false;
                }

                return pointage.ListErreurs.length > 0;
            });
            return anyErreur;
        }

        //Fonction d'initialisation des données de la picklist 
        function showPickList(val, pointage) {
            $ctrl.element = document.activeElement;
            if (pointage) {
                PointageHelperService.setPointage(pointage);
            }
            $ctrl.apiController = val;
            // {text} / {societeId} / {ciId} / {groupeId} / {materielId}
            var basePrimeControllerUrl = '/api/' + val + '/SearchLight/?societeId={0}&ciId={1}&groupeId={2}&materielId={3}';
            switch (val) {
                case "CodeAbsence":
                case "CodeDeplacement":
                case "CodeZoneDeplacement":
                    if (pointage.Personnel) {
                        basePrimeControllerUrl = String.format(basePrimeControllerUrl, null, pointage.CiId, null, null) + '&' + getReferentielRhUrl();
                    }
                    break;
                default:
                    basePrimeControllerUrl = '/api/' + val + '/SearchLight/';
                    break;
            }
            //Remplace les paramètre par null pour les informations qui n'ont pas été remplacée auparavant
            //ex : le personnel du rapport n'a pas encore été sélectionné
            basePrimeControllerUrl = String.format(basePrimeControllerUrl, null, null, null, null);
            return basePrimeControllerUrl;
        }

        function disableChangeCalendar(disable) {
            $scope.$emit('disableChangeCalendarEvent', {
                disableChangeCalendar: disable
            });
        }

        function handleCheckAstreinte(datedebut, datefin) {
            if (datedebut && datefin) {
                var superieureCondition = handleSuperieureCondition(datedebut, datefin);
                var shevalCondition = handleShevalCondition(datedebut, datefin);

                return superieureCondition && shevalCondition;
            }
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
                if (heureDateDebutAstreinte < heurDebutNuit && heureDateDebutAstreinte >= heureFinNuit &&
                    (heureDateFinAstreinte > heurDebutNuit && heureDateFinAstreinte <= minuteBeforeMinuit || heureDateFinAstreinte >= heureMinuit && heureDateFinAstreinte <= heureFinNuit) ||
                    (heureDateDebutAstreinte >= heurDebutNuit && heureDateDebutAstreinte <= minuteBeforeMinuit || heureDateDebutAstreinte >= heureMinuit && heureDateDebutAstreinte < heureFinNuit) &&
                    heureDateFinAstreinte > heureFinNuit && heureDateFinAstreinte <= heurDebutNuit) {
                    return false;
                }
            }
            return true;
        }

        function checkAllAstreintes() {
            var SuperieureError = false;
            var ShevalError = false;
            $ctrl.pointagesList.forEach(function (pointage) {
                pointage.ListRapportLigneAstreintes.forEach(function (ligneAstreinte) {
                    var superieureCondition = handleSuperieureCondition(ligneAstreinte.DateDebutAstreinte, ligneAstreinte.DateFinAstreinte);
                    var shevalCondition = handleShevalCondition(ligneAstreinte.DateDebutAstreinte, ligneAstreinte.DateFinAstreinte);

                    if (!superieureCondition) {
                        SuperieureError = true;
                    }
                    if (!shevalCondition) {
                        ShevalError = true;
                    }
                    if (!superieureCondition && !shevalCondition) {
                        $ctrl.IsSuperieureError = true;
                        $ctrl.IsShevalError = true;
                        $ctrl.parentCtrl.saveAstreintesEnable = !($ctrl.IsSuperieureError || $ctrl.IsShevalError);
                        return true;
                    }
                });
            });

            $ctrl.IsSuperieureError = SuperieureError;
            $ctrl.IsShevalError = ShevalError;
            $ctrl.parentCtrl.saveAstreintesEnable = !($ctrl.IsSuperieureError || $ctrl.IsShevalError);

            return $ctrl.IsSuperieureError || $ctrl.IsShevalError;
        }
    }
})();
