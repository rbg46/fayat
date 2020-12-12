(function (angular) {
    'use strict';

    angular.module('Fred').controller('IndemniteDeplacementController', IndemniteDeplacementController);

    IndemniteDeplacementController.$inject = ['$scope', '$q', '$filter', 'Notify', 'IndemniteDeplacementService', 'PersonnelService', 'ProgressBar', 'confirmDialog'];

    function IndemniteDeplacementController($scope, $q, $filter, Notify, IndemniteDeplacementService, PersonnelService, ProgressBar, confirmDialog) {

        // Instanciation Objet Ressources
        $scope.resources = resources;

        // Instanciation de la recherche
        $scope.recherche = "";

        // Attribut d'affichage de la liste
        $scope.checkDisplayOptions = "close-right-panel";
        $scope.checkFormatOptions = "small";

        $scope.personnel = null;
        $scope.personnelId = null;

        $scope.indemniteDeplacement = {};

        /*
         * -------------------------------------------------------------------------------------------------------------
         *                                            INIT
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         *  @description Initialisation de la liste des Indemnités de déplacement
         */
        $scope.init = function (id) {
            $scope.personnelId = id;

            $q.when()
                .then(actionGetUserRolePaie)
                .then(actionLoadPersonnel)
                .then(actionGetFilter)
                .then(actionLoad);
        };

        /*
         * -------------------------------------------------------------------------------------------------------------
         *                                            HANDLERS
         * -------------------------------------------------------------------------------------------------------------
         */

        // Handler de sélection d'une ligne de le repeater Angular
        $scope.handleSelect = function (item) {
            $scope.indemniteDeplacement = angular.copy(item);
            $scope.checkDisplayOptions = "open";
            // Libellés des PickList
            $scope.CodeCILibelleCourant = $scope.indemniteDeplacement.CI != null ? $scope.indemniteDeplacement.CI.CodeLibelle : resources.Global_ReferentielCI_Placeholder;
            $scope.CodeDeplacementLibelleCourant = $scope.indemniteDeplacement.CodeDeplacement != null ? $scope.indemniteDeplacement.CodeDeplacementLibelle : resources.Global_ReferentielCodeDeplacement_Placeholder;
            $scope.CodeZoneDeplacementLibelleCourant = $scope.indemniteDeplacement.CodeZoneDeplacement != null ? $scope.indemniteDeplacement.CodeZoneDeplacementLibelle : resources.Global_ReferentielCodeZoneDeplacement_Placeholder;
        };

        // Handler de click sur le bouton Ajouter - à adapter
        $scope.handleClickCreateNew = function () {
            $scope.formIndemniteDeplacement.$setPristine();
            actionNewIndemniteDeplacement();
            $scope.checkDisplayOptions = "open";
            // Libellés des PickList par défaut
            $scope.CodeCILibelleCourant = resources.Global_ReferentielCI_Placeholder;
            $scope.CodeDeplacementLibelleCourant = resources.Global_ReferentielCodeDeplacement_Placeholder;
            $scope.CodeZoneDeplacementLibelleCourant = resources.Global_ReferentielCodeZoneDeplacement_Placeholder;
        };

        // Handler de click sur le bouton Supprimer
        $scope.handleClickRemove = function () {
            $scope.checkDisplayOptions = "close-right-panel";
            confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationSuppression).then(function () {
                actionRemove();
            });
        };

        // Handler de click sur le bouton Enregistrer
        $scope.handleClickAddOrUpdate = function () {
            if ($scope.indemniteDeplacement.CodeDeplacement && $scope.indemniteDeplacement.CodeZoneDeplacement && $scope.indemniteDeplacement.CodeDeplacement.IGD) {
                Notify.error(resources.IndemniteDeplacement_Index_IGD_Zone_Error);
            }
            else {
                actionAddOrUpdate(false);
            }
        };

        // Handler de click sur le bouton Enregistrer et Nouveau
        $scope.handleClickAddOrUpdateAndNew = function () {
            if ($scope.indemniteDeplacement.CodeDeplacement && $scope.indemniteDeplacement.CodeZoneDeplacement && $scope.indemniteDeplacement.CodeDeplacement.IGD) {
                Notify.error(resources.IndemniteDeplacement_Index_IGD_Zone_Error);
            }
            else {
                actionAddOrUpdate(true);
            }
        };

        // Handler de click sur le bouton Cancel
        $scope.handleClickCancel = function () {
            actionCancel();
        };

        // Handler de frappe clavier dans le champs recherche
        $scope.handleSearch = function () {
            actionLoad();
        };

        // Handler de frappe clavier dans le champs kilometre
        $scope.handlerChangeKm = function (textRecherche) {
            $scope.formIndemniteDeplacement.SaisieManuelle = true;
        };

        // Handler de sélection d'une ligne de la grille Kendo
        $scope.handleClickCalculKm = function () {
            actionCalculKm();
        };

        /*
         * -------------------------------------------------------------------------------------------------------------
         *                                            ACTIONS
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @description Calcul Kilomètre
         */
        function actionCalculKm() {

            ProgressBar.start();

            IndemniteDeplacementService.CalculKm($scope.indemniteDeplacement)
                .then(function (value) {
                    if (value.Warning && value.Warning != '') {
                        Notify.warning(value.Warning);
                    }
                    actionToLocaleDate(value);
                    $scope.indemniteDeplacement = value;
                    $scope.CodeDeplacementLibelleCourant = $scope.indemniteDeplacement.CodeDeplacement != null ? $scope.indemniteDeplacement.CodeDeplacementLibelle : resources.Global_ReferentielCodeDeplacement_Placeholder;
                    $scope.CodeZoneDeplacementLibelleCourant = $scope.indemniteDeplacement.CodeZoneDeplacement != null ? $scope.indemniteDeplacement.CodeZoneDeplacementLibelle : resources.Global_ReferentielCodeZoneDeplacement_Placeholder;
                    //Notify.message(resources.CalculKmSuccess_lb);
                })
                .catch(function (reason) {
                    console.log(reason);
                    Notify.error(resources.IndemniteDeplacement_Index_PanelRight_CalculKm_Erreur);
                })
                .finally(function () { ProgressBar.complete(); });
        }

        /*
         * @description Action click sur les boutons Enregistrer
         */
        function actionAddOrUpdate(newItem) {
            if ($scope.formIndemniteDeplacement.$invalid) {
                return;
            }

            if ($scope.indemniteDeplacement.CodeZoneDeplacement == null && $scope.indemniteDeplacement.CodeDeplacement == null) {
                Notify.error(resources.IndemniteDeplacement_Index_PanelRight_CodeRequis_Erreur);
                return;
            }

            if ($scope.indemniteDeplacement.IndemniteDeplacementId == 0) {
                actionCreate(newItem);
            }
            else {
                actionUpdate(newItem);
            }
        }

        /*
         * @description Action création d'une indemnité déplacement
         */
        function actionCreate(newItem) {
            ProgressBar.start();
            IndemniteDeplacementService.Create($scope.indemniteDeplacement)
                .then(function (value) {
                    actionLoad();
                    if (newItem) {
                        $scope.handleClickCreateNew();
                    }
                    else {
                        actionCancel();
                    }
                    Notify.message(resources.Global_Notification_Enregistrement_Success);
                })
                .catch(function (reason) {
                    Notify.error(reason.Message);
                })
                .finally(function () { ProgressBar.complete(); });
        }

        /*
         * @description Action Mise à jour d'une indemnité déplacement
         */
        function actionUpdate(newItem) {
            ProgressBar.start();
            IndemniteDeplacementService.Update($scope.indemniteDeplacement)
                .then(function (value) {
                    actionLoad();
                    if (newItem) {
                        $scope.handleClickCreateNew();
                    }
                    else {
                        actionCancel();
                    }
                    Notify.message(resources.Global_Notification_Enregistrement_Success);
                })
                .catch(function (reason) {
                    console.log(reason);
                    Notify.error(resources.Global_Notification_Error);
                })
                .finally(function () { ProgressBar.complete(); });
        }

        /*
         * @description Action Suppression d'une indemnité déplacement
         */
        function actionRemove() {
            ProgressBar.start();
            IndemniteDeplacementService.Delete($scope.indemniteDeplacement.IndemniteDeplacementId)
                .then(function (value) {
                    actionLoad();
                    actionCancel();
                    Notify.message(resources.Global_Notification_Suppression_Success);
                })
                .catch(function (reason) {
                    console.log(reason);
                    Notify.error(resources.Global_Notification_Error);
                })
                .finally(function () { ProgressBar.complete(); });
        }

        /*
         * @description Action Annuler
         */
        function actionCancel() {
            $scope.checkDisplayOptions = "close-right-panel";
            $scope.formIndemniteDeplacement.$setPristine();
        }

        // Action initalisation d'une nouvelle indemnité de déplacement
        function actionNewIndemniteDeplacement() {
            IndemniteDeplacementService.New().then(function (value) {
                $scope.indemniteDeplacement = value;
                $scope.indemniteDeplacement.Personnel = $scope.personnel;
                $scope.indemniteDeplacement.PersonnelId = $scope.personnelId;
            })
                .catch(function (reason) {
                    console.log(reason);
                });
        }

        /*
         * @description Chargement des indemnités déplacement d'un personnel
         */
        function actionLoad() {
            $scope.items = [];
            return IndemniteDeplacementService.SearchByPersonnel($scope.filters, $scope.personnelId, $scope.recherche).then(function (value) {
                if (value && value.length === 0) {
                    Notify.error(resources.Global_Notification_AucuneDonnees);
                }
                else {
                    // Formatting dates
                    angular.forEach(value, function (val, key) {
                        actionToLocaleDate(val);
                    });
                    $scope.items = value;
                }
            })
                .catch(function (reason) {
                    console.log(reason);
                    Notify.error(resources.Global_Notification_Error);
                });
        }

        /*
         * @description Chargement du personnel
         */
        function actionLoadPersonnel() {
            return PersonnelService.GetById({ personnelId: $scope.personnelId }).$promise.then(function (value) {
                $scope.personnel = value;
            })
                .catch(Notify.defaultError);
        }

        // Action d'initialisation de la recherche muli-critère des sociétés
        function actionGetFilter() {
            $scope.filters = { Ci: true, CodeDeplacement: true, CodeZoneDeplacement: true };
            return $scope.filters;
        }

        function actionGetUserRolePaie() {
            IndemniteDeplacementService.IsRolePaie()
                .then(function (value) {
                    $scope.isRolePaie = value;
                })
                .catch(function (reason) {
                    console.log(reason);
                });
        }

        // Fonction permettant de retourner à la liste du Personnels
        $scope.exit = function () {
            window.location = '/Personnel/Personnel';
        };

        /*
         * -------------------------------------------------------------------------------------------------------------
         *                                            LOOKUP
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @description Création de l'URL de la Lookup
         */
        $scope.handleShowLookup = function (val) {

            $scope.apiController = val;

            var baseControllerUrl = '/api/' + val + '/SearchLight/?ciId={0}';

            switch (val) {
                case "CodeDeplacement":
                    baseControllerUrl = String.format(baseControllerUrl, $scope.indemniteDeplacement.CI.CiId);
                    break;
                case "CodeZoneDeplacement":
                    baseControllerUrl = String.format(baseControllerUrl, $scope.indemniteDeplacement.CI.CiId);
                    break;
            }

            return baseControllerUrl;
        };

        /*
         * @description Gestion de la sélection d'un élement dans la Lookup
         */
        $scope.handleLookupSelection = function (type, item) {
            switch (type) {
                case "CI":
                    {
                        $scope.indemniteDeplacement.CI = item;
                        $scope.indemniteDeplacement.CiId = item.CiId;
                        $scope.indemniteDeplacement.CiLibelle = item.Libelle;
                        $scope.CodeCILibelleCourant = item.CodeLibelle;

                        if (!$scope.indemniteDeplacement.SaisieManuelle) {
                            actionCalculKm();
                        }
                        break;
                    }
                case "CodeDeplacement":
                    $scope.indemniteDeplacement.CodeDeplacement = item;
                    $scope.indemniteDeplacement.CodeDeplacementId = item.CodeDeplacementId;
                    $scope.indemniteDeplacement.CodeDeplacementLibelle = item.Code + " - " + item.Libelle;
                    $scope.CodeDeplacementLibelleCourant = item.Code + " - " + item.Libelle;
                    break;
                case "CodeZoneDeplacement":
                    $scope.indemniteDeplacement.CodeZoneDeplacement = item;
                    $scope.indemniteDeplacement.CodeZoneDeplacementId = item.CodeZoneDeplacementId;
                    $scope.indemniteDeplacement.CodeZoneDeplacementLibelle = item.Code + " - " + item.Libelle;
                    $scope.CodeZoneDeplacementLibelleCourant = item.Code + " - " + item.Libelle;
                    break;
            }
        };

        /*
         * @description Gestion de la suppression de l'élément sélectionné dans la Lookup
         */
        $scope.handleLookupDeletion = function (type) {
            switch (type) {
                case "CI":
                    $scope.indemniteDeplacement.CI = null;
                    $scope.indemniteDeplacement.CiId = null;
                    $scope.CodeCILibelleCourant = resources.Global_ReferentielCI_Placeholder;
                    break;
                case "CodeDeplacement":
                    $scope.indemniteDeplacement.CodeDeplacement = null;
                    $scope.indemniteDeplacement.CodeDeplacementId = null;
                    $scope.CodeDeplacementLibelleCourant = resources.Global_ReferentielCodeDeplacement_Placeholder;
                    break;
                case "CodeZoneDeplacement":
                    $scope.indemniteDeplacement.CodeZoneDeplacement = null;
                    $scope.indemniteDeplacement.CodeZoneDeplacementId = null;
                    $scope.CodeZoneDeplacementLibelleCourant = resources.Global_ReferentielCodeZoneDeplacement_Placeholder;
                    break;
            }
        };

        /*
         * -------------------------------------------------------------------------------------------------------------
         *                                            DIVERS
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @description Gestion des dates
         */
        function actionToLocaleDate(indemniteDep) {
            indemniteDep.DateDernierCalcul = $filter('toLocaleDate')(indemniteDep.DateDernierCalcul);
            indemniteDep.DateCreation = $filter('toLocaleDate')(indemniteDep.DateCreation);
            indemniteDep.DateModification = $filter('toLocaleDate')(indemniteDep.DateModification);
            indemniteDep.DateSuppression = $filter('toLocaleDate')(indemniteDep.DateSuppression);
        }
    }
})(angular);