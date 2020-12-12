(function (angular) {
    'use strict';

    angular.module('Fred').component('editionPaieComponent', {
        templateUrl: '/Areas/Pointage/RapportListe/Scripts/Modals/edition-paie-modal.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: 'EditionPaieComponentController'
    });

    EditionPaieComponentController.$inject = ['RapportService', 'ProgressBar', 'EtatPaieService', 'Notify', '$q'];

    function EditionPaieComponentController(RapportService, ProgressBar, EtatPaieService, Notify, $q) {
        var $ctrl = this;

        $ctrl.exportation =
        {
            verificationTemps: 0,
            controlePointages: 1,
            controlePointagesFes: 2,
            primes: 3,
            IGD: 4,
            heuresSpecifiques: 5,
            salarieAcompte: 6,
            AbsencesMensuelsList: 7

        };


        $ctrl.$onInit = $onInit;
        $ctrl.handleCancel = handleCancel;
        $ctrl.handleChangePersonnelStatutList = handleChangePersonnelStatutList;
        $ctrl.handleChangeWeek = handleChangeWeek;
        $ctrl.handleExport = handleExport;
        $ctrl.handleLookupSelection = handleLookupSelection;
        $ctrl.handleSelectAllEtablissement = handleSelectAllEtablissement;
        $ctrl.handleSelectChoseEtablissement = handleSelectChoseEtablissement;
        $ctrl.handleShowLookup = handleShowLookup;
        $ctrl.handleDisplayOrganisationSelector = handleDisplayOrganisationSelector;
        $ctrl.handleChangeCalendarDate = handleChangeCalendarDate;
        $ctrl.handleLookupDeletion = handleLookupDeletion;
        $ctrl.displayWeek = displayWeek;

        function $onInit() {
            $ctrl.resources = $ctrl.resolve.resources;
            $ctrl.etatPaieExportModel = $ctrl.resolve.etatPaieExportModel;
            $ctrl.currentExportation = $ctrl.resolve.currentExportation;
            $ctrl.isUserFes = $ctrl.resolve.isUserFes;
            $ctrl.userOrganizationId = $ctrl.resolve.userOrganizationId;
            initPerimetreUrl();
            if (!$ctrl.etatPaieExportModel) {
                EtatPaieService.GetEtatPaieExportModel().then(function (response) {
                    $ctrl.etatPaieExportModel = response.data;
                    initFiltresEdition();
                });
            }
        }

        function initPerimetreUrl() {
            let globalUrl = "/api/Organisation/SearchLightOrganisation/?typeOrganisation={0}&";
            if ($ctrl.isUserFes) {
                $ctrl.PerimetreUrl = String.format(globalUrl, "SOCIETE,UO,PUO");
            } else {
                $ctrl.PerimetreUrl = String.format(globalUrl, "SOCIETE,UO,PUO,ETABLISSEMENT,CI");
            }
        }

        function initFiltresEdition() {
            $ctrl.etatPaieExportModel.DateComptable = new Date();
            $ctrl.etatPaieExportModel.DateType = 2;
            $ctrl.etatPaieExportModel.Filtre = 2;
            $ctrl.etatPaieExportModel.Tri = true;
            $ctrl.etatPaieExportModel.FiltresPrimeMensuelles = false;
            if ($ctrl.isUserFes) {
                $ctrl.etatPaieExportModel.DateForWeek = new Date();
                $ctrl.etatPaieExportModel.Filtre = 3;
                handleDisplayOrganisationSelector(true);
                $ctrl.etatPaieExportModel.StatutPersonnelList = ["1", "2", "3"];
                $ctrl.etatPaieExportModel.StatutOuvrier = $ctrl.etatPaieExportModel.StatutPersonnelList.includes("1");
                $ctrl.etatPaieExportModel.StatutEtam = $ctrl.etatPaieExportModel.StatutPersonnelList.includes("2");
                $ctrl.etatPaieExportModel.StatutCadre = $ctrl.etatPaieExportModel.StatutPersonnelList.includes("3");
            }
        }

        function handleCancel() {
            $ctrl.close({ $value: $ctrl.etatPaieExportModel });
        }

        function handleChangeCalendarDate() {
            actionGenerateSelectedWeekLabel();
        }

        function handleChangeWeek(isAddWeek) {
            if (isAddWeek) {
                $ctrl.etatPaieExportModel.DateForWeek = moment($ctrl.etatPaieExportModel.DateForWeek).add(1, 'week').toDate();
            } else {
                $ctrl.etatPaieExportModel.DateForWeek = moment($ctrl.etatPaieExportModel.DateForWeek).subtract(1, 'week').toDate();
            }
            actionGenerateSelectedWeekLabel();
        }

        function actionGenerateSelectedWeekLabel() {
            var momentDate = moment($ctrl.etatPaieExportModel.DateForWeek);
            var mondayDayNumber = momentDate.startOf('isoWeek').format('DD');
            var sundayDayNumber = momentDate.isoWeekday(7).format('DD');
            var mondayMonth = momentDate.startOf('isoWeek').format('MM');
            var sundayMonth = momentDate.isoWeekday(7).format('MM');
            var mondayYear = momentDate.startOf('isoWeek').format('YYYY');
            var sundayYear = momentDate.isoWeekday(7).format('YYYY');

            $ctrl.etatPaieExportModel.Mois = parseInt(sundayMonth);
            $ctrl.etatPaieExportModel.Annee = parseInt(sundayYear);

            var mondayMonthLabel = mondayMonth === sundayMonth ? '' : $ctrl.resources['Global_Month_' + mondayMonth];
            var sundayMonthLabel = $ctrl.resources['Global_Month_' + sundayMonth];
            var mondayYearLabel = mondayYear === sundayYear ? '' : mondayYear;

            $ctrl.mondayDayNumber = mondayDayNumber;
            $ctrl.etatPaieExportModel.Date = new Date(mondayMonth + "/" + mondayDayNumber + "/" + mondayYear);
            $ctrl.selectedWeek = $ctrl.resources.Global_From + " " + mondayDayNumber + " " + mondayMonthLabel + " " + mondayYearLabel + " " +
                $ctrl.resources.Global_To + " " + sundayDayNumber + " " + sundayMonthLabel + " " + sundayYear;
            $ctrl.etatPaieExportModel.SelectedWeek = $ctrl.selectedWeek[0].toUpperCase() + $ctrl.selectedWeek.slice(1).toLowerCase();
        }

        function handleChangePersonnelStatutList(type) {
            var value, boolean;

            switch (type) {
                case "StatutOuvrier":
                    value = "1";
                    boolean = $ctrl.etatPaieExportModel.StatutOuvrier;
                    break;
                case "StatutEtam":
                    value = "2";
                    boolean = $ctrl.etatPaieExportModel.StatutEtam;
                    break;
                case "StatutCadre":
                    value = "3";
                    boolean = $ctrl.etatPaieExportModel.StatutCadre;
                    break;
            }

            if (boolean) {
                $ctrl.etatPaieExportModel.StatutPersonnelList.push(value);
            }
            else {
                $ctrl.etatPaieExportModel.StatutPersonnelList.splice($ctrl.etatPaieExportModel.StatutPersonnelList.indexOf(value), 1);
            }
        }

        function handleDisplayOrganisationSelector(status) {
            if (status === false) {
                $ctrl.organisationEmpty = false;
                $ctrl.etablissementPaieListEmpty = false;
                $ctrl.etatPaieExportModel.Organisation = null;
                $ctrl.etatPaieExportModel.OrganisationId = 0;
            }
            $ctrl.etatPaieExportModel.DisplayOrganisationSelector = status;
        }

        function handleSelectAllEtablissement() {
            $ctrl.etatPaieExportModel.EtablissementPaieIdList = [];
            $ctrl.etablissementPaieListEmpty = false;
        }


        function handleSelectChoseEtablissement() {
            $ctrl.etatPaieExportModel.EtablissementPaieIdList = $ctrl.etatPaieExportModel.EtablissementPaieList.map(x => x.EtablissementPaieId);
        }

        function handleLookupSelection(val, item) {
            switch (val) {
                case "Organisation":
                    $ctrl.organisationEmpty = false;
                    $ctrl.etatPaieExportModel.Organisation = item;
                    $ctrl.etatPaieExportModel.OrganisationId = item.OrganisationId;
                    $ctrl.etatPaieExportModel.EtablissementPaieIdList = [];
                    $ctrl.etatPaieExportModel.EtablissementPaieList = [];
                    $ctrl.etatPaieExportModel.IsAllEtablissement = true;
                    RapportService.GetSocieteByOrganisationId($ctrl.etatPaieExportModel.Organisation.OrganisationId).then(function (response) {
                        $ctrl.etatPaieExportModel.Societe = response.data;
                    });
                    break;
                case "EtablissementPaie":
                    if ($ctrl.etatPaieExportModel.EtablissementPaieIdList.indexOf(item.IdRef) > -1) {
                        Notify.error($scope.resources.Rapport_ModalImport_EtablissementPaie_DejaChoisi);
                    }
                    else {
                        $ctrl.etatPaieExportModel.EtablissementPaieIdList.push(item.IdRef);
                        $ctrl.etatPaieExportModel.EtablissementPaieList.push(item);
                        $ctrl.etablissementPaieListEmpty = false;
                    }
                    break;
            }
        }

        function handleShowLookup(val) {
            if (val === "EtablissementPaie") {
                var url = '/api/EtablissementPaie/SearchLight/?page={0}&societeId={1}';
                url = String.format(url, 1, $ctrl.etatPaieExportModel.Societe.SocieteId);
                return url;
            }
        }

        function displayWeek(val) {
            $ctrl.etatPaieExportModel.DateForWeek = val.date;
            actionGenerateSelectedWeekLabel();
        }

        function handleLookupDeletion(val, item) {
            switch (val) {
                case "EtablissementPaie":
                    var indexId = $ctrl.etatPaieExportModel.EtablissementPaieIdList.indexOf(item.IdRef);
                    var indexItem = $ctrl.etatPaieExportModel.EtablissementPaieList.indexOf(item);
                    $ctrl.etatPaieExportModel.EtablissementPaieIdList.splice(indexId, 1);
                    $ctrl.etatPaieExportModel.EtablissementPaieList.splice(indexItem, 1);
                    break;
                case "Organisation":
                    $ctrl.etatPaieExportModel.OrganisationId = null;
                    $ctrl.etatPaieExportModel.Organisation = null;
                    $ctrl.etatPaieExportModel.EtablissementPaieList = [];
                    $ctrl.etatPaieExportModel.EtablissementPaieIdList = [];
                    $ctrl.etablissementPaieListEmpty = false;
                    break;
            }
        };

        function handleExport(isPdf) {
            if (actionFormVerification()) {
                $ctrl.etatPaieExportModel.Pdf = isPdf;

                $q.when()
                    .then(actionBeginDownload)
                    .then(actionExport)
                    .catch(actionHandleError)
                    .finally(actionCompleteDownload);
            }
        }

        function actionFormVerification() {
            if ($ctrl.etatPaieExportModel.Filtre === 3 && ($ctrl.etatPaieExportModel.Organisation === null || $ctrl.etatPaieExportModel.Organisation === undefined)) {
                $ctrl.organisationEmpty = true;
                return false;
            } else if ($ctrl.etatPaieExportModel.Filtre === 3 && $ctrl.etatPaieExportModel.Organisation !== null && $ctrl.etatPaieExportModel.Organisation.TypeOrganisation === "Société"
                && $ctrl.etatPaieExportModel.IsAllEtablissement === false && $ctrl.etatPaieExportModel.EtablissementPaieIdList.length === 0) {
                $ctrl.etablissementPaieListEmpty = true;
                return false;
            }
            return true;
        }

        function actionBeginDownload() {
            ProgressBar.start();
            $ctrl.disableDownload = true;
            Notify.message($ctrl.resources.AttenteGenerationRapport);
        }

        function actionExport() {
            if ($ctrl.etatPaieExportModel.DateType !== 1) {
                var date = new Date($ctrl.etatPaieExportModel.DateComptable);
                $ctrl.etatPaieExportModel.Annee = date.getFullYear();
                $ctrl.etatPaieExportModel.Mois = date.getMonth() + 1;
            }

            switch ($ctrl.currentExportation) {
                case 0:
                    return EtatPaieService.GenerateExcelVerificationTemps($ctrl.etatPaieExportModel);
                case 1:
                    return RapportService.IsExcelControlePointagesNotEmpty($ctrl.etatPaieExportModel).then(function (value) {
                        if (value) {
                            return EtatPaieService.GenerateExcelControlePointages($ctrl.etatPaieExportModel);
                        }
                        else {
                            Notify.error("Aucun pointage ne correspond aux critères sélectionnés");
                        }
                    });
                case 2:
                    if ($ctrl.etatPaieExportModel.DateType === 1)
                        return EtatPaieService.GenerateExcelControlePointagesHebdomadaire($ctrl.etatPaieExportModel);
                    else
                        return EtatPaieService.GenerateExcelControlePointagesFes($ctrl.etatPaieExportModel);
                case 3:
                    return EtatPaieService.GenerateDocumentListePrimes($ctrl.etatPaieExportModel);
                case 4:
                    return EtatPaieService.GenerateDocumentListeIGD($ctrl.etatPaieExportModel);
                case 5:
                    return EtatPaieService.GenerateDocumentListeHeuresSpecifiques($ctrl.etatPaieExportModel);
                case 6:
                    return EtatPaieService.GenerateExcelSalarieAcompte($ctrl.etatPaieExportModel);
                case 7:
                    return EtatPaieService.GenerateExcelListAbsencesMensuels($ctrl.etatPaieExportModel);
            }
        }

        function actionHandleError(error) {
            if (error && error.ExceptionMessage) {
                Notify.error(error.ExceptionMessage);
            }
            else if (error && error.Message) {
                Notify.error(error.Message);
            }
            else {
                Notify.defaultError();
            }
        }

        function actionCompleteDownload() {
            $ctrl.disableDownload = false;
            ProgressBar.complete();
        }

    }

    angular.module('Fred').controller('EditionPaieComponentController', EditionPaieComponentController);

}(angular));