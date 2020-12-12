(function () {
    'use strict';

    angular.module('Fred').service('RapportDuplicatorProccesService', RapportDuplicatorProccesService);

    RapportDuplicatorProccesService.$inject = [
        '$q',
        'RapportChangeDetectorService',
        'confirmDialog',
        '$uibModal',
        'ProgressBar',
        'RapportService',
        'Notify',
        'ModelStateErrorManager',
        'DuplicatorTimeService',
        'authorizationService'];

    function RapportDuplicatorProccesService(
        $q,
        RapportChangeDetectorService,
        confirmDialog,
        $uibModal,
        ProgressBar,
        RapportService,
        Notify,
        ModelStateErrorManager,
        DuplicatorTimeService,
        authorizationService) {
        var pontageHasErrorMessage = resources.Pointage_List_Duplicate_Error_Pointage_Has_Error;

        var service = {
            canDuplicateEnMasse: canDuplicateEnMasse,
            mustSaveModification: mustSaveModification,
            startDuplicationProcess: startDuplicationProcess
        };

        return service;

        //////////////////////////////////////////////////////////////////
        // PUBLICS METHODES                                             //
        //////////////////////////////////////////////////////////////////


        function canDuplicateEnMasse(isRolePaie) {
            // J'ai soit la permission de dupliquer soit le role paie.
            if (isRolePaie === true) {
                return true;
            }
            var permission = authorizationService.getPermission('button.show.duplicate.rapport.detail');
            if (permission !== null) {
                return true;
            }
            return false;
        }

        function mustSaveModification(duplicateInfo) {
            var hasModification = RapportChangeDetectorService.listHasChanged(duplicateInfo.rapport.ListLignes);

            if (hasModification) {
                return showSaveDialog(duplicateInfo.rapport);
            } else {

                return $q.resolve({ askSave: false });
            }
        }

        function startDuplicationProcess(duplicateInfo) {
            //ProgressBar.complete();
            return $q.when()
                .then(function () { checkErrorsOnPointages(duplicateInfo.rapport); })// verfification des erreurs du pointage => sortie process si erreurs sur le pointage.
                .then(function () { return showDuplicationComponent(duplicateInfo.rapport); })
                .then(onClickOnDuplicate)
                .then(exitProcess);

        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////// EXECUTION LORSQUE LA LISTE COMPORTE DES MODIFICATIONS
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        function showSaveDialog(rapport) {
            return confirmDialog.confirm(resources, resources.Pointage_List_Confirm_Save_Before_Duplicate)
                .then(function () {
                    return $q.resolve({ askSave: true });
                });
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////// EXECUTION LORSQUE LA LISTE N A PAS DE MODIFICATION
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        function checkErrorsOnPointages(rapport) {
            for (var i = 0; i < rapport.ListLignes.length; i++) {
                var pointage = rapport.ListLignes[i];
                if (pointage && pointage.ListErreurs && pointage.ListErreurs.length > 0) {
                    //ici je propage une erreur pour que le controlleur detect l'erreur et sorte du process
                    Notify.error(pontageHasErrorMessage);
                    throw new Error(pontageHasErrorMessage);
                }
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

        function getDataForDuplication(rapport) {
            var rapportDate = getDateWithoutTime(new Date(rapport.DateChantier));
            var nextWorkingDay = DuplicatorTimeService.getNextWorkingDay(rapportDate);
            var duplicateData = {};
            duplicateData.title = resources.Rapport_ModalDuplication_Title;
            duplicateData.rapportDuplication = angular.copy(rapport);
            duplicateData.ciDuplication = angular.copy(rapport.CI);
            duplicateData.startDate = new Date(nextWorkingDay);
            duplicateData.endDate = new Date(nextWorkingDay);
            duplicateData.canChangeCi = false;
            return duplicateData;
        }

        function onClickOnDuplicate(resultDialog) {
            ProgressBar.start(true);

            var duplicateModel = {
                RapportId: resultDialog.RapportId,
                StartDate: getDateWithoutTime(resultDialog.startDate),
                EndDate: getDateWithoutTime(resultDialog.endDate)
            };

            return RapportService.Duplicate(duplicateModel)
                .then(function (duplicateResult) {
                    onDuplicateSuccess(duplicateResult);
                })
                .catch(onDuplicateError)
                .finally(onDuplicateFinally);
        }

        function onDuplicateSuccess(duplicateResult) {
            if (duplicateResult.data.HasPartialDuplicationInDifferentZoneDeTravail) {
                Notify.warning(resources.RapportValidator_Duplication_sur_differentes_periodes);
            }
            else {
                Notify.message(resources.Pointage_List_Duplicate_Success);
            }
        }

        function onDuplicateError(error) {

            var hasAllDuplicationInDifferentZoneDeTravail = ModelStateErrorManager.hasError(error, "HasAllDuplicationInDifferentZoneDeTravail");
            if (hasAllDuplicationInDifferentZoneDeTravail) {
                var errorHasAllDuplicationInDifferentZoneDeTravail = ModelStateErrorManager.getError(error, "HasAllDuplicationInDifferentZoneDeTravail");

                Notify.error(errorHasAllDuplicationInDifferentZoneDeTravail.firstError);
                throw new Error(errorHasAllDuplicationInDifferentZoneDeTravail.firstError);
            }

            var hasErrorHasDatesInClosedMonth = ModelStateErrorManager.hasError(error, "HasDatesInClosedMonth");
            if (hasErrorHasDatesInClosedMonth) {
                var errorHasDatesInClosedMonth = ModelStateErrorManager.getError(error, "HasDatesInClosedMonth");

                Notify.error(errorHasDatesInClosedMonth.firstError);
                throw new Error(errorHasDatesInClosedMonth.firstError);
            }
            var hasInterimaireWithoutContrat = ModelStateErrorManager.hasError(error, "HasInterimaireWithoutContrat");
            if (hasInterimaireWithoutContrat) {
                var errorInterimaireWithoutContrat = ModelStateErrorManager.getError(error, "HasInterimaireWithoutContrat");

                Notify.error(errorInterimaireWithoutContrat.firstError);
                throw new Error(errorInterimaireWithoutContrat.firstError);
            }
            var duplicationOnlyOnWeekendOrHoliday = ModelStateErrorManager.hasError(error, "DuplicationOnlyOnWeekendOrHoliday");
            if (duplicationOnlyOnWeekendOrHoliday) {
                var errorDuplicationOnlyOnWeekendOrHoliday = ModelStateErrorManager.getError(error, "DuplicationOnlyOnWeekendOrHoliday");

                Notify.error(errorDuplicationOnlyOnWeekendOrHoliday.firstError);
                throw new Error(errorDuplicationOnlyOnWeekendOrHoliday.firstError);
            }
            var hasPersonnelsInactivesOnPeriode = ModelStateErrorManager.hasError(error, "HasPersonnelsInactivesOnPeriode");
            if (hasPersonnelsInactivesOnPeriode) {
                var errorHasPersonnelsInactivesOnPeriode = ModelStateErrorManager.getError(error, "HasPersonnelsInactivesOnPeriode");

                Notify.error(errorHasPersonnelsInactivesOnPeriode.firstError);
                throw new Error(errorHasPersonnelsInactivesOnPeriode.firstError);
            }

            Notify.error(resources.Global_Notification_Error);
            //ici je repropage l'erreur pour que le controlleur detect l'erreur et sorte du process
            throw new Error(error);
        }

        function onDuplicateFinally() {
            ProgressBar.complete();
        }
        function exitProcess() {
            throw new Error("Tous c'est bien passé, je lance un erreur pour que rien d'autre ne s'execute");
        }

        // retourne la date avec un time à 00:00:00:00
        function getDateWithoutTime(dateToConvert) {
            if (!dateToConvert)
                return null;

            var dateTime = dateToConvert;

            // récupération de la date pour les objets de types moment en date
            if (dateToConvert instanceof moment)
                dateTime = dateTime.toDate();

            return new Date(Date.UTC(dateTime.getFullYear(), dateTime.getMonth(), dateTime.getDate(), 0, 0, 0, 0));
        }
    }
})();
