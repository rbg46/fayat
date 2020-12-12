(function (angular) {
    'use strict';

    angular.module('Fred').controller('DatesClotureComptableController', DatesClotureComptableController);

    DatesClotureComptableController.$inject = [
        '$scope',
        'Notify',
        'DatesClotureComptableDataService',
        'DatesClotureComptableService',
        'ProgressBar',
        'PermissionsService'];

    function DatesClotureComptableController(
        $scope, Notify, DatesClotureComptableDataService, DatesClotureComptableService, ProgressBar, PermissionsService) {
        var $ctrl = this;

        // Instanciation Objet Ressources
        $scope.resources = resources;
        $ctrl.resources = resources;
        $ctrl.selectedCi = null;

        // Valeurs du formulaire
        $ctrl.currentYear = new Date().getFullYear();
        $ctrl.minAnnee = $ctrl.currentYear;

        $ctrl.isBusy = false;
        $ctrl.dataLoaded = false;
        $ctrl.showMessageInfo = true;

        $ctrl.year = [];
        init();

        function init() {
            $scope.$on('datesClotureComptableRowComponent.dateChange', function (event, month) {
                save(month);
            });
        }

        /**
         * Reload datas when selected year changed
         */
        $ctrl.changeYear = function () {
            if ($ctrl.selectedCi !== null) {
                getData();
            }
        };

        /**
         * Get the list of CI authorized for the current user
         * @param {string} val represents the controller to call
         * @returns {string} List of CI authorizes for the current user
         */
        $ctrl.clickOnLookup = function (val) {
            var basePrimeControllerUrl = '/api/' + val + '/SearchLight/?page={1}';
            return basePrimeControllerUrl;
        };

        /**
         * Manage the selection of a CI
         */
        $ctrl.ciSelectedChange = function () {
            // Call the service to get authorizations
            PermissionsService.getContextualAuthorization($ctrl.selectedCi.CiId, $ctrl.resources.permisssionKeyCloseDateButton)
                .success(changeCloseAccountPeriodPermission)
                .error(function () { Notify.error(resources.err_Authorizations); })
                .finally(getData);
        };

        function changeCloseAccountPeriodPermission(modeAuthorization) {
            PermissionsService.registerContextualPermission($ctrl.resources.permisssionKeyCloseDateButton, modeAuthorization);
        }

        /**
         * Get the accounting close dates
         */
        function getData() {
            if (!$ctrl.selectedCi) {
                Notify.error(resources.ChantierObligatoire_lb);
                return;
            }
            if (!$ctrl.isBusy) {
                $ctrl.isBusy = true;
                $ctrl.showMessageInfo = false;
                ProgressBar.start();
                //check date invalide
                if ($ctrl.currentYear === undefined) {
                    $ctrl.currentYear = new Date().getFullYear();
                }
                $ctrl.year = [];
                $scope.$broadcast('DatesClotureComptableController.clearData');
                DatesClotureComptableService.GetYearAndPreviousNextMonths($ctrl.selectedCi.CiId, $ctrl.currentYear)
                    .then(getByCIAndYearOnSucess)
                    .catch(getByCIAndYearOnError)
                    .finally(getByCIAndYearFinally);
            }
        }

        /**
         * Clear the lookup and all screen fields
         */
        $ctrl.clearLookup = function () {
            $ctrl.selectedCi = null;
            refreshCalendars();
        };

        function getByCIAndYearOnSucess(response) {
            setYearCalendar(response.data);
            $ctrl.dataLoaded = true;
        }

        function getByCIAndYearOnError(error) {
            Notify.error(resources.Data_Load_Fail);
        }

        function getByCIAndYearFinally() {
            $ctrl.isBusy = false;
            ProgressBar.complete();
        }

        function setYearCalendar(serverData) {
            var decemberPreviousYear = DatesClotureComptableDataService.getMonth(serverData, $ctrl.selectedCi.CiId, $ctrl.currentYear - 1, 12);
            var january = DatesClotureComptableDataService.getMonth(serverData, $ctrl.selectedCi.CiId, $ctrl.currentYear, 1);
            var february = DatesClotureComptableDataService.getMonth(serverData, $ctrl.selectedCi.CiId, $ctrl.currentYear, 2);
            var march = DatesClotureComptableDataService.getMonth(serverData, $ctrl.selectedCi.CiId, $ctrl.currentYear, 3);
            var april = DatesClotureComptableDataService.getMonth(serverData, $ctrl.selectedCi.CiId, $ctrl.currentYear, 4);
            var may = DatesClotureComptableDataService.getMonth(serverData, $ctrl.selectedCi.CiId, $ctrl.currentYear, 5);
            var june = DatesClotureComptableDataService.getMonth(serverData, $ctrl.selectedCi.CiId, $ctrl.currentYear, 6);
            var july = DatesClotureComptableDataService.getMonth(serverData, $ctrl.selectedCi.CiId, $ctrl.currentYear, 7);
            var august = DatesClotureComptableDataService.getMonth(serverData, $ctrl.selectedCi.CiId, $ctrl.currentYear, 8);
            var september = DatesClotureComptableDataService.getMonth(serverData, $ctrl.selectedCi.CiId, $ctrl.currentYear, 9);
            var october = DatesClotureComptableDataService.getMonth(serverData, $ctrl.selectedCi.CiId, $ctrl.currentYear, 10);
            var november = DatesClotureComptableDataService.getMonth(serverData, $ctrl.selectedCi.CiId, $ctrl.currentYear, 11);
            var december = DatesClotureComptableDataService.getMonth(serverData, $ctrl.selectedCi.CiId, $ctrl.currentYear, 12);
            var januaryNextYear = DatesClotureComptableDataService.getMonth(serverData, $ctrl.selectedCi.CiId, $ctrl.currentYear + 1, 1);

            $ctrl.year.push(decemberPreviousYear);
            $ctrl.year.push(january);
            $ctrl.year.push(february);
            $ctrl.year.push(march);
            $ctrl.year.push(april);
            $ctrl.year.push(may);
            $ctrl.year.push(june);
            $ctrl.year.push(july);
            $ctrl.year.push(august);
            $ctrl.year.push(september);
            $ctrl.year.push(october);
            $ctrl.year.push(november);
            $ctrl.year.push(december);
            $ctrl.year.push(januaryNextYear);
        }

        //////////////////////////////////////////////////////////////////
        // SAUVEGARDE                                                   //
        //////////////////////////////////////////////////////////////////

        function save(month) {
            if ($ctrl.dataLoaded) {
                if (month.DatesClotureComptableId === 0) {
                    add(month);
                } else {
                    update(month);
                }
            }
        }

        //////////////////////////////////////////////////////////////////
        // CREATION                                                     //
        //////////////////////////////////////////////////////////////////

        function add(month) {
            if (!$ctrl.selectedCi) {
                Notify.error(resources.ChantierObligatoire_lb);
                return;
            }

            if (!$ctrl.isBusy) {
                $scope.$broadcast('startClosing', {});
                $ctrl.isBusy = true;
                ProgressBar.start();
                var dateClotureComptablesForServer = DatesClotureComptableDataService.formatDataForServer(month);
                DatesClotureComptableService.Add(dateClotureComptablesForServer)
                    .then(addOnSucess)
                    .catch(addOnError)
                    .finally(addFinally);
            } else {
                Notify.error(resources.ChantierObligatoire_lb);
            }
        }

        function addOnSucess(response) {
            var newDcc = response.data;
            var currentMonth = $ctrl.year.filter(function (month) {
                return month.Annee === newDcc.Annee && month.Mois === newDcc.Mois;
            });
            currentMonth[0].DatesClotureComptableId = newDcc.DatesClotureComptableId;
            Notify.message(resources.Action_Add_Success);
        }

        function addOnError(error) {
            $scope.$broadcast('endClosing', {});
            Notify.error(error.Message);
        }

        function addFinally() {
            $ctrl.isBusy = false;
            $scope.$broadcast('endClosing', {});
            ProgressBar.complete();
        }

        //////////////////////////////////////////////////////////////////
        // MISE A JOUR                                                  //
        //////////////////////////////////////////////////////////////////

        function update(month) {
            if (!$ctrl.selectedCi) {
                Notify.error(resources.ChantierObligatoire_lb);
                return;
            }
            if (!$ctrl.isBusy) {
                $scope.$broadcast('startClosing', {});
                $ctrl.isBusy = true;
                ProgressBar.start();
                var dateClotureComptablesForServer = DatesClotureComptableDataService.formatDataForServer(month);

                DatesClotureComptableService.Update(dateClotureComptablesForServer)
                    .then(updateOnSuccess)
                    .catch(updateOnError)
                    .finally(updateFinally);
            }
        }

        function updateOnSuccess(response) {
            if (response.data && response.data.DateCloture) { Notify.message(resources.Action_Add_Success); }
            else { Notify.message(resources.Action_Update_Success); }
        }

        function updateOnError(error) {
            $scope.$broadcast('endClosing', { message: 'true' });
            Notify.error(error.Message);
        }

        function updateFinally() {
            $ctrl.isBusy = false;
            $scope.$broadcast('endClosing', { message: 'true' });
            ProgressBar.complete();
        }
    }
})(angular);