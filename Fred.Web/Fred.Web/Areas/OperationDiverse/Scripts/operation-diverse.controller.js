(function (angular) {
    'use strict';

    angular.module('Fred').controller('OperationDiverseController', OperationDiverseController);

    OperationDiverseController.$inject = ['$scope', '$q', 'Notify', 'ProgressBar', 'OperationDiverseService', 'DatesClotureComptableService', 'CIService', '$uibModal', 'fredDialog', 'FeatureFlags', 'favorisService', 'UserService', '$window', '$timeout'];

    function OperationDiverseController($scope, $q, Notify, ProgressBar, OperationDiverseService, DatesClotureComptableService, CIService, $uibModal, fredDialog, FeatureFlags, favorisService, UserService, $window, $timeout) {
        var $ctrl = this;

        angular.extend($ctrl, {
            handleSearch: handleSearch,
            handleCumul: handleCumul,
            handleDebut: handleDebut,
            handleAddFilter2Favoris: handleAddFilter2Favoris,
            handleProcessEcart: handleProcessEcart,
            handleChangePeriod: handleChangePeriod

        });

        $ctrl.popupOpened = false;
        $ctrl.featureFlagActive = FeatureFlags.getFlagStatus('MultiplePeriodeOperationDiverses');

        // Instanciation Objet Ressources    
        $ctrl.resources = resources;
        $ctrl.importEcrituresAvailable = true;
        $ctrl.rechercheAvailable = false;
        $ctrl.isCumul = false;
        $ctrl.favoriId = null;

        // Valeurs du formulaire
        $ctrl.periodeDebut = new Date();
        $ctrl.periodeFin = $ctrl.periodeDebut;
        $ctrl.periodeCouranteDebut = $ctrl.periodeDebut;
        $ctrl.periodeCouranteFin = $ctrl.periodeFin;

        var dateSelectedYear;
        var dateSelectedMonth;

        $ctrl.periodeCourante = null;
        $ctrl.periodeEnCours = true;
        $ctrl.minDate = new Date('2018-10-01');
        $ctrl.uniquePeriodeCloturee = null;
        $ctrl.periodeClose = false;
        $ctrl.firstSearchHasOccurred = false;
        $ctrl.listPeriodClosed = [];
        $ctrl.listPeriodClosedLength = 0;
        $ctrl.operationDiverseTotalComptaLibelle = $ctrl.resources.OperationDiverse_total_Compta;

        $ctrl.action = "Ouvrir";
        $ctrl.btnValiderVisible = false;
        $("div[id^=actionChoix]").removeClass("selected");
        $(".inner-triangle").addClass("ng-hide");
        $scope.acceptedAttachmentFormat = ".xls, .xlsx, .csv";
        $ctrl.file = null;

        $scope.init = function init(CiId, Year, Month, favoriId) {
            $ctrl.favoriId = parseInt(favoriId) || 0;

            if ($ctrl.favoriId !== 0)
                getFilter();
            else if (CiId && Year && Month) {
                if (!$ctrl.periodeDebut && !$ctrl.periodeFin) {
                    $ctrl.periode = new Date(Year, Month - 1);
                }
                $ctrl.firstSearchHasOccurred = true;
                $ctrl.periodeCourante = $ctrl.periode;
                $ctrl.periodeDebut = new Date(Year, Month - 1);
                $ctrl.periodeFin = new Date(Year, Month - 1);
                $ctrl.periodeCouranteDebut = $ctrl.periodeDebut;
                $ctrl.periodeCouranteFin = $ctrl.periodeFin;
                ProgressBar.start();
                CIService.GetById({ ciId: CiId }).$promise
                    .then(function (response) {
                        $ctrl.ciSelected = response;
                        consolidationCriteriasChanged().finally(function () {
                            verifyIfRechercheIsAvailable();
                            $ctrl.isBusy = false;
                            ProgressBar.complete();
                        });
                    }).catch(function (error) { Notify.error(error); });
            }
            else {
                $ctrl.periodeDebut = new Date();
                $ctrl.periodeFin = $ctrl.periodeDebut;
            }

            UserService.getCurrentUser().then(function (user) {
                if (user.Personnel.Societe.Groupe.Code === 'GFTP') {
                    $ctrl.operationDiverseTotalComptaLibelle = $ctrl.resources.OperationDiverse_total_Compta_FTP;
                }
            });
        };

        document.onkeydown = function () {
            if ($ctrl.isBusy && event.keyCode === 116) {
                $window.alert("Veuillez attendre la fin du traitement");
                return false;
            }
        };

        $window.onbeforeunload = function () {
            if ($ctrl.isBusy) {
                return false;
            }
        };

        $ctrl.consolidationLines = null;
        $ctrl.isBusy = false;

        if ($ctrl.periodeDebut.toString() === $ctrl.periodeFin.toString()) {
            $ctrl.periodeRange = $ctrl.periodeDebut;

        }

        $ctrl.handleLookupUrl = function (val) {
            var baseControllerUrl = '/api/' + val + '/SearchLight';
            return baseControllerUrl;
        };

        $q.when()
            .then(function () { $ctrl.isBusy = true; ProgressBar.start(); })
            .then(consolidationCriteriasChanged)
            .then(function () { $ctrl.isBusy = false; ProgressBar.complete(); });


        function handleProcessEcart() {

            $uibModal.open({
                templateUrl: "/Areas/OperationDiverse/Scripts/modals/ecart-modal.tpl.html",
                backdrop: "static",
                controller: 'EcartModalController',
                size: "md",
                controllerAs: '$ctrl',
                resolve: {
                    ciSelected: function () {
                        return $ctrl.ciSelected;
                    },
                    listPeriodClosed: function () {
                        return $ctrl.listPeriodClosedWhithEcart;
                    },
                    periodeDebut: function () {
                        return $ctrl.periodeDebut;
                    },
                    periodeFin: function () {
                        return $ctrl.periodeFin;
                    }
                }
            });
        }

        /**
         * Selected CI changed
         */
        $ctrl.ciSelectedChanged = function () {
            OperationDiverseService.GetCiSocietyId($ctrl.ciSelected.Organisation.OrganisationId)
                .then(function (response) {
                    $ctrl.ciSelected.SocieteId = response.data;
                    limitsSelectedDate();
                    verifyIfRechercheIsAvailable();
                })
                .catch(function (error) { Notify.error(error); });
        };

        function verifyIfRechercheIsAvailable() {
            if ($ctrl.ciSelected && $ctrl.ciSelected.SocieteId) {
                if ($ctrl.isCumul && ($ctrl.periodeDebut === null || $ctrl.periodeDebut === undefined) && $ctrl.periodeFin) {
                    $ctrl.rechercheAvailable = true;
                }
                else if (!$ctrl.isCumul && $ctrl.periodeDebut && $ctrl.periodeFin) {
                    $ctrl.rechercheAvailable = true;
                }
                else if ($ctrl.periodeDebut && $ctrl.periodeFin) {
                    $ctrl.rechercheAvailable = true;
                }
                else {
                    $ctrl.rechercheAvailable = false;
                }
            }
            else {
                $ctrl.rechercheAvailable = false;
            }
        }

        function handleChangePeriod() {
            verifyIfRechercheIsAvailable();
        }

        function handleSearch() {
            if ($ctrl.ciSelected) {
                ProgressBar.start();
                $ctrl.listPeriodClosed = [];
                $ctrl.listPeriodClosedLength = 0;
                $ctrl.uniquePeriodeCloturee = null;
                $ctrl.periodeClose = false;
                $ctrl.ecartForClosedPeriod = false;
                $ctrl.firstSearchHasOccurred = true;

                return $q.when()
                    .then(function () { $scope.$broadcast('operationDiverseCtrl.SelectedPeriodStart', $ctrl.periodeDebut); })
                    .then(function () { $scope.$broadcast('operationDiverseCtrl.SelectedPeriodEnd', $ctrl.periodeFin); })
                    .then(consolidationCriteriasChanged).finally(function () { $ctrl.isBusy = false; ProgressBar.complete(); });

            }
        }

        function handleCumul() {
            $ctrl.isCumul = !$ctrl.isCumul;

            displayResultOperationDiverse();

            verifyIfRechercheIsAvailable();
        }

        function displayResultOperationDiverse() {
            $ctrl.listPeriodClosed = [];
            if ($ctrl.ciSelected) {
                if (!$ctrl.isCumul) {
                    if ($ctrl.periodeDebut === null || $ctrl.periodeDebut === undefined) {
                        $ctrl.periodeDebut = $ctrl.periodeFin;
                    }
                }
            }

            resetPeriodeDebutWhenCumul();
        }

        function resetPeriodeDebutWhenCumul() {
            if ($ctrl.isCumul) {
                $ctrl.periodeDebut = null;
            }
        }

        function handleDebut() {
            $ctrl.periodeFin = $ctrl.periodeDebut;
            $ctrl.periodeCouranteFin = $ctrl.periodeFin;
            $ctrl.periodeCouranteDebut = $ctrl.periodeCouranteFin;
            $ctrl.listPeriodClosed = [];
        }

        function handleAddFilter2Favoris() {
            addFilter2Favoris();
        }

        function addFilter2Favoris() {
            var filterToSave = { CI: $ctrl.ciSelected, PeriodeDebut: $ctrl.periodeDebut, PeriodeFin: $ctrl.periodeFin, IsCumul: $ctrl.isCumul };
            var url = $window.location.pathname;
            if ($ctrl.favoriId !== 0) {
                url = $window.location.pathname.slice(0, $window.location.pathname.lastIndexOf("/"));
            }
            favorisService.initializeAndOpenModal("OperationDiverse", url, filterToSave);
        }

        function getFilter() {
            if ($ctrl.favoriId !== 0) {
                ProgressBar.start();
                $ctrl.periodeDebut = $ctrl.periodeFin = null;
                return favorisService.getFilterByFavoriId($ctrl.favoriId)
                    .then(function (response) {
                        $ctrl.ciSelected = response.CI;
                        $ctrl.periodeDebut = response.PeriodeDebut;
                        $ctrl.periodeFin = response.PeriodeFin;
                        $ctrl.isCumul = response.IsCumul;
                    })
                    .then(function () {
                        $ctrl.ciSelectedChanged();
                        handleSearch();
                    })
                    .catch(function () { Notify.error($ctrl.resources.Global_Notification_Error); })
                    .finally(() => {
                        ProgressBar.complete();
                    });
            }
        }

        function limitsSelectedDate() {
            if (moment($ctrl.ciSelected.DateOuverture).isBefore('2018-10-01')) {
                $ctrl.ciSelected.DateOuverture = new Date('2018-10-01');
            }
            //Commenter pour resoudre le bug 8364 desactivation de la RG006 US 5995
            //else {
            //    $ctrl.minDate = $ctrl.ciSelected.DateOuverture;
            //}
        }

        function consolidationCriteriasChanged() {
            if ($ctrl.isBusy) {
                return;
            }

            if ($ctrl.periode) {
                sessionStorage.setItem('operationDiverseFilter', JSON.stringify({ Ci: $ctrl.ciSelected, Periode: $ctrl.periode }));

                var date = new Date($ctrl.periode);
                dateSelectedYear = date.getFullYear();
                dateSelectedMonth = date.getMonth() + 1;
                return $q.all([getPeriodStatus(), canImportEcritureComptables(), getConsolidationDatas()]);
            }

            else {
                $ctrl.isBusy = true;
                return $q.all([getPeriodStatus(), canImportEcritureComptables(), getConsolidationDatas()])
                    .finally(function () {
                        if ($ctrl.periodeDebut.toString() === $ctrl.periodeFin.toString()) {
                            $ctrl.periodeRange = $ctrl.periodeDebut;
                        }
                    });
            }

        }

        function getPeriodStatus() {
            if ($ctrl.ciSelected && $ctrl.periode && !$ctrl.featureFlagActive) {
                DatesClotureComptableService.GetPeriodStatus($ctrl.ciSelected.CiId, dateSelectedYear, dateSelectedMonth)
                    .then(function (status) {
                        $ctrl.periodeEnCours = !status.data;
                    })
                    .catch(function (error) { Notify.error(error); $ctrl.isBusy = false; });
            }
            else if ($ctrl.ciSelected && $ctrl.featureFlagActive) {
                DatesClotureComptableService.GetPeriodStatusForRange($ctrl.ciSelected.CiId, $ctrl.periodeDebut !== undefined ? moment($ctrl.periodeDebut).format('YYYY-MM-DD') : null, moment($ctrl.periodeFin).format('YYYY-MM-DD'))
                    .then(function (status) {
                        $ctrl.listPeriodClosed = [];

                        angular.forEach(status.data, function (val) {
                            var o = { Mois: val.Mois, Annee: val.Annee, CiId: $ctrl.ciSelected.CiId, Option: null };
                            $ctrl.listPeriodClosed.push(o);
                            $ctrl.listPeriodClosedLength++;
                            $ctrl.periodeClose = true;
                        });

                    })
                    .catch(function (error) { Notify.error(error); $ctrl.isBusy = false; });
            }
        }

        function checkEcartForClosedPeriod() {
            // Pour chaque periode 
            $ctrl.listPeriodClosedWhithEcart = [];
            var index = 0;
            angular.forEach($ctrl.listPeriodClosed, function (periode) {
                var periodeClosed = moment(periode.Annee + '-' + periode.Mois + '-' + 15).format('YYYY-MM-DD');

                angular.forEach($ctrl.consolidationLines.FamiliesAmounts, function (family) {

                    angular.forEach(family.ListGapAmountByMonth, function (gap) {

                        if (moment(gap.m_Item1).format('YYYY-MM-DD') === periodeClosed && gap.m_Item2 !== 0) {
                            var periodeEcart = {
                                Mois: parseInt(moment(gap.m_Item1).format('M')),
                                Annee: parseInt(moment(gap.m_Item1).format('YYYY')),
                                CiId: $ctrl.ciSelected.CiId,
                                Option: null,
                                index: index
                            };
                            $ctrl.listPeriodClosed.push(periodeEcart);
                            $ctrl.listPeriodClosedWhithEcart.push(periodeEcart);
                            index++;
                            $ctrl.ecartForClosedPeriod = true;
                        }
                    });
                });
            });
            $ctrl.listPeriodClosed = removeDuplicates($ctrl.listPeriodClosed, "Mois");
            $ctrl.listPeriodClosedLength = $ctrl.listPeriodClosed.length;
            $ctrl.listPeriodClosedWhithEcart = removeDuplicates($ctrl.listPeriodClosedWhithEcart, "Mois");
            $ctrl.listPeriodClosedWhithEcart = $ctrl.listPeriodClosedWhithEcart.sort((a, b) => a.index > b.index ? 1 : -1);
            getUniquePeriodeCloturee();
        }

        function removeDuplicates(originalArray, prop) {
            var newArray = [];
            var lookupObject = {};

            for (var i in originalArray) {
                lookupObject[originalArray[i][prop]] = originalArray[i];
            }

            for (i in lookupObject) {
                newArray.push(lookupObject[i]);
            }
            return newArray;
        }

        /**
         * Vérifie si l'on peut importer les écritures comptables
         */
        function canImportEcritureComptables() {
            if ($ctrl.ciSelected && $ctrl.ciSelected.SocieteId && $ctrl.periode) {
                $ctrl.importEcrituresAvailable = OperationDiverseService.CanImportEcritureComptables($ctrl.ciSelected.SocieteId, $ctrl.ciSelected.CiId, moment($ctrl.periode).format('YYYY-MM-DD'))
                    .then(canImportEcritureComptablesOnSuccess)
                    .catch(function () { $ctrl.isBusy = false; return false; });
            }
            else if ($ctrl.ciSelected && $ctrl.ciSelected.SocieteId && ($ctrl.periodeDebut && $ctrl.periodeFin)) {
                $ctrl.importEcrituresAvailable = true;
            }
        }

        function canImportEcritureComptablesOnSuccess(response) {
            $ctrl.importEcrituresAvailable = response.data.canImport;
            return response.data && response.data.canImport;
        }

        /**
         * Récupère les informations de consolidation pour le CI et la période, par famille d'OD
         * @returns {any} Lignes de consolidation
         */
        function getConsolidationDatas() {
            if ($ctrl.ciSelected && $ctrl.periode) {
                $ctrl.consolidationLines = null;
                $ctrl.uniquePeriodeCloturee = null;
                return OperationDiverseService.GetConsolidationDatas($ctrl.ciSelected.CiId, moment($ctrl.periode).format('YYYY-MM-DD'))
                    .then(getConsolidationDatasOnSuccess)
                    .catch(getConsolidationDatasOnError);
            }
            else if ($ctrl.ciSelected && !$ctrl.periode) {
                $ctrl.consolidationLines = null;

                if ($ctrl.periodeDebut === undefined) {
                    $ctrl.periodeDebut = null;
                }

                if (FeatureFlags.getFlagStatus('ActivationUS13085_6000')) {
                    // Utilisé pour la redirection vers l'explorateur de dépenses
                    setFormatedDate($ctrl.periodeDebut, $ctrl.periodeFin);
                }

                return OperationDiverseService.GetConsolidationDatasWithRange($ctrl.ciSelected.CiId, moment($ctrl.periodeDebut).format('YYYY-MM-DD'), moment($ctrl.periodeFin).format('YYYY-MM-DD'))
                    .then(getConsolidationDatasOnSuccess)
                    .then(checkEcartForClosedPeriod)
                    .catch(getConsolidationDatasOnError);
            }
        }

        function getConsolidationDatasOnSuccess(response) {
            $ctrl.consolidationLines = response.data;
            $ctrl.isBusy = false;
        }

        function getConsolidationDatasOnError() {
            Notify.error($ctrl.resources.Global_Notification_Error);
            $ctrl.isBusy = false;
        }

        const monthNames = ["", "Janvier", "Février", "Mars", "Avril", "Mai", "Juin", "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre"];

        /**
         * Import des écritures comptables
         */
        $ctrl.importEcritureComptables = function () {
            if ($ctrl.isBusy) {
                return;
            }
            $ctrl.isBusy = true;

            if ($ctrl.periode && !$ctrl.featureFlagActive) {
                ProgressBar.start();
                importEcritureComptable();
            }
            else if ($ctrl.featureFlagActive) {
                var message = 'Importation des écritures du ' + moment($ctrl.periodeDebut).format('MM/YYYY') + ' au ' + moment($ctrl.periodeFin).format('MM/YYYY');
                if (!$ctrl.ciSelected) {
                    fredDialog.information($ctrl.resources.OperationDiverse_ImportationEcritureCompatble_CiNotExist, $ctrl.resources.OperationDiverse_FichierChargerFailed, '', $ctrl.resources.OperationDiverse_Annuler, '', function () { $ctrl.importEcrituresAvailable = true; $ctrl.isBusy = false; return false; }, '', '', '', 'userFileWindow');
                }
                else if ($ctrl.listPeriodClosedWhithEcart.length > 0 || $ctrl.listPeriodClosed.length > 0) {
                    if ($ctrl.listPeriodClosedWhithEcart.length === 0) {
                        $ctrl.listPeriodClosedWhithEcart = $ctrl.listPeriodClosed;
                    }
                    else {
                        angular.forEach($ctrl.listPeriodClosed, function (value) {
                            $ctrl.listPeriodClosedWhithEcart.push(value);
                        });
                    }
                    $ctrl.listPeriodClosedWhithEcart = removeDuplicates($ctrl.listPeriodClosedWhithEcart, "Mois");
                    message += '<br/ > Il existe au moins une période comptable clôturée';
                    message += '<ul>';
                    angular.forEach($ctrl.listPeriodClosedWhithEcart, function (periodClose) {
                        message += '<li>' + monthNames[periodClose.Mois] + " - " + periodClose.Annee + '</li>';
                    });
                    message += '</ul>';
                    fredDialog.confirmation(message, 'Confirmation d\'importation ', 'falticon flaticon flaticon-shuffle-1', $ctrl.resources.OperationDiverse_Valider, $ctrl.resources.OperationDiverse_Annuler, importEcritureComptable, function () { $ctrl.importEcrituresAvailable = true; $ctrl.isBusy = false; return false; });
                }
                else {
                    fredDialog.confirmation(message, 'Confirmation d\'importation ', 'falticon flaticon flaticon-shuffle-1', $ctrl.resources.OperationDiverse_Valider, $ctrl.resources.OperationDiverse_Annuler, importEcritureComptable, function () { $ctrl.importEcrituresAvailable = true; $ctrl.isBusy = false; return false; });
                }
                $ctrl.isBusy = false;
            }
        };

        function importEcritureComptable() {
            ProgressBar.start();
            if ($ctrl.periode) {
                $ctrl.isBusy = true;
                OperationDiverseService.ImportEcritureComptables($ctrl.ciSelected.SocieteId, $ctrl.ciSelected.CiId, moment($ctrl.periode).format('YYYY-MM-DD'))
                    .then(importEcritureComptablesOnSuccess)
                    .catch(importEcritureComptablesOnError)
                    .finally(importEcritureComptablesFinally);
            }
            else {
                $ctrl.isBusy = true;
                OperationDiverseService.ImportEcritureComptablesRange($ctrl.ciSelected.SocieteId, $ctrl.ciSelected.CiId, moment($ctrl.periodeDebut).format('YYYY-MM-DD'), moment($ctrl.periodeFin).format('YYYY-MM-DD'))
                    .then(importEcritureComptablesOnSuccess)
                    .catch(importEcritureComptablesOnError)
                    .finally(importEcritureComptablesFinally);
            }

        }

        function importEcritureComptablesOnSuccess() {
            // Je viens de lancer l'import, il n'est donc pas terminé.
            $ctrl.importEcrituresAvailable = false;
        }

        function importEcritureComptablesOnError() {
            $ctrl.importEcrituresAvailable = false;
            $ctrl.isBusy = false;
            Notify.error($ctrl.resources.Global_Notification_Error);
        }

        function importEcritureComptablesFinally() {
            ProgressBar.complete();
            $ctrl.isBusy = false;
        }

        function getUniquePeriodeCloturee() {
            $ctrl.uniquePeriodeCloturee = null;
            if ($ctrl.listPeriodClosed.length === 1) {
                $ctrl.uniquePeriodeCloturee = moment($ctrl.listPeriodClosed[0].Annee + '-' + $ctrl.listPeriodClosed[0].Mois + '-' + 15).format('YYYY-MM-DD');
            }
        }

        /**
         * Ouverture de l'écran de saisie des OD en fonction de la famille sélectionnée
         * @param {any} family Famille d'OD sélectionnée
         */
        $ctrl.openODofFamily = function (family) {
            if ($ctrl.periodeEnCours || $ctrl.listPeriodClosed) {
                var dateSelectedYear;
                var dateSelectedMonth;
                var date;
                if (family.IsAccrued) {
                    var modalInstance = $uibModal.open({
                        templateUrl: "/Areas/OperationDiverse/Scripts/modals/ventilation-modal.tpl.html",
                        backdrop: "static",
                        controller: 'VentilationModalController',
                        windowClass: 'ventilation-modal',
                        controllerAs: '$ctrl',
                        resolve: {
                            family: function () {
                                return family;
                            },
                            currencySymbol: function () {
                                return $ctrl.consolidationLines.CurrencySymbol;
                            },
                            ciSelected: function () {
                                return $ctrl.ciSelected;
                            },
                            dateSelected: function () {
                                return !$ctrl.periode ? $ctrl.periodeRange : $ctrl.periode;
                            },
                            isClosed: function () {
                                return !$ctrl.periodeEnCours ? $ctrl.listPeriodClosed : $ctrl.periodeEnCours;
                            },
                            consolidationLines: function () {
                                return $ctrl.consolidationLines;
                            }
                        }
                    });
                    modalInstance.result.then(function () { getConsolidationDatas(); });
                }
                else {
                    if ($ctrl.periode) {
                        date = new Date($ctrl.periode);
                        dateSelectedYear = date.getFullYear();
                        dateSelectedMonth = date.getMonth() + 1;
                    }
                    else if ($ctrl.periodeRange) {
                        date = new Date($ctrl.periodeRange);
                        dateSelectedYear = date.getFullYear();
                        dateSelectedMonth = date.getMonth() + 1;
                    }
                    window.location.href = '/OperationDiverse/OperationDiverse/Detaille/?societeid=' + $ctrl.ciSelected.SocieteId + '&id=' + $ctrl.ciSelected.CiId + '&year=' + dateSelectedYear + '&month=' + dateSelectedMonth + "&famille=" + family.FamilyId + "&codeFamille=" + family.FamilyCode;
                }
            }
        };

        $ctrl.choixClick = function (el, actionType) {


            var currentEl = el.currentTarget;

            $("div[id^=actionChoix]").removeClass("selected");
            $(".inner-triangle").addClass("ng-hide");

            actionType === "Ouvrir" ? $(currentEl).addClass("selected") : $(currentEl).parent("div").addClass("selected");
            $(currentEl).closest("div").find(".inner-triangle").removeClass("ng-hide");

            $ctrl.action = actionType;
            // Affichage bouton validation
            $ctrl.btnValiderVisible = true;
        };

        $ctrl.continuer = function () {
            if ($ctrl.action === "Ouvrir")
                ouvrirExempleExcelOD();
            else
                importFichier();
        };

        function importFichier() {
            if ($ctrl.file) {
                ProgressBar.start();
                $ctrl.isBusy = true;

                OperationDiverseService.ImportOperationDiverses(moment($ctrl.periode).format('YYYY-MM-DD'), $ctrl.file)
                    .then(importResult => {
                        if (importResult.valid) {
                            fredDialog.information($ctrl.resources.OperationDiverse_FichierChargerSucces, $ctrl.resources.OperationDiverse_FichierChargerSuccesTitle, 'falticon flaticon flaticon-shuffle-1', $ctrl.resources.Global_Bouton_Valider, '', function () { if ($ctrl.ciSelected) $ctrl.ciSelectedChanged(); return false; });
                        }
                        else {
                            OperationDiverseService.PostImportODResult(importResult.errors)
                                .then(postResult => {
                                    fredDialog.confirmation($ctrl.resources.OperationDiverse_FichierChargerEchec,
                                        $ctrl.resources.OperationDiverse_FichierChargerEchecTitle, 'falticon flaticon flaticon-shuffle-1', $ctrl.resources.OperationDiverse_FichierChargerEchecBouton, $ctrl.resources.OperationDiverse_Annuler, _ => {
                                            if (postResult.data !== null) OperationDiverseService.GetImportODResult(postResult.data.id);
                                        }, '');
                                });
                        }
                    })
                    .catch(e => {
                        Notify.error($ctrl.resources.Global_Notification_Error);
                    })
                    .finally(function () {
                        ProgressBar.complete();
                        $ctrl.isBusy = false;
                    });
            }
            else {
                Notify.error($ctrl.resources.OperationDiverse_AucunFichier);
            }
        }

        function ouvrirExempleExcelOD() {
            if ($ctrl.ciSelected) {
                ProgressBar.start();
                OperationDiverseService.PostExempleExcelOD($ctrl.ciSelected.CiId, moment($ctrl.periodeFin).format('YYYY-MM-01')).then(r => {
                    if (r.data === null) {
                        Notify.error($ctrl.resources.Global_Notification_AucuneDonnees);
                    }
                    else {
                        OperationDiverseService.GetExempleExcelOD(r.data.id);
                    }
                }).catch(e => {
                    Notify.error($ctrl.resources.Global_Notification_Error);
                }).finally(ProgressBar.complete());
            }
            else {
                Notify.error($ctrl.resources.OperationDiverse_centre_d_imputation);
            }
        }

        $scope.handleSelectFile = (event) => {
            $timeout(function () { angular.noop(); }, 0);

            if (event.target.files && event.target.files[0]) {
                var file = event.target.files[0];
                var formData = new FormData();
                formData.append("file", file, file.name);
                $ctrl.file = formData;
            }
        };

        function setFormatedDate(dateDebut, dateFin) {
            if (dateDebut !== null) {
                $ctrl.periodeDebutFormatee = moment(dateDebut).format('YYYY-MM-DD');
            }
            else {
                $ctrl.periodeDebutFormatee = dateDebut;
            }
            
            $ctrl.periodeFinFormatee = moment(dateFin).format('YYYY-MM-DD');
        }
    }
})(angular);