(function (angular) {
    'use strict';


    angular.module('Fred').controller('commandeController', CommandeController);

    CommandeController.$inject = ['$scope', '$window', '$q', '$filter', 'Notify', 'CommandeService', 'ProgressBar', 'favorisService', 'favoriModal', 'fredDialog', 'PieceJointeService'];

    /**
     * Controller des Commandes
     * 
     * @param {any} $scope  Scope Angular
     * @param {any} $window  Angular     
     * @param {$q} $q Promise Angular      
     * @param {$filter} $filter Filter Angular       
     * @param {any} Notify Notify
     * @param {any} CommandeService CommandeService
     * @param {any} ProgressBar ProgressBar   
     * @param {any} favorisService Service favori
     * @param {any} favoriModal Component Favori Modal
     * @param {any} fredDialog fredDialog
     * @param {any} PieceJointeService PieceJointeService
     * @returns {CIController} $ctrl
     */
    function CommandeController($scope, $window, $q, $filter, Notify, CommandeService, ProgressBar, favorisService, favoriModal, fredDialog, PieceJointeService) {

        /* -------------------------------------------------------------------------------------------------------------
         *                                            INIT
         * -------------------------------------------------------------------------------------------------------------
         */

        // assignation de la valeur du scope au controller pour les templates non mis à jour
        var $ctrl = this;

        // méthodes exposées
        angular.extend($ctrl, {
            handleOpenFavoriModal: handleOpenFavoriModal,
            handleClickDetail: handleClickDetail,
            handleSearch: handleSearch,
            handleClickSelectAll: handleClickSelectAll,
            handleClickSelectCi: handleClickSelectCi,
            handleClickSelectCommande: handleClickSelectCommande,
            handleClickNewCommande: handleClickNewCommande,
            handleExportExcel: handleExportExcel,
            handleDisplay: handleDisplay,
            handleCloturer: handleCloturer,
            handleDecloturer: handleDecloturer,
            handleDownloadAllAttachments: handleDownloadAllAttachments,
            handleAddFilter2Favoris: handleAddFilter2Favoris,
            handleSelect: handleSelect,
            handleSelectFournisseurAgence: handleSelectFournisseurAgence,
            handleShowPicklistFournisseurAgence: handleShowPicklistFournisseurAgence,
            handleClearAgenceAndFournisseur: handleClearAgenceAndFournisseur,
            cancelFilter: cancelFilter,
            resetFilter: handleResetFilter,
            displayFilterState: displayFilterState,
            handleOpenFilter: handleOpenFilter
        });

        $ctrl.init = function (favoriId) {

            /* propriété de gestion des modes d'affichage  + Bascule par dé*/
            $ctrl.displayCard = false;
            $ctrl.displayTile = false;
            $ctrl.displayTable = true;
            $ctrl.permissionKeys = PERMISSION_KEYS;
            $ctrl.favoriId = favoriId;
            $ctrl.oldFilters = null;
            $ctrl.isFilterOpened = false;
            $ctrl.AuthorType = { AuteurCreation: "AuteurCreation", AuteurModification: "AuteurModification", Valideur: "Valideur", Verrouilleur: "AuteurVerrou" };

            angular.extend($ctrl, {
                // Instanciation Objet Ressources
                resources: resources,
                recherche: "",
                filter: [],

                hasMorePage: true,
                busy: false,
                paging: { page: 1, pageSize: 25 },
                urlDetail: "/Commande/Commande/Detail",
                countCommandes: 0,

                favori: [],
                CIList: [],
                filters: {},
                newFavori: {},
                favoriForm: {}
            });

            $q.when($ctrl.favoriId)
                .then(getFilterOrFavoris)
                .then(function () { return true; })
                .then(actionLoad);

            // Evenement chargement supplémentaire
            FredToolBox.bindScrollEnd('#commande-card', actionLoadMore);
            FredToolBox.bindScrollEnd('#commande-list', actionLoadMore);
            FredToolBox.bindScrollEnd('#commande-tile', actionLoadMore);
        };

        return $ctrl;

        /* -------------------------------------------------------------------------------------------------------------
         *                                            HANDLERS
         * -------------------------------------------------------------------------------------------------------------
         */

        function handleOpenFilter() {
            $ctrl.isFilterOpened = !$ctrl.isFilterOpened;
            $ctrl.oldFilters = angular.copy($ctrl.filter);
        }

        function handleResetFilter() {
            $q.when()
                .then(getNewFilter)
                .then(function () { return true; })
                .then(actionLoad);
        }

        /*
         * @function handleClickDetail(commande)
         * @description Redirection vers le détail d'une commande
         */
        function handleClickDetail(commande) {
            actionRedirectToDetail(commande);
        }

        /*
         * @description Handler de frappe clavier dans le champs recherche
         */
        function handleSearch(filter) {
            if (!$ctrl.busy) {
                $ctrl.filter = filter;
                actionLoad(true);
            }
        }

        /*
         * @description Action de click sur la càc Tout sélectionner
         */
        function handleClickSelectAll() {
            $ctrl.allSelection ? actionSelectAll() : actionUnselectAll();
        }

        /*
         * @description Action de click sur la càc de sélection d'un CI
         */
        function handleClickSelectCi(commandesByCi) {

            if (commandesByCi.isChecked) {
                actionSelectCi(commandesByCi);
                $ctrl.allSelection = actionIsAllSelected();
            }
            else {
                actionUnselectCi(commandesByCi);
                $ctrl.allSelection = false;
            }
        }

        /*
         * @description Action de click sur la double picklist fournisseur agence
         */
        function handleSelectFournisseurAgence(fournisseur, agence) {
            $ctrl.filter.Fournisseur = fournisseur;
            if (fournisseur) {
                $ctrl.filter.FournisseurId = fournisseur.FournisseurId;
            }

            $ctrl.filter.Agence = agence;
            if (agence && !agence.IsAgencePrincipale) {
                $ctrl.filter.AgenceId = agence.AgenceId;
            }
        }

        /*
        * @description Action de click pour afficher la double picklist fournisseur agence
        */
        function handleShowPicklistFournisseurAgence() {

            // Event d'affichage de la dualpicklist
            $scope.$broadcast("showDualPicklist");
        }

        /*
        * @description Action de suppression de fournisseur agence
        */
        function handleClearAgenceAndFournisseur() {
            $ctrl.filter.Fournisseur = null;
            $ctrl.filter.FournisseurId = null;
            $ctrl.filter.Agence = null;
            $ctrl.filter.AgenceId = null;
        }

        /*
         * @description Action de click sur la càc de sélection d'une commande
         */
        function handleClickSelectCommande(commande, commandesByCi) {
            if (commande.isChecked) {
                actionSelectCommande(commande, commandesByCi);    // Action sélection d'une commande
            }
            else {
                actionUnselectCommande(commande, commandesByCi);    // Action désélection d'une commande
            }
        }

        /*
         * @description Action de click sur le bouton de création de commande
         */
        function handleClickNewCommande() {
            actionNewCommande();
        }

        /*    
         * @description Fonction d'extraction des commandes au format excel
         */
        function handleExportExcel() {
            ProgressBar.start();
            CommandeService.ExtractExcel($ctrl.filter)
                .then(function (response) {
                    window.location.href = '/api/ExportExcelPdf/RetrieveExcel/ExtractCommandes/' + response.data;
                })
                .finally(ProgressBar.complete);

        }

        /*
         * @description Ouverture du modal d'ajout d'un favori
         */
        function handleOpenFavoriModal() {
            $q.when()
                .then(actionGetNewFavori)
                .then(function (favori) {
                    favoriModal.open(resources, favori);
                });
        }

        /*
         * @description Gestion de l'affichage de la liste des commandes
         */
        function handleDisplay(mode) {
            switch (mode) {
                case 'table':
                    $ctrl.displayCard = false;
                    $ctrl.displayTile = false;
                    $ctrl.displayTable = true;
                    break;
                case 'card':
                    $ctrl.displayCard = true;
                    $ctrl.displayTile = false;
                    $ctrl.displayTable = false;
                    break;
                case 'tile':
                    $ctrl.displayCard = false;
                    $ctrl.displayTile = true;
                    $ctrl.displayTable = false;
                    break;
            }
        }

        /**
         * Permet de clôturer une commande que si elle est validée
         * @param {any} commande Commande à clôturer
         */
        function handleCloturer(commande) {
            var numCommande = commande.NumeroCommandeExterne === null || commande.NumeroCommandeExterne === '' ? commande.Numero : commande.NumeroCommandeExterne;
            var msg = $filter('formatText')($ctrl.resources.Commande_Index_Cloturer_Confirmation, numCommande);

            if (commande.PourcentageReceptionne < 100) {
                msg = $filter('formatText')($ctrl.resources.Commande_Index_Cloturer_Non_Receptionnee_Confirmation, numCommande);
            }

            fredDialog.confirmation(msg, null, "flaticon flaticon-warning", $ctrl.resources.Commande_Index_Cloturer_Button, '', function () {

                ProgressBar.start();
                CommandeService.Cloturer(commande.CommandeId)
                    .then(function (data) {
                        if (data) {
                            commande.IsStatutCloturee = data.IsStatutCloturee;
                            commande.IsStatutValidee = false;
                            commande.DateCloture = data.DateCloture;
                            commande.StatutCommandeId = data.StatutCommandeId;
                            commande.StatutCommande = data.StatutCommande;

                            Notify.message($ctrl.resources.Commande_Index_Cloturer_Notification_Success);
                        }
                        else {
                            Notify.error($ctrl.resources.Commande_Index_Cloturer_Notification_Error);
                        }
                    })
                    .catch(Notify.defaultError)
                    .finally(ProgressBar.complete);
            });
        }

        /**
         * Permet de déclôturer une commande que si elle est clôturée
         * @param {any} commande Commande à déclôturer
         */
        function handleDecloturer(commande) {
            var numCommande = commande.NumeroCommandeExterne === null || commande.NumeroCommandeExterne === '' ? commande.Numero : commande.NumeroCommandeExterne;
            var msg = $filter('formatText')($ctrl.resources.Commande_Index_Decloturer_Confirmation, numCommande);

            fredDialog.confirmation(msg, null, "flaticon flaticon-warning", $ctrl.resources.Commande_Index_Decloturer_Button, '', function () {

                ProgressBar.start();
                CommandeService.Decloturer(commande.CommandeId)
                    .then(function (data) {
                        if (data) {
                            commande.IsStatutCloturee = data.IsStatutCloturee;
                            commande.IsStatutValidee = data.IsStatutValidee;
                            commande.DateCloture = data.DateCloture;
                            commande.StatutCommandeId = data.StatutCommandeId;
                            commande.StatutCommande = data.StatutCommande;

                            Notify.message($ctrl.resources.Commande_Index_Decloturer_Notification_Success);
                        }
                    })
                    .catch(function (error) {
                        if (error && !error.Message) Notify.error(error);
                        else Notify.defaultError();
                    })
                    .finally(ProgressBar.complete);
            });
        }

        /**
         * Gérer le téléchargment d'un fichier
         * @param {any} commandeIndex Index de la commande
         */
        function handleDownloadAllAttachments(commandeIndex) {

            let commande = $ctrl.listCommandes[commandeIndex];

            // Afficher toutes les pièces jointes
            if (commande) {
                for (var i = 0; i < commande.PiecesJointesCommande.length; i++) {
                    PieceJointeService.Download(commande.PiecesJointesCommande[i].PieceJointeId);
                }
            }
        }


        /**
         * Permet d'ajouter un favori
         */
        function handleAddFilter2Favoris() {
            addFilter2Favoris();
        }


        function handleSelect(type, item) {
            var tmp;

            switch (type) {
                case 'Departement':
                    $ctrl.filter.Departement = item.Code;
                    break;
                case 'CommandeType':
                    if (item === '') {
                        $ctrl.filter.TypeId = null;
                        $ctrl.filter.TypeLibelle = undefined;
                    }
                    else {
                        tmp = JSON.parse(item);
                        $ctrl.filter.TypeId = tmp.CommandeTypeId;
                        $ctrl.filter.TypeLibelle = tmp.Libelle;
                    }
                    break;
                case 'StatutCommande':
                    if (item === '') {
                        $ctrl.filter.StatutId = null;
                        $ctrl.filter.StatutLibelle = undefined;
                    }
                    else {
                        tmp = JSON.parse(item);
                        $ctrl.filter.StatutId = tmp.StatutCommandeId;
                        $ctrl.filter.StatutLibelle = tmp.Libelle;
                    }
                    break;
                case 'SystemeExterne':
                    if (item === '') {
                        $ctrl.filter.SystemeExterneId = null;
                        $ctrl.filter.SystemeExterneLibelle = undefined;
                    }
                    else {
                        tmp = JSON.parse(item);
                        $ctrl.filter.SystemeExterneId = tmp.SystemeExterneId;
                        $ctrl.filter.SystemeExterneLibelle = tmp.LibelleAffiche;
                    }
                    break;
            }
        }

        function cancelFilter() {
            $ctrl.filter = angular.copy($ctrl.oldFilters);
        }

        // Réinitialise les filtres
        function getNewFilter() {
            return CommandeService.GetFilter()
                .then(function (value) {
                    $ctrl.filter = value;
                    return $ctrl.filter;
                })
                .catch(function () {
                    Notify.error($ctrl.resources.Global_Notification_Error);
                    return false;
                });
        }

        function displayFilterState() {
            return $ctrl.filter.FournisseurId || $ctrl.filter.CiId || $ctrl.filter.StatutId || $ctrl.filter.TypeId || $ctrl.filter.SystemeExterneId || $ctrl.filter.MesCommandes || $ctrl.filter.DateFrom || $ctrl.filter.DateTo || $ctrl.filter.MontantHTFrom ||
                $ctrl.filter.MontantHTTo || $ctrl.filter.IsAbonnement || $ctrl.filter.IsMaterielAPointer || $ctrl.filter.IsEnergie || $ctrl.filter.Author || $ctrl.filter.OnlyCommandeWithPiecesJointes;
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            ACTIONS
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @description Action Sélection de tous les éléments de la liste (Checkbox)
         */
        function actionSelectAll() {
            angular.forEach($ctrl.listCommandesByCI, function (commandesByCi) {
                commandesByCi.isChecked = true;
                actionSelectCi(commandesByCi);
            });
        }

        /*
         * @description Action Désélection de tous les éléments de la liste (Checkbox)
         */
        function actionUnselectAll() {
            angular.forEach($ctrl.listCommandesByCI, function (commandesByCi) {
                commandesByCi.isChecked = false;
                actionUnselectCi(commandesByCi);
            });
        }

        /*
         * @description Action Sélection de toutes les commandes d'un CI
         */
        function actionSelectCi(commandesByCi) {
            angular.forEach(commandesByCi.Commandes, function (commande) {
                commande.isChecked = true;
            });
        }

        /*
         * @description Action Désélection de toutes les commandes d'un CI
         */
        function actionUnselectCi(commandesByCi) {
            angular.forEach(commandesByCi.Commandes, function (commande) {
                commande.isChecked = false;
            });
        }

        /*
         * @description Action Sélection d'une Commande
         */
        function actionSelectCommande(commande, commandesByCi) {
            commandesByCi.isChecked = actionIsAllCommandesCiSelected(commandesByCi);
            $ctrl.allSelection = actionIsAllSelected();
            $ctrl.nbSelection++;
        }

        /*
         * @description Action Désélection d'une Commande
         */
        function actionUnselectCommande(commande, commandesByCi) {
            commandesByCi.isChecked = false;
            $ctrl.allSelection = false;
        }

        /*
         * @description Fonction de test de sélection de tous les groupements CI cochés
         */
        function actionIsAllSelected() {
            for (var i = 0, len = $ctrl.listCommandesByCI.length; i < len; i++) {
                if (!$ctrl.listCommandesByCI[i].isChecked) {
                    return false;
                }
            }
            return true;
        }

        /*
         * @description Fonction de test de sélection de toutes les commandes d'un CI cochées
         */
        function actionIsAllCommandesCiSelected(commandesByCi) {
            for (var i = 0, len = commandesByCi.Commandes.length; i < len; i++) {
                if (!commandesByCi.Commandes[i].isChecked) {
                    return false;
                }
            }
            return true;
        }

        /*
         * @description Action de redirection vers la page de détail d'une commande
         */
        function actionRedirectToDetail(commande) {
            //window.location = $ctrl.urlDetail + '/' + commande;
            window.location.href = $ctrl.urlDetail + '/' + commande.CommandeId + '/false';
        }

        /*
         * @description Action de création d'une nouvelle commande
         */
        function actionNewCommande() {
            window.location.href = $ctrl.urlDetail;
        }

        /*
         * @description Action Chargement supplémentaire
         */
        function actionLoadMore() {
            if (!$ctrl.busy && $ctrl.hasMorePage) {
                $ctrl.paging.page++;
                actionLoad(false);
            }
        }

        /*
         * @description Action Chargement des Commandes
         */
        function actionLoad(firstLoad) {
            sessionStorage.setItem('commandeListFilter', JSON.stringify($ctrl.filter));
            $ctrl.busy = true;
            ProgressBar.start();

            if (firstLoad) {
                $ctrl.listCommandesByCI = [];
                $ctrl.listCommandes = [];
                $ctrl.paging.page = 1;
                $ctrl.countCommandes = 0;
            }

            return CommandeService.GetGroupByCI($ctrl.filter, $ctrl.paging.page, $ctrl.paging.pageSize)
                .then(function (response) {

                    var data = response.data.GroupedCommandes;
                    var cmdCount = 0;
                    $ctrl.countCommandes = response.data.TotalCount;
                    if (data) {
                        if (firstLoad) {
                            angular.forEach(data, function (val) {
                                angular.forEach(val.Commandes, function (cmd) {
                                    actionToLocaleDate(cmd);
                                    $ctrl.listCommandes.push(cmd);
                                    cmdCount++;
                                });
                            });
                            $ctrl.listCommandesByCI = data;
                        }
                        else {
                            angular.forEach(data, function (val) {

                                var ciCmd = $filter('filter')($ctrl.listCommandesByCI, { CI: { CiId: val.CI.CiId } }, true)[0];
                                if (ciCmd) {
                                    angular.forEach(val.Commandes, function (cmd) {
                                        actionToLocaleDate(cmd);
                                        ciCmd.Commandes.push(cmd);
                                        $ctrl.listCommandes.push(cmd);
                                        cmdCount++;
                                    });
                                }
                                else {

                                    angular.forEach(val.Commandes, function (cmd) {
                                        actionToLocaleDate(cmd);
                                        $ctrl.listCommandes.push(cmd);
                                    });

                                    $ctrl.listCommandesByCI.push(val);
                                    cmdCount += val.Commandes.length;
                                }
                            });
                        }
                    }
                    $ctrl.hasMorePage = cmdCount === $ctrl.paging.pageSize;
                })
                .catch(function (reason) {
                    console.log(reason);
                })
                .finally(function () { $ctrl.busy = false; ProgressBar.complete(); });
        }

        /*
         * @function actionGetNewFavori()
         * @description Récupère un nouvelle objet Favori
         */
        function actionGetNewFavori() {
            return favorisService.GetNew("Commande").then(function (value) {
                $ctrl.favori = value;
                $ctrl.favori.Filtre = $ctrl.filter;
                return $ctrl.favori;
            });
        }

        /*
         * @function addFilter2Favoris()
         * @description Ajout des filtres en cours aux favoris
         */
        function addFilter2Favoris() {
            var filter = $ctrl.filter;
            if (filter.CI !== undefined && filter.CI !== null) //alleger le filtre CI
            {
                filter.CI.Organisation = null;
                filter.CI.Pays = null;
                filter.CI.Societe = null;
                filter.CI.EtablissementComptable = null;
            }
            if (filter.Fournisseur !== undefined && filter.Fournisseur !== null)//alleger le filtre fournisseur
            {
                filter.Fournisseur.Pays = null;
            }

            var url = $window.location.pathname;
            if ($ctrl.favoriId !== 0) {
                url = $window.location.pathname.slice(0, $window.location.pathname.lastIndexOf("/"));
            }
            favorisService.initializeAndOpenModal("Commande", url, filter);
        }

        /*  
         * @function getFilterOrFavoris
         * @description Récupère le favori. Sinon, récupère les filtres par défaut
         */
        function getFilterOrFavoris(favoriId) {
            $ctrl.favoriId = parseInt(favoriId) || 0;
            if ($ctrl.favoriId !== 0) {
                return favorisService.getFilterByFavoriId($ctrl.favoriId)
                    .then(function (response) {
                        $ctrl.filter = response;
                        $ctrl.filter.StatutLibelle = response.StatutModel ? JSON.parse(response.StatutModel).Libelle : null;
                        $ctrl.filter.TypeLibelle = response.TypeModel ? JSON.parse(response.TypeModel).Libelle : null;
                        $ctrl.filter.SystemeExterneLibelle = response.SystemeExterneModel ? JSON.parse(response.SystemeExterneModel).Libelle : null;

                        return $ctrl.filter;
                    })
                    .catch(function () {
                        Notify.error($ctrl.resources.Global_Notification_Error);
                        return false;
                    });
            }
            else {
                if (sessionStorage.getItem('commandeListFilter') !== null) {
                    $ctrl.filter = JSON.parse(sessionStorage.getItem('commandeListFilter'));
                    return $ctrl.filter;
                }
                else {
                    return CommandeService.GetFilter()
                        .then(function (value) {
                            $ctrl.filter = value
                            $ctrl.filter.Types = $ctrl.filter.Types.filter(type => type.Code !== 'I');
                            return $ctrl.filter;
                        })
                        .catch(function () {
                            Notify.error($ctrl.resources.Global_Notification_Error);
                            return false;
                        });
                }
            }
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            DIVERS
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @function actionToLocaleDate(data)
         * @description Transforme les dates UTC issues du serveur en dates locales
         * @param {Commande} data Commande 
         */
        function actionToLocaleDate(data) {
            if (data) {
                data.Date = $filter('toLocaleDate')(data.Date);
                data.DateMiseADispo = $filter('toLocaleDate')(data.DateMiseADispo);
                data.DateCreation = $filter('toLocaleDate')(data.DateCreation);
                data.DateModification = $filter('toLocaleDate')(data.DateModification);
                data.DateValidation = $filter('toLocaleDate')(data.DateValidation);
                data.DateSuppression = $filter('toLocaleDate')(data.DateSuppression);
                data.DateCloture = $filter('toLocaleDate')(data.DateCloture);
            }
        }
    }

})(angular);