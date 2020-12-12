(function (angular) {
    'use strict';
    angular.module('Fred').controller('CodeMajorationController', CodeMajorationController);

    CodeMajorationController.$inject = ['$scope', 'Notification', 'confirmDialog', 'CodeMajorationService', 'ProgressBar', 'Notify','UserService'];

    function CodeMajorationController($scope, Notification, confirmDialog, CodeMajorationService, ProgressBar, Notify, UserService) {

        // Instanciation Objet Ressources
        $scope.resources = resources;

        // Instanciation de la recherche
        $scope.recherche = "";


        // Attribut d'affichage de la liste
        $scope.checkDisplayOptions = "close-right-panel";
        $scope.checkFormatOptions = "small";

        $scope.isAlreadyUsed = false;
        // Initialisation des données
        $scope.codesMajoration = [];
        $scope.liste = [];
        $scope.optionsSociete = [];

        $scope.CodeMajoration = {};

        UserService.getCurrentUser().then(function(user) {
            $scope.isUserFes = user.Personnel.Societe.Groupe.Code.trim() === 'GFES' ? true : false;
        });


        init();

        //// Initialisation de la liste des codes majoration
        function init() {
            refreshData($scope.recherche);
        }

        //////////////////////////////////////////////////////////////////
        // Handlers                                                     //
        //////////////////////////////////////////////////////////////////

        // Handler de sélection d'une ligne du repeater Angular
        $scope.handleClickSelect = function (item) {
            $scope.isAlreadyUsed = false;
            $scope.CodeMajoration = angular.copy(item);
            $scope.checkDisplayOptions = "open";
            $scope.changeFormModel = false;
            ProgressBar.start();
            CodeMajorationService.isAlreadyUsed($scope.CodeMajoration.CodeMajorationId)
                .then((response) => {
                    $scope.isAlreadyUsed = response.data.isAlreadyUsed;
                }).catch((error) => {
                    Notify.defaultError();
                }).finally(() => {
                    $scope.isBusy = false;
                    ProgressBar.complete();
                });
        };

        // Handler d'affichage du formulaire pour un nouveau code
        $scope.handleClickAddNew = function () {
            CodeMajorationService.GetNew($scope.CodeMajoration).then(function (response) {
                $scope.isAlreadyUsed = false;
                $scope.CodeMajoration = response.data;
                $scope.formCodeMajoration.$setPristine();
                $scope.formCodeMajoration.Code.$setValidity('exist', true);
                $scope.checkDisplayOptions = "open";
                $scope.changeFormModel = false;
                if ($scope.isUserFes) {
                    $scope.CodeMajoration.IsETAM = true;
                    $scope.CodeMajoration.IsCadre = true;
                    $scope.CodeMajoration.IsOuvrier = true;
                }
            }, function (reason) {
                console.log(reason);
            });
        };

        // Handler de création/mise à jour d'un Code Majoration (bouton Enregistrer)
        $scope.handleClickAddOrUpdate = function () {
            if ($scope.formCodeMajoration.$invalid) {
                return;
            }

            if ($scope.CodeMajoration.CodeMajorationId === 0) {
                $scope.actionCreate($scope.CodeMajoration, false);
            }
            else {
                manageUpdate();
            }

        };

        function manageUpdate() {
            if ($scope.isAlreadyUsed) {
                confirmDialog.confirm($scope.resources, $scope.resources.Global_Modal_Item_Already_Used)
                    .then(function () {
                        $scope.actionUpdate($scope.CodeMajoration, false);
                    });
            } else {
                $scope.actionUpdate($scope.CodeMajoration, false);
            }
        }

        // Handler de sauvegarde d'un code majoration
        $scope.handleClickSave = function () {
            $scope.actionCreate($scope.CodeMajoration);
        };

        // Handler de gestion de la recherche
        $scope.handleClickSearch = function (recherche) {
            refreshData(recherche);

        };

        // Handler de frappe clavier dans le champs code
        $scope.handleChangeCode = function () {
            if (!$scope.formCodeMajoration.Code.$error.pattern) {
                var idCourant;

                if ($scope.CodeMajoration.CodeMajorationId !== undefined)
                    idCourant = $scope.CodeMajoration.CodeMajorationId;
                else
                    idCourant = 0;

                $scope.existCodeMajoration(idCourant, $scope.CodeMajoration.Code);
            }
        };

        // Fonction de coche des cases à cocher en fonction des valeurs renseignées pour les colonnes EtatPublic et IsActif
        // Handler de click sur le bouton Cancel
        $scope.handleClickCancel = function () {
            $scope.actionCancel();
        };

        // Handler de création/mise à jour d'un code majoration ET de création d'un nouveau
        $scope.handleClickAddOrUpdateAndNew = function () {
            if ($scope.formCodeMajoration.$invalid)
                return;
            if ($scope.CodeMajoration.CodeMajorationId === 0) {
                $scope.actionCreate($scope.CodeMajoration, true);
            }
            else {
                manageUpdateAndNew();
            }

        };

        function manageUpdateAndNew() {
            if ($scope.isAlreadyUsed) {
                confirmDialog.confirm($scope.resources, $scope.resources.Global_Modal_Item_Already_Used)
                    .then(function () {
                        $scope.actionUpdate($scope.CodeMajoration, true);
                    });
            } else {
                $scope.actionUpdate($scope.CodeMajoration, true);
            }
        }

        //////////////////////////////////////////////////////////////////
        // Actions                                                      //
        //////////////////////////////////////////////////////////////////

        // Action d'Annulation de la saisie
        $scope.actionCancel = function () {
            $scope.checkDisplayOptions = "close-right-panel";
            $scope.formCodeMajoration.$setPristine();
            $scope.formCodeMajoration.Code.$setValidity('exist', true);
        };

        // Action de sauvegarde d'un nouveau code majoration
        $scope.actionCreate = function (codeMaj, newItem) {
            ProgressBar.start();
            CodeMajorationService.New(codeMaj).then(function () {
                refreshData($scope.recherche);
                if (newItem) {
                    $scope.actionNewCodeMajoration();
                }
                else {
                    $scope.actionCancel();
                }
                $scope.formCodeMajoration.$setPristine();
                $scope.sendNotification(resources.Global_Notification_Enregistrement_Success);
            }).catch(function (reason) {
                Notify.defaultError();
            }).finally(function () {
                ProgressBar.complete();
            });
        };

        // Action de mise à jour d'un nouveau code majoration
        $scope.actionUpdate = function (codeMaj, newItem) {
            ProgressBar.start();
            CodeMajorationService.Update(codeMaj).then(function () {
                refreshData($scope.recherche);
                if (newItem) {
                    $scope.actionNewCodeMajoration();
                }
                else {
                    $scope.actionCancel();
                }
                $scope.formCodeMajoration.$setPristine();
                $scope.sendNotification(resources.Global_Notification_Enregistrement_Success);
            }).catch(function (reason) {
                Notify.defaultError();
            }).finally(function () {
                ProgressBar.complete();
            });
        };

        // Action Delete
        $scope.handleClickDelete = function (codeMajoration) {
            $scope.checkDisplayOptions = "close-right-panel";
            confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationSuppression).then(function () {
                ProgressBar.start();
                CodeMajorationService.Delete(codeMajoration).then(function () {
                    $scope.actionLoad(false);
                    $scope.actionCancel(true);
                    $scope.formCodeMajoration.$setPristine();
                }).catch(function (reason) {
                    Notify.defaultError();
                }).finally(function () {
                    ProgressBar.complete();
                });
            });
        };

        // Action Load
        $scope.actionLoad = function (withNotif) {
            ProgressBar.start();
            CodeMajorationService.GetList($scope.recherche)
                .then(function (response) {
                    $scope.codesMajoration = response.data;
                    if (response && response.data && response.length === 0) {
                        Notify.error(resources.Global_Notification_AucuneDonnees);
                    }
                })
                .catch(function (reason) {
                    if (withNotif) {
                        Notify.defaultError();
                    }
                }).finally(function () {
                    ProgressBar.complete();
                });
        };



        //////////////////////////////////////////////////////////////////
        // Gestion diverses                                             //
        //////////////////////////////////////////////////////////////////



        // Gestion des notifications de succes
        $scope.sendNotification = function (message) {
            Notification({
                message: message, title: resources.Global_Notification_Titre
            });
        };

        // Fonction de rafraichissement des données
        function refreshData(recherche) {
            ProgressBar.start();
            CodeMajorationService.GetList(recherche).then(function (response) {
                if (response && response.data && response.data.length === 0) {
                    Notify.error(resources.Global_Notification_AucuneDonnees);
                }
                $scope.codesMajoration = response.data;
            }).catch(function (reason) {

                Notify.defaultError();
            }).finally(function () {
                ProgressBar.complete();
            });
        }

        // Fonction de génération d'un nouveau code majoration "vide" prêt à la saisie
        $scope.actionNewCodeMajoration = function () {
            ProgressBar.start();
            CodeMajorationService.GetNew($scope.CodeMajoration).then(function (response) {
                $scope.isAlreadyUsed = false;
                $scope.CodeMajoration = response.data;
                $scope.formCodeMajoration.$setPristine();
                $scope.formCodeMajoration.Code.$setValidity('exist', true);
                $scope.checkDisplayOptions = "open";
                $scope.changeFormModel = false;
                if ($scope.isUserFes) {
                    $scope.CodeMajoration.IsETAM = true;
                    $scope.CodeMajoration.IsCadre = true;
                    $scope.CodeMajoration.IsOuvrier = true;
                }
            }).catch(function (reason) {

                Notify.defaultError();
            }).finally(function () {
                ProgressBar.complete();
            });
        };

        // Action de test d'existence de code de code majoration
        $scope.existCodeMajoration = function (idCourant, codeMajoration) {
            CodeMajorationService.Exists(idCourant, codeMajoration).then(function (response) {
                if (response.data) {
                    $scope.formCodeMajoration.Code.$setValidity('exist', false);
                } else {
                    $scope.formCodeMajoration.Code.$setValidity('exist', true);
                }
            }).catch(function (reason) {


            }).finally(function () {
                ProgressBar.complete();
            });
        };

    }



})(angular);