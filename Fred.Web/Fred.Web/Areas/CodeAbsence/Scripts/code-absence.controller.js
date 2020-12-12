(function (angular) {
    'use strict';

    angular.module('Fred').controller('CodeAbsenceController', CodeAbsenceController);

    CodeAbsenceController.$inject = ['$scope', 'Notification', 'CodeAbsenceService', 'ngProgressFactory', 'confirmDialog', 'TypeSocieteService','UserService'];

    function CodeAbsenceController($scope, Notification, CodeAbsenceService, ngProgressFactory, confirmDialog, TypeSocieteService, UserService) {

        //////////////////////////////////////////////////////////////////
        //                          INIT                                //
        //////////////////////////////////////////////////////////////////

        // Instanciation Objet Ressources
        $scope.resources = resources;

        // Initialisation de la variable societeId
        $scope.societeId = undefined;

        // Instanciation de la recherche
        $scope.recherche = "";

        // Instanciation Objet ProgressBar
        $scope.progressBar = ngProgressFactory.createInstance();
        $scope.progressBar.setHeight("7px");
        $scope.progressBar.setColor("#FDD835");

        // Attribut d'affichage de la liste
        $scope.checkDisplayOptions = "close-right-panel";
        $scope.checkFormatOptions = "small";
        $scope.typeSocieteCodesParams = TypeSocieteService.GetQueryParamTypeSocieteCodes([TypeSocieteService.TypeSocieteCodes.INTERNE]);

        UserService.getCurrentUser().then(function(user) {
            $scope.isUserFes = user.Personnel.Societe.Groupe.Code.trim() === 'GFES' ? true : false;
        });

        $scope.isAlreadyUsed = false;
        $scope.isBusy = false;
        // Selection dans la Picklist société
        $scope.loadData = function () {
            $scope.societeId = $scope.societe.SocieteId;
            $scope.societeLibelle = $scope.societe.CodeLibelle;
            // Chargement des données
            actionInitSearch();
            actionLoad(true);
        };


        //////////////////////////////////////////////////////////////////
        //                        HANDLERS                              //
        //////////////////////////////////////////////////////////////////

        // Handler de sélection d'une ligne
        $scope.handleSelect = function (item) {
            $scope.codeAbsence = angular.copy(item);
            $scope.checkDisplayOptions = "open";
            $scope.changeFormModel = false;
            $scope.isAlreadyUsed = false;
            $scope.isBusy = true;
            CodeAbsenceService.isAlreadyUsed($scope.codeAbsence.CodeAbsenceId)
                .then((response) => {
                    $scope.isAlreadyUsed = response.data.isAlreadyUsed;
                }).catch((error) => {
                    Notify.defaultError();
                }).finally(() => {
                    $scope.isBusy = false;
                    ProgressBar.complete();
                });

        };

        // Handler de click sur le bouton Ajouter
        $scope.handleClickCreateNew = function () {
            if ($scope.societeId !== undefined) {
                $scope.isAlreadyUsed = false;
                $scope.formCodeAbsence.$setPristine();
                $scope.formCodeAbsence.Code.$setValidity('exist', true);
                actionNewCodeAbsence();
                $scope.checkDisplayOptions = "open";
                $scope.changeFormModel = false;
            }
        };

        // Handler de click sur le bouton Enregistrer
        $scope.handleClickAddOrUpdate = function () {
            if ($scope.isAlreadyUsed) {
                confirmDialog.confirm($scope.resources, $scope.resources.Global_Modal_Item_Already_Used)
                    .then(function () {
                        actionAddOrUpdate(false, true);
                    });
            } else {
                actionAddOrUpdate(false, true);
            }

        };

        // Handler de click sur le bouton Enregistrer et Nouveau
        $scope.handleClickAddOrUpdateAndNew = function () {
            if ($scope.isAlreadyUsed) {
                confirmDialog.confirm($scope.resources, $scope.resources.Global_Modal_Item_Already_Used)
                    .then(function () {
                        actionAddOrUpdate(true, true);
                    });
            } else {
                actionAddOrUpdate(true, true);
            }

        };

        // Handler de click sur le bouton Cancel
        $scope.handleClickCancel = function () {
            actionCancel();
        };

        // Handler de frappe clavier dans le champs recherche
        $scope.handleSearch = function (recherche) {
            $scope.recherche = recherche;
            actionLoad();
        };

        // Handler de frappe clavier dans le champs code
        $scope.handleChangeCode = function () {
            if (!$scope.formCodeAbsence.Code.$error.pattern) {
                var idCourant;

                if ($scope.codeAbsence.CodeAbsenceId !== undefined)
                    idCourant = $scope.codeAbsence.CodeAbsenceId;
                else
                    idCourant = 0;
                if ($scope.societe !== null)
                    existCodeAbsence(idCourant, $scope.codeAbsence.Code, $scope.societe.SocieteId);
            }
        };

        // Handle vérifie si les heures sont numériques
        $scope.handleCheckNumeric = function (input) {
            actionSetNumericValidity(input);
        };

        //Convert Values ton int in HTML
        $scope.parseInt = function (input) {
            return parseInt(input);
        };

        //Convert Values to float in HTML
        $scope.parseFloat = function (input) {
            return parseFloat(input);
        };
        //////////////////////////////////////////////////////////////////
        //                          ACTIONS                             //
        //////////////////////////////////////////////////////////////////

        // Action click sur les boutons Enregistrer
        function actionAddOrUpdate(newItem, withNotif) {
            if ($scope.formCodeAbsence.$invalid)
                return;

            if ($scope.codeAbsence.TauxDecote === null || $scope.codeAbsence.TauxDecote === undefined) {
                $scope.codeAbsence.TauxDecote = 0;
            }

            var validEtam = parseFloat($scope.codeAbsence.NBHeuresMinETAM) <= parseFloat($scope.codeAbsence.NBHeuresMaxETAM);
            var validCo = parseFloat($scope.codeAbsence.NBHeuresMinCO) <= parseFloat($scope.codeAbsence.NBHeuresMaxCO);
            var validDefautEtam = parseFloat($scope.codeAbsence.NBHeuresDefautETAM) >= parseFloat($scope.codeAbsence.NBHeuresMinETAM) &&
                parseFloat($scope.codeAbsence.NBHeuresDefautETAM) <= parseFloat($scope.codeAbsence.NBHeuresMaxETAM);
            var validDefautCo = parseFloat($scope.codeAbsence.NBHeuresDefautCO) >= parseFloat($scope.codeAbsence.NBHeuresMinCO) &&
                parseFloat($scope.codeAbsence.NBHeuresDefautCO) <= parseFloat($scope.codeAbsence.NBHeuresMaxCO);

            if (validEtam && validCo && validDefautEtam && validDefautCo) {
                if ($scope.codeAbsence.CodeAbsenceId === 0) {
                    actionCreate(newItem, withNotif);
                } else {
                    actionUpdate(newItem, withNotif);
                }
            }
        }

        // Action Create
        function actionCreate(newItem, withNotif) {
            CodeAbsenceService.Create($scope.codeAbsence).then(function () {
                actionLoad(false);
                newItem ? $scope.handleClickCreateNew() : actionCancel();
                if (withNotif) sendNotification(resources.Global_Notification_Enregistrement_Success);
            })
                .catch(function (reason) {
                    console.log(reason);
                    if (withNotif) sendNotificationError(resources.Global_Notification_Error);
                })
                .finally(function () {
                    $scope.progressBar.complete();
                });
        }

        // Action Update
        function actionUpdate(newItem, withNotif) {
            CodeAbsenceService.Update($scope.codeAbsence)
                .then(function () {
                    actionLoad(false);
                    newItem ? $scope.handleClickCreateNew() : actionCancel();
                    if (withNotif) sendNotification(resources.Global_Notification_Enregistrement_Success);
                })
                .catch(function (reason) {
                    console.log(reason);
                    if (withNotif) sendNotificationError(resources.Global_Notification_Error);
                })
                .finally(function () {
                    $scope.progressBar.complete();
                });
        }

        // Action Cancel
        function actionCancel() {
            $scope.checkDisplayOptions = "close-right-panel";
            $scope.formCodeAbsence.$setPristine();
            $scope.formCodeAbsence.Code.$setValidity('exist', true);
        }

        // Action Delete
        $scope.handleClickDelete = function (codeAbsence) {
            $scope.checkDisplayOptions = "close-right-panel";
            confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationSuppression).then(function () {
                CodeAbsenceService.Delete(codeAbsence).then(function () {
                    actionLoad(false);
                    $scope.progressBar.complete();
                    $scope.formCodeAbsence.$setPristine();
                },
                    function (reason) {
                        console.log(reason);
                        sendNotificationError(resources.Global_Notification_Suppression_Error);
                    });
            });
        };

        // Action initalisation d'un nouveau Code Absence
        function actionNewCodeAbsence() {
            CodeAbsenceService.New($scope.societeId).then(function (response) {
                $scope.codeAbsence = response.data;
                if ($scope.isUserFes) {
                    $scope.codeAbsence.IsETAM = true;
                    $scope.codeAbsence.IsCadre = true;
                    $scope.codeAbsence.IsOuvrier = true;
                }
            },
                function (reason) {
                    console.log(reason);
                });
        }

        // Action Load
        function actionLoad(withNotif) {
            CodeAbsenceService.Search($scope.filters, $scope.recherche, $scope.societeId).then(function (response) {
                actionBuildGrid(response.data);
                if (response && response.data && response.data.length === 0) {
                    sendNotificationError(resources.Global_Notification_AucuneDonnees);
                }
            },
                function () {
                    if (withNotif) sendNotificationError(resources.Global_Notification_Error);
                });
            $scope.progressBar.complete();
        }

        // Action d'initialisation de la recherche muli-critère des Codes Absence
        function actionInitSearch() {
            $scope.filters = { Code: true, Libelle: true };
        }

        // Action de test d'existence de code absence
        function existCodeAbsence(idCourant, codeCodeAbsence, societeId) {
            CodeAbsenceService.Exists(idCourant, codeCodeAbsence, societeId).then(function (response) {
                $scope.formCodeAbsence.Code.$setValidity('exist', response.data ? false : true);
            },
                function (reason) { console.log(reason); });
        }

        // Action de test si une valeur est numérique ou pas et set la validité de l'input
        function actionSetNumericValidity(input) {
            if (input.$$rawModelValue === undefined) {
                input.$setValidity('numeric', !isNaN(parseFloat(input.$modelValue)));
            } else {
                input.$setValidity('numeric', true);
            }
        }


        //////////////////////////////////////////////////////////////////
        //                          DIVERS                              //
        //////////////////////////////////////////////////////////////////


        // Construction de la grille
        function actionBuildGrid(itemsCollection) {
            $scope.items = itemsCollection;
        }

        // Gestion des notifications de succes
        function sendNotification(message) {
            Notification({ message: message, title: resources.Global_Notification_Titre });
        }

        // Gestion des notifications d'erreur
        function sendNotificationError(message) {
            Notification.error({ message: message, positionY: 'bottom', positionX: 'right' });
        }

    }



})(angular);