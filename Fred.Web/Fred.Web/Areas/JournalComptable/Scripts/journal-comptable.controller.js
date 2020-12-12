(function (angular) {
    'use strict';

    angular.module('Fred').controller('JournalComptableController', JournalComptableController);

    JournalComptableController.$inject = ['$scope', 'Notify', 'JournalComptableService', 'ProgressBar', 'confirmDialog', 'UserService'];

    function JournalComptableController($scope, Notify, JournalComptableService, ProgressBar, confirmDialog, UserService) {
        // Instanciation Objet Ressources
        $scope.resources = resources;

        // Initialisation de la variable societeId
        $scope.societeId = undefined;

        // Instanciation de la recherche
        $scope.searchString = "";

        $scope.isAlreadyUsed = false;
        $scope.isBusy = false;

        // Attribut d'affichage de la liste
        $scope.checkDisplayOptions = "close-right-panel";
        $scope.checkFormatOptions = "small";

        // Actuellement l'import en masse des Journaux Comptable depuis ANAEL n'est que pour RAZEL BEC
        $scope.isJournauxImported = isJournauxImported();

        /**
         * Charge les journaux comptables d'une société
         */
        $scope.loadJournauxComptables = function () {
            $scope.societeId = $scope.societe.SocieteId;
            $scope.societeLibelle = $scope.societe.CodeLibelle;
            $scope.searchString = null;

            // Chargement des données
            actionLoad(true);
        };

        /**
         * Recherche d'un journal comptable dans la liste des journaux de la société sélectionnée
         * @param {any} searchString Texte recherché
         */
        $scope.searchJournalComptable = function (searchString) {
            $scope.searchString = searchString;
            actionLoad();
        };

        function isJournauxImported() {
            UserService.getCurrentUser().then(function(user) {
                if (user.Personnel.Societe.Groupe.Code.trim() === 'GRZB' || user.Personnel.Societe.Groupe.Code.trim() === 'MBTP') {
                    return true;
                }
                else {
                    return false;
                }
            });
        }

        function actionLoad(withNotif) {
            var filters = { Code: true, Libelle: true, SocieteId: $scope.societeId, ValueText: $scope.searchString, SearchExactly: false };
            JournalComptableService.GetFilteredJournalList(filters)
                .then(function (response) {
                    $scope.items = response.data;
                    if (response.data.length === 0) {
                        Notify.error(resources.Global_Notification_AucuneDonnees);
                    }
                }, function (reason) {
                    if (withNotif) Notify.error(resources.Global_Notification_Error);
                });
            ProgressBar.complete();
        }

        /**
         * Création d'un nouveau journal comptable
         */
        $scope.createNewJournal = function () {
            if ($scope.societeId !== undefined) {
                newJournalComptable();
                $scope.isAlreadyUsed = false;
                $scope.formJournalComptable.$setPristine();
                $scope.formJournalComptable.Code.$setValidity('exist', true);
                $scope.checkDisplayOptions = "open";
                $scope.changeFormModel = false;
            }
        };

        function newJournalComptable() {
            $scope.journalComptable = { JournalId: 0, SocieteId: $scope.societeId, Code: '', Libelle: '', IsActif: true };
        }

        /**
         * Check the unicity of the Code
         */
        $scope.codeChanged = function () {
            if (!$scope.formJournalComptable.Code.$error.pattern) {
                var idCourant = $scope.journalComptable.JournalId !== undefined ? $scope.journalComptable.JournalId : 0;

                if ($scope.societe !== null) { codeJournalExists(idCourant, $scope.journalComptable.Code); }
            }
        };

        function codeJournalExists(idCourant, codeNature) {
            var filters = { Code: true, Libelle: false, SocieteId: $scope.societeId, ValueText: $scope.journalComptable.Code, SearchExactly: true };
            JournalComptableService.GetFilteredJournalList(filters)
                .success(function (journaux) {
                    if (journaux !== undefined && journaux !== null && journaux.length > 0) {
                        $scope.formJournalComptable.Code.$setValidity('exist', false);
                    } else {
                        $scope.formJournalComptable.Code.$setValidity('exist', true);
                    }
                })
                .error(function (error) {
                    Notify.error(resources.Global_Notification_Error);
                })
                .finally();
        }

        /**
         * Sélectionne un journal comptable pour modification
         * @param {any} journal Journal sélectionné
         */
        $scope.selectJournal = function (journal) {
            $scope.journalComptable = angular.copy(journal);
            $scope.checkDisplayOptions = "open";
            $scope.changeFormModel = false;
            $scope.isAlreadyUsed = false;
            $scope.isBusy = true;
            JournalComptableService.isAlreadyUsed($scope.journalComptable.JournalId)
                .then((response) => {
                    $scope.isAlreadyUsed = response.data;
                }).catch((error) => {
                    Notify.defaultError();
                }).finally(() => {
                    $scope.isBusy = false;
                    ProgressBar.complete();
                });
        };

        /**
         * Sauvegarde d'un journal comptable
         * @param {bool} saveAndNewMode Indique si l'on permet la création d'un autre journal après l'enregistrement
         */
        $scope.clickSave = function (saveAndNewMode = false) {
            if ($scope.isAlreadyUsed) {
                confirmDialog.confirm($scope.resources, $scope.resources.Global_Modal_Item_Already_Used)
                    .then(function () {
                        SaveJournal(saveAndNewMode);
                    });
            } else {
                SaveJournal(saveAndNewMode);
            }
        };

        /**
         * Appels aux services d'enregistrement d'un journal
         * @param {boolean} saveAndNewMode Indique si l'on permet la création d'un autre journal après l'enregistrement
         */
        function SaveJournal(saveAndNewMode) {
            if ($scope.formJournalComptable.$invalid) {
                return;
            }

            var isNewJournal = $scope.journalComptable.JournalId === 0;
            JournalComptableService.SaveJournal($scope.journalComptable, isNewJournal)
                .success(function (journal) { SaveJournalSuccess(journal, isNewJournal, saveAndNewMode); })
                .error(function (error) { Notify.error(error); });
        }

        /**
         * Mise à jour de l'affichage suite à l'enregistrement d'un journal
         * @param {any} journal Journal enregistré
         * @param {boolean} isNewJournal Indique s'il s'agit d'un nouveau journal
         * @param {boolean} saveAndNewMode Indique si l'on permet la création d'un autre journal après l'enregistrement
         */
        function SaveJournalSuccess(journal, isNewJournal, saveAndNewMode) {
            if (isNewJournal) {
                $scope.items.push(journal);
            } else {
                var currentJournalIndex = $scope.items.findIndex(j => j.JournalId === journal.JournalId);
                $scope.items[currentJournalIndex] = journal;
            }

            if (saveAndNewMode) $scope.createNewJournal();
            else $scope.closeModificationPanel();

            Notify.message(resources.Global_Notification_Enregistrement_Success);
        }

        /**
         * Suppression du journal comptable sélectionné
         */
        $scope.clickDelete = function () {
            if ($scope.isAlreadyUsed) {
                Notify.error(resources.Global_Notification_Suppression_Error);
                return;
            }

            JournalComptableService.DeleteJournal($scope.journalComptable.JournalId)
                .success(function () { DeleteJournalSuccess(); })
                .error(function () { Notify.error(resources.Global_Notification_Suppression_Error); });
        };

        /**
         * Mise à jour de l'affichage suite à la suppression d'un journal
         */
        function DeleteJournalSuccess() {
            var journalDeletedIndex = $scope.items.findIndex(j => j.JournalId === $scope.journalComptable.JournalId);
            $scope.items.splice(journalDeletedIndex, 1);

            $scope.closeModificationPanel();

            Notify.message(resources.Global_Notification_Suppression_Success);
        }

        /**
         * Fermeture du panneau de création / modification
         */
        $scope.closeModificationPanel = function () {
            $scope.checkDisplayOptions = "close-right-panel";
            $scope.formJournalComptable.$setPristine();
            $scope.formJournalComptable.Code.$setValidity('exist', true);
            $scope.journalComptable = null;
            $scope.isAlreadyUsed = false;
        };
    }
})(angular);