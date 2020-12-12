(function (angular) {
    'use strict';

    angular.module('Fred').controller('RapportListeController', RapportListeController);

    RapportListeController.$inject = ['$scope', '$q', 'Notify', 'authorizationService', 'RapportService', 'ProgressBar', 'favorisService', 'fredDialog', 'UserService', '$filter', '$window', 'OrganisationService', '$uibModal'];

    function RapportListeController($scope, $q, Notify, authorizationService, RapportService, ProgressBar, favorisService, fredDialog, UserService, $filter, $window, OrganisationService, $uibModal) {
        // Init color selector pour favori
        $('#favoricolorselector').colorselector();

        // Instanciation Objet Ressources
        $scope.resources = resources;
        $scope.busy = false;
        const codeSOMOPA = "0143";

        var oldFilters = null;
        $scope.permissionKeys = PERMISSION_KEYS;
        $scope.getFilterOrFavoris = getFilterOrFavoris;
        $scope.addFilter2Favoris = addFilter2Favoris;
        $scope.favoriId = 0;
        $scope.AuthorType = { AuteurCreation: "AuteurCreation", Valideur1: "ValideurCDC", Valideur2: "ValideurCDT", Valideur3: "ValideurDRC", Verrouilleur: "AuteurVerrou" };

        UserService.getCurrentUser().then(function (user) {
            $scope.isSomopa = user.Personnel.Societe.Code.trim() === codeSOMOPA ? true : false;
            $scope.isUserFes = user.Personnel.Societe.Groupe.Code.trim() === 'GFES' ? true : false;
        });

        $scope.IsValid = true;
        $scope.IsRolePaie = false;
        $scope.forms = {};
        $scope.etatPaieExportModel = null;
        $scope.Societe = null;
        $scope.statutPersonnelList = [];
        $scope.isAllEtablissementFilter = true;
        $scope.paging = { page: 1, pageSize: 20, hasMorePage: false };
        $scope.currentDate = new Date();
        $scope.currentExportation = null;
        $scope.selectedRapports = [];
        $scope.allSelected = false;
        $scope.isUserHasMenuEditionPermission = false;
        $scope.permissionPrimesMensuellesVisible = false;
        $scope.ModeControleSaisie = false;
        $scope.isErrorsDispo = false;
        $scope.errorsTibco = [];
        $scope.exportation =
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
        $scope.ExportAnaModel =
        {
            Simulation: false,
            DateDebut: new Date(),
            Hebdo: false,
            Societe: undefined,
            AllEtab: true,
            EtablissementComptaList: [],
            Etablissement: {},
            SelectedWeek: ""
        };

        UserService.getCurrentUser().then(function (user) {
            $scope.userOrganizationId = user.Personnel.Societe.Organisation.OrganisationId;
        });

        // Action d'initialisation de l'écran
        $scope.firstLoad = function () {
            InitUserPermissions();
            $q.when()
                .then(ProgressBar.start)
                .then(actionIsRolePaie)
                .then(IsUserHasMenuEditionPermission)
                .then(function () { return true; /* first load */ })
                .then(actionGetRapports)
                .then(ProgressBar.complete);

            // Chargement de rapports supplémentaire
            FredToolBox.bindScrollEnd('#rapport-list', actionLoadMore);
        };

        /* -------------------------------------------------------------------------------------------------------------
         *                                            HANDLERS
         * -------------------------------------------------------------------------------------------------------------
         */

        // new hebdomadaire date picker
        $scope.handleChangeCalendarDate = function () {
            actionGenerateSelectedWeekLabel();
        };

        $scope.handleChangeWeek = function (isAddWeek) {
            actionChangeWeek(isAddWeek);
        };

        $scope.handleClickNew = function () {
            actionNew();
        };

        $scope.handleClickDetail = function (rapportid) {
            window.location = "/Pointage/Rapport/Detail/" + rapportid;
        };

        $scope.handleDeleteRapport = function (rapport) {
            $q.when()
                .then(ProgressBar.start)
                .then(function () { return rapport; })
                .then(actionDeleteRapport)
                .finally(ProgressBar.complete);
        };

        $scope.handleDuplicate = function (rapportId) {
            actionDuplicate(rapportId);
        };

        $scope.handleVerrouillerRapport = function () {
            if (!$scope.isSomopa) {
                $q.when()
                    .then(ProgressBar.start)
                    .then(actionVerrouiller)
                    .then(function () { return true; })
                    .then(actionGetRapports)
                    .finally(ProgressBar.complete);
            }
            else {
                checkErrorOnRapportToLock();
            }
        };

        $scope.handleModeControle = function () {
            $scope.ModeControleSaisie = true;
            $scope.ExportAnaModel.Simulation = true;
        };

        $scope.handleModeAnalytique = function () {
            $scope.ModeControleSaisie = false;
        };

        $scope.handleDeverrouillerRapport = function () {
            $q.when()
                .then(ProgressBar.start)
                .then(actionDeverrouiller)
                .then(function () { return true; })
                .then(actionGetRapports)
                .finally(ProgressBar.complete);
        };

        $scope.handleLoadData = function () {
            $q.when()
                .then(ProgressBar.start)
                .then(function () { return true; })
                .then(actionGetRapports)
                .finally(ProgressBar.complete);
        };

        $scope.handleShowLookup = function (val) {
            if (val === "EtablissementPaie") {
                var url = '/api/EtablissementPaie/SearchLight/?page={0}&societeId={1}';
                url = String.format(url, 1, $scope.Societe.SocieteId);
                return url;
            }
        };

        $scope.handleLookupDeletion = function (val, item) {
            switch (val) {
                case "EtablissementPaieFilter":
                    var indexId = $scope.filters.EtablissementPaieIdList.indexOf(item.IdRef);
                    var indexItem = $scope.filters.EtablissementPaieList.indexOf(item);
                    $scope.filters.EtablissementPaieIdList.splice(indexId, 1);
                    $scope.filters.EtablissementPaieList.splice(indexItem, 1);
                    break;
                case "OrganisationFilter":
                    $scope.filters.OrganisationId = null;
                    $scope.filters.Organisation = null;
                    break;
                case "EtablissementComptable":
                    var indexEtab = $scope.ExportAnaModel.EtablissementComptaList.indexOf(item);
                    $scope.ExportAnaModel.EtablissementComptaList.splice(indexEtab, 1);
                    break;
            }
        };

        $scope.onSelectSocieteLookup = function (societe) {
            if (!societe) {
                return;
            }

            $scope.ExportAnaModel.EtablissementComptaList = [];
        };

        $scope.onSelectEtablissentLookup = function (etablissement) {
            if ($scope.ExportAnaModel.EtablissementComptaList === undefined) {
                $scope.ExportAnaModel.EtablissementComptaList = [];
            }
            if (!etablissement) {
                return;
            }
            $scope.ExportAnaModel.EtablissementComptaList.push(etablissement);
        };

        $scope.validerExportAnalytique = function () {
            if (!$scope.ExportAnaModel.AllEtab) {
                LancerServiceTibco();
            }
            else {
                $scope.ExportAnaModel.EtablissementComptaList = [];
                if ($scope.ExportAnaModel.Societe) {
                    RapportService.RecupererAllEtablissementCompta($scope.ExportAnaModel.Societe.OrganisationId).then(function (response) {
                        if (response) {
                            $scope.ExportAnaModel.EtablissementComptaList = response;
                            LancerServiceTibco();
                        }
                        else {
                            Notify.message(resources.Rapport_ModalImport_ErrorMessage_Organisation);
                        }
                    });
                }
            }
        };

        // Hanlde select établissement pour le filtre
        $scope.handleSelectChoseEtablissementForFilter = function () {
            $scope.filters.EtablissementPaieIdList = $scope.filters.EtablissementPaieList.map(x => x.EtablissementPaieId);
            RapportService.GetSocieteByOrganisationId($scope.filters.OrganisationId).then(function (response) {
                $scope.Societe = response.data;
            });
        };

        // ça conçerne la vue des filtres
        $scope.handleSelectAllEtablissementForFilter = function () {
            $scope.filters.EtablissementPaieIdList = [];
        };

        $scope.handleLookupSelection = function (val, item) {
            switch (val) {
                case "EtablissementPaieFilter":
                    if ($scope.filters.EtablissementPaieIdList.indexOf(item.IdRef) > -1) {
                        Notify.error($scope.resources.Rapport_ModalImport_EtablissementPaie_DejaChoisi);
                    }
                    else {
                        $scope.filters.EtablissementPaieIdList.push(item.IdRef);
                        $scope.filters.EtablissementPaieList.push(item);
                    }
                    break;
                case "OrganisationFilter":
                    $scope.filters.OrganisationId = item.OrganisationId;
                    $scope.filters.Organisation = item;
                    $scope.filters.EtablissementPaieIdList = [];
                    $scope.filters.EtablissementPaieList = [];
                    $scope.isAllEtablissementFilter = true;
                    break;
            }
        };

        $scope.handleSelectAll = function () {
            angular.forEach($scope.listRapport, function (rapport) {
                actionSelectRapport(rapport, $scope.allSelected);
            });

            $scope.totalCountViewModel = $scope.totalCount;
        };

        /**
         * Gestion de la sélection des rapports
         * @param {any} rapport Rapport
         * @param {any} value Valeur affectée au checkbox
         */
        $scope.handleSelectRapport = function (rapport, value) {
            actionSelectRapport(rapport, value);
            if ($scope.allSelected) {
                $scope.totalCountViewModel += rapport.IsSelected ? 1 : -1;
            }
        };

        $scope.handleSelectExport = function (exportation) {

            var modalInstance = $uibModal.open({
                animation: true,
                component: 'editionPaieComponent',
                size: 'lg',
                resolve: {
                    etatPaieExportModel: function () { return $scope.etatPaieExportModel },
                    currentExportation: function () { return exportation; },
                    resources: function () { return $scope.resources; },
                    isUserFes: function () { return $scope.isUserFes; },
                    userOrganizationId: function () { return $scope.userOrganizationId }
                }
            });

            modalInstance.result.then(function (etatPaieExportModel) {
                $scope.etatPaieExportModel = etatPaieExportModel;
            });
        }

        // Réinitialise le Modal
        $scope.closeModal = function () {
            $scope.IsValid = true;
        };

        /* Handle pour annuler les changements dans les filtres */
        $scope.handleCancelFilters = function () {
            $q.when()
                .then(cancelFilters);
        };

        /* Handle pour sauvegarder les filtres avant la recherche avancée */
        $scope.handleSaveFilters = function () {
            $q.when()
                .then(saveFilters);
        };

        $scope.handleResetFilter = function () {
            $q.when()
                .then(function () { sessionStorage.removeItem('rapportListFilter'); })
                .then(getFilterOrFavoris);
        };

        $scope.displayFilterState = function () {
            if ($scope.filters) {
                return ($scope.filters.Organisation !== null && $scope.filters.Organisation !== undefined) || $scope.filters.StatutEnCours || $scope.filters.StatutValide1 || $scope.filters.StatutValide2 || $scope.filters.StatutValide3 || $scope.filters.StatutVerrouille || $scope.filters.DateComptable !== $scope.filters.DateChantierMin && !$scope.filters.DateComptable && $scope.filters.DateChantierMin !== undefined || $scope.filters.DateChantierMax || $scope.filters.DateComptable
                    || $scope.filters.Verrouilleur || $scope.filters.Valideur3 || $scope.filters.Valideur2 || $scope.filters.Valideur1 || $scope.filters.AuteurCreation;
            }

            return false;
        };

        /* -------------------------------------------------------------------------------------------------------------
         *                                            ACTIONS
         * -------------------------------------------------------------------------------------------------------------
         */

        /**
         * Action on checkbox click
         * @param {any} rapport rapport
         * @param {any} value force value (true or false)
         */
        function actionSelectRapport(rapport, value) {
            if (value !== undefined && value !== null) {
                rapport.IsSelected = value;
            }

            if (rapport) {

                var isAlreadyIn = $filter('filter')($scope.selectedRapports, { RapportId: rapport.RapportId }, true)[0];

                if (rapport.IsSelected) {
                    if (!isAlreadyIn) {
                        $scope.selectedRapports.push(rapport);
                    }
                }
                else {
                    var index = $scope.selectedRapports.indexOf(rapport);
                    $scope.selectedRapports.splice(index, 1);
                }
            }

            if (!$scope.paging.hasMorePage && $scope.selectedRapports.length === 0) {
                $scope.allSelected = false;
            }
        }

        /**
        * Ajouter une semaine à la semaine courante .
        * @param {any} isAddWeek indique si l'on doit ajouter une semaine
        */
        function actionChangeWeek(isAddWeek) {
            if (isAddWeek) {
                $scope.currentDate = moment($scope.currentDate).add(1, 'week').toDate();
            } else {
                $scope.currentDate = moment($scope.currentDate).subtract(1, 'week').toDate();
            }
            actionGenerateSelectedWeekLabel();
        }

        /**
        * Générer libelle pour la semaine selectionné
        */
        function actionGenerateSelectedWeekLabel() {
            var momentDate = moment($scope.currentDate);
            var mondayDayNumber = momentDate.startOf('isoWeek').format('DD');
            var sundayDayNumber = momentDate.isoWeekday(7).format('DD');
            var mondayMonth = momentDate.startOf('isoWeek').format('MM');
            var sundayMonth = momentDate.isoWeekday(7).format('MM');
            var mondayYear = momentDate.startOf('isoWeek').format('YYYY');
            var sundayYear = momentDate.isoWeekday(7).format('YYYY');

            var mondayMonthLabel = mondayMonth === sundayMonth ? '' : $scope.resources['Global_Month_' + mondayMonth];
            var sundayMonthLabel = $scope.resources['Global_Month_' + sundayMonth];
            var mondayYearLabel = mondayYear === sundayYear ? '' : mondayYear;

            $scope.mondayDayNumber = mondayDayNumber;
            $scope.mondayDate = new Date(mondayMonth + "/" + mondayDayNumber + "/" + mondayYear);
            $scope.selectedWeek = $scope.resources.Global_From + " " + mondayDayNumber + " " + mondayMonthLabel + " " + mondayYearLabel + " " +
                $scope.resources.Global_To + " " + sundayDayNumber + " " + sundayMonthLabel + " " + sundayYear;
            $scope.selectedWeek = $scope.selectedWeek[0].toUpperCase() + $scope.selectedWeek.slice(1).toLowerCase();
        }

        /**
         * Récupération des rapports selon les filtres 
         * @param {any} firstLoad premier chargement 
         * @returns {any} Promise
         */
        function actionGetRapports(firstLoad) {
            sessionStorage.setItem('rapportListFilter', JSON.stringify($scope.filters));

            if (firstLoad) {
                $scope.paging.page = 1;
                $scope.totalCount = 0;
            }

            return RapportService.SearchRapportWithFilters($scope.filters, $scope.paging.page, $scope.paging.pageSize)
                .then(function (data) {
                    $scope.paging.hasMorePage = data.Rapports.length === $scope.paging.pageSize;
                    $scope.totalCount = data.TotalCount;
                    if (firstLoad) {
                        $scope.listRapport = [];
                    }
                    angular.forEach(data.Rapports, function (val) {
                        // Si on a cliqué sur "Tout sélectionner", tous les rapports chargés par pagination seront sélectionnés par défaut
                        actionSelectRapport(val, $scope.allSelected);
                        $scope.listRapport.push(val);
                    });

                    for (var i = 0; i < $scope.listRapport.length; i++) {
                        var rapport = $scope.listRapport[i];
                        if (rapport.DateChantier) {
                            var date = new Date(rapport.DateChantier);
                            rapport.DateChantier = date;
                        }
                        initNomPrenom(rapport.AuteurCreation);
                        initNomPrenom(rapport.AuteurVerrou);
                        initNomPrenom(rapport.ValideurDRC);
                        initNomPrenom(rapport.ValideurCDC);
                        initNomPrenom(rapport.ValideurCDT);
                        initNomPrenom(rapport.ValideurGSP);
                    }

                    if (firstLoad) {
                        $scope.totalCountViewModel = data.TotalCount;
                    }
                })
                .catch(Notify.defaultError);
        }

        /**
         * Vérifie si on peut supprimer un rapport
         * @param {any} rapport rapport à vérifier
         * @returns {any} promise
         */
        function actionCanBeDeleted(rapport) {
            return RapportService.CanBeDeleted(rapport).then(function (value) { return value; }).catch(Notify.defaultError);
        }

        /**
         * Suppression d'un rapport
         * @param {any} rapport rapport à supprimer
         * @returns {any} promise
         */
        function actionDeleteRapport(rapport) {

            return $q.when()
                .then(function () { return rapport; })
                .then(actionCanBeDeleted)
                .then(function (canBeDeleted) {
                    if (canBeDeleted === true) {
                        fredDialog.confirmation(String.format($scope.resources.Rapport_RapportListeController_ConfirmationSuppression, rapport.RapportId), '', '', '', '',
                            function () {
                                return RapportService.DeleteRapport(rapport, true)
                                    .then(function () {
                                        Notify.message($scope.resources.Global_Notification_Suppression_Success);

                                        var i = $scope.listRapport.indexOf(rapport);
                                        if (i !== -1) {
                                            $scope.listRapport.splice(i, 1);
                                        }
                                    })
                                    .catch(function (reason) { Notify.defaultError(); console.log(reason); });
                            });
                    }
                    else {
                        fredDialog.information($scope.resources.Rapport_RapportListeController_SuppressionImpossible);
                    }
                })
                .catch(Notify.defaultError);
        }

        /**
         * Dupliquer un rapport
         * @param {any} rapportId Identifiant du rapport
         */
        function actionDuplicate(rapportId) {
            window.location = "/Pointage/Rapport/Detail/" + rapportId + "/true";
        }

        /**
         * Nouveau rapport : redirection vers URL 
         */
        function actionNew() {
            window.location = "/Pointage/Rapport/New";
        }

        /**
         * Vérrouiller un rapport
         * @returns {any} promise
         */
        function actionVerrouiller() {
            if ($scope.allSelected) {

                actionSettUnselectedRapports();
                return RapportService.VerrouillerAll($scope.filters)
                    .then(ManageLockReportsResponse)
                    .catch(Notify.defaultError);
            }

            return RapportService.VerrouillerList({ RapportList: $scope.selectedRapports, Filter: $scope.filters })
                .then(ManageLockReportsResponse)
                .catch(Notify.defaultError);
        }

        function ManageLockReportsResponse(value) {
            $scope.filters.UnselectedRapports = [];
            $scope.allSelected = false;
            Notify.message(resources.Rapport_RapportListeControler_Verrouiller_Success);
            showNotLockedReportsMessage(value);
        }

        /**
         * Déverrouiller un rapport
         * @returns {any} promise
         */
        function actionDeverrouiller() {
            if ($scope.allSelected) {

                actionSettUnselectedRapports();
                return RapportService.DeverrouillerAll($scope.filters)
                    .then(function () {
                        $scope.filters.UnselectedRapports = [];
                        $scope.allSelected = false;
                        Notify.message(resources.Rapport_RapportListeControler_Deverrouiller_Success);
                    })
                    .catch(Notify.defaultError);
            }

            return RapportService.DeverrouillerList($scope.selectedRapports)
                .then(function () {
                    $scope.filters.UnselectedRapports = [];
                    $scope.allSelected = false;
                    Notify.message(resources.Rapport_RapportListeControler_Deverrouiller_Success);
                })
                .catch(Notify.defaultError);
        }

        function actionSettUnselectedRapports() {
            $scope.filters.UnselectedRapports = $scope.listRapport.filter(r => !r.IsSelected);

            if ($scope.filters.UnselectedRapports && $scope.filters.UnselectedRapports.length > 0) {
                $scope.filters.UnselectedRapports = $scope.filters.UnselectedRapports.map(function (rapport) { return rapport.RapportId; });
            }
        }

        /**
         * Vérification si l'utilisateur connecté a un rôle paie
         * @returns {any} promise
         */
        function actionIsRolePaie() {
            //Initialisation de variables indiquant l'appartenance de l'utilisateur à des rôles concernant la paie
            return RapportService.IsRolePaie()
                .then(function (value) {
                    $scope.IsRolePaie = value;
                })
                .catch(Notify.defaultError);
        }

        /*
         * @function cancelFilters()
         * @description Annule tous les filtres
         */
        function cancelFilters() {
            $scope.filters = oldFilters;
        }

        /* 
         * @function    actionLoadMore()
         * @description Action Chargement de données supplémentaires (scroll end)
         */
        function actionLoadMore() {
            if (!$scope.busy && $scope.paging.hasMorePage) {
                $scope.busy = true;
                $scope.paging.page++;

                $q.when()
                    .then(ProgressBar.start)
                    .then(function () { return false; })
                    .then(actionGetRapports)
                    .then(function () { $scope.busy = false; ProgressBar.complete(); });
            }
        }

        function checkErrorOnRapportToLock() {
            var listRapportIdAVerrouiller = $scope.listRapport.filter(r => r.IsSelected).map(r => r.RapportId);
            ProgressBar.start();
            RapportService.GetListRapportIdWithError(listRapportIdAVerrouiller)
                .then(function (value) {
                    if (!showNotLockedReportsMessage(value)) {
                        $q.when()
                            .then(ProgressBar.start)
                            .then(actionVerrouiller)
                            .then(function () { return true; })
                            .then(actionGetRapports)
                            .finally(ProgressBar.complete);
                    }
                })
                .catch(function (reason) {
                    Notify.error($scope.resources.Global_Notification_Error);
                    console.log(reason);
                })
                .finally(ProgressBar.complete);
        }

        function showNotLockedReportsMessage(value) {
            if (value.PartialLockedReportIds && value.PartialLockedReportIds.length > 0) {
                fillLockMessage(value.PartialLockedReportIds, $scope.resources.Partial_Locked_Report)
                return true;
            } else if (value.NotLockableRaportsIds && value.NotLockableRaportsIds.length > 0) {
                fillLockMessage(value.NotLockableRaportsIds, $scope.resources.Report_Not_Locked)
                return true;
            }

            return false;
        }

        function fillLockMessage(rapportIds, msg) {
            if (rapportIds && rapportIds.length > 0) {
                var listRapportIdStr = "";
                for (var i = 0; i < rapportIds.length; i++) {
                    listRapportIdStr += " - " + rapportIds[i] + "\r";
                }
                fredDialog.information(msg + "\r" + listRapportIdStr);
            }
        }

        /*
         * @function addFilter2Favoris()
         * @description Ajout des filtres en cours aux favoris
         */
        function addFilter2Favoris() {
            var filter = $scope.filters;
            var url = $window.location.pathname;
            if ($scope.favoriId !== 0) {
                url = $window.location.pathname.slice(0, $window.location.pathname.lastIndexOf("/"));
            }
            favorisService.initializeAndOpenModal("Rapport", url, filter);
        }

        /*
         * @function getFilterOrFavoris
         * @description Récupère le favori. Sinon, récupère les filtres par défauts
         */
        function getFilterOrFavoris(favoriId) {
            $scope.favoriId = parseInt(favoriId);

            if ($scope.favoriId && $scope.favoriId !== 0) {
                favorisService.getFilterByFavoriIdOrDefault({ favoriId: $scope.favoriId, defaultFilter: $scope.filters })
                    .then(function (response) {
                        $scope.filters = response;
                        if ($scope.filters.OrganisationId !== null) {
                            OrganisationService.getLightOrganisationModelById($scope.filters.OrganisationId)
                                .then((orga) => {
                                    $scope.filters["Organisation"] = orga.data;
                                })
                                .catch(function () { Notify.error($scope.resources.Global_Notification_Error); });
                        }
                        $scope.firstLoad();
                    })
                    .catch(function () { Notify.error($scope.resources.Global_Notification_Error); });
            }
            else {
                if (sessionStorage.getItem('rapportListFilter') !== null) {
                    $scope.filters = JSON.parse(sessionStorage.getItem('rapportListFilter'));
                    $scope.firstLoad();

                }
                else {
                    RapportService.GetFilter().then(function (value) {
                        $scope.filters = value;
                        $scope.firstLoad();
                    })
                        .catch(function () { Notify.error($scope.resources.Global_Notification_Error); });
                }
            }
        }

        /*
         * @function saveFilters()
         * @description Sauvegarde temporairement les filtres de l'utilisateur
         */
        function saveFilters() {
            oldFilters = Object.assign({}, $scope.filters);
        }

        function initNomPrenom(utilisateur) {
            if (utilisateur && utilisateur.Personnel)
                utilisateur.NomPrenom = utilisateur.Personnel.Nom + " " + utilisateur.Personnel.Prenom;
        }

        /**
         * Vérification si l'utilisateur connecté a le droit de voir menu d'edition
         * @returns {any} promise
         */
        function IsUserHasMenuEditionPermission() {
            return RapportService.IsUserHasMenuEditionPermission()
                .then(function (value) {
                    $scope.isUserHasMenuEditionPermission = value;
                })
                .catch(Notify.defaultError);
        }

        /**
         * Initialisation des permissions utilisateur
         */
        function InitUserPermissions() {
            var permissionKeys = PERMISSION_KEYS;
            var rights = authorizationService.getRights(permissionKeys.AffichageMenuRapportPrimesIndex);
            if (rights) {
                $scope.permissionPrimesMensuellesVisible = rights.isVisible;
            }
        }

        function LancerServiceTibco() {
            if ($scope.ExportAnaModel.EtablissementComptaList.length === 0) {
                Notify.message(resources.Rapport_ModalImport_EtablissementPaie_Vide);
                return;
            }

            var etablissementsComptablesIds = [];
            var etablissementsComptablesCodes = [];
            angular.forEach($scope.ExportAnaModel.EtablissementComptaList, function (etab) {
                etablissementsComptablesIds.push(etab.EtablissementComptableId);
                etablissementsComptablesCodes.push(etab.Code);
            });

            UserService.getCurrentUser().then(function (user) {
                var exportAnalytiqueFilterModel = {
                    DateDebut: $scope.ExportAnaModel.DateDebut,
                    DateFin: undefined,
                    Hebdo: $scope.ExportAnaModel.Hebdo,
                    SocieteCode: $scope.ExportAnaModel.Societe.Code,
                    Simulation: $scope.ExportAnaModel.Simulation,
                    EtablissementsComptablesIds: etablissementsComptablesIds,
                    EtablissementsComptablesCodes: etablissementsComptablesCodes,
                    UserId: user.Personnel.PersonnelId,
                    AllEtablissements: $scope.ExportAnaModel.AllEtab
                };

                if ($scope.ModeControleSaisie) {
                    $scope.errorsTibco = [];
                    $scope.isErrorsDispo = false;
                    RapportService.ControlerSaisiesForTibco(exportAnalytiqueFilterModel).then(function (response) {
                        if (response) {
                            $scope.errorsTibco = response;
                            $scope.isErrorsDispo = $scope.errorsTibco.length > 0;
                            if (!$scope.isErrorsDispo) {
                                Notify.message(resources.Rapport_Exportation_NoErrors);
                            }
                        }
                    });
                }
                else {
                    RapportService.ExporterAnalytiqueForTibco(exportAnalytiqueFilterModel).then(function (response) {
                        if (response) {
                            Notify.error(response);
                        }
                        else {
                            Notify.message(resources.Rapport_Exportation_Success);
                        }
                    });
                }
            });
        }

        $scope.handleChangePersonnelStatutList = function (type) {
            var value, boolean;

            switch (type) {
                case "StatutOuvrier":
                    value = "1";
                    $scope.statutOuvrier = !$scope.statutOuvrier;
                    boolean = $scope.statutOuvrier;
                    break;
                case "StatutEtam":
                    value = "2";
                    $scope.statutEtam = !$scope.statutEtam;
                    boolean = $scope.statutEtam;
                    break;
                case "StatutCadre":
                    value = "3";
                    $scope.statutCadre = !$scope.statutCadre;
                    boolean = $scope.statutCadre;
                    break;
            }

            if (boolean) {
                $scope.statutPersonnelList.push(value);
            }
            else {
                $scope.statutPersonnelList.splice($scope.statutPersonnelList.indexOf(value), 1);
            }
        };

        $scope.onHebdoChanged = () => {
            $scope.ExportAnaModel.Simulation = $scope.ExportAnaModel.Hebdo;
        };

        $scope.displayWeek = function (val) {
            var momentDate = moment(val.date);
            var mondayDayNumber = momentDate.startOf('isoWeek').format('DD');
            var sundayDayNumber = momentDate.isoWeekday(7).format('DD');
            var mondayMonth = momentDate.startOf('isoWeek').format('MM');
            var sundayMonth = momentDate.isoWeekday(7).format('MM');
            var mondayYear = momentDate.startOf('isoWeek').format('YYYY');
            var sundayYear = momentDate.isoWeekday(7).format('YYYY');
            var mondayMonthLabel = mondayMonth === sundayMonth ? '' : resources['Global_Month_' + mondayMonth];
            var sundayMonthLabel = resources['Global_Month_' + sundayMonth];
            var mondayYearLabel = mondayYear === sundayYear ? '' : mondayYear;
            var selectedWeek = resources.Global_From + " " + mondayDayNumber + " " + mondayMonthLabel + " " + mondayYearLabel + " " +
                resources.Global_To + " " + sundayDayNumber + " " + sundayMonthLabel + " " + sundayYear;
            $scope.ExportAnaModel.SelectedWeek = selectedWeek[0].toUpperCase() + selectedWeek.slice(1).toLowerCase();
        };

        $scope.handleChangeWeek = function (isAddWeek) {
            if (isAddWeek) {
                $scope.ExportAnaModel.DateDebut = moment($scope.ExportAnaModel.DateDebut).add(1, 'week').toDate();
            } else {
                $scope.ExportAnaModel.DateDebut = moment($scope.ExportAnaModel.DateDebut).subtract(1, 'week').toDate();
            }
            $scope.displayWeek($scope.ExportAnaModel.DateDebut);
        };

        $scope.handleGetEtablissementComptable = function (organisationId) {
            return String.format('/api/EtablissementComptable/GeCurrentUserEtabComptableWithOrganisationPartentId/?organisationPereId={0}&', organisationId);
        };
    }
})(angular);