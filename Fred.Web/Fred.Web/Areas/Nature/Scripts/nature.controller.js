(function (angular) {
    'use strict';

    angular.module('Fred').controller('NatureController', NatureController);

    NatureController.$inject = ['$scope', '$http', 'Notify', 'NatureService', 'ProgressBar', 'confirmDialog'];

    function NatureController($scope, $http, Notify, NatureService, ProgressBar, confirmDialog) {

        /* -------------------------------------------------------------------------------------------------------------
         *                                            INIT
         * -------------------------------------------------------------------------------------------------------------
         */
        // Instanciation Objet Ressources
        $scope.resources = resources;

        // Initialisation de la variable societeId
        $scope.societeId = undefined;

        // Instanciation de la recherche
        $scope.recherche = "";

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
        $scope.loadData = function () {
            $scope.societe = $scope.societe;
            $scope.societeId = $scope.societe.SocieteId;
            $scope.societeLibelle = $scope.societe.CodeLibelle;
            // Chargement des données
            $scope.actionInitSearch();
            $scope.actionLoad(true);
            $scope.actionNewNature();
        };


        /* -------------------------------------------------------------------------------------------------------------
         *                                            HANDLERS
         * -------------------------------------------------------------------------------------------------------------
         */

        // Handler de sélection d'une ligne de le repeater Angular
        $scope.handleSelect = function (item) {
            $scope.nature = angular.copy(item);
            $scope.checkDisplayOptions = "open";
            $scope.changeFormModel = false;
            $scope.isAlreadyUsed = false;
            $scope.isBusy = true;

            if ($scope.nature.ResourceId !== null) {
                NatureService.GetRessourceById($scope.nature.ResourceId)
                    .then((response) => {
                        $scope.nature.Resource = response.data;
                    })
                    .catch((error) => {
                        Notify.defaultError();
                    });
            }

            NatureService.isAlreadyUsed($scope.nature.NatureId)
                .then((response) => {
                    $scope.isAlreadyUsed = response.data.isAlreadyUsed;
                }).catch((error) => {
                    Notify.defaultError();
                }).finally(() => {
                    $scope.isBusy = false;
                    ProgressBar.complete();
                });

        };

        // Handler de click sur le bouton ajouter
        $scope.handleClickCreateNew = function () {
            if ($scope.societeId !== undefined) {
                $scope.isAlreadyUsed = false;
                $scope.formNature.$setPristine();
                $scope.formNature.Code.$setValidity('exist', true);
                $scope.actionNewNature();
                $scope.checkDisplayOptions = "open";
                $scope.changeFormModel = false;
            }
        };

        // Handler de click sur le bouton Enregistrer
        $scope.handleClickAddOrUpdate = function () {
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
        $scope.handleSearch = function (recherche) {
            $scope.recherche = recherche;
            $scope.actionLoad();
        };

        // Handler de frappe clavier dans le champs code
        $scope.handleChangeCode = function () {
            if (!$scope.formNature.Code.$error.pattern) {
                var idCourant;

                if ($scope.nature.NatureId !== undefined)
                    idCourant = $scope.nature.NatureId;
                else
                    idCourant = 0;
                if ($scope.societe !== null)
                    $scope.existCodeNature(idCourant, $scope.nature.Code, $scope.societe.SocieteId);
            }
        };


        /* -------------------------------------------------------------------------------------------------------------
         *                                            ACTIONS
         * -------------------------------------------------------------------------------------------------------------
         */

        // Action click sur les boutons Enregistrer
        $scope.actionAddOrUpdate = function (newItem, withNotif) {
            if ($scope.formNature.$invalid)
                return;
            if ($scope.nature.NatureId === 0)
                $scope.actionCreate(newItem, withNotif);
            else
                $scope.actionUpdate(newItem, withNotif);
        };

        // Action Create
        $scope.actionCreate = function (newItem, withNotif) {
            NatureService.Create($scope.nature).then(function () {
                $scope.actionLoad(false);
                if (newItem) {
                    $scope.handleClickCreateNew();
                }
                else {
                    $scope.actionCancel();
                }
                ProgressBar.complete();
                if (withNotif) Notify.message(resources.Global_Notification_Enregistrement_Success);
            }, function (reason) {
                console.log(reason);
                ProgressBar.complete();
                if (withNotif) Notify.error(resources.Global_Notification_Error);
            });
        };

        // Action Update
        $scope.actionUpdate = function (newItem, withNotif) {
            NatureService.Update($scope.nature).then(function () {
                $scope.actionLoad(false);
                if (newItem) {
                    $scope.handleClickCreateNew();
                }
                else {
                    $scope.actionCancel();
                }
                ProgressBar.complete();
                if (withNotif) Notify.message(resources.Global_Notification_Enregistrement_Success);
            }, function (reason) {
                console.log(reason);
                ProgressBar.complete();
                if (withNotif) Notify.error(resources.Global_Notification_Error);
            });
        };

        // Action Cancel
        $scope.actionCancel = function () {
            $scope.resource = null;
            $scope.checkDisplayOptions = "close-right-panel";
            $scope.formNature.$setPristine();
            $scope.formNature.Code.$setValidity('exist', true);
        };

        // Action Delete
        $scope.handleClickDelete = function (nature) {
            $scope.checkDisplayOptions = "close-right-panel";
            confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationSuppression).then(function () {
                NatureService.Delete(nature).then(function () {
                    $scope.actionLoad(false);
                    ProgressBar.complete();
                    $scope.actionCancel(true);
                }, function (reason) {
                    console.log(reason);
                    Notify.error(resources.Global_Notification_Suppression_Error);
                });
            });
        };

        // Action initalisation d'un nouveau Code Nature
        $scope.actionNewNature = function () {
            NatureService.New($scope.societeId).then(function (response) {
                $scope.nature = response.data;
            }, function (reason) {
                console.log(reason);
            });
        };

        // Action Load
        $scope.actionLoad = function (withNotif) {
            NatureService.SearchCodeNatureBySociete($scope.filters, $scope.societeId, $scope.recherche).then(function (response) {
                $scope.buildGrid(response.data);
                if (response && response.data && response.length === 0) {
                    Notify.error(resources.Global_Notification_AucuneDonnees);
                }
            }, function (reason) {
                console.log(reason);
                if (withNotif) Notify.error(resources.Global_Notification_Error);
            });
            ProgressBar.complete();
        };

        // Action d'initialisation de la recherche muli-critère des Codes Nature
        $scope.actionInitSearch = function () {
            $scope.filters = { Code: true, Libelle: true };
        };

        // Action de test d'existence de code nature
        $scope.existCodeNature = function (idCourant, codeNature, societeId) {
            NatureService.Exists(idCourant, codeNature, societeId).then(function (response) {
                if (response.data) {
                    $scope.formNature.Code.$setValidity('exist', false);
                } else {
                    $scope.formNature.Code.$setValidity('exist', true);
                }
            }, function (reason) {
                console.log(reason);
            });
        };

        // Action de click sur le bouton d'export excel
        $scope.handleExportExcel = function () {
            ProgressBar.start();
            NatureService.GenerateExportExcel($scope.items)
                .then(response => {
                    NatureService.DownloadExportExcel(response.data.id);
                })
                .catch(error => {
                    Notify.error("Erreur lors de l'export excel");
                })
                .finally(() => ProgressBar.complete());
        };

        $scope.handleOnDeleteResource = onDeleteResource;
        $scope.handleShowResource = showResource;

        /* -------------------------------------------------------------------------------------------------------------
         *                                            DIVERS
         * -------------------------------------------------------------------------------------------------------------
         */

        // Construction de la grille
        $scope.buildGrid = function (itemsCollection) {
            $scope.items = itemsCollection;
        };

        $scope.onSelectResource = function (item) {
            $scope.nature.Resource = item;
            $scope.nature.ResourceId = item.RessourceId;
        };

        function onDeleteResource() {
            $scope.nature.ResourceId = null;
            $scope.nature.Resource = null;
        }

        function showResource() {
            if ($scope.societe) {
                return $scope.societe.Code === '0001';
            }

            return false;
        }
    }
})(angular);