(function (angular) {
    'use strict';

    angular.module('Fred').controller('CloturesPeriodesController', CloturesPeriodesController);

    CloturesPeriodesController.$inject = ['$filter', '$q', 'Notify', 'ProgressBar', 'CloturesPeriodesService'];

    function CloturesPeriodesController($filter, $q, Notify, ProgressBar, CloturesPeriodesService) {

        var ctrl = this;

        ctrl.resources = resources;
        ctrl.periode = new Date();
        ctrl.isBusy = false;
        ctrl.dccList = [];
        ctrl.isModeBlocEnergiesToutSelectionner = false;
        ctrl.isModeBlocDepensesToutSelectionner = false;
        ctrl.displayFilterState = displayFilterState;

        ctrl.onChangePeriod = function () {
            $q.when()
                .then(() => {
                    ctrl.filter.month = ctrl.periode.getMonth() + 1;
                    ctrl.filter.year = ctrl.periode.getFullYear();
                    sessionStorage.setItem('cloturesPeriodes.filter.month', JSON.stringify({ month: ctrl.filter.month }));
                    sessionStorage.setItem('cloturesPeriodes.filter.year', JSON.stringify({ year: ctrl.filter.year }));
                }).then(ctrl.applyFilter);
        };

        ctrl.lookupOrganisationSelection = function (item) {
            $q.when()
                .then(() => {
                    ctrl.filter.organisationId = item.OrganisationId;
                    ctrl.filter.typeOrganisationId = item.TypeOrganisationId;
                    sessionStorage.setItem('cloturesPeriodes.filter.organisation', JSON.stringify({ organisation: item }));
                }).then(ctrl.applyFilter);
        };

        ctrl.lookupOrganisationDeletion = function () {
            $q.when()
                .then(() => {
                    ctrl.filter.organisation = null;
                    ctrl.filter.organisationId = null;
                    ctrl.filter.typeOrganisationId = null;
                    sessionStorage.setItem('cloturesPeriodes.filter.organisation', JSON.stringify({ organisation: null }));
                }).then(ctrl.applyFilter);
        };

        ctrl.getFilter = function (filter) {
            if (angular.equals(ctrl.filter, {})) {
                ctrl.filter = filter ? filter : {};
            }
            return ctrl.filter;
        };

        ctrl.storeCentreImputationAndApplyFilter = function () {
            $q.when()
              .then(() => {
                  ctrl.filter.centreImputation = ctrl.filter.centreImputation.toUpperCase();
                  sessionStorage.setItem('cloturesPeriodes.filter.centreImputation', JSON.stringify({ centreImputation: ctrl.filter.centreImputation })); })
              .then(ctrl.applyFilter);
        };

        ctrl.applyFilter = function () {
            $q.when()
                .then(() => {
                    ctrl.filter.year = ctrl.periode.getFullYear();
                    ctrl.filter.month = ctrl.periode.getMonth() + 1;
                    sessionStorage.setItem('cloturesPeriodes.filter', JSON.stringify({ filter: ctrl.filter }));
                }).then(ctrl.searchFilterWithFirstLoadOrNot(ctrl.filter, true));
        };

        ctrl.resetFilter = function () {
            ctrl.filter.centreImputation = null;
            ctrl.filter.transfertFar = null;
            ctrl.filter.clotureDepenses = null;
            ctrl.filter.validationAvancement = null;
            ctrl.filter.validationControleBudgetaire = null;
            ctrl.filter.clotureSurLaPeriode = null;
            ctrl.filter.dejaTermine = false;
        };

        ctrl.searchFilterWithFirstLoadOrNot = function (filter, firstLoad) {
            if (firstLoad) {
                ctrl.dccList = [];
            }

            if (ctrl.isBusy) {
                return;
            }

            unactiveAllButtons();

            CloturesPeriodesService.searchFilter(filter)
                .then(loadInit)
                .then(activeAllButtons)
                .catch(processError);
        };

        ctrl.toggleFiltreDccPourGenererEnergies = function (dcc) {
            if (dcc) {
                toggleFiltreDccPourGenererEnergies(dcc, dcc.isDccSelectionneePourGenererEnergies);
            }

            ctrl.isModeBlocEnergiesToutSelectionner = isInModeToutSelectionner(ctrl.lstFiltreeEtSelectionneeDccPourGenererEnergies);
        };

        ctrl.toggleLstFiltreDccPourGenererEnergies = function (isSelectingAllElseDeselectingAll) {
            ctrl.isModeBlocEnergiesToutSelectionner = isSelectingAllElseDeselectingAll;
            angular.forEach(ctrl.dccList, function (dcc) {
                if (dcc) {
                    toggleFiltreDccPourGenererEnergies(dcc, isSelectingAllElseDeselectingAll);
                }
            });
        };

        ctrl.toggleFiltreDccPourUpdateClotureDepenses = function (dcc) {
            toggleFiltreDccPourUpdateDepenses(dcc, dcc.isDccSelectionneePourCloturerDepenses);

            ctrl.isModeBlocDepensesToutSelectionner = isInModeToutSelectionner(ctrl.lstFiltreeEtSelectionneeDccPourCloturerDepenses);
        };
        ctrl.toggleFiltreDccPourUpdateDeclotureDepenses = function (dcc) {
            toggleFiltreDccPourUpdateDepenses(dcc, dcc.isDccSelectionneePourDecloturerDepenses);

            ctrl.isModeBlocDepensesToutSelectionner = isInModeToutSelectionner(ctrl.lstFiltreeEtSelectionneeDccPourDecloturerDepenses);
        };

        ctrl.toggleLstFiltreDccPourUpdateDepenses = function (isSelectingAllElseDeselectingAll) {
            ctrl.isModeBlocDepensesToutSelectionner = isSelectingAllElseDeselectingAll;
            angular.forEach(ctrl.dccList, function (dcc) {
                toggleFiltreDccPourUpdateDepenses(dcc, isSelectingAllElseDeselectingAll);
            });
        };

        ctrl.cloturerDepenses = function () {
            if (ctrl.isModeBlocDepensesToutSelectionner) {
                if (ctrl.isBusy)
                    return;
                unactiveAllButtons();
                return CloturesPeriodesService.cloturerAllDepensesExclusDeselectionnes(ctrl.isModeBlocDepensesToutSelectionner, ctrl.filter, ctrl.dccList, ctrl.lstFiltreeEtSelectionneeDccPourCloturerDepenses)
                    .then(function (allDccClotures) {
                        angular.forEach(ctrl.dccList, function (dcc, key) {
                            dcc.isDccSelectionneePourCloturerDepenses = false;
                        });
                        return allDccClotures;
                    })
                    .then(onCloturerDepensesSuccess)
                    .then(activeAllButtons)
                    .catch(processError);
            }
            else {
                if (ctrl.isBusy)
                    return;
                unactiveAllButtons();
                return CloturesPeriodesService.cloturerSeulementDepensesInclusSelectionnes(ctrl.isModeBlocDepensesToutSelectionner, ctrl.filter, ctrl.lstFiltreeEtSelectionneeDccPourCloturerDepenses)
                    .then(function (allDccClotures) {
                        angular.forEach(ctrl.dccList, function (dcc, key) {
                            dcc.isDccSelectionneePourCloturerDepenses = false;
                        });
                        return allDccClotures;
                    })
                    .then(onCloturerDepensesSuccess)
                    .then(activeAllButtons)
                    .catch(processError);
            }
        };

        ctrl.cloturerDepense = function (dcc) {
            if (dcc) {
                let lstOneElementFiltreeEtSelectionneeDccPourCloturerDepenses = [];
                lstOneElementFiltreeEtSelectionneeDccPourCloturerDepenses.push(dcc);

                if (ctrl.isBusy)
                    return;
                unactiveAllButtons();
                return CloturesPeriodesService.cloturerSeulementDepensesInclusSelectionnes(false, ctrl.filter, lstOneElementFiltreeEtSelectionneeDccPourCloturerDepenses)
                    .then(onCloturerDepenseSuccess)
                    .then(activeAllButtons)
                    .catch(processError);
            }
        };

        ctrl.decloturerDepenses = function () {
            if (ctrl.isBusy)
                return;
            unactiveAllButtons();
            let lstFiltreeEtSelectionneeDccPourDecloturerDepenses = [];
            ctrl.dccList.forEach(function (d) {
                if (d.isDccSelectionneePourDecloturerDepenses) {
                    lstFiltreeEtSelectionneeDccPourDecloturerDepenses.push(d);
                }
            });
            return CloturesPeriodesService.decloturerSeulementDepensesInclusSelectionnes(ctrl.isModeBlocDepensesToutSelectionner, ctrl.filter, lstFiltreeEtSelectionneeDccPourDecloturerDepenses)
                .then(onDecloturerDepensesSuccess)
                .then(activeAllButtons)
                .catch(processError);
        };

        ctrl.decloturerDepense = function (dcc) {
            if (dcc) {
                let lstOneElementFiltreeEtSelectionneeDccPourCloturerDepenses = [];
                lstOneElementFiltreeEtSelectionneeDccPourCloturerDepenses.push(dcc);

                if (ctrl.isBusy)
                    return;
                unactiveAllButtons();
                return CloturesPeriodesService.decloturerSeulementDepensesInclusSelectionnes(false, ctrl.filter, lstOneElementFiltreeEtSelectionneeDccPourCloturerDepenses)
                    .then(onDecloturerDepenseSuccess)
                    .then(activeAllButtons)
                    .catch(processError);
            }
        };

        init();
        return ctrl;

        function processError(error) {
            console.log(error);
            activeAllButtons();
        }

        /**
         * Initialisation du controller.     
         */
        function init() {
            angular.extend(ctrl, {
                items: {
                    transfertFarItems: [{ value: null, libelle: resources.Filtre_Tiret_Tous_cb }, { value: resources.Filtre_Fait_cb, libelle: resources.Filtre_Fait_cb }, { value: resources.Filtre_Non_Fait_cb, libelle: resources.Filtre_Non_Fait_cb }],
                    clotureDepensesItems: [{ value: null, libelle: resources.Filtre_Tiret_Tous_cb }, { value: resources.Filtre_Fait_cb, libelle: resources.Filtre_Fait_cb }, { value: resources.Filtre_Non_Fait_cb, libelle: resources.Filtre_Non_Fait_cb }],
                    validationAvancementItems: [{ value: null, libelle: resources.Filtre_Tiret_Tous_cb }, { value: resources.Filtre_Fait_cb, libelle: resources.Filtre_Fait_cb }, { value: resources.Filtre_Non_Fait_cb, libelle: resources.Filtre_Non_Fait_cb }],
                    validationControleBudgetaireItems: [{ value: null, libelle: resources.Filtre_Tiret_Tous_cb }, { value: resources.Filtre_Fait_cb, libelle: resources.Filtre_Fait_cb }, { value: resources.Filtre_Non_Fait_cb, libelle: resources.Filtre_Non_Fait_cb }],
                    clotureSurLaPeriodeItems: [{ value: null, libelle: resources.Filtre_Tiret_Tous_cb }, { value: resources.Filtre_Fait_cb, libelle: resources.Filtre_Fait_cb }, { value: resources.Filtre_Non_Fait_cb, libelle: resources.Filtre_Non_Fait_cb }]
                },
                filter: null
            });

            ctrl.filter = {};
            ctrl.applyFilter();
            getFilterFromSessionStorage();
            getPeriodeFromSessionStorage();
            getCentreImputationFromSessionStorage();
            getOrganisationFromSessionStorage();


            ctrl.lstFiltreeEtSelectionneeDccPourGenererEnergies = [];
            ctrl.lstFiltreeEtSelectionneeDccPourCloturerDepenses = [];
            ctrl.lstFiltreeEtSelectionneeDccPourDecloturerDepenses = [];
        }

        function getPeriodeFromSessionStorage() {
            if (sessionStorage.getItem('cloturesPeriodes.filter.year') !== null) {
                ctrl.filter.month = JSON.parse(sessionStorage.getItem('cloturesPeriodes.filter.month')).month;
                ctrl.filter.year = JSON.parse(sessionStorage.getItem('cloturesPeriodes.filter.year')).year;
                ctrl.periode.setMonth(ctrl.filter.month - 1);
                ctrl.periode.setFullYear(ctrl.filter.year);
            }
        }

        function getCentreImputationFromSessionStorage() {
            if (sessionStorage.getItem('cloturesPeriodes.filter.centreImputation') !== null) {
                ctrl.filter.centreImputation = JSON.parse(sessionStorage.getItem('cloturesPeriodes.filter.centreImputation')).centreImputation;
            }
        }

        function getOrganisationFromSessionStorage() {
            if (sessionStorage.getItem('cloturesPeriodes.filter.organisation') !== null) {
                ctrl.filter.organisation = JSON.parse(sessionStorage.getItem('cloturesPeriodes.filter.organisation')).organisation;
                ctrl.filter.organisationId = ctrl.filter.organisation ? ctrl.filter.organisation.OrganisationId : null;
                ctrl.filter.typeOrganisationId = ctrl.filter.organisation ? ctrl.filter.organisation.TypeOrganisationId : null;
            }
        }

        /**
         * Permet de récupérer les filtres stockés dans la session
         * */
        function getFilterFromSessionStorage() {
            if (sessionStorage.getItem('cloturesPeriodes.filter') !== null) {

                ctrl.filter.month = JSON.parse(sessionStorage.getItem('cloturesPeriodes.filter')).filter.month;
                ctrl.filter.year = JSON.parse(sessionStorage.getItem('cloturesPeriodes.filter')).filter.year;
                ctrl.filter.organisation = JSON.parse(sessionStorage.getItem('cloturesPeriodes.filter')).filter.organisation;
                ctrl.filter.organisationId = JSON.parse(sessionStorage.getItem('cloturesPeriodes.filter')).filter.organisationId;
                ctrl.filter.typeOrganisationId = JSON.parse(sessionStorage.getItem('cloturesPeriodes.filter')).filter.typeOrganisationId;
                ctrl.filter.centreImputation = JSON.parse(sessionStorage.getItem('cloturesPeriodes.filter')).filter.centreImputation;
                ctrl.filter.transfertFar = JSON.parse(sessionStorage.getItem('cloturesPeriodes.filter')).filter.transfertFar;
                ctrl.filter.clotureDepenses = JSON.parse(sessionStorage.getItem('cloturesPeriodes.filter')).filter.clotureDepenses;
                ctrl.filter.validationAvancement = JSON.parse(sessionStorage.getItem('cloturesPeriodes.filter')).filter.validationAvancement;
                ctrl.filter.validationControleBudgetaire = JSON.parse(sessionStorage.getItem('cloturesPeriodes.filter')).filter.validationControleBudgetaire;
                ctrl.filter.clotureSurLaPeriode = JSON.parse(sessionStorage.getItem('cloturesPeriodes.filter')).filter.clotureSurLaPeriode;
                ctrl.filter.dejaTermine = JSON.parse(sessionStorage.getItem('cloturesPeriodes.filter')).filter.dejaTermine;
            }
            ctrl.searchFilterWithFirstLoadOrNot(ctrl.filter, true);
        }

        function loadInit(response) {
            angular.forEach(response.data.Items, function (dcc) {
                dcc.DateOuverture = $filter('toLocaleDate')(dcc.DateOuverture);
                dcc.DateFermeture = $filter('toLocaleDate')(dcc.DateFermeture);

                // Si on avait cliqué sur "Tout sélectionner", toutes les énergies chargées par pagination seront sélectionnées par défaut
                toggleFiltreDccPourGenererEnergies(dcc, ctrl.isModeBlocEnergiesToutSelectionner);
                toggleFiltreDccPourUpdateDepenses(dcc, ctrl.isModeBlocDepensesToutSelectionner);

                ctrl.dccList.push(dcc);
            });
        }

        function toggleFiltreDccPourGenererEnergies(dcc, mustCheckBox) {
            let isDccSelectionneePourGenererEnergies = false;
            if (dcc) {
                if (mustCheckBox) {
                    if (!dcc.DateCloture) {
                        isDccSelectionneePourGenererEnergies = true;
                    }
                } else { // mustUncheckBox
                    if (dcc.DateCloture) {
                        isDccSelectionneePourGenererEnergies = true;
                    }
                }
                dcc.isDccSelectionneePourGenererEnergies = isDccSelectionneePourGenererEnergies;
                if (dcc.isDccSelectionneePourGenererEnergies) {
                    let isAlreadyIn = $filter('filter')(ctrl.lstFiltreeEtSelectionneeDccPourGenererEnergies, { CiId: dcc.CiId }, true)[0];
                    if (!isAlreadyIn) {
                        // ajoute les dcc à la liste des dcc dédiées à l'énergie
                        ctrl.lstFiltreeEtSelectionneeDccPourGenererEnergies.push(dcc);
                    }
                }
                else {
                    let mapLstFiltreeEtSelectionneeDccPourGenererEnergies = ctrl.lstFiltreeEtSelectionneeDccPourGenererEnergies.map(function (e) { return e.CiId; });
                    let index = mapLstFiltreeEtSelectionneeDccPourGenererEnergies.indexOf(dcc.CiId);
                    // enlève les dcc de la liste des dcc dédiées à l'énergie
                    if (index !== -1) {
                        ctrl.lstFiltreeEtSelectionneeDccPourGenererEnergies.splice(index, 1);
                        ctrl.isModeBlocEnergiesToutSelectionner = false;
                    }
                }
            }
        }

        function toggleFiltreDccPourUpdateDepenses(dcc, mustCheckBox) {
            dcc.isDccSelectionneePourCloturerDepenses = dcc.isDccSelectionneePourDecloturerDepenses = mustCheckBox;
            if (dcc.isDccSelectionneePourCloturerDepenses) {
                let isAlreadyIn = $filter('filter')(ctrl.lstFiltreeEtSelectionneeDccPourCloturerDepenses, { CiId: dcc.CiId }, true)[0];
                if (!isAlreadyIn) {
                    // ajoute les dcc à la liste des dcc dédiées aux dépenses
                    ctrl.lstFiltreeEtSelectionneeDccPourCloturerDepenses.push(dcc);
                }
            }
            else {
                let mapLstFiltreeEtSelectionneeDccPourCloturerDepenses = ctrl.lstFiltreeEtSelectionneeDccPourCloturerDepenses.map(function (e) { return e.CiId; });
                let index = mapLstFiltreeEtSelectionneeDccPourCloturerDepenses.indexOf(dcc.CiId);
                // enlève les dcc de la liste des dcc dédiées aux dépenses
                if (index !== -1) {
                    ctrl.lstFiltreeEtSelectionneeDccPourCloturerDepenses.splice(index, 1);
                    ctrl.isModeBlocDepensesToutSelectionner = false;
                }
            }
        }

        function onCloturerDepensesSuccess(allDccClotures) {
            ctrl.lstFiltreeEtSelectionneeDccPourCloturerDepenses
                .forEach(dcc => dcc.isDccSelectionneePourCloturerDepenses = dcc.isDccSelectionneePourDecloturerDepenses = false);
            ctrl.lstFiltreeEtSelectionneeDccPourCloturerDepenses = [];
            synchronizeDccPourBlocDepenses(allDccClotures.data, true);
            Notify.message(resources.Notif_ClotureDepenses_Succes);
        }

        function onCloturerDepenseSuccess(allDccClotures) {
            if (allDccClotures.data.length > 0) {
                var dccCloturee = ctrl.dccList.find(d => d.DateClotureComptableId === allDccClotures.data[0].DateClotureComptableId);
                dccCloturee.isDccSelectionneePourCloturerDepenses = dccCloturee.isDccSelectionneePourDecloturerDepenses = false;
            }
            synchronizeDccPourBlocDepenses(allDccClotures.data, false);
            Notify.message(resources.Notif_ClotureDepense_Succes);
        }

        function onDecloturerDepensesSuccess(allDccClotures) {
            ctrl.lstFiltreeEtSelectionneeDccPourDecloturerDepenses
                .forEach(dcc => dcc.isDccSelectionneePourCloturerDepenses = dcc.isDccSelectionneePourDecloturerDepenses = false);
            ctrl.lstFiltreeEtSelectionneeDccPourDecloturerDepenses = [];
            synchronizeDccPourBlocDepenses(allDccClotures.data, false);
            Notify.message(resources.Notif_DeclotureDepenses_Succes);
        }

        function onDecloturerDepenseSuccess(allDccClotures) {
            if (allDccClotures.data.length > 0) {
                var dccDecloturee = ctrl.dccList.find(d => d.DateClotureComptableId === allDccClotures.data[0].DateClotureComptableId);
                dccDecloturee.isDccSelectionneePourCloturerDepenses = dccDecloturee.isDccSelectionneePourDecloturerDepenses = false;
            }
            synchronizeDccPourBlocDepenses(allDccClotures.data, false);
            Notify.message(resources.Notif_DeclotureDepense_Succes);
        }

        function synchronizeDccPourBlocDepenses(dccListBaseSynchro, isMajDccSelectionneePourCloturerDepenses) {
            angular.forEach(dccListBaseSynchro, function (dccBaseSynchro, key) {
                if (dccBaseSynchro) {
                    var dcc = $filter('filter')(ctrl.dccList, { CiId: dccBaseSynchro.CiId }, true)[0];
                    if (dcc) {
                        if (!dccBaseSynchro.DateCloture) {
                            dcc.DateCloture = dccBaseSynchro.DateCloture;
                        }
                        else {
                            dcc.DateCloture = $filter('toLocaleDate')(dccBaseSynchro.DateCloture);
                        }
                        if (isMajDccSelectionneePourCloturerDepenses) {
                            dcc.isDccSelectionneePourCloturerDepenses = false;
                        }
                    }
                }
            });
        }

        function activeAllButtons() {
            ctrl.isBusy = false;
            ProgressBar.complete();
        }

        function unactiveAllButtons() {
            ProgressBar.start();
            ctrl.isBusy = true;
        }

        function displayFilterState() {
            return (!ctrl.filter.transfertFar || ctrl.filter.transfertFar.includes('Tous') ) && (!ctrl.filter.clotureDepenses || ctrl.filter.clotureDepenses.includes('Tous'))
                && (!ctrl.filter.validationAvancement || ctrl.filter.validationAvancement.includes('Tous')) && (!ctrl.filter.clotureSurLaPeriode || ctrl.filter.clotureSurLaPeriode.includes('Tous'))
                && (!ctrl.filter.validationControleBudgetaire || ctrl.filter.validationControleBudgetaire.includes('Tous')) && ctrl.filter.dejaTermine !== true;
        }

        function isInModeToutSelectionner(lstFiltree) {
            if (ctrl.dccList.length === lstFiltree.length) {
                return true;
            }
        }
    }
})(angular);