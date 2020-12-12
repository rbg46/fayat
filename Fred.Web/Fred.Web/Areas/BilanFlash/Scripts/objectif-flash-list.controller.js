(function (angular) {
    'use strict';

    angular.module('Fred')
        .controller('ObjectifFlashListController', ObjectifFlashListController);

    ObjectifFlashListController.$inject = ['ProgressBar', 'Notify', 'ObjectifFlashService', '$uibModal', '$timeout', 'ModelStateErrorManager' ];

    function ObjectifFlashListController(ProgressBar, Notify, ObjectifFlashService, $uibModal, $timeout, ModelStateErrorManager) {

        var $ctrl = this;
        $ctrl.resources = resources;
        $ctrl.objectifFlashList = [];
        $ctrl.paging = { page: 1, pageSize: 20, hasMorePage: false };
        $ctrl.isBusy = false;

        $ctrl.$onInit = $onInit;
        $ctrl.handleLookupSelectionCI = handleLookupSelectionCI;
        $ctrl.handleLookupDeletionCI = handleLookupDeletionCI;
        $ctrl.handleChangeDate = handleChangeDate;
        $ctrl.handleDisplayObjectifFlashClosed = handleDisplayObjectifFlashClosed;
        $ctrl.handleRedirectionNewObjectifFlash = handleRedirectionNewObjectifFlash;
        $ctrl.handleDeleteObjectifFlash = handleDeleteObjectifFlash;
        $ctrl.handleDuplicateObjectifFlash = handleDuplicateObjectifFlash;
        $ctrl.handleExportObjectifFlash = handleExportObjectifFlash;
        $ctrl.handleRedirectionObjectifFlash = handleRedirectionObjectifFlash;
        $ctrl.Reload = Reload;


        var urlObjectifFlash = "/BilanFlash/BilanFlash/ObjectifFlash";


        function $onInit() {
            getFilters();

            FredToolBox.bindScrollEnd('#objectif-flash-list', actionLoadMore);
            window.onresize = Reload;
        }

        /******************** Méthodes public ********************/



        function handleLookupSelectionCI() {
            if ($ctrl.oldFilters.CI !== $ctrl.filters) {
                $ctrl.paging = { page: 1, pageSize: 20, hasMorePage: false };
                getObjectifFlashList();
            }
        }

        function handleLookupDeletionCI() {
            $ctrl.filters.CI = null;
            $ctrl.paging = { page: 1, pageSize: 20, hasMorePage: false };
            getObjectifFlashList();
        }


        function handleChangeDate() {
            if ($ctrl.oldFilters.DateDebut !== $ctrl.filters.DateDebut || $ctrl.oldFilters.DateFin !== $ctrl.filters.DateFin) {
                $ctrl.paging = { page: 1, pageSize: 20, hasMorePage: false };
                getObjectifFlashList();
            }
        }

        function handleDisplayObjectifFlashClosed() {
            $ctrl.filters.DisplayClosed = !$ctrl.filters.DisplayClosed;
            $ctrl.paging = { page: 1, pageSize: 20, hasMorePage: false };
            getObjectifFlashList();
        }

        function handleRedirectionNewObjectifFlash() {
            window.location.href = urlObjectifFlash;
        }

        function handleDeleteObjectifFlash(objectifFlash) {
            var modalInstance = $uibModal.open({
                animation: true,
                component: 'deleteObjectifFlashComponent',
                resolve: {
                    objectifFlash: function () { return objectifFlash; },
                    resources: function () { return $ctrl.resources; }
                }
            });

            // Traitement des données renvoyées par la modal
            modalInstance.result.then(function () {
                actionDeleteObjectifFlash(objectifFlash);
            });
        }

        function handleDuplicateObjectifFlash(objectifFlash){
            var modalInstance = $uibModal.open({
                animation: true,
                component: 'reportEtDuplicationObjectifFlashComponent',
                resolve: {
                    resources: function () { return $ctrl.resources; },
                    dateDebut: function () { return objectifFlash.DateDebut; },
                    dateFin: function () { return objectifFlash.DateFin; }, 
                    duplicate: function () {return true;}
                }
            });

            modalInstance.result.then(function (date) {
                actionDuplicateObjectifFlash(date, objectifFlash);
            });
        }

        function handleExportObjectifFlash(objectifFlash) {
            var modalInstance = $uibModal.open({
                animation: true,
                component: 'exportBilanFlashComponent',
                resolve: {
                    resources: function () { return $ctrl.resources; },
                    dateDebut: function () { return objectifFlash.DateDebut; },
                    dateFin: function () { return objectifFlash.DateFin; }
                }
            });

            modalInstance.result.then(function (data) {
                ProgressBar.start();
                var fileName = data.templateName + (data.isPdf ? '.pdf' : '.xlsx');
                ObjectifFlashService.ExportBilanFlash(data.templateName, objectifFlash.ObjectifFlashId, data.dateDebut, data.dateFin, data.isPdf)
                    .then(function (data) {
                        ObjectifFlashService.ExportBilanFlashDownload(data.data.id, fileName);
                    })
                    .catch(getObjectifFlashValidationErrors)
                    .finally(getObjectifFlashListOnFinally);
            });
        }

        function handleRedirectionObjectifFlash(objectifFlashId) {
            window.location.href = urlObjectifFlash + '/' + objectifFlashId;
        }

        function Reload() {
            if ($ctrl.paging.hasMorePage && !FredToolBox.hasVerticalScrollbarVisible('#objectif-flash-list')) {
                actionLoadMore();
            }
        }

        /******************** Méthodes private ********************/

        function actionLoadMore() {
            if ($ctrl.paging.hasMorePage) {
                $ctrl.paging.page++;
                getObjectifFlashList();
            }
        }

        function getFilters() {
            if (sessionStorage.getItem('bilanFlashFilter') !== null) {
                $ctrl.filters = JSON.parse(sessionStorage.getItem('bilanFlashFilter'));
                getObjectifFlashList();
            } else {
                ObjectifFlashService.GetNewObjectifFlashFilter()
                .success(getFiltersOnSuccess)
                .error(getFiltersOnError);
            }
        }

        function getFiltersOnSuccess(filters) {
            $ctrl.filters = filters;
            getObjectifFlashList();
        }

        function getFiltersOnError() {
            Notify.error($ctrl.resources.Global_Notification_Error);
        }

        function getObjectifFlashList() {
            if(!$ctrl.isBusy){
                if($ctrl.paging.page === 1){
                    $ctrl.objectifFlashList = [];
                }

                $ctrl.oldFilters = angular.copy($ctrl.filters);
                sessionStorage.setItem('bilanFlashFilter', JSON.stringify($ctrl.filters));

                $ctrl.isBusy = true;
                ProgressBar.start();
                ObjectifFlashService.getObjectifFlashList($ctrl.filters, $ctrl.paging)
                    .success(getObjectifFlashListOnSuccess)
                    .error(getOjectifFlashListOnError)
                    .finally(getObjectifFlashListOnFinally);
            }
        }

        function getObjectifFlashListOnSuccess(result) {
            $ctrl.objectifFlashList = $ctrl.objectifFlashList.concat(result.Items);
            $ctrl.TotalCount = result.TotalCount;
            $ctrl.paging.hasMorePage = $ctrl.objectifFlashList.length !== $ctrl.TotalCount;
        }

        function getOjectifFlashListOnError(error) {
            var errorMessage = '';
            for (var errProperty in error.ModelState) {
                if (resources.hasOwnProperty('err_' + errProperty)) {
                    errorMessage += resources['err_' + errProperty] + "<br>";
                }
            }

            Notify.error(errorMessage);
        }

        function getObjectifFlashValidationErrors(error) {
            var validationError = ModelStateErrorManager.getErrors(error);
            if (validationError) {
                Notify.error(validationError.replace(/\n/g, "<br />"));
                return;
            }

                Notify.error($ctrl.resources.Global_Notification_Error);
        }

        function getObjectifFlashListOnFinally() {
            ProgressBar.complete();
            $ctrl.isBusy = false;
            $timeout(Reload, 0, true);
        }

        function actionDeleteObjectifFlash(objectifFlash) {
            ObjectifFlashService.deleteObjectifFlash(objectifFlash.ObjectifFlashId)
                .then(function () {
                    $ctrl.paging = { page: 1, pageSize: 20, hasMorePage: false };
                    getObjectifFlashList();
                    Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
                })
                .catch(function (error) {
                    Notify.error($ctrl.resources.Global_Notification_Error);
                });
        }

        function actionDuplicateObjectifFlash(date, objectifFlash) {
            ProgressBar.start();
            ObjectifFlashService.duplicateObjectifFlash(date, objectifFlash.ObjectifFlashId)
                .then(function (data) {
                    var objectifFlashId = data.data;
                    handleRedirectionObjectifFlash(objectifFlashId);
                    Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
                })
                .catch(function (error) {
                    Notify.error($ctrl.resources.Global_Notification_Error);
                })
                .finally(getObjectifFlashListOnFinally);
        }

    }
})(angular);