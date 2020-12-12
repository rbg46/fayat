(function (angular) {
    'use strict';

    angular.module('Fred').controller('ReceptionTableauController', ReceptionTableauController);

    ReceptionTableauController.$inject = [
        '$filter',
        '$q',
        'ReceptionTableauService',
        'ProgressBar',
        'Notify',
        'confirmDialog',
        'PieceJointeService',
        'favorisService',
        '$window',
        'ReceptionStampButtonService',
        'ReceptionTableauDataFormatService',
        'ReceptionTableauSelectorService',
        'ReceptionTableauIsVisableService',
        'ModelStateErrorManager',
        'ReceptionTableauQuantityService',
        'UserService'
    ];


    function ReceptionTableauController(
        $filter,
        $q,
        ReceptionTableauService,
        ProgressBar,
        Notify,
        confirmDialog,
        PieceJointeService,
        favorisService,
        $window,
        ReceptionStampButtonService,
        ReceptionTableauDataFormatService,
        ReceptionTableauSelectorService,
        ReceptionTableauIsVisableService,
        ModelStateErrorManager,
        ReceptionTableauQuantityService,
        UserService
    ) {

        /* -------------------------------------------------------------------------------------------------------------
         *                                            INIT
         * -------------------------------------------------------------------------------------------------------------
         */
        var $ctrl = this;

        var periodeDebutCourante = null;


        init();

        /*
         * Initialisation du controller.
         */
        function init() {

            // Instanciation Objet Ressources
            $ctrl.resources = resources;

            // Initialisation des données                           
            $ctrl.filter = {};
            $ctrl.oldFilter = null;
            $ctrl.receptions = [];
            $ctrl.receptionResult = { Receptions: [] };
            $ctrl.cumul = false;

            $ctrl.today = moment();
            $ctrl.receptionForm = {};
            $ctrl.allSelected = false;
            $ctrl.achats = 1;

            // Affichage des colones
            $ctrl.displayColRcpt = true;
            $ctrl.displayCol = false;

            $ctrl.busy = false;
            $ctrl.paging = { pageSize: 20, page: 1, hasMorePage: true };
            $ctrl.tacheSysteme = { codeTacheEcartInterim: "999996" };

            FredToolBox.bindScrollEnd('#reception-table', actionLoadMore);

            UserService.getCurrentUser().then(function (user) {
                $ctrl.isUserRzb = user.Personnel.Societe.Groupe.Code.trim() === 'GRZB' ? true : false;
            });
        }



        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////      LOAD    /////////////////////////////////////////////////////::::::://////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        $ctrl.handleLoadPage = function (favoriId) {
            $ctrl.favoriId = parseInt(favoriId) || 0;

            $q.when()
                .then(getFilterOrFavoris());
        };

        /*
        * @function getFilterOrFavoris
        * @description Récupère le favori. Sinon, récupère les filtres par défaut
        */
        function getFilterOrFavoris() {
            if ($ctrl.favoriId !== 0) {
                return favorisService.getFilterByFavoriId($ctrl.favoriId)
                    .then(function (response) {
                        $ctrl.filter = response;
                        actionReload(true);
                        return $ctrl.filter;
                    })
                    .catch(function () {
                        Notify.error($ctrl.resources.Global_Notification_Error);
                    });
            }
            else {
                return actionGetFilter();
            }
        }

        /*
      * @description Récupération des filtres de recherche
      */
        function actionGetFilter() {
            if (sessionStorage.getItem('receptionTableFilter') !== null) {
                $ctrl.filter = JSON.parse(sessionStorage.getItem('receptionTableFilter'));
                periodeDebutCourante = $ctrl.filter.DateFrom;
                actionReload(true);
                return $ctrl.filter;
            } else {
                return ReceptionTableauService.Filter()
                    .then(function (filter) {
                        $ctrl.filter = filter.data;
                        $ctrl.filter.DateFrom = $ctrl.filter.DateFrom ? $filter('toLocaleDate')($ctrl.filter.DateFrom) : $ctrl.today;
                        $ctrl.filter.DefaultDateFrom = angular.copy($ctrl.filter.DateFrom);
                        $ctrl.filter.DateTo = $ctrl.filter.DateTo ? $filter('toLocaleDate')($ctrl.filter.DateTo) : $ctrl.today;
                        $ctrl.filter.DefaultDateTo = angular.copy($ctrl.filter.DateTo);
                        $ctrl.filter.IncludeReceptionInterimaire = $ctrl.isUserRzb;
                        periodeDebutCourante = $ctrl.filter.DateFrom;
                        actionReload(true);
                    })
                    .catch(Notify.defaultError);
            }
        }

        /*
       * @description Fonction de rafraichissement des données
       */
        function actionReload(firstLoad) {
            sessionStorage.setItem('receptionTableFilter', JSON.stringify($ctrl.filter));
            if (!$ctrl.busy) {

                actionOnBegin();

                if (firstLoad) {
                    $ctrl.allSelected = false;
                    $ctrl.receptions = [];
                    $ctrl.receptionResult.SoldeFarTotal = 0;
                    $ctrl.receptionResult.MontantHTTotal = 0;
                    $ctrl.receptionResult.AllReceptionsIds = [];
                    $ctrl.receptionResult.Count = 0;
                    $ctrl.receptionResult.AllVisableReceptionIds = [];
                    $ctrl.paging.page = 1;

                    setReceptionVisablesWarning();

                    ReceptionTableauService.SearchReceptions($ctrl.filter, $ctrl.paging.page, $ctrl.paging.pageSize)
                        .then(function (receptions) {

                            $ctrl.receptionResult.SoldeFarTotal = receptions.data.SoldeFarTotal;//html
                            $ctrl.receptionResult.MontantHTTotal = receptions.data.MontantHTTotal;//html
                            $ctrl.receptionResult.AllReceptionsIds = receptions.data.AllReceptionsIds;//sert a rechercher les suivantes
                            $ctrl.receptionResult.Count = receptions.data.Count;
                            $ctrl.receptionResult.AllVisableReceptionIds = receptions.data.AllVisableReceptionIds;//sert a comptage des visables
                            ReceptionTableauSelectorService.Initialize($ctrl.receptionResult.AllVisableReceptionIds);//init service des visables
                            actionBuildData(receptions.data.Receptions);//construit les receptions

                        })
                        .catch(Notify.defaultError)
                        .finally(actionOnFinally);
                } else {

                    var selectedIds = $ctrl.receptionResult.AllReceptionsIds.slice(($ctrl.paging.page - 1) * $ctrl.paging.pageSize, $ctrl.paging.page * $ctrl.paging.pageSize);

                    ReceptionTableauService.SearchNextReceptions($ctrl.filter, selectedIds)
                        .then(function (receptions) {
                            actionBuildData(receptions.data);
                        })
                        .catch(Notify.defaultError)
                        .finally(actionOnFinally);
                }


            }
        }

        /*
       * @description Construction de la datasource du tree view
       */
        function actionBuildData(response) {

            $ctrl.paging.hasMorePage = response.length === $ctrl.paging.pageSize;

            angular.forEach(response, function (reception) {

                ReceptionTableauDataFormatService.formatReception(reception);

                selectReceptionIfNecessarry(reception);

                $ctrl.receptions.push(reception);
            });

            setReceptionVisablesWarning();

            setUrlRessourcesRecommandeesEnabled();

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////     RE-LOAD    ////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /* 
         * @function    actionLoadMore()
         * @description Action Chargement de données supplémentaires (scroll end)
         */
        function actionLoadMore() {
            if (!$ctrl.busy && $ctrl.paging.hasMorePage) {
                $ctrl.paging.page++;
                actionReload(false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////     RESET DU FILTRE    ////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        $ctrl.handleResetFilter = function () {
            $q.when()
                .then(sessionStorage.removeItem('receptionTableFilter'))
                .then(actionGetFilter);
        };

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////      FILTRE TUILE   ////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        $ctrl.canShowTileAViser = function () {
            return $ctrl.filter.AViser;
        };

        $ctrl.onClickTileAViser = function (reload) {
            $ctrl.filter.AViser = false;
            $ctrl.filter.Visees = false;
            $ctrl.filter.Far = false;
            if (reload) {
                actionReload(true);
            }

        };

        $ctrl.canShowTileVisees = function () {
            return $ctrl.filter.Visees;
        };

        $ctrl.onClickTileVisees = function (reload) {
            $ctrl.filter.AViser = false;
            $ctrl.filter.Visees = false;
            $ctrl.filter.Far = false;
            if (reload) {
                actionReload(true);
            }
        };
        $ctrl.canShowTileFar = function () {
            return $ctrl.filter.Far;
        };

        $ctrl.onClickTilefar = function (reload) {
            $ctrl.filter.AViser = false;
            $ctrl.filter.Visees = false;
            $ctrl.filter.Far = false;
            if (reload) {
                actionReload(true);
            }
        };
        $ctrl.canShowTileAbonnement = function () {
            return $ctrl.filter.IsAbonnement;
        };

        $ctrl.onClickTileAbonnement = function (reload) {
            $ctrl.filter.IsAbonnement = false;
            if (reload) {
                actionReload(true);
            }
        };
        $ctrl.canShowTileMaterielsAPointer = function () {
            return $ctrl.filter.IsMaterielAPointer;
        };

        $ctrl.onClickTileMaterielsAPointer = function (reload) {
            $ctrl.filter.IsMaterielAPointer = false;
            if (reload) {
                actionReload(true);
            }
        };
        $ctrl.canShowTileEnergies = function () {
            return $ctrl.filter.IsEnergie;
        };

        $ctrl.onClickTileEnergies = function (reload) {
            $ctrl.filter.IsEnergie = false;
            if (reload) {
                actionReload(true);
            }
        };
        $ctrl.canShowTilePiecesJointes = function () {
            return $ctrl.filter.OnlyReceptionWithPiecesJointes;
        };

        $ctrl.onClickTilePiecesJointes = function (reload) {
            $ctrl.filter.OnlyReceptionWithPiecesJointes = false;
            if (reload) {
                actionReload(true);
            }
        };


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////    CHANGEMENT DE DATE SUR LE FILTRE/////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /**
        * Gestion du bouton Cumul
        */
        $ctrl.handleCumul = function () {
            $ctrl.cumul = !$ctrl.cumul;
            $ctrl.filter.DateFrom = $ctrl.cumul ? null : $ctrl.today;
            actionReload(true);
        };


        $ctrl.handleChangeDateDebut = function () {
            if (!periodeDebutCourante || !IsSamePeriod($ctrl.filter.DateFrom, new Date(periodeDebutCourante))) {
                periodeDebutCourante = $ctrl.filter.DateFrom;
                actionReload(true);
            }
        };

        $ctrl.handleChangeDateFin = function (date) {
            actionReload(true);
        };

        /**
        * Rechargement de la liste des réceptions
        */
        $ctrl.handleLoad = function () {
            actionReload(true);
        };

        $ctrl.setDateToEqualDateFrom = function () {
            $ctrl.filter.DateTo = $ctrl.filter.DateFrom;
            actionReload(true);
        };

        // Indique si 2 dates sont sur la même période
        // return : true si les 2 dates sont sur la même période, sinon false
        function IsSamePeriod(date1, date2) {
            if (!hasValue(date1) && hasValue(date2)) {
                return false;
            }
            if (hasValue(date1) && !hasValue(date2)) {
                return false;
            }
            return date1.getFullYear() === date2.getFullYear() && date1.getMonth() === date2.getMonth();
        }

        function hasValue(data) {
            return data !== undefined && data !== null && data !== "";
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////     GESTION AFFICHAGE BOUTTON SELECTION    /////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        /**
        * @description sert a savoir si on peut afficher le boutton pour visée une reception
        * @param {reception}  reception sur laquelle on cherche a savoir si on peut afficher le boutton pour visée
        * @returns {boolean}  true si on peut afficher le boutton pour visée une reception
        */
        $ctrl.canShowButtonStamp = function (reception) {
            return ReceptionStampButtonService.canShowButtonStamp(reception);
        };

        /**
         * @description sert a savoir le style display a applique sur la cellule
         * @param {reception}  reception sur laquelle on cherche a appliqué le style
         * @returns {string}  flex si on peut afficher,  table-cell si on peut afficher
         */
        $ctrl.getStyleDisplay = function (reception) {
            return ReceptionStampButtonService.getStyleDisplay(reception);
        };

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////      GESTION SELECTION    /////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        $ctrl.selectedChanged = function () {
            setReceptionVisablesWarning();
        };


        /**
         * Gestion de la sélection ou déselection complète des lignes de réceptions
         * @param {any} object Réception
         * @param {any} isSelectingAll Sélection ou Déselection
         */
        $ctrl.handleSelectAll = function () {
            $ctrl.allSelected = true;
            selectAllReceptions();
            setReceptionVisablesWarning();
        };

        function selectAllReceptions() {
            angular.forEach($ctrl.receptions, function (reception) {
                markReceptionAsSelected(reception);
            });
        }
        $ctrl.handleUnSelectAll = function () {
            $ctrl.allSelected = false;
            unSelectAllReceptions();
            setReceptionVisablesWarning();
        };


        function unSelectAllReceptions() {
            angular.forEach($ctrl.receptions, function (reception) {
                reception.isReceptionSelected = false;
            });
        }

        function selectReceptionIfNecessarry(reception) {
            if ($ctrl.allSelected) {
                markReceptionAsSelected(reception);
            }
        }

        function markReceptionAsSelected(reception) {
            if (ReceptionTableauIsVisableService.isVisable(reception)) {
                reception.isReceptionSelected = true;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////      MISE A JOUR    ///////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        $ctrl.markAsModified = function (reception) {
            reception.isModified = true;
        };

        $ctrl.handleCanUpdateReceptions = function () {
            return ReceptionTableauSelectorService.hasReceptionsSelecteds($ctrl.allSelected, $ctrl.receptions) && !$ctrl.busy;
        };


        ///**
        // * Mise à jour de la liste des réceptions sélectionnées
        // */
        $ctrl.handleUpdateReceptions = function () {

            var allInvalidElements = angular.element(document.querySelectorAll('.selected-row .ng-invalid'));

            if (allInvalidElements && allInvalidElements.length === 0) {

                var modifiedReceptionsIds = ReceptionTableauSelectorService.getModifiedReceptionsIds($ctrl.receptions);

                if (modifiedReceptionsIds.length > 0) {

                    actionOnBegin();

                    ReceptionTableauService.IsBlockedInReception(modifiedReceptionsIds)
                        .then(actionConfirmSave)
                        .then(actionUpdateReceptions)
                        .then(reloadRowUpdatedForRefresh)
                        .then(updateRowWithBackendData)
                        .then(UpdateReceptionsSuccess)
                        .catch(UpdateReceptionsOnError)
                        .finally(actionOnFinally);
                }
            }
            else {
                Notify.error("Erreur, veuillez saisir tous les champs obligatoires encadrés en rouge dans la liste.");
            }
        };



        /**
        * Confirmation de sauvegarde
        * => Vérifie si au moins une réception possède une date dont la période est bloquée en réception ou non
        * @param {any} isBlockedInReception est bloquée ou pas
        * @returns {any} promise
        */
        function actionConfirmSave(isBlockedInReception) {
            if (isBlockedInReception.data) {
                return confirmDialog.confirm(resources, "Certaines réceptions de la sélection sont datées sur une période déjà transférée en FAR/clôturée, et vont donc être comptabilisées sur une période plus récente. Voulez-vous continuer ? ")
                    .catch(function (error) {
                        throw new Error('stop process');
                    });
            }
        }


        /**
       * Sauvegarde des réceptions
       * @param {any} isConfirmed confirmation pop-in
       * @returns {any} promise
       */
        function actionUpdateReceptions() {
            ReceptionTableauQuantityService.cleanReceptionQuantityValidationErrors($ctrl.receptions);
            var modifiedReceptions = ReceptionTableauSelectorService.getModifiedReceptions($ctrl.receptions);
            return ReceptionTableauService.UpdateReceptions(modifiedReceptions);

        }

        function reloadRowUpdatedForRefresh() {
            var modifiedReceptionsIds = ReceptionTableauSelectorService.getModifiedReceptionsIds($ctrl.receptions);
            return ReceptionTableauService.SearchNextReceptions($ctrl.filter, modifiedReceptionsIds);
        }




        function updateRowWithBackendData(response) {
            let receptions = response.data;
            angular.forEach(receptions, function (reception, key) {

                ReceptionTableauDataFormatService.formatReception(reception);

                var receptionInArray = $filter('filter')($ctrl.receptions, { DepenseId: reception.DepenseId }, true)[0];

                var index = $ctrl.receptions.indexOf(receptionInArray);

                if (index !== -1) {
                    $ctrl.receptions[index] = reception;
                }
            });
        }

        function UpdateReceptionsSuccess() {
            actionUnselectReceptions();
            Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
        }

        function UpdateReceptionsOnError(error) {
            if (error.message === 'stop process') {
                return;
            }
            if (ReceptionTableauQuantityService.hasReceptionQuantityNegativeError(error, $ctrl.receptions)) {
                ReceptionTableauQuantityService.markReceptionQuantityValidationErrors(error, $ctrl.receptions, $ctrl.resources);
                Notify.defaultError();
                return;
            }
            var oneReceptionIsBlockedError = ModelStateErrorManager.getError(error, "Receptions");
            if (oneReceptionIsBlockedError.hasThisError) {
                Notify.error(oneReceptionIsBlockedError.firstError);
            }
            else {
                Notify.defaultError();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////     VISEES LES RECEPTIONS    //////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        $ctrl.canShowReceptionVisablesWarning = function () {
            return $ctrl.allSelected && $ctrl.receptions && $ctrl.receptions.length > 0;
        };

        function setReceptionVisablesWarning() {
            var visableCount = ReceptionTableauSelectorService.getVisablesReceptionCount($ctrl.allSelected, $ctrl.receptions);
            $ctrl.receptionVisablesWarning = visableCount + ' réceptions seront visées.';
        }

        $ctrl.canStampReceptions = function () {
            return ReceptionTableauSelectorService.hasReceptionsSelecteds($ctrl.allSelected, $ctrl.receptions) && !$ctrl.busy;
        };


        /**
         * Signature des réceptions sélectionnées
         *  - Enregistrement des modifications
         *  - Envoi des réceptions vers SAP
         */
        $ctrl.handleStampReceptions = function () {

            var allInvalidElements = angular.element(document.querySelectorAll('.selected-row .ng-invalid'));

            if (allInvalidElements && allInvalidElements.length === 0) {

                var selectedVisableReceptionsIds = ReceptionTableauSelectorService.getVisablesReceptionIds($ctrl.receptions);

                if (selectedVisableReceptionsIds.length > 0) {

                    actionOnBegin();

                    getIsBlockedPromise()
                        .then(actionConfirmSaveBeforeStamp)
                        .then(actionUpdateReceptionsBeforeStamp)
                        .then(actionStampReceptions)
                        .then(reloadRowUpdatedAfterStamp)
                        .then(updateRowWithBackendDataAfterStamp)
                        .then(actionUnselectReceptions)
                        .then(actionOnStampSuccess)
                        .catch(actionOnStampOnError)
                        .finally(actionOnFinally);

                }

            }
            else {
                Notify.error("Erreur, veuillez saisir tous les champs obligatoires encadrés en rouge dans la liste.");
            }
        };

        function getIsBlockedPromise() {
            var selectedReceptionsIds = ReceptionTableauSelectorService.getVisablesReceptionIds($ctrl.allSelected, $ctrl.receptions);
            return ReceptionTableauService.IsBlockedInReception(selectedReceptionsIds);
        }

        function actionConfirmSaveBeforeStamp(isBlockedInReception) {
            if (isBlockedInReception.data) {
                return confirmDialog.confirm(resources, "Certaines réceptions de la sélection sont datées sur une période déjà transférée en FAR/clôturée, et vont donc être comptabilisées sur une période plus récente. Voulez-vous continuer ? ")
                    .catch(function (error) {
                        throw new Error('stop process');
                    });
            }
        }

        /**
        * Sauvegarde des réceptions avant d'apposer le visa des réceptions
        * @param {any} isConfirmed confirmation pop-in
        * @returns {any} promise
        */
        function actionUpdateReceptionsBeforeStamp() {
            ReceptionTableauQuantityService.cleanReceptionQuantityValidationErrors($ctrl.receptions);
            var modifiedReceptions = ReceptionTableauSelectorService.getModifiedReceptions($ctrl.receptions);
            return ReceptionTableauService.UpdateReceptions(modifiedReceptions);
        }


        /**
        * Signature des réceptions
        * @param {any} isConfirmed confirmation pop-in
        * @returns {any} promise
        */
        function actionStampReceptions() {
            var selectedReceptionsIds = ReceptionTableauSelectorService.getVisablesReceptionIds($ctrl.allSelected, $ctrl.receptions);
            return ReceptionTableauService.StampReceptionsByIds(selectedReceptionsIds);
        }


        function reloadRowUpdatedAfterStamp() {
            var selectedReceptionsIds = ReceptionTableauSelectorService.getVisibleSelectedReceptionIds($ctrl.receptions);
            return ReceptionTableauService.SearchNextReceptions($ctrl.filter, selectedReceptionsIds);
        }

        function updateRowWithBackendDataAfterStamp(response) {
            let receptions = response.data;
            angular.forEach(receptions, function (reception, key) {

                ReceptionTableauDataFormatService.formatReception(reception);

                var receptionInArray = $filter('filter')($ctrl.receptions, { DepenseId: reception.DepenseId }, true)[0];

                var index = $ctrl.receptions.indexOf(receptionInArray);

                if (index !== -1) {
                    $ctrl.receptions[index] = reception;
                }
            });
        }

        function actionOnStampOnError(error) {  
            if (error.message === 'stop process') {
                return;
            }
            if (ReceptionTableauQuantityService.hasReceptionQuantityNegativeError(error, $ctrl.receptions)) {
                ReceptionTableauQuantityService.markReceptionQuantityValidationErrors(error, $ctrl.receptions, $ctrl.resources);
                Notify.defaultError();
                return;
            }
            var oneReceptionIsBlockedError = ModelStateErrorManager.getError(error, "Receptions");
            if (oneReceptionIsBlockedError.hasThisError) {
                Notify.error(oneReceptionIsBlockedError.firstError);
            }
            else {
                Notify.defaultError();
            }
        }

        /**
         * Action après visa réussi des réceptions
         * @param {any} receptions liste de réceptions
         */
        function actionOnStampSuccess(receptions) {
            Notify.message("Visa effectué avec succès !");
        }



        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////// COMMON UPDATE + VISEE RECEPTIONS ///////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        /**
         * Deselectionne les réceptions
        */
        function actionUnselectReceptions() {
            angular.forEach($ctrl.receptions, function (reception, key) {
                reception.isReceptionSelected = false;
            });
            $ctrl.allSelected = false;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////// EXPORT + EXCEL  ///////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /**
         * Gestion de l'export Excel.
         */
        $ctrl.handleExport = function () {
            if (!$ctrl.busy) {
                actionOnBegin();
                ReceptionTableauService.Export($ctrl.filter)
                    .then(function (response) {
                        // Redirection
                        window.location.href = "/api/Reception/Export/" + response.data.id;
                    })
                    .catch(Notify.defaultError)
                    .finally(actionOnFinally);
            }
        };

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////     LOOKUP        /////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        /*
         * @description Gestion de l'URL pour la Lookup
         */
        $ctrl.handleShowLookup = function (val, reception) {
            var basePrimeControllerUrl = '/api/' + val + '/SearchLight/?page=1&pageSize=20&societeId={0}&ciId={1}';
            switch (val) {
                case "Tache":
                    basePrimeControllerUrl = String.format(basePrimeControllerUrl, null, reception.CiId);
                    break;
                case "Ressource":
                    basePrimeControllerUrl = "/api/" + val + "/SearchLight/?societeId=" + reception.CommandeLigne.Commande.CI.SocieteId + "&ressourceId=" + reception.RessourceId + "&";
                    break;
                case "Organisation":
                    basePrimeControllerUrl = "/api/Organisation/SearchLightOrganisation/?typeOrganisation=ETABLISSEMENT,CI&";
                    break;
                default:
                    basePrimeControllerUrl = '/api/' + val + '/SearchLight/';
                    break;
            }

            return basePrimeControllerUrl;
        };


        $ctrl.handleRessourceChanged = function (item, reception) {
            reception.RessourceId = item.IdRef;
            reception.isModified = true;
            setUrlRessourcesRecommandeesEnabled();
        };

        $ctrl.handleTacheChanged = function (item, reception) {
            reception.TacheId = item.IdRef;
            reception.isModified = true;
        };

        $ctrl.handleOrganisationFilterChanged = function (item) {
            $ctrl.filter.OrganisationId = item.IdRef;
            actionReload(true);
        };

        $ctrl.handleDeleteOrganisationFilter = function () {
            $ctrl.filter.OrganisationId = null;
            $ctrl.filter.Organisation = null;
            actionReload(true);
        };


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////    CALCUL MONTANTS   //////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /**
         * Recalcul du montant HT réceptionné
         * @param {any} reception ligne de réception courante
         */
        $ctrl.handleCalculMontantReceptionne = function (reception) {
            // Mise à jour MontantHT de la réception
            reception.MontantHT = reception.Quantite * reception.PUHT;

            var receptions = $filter('filter')($ctrl.receptions, { CommandeLigne: { CommandeId: reception.CommandeLigne.CommandeId } }, true);

            angular.forEach(receptions, function (val, key) {
                val.CommandeLigne.Commande.MontantHTReceptionne = actionCalculCumulMontantReceptionne(receptions);
            });
        };

        /**
         * Recalcul du montantHTTotal
         * @param {any} r reception courante
         */
        $ctrl.handleCalculMontantHTTotal = function (r) {
            if ($ctrl.previousMontantHT !== r.MontantHT) {
                // Mise à jour MontantHTTotal (toutes les réceptions)
                $ctrl.receptionResult.MontantHTTotal = $ctrl.receptionResult.MontantHTTotal - $ctrl.previousMontantHT + r.MontantHT;
            }

        };

        /**
         * Recalcul du cumul montant HT réceptionné
         * @param {any} receptions liste de réceptions
         * @returns {any} liste
         */
        function actionCalculCumulMontantReceptionne(receptions) {
            var result = 0;
            angular.forEach(receptions, function (val, key) {
                result += val.MontantHT;
            });
            return result;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////    ATTACHEMENTS   /////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /**
         * Permet d'ouvrir toutes les pièces jointes d'une commande
         * @param {any} receptioIndex index dans le tableau
         */
        $ctrl.handleDownloadCommandeAttachments = function (receptioIndex) {

            // Récupérer depuis l'index
            let piecesJointes = $ctrl.receptions[receptioIndex].CommandeLigne.Commande.PiecesJointesCommande;

            for (var i = 0; i < piecesJointes.length; i++) {
                PieceJointeService.Download(piecesJointes[i].PieceJointeId);
            }
        };

        /**
         * Permet d'ouvrir toutes les pièces jointes d'une commande
         * @param {any} receptioIndex index dans le tableau
         */
        $ctrl.handleDownloadReceptionAttachments = function (receptioIndex) {

            // Récupérer depuis l'index
            let piecesJointes = $ctrl.receptions[receptioIndex].PiecesJointesReception;

            for (var i = 0; i < piecesJointes.length; i++) {
                PieceJointeService.Download(piecesJointes[i].PieceJointeId);
            }
        };


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////    FILTRES        /////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        $ctrl.handleAddFilter2Favoris = function () {
            addFilter2Favoris();
        };

        $ctrl.handleSaveFilter = function () {
            saveFilter();
        };

        $ctrl.handleCancelFilter = function () {
            cancelFilter();
        };
        $ctrl.applyTableauReceptionFilter = function () {
            actionReload(true);
        };



        /*
         * @function addFilter2Favoris()
         * @description Enregistrement des filtres actuels en tant que favori
         */
        function addFilter2Favoris() {
            var filterToSave = $ctrl.filter;
            var url = $window.location.pathname;
            if ($ctrl.favoriId !== 0) {
                url = $window.location.pathname.slice(0, $window.location.pathname.lastIndexOf("/"));
            }
            favorisService.initializeAndOpenModal("TableauReception", url, filterToSave);
        }

        /**
         * Sauvegarde les filtres acutels
         */
        function saveFilter() {
            $ctrl.oldFilter = Object.assign({}, $ctrl.filter);

        }

        /**
         * Annule les filtres
         */
        function cancelFilter() {
            $ctrl.filter = Object.assign({}, $ctrl.oldFilter);
        }

        $ctrl.displayFilterState = function displayFilterState() {
            return $ctrl.filter.Far || $ctrl.filter.IsAbonnement || $ctrl.filter.OnlyReceptionWithPiecesJointes || $ctrl.filter.IsEnergie || $ctrl.filter.IsMaterielAPointer || $ctrl.filter.Visees || $ctrl.filter.AViser;
        };

        function setUrlRessourcesRecommandeesEnabled() {
            $ctrl.resssourcesRecommandeesOnly = 0;
            if ($ctrl.receptions.length > 0 && $ctrl.receptions[0].CI && $ctrl.receptions[0].CI.EtablissementComptable && $ctrl.receptions[0].CI.EtablissementComptable.RessourcesRecommandeesEnabled) {
                $ctrl.resssourcesRecommandeesOnly = 1;
            }
        }



        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////     COMMON    //////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /**
        * Action initiale d'une promesse
        */
        function actionOnBegin() {
            ProgressBar.start();
            $ctrl.busy = true;
        }

        /**
         * Action finale d'une promesse
         */
        function actionOnFinally() {
            ProgressBar.complete();
            $ctrl.busy = false;
        }

    }
})(angular);
