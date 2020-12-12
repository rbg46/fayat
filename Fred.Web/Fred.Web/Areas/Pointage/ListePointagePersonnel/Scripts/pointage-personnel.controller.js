(function (angular) {
    'use strict';

    angular.module('Fred').controller('PointagePersonnelController', PointagePersonnelController);

    PointagePersonnelController.$inject = ['UserService', '$scope', '$q', '$document', 'Notify', 'PointagePersonnelService', 'PersonnelPickerService', 'ProgressBar', 'DatePickerService', 'fredSubscribeService', '$window', 'favorisService', 'authorizationService'];

    function PointagePersonnelController(UserService, $scope, $q, $document, Notify, PointagePersonnelService, PersonnelPickerService, ProgressBar, DatePickerService, fredSubscribeService, $window, favorisService, authorizationService) {

        // assignation de la valeur du scope au controller pour les templates non mis à jour
        var $ctrl = this;
        var bodyRef = angular.element($document[0].body);
        const codeSOMOPA = "0143";
        const RapportLigneStatutVerrouille = 5;

        // méthodes exposées
        $ctrl.favoriId = 0;

        UserService.getCurrentUser().then(function (user) {
            $ctrl.isSomopa = user.Personnel.Societe.Code.trim() === codeSOMOPA ? true : false;
        });

        $ctrl.displayBanner = false;
        $ctrl.personnel = null;
        $ctrl.pointagesList = [];
        $ctrl.typeExport = { excel: 0, pdf: 1 };
        $ctrl.handleClickSave = handleClickSave;
        $ctrl.handleClickCancel = handleClickCancel;
        $ctrl.handleExport = handleExport;
        $ctrl.selectedDate = null;
        $ctrl.IsGSP = false;
        $ctrl.isWeek = false;
        $ctrl.handleChangeWeek = handleChangeWeek;
        $ctrl.handleChangeMonth = handleChangeMonth;
        $ctrl.setIsWeekPeriode = setIsWeekPeriode;
        $ctrl.handleDefaultPointage = handleDefaultPointage;
        $ctrl.handleChangeCalendarMonth = handleChangeCalendarMonth;
        //////////////////////////////////////////////////////////////////
        // Déclaration des fonctions publiques                          //
        //////////////////////////////////////////////////////////////////
        $ctrl.week = null;
        $ctrl.resources = resources;
        $ctrl.actionToggleSidebar = actionToggleSidebar;
        $ctrl.userOrganisationId = 0;
        $ctrl.saveEnable = true;
        $ctrl.saveAstreintesEnable = true;
        $ctrl.currentWeek = new Date();
        $ctrl.currentMonth = new Date();
        $ctrl.hideForNow = true; // Masquer l'element pour le moment
        $ctrl.disableChangeCalendar = false;
        $ctrl.getFilterOrFavoris = getFilterOrFavoris;
        $ctrl.addFilter2Favoris = addFilter2Favoris;

        /**
         * Initialisation du controller.
         * @param {integer} favoriId Identifiant du favori
         * @param {integer} personnelId Identifiant du personnel
         * @param {Date} periode Premier jour de la période sélectionnée
         */
        $ctrl.init = function (favoriId, personnelId, periode) {
            $ctrl.permissionKeys = PERMISSION_KEYS;
            $ctrl.globalReadOnly = authorizationService.getRights($ctrl.permissionKeys.AffichageMenuPointagePersonnelIndex).isReadOnly;
            fredSubscribeService.subscribe({ eventName: 'pointage-personnel-ask-save', callback: handleClickSave });
            $scope.$on('changePeriode', function () { chargeListPointages(); });
            $scope.$on('changePersonnel', function () { chargeListPointages(); });
            $scope.$on('openPointage', function () { chargePointage(); });
            $scope.$on('list.total.ci', function (event, data) {
                $ctrl.PointageList = data.pointagelist;
                chargePointageAllCi($ctrl.PointageList);
            });
            $scope.$on('validatePointage', function () { updatePointage(); });
            $scope.$on('disableChangeCalendarEvent', function (event, data) {
                $ctrl.disableChangeCalendar = data.disableChangeCalendar;
            });
            initUserRole();

            UserService.getCurrentUser().then(function (user) {
                $ctrl.userOrganisationId = user.Personnel.Societe.Organisation.OrganisationId;
            });

            getFilterOrFavoris(favoriId, personnelId, periode);
        };

        function initUserRole() {
            PointagePersonnelService.IsGSP()
                .then(function (value) { $ctrl.IsGSP = value.data; });
        }

        function chargeListPointages() {
            $scope.$broadcast('chargeListPointages');
        }

        function chargePointage() {
            $scope.$broadcast('chargePointage');

        }
        function chargePointageAllCi(PointageList) {
            $scope.$broadcast('list.total.ci.show', { listPointages: PointageList });
        }

        function updatePointage() {
            $scope.$broadcast('updatePointage');
        }

        function handleClickSave() {
            if ($ctrl.saveEnable) {
                $ctrl.saveEnable = false;
                ProgressBar.start();
                var list = [];
                for (var i = 0; i < $ctrl.pointagesList.length; i++) {
                    if ($ctrl.pointagesList[i].PointageId !== undefined
                        && $ctrl.pointagesList[i].CiId !== undefined
                        && $ctrl.pointagesList[i].DatePointage !== undefined) {
                        $ctrl.pointagesList[i].Ci = null;
                        $ctrl.pointagesList[i].RapportLigneStatutId = RapportLigneStatutVerrouille;
                        list.push($ctrl.pointagesList[i]);
                    }
                }
                if (list && list.length > 0) {
                    if ($ctrl.isSomopa) {
                        actionCheckListePointages(list);
                    }
                    else {
                        actionSave(list);
                    }
                } else {
                    ProgressBar.complete();
                    Notify.error('Aucune ligne à enregistrer');
                    $ctrl.saveEnable = true;
                }
            }
        }

        function handleClickCancel() {
            $scope.$emit('changePersonnel');
        }

        function actionSave(list) {
            PointagePersonnelService.Save(list)
                .then(function (result) { actionLoadSuccessSave(result); })
                .catch(actionLoadErrorSave)
                .finally(actionEndSave);
        }

        function actionLoadSuccessSave(result) {
            if (result && result.data && result.data.Errors.length > 0) {
                angular.forEach(result.data.Errors, function (val, key) {
                    Notify.error(val);
                });
            }
            else {
                Notify.message('Enregistrement effectué avec succès.');
                PointagePersonnelService.GetPersonnel($ctrl.personnel.PersonnelId)
                    .then(function (value) { $ctrl.personnel = value.data; PersonnelPickerService.setPersonnel($ctrl.personnel); chargeListPointages(); })
                    .catch(Notify.defaultError);
            }
        }

        function actionLoadErrorSave(error) {
            if (error.data.ExceptionMessage) {
                Notify.error(error.data.ExceptionMessage);
            } else if (error.data) {
                Notify.error(error.data);
            }
            else {
                Notify.error('Une erreur serveur est survenue.');
            }
        }

        function actionEndSave() {
            ProgressBar.complete();
            $ctrl.saveEnable = true;
        }

        function actionCheckListePointages(list) {
            PointagePersonnelService.CheckListePointages(list)
                .then(function (value) { checkListPointages(list, value.data); })
                .catch(actionLoadErrorSave)
                .finally(actionEndSave);
        }

        function checkListPointages(listToCheck, listChecked) {
            var anyError = false;
            for (var i = 0; i < listToCheck.length; i++) {
                listToCheck[i].ListErreurs = listChecked[i].ListErreurs;
                if (listToCheck[i].ListErreurs.length !== 0) {
                    anyError = true;
                }
            }
            if (anyError) {
                Notify.error("Echec de l'enregistrement, veuillez corriger les erreurs.");
            }
            else {
                actionSave(listToCheck);
            }
        }

        /*
         * @description Gestion des exports Excel et PDF
         */
        function handleExport(typeExport) {
            $q.when()
                .then(ProgressBar.start)
                .then(actionCheckBeforeExport)
                .then(function (response) {
                    if (response) {
                        PointagePersonnelService.GetPointagePersonnelExport(PersonnelPickerService.getPersonnel().PersonnelId, DatePickerService.getPeriode(), typeExport);
                    }
                    else {
                        Notify.error("Il n'y a aucun pointage à exporter.");
                    }
                })
                .then(ProgressBar.complete);
        }

        /**
         * Vérifie s'il existe au moins un pointage avant d'exporter
         * @returns {any} Promise
         */
        function actionCheckBeforeExport() {
            return PointagePersonnelService.GetPointagesByPersonnelIdAndPeriode(DatePickerService.getPeriode(), PersonnelPickerService.getPersonnel() ? PersonnelPickerService.getPersonnel().PersonnelId : 0)
                .then(function (response) {
                    if (response.data && response.data.Pointages && response.data.Pointages.length > 0) {
                        return true;
                    }
                    return false;
                })
                .catch(Notify.defaultError);
        }

        function handleChangeWeek(periode) {
            if (periode && $ctrl.isWeek && !compareDates($ctrl.currentWeek, periode)) {
                DatePickerService.setPeriode(periode);
                chargeListPointages();
                $ctrl.currentWeek = periode;
                $ctrl.currentMonth = new Date();
            }
        }

        function handleChangeMonth(periode, currentMonth) {
            if (periode && !$ctrl.isWeek && !compareDates(currentMonth, periode)) {
                DatePickerService.setPeriode(periode);
                chargeListPointages();
                $ctrl.currentMonth = periode;
                $ctrl.currentWeek = new Date();
            }
        }

        function handleChangeCalendarMonth() {
            $ctrl.currentMonth = null;
            setIsWeekPeriode(false);
            handleChangeMonth($ctrl.periode, $ctrl.currentMonth);
        }

        function setIsWeekPeriode(value) {
            $ctrl.currentWeek = null;
            DatePickerService.setIsPeriodeWeek(value);
        }

        /*
         * @description Toggle du panneau latéral
         */
        function actionToggleSidebar(displayBanner) {
            if (displayBanner) {
                bodyRef.addClass('ovh');
            }
            else {
                bodyRef.removeClass('ovh');
            }
        }

        function compareDates(date1, date2) {
            if (date2 === null || date1 === null) {
                return false;
            }

            if ($ctrl.isWeek) {
                return date1.getFullYear() === date2.getFullYear()
                    && date1.getDate() === date2.getDate()
                    && date1.getMonth() === date2.getMonth();
            }

            return date1.getFullYear() === date2.getFullYear()
                && date1.getMonth() === date2.getMonth();

        }

        function addFilter2Favoris() {
            var filter = {
                Periode: $ctrl.periode,
                PersonnelId: $ctrl.personnel.PersonnelId
            };

            var url = $window.location.pathname;
            if ($ctrl.favoriId !== 0) {
                url = $window.location.pathname.slice(0, $window.location.pathname.lastIndexOf("/"));
            }
            favorisService.initializeAndOpenModal("ListePointagePersonnel", url, filter);
        }

        function getFilterOrFavoris(favoriId, personnelId, periode) {
            $ctrl.favoriId = parseInt(favoriId) || 0;

            var filter = {
                Periode: null,
                Personnel: null
            };
            if ($ctrl.favoriId !== 0) {
                favorisService.getFilterByFavoriIdOrDefault({ favoriId: $ctrl.favoriId, defaultFilter: filter })
                    .then(function (response) {
                        PointagePersonnelService.GetPersonnel(response.PersonnelId).then(function (value) {
                            $ctrl.personnel = value.data;
                            $ctrl.periode = new Date(response.Periode);
                            LoadWithFilter();
                        })
                            .catch(function () { Notify.error($scope.resources.Global_Notification_Error); });
                    })
                    .catch(function () { Notify.error($scope.resources.Global_Notification_Error); });
            } else if (personnelId && periode) {
                PointagePersonnelService.GetPersonnel(personnelId).then(function (value) {
                    $ctrl.personnel = value.data;
                    var date = periode.split('-');
                    var month = date[0] - 1; // en JS, 0 = janvier, 1 = février, etc...      
                    $ctrl.periode = new Date(date[1], month, 1);
                    LoadWithFilter();
                })
                    .catch(function () { Notify.error($scope.resources.Global_Notification_Error); });
            } else if (sessionStorage.getItem('pointagePersonnelFilter') !== null) {
                $ctrl.personnel = JSON.parse(sessionStorage.getItem('pointagePersonnelFilter')).Personnel;
                $ctrl.periode = new Date(JSON.parse(sessionStorage.getItem('pointagePersonnelFilter')).Periode);
                LoadWithFilter();
            } else {
                $ctrl.periode = new Date();
            }
        }

        function LoadWithFilter() {
            PersonnelPickerService.setPersonnel($ctrl.personnel);
            DatePickerService.setPeriode($ctrl.periode);
            chargeListPointages();
        }

        function handleDefaultPointage() {
            $scope.$broadcast('handleDefaultPointage');
        }
    }
}(angular));
