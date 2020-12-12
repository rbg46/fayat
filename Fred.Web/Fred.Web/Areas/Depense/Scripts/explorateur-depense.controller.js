(function (angular) {
    'use strict';

    angular.module('Fred').controller('ExplorateurDepenseController', ExplorateurDepenseController);

    ExplorateurDepenseController.$inject = ['$scope', '$q', '$filter', 'DepenseService', 'Notify', 'ProgressBar', 'confirmDialog', 'favorisService', 'ExplorateurDepenseHelperService', 'FamilleOperationDiverseService', '$uibModal', 'CIService', '$timeout', '$window', 'UserService', 'FeatureFlags'];

    function ExplorateurDepenseController($scope, $q, $filter, DepenseService, Notify, ProgressBar, confirmDialog, favorisService, ExplorateurDepenseHelperService, FamilleOperationDiverseService, $uibModal, CIService, $timeout, $window, UserService, FeatureFlags) {
        // assignation de la valeur du scope au controller pour les templates non mis à jour
        var $ctrl = this;

        init();

        return $ctrl;

        /**
         * Initialisation du controller.
         * 
         */
        function init() {
            angular.extend($ctrl, {
                // Instanciation Objet Ressources
                resources: resources,
                busy: false,
                paging: { pageSize: 25, page: 1, hasMorePage: true },
                data: [],
                depenses: [],
                filter: null,
                oldFilter: null,
                oldFilters: null,
                favoriId: null,
                axeAnalytique: { tacheRessource: 0, ressourceTache: 1 },
                typeAxes: {
                    T1: { order: 0, name: "T1", selected: true, type: "T" },
                    T2: { order: 1, name: "T2", selected: true, type: "T" },
                    T3: { order: 2, name: "T3", selected: true, type: "T" },
                    Chapitre: { order: 0, name: "Chapitre", selected: true, type: "R" },
                    SousChapitre: { order: 1, name: "SousChapitre", selected: true, type: "R" },
                    Ressource: { order: 2, name: "Ressource", selected: true, type: "R" }
                },
                msgAxeAnalytique: "",
                showPanelAxe: false,
                panelAxeOpened: false,
                cumul: false,
                today: moment(),
                totalMontantHT: 0,
                allChecked: false,
                exportOptions: { all: 1, selected: 2 },
                selectedDepenses: [],
                preselectedCiId: null,
                preselectedPeriodeDebut: null,
                preselectedPeriodeFin: null
            });

            $ctrl.filters = [
                { id: 'reception', value: { label: "Réception", active: false } },
                { id: 'far', value: { label: "Ajust. FAR", active: false } },
                { id: 'facturation', value: { label: "Facture", active: false } },
                { id: 'valorisation', value: { label: "Valorisation", active: false } },
                { id: 'od', value: { label: "OD", active: false } },
                { id: 'ecart', value: { label: "Ecart", active: false } },
                { id: 'nonCommandee', value: { label: "Non cmdée", active: false } },
                { id: 'avoir', value: { label: "Avoir", active: false } },
                { id: 'dateDepenseDebut', value: { _label: "Début dépense :", label: "Début depense :", active: false } },
                { id: 'dateDepenseFin', value: { _label: "Fin dépense :", label: "Fin dépense :", active: false } },
                { id: 'dateFactureDebut', value: { _label: "Début facture :", label: "Début facture :", active: false } },
                { id: 'dateFactureFin', value: { _label: "Fin facture :", label: "Fin facture :", active: false } },
                { id: 'dateRapprochement', value: { _label: "Date rappro. :", label: "Date rappro. :", active: false } },
                { id: 'montantHTDebut', value: { _label: "Montant >=", label: "Montant >=", active: false } },
                { id: 'montantHTFin', value: { _label: "Montant <=", label: "Montant <=", active: false } },
                { id: 'Fournisseur', value: { _label: "Fourn. :", label: "Fourn. :", active: false } },
                { id: 'Ressource', value: { _label: "Ress. :", label: "Ress. :", active: false } },
                { id: 'Tache', value: { _label: "Tâche :", label: "Tâche :", active: false } },
                { id: 'moInt', value: { label: "MO Int.", active: false } },
                { id: 'moInterim', value: { label: "MO Interim.", active: false } },
                { id: 'materielInt', value: { label: "Matériel Int.", active: false } },
                { id: 'materielExt', value: { label: "Matériel Ext.", active: false } },
                { id: 'Agence', value: { _label: "Agence :", label: "Agence :", active: false } },
                { id: 'personnelInInsertion', value: { _label: "En insertion", label: "En insertion", active: false } },
                { id: 'EnergieOnly', value: { _label: "Energie Uniquement", label: "Energie Uniquement", active: false } },
                { id: 'typeOdRct', value: { active: false } },
                { id: 'typeOdMo', value: { active: false } },
                { id: 'typeOdAch', value: { active: false } },
                { id: 'typeOdMit', value: { active: false } },
                { id: 'typeOdMi', value: { active: false } },
                { id: 'typeOdOth', value: { active: false } },
                { id: 'typeOdFg', value: { active: false } },
                { id: 'typeOdOthd', value: { active: false } },
                { id: 'TypeRessource', value: { _label: "Type ress. :", label: "Type ress. :", active: false } }
            ];

            FredToolBox.bindScrollEnd('#explorateur-depense-list', actionLoadMore);

            // Default axis
            $ctrl.selectedAxeTaches = [$ctrl.typeAxes.T1, $ctrl.typeAxes.T2, $ctrl.typeAxes.T3];
            $ctrl.selectedAxeRessources = [$ctrl.typeAxes.Chapitre, $ctrl.typeAxes.SousChapitre, $ctrl.typeAxes.Ressource];
            $ctrl.filterActiveOnAxe = false;
            $ctrl.featureFlagActive = FeatureFlags.getFlagStatus('ActivationUS13085_6000');

            $ctrl.initFilters = angular.copy($ctrl.filters);
            $ctrl.initTypeAxes = angular.copy($ctrl.typeAxes);
            $ctrl.initSelectedAxeTaches = angular.copy($ctrl.selectedAxeTaches);
            $ctrl.initSelectedAxeRessources = angular.copy($ctrl.selectedAxeRessources);

            $ctrl.handleShowLookup = handleShowLookup;
            $ctrl.handleLookupSelection = handleLookupSelection;
            $ctrl.handleLookupDeletion = handleLookupDeletion;
            $ctrl.handleSelectAxeAnalytique = handleSelectAxeAnalytique;
            $ctrl.handleCollapse = handleCollapse;
            $ctrl.handleSearch = handleSearch;
            $ctrl.handleSelectAxeAnalytiqueOrder = handleSelectAxeAnalytiqueOrder;
            $ctrl.handleSelectAxeAnalytiqueDetail = handleSelectAxeAnalytiqueDetail;
            $ctrl.handleValidateAxeAnalytique = handleValidateAxeAnalytique;
            $ctrl.handleCancelAxeAnalytique = handleCancelAxeAnalytique;
            $ctrl.handleOpenAxeAnalytiquePanel = handleOpenAxeAnalytiquePanel;
            $ctrl.handleInitFilter = handleInitFilter;
            $ctrl.handleResetFilter = handleResetFilter;
            $ctrl.handleCumul = handleCumul;
            $ctrl.handleCollapseOrUncollapseAll = handleCollapseOrUncollapseAll;
            $ctrl.handleSelectAllAxeAnalytique = handleSelectAllAxeAnalytique;
            $ctrl.handleOpenExportModal = handleOpenExportModal;
            $ctrl.handleSelectDepense = ExplorateurDepenseHelperService.actionSelectDepense;
            $ctrl.handleSelecAlltDepenses = ExplorateurDepenseHelperService.actionSelectAllDepenses;
            $ctrl.handleUncheckAll = handleUncheckAll;
            $ctrl.handleFilterSelection = handleFilterSelection;
            $ctrl.handleOpenReplaceTaskHistoryModal = handleOpenReplaceTaskHistoryModal;
            $ctrl.handleOpenReplaceTaskModal = handleOpenReplaceTaskModal;
            $ctrl.handleAddFilter2Favoris = handleAddFilter2Favoris;
            $ctrl.handleLoadFavori = handleLoadFavori;
            $ctrl.handleSaveFilter = handleSaveFilter;
            $ctrl.handleCancelFilter = handleCancelFilter;
            $ctrl.reloadRightPanel = reloadRightPanel;
            $ctrl.handleSelectFournisseurAgence = handleSelectFournisseurAgence;
            $ctrl.handleShowPickListFournisseurAgence = handleShowPickListFournisseurAgence;
            $ctrl.handleClearAgenceAndFournisseur = handleClearAgenceAndFournisseur;
            $ctrl.isChecked = ExplorateurDepenseHelperService.isChecked;

            $ctrl.initWithCiId = initWithCiId;
            $ctrl.initSearchFilterRedirect = initSearchFilterRedirect;

            $ctrl.handleOnOrderChange = handleOnOrderChange;

            // Ici on définit l'utilisateur (en attendant le multi société propre) pour pouvoir appeller les API selon Razel ou FayatTP
            UserService.getCurrentUser().then(function(user) {
                $ctrl.isUserRZB = user.Personnel.Societe.Groupe.Code.trim() === 'GRZB' ? true : false;
                $ctrl.isUserFayatTP = user.Personnel.Societe.Groupe.Code.trim() === 'GFTP' ? true : false;
                $ctrl.userCompanyId = user.Personnel.Societe.SocieteId;

                initTypeOdFilter();
            });
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            HANDLERS
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @description Action de click sur la double picklist fournisseur agence
         */
        function handleSelectFournisseurAgence(fournisseur, agence) {
            handleLookupSelection('Fournisseur', fournisseur);
            if (agence && !agence.IsAgencePrincipale) {
                handleLookupSelection('Agence', agence);
            }
        }

        /*
        * @description Action de click pour afficher la double picklist fournisseur agence
        */
        function handleShowPickListFournisseurAgence() {

            // Event d'affichage de la dualpicklist
            $scope.$broadcast("showDualPicklist");
        }

        /*
        * @description Action de suppression de fournisseur agence
        */
        function handleClearAgenceAndFournisseur() {
            handleLookupDeletion('Fournisseur');
            handleLookupDeletion('Agence');
        }

        /**
         * Lance la recherche
         */
        function handleSearch() {
            if (!$ctrl.filter.CiId) {
                confirmDialog.confirm(resources, "Veuillez sélectionner un CI avant de lancer la recherche.", "flaticon flaticon-warning");
            }
            else if (!$ctrl.filter.PeriodeFin) {
                confirmDialog.confirm(resources, "Veuillez sélectionner une Période de Fin avant de lancer la recherche.", "flaticon flaticon-warning");
            }
            else {
                $q.when()
                    .then(ProgressBar.start)
                    .then(function () {
                        changePeriodeFilter();
                        $ctrl.filter.Axes = [];
                        return $ctrl.filter;
                    })
                    .then(actionGetTree)
                    .then(actionCalculTotalMontantHT)
                    .then(ProgressBar.complete);
            }
        }

        function handleOnOrderChange(field) {
            // Désactive tous les autres tris
            Object.keys($ctrl.filter).forEach(function (key, index) {
                if (key.endsWith('Asc') && key !== field) {
                    $ctrl.filter[key] = null;
                }
            });
            // Recharge le tableau de droite
            reloadRightPanel();
        }

        /**
         * Valider les axes analytiques et referme le panneau latéral
         */
        function handleValidateAxeAnalytique() {
            if (actionCheckAxeAnalytique()) {
                $ctrl.showPanelAxe = !$ctrl.showPanelAxe;
                $ctrl.panelAxeOpened = true;

                if ($ctrl.filter.AxePrincipal.length === 0) {
                    $ctrl.filter.AxePrincipal = $ctrl.filter.AxeSecondaire;
                    $ctrl.filter.AxeSecondaire = [];

                    if ($ctrl.filter.AxeAnalytique === $ctrl.axeAnalytique.tacheRessource) {
                        $ctrl.filter.AxeAnalytique = $ctrl.axeAnalytique.ressourceTache;
                    }
                    else if ($ctrl.filter.AxeAnalytique === $ctrl.axeAnalytique.ressourceTache) {
                        $ctrl.filter.AxeAnalytique = $ctrl.axeAnalytique.tacheRessource;
                    }
                }
                // Save param before validation
                $ctrl.savedFilterAxeAnalytique = angular.copy($ctrl.filter);
                $ctrl.savedSelectedAxeTaches = angular.copy($ctrl.selectedAxeTaches);
                $ctrl.savedSelectedAxeRessources = angular.copy($ctrl.selectedAxeRessources);
                $ctrl.savedTypeAxes = angular.copy($ctrl.typeAxes);
                checkFilterActiveOnAxe();
            }
        }

        /**
         * Gestion du bouton Cumul
         */
        function handleCumul() {
            $ctrl.cumul = !$ctrl.cumul;
            $ctrl.filter.PeriodeDebut = $ctrl.cumul ? null : $ctrl.today;
        }

        /**
         * Annule les axes analytiques et referme le panneau latéral
         */
        function handleCancelAxeAnalytique() {
            $ctrl.typeAxes = $ctrl.savedTypeAxes;
            $ctrl.filter = $ctrl.savedFilterAxeAnalytique;
            $ctrl.selectedAxeTaches = $ctrl.savedSelectedAxeTaches;
            $ctrl.selectedAxeRessources = $ctrl.savedSelectedAxeRessources;
            $ctrl.showPanelAxe = !$ctrl.showPanelAxe;
            checkFilterActiveOnAxe();
        }

        /**
         * Ouverture du panneau latéral axe analytique
         */
        function handleOpenAxeAnalytiquePanel() {
            $ctrl.showPanelAxe = !$ctrl.showPanelAxe;

            actionCheckAxeAnalytique();

            // Save param before modification
            $ctrl.savedFilterAxeAnalytique = angular.copy($ctrl.filter);
            $ctrl.savedSelectedAxeTaches = angular.copy($ctrl.selectedAxeTaches);
            $ctrl.savedSelectedAxeRessources = angular.copy($ctrl.selectedAxeRessources);
            $ctrl.savedTypeAxes = angular.copy($ctrl.typeAxes);
        }

        /**
         * Décoche toute la sélection
         * */
        function handleUncheckAll() {
            $ctrl.allChecked = false;
            handleSelectAllAxeAnalytique();
        }

        /**
         * Sélection de tous les axes analytique
         */
        function handleSelectAllAxeAnalytique() {
            $ctrl.children = [];

            ExplorateurDepenseHelperService.actionCheckAllAxeAnalytique($ctrl.data, $ctrl.allChecked);

            if ($ctrl.allChecked) {
                angular.forEach($ctrl.data, function (val, key) {

                    val.Picked = false;

                    // Unpicked all children
                    actionUnpickAxeEnfants(val);

                    // Then delete from filter
                    ExplorateurDepenseHelperService.actionDeleteAxeEnfants($ctrl.children, $ctrl.filter.Axes);

                    var tuple = { Axe1: val, Axe2: null, Id: val.Id };

                    actionAddOrDelete(tuple);
                });
            }
            else {
                angular.forEach($ctrl.data, function (val, key) {
                    val.Picked = false;
                    // Unpicked all children
                    actionUnpickAxeEnfants(val);
                });
                ExplorateurDepenseHelperService.deselectAllDepense($ctrl.selectedDepenses);
                $ctrl.filter.Axes = [];
            }

            // Récupération des dépenses
            reloadRightPanel();
        }

        /**
         * Recharge la partie droite du tableau
         * en conservant la sélection précédente
         */
        function reloadRightPanel() {
            // Récupération des dépenses
            $q.when()
                .then(ProgressBar.start)
                .then(function () { return actionLoadDepenses($ctrl.filter, true); })
                .then(ProgressBar.complete)
                .then(delayedLoadMoreIfRequired);
        }

        /**
         * Sélection d'un axe et récupération des dépenses associées
         * @param {any} event event
         * @param {any} levelNumber niveau d'axe
         * @param {any} currentLevel axe courant
         * @param {any} axe1 axe 1
         * @param {any} axe2 axe 2
         * @param {any} axe3 axe 3
         * @param {any} axe4 axe 4
         */
        function handleSelectAxeAnalytique(event, levelNumber, currentLevel, axe1, axe2, axe3, axe4) {
            var axePCount = $ctrl.filter.AxePrincipal.length;
            var axeSCount = $ctrl.filter.AxeSecondaire.length;
            $ctrl.children = [];

            // Gestion du click sur l'axe
            if (event) {
                if (currentLevel.Checked && !currentLevel.Picked) {
                    return;
                }
                else {
                    currentLevel.Checked = !currentLevel.Checked;
                   
                }
            }

            if (!currentLevel.Checked) {
                ExplorateurDepenseHelperService.deselectAllDepense($ctrl.selectedDepenses);
            }

            currentLevel.Picked = !currentLevel.Picked;

            ExplorateurDepenseHelperService.actionCheckAxeAnalytique(currentLevel, $ctrl.data);

            //Get tuple : axe1 et axe 2
            var tuple = ExplorateurDepenseHelperService.actionGetTuple(levelNumber, currentLevel, axe1, axe2, axe3, axe4, axePCount, axeSCount, $ctrl.typeAxes);

            // Unpicked all children
            actionUnpickAxeEnfants(currentLevel);

            // Then delete from filter
            ExplorateurDepenseHelperService.actionDeleteAxeEnfants($ctrl.children, $ctrl.filter.Axes);

            // Add or delete tuple
            actionAddOrDelete(tuple);

            // Récupération des dépenses
            reloadRightPanel();
        }

        /**
         * Sélection de l'axe analytique (Tache>Ressource ou Ressource>Tache)
         * @param {any} order ordre choisit
         */
        function handleSelectAxeAnalytiqueOrder(order) {

            if ($ctrl.filter.AxeAnalytique !== order) {
                if (order === $ctrl.axeAnalytique.tacheRessource && $ctrl.selectedAxeTaches.length === 0) {
                    Notify.error("Veuillez sélectionner au moins niveau de tâche");
                }
                else if (order === $ctrl.axeAnalytique.ressourceTache && $ctrl.selectedAxeRessources.length === 0) {
                    Notify.error("Veuillez sélectionner au moins niveau de resource");
                }
                else {
                    actionReorderAxes(order);
                    $ctrl.filter.AxeAnalytique = order;
                }
            }
        }

        /**
         * Sélection des axes
         * @param {any} typeAxe type d'axe
         */
        function handleSelectAxeAnalytiqueDetail(typeAxe) {
            typeAxe.selected = !typeAxe.selected;

            if (typeAxe.type === "T") {
                actionAddOrDeleteAxe($ctrl.selectedAxeTaches, typeAxe);
                $ctrl.selectedAxeTaches.sort(function (a, b) { return a.order - b.order; });
            }
            else if (typeAxe.type === "R") {
                actionAddOrDeleteAxe($ctrl.selectedAxeRessources, typeAxe);
                $ctrl.selectedAxeRessources.sort(function (a, b) { return a.order - b.order; });
            }

            if ($ctrl.selectedAxeRessources.length > 0 || $ctrl.selectedAxeTaches.length > 0) {
                $ctrl.msgAxeAnalytique = "";
            }

            actionReorderAxes($ctrl.filter.AxeAnalytique);
        }

        /**
         * Gestion du plier/déplier des axes analytiques     
         * @param {any} id identifiant du panel
         * @param {any} close boolean force la fermeture
         */
        function handleCollapse(id, close) {
            var e = document.querySelector(id);
            var action = "";

            if (!close) {
                var isExpanded = angular.element(e).attr("aria-expanded");
                action = isExpanded === "true" ? "hide" : "show";
            }
            else {
                action = "hide";
            }

            angular.element(e).collapse(action);
        }

        /**
         * Déplie ou replie tout le tableau de gauche
         * @param {any} collapse vrai ou faux
         */
        function handleCollapseOrUncollapseAll(collapse) {
            if (collapse === true) {
                angular.element(document.querySelectorAll('.explorator-list .collapse')).collapse('hide');
            }
            else {
                angular.element(document.querySelectorAll('.explorator-list .collapse:not(.in)')).collapse('show');
            }
        }

        /**
         * Gestion de l'URL pour la Lookup
         * @param {any} val Entité    
         * @returns {any} URL
         */
        function handleShowLookup(val) {
            var url = '/api/' + val + '/SearchLight/page=1&pageSize=20&societeId={0}&ciId={1}';
            switch (val) {
                case "CI":
                    url = String.format(url, null, null);
                    break;
                case "Ressource":
                    url = String.format(url, $ctrl.filter.CI.SocieteId, null);
                    break;
                case "Fournisseur":
                    url = String.format(url, null, null);
                    break;
                case "TypeRessource":
                    url = String.format(url, $ctrl.filter.CI.SocieteId, null);
                    break;
                default:
                    url = '/api/' + val + '/SearchLight/';
                    break;
            }
            return url;
        }

        /**
         * Détermine si le CI fait partie d'une societé SEP ou non
         * @param {any} ciId id du CI
         */
        function IsSep(ciId) {
            DepenseService.IsSep(ciId)
                .then((response) => {
                    $ctrl.filter.Sep = response.data.isSep;
                });
        }

        /**
         * Gestion de la sélection des items des lookup
         * @param {any} type Type de lookup
         * @param {any} item objet sélectionné
         */
        function handleLookupSelection(type, item) {
            var f = $filter('filter')($ctrl.filters, { id: type }, true)[0];

            switch (type) {
                case "CI":
                    $ctrl.filter.CiId = item.CiId;
                    $ctrl.filters.find(x => x.id === 'EnergieOnly').value.active = false;
                    IsSep(item.CiId);
                    break;
                case "Ressource":
                    $ctrl.filter.RessourceId = item.RessourceId;

                    if (f) {
                        if ($ctrl.filter.RessourceId && !f.value.active) {
                            f.value.active = !f.value.active;
                        }

                        f.value.label = f.value._label + ' ' + item.Code;
                    }
                    break;
                case "Tache":
                    $ctrl.filter.TacheId = item.TacheId;

                    if (f) {
                        if ($ctrl.filter.TacheId && !f.value.active) {
                            f.value.active = !f.value.active;
                        }

                        f.value.label = f.value._label + ' ' + item.Code;
                    }
                    break;
                case "Fournisseur":
                    $ctrl.filter.FournisseurId = item.FournisseurId;
                    $ctrl.filter.Fournisseur = item;

                    if (f) {
                        if ($ctrl.filter.FournisseurId && !f.value.active) {
                            f.value.active = !f.value.active;
                        }

                        f.value.label = f.value._label + ' ' + item.Code;
                    }
                    break;
                case "Agence":
                    $ctrl.filter.AgenceId = item.AgenceId;
                    $ctrl.filter.Agence = item;

                    if (f) {
                        if ($ctrl.filter.AgenceId && !f.value.active) {
                            f.value.active = !f.value.active;
                        }

                        f.value.label = f.value._label + ' ' + item.Code;
                    }
                    break;
                case "TypeRessource":
                    $ctrl.filter.TypeRessourceId = item.TypeRessourceId;
                    $ctrl.filter.TypeRessource = item;

                    if (f) {
                        if ($ctrl.filter.TypeRessourceId && !f.value.active) {
                            f.value.active = !f.value.active;
                        }

                        f.value.label = f.value._label + ' ' + item.Code;
                    }
                    break;
                default:
                    break;
            }
            getIsFilteredAndRedBubbleFiltered();
        }

        /**
         * Gestion de la suppression des items des lookup
         * @param {any} type Type de lookup
         */
        function handleLookupDeletion(type) {
            var f = $filter('filter')($ctrl.filters, { id: type }, true)[0];

            if (f) {
                f.value.active = false;
                f.value.label = f.value._label;
            }

            switch (type) {
                case "CI":
                    $ctrl.filter.CI = null;
                    $ctrl.filter.CIId = null;
                    break;
                case "Ressource":
                    $ctrl.filter.Ressource = null;
                    $ctrl.filter.RessourceId = null;
                    break;
                case "Tache":
                    $ctrl.filter.TacheId = null;
                    $ctrl.filter.Tache = null;
                    break;
                case "Fournisseur":
                    $ctrl.filter.Fournisseur = null;
                    $ctrl.filter.FournisseurId = null;
                    break;
                case "Agence":
                    $ctrl.filter.Agence = null;
                    $ctrl.filter.AgenceId = null;
                    break;
                case "TypeRessource":
                    $ctrl.filter.TypeRessource = null;
                    $ctrl.filter.TypeRessourceId = null;
                    break;
                default:
                    break;
            }

            getIsFilteredAndRedBubbleFiltered();
        }

        /**
         * Réinitialisation du filtre (uniquement ceux définis dans la Recherche Avancée)
         */
        function handleInitFilter() {
            $ctrl.filter.DateDepenseDebut = null;
            $ctrl.filter.DateDepenseFin = null;
            $ctrl.filter.DateFactureDebut = null;
            $ctrl.filter.DateFactureFin = null;
            $ctrl.filter.Fournisseur = null;
            $ctrl.filter.FournisseurId = null;
            $ctrl.filter.Ressource = null;
            $ctrl.filter.RessourceId = null;
            $ctrl.filter.Tache = null;
            $ctrl.filter.TacheId = null;
            $ctrl.filter.MontantHTDebut = null;
            $ctrl.filter.MontantHTFin = null;
            $ctrl.filter.DateRapprochement = null;
            $ctrl.filter.PersonnelInInsertion = false;
            $ctrl.filter.TakeReception = false;
            $ctrl.filter.TakeFar = false;
            $ctrl.filter.TakeValorisation = false;
            $ctrl.filter.TakeOd = false;
            $ctrl.filter.TakeOdRct = false;
            $ctrl.filter.TakeOdMo = false;
            $ctrl.filter.TakeOdAch = false;
            $ctrl.filter.TakeOdMit = false;
            $ctrl.filter.TakeOdMi = false;
            $ctrl.filter.TakeOdOth = false;
            $ctrl.filter.TakeOdFg = false;
            $ctrl.filter.TakeOdOthd = false;
            $ctrl.filter.TakeEcart = false;
            $ctrl.filter.TakeAvoir = false;
            $ctrl.filter.TakeFacturation = false;
            $ctrl.filter.TakeNonCommandee = false;
            $ctrl.filter.TakeMOInt = false;
            $ctrl.filter.TakeMOInterim = false;
            $ctrl.filter.TakeMaterielInt = false;
            $ctrl.filter.TakeMaterielExt = false;
            $ctrl.filter.EnergieOnly = false;
            $ctrl.filter.TypeRessource = null;
            $ctrl.filter.TypeRessourceId = null;

            angular.forEach($ctrl.filters, function (val) { val.value.active = false; });
            $ctrl.isFiltered = false;
        }

        /*
         * @description Gestion de la modal export des dépenses
         */
        function handleOpenExportModal() {
            var modalInstance = $uibModal.open({
                animation: true,
                component: 'exportDepenseComponent',
                resolve: {
                    resources: function () { return $ctrl.resources; },
                    hasSelectedDepenses: function () { return $ctrl.filter.Axes && $ctrl.filter.Axes.length > 0; }
                }
            });

            // Gestion résultat de la modal
            modalInstance.result.then(function (value) {
                var filter = angular.copy($ctrl.filter);

                if (value === $ctrl.exportOptions.all) {
                    filter.Axes = [];

                    angular.forEach($ctrl.data, function (val, key) {
                        var tuple = { Axe1: val, Axe2: null, Id: val.Id };
                        filter.Axes.push(tuple);
                    });
                }

                $q.when()
                    .then(ProgressBar.start)
                    .then(function () { return filter; })
                    .then(actionGetExportExcel)
                    .finally(ProgressBar.complete);
            });
        }

        function handleOpenReplaceTaskModal(selectedDepenses) {
            var modalInstance = $uibModal.open({
                animation: true,
                component: 'replaceTaskComponent',
                resolve: {
                    resources: function () { return $ctrl.resources; },
                    selectedDepenses: function () {
                        return selectedDepenses;
                    },
                    CI: function () {
                        return $ctrl.filter.CiId;
                    },
                    scope: $ctrl
                }

            });

            // Gestion résultat de la modal
            modalInstance.result.then(function (value) {
            });
        }

        function handleOpenReplaceTaskHistoryModal(selectedDepense) {
            var modalInstance = $uibModal.open({
                animation: true,
                size: 'lg',
                windowClass: 'task-histo-modal',
                component: 'replaceTaskHistoryComponent',
                resolve: {
                    resources: function () { return $ctrl.resources; },
                    selectedDepense: function () {
                        return selectedDepense;
                    },
                    scope: $ctrl
                }
            });

            // Gestion résultat de la modal
            modalInstance.result.then(function (value) {
            });
        }

        function handleFilterSelection(type, forceDelete) {
            var f = $filter('filter')($ctrl.filters, { id: type }, true)[0];
            switch (type) {
                case 'reception':
                    $ctrl.filter.TakeReception = !$ctrl.filter.TakeReception;
                    f.value.active = !f.value.active;
                    break;
                case 'facturation':
                    $ctrl.filter.TakeFacturation = !$ctrl.filter.TakeFacturation;
                    f.value.active = !f.value.active;
                    break;
                case 'far':
                    $ctrl.filter.TakeFar = !$ctrl.filter.TakeFar;
                    f.value.active = !f.value.active;
                    break;
                case 'valorisation':
                    $ctrl.filter.TakeValorisation = !$ctrl.filter.TakeValorisation;
                    f.value.active = !f.value.active;
                    break;
                case 'od':
                    $ctrl.filter.TakeOd = !$ctrl.filter.TakeOd;
                    f.value.active = !f.value.active;
                    break;
                case 'ecart':
                    $ctrl.filter.TakeEcart = !$ctrl.filter.TakeEcart;
                    f.value.active = !f.value.active;
                    break;
                case 'nonCommandee':
                    $ctrl.filter.TakeNonCommandee = !$ctrl.filter.TakeNonCommandee;
                    f.value.active = !f.value.active;
                    break;
                case 'avoir':
                    $ctrl.filter.TakeAvoir = !$ctrl.filter.TakeAvoir;
                    f.value.active = !f.value.active;
                    break;
                case 'dateDepenseDebut':
                    if ($ctrl.filter.DateDepenseDebut && !f.value.active) {
                        f.value.active = !f.value.active;
                    }

                    var ddd = $filter('date')($ctrl.filter.DateDepenseDebut, 'dd/MM/yyyy');

                    if (ddd) {
                        f.value.label = f.value._label + ' ' + ddd;
                    }
                    else {
                        f.value.active = false;
                        $ctrl.filter.DateDepenseDebut = null;
                        f.value.label = f.value._label;
                    }

                    if (forceDelete) {
                        f.value.active = false;
                        $ctrl.filter.DateDepenseDebut = null;
                        f.value.label = f.value._label;
                    }
                    break;
                case 'dateDepenseFin':
                    if ($ctrl.filter.DateDepenseFin && !f.value.active) {
                        f.value.active = !f.value.active;
                    }

                    var ddf = $filter('date')($ctrl.filter.DateDepenseFin, 'dd/MM/yyyy');

                    if (ddf) {
                        f.value.label = f.value._label + ' ' + ddf;
                    }
                    else {
                        f.value.active = false;
                        $ctrl.filter.DateDepenseFin = null;
                        f.value.label = f.value._label;
                    }

                    if (forceDelete) {
                        f.value.active = false;
                        $ctrl.filter.DateDepenseFin = null;
                        f.value.label = f.value._label;
                    }
                    break;
                case 'dateFactureDebut':
                    if ($ctrl.filter.DateFactureDebut && !f.value.active) {
                        f.value.active = !f.value.active;
                    }

                    var dfd = $filter('date')($ctrl.filter.DateFactureDebut, 'dd/MM/yyyy');

                    if (dfd) {
                        f.value.label = f.value._label + ' ' + dfd;
                    }
                    else {
                        f.value.active = false;
                        $ctrl.filter.DateFactureDebut = null;
                        f.value.label = f.value._label;
                    }

                    if (forceDelete) {
                        f.value.active = false;
                        $ctrl.filter.DateFactureDebut = null;
                        f.value.label = f.value._label;
                    }
                    break;
                case 'dateFactureFin':
                    if ($ctrl.filter.DateFactureFin && !f.value.active) {
                        f.value.active = !f.value.active;
                    }

                    var dff = $filter('date')($ctrl.filter.DateFactureFin, 'dd/MM/yyyy');

                    if (dff) {
                        f.value.label = f.value._label + ' ' + dff;
                    }
                    else {
                        f.value.active = false;
                        $ctrl.filter.DateFactureFin = null;
                        f.value.label = f.value._label;
                    }

                    if (forceDelete) {
                        f.value.active = false;
                        $ctrl.filter.DateFactureFin = null;
                        f.value.label = f.value._label;
                    }
                    break;
                case 'dateRapprochement':
                    if ($ctrl.filter.DateRapprochement && !f.value.active) {
                        f.value.active = !f.value.active;
                    }

                    var d2 = $filter('date')($ctrl.filter.DateRapprochement, 'MM/yyyy');

                    if (d2) {
                        f.value.label = f.value._label + ' ' + d2;
                    }
                    else {
                        f.value.active = false;
                        $ctrl.filter.DateRapprochement = null;
                        f.value.label = f.value._label;
                    }

                    if (forceDelete) {
                        f.value.active = false;
                        $ctrl.filter.DateRapprochement = null;
                        f.value.label = f.value._label;
                    }
                    break;
                case 'montantHTDebut':
                    if ($ctrl.filter.MontantHTDebut && !f.value.active) {
                        f.value.active = !f.value.active;
                    }

                    if ($ctrl.filter.MontantHTDebut !== undefined && $ctrl.filter.MontantHTDebut !== null && $ctrl.filter.MontantHTDebut !== "") {
                        f.value.label = f.value._label + ' ' + $filter('number')($ctrl.filter.MontantHTDebut, 0);
                    }
                    else {
                        f.value.active = false;
                        $ctrl.filter.MontantHTDebut = null;
                        f.value.label = f.value._label;
                    }

                    if (forceDelete) {
                        f.value.active = false;
                        $ctrl.filter.MontantHTDebut = null;
                        f.value.label = f.value._label;
                    }
                    break;
                case 'montantHTFin':
                    if ($ctrl.filter.MontantHTFin && !f.value.active) {
                        f.value.active = !f.value.active;
                    }

                    if ($ctrl.filter.MontantHTFin !== undefined && $ctrl.filter.MontantHTFin !== null && $ctrl.filter.MontantHTFin !== "") {
                        f.value.label = f.value._label + ' ' + $filter('number')($ctrl.filter.MontantHTFin, 0);
                    }
                    else {
                        f.value.active = false;
                        $ctrl.filter.MontantHTFin = null;
                        f.value.label = f.value._label;
                    }

                    if (forceDelete) {
                        f.value.active = false;
                        $ctrl.filter.MontantHTFin = null;
                        f.value.label = f.value._label;
                    }
                    break;
                case 'Fournisseur':
                    if (forceDelete) {
                        f.value.active = false;
                        $ctrl.filter.FournisseurId = null;
                        $ctrl.filter.Fournisseur = null;
                        f.value.label = f.value._label;
                    }
                    break;
                case 'Agence':
                    if (forceDelete) {
                        f.value.active = false;
                        $ctrl.filter.AgenceId = null;
                        $ctrl.filter.Agence = null;
                        f.value.label = f.value._label;
                    }
                    break;
                case 'Ressource':
                    if (forceDelete) {
                        f.value.active = false;
                        $ctrl.filter.RessourceId = null;
                        $ctrl.filter.Ressource = null;
                        f.value.label = f.value._label;
                    }
                    break;
                case 'Tache':
                    if (forceDelete) {
                        f.value.active = false;
                        $ctrl.filter.TacheId = null;
                        $ctrl.filter.Tache = null;
                        f.value.label = f.value._label;
                    }
                    break;
                case 'moInt':
                    $ctrl.filter.TakeMOInt = !$ctrl.filter.TakeMOInt;
                    f.value.active = !f.value.active;
                    break;
                case 'moInterim':
                    $ctrl.filter.TakeMOInterim = !$ctrl.filter.TakeMOInterim;
                    f.value.active = !f.value.active;
                    break;
                case 'materielInt':
                    $ctrl.filter.TakeMaterielInt = !$ctrl.filter.TakeMaterielInt;
                    f.value.active = !f.value.active;
                    break;
                case 'materielExt':
                    $ctrl.filter.TakeMaterielExt = !$ctrl.filter.TakeMaterielExt;
                    f.value.active = !f.value.active;
                    break;
                case 'personnelInInsertion':
                    $ctrl.filter.PersonnelInInsertion = !$ctrl.filter.PersonnelInInsertion;
                    f.value.active = !f.value.active;
                    break;
                case 'EnergieOnly':
                    $ctrl.filter.EnergieOnly = !$ctrl.filter.EnergieOnly;
                    f.value.active = !f.value.active;
                    break;
                case 'typeOdRct':
                    $ctrl.filter.TakeOdRct = !$ctrl.filter.TakeOdRct;
                    f.value.active = !f.value.active;
                    f.value.label = $ctrl.typeOdFilterRct
                    break;
                case 'typeOdMo':
                    $ctrl.filter.TakeOdMo = !$ctrl.filter.TakeOdMo;
                    f.value.active = !f.value.active;
                    f.value.label = $ctrl.typeOdFilterMo
                    break;
                case 'typeOdAch':
                    $ctrl.filter.TakeOdAch = !$ctrl.filter.TakeOdAch;
                    f.value.active = !f.value.active;
                    f.value.label = $ctrl.typeOdFilterAch
                    break;
                case 'typeOdMit':
                    $ctrl.filter.TakeOdMit = !$ctrl.filter.TakeOdMit;
                    f.value.active = !f.value.active;
                    f.value.label = $ctrl.typeOdFilterMit
                    break;
                case 'typeOdMi':
                    $ctrl.filter.TakeOdMi = !$ctrl.filter.TakeOdMi;
                    f.value.active = !f.value.active;
                    f.value.label = $ctrl.typeOdFilterMi
                    break;
                case 'typeOdOth':
                    $ctrl.filter.TakeOdOth = !$ctrl.filter.TakeOdOth;
                    f.value.active = !f.value.active;
                    f.value.label = $ctrl.typeOdFilterOth
                    break;
                case 'typeOdFg':
                    $ctrl.filter.TakeOdFg = !$ctrl.filter.TakeOdFg;
                    f.value.active = !f.value.active;
                    f.value.label = $ctrl.typeOdFilterFg
                    break;
                case 'typeOdOthd':
                    $ctrl.filter.TakeOdOthd = !$ctrl.filter.TakeOdOthd;
                    f.value.active = !f.value.active;
                    f.value.label = $ctrl.typeOdFilterOthd
                    break;
                case 'TypeRessource':
                    if (forceDelete) {
                        f.value.active = false;
                        $ctrl.filter.TypeRessourceId = null;
                        $ctrl.filter.TypeRessource = null;
                        f.value.label = f.value._label;
                    }
                    break;
            }
            getIsFilteredAndRedBubbleFiltered();
        }

        function associateLibelleWithTypeOdFilter(val) {
            var codeTypeOd = val.Code;
            var libelleTypeOd = val.LibelleCourt;

            switch (codeTypeOd) {
                case 'RCT':
                    $ctrl.typeOdFilterRct = libelleTypeOd;
                    break;
                case 'MO':
                    $ctrl.typeOdFilterMo = libelleTypeOd;
                    break;
                case 'ACH':
                    $ctrl.typeOdFilterAch = libelleTypeOd;
                    break;
                case 'MIT':
                    $ctrl.typeOdFilterMit = libelleTypeOd;
                    break;
                case 'MI':
                    $ctrl.typeOdFilterMi = libelleTypeOd;
                    break;
                case 'OTH':
                    $ctrl.typeOdFilterOth = libelleTypeOd;
                    break;
                case 'FG':
                    $ctrl.typeOdFilterFg = libelleTypeOd;
                    break;
                case 'OTHD':
                    $ctrl.typeOdFilterOthd = libelleTypeOd;
                    break;
            }
        }

        function handleResetFilter() {
            sessionStorage.removeItem('explorateurDepenseFilter');
            $ctrl.filters = angular.copy($ctrl.initFilters);
            $ctrl.typeAxes = angular.copy($ctrl.initTypeAxes);
            $ctrl.selectedAxeTaches = angular.copy($ctrl.initSelectedAxeTaches);
            $ctrl.selectedAxeRessources = angular.copy($ctrl.initSelectedAxeRessources);
            $ctrl.filterActiveOnAxe = false;
            getIsFilteredAndRedBubbleFiltered();
            actionGetNewFilter();
        }

        function handleAddFilter2Favoris() {
            addFilter2Favoris();
        }

        function handleLoadFavori(favoriId) {
            $ctrl.favoriId = parseInt(favoriId) || 0;
            getFilterOrFavoris();
        }

        function handleSaveFilter() {
            saveFilter();
        }

        function handleCancelFilter() {
            cancelFilter();
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            ACTIONS
         * -------------------------------------------------------------------------------------------------------------
         */

        /**
         * Action génération et récupération du fichier excel des dépenses
         * @param {any} filter filtre
         * @returns {any} promise
         */
        function actionGetExportExcel(filter) {
            if ($ctrl.isUserFayatTP) {
                return DepenseService.GenerateExportForFayatTP(filter)
                    .then(function (response) {
                        DepenseService.GetExport(response.data.id, filter.CI.Code);
                    })
                    .catch(function (error) { console.log(error); });
            } else {
                return DepenseService.GenerateExport(filter)
                    .then(function (response) {
                        DepenseService.GetExport(response.data.id, filter.CI.Code);
                    })
                    .catch(function (error) { console.log(error); });
            }
        }

        function actionCheckAxeAnalytique() {
            if ($ctrl.selectedAxeRessources.length === 0 && $ctrl.selectedAxeTaches.length === 0) {
                $ctrl.msgAxeAnalytique = "Veuillez sélectionner au moins un axe.";
                return false;
            }
            else {
                $ctrl.msgAxeAnalytique = "";
                return true;
            }
        }

        /**
         * Ré ordonance des axes (principal et secondaire) en fonction de l'ordre choisit
         * @param {any} order ordre d'exploration          
         */
        function actionReorderAxes(order) {

            $ctrl.filter.AxePrincipal = [];
            $ctrl.filter.AxeSecondaire = [];

            if (order === $ctrl.axeAnalytique.ressourceTache) {
                angular.forEach($ctrl.selectedAxeRessources, function (val) { $ctrl.filter.AxePrincipal.push(val.name); });
                angular.forEach($ctrl.selectedAxeTaches, function (val) { $ctrl.filter.AxeSecondaire.push(val.name); });
            }
            else if (order === $ctrl.axeAnalytique.tacheRessource) {
                angular.forEach($ctrl.selectedAxeTaches, function (val) { $ctrl.filter.AxePrincipal.push(val.name); });
                angular.forEach($ctrl.selectedAxeRessources, function (val) { $ctrl.filter.AxeSecondaire.push(val.name); });
            }
        }

        /**
         * Ajout ou suppression d'un axe dans sa liste
         * @param {any} list liste d'axes choisi
         * @param {any} typeAxe type d'axe
         */
        function actionAddOrDeleteAxe(list, typeAxe) {
            var obj = $filter('filter')(list, { name: typeAxe.name }, true)[0];

            if (!obj) {
                list.push(typeAxe);
            }
            else {
                var index = list.indexOf(obj);
                list.splice(index, 1);
            }
            checkFilterActiveOnAxe();
        }

        function checkFilterActiveOnAxe() {
            if ($ctrl.selectedAxeTaches.length !== 3 || $ctrl.selectedAxeRessources.length !== 3) {
                $ctrl.filterActiveOnAxe = true;
            }
            else {
                $ctrl.filterActiveOnAxe = false;
            }
        }

        /**
         * Récupération des dépenses
         * @param {any} filter filtre des dépenses
         * @param {any} firstLoad premier chargement ou pas
         * @returns {any} promise
         */
        function actionLoadDepenses(filter, firstLoad) {
            if (!$ctrl.busy) {
                $ctrl.busy = true;

                if (firstLoad) {
                    $ctrl.depenses = [];
                    $ctrl.paging.page = 1;
                }

                if (filter.Axes.length === 0) {
                    $ctrl.busy = false;
                }
                else {
                    changePeriodeFilter();
                    if ($ctrl.isUserFayatTP) {
                        return getDepensesByAxesForFayatTP(filter);
                    } else {
                        return getDepensesByAxes(filter);
                    }
                }
            }
        }

        /**
         * Récupération des dépenses par Axes
         * @param {any} filter filtre des dépenses
         * @returns {any} promise
         */
        function getDepensesByAxes(filter) {
            return DepenseService.GetDepensesByAxe(filter, $ctrl.paging.page, $ctrl.paging.pageSize)
                .then(handleResponseGetDepensesByAxes())
                .catch(function (reason) { console.log(reason); })
                .finally(function () { $ctrl.busy = false; });
        }

        /**
         * Récupération des dépenses par Axes pour Fayat TP
         * @param {any} filter filtre des dépenses
         * @returns {any} promise
         */
        function getDepensesByAxesForFayatTP(filter) {
            return DepenseService.GetDepensesByAxeForFayatTP(filter, $ctrl.paging.page, $ctrl.paging.pageSize)
                .then(handleResponseGetDepensesByAxes())
                .catch(function (reason) { console.log(reason); })
                .finally(function () { $ctrl.busy = false; });
        }

        /**
         * Handle du retour de la récupération des dépenses par Axes
         * @returns {any} promise
         */
        function handleResponseGetDepensesByAxes() {
            return function (response) {
                $ctrl.explorateurDepenseResult = response.data;
                angular.forEach(response.data.Depenses, function (d) {
                    d.Selected = false;
                    d.TacheTooltip = d.TacheOrigineCodeLibelle ? String.format($ctrl.resources.TacheTooltip, d.TacheOrigineCodeLibelle, $filter('date')(d.DateComptableRemplacement, 'MM/yy')) : d.Tache.CodeLibelle;
                    $ctrl.depenses.push(d);
                });
                $ctrl.paging.hasMorePage = response.data.Depenses.length === $ctrl.paging.pageSize;
            };
        }

        /**
         * Récupération d'un nouveau filtre searchExplorateurDepense
         * @returns {any} promise
         */
        function actionGetNewFilter() {
            if (sessionStorage.getItem('explorateurDepenseFilter') !== null) {
                for (var axe in $ctrl.typeAxes) {
                    $ctrl.typeAxes[axe].selected = false;
                }
                $ctrl.filter = JSON.parse(sessionStorage.getItem('explorateurDepenseFilter')).filterValue;
                $ctrl.filters = JSON.parse(sessionStorage.getItem('explorateurDepenseFilter')).filterLabel;
                getIsFilteredAndRedBubbleFiltered();
                handleSearch();
                $ctrl.filter.PeriodeDebut = $ctrl.filter.IsCumul ? null : $ctrl.filter.PeriodeDebut;
                $ctrl.cumul = $ctrl.filter.IsCumul ? $ctrl.filter.IsCumul : $ctrl.cumul;
                $ctrl.selectedAxeTaches = [];
                $ctrl.selectedAxeRessources = [];
                var filterAxe = $ctrl.filter.AxePrincipal.concat($ctrl.filter.AxeSecondaire);
                filterAxe.forEach(fa => {
                    for (var axe in $ctrl.typeAxes) {
                        if (axe === fa) {
                            $ctrl.typeAxes[axe].selected = true;
                            if ($ctrl.typeAxes[axe].type === "T") {
                                $ctrl.selectedAxeTaches.push($ctrl.typeAxes[axe]);
                            } else if ($ctrl.typeAxes[axe].type === "R") {
                                $ctrl.selectedAxeRessources.push($ctrl.typeAxes[axe]);
                            }
                        }
                    }
                });
                checkFilterActiveOnAxe();
            } else {
                return DepenseService.GetNewFilter()
                    .then(function (filter) {
                        filter.data.PeriodeDebut = $filter('toLocaleDate')(filter.data.PeriodeDebut);
                        filter.data.PeriodeFin = $filter('toLocaleDate')(filter.data.PeriodeFin);

                        $ctrl.filter = filter.data;

                        //Maintenant que le filtre est fait, on peut regarder si on a un CI déjà pré choisi
                        //spécifié dans l'URL
                        if ($ctrl.preselectedCiId !== null) {
                            CIService.GetById({ ciId: $ctrl.preselectedCiId }).$promise.then(function (response) {
                                var preSelectedCi = response;
                                if (preSelectedCi !== null) {
                                    handleLookupSelection('CI', preSelectedCi);
                                    $ctrl.filter.CI = preSelectedCi;

                                    if ($ctrl.featureFlagActive) {
                                        // On regarde si des éléments sont déjà pré-choisis / spécifiés dans l'URL
                                        assignValuesSinceRedirect();
                                    }
                                }
                            });
                        }
                        return $ctrl.filter;
                    });
            }
        }

        function assignValuesSinceRedirect() {
            $ctrl.filter.PeriodeDebut = $ctrl.preselectedPeriodeDebut;
            $ctrl.filter.PeriodeFin = $ctrl.preselectedPeriodeFin;
            $ctrl.launchSearchWithFilterSelectionSinceRedirect = true;

            switch ($ctrl.preselectedCodeFamille) {
                case "ACH":
                    handleFilterSelection('reception');
                    handleFilterSelection('facturation');
                    handleFilterSelection('far');
                    handleFilterSelection('typeOdAch');
                    break;
                case "MO":
                    handleFilterSelection('valorisation');
                    handleFilterSelection('moInt');
                    handleFilterSelection('typeOdMo');
                    break;
                case "MIT":
                    handleFilterSelection('valorisation');
                    handleFilterSelection('materielInt');
                    handleFilterSelection('typeOdMit');
                    break;
                default:
                    $ctrl.launchSearchWithFilterSelectionSinceRedirect = false;
                    break;
            }

            // Si on est dans le cadre d'une redirection (Rappro Compta Gestion vers Explo de dépenses)
            if ($ctrl.launchSearchWithFilterSelectionSinceRedirect) {
                handleSearch();
            }
        }

        /**
         * Récupération de l'arbre d'exploration selon un filtre
         * @param {any} filter filtre choisi
         * @returns {any} promise
         */
        function actionGetTree(filter) {
            sessionStorage.setItem('explorateurDepenseFilter', JSON.stringify({ filterValue: filter, filterLabel: $ctrl.filters }));

            // Chargement de l'arborescence d'exploration
            return DepenseService.GetExplorateurDepenseTree(filter)
                .then(ExplorateurDepenseHelperService.actionOnNoData)
                .then(ExplorateurDepenseHelperService.actionFormattingData)
                .then(function (data) {
                    $ctrl.data = data.data;
                    $ctrl.depenses = [];
                    $ctrl.paging.page = 1;
                    $ctrl.selectedNode = null;
                    $ctrl.allChecked = false;
                    return $ctrl.data;
                })
                .catch(function (error) { console.log(error); });
        }

        /**
         * Calcul du montant HT total
         * @param {any} data liste d'axe d'exploration
         */
        function actionCalculTotalMontantHT(data) {
            $ctrl.totalMontantHT = 0;
            angular.forEach(data, function (val) { $ctrl.totalMontantHT += val.MontantHT; });
        }

        /**
         * Charge de manière différé les données supplémentaires
         */
        function delayedLoadMoreIfRequired() {
            $timeout(function () {
                if ($ctrl.paging.hasMorePage && !FredToolBox.hasVerticalScrollbarVisible("#explorateur-depense-list")) {
                    actionLoadMore();
                }
            });
        }

        /**
         * Action Chargement supplémentaire
         */
        function actionLoadMore() {
            if (!$ctrl.busy && $ctrl.paging.hasMorePage) {
                $ctrl.paging.page++;
                $q.when()
                    .then(ProgressBar.start)
                    .then(function () { return actionLoadDepenses($ctrl.filter, false); })
                    .then(ProgressBar.complete)
                    .then(delayedLoadMoreIfRequired);
            }
        }

        /**
         * Action ajout ou suppression de l'axe dans une liste          
         * @param {any} tuple Tuple axe1 et axe2
         */
        function actionAddOrDelete(tuple) {
            var found = $filter('filter')($ctrl.filter.Axes, { Id: tuple.Id }, true)[0];

            if (!found) {
                $ctrl.filter.Axes.push(tuple);
            }
            else {
                if (!$ctrl.allChecked) {
                    $ctrl.filter.Axes.splice($ctrl.filter.Axes.indexOf(found), 1);
                }
            }
        }

        /**
         * Unpick les axes enfants
         * @param {any} level niveau courant
         */
        function actionUnpickAxeEnfants(level) {
            if (level.SousExplorateurAxe && level.SousExplorateurAxe.length > 0) {

                angular.forEach(level.SousExplorateurAxe, function (val, key) {
                    val.Picked = false;
                    $ctrl.children.push(val);
                    actionUnpickAxeEnfants(val);
                });
            }
        }

        function initWithCiId(ciId) {

            ciId = parseInt(ciId);

            if (!isNaN(ciId)) {
                $ctrl.preselectedCiId = ciId;
            }
        }

        function initSearchFilterRedirect(dateDebut, dateFin, codeFamille) {
            if ($ctrl.featureFlagActive) {
                if (dateFin !== "" && codeFamille !== "") {
                    // Si dateFin et codeFamille sont renseignés alors nous sommes dans le cadre de la redirection
                    if (dateDebut !== null && dateDebut !== "") {
                        $ctrl.preselectedPeriodeDebut = dateDebut;
                    }
                    else {
                        $ctrl.preselectedPeriodeDebut = null;
                        $ctrl.cumul = true;
                    }

                    $ctrl.preselectedPeriodeFin = dateFin;
                    $ctrl.preselectedCodeFamille = codeFamille;
                }
            }
        }

        function initTypeOdFilter() {
            if ($ctrl.featureFlagActive) {
                return FamilleOperationDiverseService.GetOdTypesByCompanyId($ctrl.userCompanyId)
                    .then(function (response) {
                        $ctrl.typeOdFilter = response;
                        angular.forEach(response.data, function (val) {
                            associateLibelleWithTypeOdFilter(val);
                        });
                    })
                    .catch(function () {
                        Notify.error($ctrl.resources.Global_Notification_Error);
                    });
            }
        }

        function getIsFilteredAndRedBubbleFiltered() {

           

            $ctrl.isRedBubbleFiltered = false;
            $ctrl.isFiltered = $filter('filter')($ctrl.filters, { value: { active: true } }, true).length > 0;
            var isFilteredWithoutTypeAndSousType = $filter('filter')($ctrl.filters.filter(filter => filter.id !== 'reception' && filter.id !== 'far' && filter.id !== 'facturation' && filter.id !== 'valorisation' && filter.id !== 'od' &&
                filter.id !== 'ecart' && filter.id !== 'nonCommandee' && filter.id !== 'avoir'), { value: { active: true } }, true).length > 0;

            if ($ctrl.featureFlagActive) {
                isFilteredWithoutTypeAndSousType = $filter('filter')($ctrl.filters.filter(filter => filter.id !== 'reception' && filter.id !== 'far' && filter.id !== 'facturation' && filter.id !== 'valorisation' &&
                    filter.id !== 'ecart' && filter.id !== 'nonCommandee' && filter.id !== 'avoir'), { value: { active: true } }, true).length > 0;
            }

            if (!isFilteredWithoutTypeAndSousType) {
                var isFilteredWithoutType = $filter('filter')($ctrl.filters.filter(filter => filter.id !== 'reception' && filter.id !== 'far' && filter.id !== 'facturation' && filter.id !== 'valorisation' && filter.id !== 'od'), { value: { active: true } }, true).length > 0;
                var isFilteredWithoutSousType = $filter('filter')($ctrl.filters.filter(filter => filter.id !== 'ecart' && filter.id !== 'nonCommandee' && filter.id !== 'avoir'), { value: { active: true } }, true).length > 0;
                var isFilteredAllType = $filter('filter')($ctrl.filters.filter(filter => filter.id === 'reception' || filter.id === 'far' || filter.id === 'facturation' || filter.id === 'valorisation' || filter.id === 'od'), { value: { active: true } }, true).length === 5;
                var isFilteredAllSousType = $filter('filter')($ctrl.filters.filter(filter => filter.id === 'ecart' || filter.id === 'nonCommandee' || filter.id === 'avoir'), { value: { active: true } }, true).length === 3;

                if ($ctrl.featureFlagActive) {
                    isFilteredWithoutType = $filter('filter')($ctrl.filters.filter(filter => filter.id !== 'reception' && filter.id !== 'far' && filter.id !== 'facturation' && filter.id !== 'valorisation'), { value: { active: true } }, true).length > 0;
                    isFilteredAllType = $filter('filter')($ctrl.filters.filter(filter => filter.id === 'reception' || filter.id === 'far' || filter.id === 'facturation' || filter.id === 'valorisation'), { value: { active: true } }, true).length === 4;
                }

                if ($ctrl.isFiltered && !isFilteredWithoutType) {
                    if (!isFilteredAllType) {
                        $ctrl.isRedBubbleFiltered = true;
                    }
                } else if ($ctrl.isFiltered && !isFilteredWithoutSousType) {
                    if (!isFilteredAllSousType) {
                        $ctrl.isRedBubbleFiltered = true;
                    }
                } else if ($ctrl.isFiltered && isFilteredWithoutType && isFilteredWithoutSousType) {
                    if (isFilteredAllType && !isFilteredAllSousType || !isFilteredAllType && isFilteredAllSousType || !isFilteredAllType && !isFilteredAllSousType) {
                        $ctrl.isRedBubbleFiltered = true;
                    }
                }
            } else {
                $ctrl.isRedBubbleFiltered = true;
            }
        }

        /**
         * @description Enregistrement des filtres actuels en tant que favori
         * */
        function addFilter2Favoris() {
            var filterToSave = $ctrl.filter;
            var url = $window.location.pathname;
            if ($ctrl.favoriId !== 0) {
                url = $window.location.pathname.slice(0, $window.location.pathname.lastIndexOf("/"));
            }
            favorisService.initializeAndOpenModal("ExplorateurDepense", url, filterToSave);
        }

        /**
         * Récupère les filtres via le favori. Sinon récupère les filtres en cache ou en base de données
         * @returns {any} Filtres
         */
        function getFilterOrFavoris() {
            if ($ctrl.favoriId !== 0) {
                return favorisService.getFilterByFavoriId($ctrl.favoriId)
                    .then(function (response) {
                        $ctrl.filter = response;
                        return $ctrl.filter;
                    })
                    .then(function (filter) {
                        activateFilter(filter);
                        handleSearch();
                    })
                    .catch(function () {
                        Notify.error($ctrl.resources.Global_Notification_Error);
                    });
            }
            else {
                actionGetNewFilter();
            }
        }

        /**
         * Active l'affichage des filtres
         * @param {any} filterLoaded Filtres chargés
         */
        function activateFilter(filterLoaded) {

            if (filterLoaded.AxePrincipal.indexOf('T1') === -1) { $ctrl.typeAxes.T1.selected = false; $ctrl.filterActiveOnAxe = true; }
            if (filterLoaded.AxePrincipal.indexOf('T2') === -1) { $ctrl.typeAxes.T2.selected = false; $ctrl.filterActiveOnAxe = true; }
            if (filterLoaded.AxePrincipal.indexOf('T3') === -1) { $ctrl.typeAxes.T3.selected = false; $ctrl.filterActiveOnAxe = true; }

            if (filterLoaded.AxeSecondaire.indexOf('Chapitre') === -1) { $ctrl.typeAxes.Chapitre.selected = false; $ctrl.filterActiveOnAxe = true; }
            if (filterLoaded.AxeSecondaire.indexOf('SousChapitre') === -1) { $ctrl.typeAxes.SousChapitre.selected = false; $ctrl.filterActiveOnAxe = true; }
            if (filterLoaded.AxeSecondaire.indexOf('Ressource') === -1) { $ctrl.typeAxes.Ressource.selected = false; $ctrl.filterActiveOnAxe = true; }

            $ctrl.filters[0].value.active = filterLoaded.TakeReception;
            $ctrl.filters[1].value.active = filterLoaded.TakeFar;
            $ctrl.filters[2].value.active = filterLoaded.TakeFacturation;
            $ctrl.filters[3].value.active = filterLoaded.TakeValorisation;

            if (!$ctrl.featureFlagActive) {
                $ctrl.filters[4].value.active = filterLoaded.TakeOd;
            }

            $ctrl.filters[5].value.active = filterLoaded.TakeEcart;
            $ctrl.filters[6].value.active = filterLoaded.TakeNonCommandee;
            $ctrl.filters[7].value.active = filterLoaded.TakeAvoir;
            filterLoaded.Fournisseur !== null ? ($ctrl.filters[15].value.active = true, handleLookupSelection('Fournisseur', filterLoaded.Fournisseur)) : false;
            filterLoaded.Fournisseur !== null ? ($ctrl.filters[16].value.active = true, handleLookupSelection('Ressource', filterLoaded.Ressource)) : false;
            filterLoaded.Fournisseur !== null ? ($ctrl.filters[17].value.active = true, handleLookupSelection('Tache', filterLoaded.Tache)) : false;
            $ctrl.filters[18].value.active = filterLoaded.TakeMOInt;
            $ctrl.filters[19].value.active = filterLoaded.TakeMOInterim;
            $ctrl.filters[20].value.active = filterLoaded.TakeMaterielInt;
            $ctrl.filters[21].value.active = filterLoaded.TakeMaterielExt;
            $ctrl.filters[23].value.active = filterLoaded.PersonnelInInsertion;
            $ctrl.filters[24].value.active = filterLoaded.EnergieOnly;

            if ($ctrl.featureFlagActive) {
                $ctrl.filters[25].value.active = filterLoaded.TakeOdRct;
                $ctrl.filters[26].value.active = filterLoaded.TakeOdMo;
                $ctrl.filters[27].value.active = filterLoaded.TakeOdAch;
                $ctrl.filters[28].value.active = filterLoaded.TakeOdMit;
                $ctrl.filters[29].value.active = filterLoaded.TakeOdMi;
                $ctrl.filters[30].value.active = filterLoaded.TakeOdOth;
                $ctrl.filters[31].value.active = filterLoaded.TakeOdFg;
                $ctrl.filters[32].value.active = filterLoaded.TakeOdOthd;
                $ctrl.filters[33].value.active = true, handleLookupSelection('TypeRessource', filterLoaded.TypeRessource);
            }

            $ctrl.handleFilterSelection('dateDepenseDebut');
            $ctrl.handleFilterSelection('dateDepenseFin');
            $ctrl.handleFilterSelection('dateFactureDebut');
            $ctrl.handleFilterSelection('dateFactureFin');
            $ctrl.handleFilterSelection('dateRapprochement');
            $ctrl.handleFilterSelection('montantHTDebut');
            $ctrl.handleFilterSelection('montantHTFin');
            $ctrl.handleFilterSelection('EnergieOnly');
        }

        /**
         * Sauvegarde les filtres acutels
         */
        function saveFilter() {
            $ctrl.oldFilter = Object.assign({}, $ctrl.filter);
            $ctrl.oldFilters = JSON.parse(JSON.stringify($ctrl.filters));
        }

        /**
         * Annule les filtres
         */
        function cancelFilter() {
            $ctrl.filter = Object.assign({}, $ctrl.oldFilter);
            $ctrl.filters = JSON.parse(JSON.stringify($ctrl.oldFilters));
        }

        /**
         * Modifie les dates avec la date début de mois et date fin de mois
         * */
        function changePeriodeFilter() {
            // Bug 8596 :BUG_US_680 Problème de cumul : si on est en cumul on laisse la date de début vide
            if (!$ctrl.cumul) {
                $ctrl.filter.PeriodeDebut = moment($ctrl.filter.PeriodeDebut).startOf('month').format('YYYY-MM-DD');
            }

            $ctrl.filter.PeriodeFin = moment($ctrl.filter.PeriodeFin).endOf('month').format('YYYY-MM-DD 23:59:59');
        }
    }
})(angular);