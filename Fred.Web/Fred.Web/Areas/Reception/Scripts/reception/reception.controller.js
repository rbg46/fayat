(function (angular) {
    'use strict';

    angular.module('Fred').controller('ReceptionController', ReceptionController);

    ReceptionController.$inject = ['$scope',
        '$filter',
        '$q',
        '$uibModal',
        'ReceptionService',
        'ProgressBar',
        'Notify',
        'confirmDialog',
        'FeatureFlags',
        '$timeout',
        'favorisService',
        '$window',
        'PieceJointeService',
        'CommandeLigneLockService',
        'ActionButtonEnableService',
        'AttachementManagerService',
        'MontantQuantitePourcentageCalculatorService',
        'CommandeFormatorService',
        'CommandeCommandeLigneSelectorService',
        'ModelStateErrorManager',
        'FilterService',
        'GroupeFeatureService'];

    function ReceptionController($scope,
        $filter,
        $q,
        $uibModal,
        ReceptionService,
        ProgressBar,
        Notify,
        confirmDialog,
        FeatureFlags,
        $timeout,
        favorisService,
        $window,
        PieceJointeService,
        CommandeLigneLockService,
        ActionButtonEnableService,
        AttachementManagerService,
        MontantQuantitePourcentageCalculatorService,
        CommandeFormatorService,
        CommandeCommandeLigneSelectorService,
        ModelStateErrorManager,
        FilterService,
        GroupeFeatureService) {

        var $ctrl = this;

        init();

        /*
         * Initialisation du controller.
         */
        function init() {

            // Instanciation Objet Ressources
            $ctrl.resources = resources;
            $ctrl.totalCount = 0;
            // Initialisation des données                           
            $ctrl.viewRecherche = "";
            $ctrl.favoriId = 0;
            $ctrl.resssourcesRecommandeesOnly = 0;
            $ctrl.filter = {};
            $ctrl.data = [];
            $ctrl.today = moment();
            $ctrl.busy = false;
            $ctrl.paging = { pageSize: 20, page: 1, hasMorePage: true };
            $ctrl.externLoad = false;
            $ctrl.oldFilter = {};

            FredToolBox.bindScrollEnd('#reception-list', actionLoadMore2);

            $ctrl.typeCodes = { fourniture: 'F', prestation: 'P', location: 'L' };
            $ctrl.selectedDisplay = "TB_LIGNE_";
            $ctrl.selectedDisplayId = 2;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////// CHARGEMENT /////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        $ctrl.handleLoadPage = function handleLoadPage(id, commandeSoldee, favoriId) {
            $ctrl.favoriId = parseInt(favoriId) || 0;
            var promise = null;
            promise = getFilterOrFavoris();
            promise
                .then(function () {
                    if (id && commandeSoldee) {
                        $ctrl.viewRecherche = id;
                        $ctrl.filter.ValueText = id;
                        $ctrl.filter.IsSoldee = commandeSoldee;
                        $ctrl.externLoad = true;
                    }

                    return true;
                })
                .then(GroupeFeatureService.loadGroupeFeatures)
                .then(CommandeLigneLockService.initializeIfUserCanShowLockUnLockButton)
                .then(actionReload);
        };

        /**  
         * @description Récupère le favori. Sinon, récupère les filtres par défaut
         * @returns {any} Filtres
         */
        function getFilterOrFavoris() {
            if ($ctrl.favoriId > 0) {
                return favorisService.getFilterByFavoriId($ctrl.favoriId)
                    .then(function (response) {
                        $ctrl.filter = response;
                        $ctrl.viewRecherche = $ctrl.filter.ValueText;
                    })
                    .catch(function () {
                        Notify.error($ctrl.resources.Global_Notification_Error);
                    });
            }
            else {
                if (sessionStorage.getItem('receptionListFilter') !== null) {
                    return Promise.resolve().then(function () {
                        $ctrl.filter = JSON.parse(sessionStorage.getItem('receptionListFilter')).FilterValue;
                        $ctrl.viewRecherche = JSON.parse(sessionStorage.getItem('receptionListFilter')).FilterValue.ValueText;
                    });
                }
                else {
                    return Promise.resolve().then(function () {
                        $ctrl.filter = FilterService.GetInitialFilter();
                    });
                }
            }
        }

        /*
      * @description Rafraichissement des données
      */
        $ctrl.handleReload = function handleReload() {
            $ctrl.externLoad = false;
            $ctrl.filter.ValueText = $ctrl.viewRecherche;
            actionReload(true);
        };

        /* 
       * @function    actionLoadMore()
       * @description Action Chargement de données supplémentaires (scroll end)
       */
        $ctrl.actionLoadMore = function actionLoadMore() {
            actionLoadMore2();
        };

        function actionLoadMore2() {
            if (!$ctrl.busy && !$ctrl.paging.hasMorePage) {
                $ctrl.paging.page++;
                actionReload(false);
            }
        }

        /**
         * Fonction de rafraichissement des données
         * @param {any} firstLoad boolean         
         * @returns {any} promise
         */
        function actionReload(firstLoad) {
            sessionStorage.setItem('receptionListFilter', JSON.stringify({ FilterValue: $ctrl.filter, FilterLabel: $ctrl.filters }));
            if (!$ctrl.busy) {
                ProgressBar.start();

                $ctrl.busy = true;

                if (firstLoad) {
                    $ctrl.data = [];
                    $ctrl.paging.page = 1;
                    CommandeCommandeLigneSelectorService.clearSelectedLignes();
                }

                $ctrl.totalCount = 0;

                return ReceptionService.GetCommandesToReceive($ctrl.filter, $ctrl.paging.page, $ctrl.paging.pageSize)
                    .then(actionBuildData)
                    .catch(Notify.defaultError)
                    .finally(function () {
                        $ctrl.busy = false;
                        ProgressBar.complete();
                    });
            }
        }

        /*
         * @description Construction de la datasource du tree view
         */
        function actionBuildData(response) {

            $ctrl.paging.hasMorePage = response.data.Commandes.length !== $ctrl.paging.pageSize;

            $ctrl.totalCount = response.data.TotalCount;

            // Affecte le numéro de commande externe au champ de recherche, le cas échéant
            if ($ctrl.externLoad && $ctrl.totalCount === 1) {
                var commande = response.data.Commandes[0];
                if (commande.NumeroCommandeExterne !== null && commande.NumeroCommandeExterne !== '') {
                    $ctrl.viewRecherche = commande.NumeroCommandeExterne;
                }
            }

            angular.forEach(response.data.Commandes, function (cmd) {
                CommandeFormatorService.formatCommandeAndChildren(cmd, $ctrl.filter.OnlyCommandeWithAtLeastOneCommandeLigneLocked);
                $ctrl.data.push(cmd);
            });

            $timeout(function () { $ctrl.handleDisplay($ctrl.selectedDisplay, false); }, 0);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////   GESTION DES FILTRES /////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /**
         * Gestion de la sélection de filtre
         * @param {any} type type de filtre
         * @param {any} forceDelete suppression forcée
         */
        $ctrl.handleFilterSelection = function handleFilterSelection(type, forceDelete) {
            switch (type) {
                case 'fourniture':
                    actionAddOrDeleteType($ctrl.typeCodes.fourniture);
                    break;
                case 'prestation':
                    actionAddOrDeleteType($ctrl.typeCodes.prestation);
                    break;
                case 'location':
                    actionAddOrDeleteType($ctrl.typeCodes.location);
                    break;
                case 'onlyAbonnement':
                    $ctrl.filter.IsMaterielAPointer = false;
                    break;
                case 'onlyMaterielAPointer':
                    $ctrl.filter.IsAbonnement = false;
                    break;
                case 'OnlyCommandeWithAtLeastOneCommandeLigneLocked':
                    if ($ctrl.filter.OnlyCommandeWithAtLeastOneCommandeLigneLocked) {
                        $ctrl.filter.IsSoldee = true;
                    }
                    break;
            }
        };

        /**
         * Gestion de la sélection des items des lookup
         * @param {any} type Type de lookup
         * @param {any} item objet sélectionné
         * @param {any} reload reload
         */
        $ctrl.handleLookupSelection = function handleLookupSelection(type, item, reload) {

            switch (type) {
                case "CI":
                    $ctrl.filter.CiId = item.CiId;
                    cleanDependance();
                    if (reload) {
                        $ctrl.handleReload();
                    }
                    setUrlRessourcesRecommandeesEnabled();
                    break;
                case "Ressource":
                    $ctrl.filter.RessourceId = item.RessourceId;
                    break;
                case "Tache":
                    $ctrl.filter.TacheId = item.TacheId;
                    break;
                case "Fournisseur":
                    $ctrl.filter.FournisseurId = item.FournisseurId;
                    $ctrl.filter.Fournisseur = item;
                    if (reload) {
                        $ctrl.handleReload();
                    }
                    break;
                case "Agence":
                    $ctrl.filter.AgenceId = item.AgenceId;
                    $ctrl.filter.Agence = item;
                    if (reload) {
                        $ctrl.handleReload();
                    }
                    break;
                case "AuteurCreation":
                    $ctrl.filter.AuteurCreationId = item.PersonnelId;
                    break;
                default:
                    break;
            }

        };

        /**
         * Gestion de la suppression des items des lookup
         * @param {any} type Type de lookup
         * @param {any} reload reload
         */
        $ctrl.handleLookupDeletion = function handleLookupDeletion(type, reload) {
            //cleanTuiles(type);

            switch (type) {
                case "CI":
                    $ctrl.filter.CI = null;
                    $ctrl.filter.CiId = null;
                    cleanDependance();
                    if (reload) {
                        $ctrl.handleReload();
                    }
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
                    if (reload) {
                        $ctrl.handleReload();
                    }
                    break;
                case "Agence":
                    $ctrl.filter.Agence = null;
                    $ctrl.filter.AgenceId = null;
                    if (reload) {
                        $ctrl.handleReload();
                    }
                    break;
                case "AuteurCreation":
                    $ctrl.filter.AuteurCreation = null;
                    $ctrl.filter.AuteurCreationId = null;
                    break;
                default:
                    break;
            }

        };

        function cleanDependance() {
            $ctrl.filter.RessourceId = null;
            $ctrl.filter.Ressource = null;
            $ctrl.filter.TacheId = null;
            $ctrl.filter.Tache = null;
        }

        /**
        * Ajout ou suppression d'un type de commande dans la liste
        * @param {any} type type de commande
        */
        function actionAddOrDeleteType(type) {
            var i = $ctrl.filter.TypeCodes.indexOf(type);
            if (i > -1) {
                $ctrl.filter.TypeCodes.splice(i, 1);
            }
            else {
                $ctrl.filter.TypeCodes.push(type);
            }
        }

        /**
       * @function handleSaveFilter
       * @description Sauvegarde localement les filtres actuels
       */
        $ctrl.handleSaveFilter = function handleSaveFilter() {
            saveFilter();
        };

        /**
         * @function handleCancelFilter
         * @description Annule les changements effectués sur les filtres
         */
        $ctrl.handleCancelFilter = function handleCancelFilter() {
            cancelFilter();
        };

        function saveFilter() {
            $ctrl.oldFilter = Object.assign({}, $ctrl.filter);
        }

        function cancelFilter() {
            $ctrl.filter = Object.assign({}, $ctrl.oldFilter);
        }

        $ctrl.hasFilterActive = function () {
            return FilterService.getIfOneFilterIsSelected($ctrl.filter);
        };

        /*
        * @description Remise à zéro des filtres de recherche
        */
        $ctrl.handleResetFilter = function handleResetFilter() {
            $ctrl.filter = FilterService.GetInitialFilter();
            $ctrl.filter.IsSoldee = false;
            $ctrl.filter.IsAbonnement = false;
            $ctrl.viewRecherche = '';
            actionReload(true);
        };

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////     AJOUT  RECEPTION          ////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        $ctrl.handleAddReceptionAsync = async function handleAddReceptionAsync(commande, commandeLigne) {

            if ($ctrl.busy) {
                return;
            }

            $ctrl.busy = true;

            if (!commandeLigne.CommandeLigneId) {
                //si je n'ai pas de commande ligne je sort. j'en ai besoin pour faire l'appel SUIVANT.
                onFinally();
                return;
            }

            //Recuperation d'une reception générer par le back
            try {
                var newReceptionGenerated = await ReceptionService.New(commandeLigne.CommandeLigneId);

                newReceptionGenerated.Date = $filter('toLocaleDate')(newReceptionGenerated.Date);

            } catch (e) {
                Notify.defaultError();
                onFinally();  //si il y a eu une erreur je sort.
                return;
            }

            ProgressBar.complete();

            var modalInstance = $uibModal.open({
                animation: true,
                component: 'receptionModalComponent',
                resolve: {
                    mode: function () { return "ADD"; },
                    commandeLigne: function () { return commandeLigne; },
                    commande: function () { return commande; },
                    reception: function () { return newReceptionGenerated; },
                    resources: function () { return $ctrl.resources; },
                    handleShowLookup: function () {
                        return $ctrl.handleShowLookup;
                    }
                }
            });

            try {
                await modalInstance.result;
            }
            catch (e) {
                onFinally();
                return;
            }

            MontantQuantitePourcentageCalculatorService.actionUpdateFigures($ctrl.data);

            onFinally();
        };


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////     MODIFICATION  RECEPTION          /////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        $ctrl.handleUpdateReceptionAsync = async function handleUpdateReceptionAsync(commande, commandeLigne, reception) {

            var recep = angular.copy(reception);

            if ($ctrl.busy) {
                return;
            }

            $ctrl.busy = true;

            var modalInstance = $uibModal.open({
                animation: true,
                component: 'receptionModalComponent',
                resolve: {
                    mode: function () { return "UPDATE"; },
                    commandeLigne: function () { return commandeLigne; },
                    commande: function () { return commande; },
                    reception: function () { return recep; },
                    resources: function () { return $ctrl.resources; },
                    handleShowLookup: function () {
                        return $ctrl.handleShowLookup;
                    }
                }
            });

            try {
                var popupResult = await modalInstance.result;
            } catch (e) {
                onFinally();
                return;
            }

            MontantQuantitePourcentageCalculatorService.actionUpdateFigures($ctrl.data);

            onFinally();

        };



        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////   DUPLICATION RECEPTION  //////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        $ctrl.handleDuplicateReceptionAsync = async function handleDuplicateReceptionAsync(cmd, commandeLigne, reception) {

            if ($ctrl.busy) {
                return;
            }
            ProgressBar.start();
            $ctrl.busy = true;

            if (!commandeLigne.CommandeLigneId && reception.DepenseId) {
                onFinally();
                return;
            }

            try {
                var duplicateReceptionResult = await ReceptionService.Duplicate(commandeLigne.CommandeLigneId, reception.DepenseId);
                var duplicateReception = duplicateReceptionResult.data;
            } catch (e) {
                Notify.defaultError();
                onFinally();
                return;
            }

            ProgressBar.complete();

            var modalInstance = $uibModal.open({
                animation: true,
                component: 'receptionModalComponent',
                resolve: {
                    mode: function () { return "ADD"; },
                    commandeLigne: function () { return commandeLigne; },
                    commande: function () { return cmd; },
                    reception: function () { return duplicateReception; },
                    resources: function () { return $ctrl.resources; },
                    handleShowLookup: function () {
                        return $ctrl.handleShowLookup;
                    }
                }
            });

            try {
                var popupResult = await modalInstance.result;
            } catch (e) {
                onFinally();
                return;
            }

            MontantQuantitePourcentageCalculatorService.actionUpdateFigures($ctrl.data);

            onFinally();
        };

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////  SUPRESSION D UNE RECEPTION   ////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /*
         * @description Fonction de suppression d'une réception
         */
        $ctrl.handleDeleteReception = async function handleDeleteReception(cmdLigne, reception, index) {
            if (!$ctrl.busy) {

                $ctrl.busy = true;

                try {
                    await confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationSuppression);
                } catch (e) {
                    onFinally();
                    return;
                }

                try {
                    ProgressBar.start();

                    await ReceptionService.Delete(reception.DepenseId);

                    actionDeleteReceptionFront({
                        CommandeLigne: cmdLigne,
                        Index: index
                    });

                    await CommandeLigneLockService.updateVerrouOnCommandeLigne(cmdLigne);

                    MontantQuantitePourcentageCalculatorService.actionUpdateFigures($ctrl.data);

                } catch (err) {
                    onDeleteError(err, reception);
                } finally {
                    onFinally();
                }

            }

        };

        function onDeleteError(error, reception) {
            var oneReceptionIsBlockedError = ModelStateErrorManager.getError(error, "Receptions");

            var errorName = 'Quantite_' + reception.DepenseId;
            var receptionQuantiteNegativeError = ModelStateErrorManager.getError(error, errorName);

            if (oneReceptionIsBlockedError.hasThisError) {
                Notify.error(oneReceptionIsBlockedError.firstError);
            }
            else if (receptionQuantiteNegativeError.hasThisError) {
                Notify.error(receptionQuantiteNegativeError.firstError);
            }
            else {
                Notify.defaultError();
            }
        }

        /**
        * Suppression de la réception
        * @param {any} data data contenant la ligne et la réception courante
        */
        function actionDeleteReceptionFront(data) {
            if (data.Index > -1) {
                data.CommandeLigne.DepensesReception.splice(data.Index, 1);
                Notify.message($ctrl.resources.Global_Notification_Suppression_Success);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////     EXTRACT EXCEL   ////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /*
         * @description Handler de click sur le bouton Excel
         */
        $ctrl.handleExtractExcel = function handleExtractExcel() {
            if (!$ctrl.busy) {

                $ctrl.busy = true;
                ReceptionService.GenerateExcel($ctrl.filter)
                    .then(function (response) { window.location.href = '/api/ExportExcelPdf/RetrieveExcel/ListeReceptions/' + response.data.id; })
                    .catch(Notify.defaultError)
                    .finally(function () { $ctrl.busy = false; });
            }
        };

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /*
         * @description Gestion de la modal Commande Buyer
         */
        $ctrl.handleOpenCommandeBuyerModal = function handleOpenCommandeBuyerModal() {
            if (!$ctrl.cmdBuyerModalOpen && !$ctrl.busy) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    component: 'commandeBuyerComponent',
                    resolve: {
                        resources: function () { return $ctrl.resources; },
                        today: function () { return $ctrl.today; },
                        handleShowLookup: function () { return $ctrl.handleShowLookup; }
                    }
                });

                modalInstance.opened.then(function () {
                    $ctrl.cmdBuyerModalOpen = true;
                });

                // we want to update state whether the modal closed or was dismissed,
                // so use finally to handle both resolved and rejected promises.
                modalInstance.result.finally(function (selectedItem) {
                    $ctrl.cmdBuyerModalOpen = false;
                });
            }
        };

        /*
         * @description Gestion de l'URL pour la Lookup
         */
        $ctrl.handleShowLookup = function handleShowLookup(val, reception) {
            var url = '/api/' + val + '/SearchLight/?page=1&pageSize=20&societeId={0}&ciId={1}';
            switch (val) {
                case "Tache":
                    url = String.format(url, null, reception.CiId);
                    break;
                case "EtablissementComptable":
                    url = String.format(url, null, null);
                    break;
                case "CI":
                    url = String.format(url, null, null);
                    break;
                case "Ressource":
                    url = String.format(url, $ctrl.filter.CI.SocieteId, null);
                    break;
                case "Fournisseur":
                    url = String.format(url, null, null);
                    break;
                default:
                    url = '/api/' + val + '/SearchLight/';
                    break;
            }
            return url;
        };

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////// GESTIONS DE L AFFICHAGE VISUEL /////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /**
         * Gestion des différents affichage de ligne
         * @param {any} obj identifiant
         * @param {any} byClass byClass
         */
        $ctrl.handleDisplay = function handleDisplay(obj, byClass) {
            if (!obj) {
                actionDisplay('TB_', 'show', byClass);
                $ctrl.selectedDisplayId = 3;
                $ctrl.selectedDisplay = null;
            }
            else if (obj === 'TB_LIGNE_') {
                actionDisplay('TB_COMMANDE_', 'show', byClass);
                actionDisplay(obj, 'hide', byClass);
                $ctrl.selectedDisplayId = 2;
                $ctrl.selectedDisplay = obj;
            }
            else {
                actionDisplay(obj, 'hide', byClass);
                $ctrl.selectedDisplayId = 1;
                $ctrl.selectedDisplay = obj;
            }
        };

        $ctrl.handleSelectDisplay = function handleSelectDisplay(code) {
            $ctrl.selectedDisplay = code;
        };


        /**
         * Gestion affichage des lignes
         * @param {any} id identifiants tableau
         * @param {any} action action exécutée (show, hide)
         * @param {any} byClass action JS en utilisant les class CSS ou les fonctions JS
         */
        function actionDisplay(id, action, byClass) {
            var e = document.querySelectorAll("[id^='" + id + "']");

            if (!byClass) {
                angular.element(e).collapse(action);
            }
            else {
                if (action === 'show') {
                    angular.element(e).addClass('in');
                }
                else {
                    angular.element(e).removeClass('in');
                }
            }
        }

        /*
         * @description Gestion du collapse et collapsed des lignes du tableau
         */
        $ctrl.handleCollapse = function handleCollapse(id) {
            var e = document.querySelector(id);
            var isExpanded = angular.element(e).attr("aria-expanded");
            var action = "";
            action = isExpanded === "true" ? "hide" : "show";
            angular.element(e).collapse(action);
        };

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////// GESTIONS DE LA PICK LIST FOURNISSEUR - AGENCE///////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /*
         * @description Action de click sur la double picklist fournisseur agence
         */
        $ctrl.handleSelectFournisseurAgence = function handleSelectFournisseurAgence(fournisseur, agence) {
            $ctrl.handleLookupSelection('Fournisseur', fournisseur);
            if (agence && !agence.IsAgencePrincipale) {
                $ctrl.handleLookupSelection('Agence', agence);
            }
        };

        /*
         * @description Action de click sur la double picklist fournisseur agence avec reload de la page
         */
        $ctrl.handleSelectFournisseurAgenceWithReload = function handleSelectFournisseurAgenceWithReload(fournisseur, agence) {
            $ctrl.handleSelectFournisseurAgence(fournisseur, agence);
            $ctrl.handleReload();
        };

        /*
        * @description Action de click pour afficher la double picklist fournisseur agence
        */
        $ctrl.handleShowPicklistFournisseurAgence = function handleShowPicklistFournisseurAgence() {
            // Event d'affichage de la dualpicklist
            $scope.$broadcast("showDualPicklist");
        };

        $ctrl.handleShowPicklistFournisseurAgenceWithReload = function handleShowPicklistFournisseurAgenceWithReload() {
            // Event d'affichage de la dualpicklist
            $scope.$broadcast("showDualPicklistWithReload");
        };

        /*
        * @description Action de suppression de fournisseur agence
        */
        $ctrl.handleClearAgenceAndFournisseur = function handleClearAgenceAndFournisseur() {
            $ctrl.handleLookupDeletion('Fournisseur');
            $ctrl.handleLookupDeletion('Agence');
        };

        /*
        * @description Action de suppression de fournisseur agence avec reload de la page
        */
        $ctrl.handleClearAgenceAndFournisseurWithReload = function handleClearAgenceAndFournisseurWithReload() {
            $ctrl.handleClearAgenceAndFournisseur();
            $ctrl.handleReload();
        };


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////// GESTIONS DES PIECES JOINTES  ////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /**
         * Gérer le téléchargment d'un fichier
         * @param {any} commandeIndex commande Id
         */
        $ctrl.handleDownloadAllAttachmentsCommande = function handleDownloadAllAttachmentsCommande(commandeIndex) {

            let commande = $ctrl.data[commandeIndex];

            // Afficher toutes les pièces jointes
            if (commande) {
                for (var i = 0; i < commande.PiecesJointesCommande.length; i++) {
                    PieceJointeService.Download(commande.PiecesJointesCommande[i].PieceJointeId);
                }
            }
        };

        /**
         * Gérer le téléchargment d'un fichier
         * @param {any} commandeIndex commande Id
         * @param {any} commandeLigneIndex index ligne Id
         *  @param {any} depenseIndex depense Id
         */
        $ctrl.handleDownloadAllAttachmentsReception = function handleDownloadAllAttachmentsReception(commandeIndex, commandeLigneIndex, depenseIndex) {

            let depense = $ctrl.data[commandeIndex].Lignes[commandeLigneIndex].DepensesReception[depenseIndex];

            // Afficher toutes les pièces jointes
            if (depense) {
                for (var i = 0; i < depense.PiecesJointesReception.length; i++) {
                    PieceJointeService.Download(depense.PiecesJointesReception[i].PieceJointeId);
                }
            }
        };


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////// SELECTION  COMMANDE COMMANDE - COMMANDE LIGNE ///////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        $ctrl.canSelectCommande = function (cmd) {
            return CommandeCommandeLigneSelectorService.canSelectCommande(cmd);
        };

        $ctrl.handleSelectCommande = function (cmd) {
            CommandeCommandeLigneSelectorService.handleSelectCommande(cmd);
        };

        $ctrl.canSelectCommandeLigne = function (commandeLigne) {
            return CommandeCommandeLigneSelectorService.canSelectCommandeLigne(commandeLigne);
        };

        $ctrl.handleSelectCommandeLigne = function (cmd, cmdLigne) {
            CommandeCommandeLigneSelectorService.handleSelectCommandeLigne(cmd, cmdLigne);
        };


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////// MODALE CREATION RECEPTION MULTIPLE ///////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /**
        * Ouverture de la modal permettant de gérer l'ajout multiple de réceptions (selon la sélection)
        */
        $ctrl.handleAddReceptionList = function handleAddReceptionList() {
            $q.when().then(actionOpenReceptionListModal);
        };

        $ctrl.canOpenReceptionListModal = function canOpenReceptionListModal() {
            var selectedCommandeLignes = CommandeCommandeLigneSelectorService.getSelecteLignes();
            return selectedCommandeLignes && selectedCommandeLignes.length !== 0;
        };

        /**
         * Modal d'ajout multiple de réceptions
         */
        function actionOpenReceptionListModal() {
            if (!$ctrl.busy) {

                var selectedCommandeLignes = CommandeCommandeLigneSelectorService.getSelecteLignes();
                var modalInstance = $uibModal.open({
                    animation: true,
                    component: 'addReceptionListComponent',
                    windowClass: 'multiple-reception-modal',
                    resolve: {
                        selectedCommandeLignes: function () { return selectedCommandeLignes; },
                        data: function () { return $ctrl.data; },
                        resources: function () { return $ctrl.resources; },
                        handleShowLookup: function () { return $ctrl.handleShowLookup; }
                    }
                });

                // On désélectionne toutes les lignes de commandes à la fermeture de la modal      
                modalInstance.closed.then(function () {
                    CommandeCommandeLigneSelectorService.actionDeselectAll($ctrl.data);
                });
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////   FAVORIS  ////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /**
        * @function handleAddFilter2Favoris
        * @description Ajout d'un favori
        */
        $ctrl.handleAddFilter2Favoris = function handleAddFilter2Favoris() {
            addFilter2Favoris();
        };

        /**
      * @description Enregistrement des filtres actuels en tant que favori
      * */
        function addFilter2Favoris() {
            var filterToSave = $ctrl.filter;
            var url = $window.location.pathname;
            if ($ctrl.favoriId !== 0) {
                url = $window.location.pathname.slice(0, $window.location.pathname.lastIndexOf("/"));
            }
            favorisService.initializeAndOpenModal("CommandeReception", url, filterToSave);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////// COMMON ////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        function onFinally() {
            $ctrl.busy = false;
            ProgressBar.complete();
        }

        function setUrlRessourcesRecommandeesEnabled() {
            $ctrl.resssourcesRecommandeesOnly = 0;
            if ($ctrl.filter.CI && $ctrl.filter.CI.EtablissementComptable && $ctrl.filter.CI.EtablissementComptable.RessourcesRecommandeesEnabled) {
                $ctrl.resssourcesRecommandeesOnly = 1;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////GESTION DES VERROUS SUR LES LIGNES DE COMMANDES :///////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        $ctrl.getIfCommandeLigneAddButtonIsDisabled = function getIfCommandeLigneAddButtonIsDisabled(commandeLigne) {
            return ActionButtonEnableService.getIfCommandeLigneAddButtonIsDisabled(commandeLigne);
        };

        $ctrl.getIfReceptionUpdateButtonsIsDisabled = function getIfReceptionUpdateButtonsIsDisabled(commandeLigne, reception) {
            return ActionButtonEnableService.getIfReceptionUpdateButtonsIsDisabled(commandeLigne, reception);
        };

        $ctrl.getIfReceptionDeleteButtonsIsDisabled = function getIfReceptionDeleteButtonsIsDisabled(commandeLigne, reception) {
            return ActionButtonEnableService.getIfReceptionDeleteButtonsIsDisabled(commandeLigne, reception);
        };

        $ctrl.getIfReceptionDuplicateButtonsIsDisabled = function getIfReceptionDuplicateButtonsIsDisabled(commandeLigne) {
            return ActionButtonEnableService.getIfReceptionDuplicateButtonsIsDisabled(commandeLigne);
        };

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////        AFFICHAGE DE LA LIGNE DE COMMANDE      //////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        $ctrl.getIfCommandeLigneIsVisible = function (commandeLigne) {
            return commandeLigne.IsVisible;
        };

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////   AFFICHAGE DU FILTRE SUR LES LIGNE VERROUILLER  //////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        $ctrl.canShowFilterOnlyCommandeLigneLocked = function () {
            return CommandeLigneLockService.canShowFilter();
        };

        return $ctrl;
    }
})(angular);
