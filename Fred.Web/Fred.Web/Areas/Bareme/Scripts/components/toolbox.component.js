(function () {
    'use strict';

    var toolboxComponent = {
        templateUrl: '/Areas/Bareme/Scripts/components/toolbox.component.html',
        bindings: {
            resources: '<',
            favoriId: '<',
            typeBareme: '<',
            type: '<',
            dismiss: '&'
        },
        controller: toolboxController
    };

    angular.module('Fred').component('toolboxComponent', toolboxComponent);

    angular.module('Fred').controller('toolboxController', toolboxController);

    toolboxController.$inject = ['$scope', '$filter', 'BaremeService', 'ProgressBar', 'Notify', '$uibModal', 'confirmDialog', '$window', 'favorisService'];

    function toolboxController($scope, $filter, BaremeService, ProgressBar, Notify, $uibModal, confirmDialog, $window, favorisService) {

        var $ctrl = this;
        var periodeCourante = new Date();
        var organisationCourante = null;
        var ciCourant = null;
        $ctrl.listChecked = new Array();
        $ctrl.isPdfOrga = false;


        //////////////////////////////////////////////////////////////////
        // Déclaration des fonctions publiques                          //
        //////////////////////////////////////////////////////////////////
        $ctrl.changePeriode = changePeriode;
        $ctrl.changeOrganisation = changeOrganisation;
        $ctrl.changeCI = changeCI;
        $ctrl.handleClickSave = handleClickSave;
        $ctrl.handleClickCancel = handleClickCancel;
        $ctrl.handleExpandAll = handleExpandAll;
        $ctrl.handleCollapseAll = handleCollapseAll;
        $ctrl.handleSearchChange = handleSearchChange;
        $ctrl.popupConfirmSynchro = popupConfirmSynchro;
        $ctrl.handleSynchro = handleSynchro;
        $ctrl.addFilter2Favoris = addFilter2Favoris;

        //////////////////////////////////////////////////////////////////
        // Init                                                         //
        //////////////////////////////////////////////////////////////////
        $ctrl.$onInit = function () {

            getFilterOrFavoris($ctrl.favoriId);

            $scope.$on('readonlyBareme', function (event, readonly, readonlyMessage) {
                $ctrl.readonly = readonly;
                $ctrl.readonlyMessage = readonlyMessage;
            });
            $scope.$on('parentEmpty', function (event, arg) {
                $ctrl.parentEmpty = arg;
                if ($ctrl.parentEmpty) {
                    if ($ctrl.readonlyMessage.length > 0) {
                        $ctrl.readonlyMessage += "\n";
                    }
                    $ctrl.readonlyMessage += "Synchronisation impossible, l'organisation parent est vide.";
                }
            });
        };

        //////////////////////////////////////////////////////////////////
        // Actions                                                      //
        //////////////////////////////////////////////////////////////////
        function changePeriode() {
            if (!periodeCourante || !BaremeService.IsSamePeriod($ctrl.periode, periodeCourante)) {
                CheckForChanges(
                    function () {
                        periodeCourante = $ctrl.periode;
                        $scope.$emit('changePeriode', $filter('date')($ctrl.periode, 'MM-dd-yyyy'));

                        if ($ctrl.type === $ctrl.typeBareme.Organisation) {
                            sessionStorage.setItem('baremeExploitationOrganisationFilter', JSON.stringify({ Organisation: $ctrl.organisation, Periode: periodeCourante }))
                        } else if ($ctrl.type === $ctrl.typeBareme.CI) {
                            sessionStorage.setItem('baremeExploitationCIFilter', JSON.stringify({ CI: $ctrl.ci, Periode: periodeCourante }))
                        }
                    },
                    function () {
                        $ctrl.periode = periodeCourante;
                    });
            }
        }

        function changeOrganisation() {
            if (!organisationCourante || $ctrl.organisation.OrganisationId !== organisationCourante.OrganisationId) {
                CheckForChanges(
                    function () {
                        organisationCourante = $ctrl.organisation;
                        $scope.$emit('changeOrganisation', $ctrl.organisation);
                        if ($ctrl.type === $ctrl.typeBareme.Organisation) {
                            sessionStorage.setItem('baremeExploitationOrganisationFilter', JSON.stringify({ Organisation: $ctrl.organisation, Periode: periodeCourante }))
                        }
                    },
                    function () {
                        $ctrl.organisation = organisationCourante;
                    });
            }
        }

        function changeCI() {
            if (!ciCourant || $ctrl.ci.CiId !== ciCourant.CiId) {
                CheckForChanges(
                    function () {
                        ciCourant = $ctrl.ci;
                        $scope.$emit('changeCI', $ctrl.ci);
                        if ($ctrl.type === $ctrl.typeBareme.CI) {
                            sessionStorage.setItem('baremeExploitationCIFilter', JSON.stringify({ CI: $ctrl.ci, Periode: periodeCourante }))
                        }
                    },
                    function () {
                        $ctrl.ci = ciCourant;
                    });
            }
        }

        function handleClickSave() {
            $scope.$emit('saveRequested');
        }
        function handleClickCancel() {
            $scope.$emit('cancelRequested');
        }
        function handleExpandAll() {
            $scope.$emit('expandAllRequested');
        }
        function handleCollapseAll() {
            $scope.$emit('collapseAllRequested');
        }
        function handleSearchChange() {
            $scope.$emit('searchRequested', $ctrl.search);
        }

        function handleSynchroCI() {
            actionSynchroCI();
        }

        function actionSynchroCI() {
            ProgressBar.start();
            BaremeService.SynchroCI($filter('date')($ctrl.periode, 'MM-dd-yyyy'), $ctrl.ci ? $ctrl.ci.CiId : 0)
                .then(result => SynchroCIOK(result.data))
                .catch(SynchroCIKO)
                .finally(() => ProgressBar.complete());
        }

        function SynchroCIOK(result) {
            if (result.MessageErreur) {
                Notify.error(result.MessageErreur);
            }
            else {
                Notify.message($ctrl.resources.BaremeExploitationCI_SynchronisationTerminee);
                $scope.$emit('postSynchro');
            }
        }

        function SynchroCIKO() {
            Notify.error($ctrl.resources.BaremeExploitationCI_SynchronisationEchec);
        }

        function handleSynchroOrga() {
            ProgressBar.start();
            BaremeService.SynchroOrga($filter('date')($ctrl.periode, 'MM-dd-yyyy'), $ctrl.organisation.OrganisationId)
                .then(SynchroOrgaOK)
                .catch(SynchroOrgaKO)
                .finally(() => ProgressBar.complete());
        }

        function SynchroOrgaOK() {
            Notify.message($ctrl.resources.BaremeExploitationCI_SynchronisationTerminee);
            $scope.$emit('postSynchro');
        }

        function SynchroOrgaKO() {
            Notify.error($ctrl.resources.BaremeExploitationCI_SynchronisationEchec);
        }

        function CheckForChanges(acceptChangeFunc, dismissChangeFunc) {
            if (BaremeService.HasPendingChanges()) {
                confirmDialog.confirm(resources, $ctrl.resources.BaremeExploitationOrga_ConfirmerModificationsPerdues, "flaticon flaticon-warning")
                    .then(acceptChangeFunc)
                    .catch(dismissChangeFunc);
            }
            else {
                acceptChangeFunc();
            }
        }

        function handleSynchro() {
            if ($ctrl.type === $ctrl.typeBareme.Organisation) {
                handleSynchroOrga();
            }
            else if ($ctrl.type === $ctrl.typeBareme.CI) {
                handleSynchroCI();
            }
        }

        function popupConfirmSynchro() {
            $("#confirmationSynchroModal").modal();
        }

        /*
* @function addFilter2Favoris()
* @description Crée un nouveau favori
*/
        function addFilter2Favoris() {
            var url = $window.location.pathname;
            if ($ctrl.favoriId !== 0) {
                url = $window.location.pathname.slice(0, $window.location.pathname.lastIndexOf("/"));
            }

            var filter = getFilterByTypeBareme();

            if ($ctrl.type === $ctrl.typeBareme.Organisation) {
                favorisService.initializeAndOpenModal("BaremeExploitationOrganisation", url, filter);
            }
            else if ($ctrl.type === $ctrl.typeBareme.CI) {
                favorisService.initializeAndOpenModal("BaremeExploitationCI", url, filter);
            }
        }

        function getFilterOrFavoris(favoriId) {
            $ctrl.favoriId = parseInt(favoriId);
            var filter = getFilterByTypeBareme();
            if ($ctrl.favoriId !== 0) {
                favorisService.getFilterByFavoriIdOrDefault({ favoriId: $ctrl.favoriId, defaultFilter: filter }).then(function (response) {
                    $ctrl.periode = new Date(response.Periode)
                    $scope.$emit('changePeriode', $filter('date')($ctrl.periode, 'MM-dd-yyyy'));
                    if ($ctrl.type === $ctrl.typeBareme.Organisation) {
                        $ctrl.organisation = response.Organisation;
                        $scope.$emit('changeOrganisation', $ctrl.organisation);
                    }
                    else if ($ctrl.type === $ctrl.typeBareme.CI) {
                        $ctrl.ci = response.CI;
                        $scope.$emit('changeCI', $ctrl.ci);
                    }
                }).catch(function (error) { console.log(error); })
            } else if ($ctrl.type === $ctrl.typeBareme.Organisation) {
                if (sessionStorage.getItem('baremeExploitationOrganisationFilter') !== null) {
                    $ctrl.periode = JSON.parse(sessionStorage.getItem('baremeExploitationOrganisationFilter')).Periode
                    $scope.$emit('changePeriode', $filter('date')($ctrl.periode, 'MM-dd-yyyy'));
                    $ctrl.organisation = JSON.parse(sessionStorage.getItem('baremeExploitationOrganisationFilter')).Organisation
                    if ($ctrl.organisation !== null) {
                        $scope.$emit('changeOrganisation', $ctrl.organisation);
                    }
                }
                else {
                    $ctrl.periode = periodeCourante;
                    $scope.$emit('changePeriode', $filter('date')($ctrl.periode, 'MM-dd-yyyy'));
                }
            } else if ($ctrl.type === $ctrl.typeBareme.CI) {
                if (sessionStorage.getItem('baremeExploitationCIFilter') !== null) {
                    $ctrl.periode = JSON.parse(sessionStorage.getItem('baremeExploitationCIFilter')).Periode
                    $scope.$emit('changePeriode', $filter('date')($ctrl.periode, 'MM-dd-yyyy'));
                    $ctrl.ci = JSON.parse(sessionStorage.getItem('baremeExploitationCIFilter')).CI
                    if ($ctrl.ci !== null) {
                        $scope.$emit('changeCI', $ctrl.ci);
                    }
                }
                else {
                    $ctrl.periode = periodeCourante;
                    $scope.$emit('changePeriode', $filter('date')($ctrl.periode, 'MM-dd-yyyy'));

                }
            } else {
                $ctrl.periode = periodeCourante;
                $scope.$emit('changePeriode', $filter('date')($ctrl.periode, 'MM-dd-yyyy'));

            }
        }

        $ctrl.handleExportExcel = function (isPDF = $ctrl.isPdfOrga) {
            if ($ctrl.type === $ctrl.typeBareme.Organisation) {
                var object = {
                    period: $filter('date')($ctrl.periode, 'MM-dd-yyyy'),
                    levelOfSelectedOrganisation: $ctrl.organisation.TypeOrganisationId,
                    idSelectedOrganisation: $ctrl.organisation.OrganisationId,
                    listOfCheckedLevel: $ctrl.listChecked
                };
                ProgressBar.start();
                BaremeService.ExportExcelOrganisation(object, isPDF)
                    .then(
                        (result) => { BaremeService.ExtractBaremeOrganisationExcel(result.data.id, isPDF); }
                    )
                    .catch(error => {
                        Notify.error("Erreur lors de l'export excel");
                    })
                    .finally(() => ProgressBar.complete());
            }
            else {
                var objectCi = {
                    period: $filter('date')($ctrl.periode, 'MM-dd-yyyy'),
                    selectedCiId: $ctrl.ci.CiId,
                    selectedSocieteId: $ctrl.ci.SocieteId
                };
                ProgressBar.start();
                BaremeService.ExportExcelCi(objectCi, isPDF)
                    .then(
                        (result) => { BaremeService.ExtractBaremeCiExcel(result.data.id, isPDF); }
                    )
                    .catch(error => {
                        Notify.error($ctrl.resources.BaremeExploitation_ErrorExportMsg);
                    })
                    .finally(() => ProgressBar.complete());
            }

        };

        $ctrl.IsUo = function () {
            return $ctrl.organisation.TypeOrganisationId === 6;
        };

        $ctrl.IsEtablissemnt = function () {
            return $ctrl.organisation.TypeOrganisationId === 7;
        };

        $ctrl.handleClickExportOrga = function (isPdfOrganisation) {
            if ($ctrl.organisation.TypeOrganisationId !== 4) {
                $uibModal.open({
                    animation: true,
                    component: 'exportBaremeModal',
                    resolve: {
                        IsUo: $ctrl.IsUo() || $ctrl.IsEtablissemnt(),
                        IsEtablissement: $ctrl.IsEtablissemnt(),
                        Period: $ctrl.periode,
                        TypeOrganisationId: $ctrl.organisation.TypeOrganisationId,
                        OrganisationId: $ctrl.organisation.OrganisationId,
                        isPdfOrganisation: isPdfOrganisation,
                        ressources: $ctrl.resources
                    }
                });
            }
            else {
                $ctrl.handleExportExcel(isPdfOrganisation);
            }
        };

        function getFilterByTypeBareme() {
            var filter = null;
            if ($ctrl.type === $ctrl.typeBareme.Organisation) {
                filter = {
                    Periode: $ctrl.periode,
                    Organisation: $ctrl.organisation
                }
            }
            else if ($ctrl.type === $ctrl.typeBareme.CI) {
                filter = {
                    Periode: $ctrl.periode,
                    CI: $ctrl.ci
                }
            }
            return filter;
        }
    }
})();
