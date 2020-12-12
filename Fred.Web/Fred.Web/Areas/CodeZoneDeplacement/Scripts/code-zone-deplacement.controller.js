(function (angular) {
    'use strict';

    angular.module('Fred').controller('CodeZoneDeplacementController', CodeZoneDeplacementController);

    CodeZoneDeplacementController.$inject = ['$scope', 'Notification', 'CodeZoneDeplacementService', 'ProgressBar', 'confirmDialog', 'Notify', 'TypeSocieteService', 'UserService'];

    function CodeZoneDeplacementController($scope, Notification, CodeZoneDeplacementService, ProgressBar, confirmDialog, Notify, TypeSocieteService, UserService) {

        // Instanciation Objet Ressources
        $scope.resources = resources;

        // Instanciation de la recherche
        $scope.recherche = "";
        // Attribut d'affichage de la liste
        $scope.checkDisplayOptions = "close-right-panel";
        $scope.checkFormatOptions = "small";
        $scope.isAlreadyUsed = false;
        $scope.typeSocieteCodesParams = TypeSocieteService.GetQueryParamTypeSocieteCodes([TypeSocieteService.TypeSocieteCodes.INTERNE]);

        UserService.getCurrentUser().then(function(user) {
            $scope.isUserFes = user.Personnel.Societe.Groupe.Code.trim() === 'GFES' ? true : false;
        });

        // Selection dans la Picklist société
        $scope.loadData = function () {
            $scope.societeId = $scope.societe.SocieteId;
            $scope.societeLibelle = $scope.societe.CodeLibelle;
            // Chargement des données
            $scope.actionInitSearch();
            $scope.actionLoad(true);
            $scope.actionNewCodeZoneDeplacement();
        };

        //////////////////////////////////////////////////////////////////
        // Handlers                                                     //
        //////////////////////////////////////////////////////////////////

        // Handler de sélection d'une ligne du repeater Angular
        $scope.handleSelect = function (item) {
            $scope.isAlreadyUsed = false;
            $scope.codeZoneDeplacement = angular.copy(item);
            $scope.codeZoneDeplacement.View = {
                KmMini: $scope.codeZoneDeplacement.KmMini.toString(),
                KmMaxi: $scope.codeZoneDeplacement.KmMaxi.toString()
            };
            $scope.checkDisplayOptions = "open";
            $scope.changeFormModel = false;
            $scope.isBusy = true;
            ProgressBar.start();
            CodeZoneDeplacementService.isAlreadyUsed($scope.codeZoneDeplacement.CodeZoneDeplacementId)
                .then(function (response) {
                    $scope.isAlreadyUsed = response.data.isAlreadyUsed;
                }).catch(function (error) {
                    Notify.defaultError();
                }).finally(function () {
                    $scope.isBusy = false;
                    ProgressBar.complete();
                });
        };

        // Handler de click sur le bouton Ajouter
        $scope.handleClickCreate = function () {
            if ($scope.societeId != undefined) {
                $scope.isAlreadyUsed = false;
                $scope.formCodeZoneDeplacement.$setPristine();
                $scope.formCodeZoneDeplacement.Code.$setValidity('exist', true);
                $scope.actionNewCodeZoneDeplacement();
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
            if (!$scope.formCodeZoneDeplacement.Code.$error.pattern) {
                var idCourant;

                if ($scope.codeZoneDeplacement.CodeZoneDeplacementId != undefined)
                    idCourant = $scope.codeZoneDeplacement.CodeZoneDeplacementId;
                else
                    idCourant = 0;
                if ($scope.societe != null)
                    $scope.existCodeZoneDeplacement(idCourant, $scope.codeZoneDeplacement.Code, $scope.societe.SocieteId);
            }
        };

        //////////////////////////////////////////////////////////////////
        // Actions                                                      //
        //////////////////////////////////////////////////////////////////

        // Action click sur les boutons Enregistrer
        $scope.actionAddOrUpdate = function (newItem, withNotif) {
            if ($scope.formCodeZoneDeplacement.$invalid || !validCodeZoneDeplacement($scope.codeZoneDeplacement))
                return;
            if ($scope.codeZoneDeplacement.CodeZoneDeplacementId === 0)
                $scope.actionCreate(newItem, withNotif);
            else
                $scope.actionUpdate(newItem, withNotif);
        };

        // Action Create
        $scope.actionCreate = function (newItem, withNotif) {
            CodeZoneDeplacementService.Create($scope.codeZoneDeplacement).then(function () {
                $scope.actionLoad(false);
                if (newItem) {
                    $scope.handleClickCreate();
                }
                else {
                    $scope.actionCancel();
                }
                ProgressBar.complete();
                if (withNotif) $scope.sendNotification(resources.Global_Notification_Enregistrement_Success);
            }, function (reason) {
                console.log(reason);
                ProgressBar.complete();
                if (withNotif) $scope.sendNotificationError(resources.Global_Notification_Error);
            });
        };

        // Action Update
        $scope.actionUpdate = function (newItem, withNotif) {
            CodeZoneDeplacementService.Update($scope.codeZoneDeplacement).then(function () {
                $scope.actionLoad(false);
                if (newItem) {
                    $scope.handleClickCreate();
                }
                else {
                    $scope.actionCancel();
                }
                ProgressBar.complete();
                if (withNotif) $scope.sendNotification(resources.Global_Notification_Enregistrement_Success);
            }, function (reason) {
                console.log(reason);
                ProgressBar.complete();
                if (withNotif) $scope.sendNotificationError(resources.Global_Notification_Error);
            });
        };

        // Action Cancel
        $scope.actionCancel = function () {
            $scope.checkDisplayOptions = "close-right-panel";
            $scope.formCodeZoneDeplacement.$setPristine();
            $scope.formCodeZoneDeplacement.Code.$setValidity('exist', true);
        };

        // Action Delete
        $scope.handleClickDelete = function (codeZoneDeplacement) {
            $scope.checkDisplayOptions = "close-right-panel";
            confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationSuppression).then(function () {
                CodeZoneDeplacementService.Delete(codeZoneDeplacement).then(function () {
                    $scope.actionLoad(false);
                    $scope.sendNotification(resources.Global_Notification_Suppression_Success);
                    ProgressBar.complete();
                    $scope.actionCancel(true);
                }, function (reason) {
                    console.log(reason);
                    $scope.sendNotificationError(resources.Global_Notification_Suppression_Error);
                });
            });
        };

        // Action initalisation d'un nouveaux code zone deplacement
        $scope.actionNewCodeZoneDeplacement = function () {
            CodeZoneDeplacementService.New($scope.societeId).then(function (value) {
                $scope.codeZoneDeplacement = value.data;
                if ($scope.isUserFes) {
                    $scope.codeZoneDeplacement.IsETAM = true;
                    $scope.codeZoneDeplacement.IsCadre = true;
                    $scope.codeZoneDeplacement.IsOuvrier = true;
                }
            }, function (reason) {
                console.log(reason);
            });
        };

        // Action Load
        $scope.actionLoad = function (withNotif) {
            CodeZoneDeplacementService.Search($scope.filter, $scope.societeId, $scope.recherche).then(function (value) {
                $scope.buildGrid(value.data);

                if (value && value.length === 0) {
                    Notify.error(resources.Global_Notification_AucuneDonnees);
                }
            }, function (reason) {
                console.log(reason);
                if (withNotif) $scope.sendNotificationError(resources.Global_Notification_Error);
            });
            ProgressBar.complete();
        };

        // Action d'initialisation de la recherche muli-critère des sociétés
        $scope.actionInitSearch = function () {
            $scope.filter = { Code: true, Libelle: true };
        };

        // Action de test d'existence de code ZoneDeplacement
        $scope.existCodeZoneDeplacement = function (idCourant, code, societeId) {
            CodeZoneDeplacementService.Exists(idCourant, code, societeId).then(function (value) {
                if (value.data) {
                    $scope.formCodeZoneDeplacement.Code.$setValidity('exist', false);
                } else {
                    $scope.formCodeZoneDeplacement.Code.$setValidity('exist', true);
                }
            }, function (reason) {
                console.log(reason);
            });
        };

        //////////////////////////////////////////////////////////////////
        // Gestion diverses                                             //
        //////////////////////////////////////////////////////////////////


        // Construction de la grille
        $scope.buildGrid = function (itemsCollection) {
            $scope.items = itemsCollection;
        };

        // Gestion des notifications de succes
        $scope.sendNotification = function (message) {
            Notification({ message: message, title: resources.Global_Notification_Titre });
        };

        // Gestion des notifications d'erreur
        $scope.sendNotificationError = function (message) {
            Notification.error({ message: message, positionY: 'bottom', positionX: 'right' });
        };

        /**
       * Contrôle si le code zone déplacement est valide.
       * 
       * @param {any} codeZoneDeplacement modèle
       * @returns {Boolean} true si le code zone déplacement est valide, false sinon.
       */
        function validCodeZoneDeplacement(codeZoneDeplacement) {
            UpdateFromView(codeZoneDeplacement);
            return codeZoneDeplacement &&
                codeZoneDeplacement.KmMini < codeZoneDeplacement.KmMaxi &&
                codeZoneDeplacement.KmMini >= 0 &&
                codeZoneDeplacement.KmMaxi >= 0 &&
                codeZoneDeplacement.KmMaxi <= 9999;
        }

        $scope.MinMaxInvalid = function () {
            UpdateFromView($scope.codeZoneDeplacement);
            return $scope.codeZoneDeplacement.KmMini !== null
                && $scope.codeZoneDeplacement.KmMaxi !== null
                && $scope.codeZoneDeplacement.KmMini >= $scope.codeZoneDeplacement.KmMaxi;
        };

        function UpdateFromView(codeZoneDeplacement) {
            codeZoneDeplacement.KmMini = isNaN(codeZoneDeplacement.View.KmMini) ? null : parseFloat(codeZoneDeplacement.View.KmMini);
            codeZoneDeplacement.KmMaxi = isNaN(codeZoneDeplacement.View.KmMaxi) ? null : parseFloat(codeZoneDeplacement.View.KmMaxi);
        }
    }

})(angular);