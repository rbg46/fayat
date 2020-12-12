(function (angular) {
    'use strict';

    angular.module('Fred').controller('PrimeController', PrimeController);

    PrimeController.$inject = ['UserService', '$scope', 'Notify', 'PrimeService', 'ProgressBar', 'confirmDialog', 'TypeSocieteService', 'ModelStateErrorManager'];

    function PrimeController(UserService, $scope, Notify, PrimeService, ProgressBar, confirmDialog, TypeSocieteService, ModelStateErrorManager) {

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
        $scope.typeSocieteCodesParams = TypeSocieteService.GetQueryParamTypeSocieteCodes([TypeSocieteService.TypeSocieteCodes.INTERNE, TypeSocieteService.TypeSocieteCodes.PARTENAIRE]);

        $scope.errorMessage = null;

        UserService.getCurrentUser().then(function(user) {
            $scope.isUserFes = user.Personnel.Societe.Groupe.Code.trim() === 'GFES' ? true : false;

            $scope.userOrganizationId = user.Personnel.Societe.Organisation.OrganisationId;
        });

        // Selection dans la Picklist société
        $scope.loadData = function () {
            $scope.societe = $scope.societe;
            $scope.societeId = $scope.societe.SocieteId;
            $scope.societeLibelle = $scope.societe.CodeLibelle;
            //Chargement des données
            $scope.actionInitSearch();
            $scope.actionLoad(true);
            $scope.actionNewPrime();
        };

        // Retourne vrai si la prime est de type horaire
        $scope.IsPrimeHoraire = function (prime) {
            if (prime) {
                return prime.PrimeType === 1;
            }
            else {
                return false;
            }
        };

        // Retourne vrai si la prime est de type journaliere
        $scope.IsPrimeJournaliere = function (prime) {
            if (prime) {
                return prime.PrimeType === 0;
            }
            else {
                return false;
            }
        };

        //Retourne vrai si c'est un etamIac 
        $scope.IsEtamIac = function (prime) {
            if (prime) {
                if (prime.TargetPersonnel === 2) {
                    prime.Publique = true;
                    return true;
                }
                else {
                    return false;
                }
            }
        };

        //Retourne vrai si c'est un ouvrier
        $scope.IsOuvrier = function (prime) {
            if (prime) {
                if (prime.TargetPersonnel === 1) {
                    prime.Publique = true;
                    return true;
                }
                else {
                    return false;
                }
            }
        };

        // Retourne vrai si la prime est de type mensuelle
        $scope.IsPrimeMensuelle = function (prime) {
            if (prime) {
                if (prime.PrimeType === 2) {
                    prime.Publique = true; // Une prime de type Mensuelle est obligatoirement Publique
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        };


        /* -------------------------------------------------------------------------------------------------------------
         *                                            HANDLERS
         * -------------------------------------------------------------------------------------------------------------
         */

        // Handler de sélection d'une ligne de le repeater Angular
        $scope.handleSelect = function (item) {
            $scope.isAlreadyUsed = false;
            $scope.prime = angular.copy(item);
            $scope.checkDisplayOptions = "open";
            $scope.changeFormModel = false;
            ProgressBar.start();
            $scope.isBusy = true;
            PrimeService.isAlreadyUsed($scope.prime.PrimeId)
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
            if ($scope.societe.SocieteId !== undefined) {
                $scope.isAlreadyUsed = false;
                $scope.formPrime.$setPristine();
                $scope.formPrime.Code.$setValidity('exist', true);
                $scope.actionNewPrime();
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
            if (!$scope.formPrime.Code.$error.pattern) {
                var idCourant;

                if ($scope.prime.PrimeId !== undefined)
                    idCourant = $scope.prime.PrimeId;
                else
                    idCourant = 0;
                if ($scope.societe !== null)
                    $scope.existCodeprime(idCourant, $scope.prime.Code, $scope.societeId);
            }
        };

        /* -------------------------------------------------------------------------------------------------------------
         *                                            ACTIONS
         * -------------------------------------------------------------------------------------------------------------
         */

        // Action click sur les boutons Enregistrer
        $scope.actionAddOrUpdate = function (newItem, withNotif) {
            if ($scope.formPrime.$invalid)
                return;
            if ($scope.prime.PrimeId === 0)
                $scope.actionCreate(newItem, withNotif);
            else
                $scope.actionUpdate(newItem, withNotif);
        };

        // Action Create
        $scope.actionCreate = function (newItem, withNotif) {
            PrimeService.Create($scope.prime).then(function () {
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
                if (withNotif) Notify.error($scope.errorMessage(reason));
            });
        };

        // Action Update
        $scope.actionUpdate = function (newItem, withNotif) {
            PrimeService.Update($scope.prime).then(function () {
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
                ProgressBar.complete();
                if (withNotif) Notify.error($scope.errorMessage(reason));
            });
        };

        // Action Cancel
        $scope.actionCancel = function () {
            $scope.checkDisplayOptions = "close-right-panel";
            $scope.formPrime.$setPristine();
            $scope.formPrime.Code.$setValidity('exist', true);
        };

        // Action Delete
        $scope.handleClickDelete = function (prime) {
            $scope.checkDisplayOptions = "close-right-panel";
            confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationSuppression).then(function () {
                PrimeService.Delete(prime).then(function () {
                    $scope.actionLoad(false);
                    ProgressBar.complete();
                    $scope.actionCancel(true);
                }, function (reason) {

                    Notify.error(resources.Global_Notification_Suppression_Error);
                });
            });
        };

        // Action initalisation d'une nouvelle société
        $scope.actionNewPrime = function () {
            PrimeService.New($scope.societeId).then(function (response) {
                $scope.prime = response.data;
                if ($scope.isUserFes) {
                    $scope.prime.IsETAM = true;
                    $scope.prime.IsCadre = true;
                    $scope.prime.IsOuvrier = true;
                }
            }, function (reason) {

            });
        };

        // Action Load
        $scope.actionLoad = function (withNotif) {
            ProgressBar.start();
            PrimeService.Search($scope.filters, $scope.societeId, $scope.recherche).then(function (response) {
                $scope.items = response.data;
                if (response && response.data && response.data.length === 0) {
                    Notify.error(resources.Global_Notification_AucuneDonnees);
                }
            }, function (reason) {

                if (withNotif) {
                    Notify.error(resources.Global_Notification_Error);
                }
            });
            ProgressBar.complete();
        };

        // Action d'initialisation de la recherche muli-critère des sociétés
        $scope.actionInitSearch = function () {
            $scope.filters = { Code: true, Libelle: true };
        };

        // Action de test d'existence du code prime
        $scope.existCodeprime = function (idCourant, code, libelle) {
            PrimeService.Exists(idCourant, code, libelle).then(function (response) {
                if (response.data) {
                    $scope.formPrime.Code.$setValidity('exist', false);
                } else {
                    $scope.formPrime.Code.$setValidity('exist', true);
                }
            }, function (reason) {

            });
        };

        // Actions -Gestion des erreurs
        $scope.errorMessage = function (error) {
            var validationError = ModelStateErrorManager.getErrors(error);
            if (validationError) {
                return validationError;
            }
            else if (error.data.Message) {
                return error.data.Message;
            }
            else {
                return resources.Global_Notification_Error;
            }
        };
    }
})(angular);