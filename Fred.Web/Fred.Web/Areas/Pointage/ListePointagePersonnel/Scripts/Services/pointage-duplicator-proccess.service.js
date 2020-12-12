(function () {
    'use strict';

    angular.module('Fred').service('PointageDuplicatorProccesService', PointageDuplicatorProccesService);

    PointageDuplicatorProccesService.$inject = [
        '$q',
        'PointageListChangeDetectorService',
        'confirmDialog',
        '$uibModal',
        'fredSubscribeService',
        'ProgressBar',
        'PointagePersonnelService',
        'Notify',
        'DuplicatorTimeService',
        'PointageDuplicatorTimeService'];

    function PointageDuplicatorProccesService(
        $q,
        PointageListChangeDetectorService,
        confirmDialog,
        $uibModal,
        fredSubscribeService,
        ProgressBar,
        PointagePersonnelService,
        Notify,
        DuplicatorTimeService,
        PointageDuplicatorTimeService) {

        var isInDuplicatedProcess = false; //permet de savoir si nous somme dans le process de duplication
        var pontageHasErrorMessage = resources.Pointage_List_Duplicate_Error_Pointage_Has_Error;
        var demandeSauvegarde = resources.Pointage_List_Duplicate_Message_Save_Before_Duplicate;

        var savedPointageBeforeSave = null;

        init();

        var service = {
            startDuplicationProcess: startDuplicationProcess
        };

        return service;

        //////////////////////////////////////////////////////////////////
        // PUBLICS METHODES                                             //
        //////////////////////////////////////////////////////////////////

        function startDuplicationProcess(duplicateInfo) {
            if (duplicateInfo.pointage.IsLocked) {
                return;
            }

            var hasModification = PointageListChangeDetectorService.listHasChanged(duplicateInfo.pointagesList);

            if (hasModification) {
                executeDuplicationProcessWhenListIsModified(duplicateInfo.pointage);
            } else {
                executeDuplicationProcess(duplicateInfo.pointage);
            }
        }

        //////////////////////////////////////////////////////////////////
        // PRIVATE METHODES                                             //
        //////////////////////////////////////////////////////////////////

        // Souscription a l'evenement de fin de chargement afin de reagir si nous somme dans l'execution du process de duplication
        function init() {
            fredSubscribeService.subscribe({ eventName: 'list-finish-to-load', callback: executeDuplicationProcessAfterSave });
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////// EXECUTION LORSQUE LA LISTE COMPORTE DES MODIFICATIONS
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        function executeDuplicationProcessWhenListIsModified(pointage) {
            $q.when()
                .then(function () {
                    savedPointageBeforeSave = pointage;
                })
                .then(function () {
                    showConfirmSaveBeforeDuplicate();
                })
                .catch(manageErrorOnDuplicateWhenListIsModifiedProcess)//je gere toutes les erreurs du process
        }

        function checkPointageIsNotNew(pointage) {
            if (pointage && pointage.PointageId === 0) {
                savedPointageBeforeSave = null;
                throw new Error(demandeSauvegarde);
            }
        }

        function showConfirmSaveBeforeDuplicate() {
            return confirmDialog.confirm(resources, resources.Pointage_List_Confirm_Save_Before_Duplicate)
                .then(function () {
                    ProgressBar.start(true);
                    isInDuplicatedProcess = true;
                    fredSubscribeService.raiseEvent('pointage-personnel-ask-save', {});
                });
        }

        function manageErrorOnDuplicateWhenListIsModifiedProcess(error) {
            if (error && error.message === demandeSauvegarde) {
                confirmDialog.confirm(resources, demandeSauvegarde)
                    .then(function () {
                        fredSubscribeService.raiseEvent('pointage-personnel-ask-save', {});
                    }).catch(function () {

                    });
            }

        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////// EXECUTION LORSQUE LA LISTE VIENT D ETRE SAUVEGARDER POUR UNE DUPLICATION
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        function executeDuplicationProcessAfterSave(pointagesList) {
            if (isInDuplicatedProcess) {
                ProgressBar.complete();
                if (savedPointageBeforeSave !== 'undefined' && savedPointageBeforeSave.PointageId === 0) {
                    isInDuplicatedProcess = false;
                    var newPointageRow = pointagesList.reduce(function (prev, current) {
                        if (+current.PointageId > +prev.PointageId) {
                            return current;
                        } else {
                            return prev;
                        }
                    });
                    var newPointagesOfServer = pointagesList.filter(function (p) {
                        return p.PointageId === newPointageRow.PointageId;
                    });
                }
                else if (savedPointageBeforeSave.PointageId != 0) {
                    isInDuplicatedProcess = false;
                    var newPointagesOfServer = pointagesList.filter(function (p) {
                        return p.PointageId === savedPointageBeforeSave.PointageId;
                    });
                }
                if (newPointagesOfServer && newPointagesOfServer.length > 0 && newPointagesOfServer[0]) {
                    executeDuplicationProcess(newPointagesOfServer[0]);
                }
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////// EXECUTION LORSQUE LA LISTE COMPORTE N A PAS DE MODIFICATION
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        function executeDuplicationProcess(pointage) {
            $q.when()

                .then(function () { checkErrorsOnPointage(pointage); })// verfification des erreurs du pointage => sortie process si erreurs sur le pointage.
                .then(function () { return showDuplicationComponent(pointage); })// affichage de la popup
                .then(onClickOnDuplicate)//je lance la duplication cote server et je redemande le chargement de la page
                .then(onDuplicateProcessSuccess)//
                .catch(manageErrorOnDuplicateProcess)//je gere toutes les erreurs du process
                .finally(onDuplicateProcessEnd);
        }

        function checkErrorsOnPointage(pointage) {
            if (pointage && pointage.ListErreurs && pointage.ListErreurs.length > 0) {
                throw new Error(pontageHasErrorMessage);
            }
        }

        function showDuplicationComponent(pointage) {
            var modalInstance = $uibModal.open({
                animation: true,
                component: 'duplicatorRapportPointageModalComponent',
                resolve: {
                    resources: function () { return resources; },
                    data: getDataForDuplication(pointage)

                }
            });

            return modalInstance.result;
        }

        function getDataForDuplication(pointage) {
            var pointageDate = new Date(pointage.Day.Date);
            var nextWorkingDay = DuplicatorTimeService.getNextWorkingDay(pointageDate);
            var duplicateData = {};
            duplicateData.pointageDuplication = angular.copy(pointage);
            duplicateData.ciDuplication = angular.copy(pointage.Ci);
            duplicateData.startDate = new Date(nextWorkingDay);
            duplicateData.endDate = new Date(nextWorkingDay);
            duplicateData.title = resources.Pointage_List_Duplicate_Popup_Title;
            duplicateData.canChangeCi = !(duplicateData.pointageDuplication.HeureNormale > 0);
            return duplicateData;
        }

        function onClickOnDuplicate(resultDialog) {
            ProgressBar.start(true);

            var duplicateModel = {
                RapportLigneId: resultDialog.RapportLigneId,
                StartDate: resultDialog.startDate,
                EndDate: resultDialog.endDate,
                CiId: resultDialog.ciId
            };

            return PointagePersonnelService.duplicate(duplicateModel)
                .then(function (duplicateResult) {
                    onDuplicateSuccess(duplicateResult, resultDialog);
                });
        }

        function onDuplicateSuccess(duplicateResult, resultDialog) {
            if (duplicateResult.data.HasPartialDuplicationInDifferentZoneDeTravail) {
                Notify.warning(resources.RapportValidator_Duplication_sur_differentes_periodes);
            }

            if (duplicateResult.data.HasDatesInClosedMonth) {
                throw new Error(resources.Pointage_List_Duplicate_Error_Month_closed);
            }

            if (duplicateResult.data.DuplicationOnlyOnWeekendOrHoliday === true) {
                throw new Error(resources.Pointage_List_Duplicate_Error_Duplication_Only_OnWeekend);
            }

            if (duplicateResult.data.PersonnelIsInactiveInPeriode === true) {
                throw new Error(resources.Pointage_List_Duplicate_Error_Duplication_Personnel_Inactif);
            }

            if (duplicateResult.data.InterimaireDuplicationState === InterimaireDuplicationState.NothingDayDuplicate) {
                throw new Error(resources.Pointage_PointageController_message_hors_contrat);
            }
            // Rechargement de la page si la duplication concerne le mois courrant.
            if (PointageDuplicatorTimeService.monthIsImpactedByDuplication(resultDialog)) {
                //je redemande le chargement de la page.
                fredSubscribeService.raiseEvent('pointage-duplicator-process-ask-load', {});
            }

            if (duplicateResult.data.InterimaireDuplicationState === InterimaireDuplicationState.PartialDuplicate) {
                throw new Error(resources.Pointage_PointageController_message_partial_contrat_actif);
            }
        }

        function onDuplicateProcessSuccess() {
            Notify.message(resources.Pointage_List_Duplicate_Success);
        }

        function manageErrorOnDuplicateProcess(error) {
            if (error && error.message === pontageHasErrorMessage) {
                Notify.error(error.message);
            }
            else if (error && error.message === resources.Pointage_List_Duplicate_Error_Month_closed) {
                Notify.error(error.message);
            }
            else if (error && error.message === resources.Pointage_List_Duplicate_Error_Duplication_Only_OnWeekend) {
                Notify.error(error.message);
            }
            else if (error && error.message === resources.Pointage_List_Duplicate_Error_Duplication_Personnel_Inactif) {
                Notify.error(error.message);
            }
            else if (error && error.message === resources.Pointage_PointageController_message_hors_contrat) {
                Notify.error(error.message);
            }
            else if (error && error.message === resources.Pointage_PointageController_message_partial_contrat_actif) {
                Notify.warning(error.message);
            }
            else if (error && error.duplicatedDialogClosed === true) {
                //fermeture du dialog sans lancement de la duplication => je ne fait rien
            }
            else if (error && error === "backdrop click") {
                //fermeture du dialog sans lancement de la duplication => je ne fait rien
            }
            else {
                Notify.error(resources.Global_Notification_Error);
            }
        }

        function onDuplicateProcessEnd() {
            savedPointageBeforeSave = null;
            ProgressBar.complete();
        }

    }
})();
