(function (angular) {
    'use strict';

    angular.module('Fred').controller('MaterielController', MaterielController);

    MaterielController.$inject = ['$scope', 'Notify', 'MaterielService', 'ProgressBar', 'confirmDialog', '$q', 'UserService'];

    function MaterielController($scope, Notify, MaterielService, ProgressBar, confirmDialog, $q, UserService) {

        /* -------------------------------------------------------------------------------------------------------------
         *                                            INIT
         * -------------------------------------------------------------------------------------------------------------
         */

        // Instanciation Objet Ressources
        $scope.resources = resources;

        // Initialisation de la variable societeId
        $scope.societeId = undefined;

        // Instanciation de la recherche
        $scope.searchText = "";

        UserService.getCurrentUser().then(function(user) {
            var userGroupCode = user.Personnel.Societe.Groupe.Code.trim();

            $scope.isGroupRZb = userGroupCode === 'GRZB';
            $scope.isUserFes = userGroupCode === 'GFES';
        });

        // Attribut d'affichage de la liste
        $scope.checkDisplayOptions = "close-right-panel";
        $scope.checkFormatOptions = "small";

        $scope.isAlreadyUsed = false;
        $scope.isBusy = false;
        // RefPicklist
        $scope.refDictionnary = {};
        $scope.refname = null;
        $scope.checkDisplayReferential = "closeReferentials";

        // Selection dans la Picklist société
        $scope.resetAndSearchMateriels = function() {
            $scope.societeId = $scope.societe.SocieteId;
            $scope.items = [];
            $scope.paging = { pageSize: 25, pageIndex: 0 };
            $scope.hasMorePage = true;
            $scope.searchMateriels();
        };

        /* -------------------------------------------------------------------------------------------------------------
         *                                            HANDLERS
         * -------------------------------------------------------------------------------------------------------------
         */

        // Handler de click sur le bouton Ajouter
        $scope.handleClickCreateNew = function () {
            if ($scope.societeId !== undefined) {
                $scope.isAlreadyUsed = false;
                $scope.formMateriel.$setPristine();
                $scope.formMateriel.Code.$setValidity('exist', true);
                $scope.actionNewMateriel();
                $scope.checkDisplayOptions = "open";
                $scope.changeFormModel = false;
                $scope.libelleCourant = resources.Global_ReferentielRessource_Placeholder;
            }
        };

        // Handler de click sur le bouton Enregistrer
        $scope.handleClickAddOrUpdate = function () {
            $scope.materiel.EtablissementComptableId = $scope.materiel.EtablissementComptableId === 0 ? null : $scope.materiel.EtablissementComptableId;
            if ($scope.isAlreadyUsed) {
                confirmDialog.confirm($scope.resources, $scope.resources.Global_Modal_Item_Already_Used)
                    .then(function () {
                        $scope.actionAddOrUpdate(false, true);
                    });
            } else {
                $scope.actionAddOrUpdate(false, true);
            }

        };

        // Handler de click sur le bouton Enregistrer et Nouveau
        $scope.handleClickAddOrUpdateAndNew = function () {
            $scope.materiel.EtablissementComptableId = $scope.materiel.EtablissementComptableId === 0 ? null : $scope.materiel.EtablissementComptableId;
            if ($scope.isAlreadyUsed) {
                confirmDialog.confirm($scope.resources, $scope.resources.Global_Modal_Item_Already_Used)
                    .then(function () {
                        $scope.actionAddOrUpdate(true, true);
                    });
            } else {
                $scope.actionAddOrUpdate(true, true);
            }

        };

        // Handler de click sur le bouton Cancel
        $scope.handleClickCancel = function () {
            $scope.actionCancel();
        };

        // Handler de frappe clavier dans le champs recherche
        $scope.handleSearch = function(searchText) {
            $scope.searchText = searchText;
            $scope.resetAndSearchMateriels();
        };

        // Handler de frappe clavier dans le champs code
        $scope.handleChangeCode = function () {
            if (!$scope.formMateriel.Code.$error.pattern) {
                var idCourant;

                if ($scope.materielId !== undefined)
                    idCourant = $scope.materielId;
                else
                    idCourant = 0;
                if ($scope.societe !== null)
                    $scope.existCodeMateriel(idCourant, $scope.materiel.Code, $scope.materiel.SocieteId);
            }
        };

        $scope.onSelectFournisseurLookup = function (item) {
            $scope.materiel.FournisseurId = item.FournisseurId;
        };

        $scope.onDeleteFournisseurLookup = function () {
            $scope.materiel.Fournisseur = null;
        };

        $scope.openDateDebutLocation = function () {
            $scope.dateDebutLocationOpened = true;
        };


        $scope.openDateFinLocation = function () {
            $scope.dateFinLocationOpened = true;
        };


        function clearMaterielLocationIfNecessary() {
            if ($scope.materiel.MaterielLocation === false) {
                $scope.materiel.Fournisseur = null;
                $scope.materiel.FournisseurId = null;
                $scope.materiel.DateDebutLocation = null;
                $scope.materiel.DateFinLocation = null;
            }
        }


        /* -------------------------------------------------------------------------------------------------------------
         *                                            ACTIONS
         * -------------------------------------------------------------------------------------------------------------
         */

        // Action click sur les boutons Enregistrer
        $scope.actionAddOrUpdate = function (newItem, withNotif) {
            if ($scope.formMateriel.$invalid || !checkFournisseurIsOk() || ($scope.isUserFes && $scope.materiel.EtablissementComptable === null))
                return;
            ProgressBar.start();
            if ($scope.materielId === 0)
                $scope.actionCreate(newItem, withNotif);
            else
                $scope.actionUpdate(newItem, withNotif);
        };

        function checkFournisseurIsOk() {
            if ($scope.materiel.MaterielLocation && !$scope.materiel.Fournisseur) {
                return false;
            }
            return true;
        }

        // Action Create
        $scope.actionCreate = function (newItem, withNotif) {
            clearMaterielLocationIfNecessary();
            MaterielService.Create($scope.materiel).then(function () {
                $scope.resetAndSearchMateriels();
                if (newItem) {
                    $scope.handleClickCreateNew();
                }
                else {
                    $scope.actionCancel();
                }
                if (withNotif) Notify.message(resources.Global_Notification_Enregistrement_Success);
            }, function (reason) {
                console.log(reason);
                if (withNotif) Notify.error(resources.Global_Notification_Error);
            });
            ProgressBar.complete();
        };

        // Action Update
        $scope.actionUpdate = function (newItem, withNotif) {
            clearMaterielLocationIfNecessary();
            MaterielService.Update($scope.materiel).then(function () {
                $scope.resetAndSearchMateriels();
                if (newItem) {
                    $scope.handleClickCreateNew();
                }
                else {
                    $scope.actionCancel();
                }
                if (withNotif) Notify.message(resources.Global_Notification_Enregistrement_Success);
            }, function (reason) {
                console.log(reason);
                ProgressBar.complete();
                if (withNotif) Notify.error(resources.Global_Notification_Error);
            });
            ProgressBar.complete();
        };

        // Action Cancel
        $scope.actionCancel = function () {
            $scope.checkDisplayOptions = "close-right-panel";
            $scope.formMateriel.$setPristine();
            $scope.formMateriel.Code.$setValidity('exist', true);
        };

        // Action Delete
        $scope.handleClickDelete = function (materiel) {
            $scope.checkDisplayOptions = "close-right-panel";
            confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationSuppression).then(function () {
                MaterielService.Delete(materiel).then(function () {
                    $scope.searchMateriels();
                    ProgressBar.complete();
                    $scope.actionCancel(true);
                }, function (reason) {
                    console.log(reason);
                    Notify.error(resources.Global_Notification_Suppression_Error);
                });
            });
        };

        // Action initalisation d'un nouveau Code Matériel
        $scope.actionNewMateriel = function () {
            MaterielService.New($scope.societeId).then(function (response) {
                $scope.materiel = response.data;
                $scope.materielId = 0;
            }, function (reason) {
                console.log(reason);
            });
        };

        // Action Load
        $scope.searchMateriels = function() {
            $scope.isBusy = true;
            ProgressBar.start();

            MaterielService.Search($scope.societeId, $scope.searchText, $scope.paging.pageSize, $scope.paging.pageIndex).then(function(response) {
                if (response && response.data && response.data.length === 0) {
                    Notify.error(resources.Global_Notification_AucuneDonnees);
                } else {
                    $scope.items.push(...response.data);
                    $scope.hasMorePage = response.data.length !== $scope.paging.pageSize;
                    FredToolBox.bindScrollEnd('#Table .refs-list', $scope.searchMaterielsNextPage);

                    $scope.isBusy = false;
                    ProgressBar.complete();
                }
            }, function() {
                var isFirstLoad = $scope.paging.pageIndex === 0;
                if (isFirstLoad === true) {
                    Notify.error(resources.Global_Notification_Error);
                }
            });
        };

        $scope.searchMaterielsNextPage = function() {
            if (!$scope.isBusy && !$scope.hasMorePage) {
                $scope.paging.pageIndex++;
                $scope.searchMateriels();
            }
        };

        // Action de test d'existence du Code Matériel
        $scope.existCodeMateriel = function (idCourant, codeMateriel, societeId) {
            MaterielService.Exists(idCourant, codeMateriel, societeId).then(function (response) {
                if (response.data) {
                    $scope.formMateriel.Code.$setValidity('exist', false);
                } else {
                    $scope.formMateriel.Code.$setValidity('exist', true);
                }
            }, function (reason) {
                console.log(reason);
            });
        };

        $scope.editMateriel = function(materielId) {
            $scope.materielId = materielId;
            $scope.isAlreadyUsed = false;
            $scope.isBusy = true;

            var getMaterielPromise = MaterielService.GetMaterielById($scope.materielId).then(function(response) {
                $scope.materiel = response.data;
                $scope.materiel.DateDebutLocation = response.data.DateDebutLocation ? new Date(response.data.DateDebutLocation) : null;
                $scope.materiel.DateFinLocation = response.data.DateFinLocation ? new Date(response.data.DateFinLocation) : null;
                $scope.libelleCourant = response.data.Ressource !== null ? response.data.Ressource.CodeLibelle : resources.Global_ReferentielRessource_Placeholder;
                $scope.checkDisplayOptions = "open";
                $scope.changeFormModel = false;
            });

            var isAlreadyUsedPromise = MaterielService.isAlreadyUsed($scope.materielId).then((response) => {
                $scope.isAlreadyUsed = response.data.isAlreadyUsed;
            });

            $q.all([getMaterielPromise, isAlreadyUsedPromise])
                .catch(function() {
                    Notify.error(resources.Global_Notification_Error);
                })
                .finally(() => {
                    $scope.isBusy = false;
                    ProgressBar.complete();
                });
        };

        /* -------------------------------------------------------------------------------------------------------------
         *                                            DIVERS
         * -------------------------------------------------------------------------------------------------------------
         */

        $scope.showPickList = function (val) {
            var baseControllerUrl = '/api/' + val + '/SearchLight/?societeId=' + $scope.materiel.SocieteId;

            $scope.apiController = val;
            switch (val) {
                case "RessourceMateriel":
                    baseControllerUrl = String.format(baseControllerUrl);
                    break;
            }
            return baseControllerUrl;
        };

        // pickListeAgenceRattachement
        $scope.loadRessource = function (item) {
            $scope.materiel.Ressource = item;
            $scope.materiel.RessourceId = item.IdRef;
            $scope.libelleCourant = item.CodeLibelle;
            if (!$.isNumeric($scope.materiel.RessourceId))
                $scope.materiel.RessourceId = null;
        };

        // pickListeAgenceRattachement - delete
        $scope.handleDeletePickListRessource = function () {
            $scope.materiel.Ressource = null;
            $scope.materiel.RessourceId = null;
            $scope.libelleCourant = resources.Global_ReferentielRessource_Placeholder;
        };

        $scope.loadEtablissementComptable = function (item) {
            $scope.materiel.EtablissementComptableId = item.IdRef;
            $scope.libelleCourantEtablissemnt = item.CodeLibelle;
            if (!$.isNumeric($scope.materiel.EtablissementComptableId))
                $scope.materiel.EtablissementComptableId = null;
        };

        $scope.handleDeletePickListEtablissementComptable = function () {
            $scope.materiel.EtablissementComptable = null;
            $scope.materiel.EtablissementComptableId = null;
            $scope.libelleCourantEtablissemnt = resources.Global_ReferentielEtablissementComptable_Placeholder;
        };

    }



})(angular);