(function (angular) {
    'use strict';

    angular.module('Fred').controller('MoyenController', MoyenController);

    MoyenController.$inject = ['$scope', '$q', 'MoyenService', 'Notify', 'ProgressBar', 'confirmDialog'];

    function MoyenController($scope, $q, MoyenService, Notify, ProgressBar, confirmDialog) {
        // assignation de la valeur du scope au controller pour les templates non mis à jour
        var $ctrl = this;

        // méthodes exposées
        angular.extend($ctrl, {
            handleLookupSelection: handleLookupSelection,
            handleGetLookupUrl: handleGetLookupUrl,
            handleRemoveNumParcFilter: handleRemoveNumParcFilter,
            handleRemoveImmatriculationFilter: handleRemoveImmatriculationFilter,
            handleRemoveAffectationTypeFilter: handleRemoveAffectationTypeFilter,
            handleRemoveTypeMoyenFilter: handleRemoveTypeMoyenFilter,
            handleRemoveSousTypeMoyenFilter: handleRemoveSousTypeMoyenFilter,
            handleRemoveModelMoyenFilter: handleRemoveModelMoyenFilter,
            handleRemoveSocieteFilter: handleRemoveSocieteFilter,
            handleRemoveEtablissementFilter: handleRemoveEtablissementFilter,
            handleRemoveSiteFilter: handleRemoveSiteFilter,
            handleApplyFilter: handleApplyFilter,
            handleResetFilter: handleResetFilter,
            handleInitAffectation: handleInitAffectation,
            handleInitAffectationFailure: handleInitAffectationFailure,
            handleValidateButton: handleValidateButton,
            toggleAffectationMoyenSelection: toggleAffectationMoyenSelection,
            handleInitRestitution: handleInitRestitution,
            handleInitRestitutionFailure: handleInitRestitutionFailure,
            handleRemovePersonnelFilter: handleRemovePersonnelFilter,
            handleRemoveCiFilter: handleRemoveCiFilter,
            handleCancel: handleCancel,
            handleToggleSelectAll: handleToggleSelectAll,
            handleInitMaintenance: handleInitMaintenance,
            handleInitMaintenanceFailure: handleInitMaintenanceFailure,
            handleInitLocation: handleInitLocation,
            handleChangeView: handleChangeView,
            handleInitRapportExtraction: handleInitRapportExtraction
        });

        init();

        /**
         * Initialisation du controller.     
         */
        function init() {
            angular.extend($ctrl, {
                // Instanciation Objet Ressources
                resources: resources,
                hasMorePage: true,
                paging: { pageSize: 25, currentPage: 5 },
                affectationMoyenList: [],
                selectedPersonnel: null,
                selectedCi: null,
                selectedMoyen: null,
                selectedImmatriculation: null,
                selectedAffectationMoyenType: null,
                selectedSociete: null,
                selectedEtablissement: null,
                selectedTypeMoyen: null,
                selectedSousTypeMoyen: null,
                selectedModelMoyen: null,
                selectedSite: null,
                filters: {
                    TypeMoyen: '',
                    SousTypeMoyen: '',
                    ModelMoyen: '',
                    Societe: '',
                    EtablissementComptableId: null,
                    NumParc: '',
                    AffectationMoyenTypeId: '',
                    IsDateFinPredictedOutdated: false,
                    IsToBringBack: false,
                    IsActive: false,
                    DateFrom: '',
                    DateTo: '',
                    isAffectationView: true
                },
                responsableOrManagerName: '',
                updatedAffectationMoyenList: [],
                showAffectationButton: false,
                showRestitutionButton: false,
                showMaintenanceButton: false,
                LOC: 'LOC',
                DISP: 'DISP',
                MAINT: 'MAINT',
                SelectedAll: false,
                isAffectationView: true,
                isBusy: false
            });


            FredToolBox.bindScrollEnd('#containerTableauAffectations', actionLoadMore);


            // -- Selection d'une affectation
            $scope.$on('event.operation.moyen', function (event, obj) {
                angular.forEach(actionGetSelectedMoyenList(true), function (el) {
                    el.Selected = false;
                });
                actionApplyAffectationMoyen(obj.affectationMoyenList);
                $ctrl.showAffectationButton = false;
                $ctrl.showRestitutionButton = false;
                $ctrl.showMaintenanceButton = false;
                $ctrl.SelectedAll = false;
            });

            // -- Refresh des données 
            $scope.$on('event.operation.refresh', function (event, obj) {
                handleResetFilter();
            });

            // -- Refresh pour lançer la mise à jour des pointages
            $scope.$on('event.update.pointage.materiel', function (event, obj) {
                actionUpdatePointageMoyen(obj);
            });

            // - Refresh pour envoyer l'export des moyens
            $scope.$on('event.envoi.pointage.moyen', function (event, obj) {
                actionExportPointageMoyen(obj);
            });

            // Relance la recherche avec les filtres en cas de suppression d'une location
            $scope.$on('event.applyfilter.moyen', function () {
                handleApplyFilter();
            });

            // -- Refresh des données 
            $scope.$on('event.refresh.moyen', function () {
                actionSearchWithFilters($ctrl.filters, true);
            });

            // Récuperation des familles des affectations des moyens
            actionGetAffectationMoyenFamilleByTypeMoyen();
        }

        /*
         * @function actionSearchWithFilters
         * @description Action de recherche d'une affectation à partir de filtre
         */
        function actionSearchWithFilters(filters, firstLoad) {
            if (firstLoad) {
                $ctrl.affectationMoyenList = [];
                $ctrl.paging.currentPage = 1;
            }
            ProgressBar.start();
            MoyenService.SearchWithFilters({ page: $ctrl.paging.currentPage, pageSize: $ctrl.paging.pageSize }, filters)
                .$promise
                .then(function (value) {
                    angular.forEach(value, function (val) {
                        $ctrl.affectationMoyenList.push(val);
                    });
                    $ctrl.hasMorePage = value.length === $ctrl.paging.pageSize;
                    $ctrl.SelectedAll = false;
                    $ctrl.updatedAffectationMoyenList = [];
                })
                .catch(function (error) {
                    Notify.error(error);
                })
                .finally(ProgressBar.complete);
        }

        /* 
         * @function actionLoadMore()
         * @description Action de chargement de données supplémentaires (scroll end)
         */
        function actionLoadMore() {
            if ($ctrl.hasMorePage) {
                $ctrl.paging.currentPage++;
                actionSearchWithFilters($ctrl.filters, false);
            }
        }

        //Fonction de chargement des données de l'item sélectionné dans la picklist
        function handleLookupSelection(type, item) {
            if (item) {
                switch (type) {
                    case "CI":
                        $ctrl.filters.CiId = item.CiId;
                        $ctrl.filters.PersonnelId = null;
                        $ctrl.selectedPersonnel = null;
                        break;

                    case "Personnel":
                        $ctrl.filters.PersonnelId = item.PersonnelId;
                        $ctrl.filters.CiId = null;
                        $ctrl.selectedCi = null;
                        break;

                    case "Site":
                        $ctrl.filters.SiteActuelId = item.SiteId;
                        $ctrl.selectedSite = item;
                        break;

                    case "TypeMoyen":
                        $ctrl.filters.TypeMoyen = item.Code;
                        $ctrl.selectedTypeMoyen = item;

                        handleRemoveSousTypeMoyenFilter();
                        break;

                    case "SousTypeMoyen":
                        $ctrl.filters.SousTypeMoyen = item.Code;
                        $ctrl.selectedSousTypeMoyen = item;

                        handleRemoveModelMoyenFilter();
                        break;

                    case "ModelMoyen":
                        $ctrl.filters.ModelMoyen = item.Code;
                        $ctrl.selectedModelMoyen = item;

                        handleRemoveSocieteFilter();
                        break;

                    case "Societe":
                        $ctrl.filters.Societe = item.Code;
                        $ctrl.selectedSociete = item;

                        handleRemoveEtablissementFilter();
                        break;

                    case "Etablissement":
                        $ctrl.filters.EtablissementComptableId = item.EtablissementComptableId;
                        $ctrl.selectedEtablissement = item;

                        handleRemoveMoyenFilter();
                        break;

                    case "NumParc":
                        $ctrl.filters.NumParc = item.Code;
                        $ctrl.selectedMoyen = item;

                        handleRemoveImmatriculationFilter();
                        break;

                    case "NumImmatriculation":
                        $ctrl.filters.NumImmatriculation = item.Immatriculation;
                        $ctrl.selectedImmatriculation = item;
                        break;

                    case "Statut":
                        $ctrl.filters.AffectationMoyenTypeId = item.AffectationMoyenTypeId;
                        $ctrl.selectedAffectationMoyenType = item;
                        break;

                }
                actionUpdateResponsableOrManagerName(type, item);
            }
        }

        function handleGetLookupUrl(val) {
            var baseControllerUrl = "";

            switch (val) {
                case "TypeMoyen":
                    baseControllerUrl = '/api/Moyen/Type/SearchLight';
                    break;

                case "SousTypeMoyen":
                    baseControllerUrl = String.format('/api/Moyen/SousType/SearchLight/?typeMoyen={0}&', $ctrl.filters.TypeMoyen);
                    break;

                case "ModelMoyen":
                    baseControllerUrl = String.format('/api/Moyen/Model/SearchLight/?typeMoyen={0}&sousTypeMoyen={1}&',
                        $ctrl.filters.TypeMoyen,
                        $ctrl.filters.SousTypeMoyen);
                    break;
                case "Societe":
                    baseControllerUrl = String.format('/api/Moyen/Societe/SearchLight/?typeMoyen={0}&sousTypeMoyen={1}&modelMoyen={2}&',
                        $ctrl.filters.TypeMoyen,
                        $ctrl.filters.SousTypeMoyen,
                        $ctrl.filters.ModelMoyen);
                    break;
                case "Etablissement":
                    baseControllerUrl = String.format('/api/Moyen/Etablissement/SearchLight/?typeMoyen={0}&sousTypeMoyen={1}&modelMoyen={2}&societe={3}&',
                        $ctrl.filters.TypeMoyen,
                        $ctrl.filters.SousTypeMoyen,
                        $ctrl.filters.ModelMoyen,
                        $ctrl.filters.Societe);
                    break;
                case "NumParc":
                    baseControllerUrl = MoyenService.getLookupBaseControllerUrl("NumParc", $ctrl.filters);
                    break;

                case "NumImmatriculation":
                    baseControllerUrl = MoyenService.getLookupBaseControllerUrl("NumImmatriculation", $ctrl.filters);
                    break;

                case "Statut":
                    baseControllerUrl = '/api/AffectationMoyen/Type/SearchLight/';
                    break;

                case "Site":
                    baseControllerUrl = '/api/Site/SearchLight/';
                    break;
            }

            return baseControllerUrl;
        }

        /*
         * @function handleApplyFilter()
         * @description Applique le filtre séléctionné . 
         */
        function handleApplyFilter() {

            // Dans le cas de la vue rapport
            if (!$ctrl.isAffectationView) {
                $scope.$broadcast('event.update.rapport.data', { filters: $ctrl.filters });
                return;
            }

            if ($ctrl.updatedAffectationMoyenList && $ctrl.updatedAffectationMoyenList.length > 0) {
                confirmDialog.confirm($ctrl.resources, 'Attention vos données d’affectations n’ont pas été validées, si vous quittez cet écran, vos mises à jour seront perdues !')
                    .then(function () {
                        actionSearchWithFilters($ctrl.filters, true);
                    });
            } else {
                actionSearchWithFilters($ctrl.filters, true);
            }
        }

        /*
         * @function hanlde init affectation
         * @description Initialise le component des affectations
         */
        function handleInitAffectation() {
            $scope.$broadcast('event.init.affectation.mode', { selectedMoyenList: actionGetSelectedMoyenList() });
        }

        /*
         * @function hanlde init restitution
         * @description Initialise le component des restitutions
         */
        function handleInitRestitution() {
            if ($ctrl.affectationMoyenFamilleByTypeMoyenList) {
                let selectedMoyenList = actionGetSelectedMoyenList();
                let firstTypeByLocation = (selectedMoyenList && selectedMoyenList.length > 0) ? actionLocationTypeOfMoyenAffectation(selectedMoyenList[0]) : null;
                let filterFamille = firstTypeByLocation ? $ctrl.LOC : $ctrl.DISP;
                let moyenAffectationFamille = $ctrl.affectationMoyenFamilleByTypeMoyenList.filter(function (f) {
                    return f.AffectationMoyenFamilleCode === filterFamille;
                });

                $scope.$broadcast('event.init.restitution.mode',
                    {
                        affectationMoyenFamilleByTypeMoyenList: moyenAffectationFamille,
                        selectedMoyenList: selectedMoyenList
                    });
            }
            else {
                Notify.warning('Erreur de lecture des familles des moyens .');
            }
        }

        /*
         * @function handleInitAffectationFailure
         * @description Hanlde init affectation failure
         */
        function handleInitAffectationFailure() {
            Notify.warning('Veuillez séléctionner au moins un moyen avec un status actif .');
        }

        /*
         * @function handleInitRestitutionFailure
         * @description Hanlde init restitution failure
         */
        function handleInitRestitutionFailure() {
            let selectedMoyenList = actionGetSelectedMoyenList();
            if (!selectedMoyenList || selectedMoyenList.length === 0) {
                Notify.warning('Veuillez séléctionner au moins un moyen avec un status actif.');
                return;
            }

            Notify.warning($ctrl.restitutionSelectionErrorMessage);
        }

        /*
         * @function hanlde Init Rapport Extraction
         * @description Rapport Extraction
         */
        function handleInitRapportExtraction() {
            $scope.$broadcast('event.init.rapport.extraction', { filters: $ctrl.filters });
        }


        /*
         * @function hanlde validation button
         * @description validation de la page de gestion des moyens
         */
        function handleValidateButton() {
            if (!$ctrl.updatedAffectationMoyenList || $ctrl.updatedAffectationMoyenList.length === 0) {
                Notify.warning('Merci d’éffectuer des opérations (affectation, restitution ou maintenance) pour valider');
                return;
            }

            actionPrepareAffectationDataForValidation();

            $q.when()
                .then(ProgressBar.start)
                .then(function () {
                    var model = {
                        AffectationMoyenModelList: $ctrl.updatedAffectationMoyenList
                    };

                    MoyenService.ValidateAffectationMoyen(model)
                        .$promise
                        .then(function () {
                            $ctrl.updatedAffectationMoyenList = [];
                            Notify.message('Validation éffectuée avec succès');
                            $ctrl.SelectedAll = false;
                            $ctrl.handleApplyFilter();
                        })
                        .catch(function () {
                            Notify.error(resources.Global_Notification_Error);
                        })
                        .finally(function () {
                            ProgressBar.complete();
                        });
                });
        }

        /**
         * Change la vue principale : Affectation ou rapport
         * @param {boolean} isAffectationView Si True on affiche la vue des affectations. Si false ça affiche la vue des rapports
         */
        function handleChangeView(isAffectationView) {
            $ctrl.isAffectationView = isAffectationView;
        }

        /*
         * @function Apply affectation moyen
         * @description Applique les éléments validés sur la ligne d'affectation
         */
        function actionApplyAffectationMoyen(affectationMoyenList) {
            if (affectationMoyenList) {
                var newItems = affectationMoyenList.filter(function (r) { return r.IsToAdd; });
                angular.forEach(newItems, function (item) {
                    $ctrl.affectationMoyenList.push(item);
                    actionUpdateAffectationMoyenListToUpdate(item);
                });

                var updatedItems = affectationMoyenList.filter(function (r) { return !r.IsToAdd; });
                angular.forEach(updatedItems, function (item) {
                    actionUpdateAffectationMoyenListToUpdate(item);
                });

                $ctrl.affectationMoyenList = MoyenService.sortByNumParcAndDateDebut($ctrl.affectationMoyenList);
            }
        }

        /*
         * @function action update affectation moyen list to update
         * @description modification de la liste des affectations des moyens
         */
        function actionUpdateAffectationMoyenListToUpdate(el) {
            if (!el) {
                return;
            }

            var isElementInArray = $ctrl.updatedAffectationMoyenList.some(function (e) { return e.AffectationMoyenId === el.AffectationMoyenId; });
            if (isElementInArray) {
                $ctrl.updatedAffectationMoyenList = $ctrl.updatedAffectationMoyenList.filter(function (v) {
                    return v.AffectationMoyenId !== el.AffectationMoyenId;
                });
            }

            $ctrl.updatedAffectationMoyenList.push(el);
        }

        /*
         * @function toggleAffectationMoyenSelection
         * @description toggle affectation moyen selection
         */
        function toggleAffectationMoyenSelection() {
            actionIsAllElementsChecked();
            let selectedMoyenList = actionGetSelectedMoyenList();
            $ctrl.showAffectationButton = selectedMoyenList && selectedMoyenList.length > 0;

            // La restitution et la maintenance doivent se faire sur des moyens homogénes de type (Par chapitre et par type de location)
            let firstElementChapitreId = selectedMoyenList && selectedMoyenList.length > 0
                ? actionGetChapitreOfMoyenAffectation(selectedMoyenList[0])
                : null;

            let isShowRestitutionAndMaintenance = firstElementChapitreId && selectedMoyenList && selectedMoyenList.every(function (e) {
                return actionGetChapitreOfMoyenAffectation(e) === firstElementChapitreId;
            });

            if (!isShowRestitutionAndMaintenance) {
                $ctrl.maintenanceSelectionErrorMessage = 'La maintenance n’est pas applicable, car les moyens sélectionnés sont de types différents (famille et/ou location) ! ';
                $ctrl.restitutionSelectionErrorMessage = 'La restitution n’est pas applicable, car les moyens sélectionnés sont de types différents (famille et/ou location) ! ';
            }

            $ctrl.showRestitutionButton = $ctrl.showMaintenanceButton = isShowRestitutionAndMaintenance;
            let isFirstElementTypeLocation = selectedMoyenList && selectedMoyenList.length > 0 && selectedMoyenList[0].Moyen
                ? selectedMoyenList[0].Moyen.IsLocation
                : false;

            if ($ctrl.showMaintenanceButton) {
                $ctrl.showMaintenanceButton = !isFirstElementTypeLocation;
            }

            if (isFirstElementTypeLocation) {
                $ctrl.maintenanceSelectionErrorMessage = 'La maintenance n’est pas applicable sur une location';
            }

            if ($ctrl.showRestitutionButton) {
                $ctrl.showRestitutionButton = selectedMoyenList && selectedMoyenList.every(function (e) {
                    return actionLocationTypeOfMoyenAffectation(e) === isFirstElementTypeLocation;
                });
                if (!$ctrl.showRestitutionButton) {
                    $ctrl.restitutionSelectionErrorMessage = 'Attention, dans la sélection, il existe des moyens en location et des moyens standard, veuillez refaire votre sélection ! ';
                }
            }
        }

        /*
         * @function actionGetSelectedMoyenList
         * @description Get selected moyen list
         */
        function actionGetSelectedMoyenList(all) {
            let selectedAffectationMoyenList = $ctrl.affectationMoyenList.filter(function (o) {
                return o.Selected && (all || o.IsActive);
            });

            angular.forEach(selectedAffectationMoyenList, function (val) {
                val.IsToAdd = false;
            });

            return selectedAffectationMoyenList;
        }

        /*
         * @function action get affectation famille by type moyen
         * @description Get selected moyen list
         */
        function actionGetAffectationMoyenFamilleByTypeMoyen() {
            $ctrl.affectationMoyenFamilleByTypeMoyenList = [];
            MoyenService.GetAffectationMoyenFamilleByTypeModel()
                .$promise
                .then(function (value) {
                    angular.forEach(value, function (val) {
                        $ctrl.affectationMoyenFamilleByTypeMoyenList.push(val);
                    });
                })
                .catch(function (error) {
                    console.log(error);
                });
        }

        /*
         * @function action get chapitre of moyen affectation
         * @description Get chapitre of moyen affectation
         */
        function actionGetChapitreOfMoyenAffectation(moyenAffectation) {
            if (moyenAffectation
                && moyenAffectation.Moyen
                && moyenAffectation.Moyen.Ressource
                && moyenAffectation.Moyen.Ressource.SousChapitre
                && moyenAffectation.Moyen.Ressource.SousChapitre.Chapitre) {

                return moyenAffectation.Moyen.Ressource.SousChapitre.Chapitre.ChapitreId;
            }

            return null;
        }

        /*
         * @function action get location type of moyen affectation
         * @description Action get location type of moyen affectation
         */
        function actionLocationTypeOfMoyenAffectation(moyenAffectation) {
            if (moyenAffectation
                && moyenAffectation.Moyen) {

                return moyenAffectation.Moyen.IsLocation;
            }

            return null;
        }

        function actionUpdateResponsableOrManagerName(type, item) {
            switch (type) {
                case "CI":
                    if (item.ResponsableAdministratif) {
                        $ctrl.responsableOrManagerName = item.ResponsableAdministratif.LibelleRef;
                    }
                    else {
                        $ctrl.responsableOrManagerName = '';
                    }
                    break;
                case "Personnel":
                    if (item.Manager) {
                        $ctrl.responsableOrManagerName = item.Manager.LibelleRef;
                    }
                    else {
                        $ctrl.responsableOrManagerName = '';
                    }
                    break;
                default:
                    $ctrl.responsableOrManagerName = '';
                    break;
            }
        }

        function handleRemoveNumParcFilter() {
            $ctrl.filters.NumParc = '';
            $ctrl.selectedMoyen = null;
        }

        function handleRemoveAffectationTypeFilter() {
            $ctrl.filters.AffectationMoyenTypeId = '';
            $ctrl.selectedAffectationMoyenType = null;
        }
        function handleRemovePersonnelFilter() {
            $ctrl.filters.PersonnelId = '';
            $ctrl.selectedPersonnel = null;
            $ctrl.responsableOrManagerName = null;
        }

        function handleRemoveCiFilter() {
            $ctrl.filters.CiId = '';
            $ctrl.selectedCi = null;
            $ctrl.responsableOrManagerName = null;
        }

        function handleRemoveTypeMoyenFilter() {
            $ctrl.filters.TypeMoyen = '';
            $ctrl.selectedTypeMoyen = null;

            $ctrl.filters.SousTypeMoyen = '';
            $ctrl.selectedSousTypeMoyen = null;

            $ctrl.filters.ModelMoyen = '';
            $ctrl.selectedModelMoyen = null;

            $ctrl.filters.Societe = '';
            $ctrl.selectedSociete = null;

            $ctrl.filters.EtablissementComptableId = null;
            $ctrl.selectedEtablissement = null;

            $ctrl.filters.NumParc = '';
            $ctrl.selectedMoyen = null;

            $ctrl.filters.NumImmatriculation = '';
            $ctrl.selectedImmatriculation = null;
        }

        function handleRemoveSousTypeMoyenFilter() {
            $ctrl.filters.SousTypeMoyen = '';
            $ctrl.selectedSousTypeMoyen = null;

            $ctrl.filters.ModelMoyen = '';
            $ctrl.selectedModelMoyen = null;

            $ctrl.filters.Societe = '';
            $ctrl.selectedSociete = null;

            $ctrl.filters.EtablissementComptableId = null;
            $ctrl.selectedEtablissement = null;

            $ctrl.filters.NumParc = '';
            $ctrl.selectedMoyen = null;

            $ctrl.filters.NumImmatriculation = '';
            $ctrl.selectedImmatriculation = null;
        }

        function handleRemoveModelMoyenFilter() {
            $ctrl.filters.ModelMoyen = '';
            $ctrl.selectedModelMoyen = null;

            $ctrl.filters.Societe = '';
            $ctrl.selectedSociete = null;

            $ctrl.filters.EtablissementComptableId = null;
            $ctrl.selectedEtablissement = null;

            $ctrl.filters.NumParc = '';
            $ctrl.selectedMoyen = null;

            $ctrl.filters.NumImmatriculation = '';
            $ctrl.selectedImmatriculation = null;
        }

        function handleRemoveSocieteFilter() {
            $ctrl.filters.Societe = '';
            $ctrl.selectedSociete = null;

            $ctrl.filters.EtablissementComptableId = null;
            $ctrl.selectedEtablissement = null;

            $ctrl.filters.NumParc = '';
            $ctrl.selectedMoyen = null;

            $ctrl.filters.NumImmatriculation = '';
            $ctrl.selectedImmatriculation = null;
        }

        function handleRemoveEtablissementFilter() {
            $ctrl.filters.EtablissementComptableId = null;
            $ctrl.selectedEtablissement = null;

            $ctrl.filters.NumParc = '';
            $ctrl.selectedMoyen = null;

            $ctrl.filters.NumImmatriculation = '';
            $ctrl.selectedImmatriculation = null;
        }

        function handleRemoveMoyenFilter() {
            $ctrl.filters.NumParc = '';
            $ctrl.selectedMoyen = null;

            $ctrl.filters.NumImmatriculation = '';
            $ctrl.selectedImmatriculation = null;
        }

        function handleRemoveImmatriculationFilter() {
            $ctrl.filters.NumImmatriculation = '';
            $ctrl.selectedImmatriculation = null;
        }

        function handleRemoveSiteFilter() {
            $ctrl.filters.SiteActuelId = '';
            $ctrl.selectedSite = null;
        }

        function handleResetFilter() {
            if ($ctrl.selectedMoyen != null
                || $ctrl.selectedImmatriculation != null
                || $ctrl.selectedAffectationMoyenType != null
                || $ctrl.selectedTypeMoyen != null
                || $ctrl.selectedSousTypeMoyen != null
                || $ctrl.selectedModelMoyen != null
                || $ctrl.selectedSociete != null
                || $ctrl.selectedEtablissement != null
                || $ctrl.selectedSite != null
                || $ctrl.filters.IsDateFinPredictedOutdated == true
                || $ctrl.filters.IsToBringBack == true
                || $ctrl.filters.IsActive == false
                || $ctrl.filters.DateFrom
                || $ctrl.filters.DateTo
            ) {
                // Numéro parc
                $ctrl.filters.NumParc = '';
                $ctrl.selectedMoyen = null;

                // Numéro d'immatriculation 
                $ctrl.filters.NumImmatriculation = '';
                $ctrl.selectedImmatriculation = null;

                // Type affectation moyen
                $ctrl.filters.AffectationMoyenTypeId = '';
                $ctrl.selectedAffectationMoyenType = null;

                // Type moyen
                $ctrl.filters.TypeMoyen = '';
                $ctrl.selectedTypeMoyen = null;

                // Sous type moyen
                $ctrl.filters.SousTypeMoyen = '';
                $ctrl.selectedSousTypeMoyen = null;

                // Model moyen
                $ctrl.filters.ModelMoyen = '';
                $ctrl.selectedModelMoyen = null;

                // Société
                $ctrl.filters.Societe = '';
                $ctrl.selectedSociete = null;

                // Etablissement
                $ctrl.filters.EtablissementComptableId = null;
                $ctrl.selectedEtablissement = null;

                // Site
                $ctrl.filters.SiteActuelId = '';
                $ctrl.selectedSite = null;

                // Date fin prévisionnelle dépassée
                $ctrl.filters.IsDateFinPredictedOutdated = false;

                // Moyen à repatrier
                $ctrl.filters.IsToBringBack = false;

                // Is active
                $ctrl.filters.IsActive = false;

                // Dates
                $ctrl.filters.DateFrom = '';
                $ctrl.filters.DateTo = '';

            }
            handleApplyFilter();
        }

        // Handle cancel action
        function handleCancel() {
            confirmDialog.confirm($ctrl.resources, $ctrl.resources.Global_Modal_ConfirmationAnnulation)
                .then(function () {
                    actionSearchWithFilters($ctrl.filters, true);
                });
        }

        // Handle toggle select all
        function handleToggleSelectAll(check) {
            if (!$ctrl.affectationMoyenList || $ctrl.affectationMoyenList.length === 0) {
                $ctrl.SelectedAll = false;
                return;
            }

            angular.forEach($ctrl.affectationMoyenList, function (el) {
                el.Selected = check;
            });

            $ctrl.showAffectationButton = check;
            $ctrl.showRestitutionButton = check;
            $ctrl.showMaintenanceButton = check;
        }

        // Handle toggle select all
        function actionIsAllElementsChecked() {
            if (!$ctrl.affectationMoyenList || $ctrl.affectationMoyenList.length === 0) {
                $ctrl.SelectedAll = false;
            }

            $ctrl.SelectedAll = $ctrl.affectationMoyenList.every(function (o) {
                return o.Selected;
            });
        }

        // Handle init maintenance
        function handleInitMaintenance() {
            if ($ctrl.affectationMoyenFamilleByTypeMoyenList) {
                let selectedMoyenList = actionGetSelectedMoyenList();
                let moyenAffectationFamille = $ctrl.affectationMoyenFamilleByTypeMoyenList.filter(function (f) {
                    return f.AffectationMoyenFamilleCode === $ctrl.MAINT;
                });

                $scope.$broadcast('event.init.restitution.mode',
                    {
                        affectationMoyenFamilleByTypeMoyenList: moyenAffectationFamille,
                        selectedMoyenList: selectedMoyenList
                    });
            }
            else {
                Notify.warning('Erreur de lecture des familles des moyens .');
            }
        }

        // Handle init maintenance failure
        function handleInitMaintenanceFailure() {
            let selectedMoyenList = actionGetSelectedMoyenList();
            if (!selectedMoyenList || selectedMoyenList.length === 0) {
                Notify.warning('Veuillez séléctionner des moyens qui ne sont pas de type location pour éffectuer la maintenance.');
                return;
            }
            Notify.warning($ctrl.maintenanceSelectionErrorMessage);
        }

        // Preparation des données d'affectation pour la validation
        function actionPrepareAffectationDataForValidation() {
            if ($ctrl.updatedAffectationMoyenList && $ctrl.updatedAffectationMoyenList.length > 0) {
                angular.forEach($ctrl.updatedAffectationMoyenList, function (item) {
                    if (MoyenService.isRestitutionType(item.AffectationMoyenTypeId)) {
                        item.CiId = null;
                        item.PersonnelId = null;
                        item.ConducteurId = null;
                    }
                    else if (MoyenService.isMaintenanceType(item.AffectationMoyenTypeId)) {
                        item.CiId = null;
                        item.PersonnelId = null;
                        item.ConducteurId = null;
                        item.SiteId = null;
                    }

                    item.AffectedTo = null;
                    item.Ci = null;
                    item.Moyen = null;
                    item.Personnel = null;
                    item.Conducteur = null;
                    item.TypeAffectation = null;
                    item.Site = null;
                    item.AffectationMoyenId = item.AffectationMoyenId > 0 ? item.AffectationMoyenId : 0;
                });
            }
        }

        // Hanlde init location
        function handleInitLocation() {
            $scope.$broadcast('event.init.location');
        }

        // Hanlde mise à jour des pointages matériel
        function actionUpdatePointageMoyen(dates) {
            let dateDebut = moment(dates.dateDebut).format('YYYY-MM-DD');
            let dateFin = moment(dates.dateFin).format('YYYY-MM-DD');
            $ctrl.errorPersonnel = [];

            $q.when()
                .then(BeginAction)
                .then(function () {
                    MoyenService.UpdateMoyenPointage({ dateDebut: dateDebut, dateFin: dateFin })
                        .$promise
                        .then(function (result) {
                            actionOnUpdateSuccess(result);
                        })
                        .catch(function (error) {
                            Notify.error(error);
                        })
                        .finally(EndAction);
                });
        }

        // Succés de l'opération de la mise à jour des pointages .
        function actionOnUpdateSuccess(result) {
            if (!result.Error) {
                if (result.PersonnelListNotRegistred) {
                    Notify.warning('Mise à jour effectuée avec succés . Attention il y a des personnel sans pointage .');
                } else {
                    Notify.message('Mise à jour effectuée avec succés');
                }
            } else {
                Notify.error(result.Error);
            }

            $ctrl.errorPersonnel = result.PersonnelListNotRegistred;
        }

        // Hanlde mise à jour des pointages matériel
        function actionExportPointageMoyen(dates) {
            let dateDebut = moment(dates.dateDebut).format('YYYY-MM-DD');
            let dateFin = moment(dates.dateFin).format('YYYY-MM-DD');

            $q.when()
                .then(ProgressBar.start)
                .then(function () {
                    MoyenService.ExportPointageMoyen({ dateDebut: dateDebut, dateFin: dateFin })
                        .$promise
                        .then(function (result) {
                            actionOnSuccessPointageMoyen(result);
                        })
                        .catch(function (error) {
                            Notify.error("Erreur lors de l'envoi du flux .");
                        })
                        .finally(function () {
                            ProgressBar.complete();
                        });
                });
        }

        // Hanlde succés mise à jour des pointages matériel
        function actionOnSuccessPointageMoyen(model) {
            if (model.Code === MoyenService.EnvoiPointageMoyenResultSuccessCode) {
                let message = 'Flux envoyé avec succès : ';
                Notify.message(message + model.Message);
            } else {
                Notify.error(model.Message);
            }
        }

        /*
		 *@description Début d'une action
		 */
        function BeginAction() {
            ProgressBar.start();
            $ctrl.isBusy = true;
        }

		/*
		 *@description Fin d'une action
		 */
        function EndAction() {
            ProgressBar.complete();
            $ctrl.isBusy = false;
        }
    }
}(angular));