(function (angular) {
    'use strict';

    angular.module('Fred').controller('RapportController', RapportController);

    RapportController.$inject = ['$scope', '$filter', '$timeout', 'Notify', 'confirmDialog', 'RapportService', 'BilanFlashService', 'ProgressBar', '$window', 'fredSubscribeService', '$q', '$location', 'PermissionsService', 'authorizationService', '$rootScope', 'UserService', 'RapportDuplicatorProccesService', 'FeatureFlags', '$uibModal'];

    function RapportController($scope, $filter, $timeout, Notify, confirmDialog, RapportService, BilanFlashService, ProgressBar, $window, fredSubscribeService, $q, $location, PermissionsService, authorizationService, $rootScope, UserService, RapportDuplicatorProccesService, FeatureFlags, $uibModal) {

        $scope.isCurrentUserFes = false;
        $scope.currentUsers = null;
        $scope.userOrganizationId = null;
        const codeSOMOPA = "0143";
        const roleLevels = {
            LevelCDC: 1,
            LevelCDT: 2,
            LevelDRC: 3,
            LevelCSP: 4,
            LevelGSP: 5
        };

        UserService.getCurrentUser().then(function (user) {
            $scope.isUserFes = user.Personnel.Societe.Groupe.Code.trim() === 'GFES' ? true : false;
            $scope.isUserFayatTP = user.Personnel.Societe.Groupe.Code.trim() === 'GFTP' ? true : false;
            $scope.materielTypeList = [
                { Id: 1, Libelle: "Interne" },
                { Id: 2, Libelle: "Externe" }
            ];

            $scope.currentUsers = user.Personnel;
            $scope.userOrganizationId = $scope.currentUsers.Societe.OrganisationId;
            if ($scope.currentUsers.Societe.Groupe.Code.trim() === "GFES") {
                $scope.isCurrentUserFes = true;
            }
            if ($scope.currentUsers.Societe.Code.trim() === codeSOMOPA) {
                $scope.isCurrentUserSOMOPA = true;
            }
        });

        function onScroll() {
            var scrollLeft = $(".rapport-table tbody").scrollLeft();

            $('.rapport-table thead').css("left", -scrollLeft); //fix the thead relative to the body scrolling
            $('.rapport-table tfoot').css("left", -scrollLeft); //fix the tfoot relative to the body scrolling

            for (var i = 1; i < 7; i++) {
                $('.rapport-table thead th:nth-child(' + i.toString() + ')').css("left", scrollLeft); //fix the first cell of the header
                $('.rapport-table tbody td:nth-child(' + i.toString() + ')').css("left", scrollLeft); //fix the first column of tdbody
                $('.rapport-table tfoot td:nth-child(' + i.toString() + ')').css("left", scrollLeft); //fix the first cell of the header
            }
        }

        //detect a scroll event on the tbody
        /*
           Setting the thead left value to the negative valule of tbody.scrollLeft will make it track the movement
           of the tbody element. Setting an elements left value to that of the tbody.scrollLeft left makes it maintain          
           it's relative position at the left of the table. 
        */

        $('.rapport-table tbody').scroll(onScroll);

        var element = angular.element(document.querySelector('.rapport-table'));
        var thead = angular.element(element[0].querySelector('.rapport-table thead'));
        var tbody = angular.element(element[0].querySelector('.rapport-table tbody'));
        var tfoot = angular.element(element[0].querySelector('.rapport-table tfoot'));

        /* -------------------------------------------------------------------------------------------------------------
         *                                            RESPONSIVE TABLEAU
         * -------------------------------------------------------------------------------------------------------------
         */
        function toResize(fullscreen) {
            var parentWidth = $window.innerWidth - 25;
            var bodyHeight = $window.innerHeight - $('#rapport-container').offset().top - $(thead).height() - $(tfoot).height() - 1;
            var parent = angular.element(document.querySelector('#rapport-container'));

            parent.css('margin-left', -30);

            if (fullscreen) {
                parent.css('margin-left', -10);
                parentWidth = parent.width() - 10;
            }

            /* Resize largeur du tableau en fonction la largeur de la page */
            $(element).width(parentWidth);

            // Resize largeur 
            $(thead).width(parentWidth);
            $(tbody).width(parentWidth);
            $(tfoot).width(parentWidth);

            // Resize hauteur body
            $(tbody).height(bodyHeight);
        }

        toResize();

        var throttleFn = throttle(toResize, 500, this);

        angular.element($window).on('resize', throttleFn);

        $scope.$on('$destroy', function unsubscribe() {
            angular.element($window).off('resize', throttleFn);
        });

        $scope.handleToggleFullscreen = function (fs) {
            $scope.isFullscreen = fs;
            $timeout(function () { toResize(fs); });
        };

        /* -------------------------------------------------------------------------------------------------------------
        *                                            INIT
        * -------------------------------------------------------------------------------------------------------------
        */

        // Instanciation Objet Ressources
        $scope.resources = resources;

        //Instanciation du compteur de lignes du rapport
        $scope.RapportLigneCounter = 0;

        // Rapport
        $scope.Rapport = {};
        $scope.ListCommentaires = new Array();
        $scope.listPersonnelDoublon = new Array();
        $scope.busy = false;

        $scope.CanBeLockedBeforeSave = true;
        $scope.ListPrimes = [];
        $scope.ListTaches = [];
        $scope.IsRoleChantier = false;
        $scope.IsRolePaie = false;
        $scope.IsGSP = false;
        $scope.IsWeekHoursMax = false;
        $scope.displayWeekHoursError = false;
        $scope.weekHoursError = new Array();
        $scope.IsFes = false;
        $scope.IsSOMOPA = false;
        $scope.PermissionDeplacementVisible = false;
        $scope.ListMajorations = [];
        $scope.IsMajorationMax = false;
        $scope.IsWorkHoursMax = false;
        $scope.tachenumber = 0;
        $scope.filter = {
            displayMaterial: true,
            displayCol: true,
            displayPersonnelDetail: true,
            CI: null
        };

        // Constantes
        // Statut ETAM
        $scope.StatutsETAM = ["2", "4", "5"];
        // Statut Ouvrier et ETAM
        $scope.StatutsCO = ["1", "3"];

        // Default Variable        
        $scope.warningList = new Array();
        $scope.errorList = new Array();
        $scope.isFullscreen = false;
        $scope.displayComments = false;
        $scope.HoraireDebutM = null;
        $scope.HoraireFinM = null;
        $scope.HoraireDebutS = null;
        $scope.HoraireFinS = null;
        $scope.Meteo = null;
        $scope.Evenements = null;
        // Propriétés à sommer
        $scope.summonProp = {
            HeureNormale: "HeureNormale",
            HeureMajoration: "HeureMajoration",
            HeureAbsence: "HeureAbsence",
            MaterielMarche: "MaterielMarche",
            MaterielArret: "MaterielArret",
            MaterielPanne: "MaterielPanne",
            MaterielIntemperie: "MaterielIntemperie",
            TotalHeureMateriel: "TotalHeureMateriel",
            TotalHeuresMajorees: "TotalHeuresMajorees"
        };

        $scope.ValidationProp = {
            IsWorkHoursMax: "IsWorkHoursMax",
            IsHouresMajorationMax: "IsHouresMajorationMax",
            IsMajorationMax: "IsMajorationMax",
            IsWorkHouresMax: "IsWorkHouresMax"
        };

        $scope.PrimeCodes = {
            PrimeES: "ES",
            PrimeINS: "INS",
            PrimeAE: "AE"
        };

        $scope.SaveEvenements = SaveEvenements;
        $scope.CancelEvenements = CancelEvenements;
        $scope.disableCodeZone = disableCodeZone;

        $scope.SaveButtonToolTip = resources.Global_Bouton_Enregistrer_Tooltip;

        $scope.addTempList = function () {
            if ($scope.Rapport.ListLignes) {
                $scope.Rapport.ListLignes.forEach(function (ligne) {
                    ligne.tempListRapportLigneTaches = JSON.parse(JSON.stringify(ligne.ListRapportLigneTaches));
                    ligne.tempListRapportLigneMajorations = JSON.parse(JSON.stringify(ligne.ListRapportLigneMajorations));
                    ligne.tempListRapportLignePrimes = JSON.parse(JSON.stringify(ligne.ListRapportLignePrimes));
                });
                reformDataAfterFill();
            }
        };

        /**
         * Controller scope initialization
         * @param {any} rapportId Rapport identifier
         * @param {any} duplicate Indicates id the rapport is duplicated
         */
        $scope.init = function (rapportId, duplicate) {
            $scope.permissionKeys = PERMISSION_KEYS;
            InitPermissionShowDeplacement();
            $scope.duplicate = duplicate;
            $scope.RapportId = rapportId;
            GetOrgnanisationId();
            // Fonction permettant de retourner à la liste des rapports      
            fredSubscribeService.subscribe({ eventName: 'goBack', callback: function () { window.location = '/Pointage/Rapport'; }, tooltip: "Retour à la liste des rapports" });

            if (rapportId === "" || rapportId === undefined) {
                $q.when()
                    .then(actionOnBegin)
                    .then(actionGetFilter)
                    .then(function () {
                        if ($scope.Rapport.CiId)
                            return actionNewRapport($scope.Rapport.CiId);
                        else
                            return actionNewRapport();
                    })
                    .finally(actionOnFinally);
            }
            else {
                $q.when()
                    .then(actionOnBegin)
                    .then(actionGetFilter)
                    .then(function () { return { RapportId: rapportId, Duplicate: duplicate, Validate: true }; })
                    .then(actionGetRapport)
                    .then(toResize)
                    .then(function () {
                        //ouverture du calendrier pour les rapports dupliqués uniquement
                        if ($scope.duplicate) {
                            actionOpenDateChantierDatePicker();
                        }
                    })
                    .finally(actionOnFinally);
            }

            $rootScope.$on('menuMode.changed', function (event, mode) {
                $timeout(function () { toResize(); });
            });
        };

        function InitPermissionShowDeplacement() {
            var rights = authorizationService.getRights($scope.permissionKeys.AffichageRapportJournalierDeplacement);
            if (rights) {
                $scope.PermissionDeplacementVisible = rights.isVisible;
            }
        }

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

        /* -------------------------------------------------------------------------------------------------------------
        *                                            HANDLERS
        * -------------------------------------------------------------------------------------------------------------
        */

        // Handler de click sur le bouton dupliquer un rapport
        $scope.handleClickDuplicateRapport = function () {
            $q.when()
                .then(actionOnBegin)
                .then(function () { return { RapportId: $scope.Rapport.RapportId, Duplicate: true, Validate: true }; })
                .then(actionGetRapport)
                .then(function () { $location.path('/Pointage/Rapport/New/'); })
                .then(actionOpenDateChantierDatePicker)
                .finally(actionOnFinally);
        };

        $scope.canDuplicateEnMasse = function () {
            return RapportDuplicatorProccesService.canDuplicateEnMasse($scope.IsRolePaie);
        };

        $scope.handleClickDuplicationEnMasseRapport = function () {
            $q.when()
                .then(function () {
                    return {
                        rapport: $scope.Rapport
                    };
                })
                .then(RapportDuplicatorProccesService.mustSaveModification)
                .then(function (result) {
                    if (result.askSave === true) {
                        return saveBeforeDuplicatePromise();
                    } else {
                        return duplicatePromise();
                    }
                });
        };

        function saveBeforeDuplicatePromise() {
            return $q.when()
                .then(function () {
                    return {
                        rapport: $scope.Rapport
                    };
                })
                .then(actionOnBegin)
                .then(actionSave)
                .catch(function (error) {
                    actionOnError(error);
                    throw new Error('exit process');
                })
                .then(function () {
                    return {
                        rapport: $scope.Rapport
                    };
                })
                // si pas d'erreur apres la sauvegarde,
                //je lance la duplication car la sauvegarde a normalement supprimer les erreurs
                .then(RapportDuplicatorProccesService.startDuplicationProcess)
                .finally(actionOnFinally);
        }

        function duplicatePromise() {
            return $q.when()
                .then(function () {
                    return {
                        rapport: $scope.Rapport
                    };
                })
                .then(RapportDuplicatorProccesService.startDuplicationProcess);
        }

        // Handler de click sur le bouton nouveau rapport
        $scope.handleClickNewRapport = function (CiId) {
            $q.when()
                .then(actionOnBegin)
                .then(function () { return CiId; })
                .then(actionNewRapport)
                .then(function () { $location.path('/Pointage/Rapport/New/'); })
                .finally(actionOnFinally);
        };

        // Handler de click sur le bouton Ajouter une ligne
        $scope.handleClickAddRow = function () {
            if (!$scope.rapportIsReadOnly()) {
                $q.when()
                    .then(actionOnBegin)
                    .then(actionAddRow)
                    .then(function () { $timeout(function () { onScroll(); }); })
                    .then($scope.addTempList)
                    .finally(actionOnFinally);
            }
        };

        // Handler de click sur le bouton Supprimer une ligne
        $scope.handleClickDeleteRow = function (ligne) {
            if (!$scope.Rapport.Cloture || !$scope.Rapport.Verrouille || !$scope.Rapport.ValidationSuperieur) {
                actionDeleteRow(ligne);
                actionRefreshRapportLigneCounter();
            }
        };

        // Handler de click sur le bouton Dupliquer une ligne
        $scope.handleClickDuplicateRow = function (ligne) {
            if (!$scope.Rapport.Cloture || !$scope.Rapport.Verrouille || !$scope.Rapport.ValidationSuperieur) {
                $q.when()
                    .then(function () { return ligne; })
                    .then(actionDuplicateRow)
                    .then(actionRefreshRapportLigneCounter)
                    .then(function () { $timeout(function () { onScroll(); }); });
            }
        };

        // Handler de click sur le bouton initializer une ligne
        $scope.handleClickInitializeRow = function (ligne) {
            if (!$scope.Rapport.Cloture || !$scope.Rapport.Verrouille || !$scope.Rapport.ValidationSuperieur) {
                confirmDialog.confirm($scope.resources, $scope.resources.Rapport_Modal_Initialisation_Ligne_Rapport)
                    .then(function () {
                        actionInitializeRow(ligne);
                        actionRefreshRapportLigneCounter();
                    });
            }
        };

        // Handler de click sur le bouton supprimer une prime
        $scope.handleClickDeletePrime = function (index) {
            if (!($scope.Rapport.Cloture || $scope.Rapport.ValidationSuperieur || $scope.Rapport.Verrouille && !$scope.IsRolePaie))
                actionDeletePrime(index);
        };

        // Handler de click sur le bouton supprimer une tache
        $scope.handleClickDeleteTache = function (index) {
            if (!$scope.rapportIsPartialLocked()) {
                actionDeleteTache(index);
            }
        };

        // Handler de click sur le bouton supprimer une majoration
        $scope.handleClickDeleteMajoration = function (index) {
            if (!($scope.Rapport.Cloture || $scope.Rapport.ValidationSuperieur || $scope.Rapport.Verrouille && !$scope.IsRolePaie))
                actionDeleteMajoration(index);
        };

        // Sauvegarde directe
        $scope.handleSave = function () {
            $q.when()
                .then(actionOnBegin)
                .then(actionSave)
                .catch(actionOnError)
                .finally(actionOnFinally);
        };

        // Sauvegarde après vérification
        $scope.handleClickSave = function () {
            if (!$scope.Rapport.CI) {
                Notify.warning($scope.resources.Rapport_Ci_Warning_Message_Before_Saving);
            }
            else {
                var result = actionCheckBeforeSave();
                if ($scope.Rapport.CI.Societe.TypeSociete.Code === 'SEP' && $scope.Rapport.CI.CompteInterneSepId === null) {
                    // RG_6472_007b
                    Notify.error($scope.resources.Personnel_ContratInterim_Error_CompteInterneSepId_IsNull);
                }
                else if (result.isPersonnelDoublon) {
                    $scope.popupDoublonPersonnel();
                }
                else if (result.isTacheVide) {
                    $scope.popupTacheVide();
                }
                else {
                    $scope.handleSave();
                }

            }
        };


        $scope.handleChangeDisplayMaterial = function () {
            $scope.filter.displayMaterial = !$scope.filter.displayMaterial;
            $scope.filter.displayCol = !$scope.filter.displayCol;
            sessionStorage.setItem('rapportDetailFilter', JSON.stringify($scope.filter));
        };

        $scope.handleChangeDisplayPersonnel = function () {
            $scope.filter.displayPersonnelDetail = !$scope.filter.displayPersonnelDetail;
            sessionStorage.setItem('rapportDetailFilter', JSON.stringify($scope.filter));
        };

        //************* PICKLIST HANDLERS *************//

        //Fonction de suppression du personnel sélectionné dans la picklist
        $scope.handleDeletePickListItemPersonnel = function () {
            var rapportLigne = $scope.RowPersoDeleted;
            rapportLigne.Personnel = null;
            rapportLigne.PersonnelId = null;
            rapportLigne.PrenomNomTemporaire = null;
            rapportLigne.PersonnelSelectionne = resources.Global_ReferentielPersonnel_Placeholder;
            rapportLigne.HeureNormale = 0;
            rapportLigne.HeureTotalTravail = 0;
            actionInitialiserMajoration(rapportLigne);
            actionInitialiserAbsence(rapportLigne);
            actionInitialiserDeplacement(rapportLigne);
            actionInitialiserListeDesPrimes(rapportLigne);
            actionIsUpdated(rapportLigne);
        };

        //Fonction de suppression du matériel sélectionné dans la picklist
        $scope.handleDeletePickListItemMateriel = function () {
            var rapportLigne = $scope.RowMaterielDeleted;
            actionInitialiserMateriel(rapportLigne);
            actionIsUpdated(rapportLigne);
            actionCalculateTotalHeureMateriel(rapportLigne);
        };

        //Fonction de suppression du ci sélectionné dans la picklist
        $scope.handleDeletePickListCI = function () {
            $scope.Rapport.CI = null;
            $scope.Rapport.CiId = 0;
        };

        ////Fonction de suppression du code majoration sélectionné dans la picklist
        $scope.handleDeletePickListCodeMajoration = function (rapportLigne) {
            rapportLigne.CodeMajorationId = null;
            rapportLigne.CodeMajoration = null;
            rapportLigne.CodeMajorationSelectionne = null;
            rapportLigne.CodeMajorationAbrege = null;
            actionIsUpdated(rapportLigne);
        };

        //Fonction de suppression du code absence sélectionné dans la picklist
        $scope.handleDeletePickListCodeAbsence = function (rapportLigne) {
            rapportLigne.CodeAbsenceId = null;
            rapportLigne.CodeAbsence = null;
            rapportLigne.CodeAbsenceSelectionne = null;
            rapportLigne.CodeAbsenceAbrege = null;
            rapportLigne.HeureAbsence = 0;
            rapportLigne.NumSemaineIntemperieAbsence = null;
            actionIsUpdated(rapportLigne);
        };

        //Fonction de suppression du code déplacement sélectionné dans la picklist
        $scope.handleDeletePickListCodeDeplacement = function (rapportLigne) {
            rapportLigne.CodeDeplacementId = null;
            rapportLigne.CodeDeplacement = null;
            rapportLigne.CodeDeplacementSelectionne = null;
            rapportLigne.CodeDeplacementAbrege = null;
            actionIsUpdated(rapportLigne);
        };

        //Fonction de suppression du code zone déplacement sélectionné dans la picklist
        $scope.handleDeletePickListCodeZoneDeplacement = function (rapportLigne) {
            rapportLigne.CodeZoneDeplacementId = null;
            rapportLigne.CodeZoneDeplacement = null;
            rapportLigne.CodeZoneDeplacementSelectionne = null;
            rapportLigne.CodeZoneDeplacementAbrege = null;
            actionIsUpdated(rapportLigne);
        };

        //************* CHANGES HANDLERS *************//

        // Handler changement de CI
        $scope.handleClickChangeCI = function () {
            if ($scope.Rapport.CiId > 0) {
                if (!($scope.Rapport.Cloture || $scope.Rapport.ValidationSuperieur || $scope.Rapport.Verrouille && !$scope.IsRolePaie))
                    return $scope.popupRemoveCI();
            }
            else {
                return $scope.handleLookupUrl('CI');
            }
        };

        // Handler changement du code majoration
        $scope.handleChangeCodeMajoration = function (ligne) { actionChangeCodeMajoration(ligne); };

        $scope.handleChangeHeureMajoration = function (ligne, ligneMajoration) {

            if (ligneMajoration) {
                ligne.ListRapportLigneMajorations.filter(i => i.CodeMajorationId === ligneMajoration.CodeMajorationId)[0].HeureMajoration = ligneMajoration.HeureMajoration;
            }

            actionCalculHeuresNormales(ligne);

            actionIsUpdated(ligne);
        };

        $scope.handleChangeHeureAbsence = function (ligne) { actionIsUpdated(ligne); };

        // Handler changement du code absence
        $scope.handleChangeCodeAbsence = function (ligne) { actionChangeCodeAbsence(ligne); };

        $scope.handleChangeAvecChauffeur = function (ligne) {
            if (ligne.AvecChauffeur === true) {
                $scope.RowPersoDeleted = ligne;
                $scope.handleDeletePickListItemPersonnel();
            }
            actionIsUpdated(ligne);
        };

        $scope.handleIsModified = function (ligne) { actionIsUpdated(ligne); };

        $scope.handleChangePrime = function (rapportLigne, lignePrime) {
            rapportLigne.ListRapportLignePrimes.filter(p => p.PrimeId === lignePrime.PrimeId)[0].IsChecked = lignePrime.IsChecked;
            rapportLigne.ListRapportLignePrimes.filter(p => p.PrimeId === lignePrime.PrimeId)[0].HeurePrime = lignePrime.HeurePrime;
            $scope.handleIsModified(rapportLigne);
        };

        $scope.handleRefreshIndemniteDeplacement = function (rapportLigne) {
            if (!$scope.personnelDataLocked(rapportLigne)) {
                $q.when()
                    .then(actionOnBegin)
                    .then(function () { return rapportLigne; })
                    .then(actionRefreshIndemniteDeplacement)
                    .finally(actionOnFinally);
            }
        };

        // Handler changement heure de tache
        $scope.handleChangeHeureTache = function (rapportLigne, ligneTache) {
            if (rapportLigne.ListRapportLigneTaches.length > 0) rapportLigne.ListRapportLigneTaches.filter(x => x.Tache.CodeRef === ligneTache.Tache.CodeRef)[0].HeureTache = ligneTache.HeureTache;
            if (!$scope.rapportIsReadOnly()) {
                actionCalculateHeuresTravailles(rapportLigne, $scope.Rapport.Verrouille && !$scope.IsRolePaie);
                actionCalculateTotalHeureMateriel(rapportLigne);
            }
            actionIsUpdated(rapportLigne);
        };

        // Handler changement du PrenomNomTemporaire
        $scope.handleChangePrenomNomTemporaire = function (rapportLigne) {
            actionChangePrenomNomTemporaire(rapportLigne);
            actionIsUpdated(rapportLigne);
        };

        // Handler changement du MaterielNomTemporaire
        $scope.handleChangeMaterielNomTemporaire = function (rapportLigne) {
            actionChangeMaterielNomTemporaire(rapportLigne);
            actionIsUpdated(rapportLigne);
        };

        //US 2436
        // Handler de click sur le bouton verrouiller le rapport
        $scope.handleVerrouillerRapport = function () {
            $q.when()
                .then(actionOnBegin)
                .then(actionVerrouillerRapport)
                .finally(actionOnFinally);
        };

        // Handler de click sur le bouton deverrouiller le rapport
        $scope.handleDeverrouillerRapport = function () {
            $q.when()
                .then(actionOnBegin)
                .then(actionDeverrouillerRapport)
                .finally(actionOnFinally);
        };

        // Action de suppression d'un rapport
        $scope.handleDeleteRapport = function () {
            $q.when()
                .then(actionOnBegin)
                .then(actionDeleteRapport)
                .finally(actionOnFinally);
        };

        /*
         * @description Gérer l'action ajouter une sortie astreinte
         */
        $scope.handleAddSortieAstreinte = function (rapportLigne) {
            var newSortieAstreinte = {
                RapportLigneAstreinteId: 0,
                RapportLigneId: rapportLigne.PointageId,
                AstreinteId: rapportLigne.AstreinteId,
                DateDebutAstreinte: moment($scope.Rapport.DateChantier),
                DateFinAstreinte: moment($scope.Rapport.DateChantier).add(1, "hours")
            };
            rapportLigne.ListRapportLigneAstreintes.push(newSortieAstreinte);
        };

        /*
         * @description Gérer l'action supprimer une sortie astreinte
         */
        $scope.handleDeleteSortieAstreinte = function (rapportLigne, ligneAstreinte) {
            var indexOfSortieAstreinteToDelete = rapportLigne.ListRapportLigneAstreintes.indexOf(ligneAstreinte);
            if (indexOfSortieAstreinteToDelete !== -1) {
                rapportLigne.ListRapportLigneAstreintes.splice(indexOfSortieAstreinteToDelete, 1);
            }
        };

        /*
         * @description Permet de récupérer le format HH:mm d'une date
         */
        $scope.handleGetAstreinteTime = function (astreinteDate) {
            if (astreinteDate) {
                return moment(astreinteDate).format('HH:mm');
            }
            return "";
        };

        /*
         * @description Gérer l'action modifier les dates d'une sortie astreinte
         */
        $scope.handleChangeAstreinteDates = function (rapportLigne, ligneAstreinte) {
            $scope.RapportLigneIndexToUpdateAstreinte = $scope.Rapport.ListLignes.indexOf(rapportLigne);
            $scope.AstreinteIndexToUpdate = rapportLigne.ListRapportLigneAstreintes.indexOf(ligneAstreinte);
            $("#changeAstreinteDateModal").modal('toggle');
        };

        /*
         * @description Permet de récupérer la date max de la fin d'une sortie astreinte
         */
        $scope.handleGetFinAstreinteMaxDate = function (date) {
            if (date) {
                return moment(date).add(1, 'days').set({ hour: 23, minute: 59 }).format("DD/MM/YYYY HH:mm");
            }
            return "";
        };

        /*
         * @description Permet de récupérer la date min de la début d'une sortie astreinte
         */
        $scope.handleGetDebutAstreinteMinDate = function (date) {
            if (date) {
                return moment(date).set({ hour: 0, minute: 0 }).format("DD/MM/YYYY HH:mm");
            }
            return "";
        };

        /*
         * @description Permet de récupérer la date max de la début d'une sortie astreinte
         */
        $scope.handleGetDebutAstreinteMaxDate = function (date) {
            if (date) {
                return moment(date).set({ hour: 23, minute: 45 }).format("DD/MM/YYYY HH:mm");
            }
            return "";
        };

        /*
         * @description Permet de vérifier la date fin d'une sortie astreinte lors de changement de la date début sortie
         */
        $scope.handleCheckAstreinteDateFin = function () {
            var dateDebutAstreinte = $scope.Rapport
                .ListLignes[$scope.RapportLigneIndexToUpdateAstreinte]
                .ListRapportLigneAstreintes[$scope.AstreinteIndexToUpdate]
                .DateDebutAstreinte;

            var dateFinAstreinte = $scope.Rapport
                .ListLignes[$scope.RapportLigneIndexToUpdateAstreinte]
                .ListRapportLigneAstreintes[$scope.AstreinteIndexToUpdate]
                .DateFinAstreinte;

            if (dateDebutAstreinte >= dateFinAstreinte) {
                $scope.Rapport.ListLignes[$scope.RapportLigneIndexToUpdateAstreinte].ListRapportLigneAstreintes[$scope.AstreinteIndexToUpdate].DateFinAstreinte = moment(dateDebutAstreinte).add(1, "hours");
                Notify.error(resources.Rapport_Warning_Astrieintes_Change);

            }

            CheckAtreinteHours();
        };

        //initialisation des libelles à affiché dans les directives picklistcaller
        function actionInitRapportLigneLibelle() {
            $scope.Rapport.ListLignes.forEach(function (element) {
                //Personnel        
                element.PersonnelSelectionne = element.Personnel && element.Personnel.PersonnelId ? element.Personnel.LibelleLong
                    : element.PrenomNomTemporaire !== null ? element.PrenomNomTemporaire : resources.Global_ReferentielPersonnel_Placeholder;


                //Materiel
                element.MaterielSelectionne = element.Materiel ? element.Materiel.LibelleLong
                    : element.MaterielNomTemporaire !== null ? element.MaterielNomTemporaire : resources.Global_ReferentielMateriel_Placeholder;
                //Code majoration
                //Libelle tooltip
                element.CodeMajorationSelectionne = element.CodeMajoration ? element.CodeMajoration.Code + " - " + element.CodeMajoration.Libelle : null;
                //libelle affiché
                element.CodeMajorationAbrege = element.CodeMajoration ? element.CodeMajoration.Code : null;

                //Code absence
                //libelle tooltip
                element.CodeAbsenceSelectionne = element.CodeAbsence ? element.CodeAbsence.Code + " - " + element.CodeAbsence.Libelle : null;
                //libelle affiché
                element.CodeAbsenceAbrege = element.CodeAbsence ? element.CodeAbsence.Code : null;

                //Code déplacement
                //libelle tooltip
                element.CodeDeplacementSelectionne = element.CodeDeplacement ? element.CodeDeplacement.Code + " - " + element.CodeDeplacement.Libelle : null;
                //libelle affiché
                element.CodeDeplacementAbrege = element.CodeDeplacement ? element.CodeDeplacement.Code : null;
                //Code zone déplacement
                //libelle tooltip
                element.CodeZoneDeplacementSelectionne = element.CodeZoneDeplacement ? element.CodeZoneDeplacement.Code + " - " + element.CodeZoneDeplacement.Libelle : null;
                //libelle affiché
                element.CodeZoneDeplacementAbrege = element.CodeZoneDeplacement ? element.CodeZoneDeplacement.Code : null;
            });
        }

        function actionInitPersonnelTemp() {
            $scope.Rapport.ListLignes.forEach(function (ligne) {
                if (ligne.PrenomNomTemporaire !== null && ligne.Personnel === null) {
                    ligne.Personnel = {
                        CodeRef: ligne.PrenomNomTemporaire,
                        LibelleRef: "",
                        LibelleLong: ligne.PrenomNomTemporaire
                    };
                }
            });
        }

        function actionInitMaterielTemp() {
            $scope.Rapport.ListLignes.forEach(function (ligne) {
                if (ligne.MaterielNomTemporaire !== null && ligne.Materiel === null) {
                    ligne.Materiel = {
                        CodeRef: ligne.MaterielNomTemporaire,
                        LibelleRef: "",
                        LibelleLong: ligne.MaterielNomTemporaire
                    };
                }
            });
        }

        // Action initialisation d'un nouveau rapport
        function actionNewRapport(CiId) {
            CiId = CiId || null;
            return RapportService.New(CiId)
                .then(function (value) { $scope.Rapport = value; })
                .then(actionConvertToLocaleDate)
                .then(actionApplyRGCIOnRapport)
                .then(actionActualiseIsFes)
                .then(actionActualiseIsSOMOPA)
                .then(actionInitRapportLigneLibelle)
                .then(actionRefreshRapportLigneCounter)
                .then(actionOpenDateChantierDatePicker)
                .catch(actionOnError);
        }

        //US 2436
        function actionVerrouillerRapport() {
            return RapportService.VerrouillerRapport($scope.Rapport)
                .then(function (value) { return { RapportId: $scope.Rapport.RapportId, Duplicate: false, Validate: false }; })
                .then(actionGetRapport)
                .then(function () { Notify.message(resources.Rapport_Detail_VerrouillerRapport); })
                .catch(actionOnError);
        }

        //US 2436
        function actionDeverrouillerRapport() {
            return RapportService.DeverrouillerRapport($scope.Rapport)
                .then(function () { return { RapportId: $scope.Rapport.RapportId, Duplicate: false, Validate: false }; })
                .then(actionGetRapport)
                .then(function () { Notify.message(resources.Rapport_Detail_DeverrouillerRapport); })
                .catch(actionOnError);
        }

        /**
         * Conversion des dates UTC en dates locales
         */
        function actionConvertToLocaleDate() {
            var isChrome = !!window.chrome && (!!window.chrome.webstore || !!window.chrome.runtime);
            if ($scope.Rapport) {
                InitEvenements();
                if ($scope.Rapport.DateModification !== null) { $scope.Rapport.DateModification = $filter('toLocaleDate')($scope.Rapport.DateModification); }
                if ($scope.Rapport.DateCreation !== null) { $scope.Rapport.DateCreation = $filter('toLocaleDate')($scope.Rapport.DateCreation); }
                if ($scope.Rapport.ListLignes) {
                    angular.forEach($scope.Rapport.ListLignes, function (ligne) {
                        if (ligne.ListRapportLigneAstreintes) {
                            angular.forEach(ligne.ListRapportLigneAstreintes, function (rapportLigneAstreinte) {
                                if (rapportLigneAstreinte.DateDebutAstreinte !== undefined || rapportLigneAstreinte.DateDebutAstreinte !== null) {
                                    if (isChrome) {
                                        rapportLigneAstreinte.DateDebutAstreinte = moment(rapportLigneAstreinte.DateDebutAstreinte).utc(false);
                                    }
                                    else {
                                        rapportLigneAstreinte.DateDebutAstreinte = moment($filter('toLocaleDate')(rapportLigneAstreinte.DateDebutAstreinte)).utc(false);
                                    }
                                }

                                if (rapportLigneAstreinte.DateFinAstreinte !== undefined || rapportLigneAstreinte.DateFinAstreinte !== null) {
                                    if (isChrome) {
                                        rapportLigneAstreinte.DateFinAstreinte = moment(rapportLigneAstreinte.DateFinAstreinte).utc(false);
                                    }
                                    else {
                                        rapportLigneAstreinte.DateFinAstreinte = moment($filter('toLocaleDate')(rapportLigneAstreinte.DateFinAstreinte)).utc(false);
                                    }
                                }
                            });
                        }
                    });
                }
            }
        }

        // Action de récupération d'un rapport existant
        function actionGetRapport(param) {
            return RapportService.Get(param.RapportId, param.Duplicate, param.Validate)
                .then(function (rapport) { actionProcessRapport(rapport, param.Duplicate); })
                .catch(actionOnError);
        }

        // Traite un rapport qui vient d'être chargé
        function actionProcessRapport(rapport, duplicate) {
            $q.when()
                .then(function () {
                    $scope.Rapport = rapport;
                    $scope.BeforeChangeRapportDate = rapport.DateChantier;
                    actionInitCommentaire($scope.Rapport.ListTaches);
                    $scope.ListPrimes = Array.from($scope.Rapport.ListPrimes);
                    $scope.ListTaches = Array.from($scope.Rapport.ListTaches);
                    $scope.CanBeLockedBeforeSave = $scope.isUserFayatTP ? $scope.Rapport.ListLignes.filter(rl => (rl.PersonnelId === null || rl.Personnel === null || rl.Personnel.PersonnelId === null || rl.Personnel.PersonnelId === undefined) && rl.PrenomNomTemporaire !== null && !rl.IsDeleted).length === 0 : true;
                    //calcule le nombre de taches 
                    $scope.tachenumber = $scope.ListTaches.length;

                    $scope.filter.CI = $scope.Rapport.CI;
                    sessionStorage.setItem('rapportDetailFilter', JSON.stringify($scope.filter));

                    reformDataAfterFill();


                    actionActualiseIsFes();
                    actionActualiseIsSOMOPA();
                    if ($scope.IsFes) {
                        $scope.ListMajorations = Array.from($scope.Rapport.ListMajorations);
                    }

                    for (var i = 0; i < $scope.Rapport.ListLignes.length; i++) {
                        var l = $scope.Rapport.ListLignes[i];
                        actionCalculateHeuresTravailles(l);
                        actionCalculateTotalHeureMateriel(l);
                    }
                    if (duplicate) Notify.message(resources.Rapport_RapportController_DupliquerRapport_Success);

                    // Toutes les actions qui suivent dépendent de $scope.Rapport, donc bien veillez à ce que cette variable ne soit pas nulle
                })
                .then(actionConvertToLocaleDate)
                .then(actionInitRapportLigneLibelle)
                .then(actionGetUserRoles)
                .then(actionRefreshRapportLigneCounter)
                .then(actionInitPersonnelTemp)
                .then(actionInitMaterielTemp)
                .then(actionSetBoldProperty)
                .then(actionGetContextualAuthorizationForValidateBtn);
        }

        /*
         * @description Obtenir la liste des autorisations contextuelles de l'utilisateur actuel
         */
        function actionGetContextualAuthorizationForValidateBtn() {
            return PermissionsService.getContextualAuthorization($scope.Rapport.CiId, "button.enabled.validate.rapport.index")
                .success(actionRegisterContextualPermission)
                .error(function () { Notify.error(resources.err_Authorizations); })
                .finally(function () {
                    $scope.$broadcast('refresh-fred-authorisation');
                });
        }

        function reformDataAfterFill() {
            $scope.Rapport.ListLignes.forEach(function (ligne) {
                ligne.tempListRapportLigneTaches = JSON.parse(JSON.stringify(ligne.ListRapportLigneTaches));
                if (ligne.tempListRapportLigneTaches.length < $scope.ListTaches.length) {

                    var tachesNotExist = $scope.ListTaches.filter(i => ligne.tempListRapportLigneTaches.every(elem => elem.Tache.Code !== i.Code));

                    tachesNotExist.forEach(function (tache) {
                        var rapportLigneTache = {
                            RapportLigneTacheId: 0,
                            RapportLigneId: ligne.RapportLigneId,
                            TacheId: tache.TacheId,
                            Tache: tache,
                            HeureTache: 0,
                            IsCreated: true,
                            IsUpdated: false,
                            IsDeleted: false,
                            IsTemp: true
                        };
                        var indexofTache = $scope.ListTaches.indexOf(tache);
                        ligne.tempListRapportLigneTaches.splice(indexofTache, 0, rapportLigneTache);
                    });
                }

                //temp for majoration
                ligne.tempListRapportLigneMajorations = JSON.parse(JSON.stringify(ligne.ListRapportLigneMajorations));
                if (ligne.ListRapportLigneMajorations.length < $scope.Rapport.ListMajorations.length) {
                    var MajorationNotExist = $scope.Rapport.ListMajorations.filter(i => ligne.tempListRapportLigneMajorations.every(elem => elem.CodeMajorationId !== i.CodeMajorationId));
                    MajorationNotExist.forEach(function (majoration) {
                        var rapportLigneMajoration = {
                            RapportLigneMajorationId: 0,
                            RapportLigneId: ligne.PointageId,
                            CodeMajorationId: majoration.CodeMajorationId,
                            CodeMajoration: majoration,
                            HeureMajoration: 0,
                            IsCreated: true,
                            IsUpdated: false,
                            IsDeleted: false,
                            IsTemp: true
                        };
                        var indexofMajoration = $scope.Rapport.ListMajorations.indexOf(majoration);
                        ligne.tempListRapportLigneMajorations.splice(indexofMajoration, 0, rapportLigneMajoration);


                    });
                }

                //temp for Prime
                ligne.tempListRapportLignePrimes = JSON.parse(JSON.stringify(ligne.ListRapportLignePrimes));
                if (ligne.tempListRapportLignePrimes.length < $scope.Rapport.ListPrimes.length) {
                    var primeNotExist = $scope.Rapport.ListPrimes.filter(i => ligne.tempListRapportLignePrimes.every(elem => elem.PrimeId !== i.PrimeId));
                    primeNotExist.forEach(function (prime) {
                        var rapportLignePrime = {
                            RapportLignePrimeId: 0,
                            RapportLigneId: ligne.RapportLigneId,
                            PrimeId: prime.PrimeId,
                            Prime: prime,
                            IsChecked: false,
                            HeurePrime: null,
                            IsCreated: true,
                            IsUpdated: false,
                            IsDeleted: false,
                            IsTemp: true
                        };
                        var indexofPrime = $scope.Rapport.ListPrimes.indexOf(prime);
                        ligne.tempListRapportLignePrimes.splice(indexofPrime, 0, rapportLignePrime);
                    });
                }
            });
        }

        function actionRegisterContextualPermission(modeAuthorization) {
            return PermissionsService.registerContextualPermission("button.enabled.validate.rapport.index", modeAuthorization);
        }
        // Action de création d'un rapport
        function actionSave() {
            return $q.when()
                .then(actionUpdateCommentaires)
                .then(actionAddOrUpdateRapport);
        }

        function actionAddOrUpdateRapport() {
            checkTimesOnSave();

            return RapportService.AddOrUpdateRapport($scope.Rapport)
                .then(function (rapport) {
                    $scope.listTachesVides = new Array();
                    return rapport;
                })
                .then(function (rapport) {
                    actionProcessRapport(rapport, false);
                })
                .then(function () {
                    modifyBrowserUrl($scope.Rapport.RapportId);
                    Notify.message(resources.Rapport_Detail_SaveRapport);
                })
                .catch(function (error) {
                    if (error.ModelState.Pointage && error.ModelState.Pointage[0].length > 0) {
                        Notify.error(error.ModelState.Pointage[0]);
                    } else if (error.ModelState.DateChantier && error.ModelState.DateChantier[0].length > 0) {
                        Notify.error(error.ModelState.DateChantier[0]);
                    } else if (error.ModelState.RapportExist && error.ModelState.RapportExist[0].length > 0) {
                        Notify.error(error.ModelState.RapportExist[0]);
                    }
                });
        }

        /*
         * @description Permet d'actualiser la valeur de IsFes
         */
        function actionActualiseIsFes() {
            if ($scope.Rapport.CI === null) {
                $scope.IsFes = $scope.isCurrentUserFes;
            }
            else {
                $scope.IsFes = $scope.Rapport.CI !== null && $scope.Rapport.CI.Societe !== null && $scope.Rapport.CI.Societe.Groupe !== null && $scope.Rapport.CI.Societe.Groupe.Code.trim() === resources.Code_Groupe_FES;
            }
        }

        /*
         * @description Permet d'actualiser la valeur de IsSOMOPA
         */
        function actionActualiseIsSOMOPA() {
            if ($scope.Rapport.CI === null) {
                $scope.IsSOMOPA = $scope.isCurrentUserSOMOPA;
            }
            else {
                $scope.IsSOMOPA = $scope.Rapport.CI !== null && $scope.Rapport.CI.Societe !== null && $scope.Rapport.CI.Societe.Code.trim() === codeSOMOPA;
            }
        }

        /*
         * @description Action de mise à jour de la date du rapport
         */
        function InitializeAstreintesInformations() {
            $q.when()
                .then(actionOnBegin)
                .then(function () {
                    return RapportService.InitializeAstreintesInformations($scope.Rapport)
                        .then(function () {
                            angular.forEach($scope.Rapport.ListLignes, function (ligne) {
                                ligne.ListRapportLigneAstreintes = [];
                            });
                        })
                        .catch(actionOnError);
                })
                .then(function () {
                    return RapportService.FulfillAstreintesInformations($scope.Rapport)
                        .then(function (value) {
                            $scope.Rapport.ListLignes = value.ListLignes;
                        });
                })
                .then(actionConvertToLocaleDate)
                .then(actionInitRapportLigneLibelle)
                .then(actionOnFinally)
                .then($scope.addTempList);
        }

        /**
         * Modification de l'URL en cas de création d'un nouveau rapport
         * @param {any} rapportId Identifiant du rapport
         * @returns {any} id
         */
        function modifyBrowserUrl(rapportId) {
            $location.path('/Pointage/Rapport/Detail/' + rapportId);
            return rapportId;
        }

        function actionGetObjectifsFlashListForSaisieQuantitesInRapport() {
            var rapportObjectifFlash = {
                RapportId: $scope.Rapport.RapportId,
                DateChantier: $scope.Rapport.DateChantier,
                CiId: $scope.Rapport.CiId,
                ListTaches: $scope.Rapport.ListTaches
            };
            // Récupérations Objectifs Flash associés au rapport
            return BilanFlashService.getObjectifsFlashListForRapport(rapportObjectifFlash)
                .then(getObjectifsFlashListForSaisieQuantitesInRapportOnSuccess)
                .then(function (data) {
                    saveObjectifFlashTacheRapportRealise(data);
                });
        }

        function getObjectifsFlashListForSaisieQuantitesInRapportOnSuccess(data) {

            return $q(function (resolve, reject) {
                // pas d'objectif flash pour le rapport, pas de popup affichée
                if (!data.data || data.data.length === 0) {
                    resolve();
                    return;
                }
                ProgressBar.complete();
                var modalInstance = $uibModal.open({
                    animation: true,
                    component: 'avancementBilanFlashComponent',
                    backdrop: 'static',
                    size: 'lg',
                    resolve: {
                        objectifFlashList: function () { return data.data; },
                        rapport: function () { return $scope.Rapport; },
                        resources: function () { return $scope.resources; }
                    }
                });

                modalInstance.result.then(function (data) {
                    ProgressBar.start();
                    resolve(data);
                }, function () {
                    actionOnFinally();
                });
            });
        }

        function saveObjectifFlashTacheRapportRealise(data) {
            if (data) {
                return BilanFlashService.saveObjectifFlashTacheRapportRealise($scope.Rapport.RapportId, data);
            }
        }

        $scope.handleCheckTacheVideAndSave = function () {
            if ($scope.listTachesVides !== undefined && $scope.listTachesVides.length > 0) {
                $scope.popupTacheVide();
            }
            else {
                $scope.handleSave();
            }
        };

        function actionUpdateCommentaires() {
            for (var i = 0; i < $scope.ListCommentaires.length; i++) {
                var commentaire = $scope.ListCommentaires[i];
                if (commentaire.Commentaire === "") {
                    commentaire.IsDeleted = true;
                }
            }
            $scope.Rapport.ListCommentaires = $scope.ListCommentaires.filter(commentaire => !(commentaire.IsDeleted && commentaire.RapportTacheId === 0));
        }

        function actionInitCommentaire(listTaches) {
            $scope.ListCommentaires = [];
            for (var i = 0; i < listTaches.length; i++) {
                var tache = listTaches[i];
                var commentaire = $scope.Rapport.ListCommentaires.filter(commentaire => commentaire.Tache.TacheId === tache.TacheId);
                if (commentaire.length === 0) {
                    $scope.ListCommentaires.push(actionGetNewCommentaire(0, $scope.Rapport.RapportId, tache.TacheId, tache.CodeLibelle, ""));
                }
                else {
                    var rapportTache = commentaire[0];
                    $scope.ListCommentaires.push(actionGetNewCommentaire(rapportTache.RapportTacheId, rapportTache.RapportId, rapportTache.TacheId, tache.CodeLibelle, rapportTache.Commentaire));
                }
            }
        }

        function actionGetNewCommentaire(rapportTacheId, rapportId, tacheId, codeLibelle, commentaire) {
            return { RapportTacheId: rapportTacheId, RapportId: rapportId, TacheId: tacheId, CodeLibelle: codeLibelle, Commentaire: commentaire, IsDeleted: false };
        }

        /**
         * Action ajouter un commentaire à une tâche
         * @param {any} tache tâche
         */
        function actionAddCommentaire(tache) {
            var rapportTache = $scope.Rapport.ListCommentaires.filter(commentaire => commentaire.Tache.TacheId === tache.TacheId);
            if (rapportTache.length === 0) {
                $scope.ListCommentaires.push(actionGetNewCommentaire(0, $scope.Rapport.RapportId, tache.TacheId, tache.CodeLibelle, ""));
            }
            else {
                rapportTache.IsDeleted = false;
            }
        }

        function actionValidationRapport() {
            return $q.when()
                .then(actionGetObjectifsFlashListForSaisieQuantitesInRapport)
                .then(actionCheckRapport)
                .then(function (rapport) {

                    // Validation du rapport si aucune erreur
                    if (rapport.ListErreurs && rapport.ListErreurs.length === 0 ||
                        !rapport.ListErreurs) {
                        actionUpdateCommentaires();

                        RapportService.ValidationRapport($scope.Rapport, true)
                            .then(function (value) {
                                // Retour à la liste des rapports
                                window.location = "/Pointage/Rapport/Index/validation_success";
                            })
                            .catch(actionOnError);
                    }
                    else {
                        actionAddOrUpdateRapport();
                    }
                })
                .catch(actionOnError);
        }

        function actionCheckRapport() {

            return RapportService.CheckRapport($scope.Rapport)
                .then(function (value) {
                    $scope.Rapport = value;
                    actionConvertToLocaleDate();
                    actionInitRapportLigneLibelle();

                    if ($scope.Rapport.ListErreurs.length > 0) {
                        Notify.error($scope.resources.Rapport_Detail_Error_Validation + ' ' + $scope.resources.Global_Control_Saisie_Erreur);
                    }
                    return $scope.Rapport;
                })
                .catch(actionOnError);
        }

        // Action ajout d'une ligne de rapport
        function actionAddRow() {
            if ($scope.RapportLigneCounter === 0) {
                return actionApplyRGCIOnRapport();
            }
            else {
                return RapportService.AddNewRapportLigne($scope.Rapport)
                    .then(function (value) { $scope.Rapport = value; })
                    .then(actionConvertToLocaleDate)
                    .then(actionInitRapportLigneLibelle)
                    .then(actionRefreshRapportLigneCounter)
                    .catch(actionOnError);
            }
        }

        // Action ajout d'une prime dans le rapport
        function actionAddPrime(prime) {
            // Test d'existance de la prime dans le rapport
            if (!actionContainsObject(prime, $scope.ListPrimes)) {

                // Test du dépassement du nombre de primes
                if ($scope.ListPrimes.length + 1 <= $scope.Rapport.NbMaxPrimes) {
                    if ($scope.IsFes) {
                        handlePrimeFesAfffectation(prime);
                    }
                    else {
                        affectPrime(prime);
                    }
                }
                else {
                    Notify.warning($scope.resources.Rapport_RapportController_NbrMaxPrime_Info);
                }
            }
            else {
                Notify.warning($scope.resources.Rapport_RapportController_PrimeExisteDeja_Info);
            }
        }

        // Action ajout d'une majoration dans le rapport
        function actionAddMajoration(majoration) {
            // Test d'existance de la majoration dans le rapport
            if (!actionContainsObject(majoration, $scope.ListMajorations)) {

                // Test du dépassement du nombre des majorations
                if ($scope.ListMajorations.length + 1 <= $scope.Rapport.NbMaxMajorations) {
                    $scope.ListMajorations.push(majoration);
                    for (var i = 0; i < $scope.Rapport.ListLignes.length; i++) {
                        var ligne = $scope.Rapport.ListLignes[i];
                        var rapportLigneMajoration = {
                            RapportLigneMajorationId: 0,
                            RapportLigneId: ligne.PointageId,
                            CodeMajorationId: majoration.CodeMajorationId,
                            CodeMajoration: majoration,
                            HeureMajoration: 0,
                            IsCreated: true,
                            IsUpdated: false,
                            IsDeleted: false
                        };
                        ligne.ListRapportLigneMajorations.push(rapportLigneMajoration);
                        ligne.tempListRapportLigneMajorations.push(rapportLigneMajoration);
                    }
                }
                else {
                    Notify.warning($scope.resources.Rapport_RapportController_NbrMaxMajoration_Info);
                }
            }
            else {
                Notify.warning($scope.resources.Rapport_RapportController_MajorationExisteDeja_Info);
            }
        }

        // Action ajout d'une tache dans le rapport
        function actionAddTache(tache) {
            // Test d'existance de la prime dans le rapport
            if (!actionContainsObject(tache, $scope.ListTaches)) {

                // Test du dépassement du nombre de Taches
                $scope.ListTaches.push(tache);
                for (var i = 0; i < $scope.Rapport.ListLignes.length; i++) {
                    var ligne = $scope.Rapport.ListLignes[i];
                    var rapportLigneTache = {
                        RapportLigneTacheId: 0,
                        RapportLigneId: ligne.RapportLigneId,
                        TacheId: tache.TacheId,
                        Tache: tache,
                        HeureTache: 0,
                        IsCreated: true,
                        IsUpdated: false,
                        IsDeleted: false
                    };
                    ligne.ListRapportLigneTaches.push(rapportLigneTache);
                    ligne.tempListRapportLigneTaches.push(rapportLigneTache);
                }
                actionAddCommentaire(tache);
            }
            else {
                Notify.warning($scope.resources.Rapport_RapportController_TacheExisteDeja_Info);
            }
        }

        function actionDeleteRapport() {
            return RapportService.DeleteRapport($scope.Rapport)
                .then(function (value) {
                    // Retour à la liste des rapports
                    window.location = "/Pointage/Rapport/Index/delete_success";
                })
                .catch(actionOnError);
        }

        // Action suppression d'une ligne de rapport
        function actionDeleteRow(ligne) {
            if (ligne.PointageId === undefined || ligne.PointageId === 0 || ligne.PointageId === null) {
                var index = $scope.Rapport.ListLignes.indexOf(ligne);
                $scope.Rapport.ListLignes.splice(index, 1);
            }
            ligne.IsChecked = false;
            ligne.IsCreated = false;
            ligne.IsUpdated = false;
            ligne.IsDeleted = true;

            if ($scope.Rapport.ListLignes) {
                $scope.CanBeLockedBeforeSave = $scope.isUserFayatTP ? $scope.Rapport.ListLignes.filter(rl => (rl.PersonnelId === null || rl.Personnel === null || rl.Personnel.PersonnelId === null || rl.Personnel.PersonnelId === undefined) && rl.PrenomNomTemporaire !== null && !rl.IsDeleted).length === 0 : true;
                $scope.Rapport.CanBeLocked = $scope.isUserFayatTP ? $scope.CanBeLockedBeforeSave : true;
            }
        }

        /*
         * @description Action pour initialiser une ligne de rapport
         */
        function actionInitializeRow(ligne) {
            actionInitialiserMajoration(ligne);
            actionInitialiserAbsence(ligne);
            actionInitialiserDeplacement(ligne);
            actionInitialiserListeDesPrimes(ligne);
            actionInitialiserListeDesTaches(ligne);
            actionInitialiserMateriel(ligne);
            ligne.ListRapportLigneAstreintes = [];
            // Initialisation des heures de travail
            ligne.HeureNormale = 0;
            ligne.HeureTotal = 0;
            ligne.HeureTotalTravail = 0;
        }

        /*
         * @description action initialiser l'absence d'une ligne de rapport
         */
        function actionInitialiserAbsence(ligneRapport) {
            if (ligneRapport) {
                ligneRapport.CodeAbsence = null;
                ligneRapport.CodeAbsenceSelectionne = null;
                ligneRapport.CodeAbsenceAbrege = null;
                ligneRapport.CodeAbsenceId = null;
                ligneRapport.HeureAbsence = 0;
                ligneRapport.NumSemaineIntemperieAbsence = null;
            }
        }

        /*
         * @description action initialiser les deplacements d'une ligne de rapport
         */
        function actionInitialiserDeplacement(ligneRapport) {
            ligneRapport.CodeDeplacementId = null;
            ligneRapport.CodeDeplacement = null;
            ligneRapport.CodeDeplacementSelectionne = null;
            ligneRapport.CodeDeplacementAbrege = null;
            ligneRapport.CodeZoneDeplacementId = null;
            ligneRapport.CodeZoneDeplacement = null;
            ligneRapport.CodeZoneDeplacementSelectionne = null;
            ligneRapport.CodeZoneDeplacementAbrege = null;
            ligneRapport.DeplacementIV = false;
        }
        /*
         * @description action initialiser majoration d'une ligne de rapport
         */
        function actionInitialiserMajoration(ligneRapport) {
            if (ligneRapport && ligneRapport.ListRapportLigneMajorations && ligneRapport.ListRapportLigneMajorations.length > 0) {
                angular.forEach(ligneRapport.ListRapportLigneMajorations, function (value) {
                    if (!value.IsDeleted) {
                        value.HeureMajoration = 0;
                    }
                });

                for (var i = 0; i < $scope.Rapport.ListLignes.length; i++) {
                    var ligne = $scope.Rapport.ListLignes[i];
                    actionCalculHeuresNormales(ligne);
                }
            }
            else {
                ligneRapport.CodeMajoration = null;
                ligneRapport.CodeMajorationId = null;
                ligneRapport.CodeMajorationAbrege = null;
                ligneRapport.CodeMajorationField = null;
                ligneRapport.HeureMajoration = 0;
            }
        }

        /*
         * @description action initialiser la liste des primes d'une ligne de rapport
         */
        function actionInitialiserListeDesPrimes(ligneRapport) {
            if (ligneRapport && ligneRapport.ListRapportLignePrimes) {
                angular.forEach(ligneRapport.ListRapportLignePrimes, function (value) {
                    if (!value.IsDeleted) {
                        value.HeurePrime = null;
                        value.IsChecked = false;
                    }
                });
            }
        }

        /*
         * @description action initialiser la liste des taches d'une ligne de rapport
         */
        function actionInitialiserListeDesTaches(ligneRapport) {
            if (ligneRapport && ligneRapport.ListRapportLigneTaches) {
                angular.forEach(ligneRapport.ListRapportLigneTaches, function (value) {
                    if (!value.IsDeleted) {
                        value.HeureTache = 0;
                    }
                });
            }
        }

        /*
         * @description action initialiser les pointages material d'une ligne de rapport
         */
        function actionInitialiserMateriel(ligneRapport) {
            if (ligneRapport) {
                ligneRapport.MaterielId = null;
                ligneRapport.Materiel = null;
                ligneRapport.MaterielNomTemporaire = null;
                ligneRapport.MaterielSelectionne = resources.Global_ReferentielMateriel_Placeholder;
                ligneRapport.MaterielMarche = 0;
                ligneRapport.MaterielArret = 0;
                ligneRapport.MaterielPanne = 0;
                ligneRapport.MaterielIntemperie = 0;
                ligneRapport.AvecChauffeur = false;
            }
        }

        // Action de suppresion des toutes les lignes du tableau = f
        function actionDeleteFieldLinkWithCI() {
            $scope.Rapport.ValideurCDC = null;
            $scope.Rapport.ValideurCDT = null;
            $scope.Rapport.ValideurDRC = null;
            $scope.Rapport.ValideurGSP = null;
            $scope.Rapport.ListLignes = [];
        }

        // Action suppression d'une prime
        function actionDeletePrime(index) {
            var prime = $scope.ListPrimes[index];
            $scope.ListPrimes.splice(index, 1);

            for (var i = 0; i < $scope.Rapport.ListLignes.length; i++) {
                var ligne = $scope.Rapport.ListLignes[i];

                //delete from temp list primes
                ligne.tempListRapportLignePrimes.forEach(function (templignePrime, tmpIndex) {
                    if (templignePrime.PrimeId === prime.PrimeId) {
                        ligne.tempListRapportLignePrimes.splice(tmpIndex, 1);
                    }
                });

                // Je ne sors pas de la boucle tant que toutes les primes n'ont pas été testées car dans la saisie d'un rapport je peux ajouter
                // une prime, la supprimer, mais en fait la rajouter donc j'ai potentiellement dans ma liste plusieurs fois la même prime
                // mais avec les attributs de CRUD différents
                for (var j = 0; j < ligne.ListRapportLignePrimes.length; j++) {
                    var lignePrime = ligne.ListRapportLignePrimes[j];

                    if (lignePrime.PrimeId === prime.PrimeId) {
                        if (lignePrime.IsCreated) {
                            ligne.ListRapportLignePrimes.splice(j, 1);
                        }
                        else if (!lignePrime.IsDeleted) {
                            lignePrime.IsDeleted = true;

                            // Si une prime a une heure de renseignée ou si elle est cochée, on met la date de modif de la ligne à jour
                            if (lignePrime.HeurePrime > 0 || lignePrime.IsChecked) {
                                actionIsUpdated(ligne);
                            }
                        }
                    }
                }
            }
        }

        // Action suppression d'une tâche
        function actionDeleteTache(index) {
            var tache = $scope.ListTaches[index];
            $scope.ListTaches.splice(index, 1);
            $scope.ListCommentaires[index].IsDeleted = true;

            for (var j = 0; j < $scope.Rapport.ListLignes.length; j++) {
                var ligne = $scope.Rapport.ListLignes[j];
                //delete from temp list taches
                ligne.tempListRapportLigneTaches.forEach(function (templigneTache, tmpIndex) {
                    if (templigneTache.TacheId === tache.TacheId) {
                        ligne.tempListRapportLigneTaches.splice(tmpIndex, 1);
                    }
                });
                for (var k = 0; k < ligne.ListRapportLigneTaches.length; k++) {
                    var ligneTache = ligne.ListRapportLigneTaches[k];

                    if (ligneTache.TacheId === tache.TacheId) {
                        if (ligneTache.IsCreated) {
                            ligne.ListRapportLigneTaches.splice(k, 1);
                        }
                        else if (!ligneTache.IsDeleted) {
                            ligneTache.IsDeleted = true;

                            // Si une tâche a une heure de renseignée, on met la date de modif de la ligne à jour
                            if (ligneTache.HeureTache > 0) {
                                actionIsUpdated(ligne);
                            }
                        }
                    }
                }
                actionCalculateHeuresTravailles(ligne, $scope.Rapport.Verrouille && !$scope.IsRolePaie);
                actionCalculateTotalHeureMateriel(ligne);
            }
        }

        function actionDeleteMajoration(index) {
            var majoration = $scope.ListMajorations[index];
            $scope.ListMajorations.splice(index, 1);

            for (var i = 0; i < $scope.Rapport.ListLignes.length; i++) {
                var ligne = $scope.Rapport.ListLignes[i];

                ligne.tempListRapportLigneMajorations.forEach(function (templigneMaj, tmpIndex) {
                    if (templigneMaj.CodeMajorationId === majoration.CodeMajorationId) {
                        ligne.tempListRapportLigneMajorations.splice(tmpIndex, 1);
                    }
                });

                for (var j = 0; j < ligne.ListRapportLigneMajorations.length; j++) {
                    var ligneMajoration = ligne.ListRapportLigneMajorations[j];
                    if (ligneMajoration.CodeMajorationId === majoration.CodeMajorationId) {
                        if (ligneMajoration.IsCreated) {
                            ligne.ListRapportLigneMajorations.splice(j, 1);
                        }
                        else if (!ligneMajoration.IsDeleted) {
                            ligneMajoration.IsDeleted = true;

                            // Si une majoration a une heure de renseignée ou si elle est cochée, on met la date de modif de la ligne à jour
                            if (ligneMajoration.HeureMajoration > 0) {
                                actionIsUpdated(ligne);
                            }
                        }
                    }
                }

                actionCalculHeuresNormales(ligne);
            }
        }

        // Action duplication d'une ligne de rapport
        function actionDuplicateRow(ligne) {
            ligne.IsChecked = false;

            var ligneToDuplicate = JSON.parse(JSON.stringify(ligne));

            ligneToDuplicate.IsChecked = false;

            // On met l'ID technique de la ligne à 0
            ligneToDuplicate.PointageId = 0;
            ligneToDuplicate.RapportLigneId = 0;
            ligneToDuplicate.IsDeleted = false;
            ligneToDuplicate.IsCreated = true;
            ligneToDuplicate.IsUpdated = false;

            // On met l'ID technique des primes à 0
            for (var i = 0; i < ligneToDuplicate.ListRapportLignePrimes.length; i++) {
                var lignePrime = ligneToDuplicate.ListRapportLignePrimes[i];

                lignePrime.RapportLignePrimeId = 0;
                lignePrime.IsDeleted = ligne.ListRapportLignePrimes[i].IsDeleted;
                lignePrime.IsCreated = true;
                lignePrime.IsUpdated = false;
            }

            // On met l'ID technique des taches à 0
            for (var j = 0; j < ligneToDuplicate.ListRapportLigneTaches.length; j++) {
                var ligneTache = ligneToDuplicate.ListRapportLigneTaches[j];

                ligneTache.RapportLigneTacheId = 0;
                ligneTache.IsDeleted = ligne.ListRapportLigneTaches[j].IsDeleted;
                ligneTache.IsCreated = true;
                ligneTache.IsUpdated = false;
            }

            if ($scope.IsFes) {
                for (var k = 0; k < ligneToDuplicate.ListRapportLigneMajorations.length; k++) {
                    var ligneMajoration = ligneToDuplicate.ListRapportLigneMajorations[k];
                    ligneMajoration.RapportLigneMajorationId = 0;
                    ligneMajoration.IsDeleted = ligne.ListRapportLigneMajorations[k].IsDeleted;
                    ligneMajoration.IsCreated = true;
                    ligneMajoration.IsUpdated = false;
                }
            }

            $scope.Rapport.ListLignes.push(ligneToDuplicate);
        }

        function actionCheckBeforeSave() {
            $scope.listPersonnelDoublon = new Array();
            $scope.listTachesVides = new Array();
            var tblNoms = new Array();
            var tblNomsDoublons = new Array();

            for (var i = 0; i < $scope.Rapport.ListLignes.length; i++) {
                if (!$scope.Rapport.ListLignes[i].IsDeleted) {
                    var personnel = $scope.Rapport.ListLignes[i].Personnel;
                    if (personnel) {

                        if (i > 0 && tblNoms.indexOf(personnel.PersonnelId) !== -1 && tblNomsDoublons.indexOf(personnel.PersonnelId) === -1) {
                            tblNomsDoublons.push(personnel.PersonnelId);
                            $scope.listPersonnelDoublon.push(personnel.MatriculeNomPrenom);
                        } else {
                            if (tblNoms.indexOf(personnel.PersonnelId) === -1)
                                tblNoms.push(personnel.PersonnelId);
                        }
                    }
                }
            }
            actionEmptyTache();

            return { isPersonnelDoublon: $scope.listPersonnelDoublon.length > 0, isTacheVide: $scope.listTachesVides.length > 0 };
        }

        function actionEmptyTache() {
            $scope.listTachesVides = new Array();
            var ligne = null;
            var listTache = new Array();

            for (var i = 0; i < $scope.Rapport.ListLignes.length; i++) {
                if (!$scope.Rapport.ListLignes[i].IsDeleted) {
                    ligne = $scope.Rapport.ListLignes[i];
                    break;
                }
            }
            if (ligne) {
                for (var j = 0; j < ligne.ListRapportLigneTaches.length; j++) {
                    if (!ligne.ListRapportLigneTaches[j].IsDeleted) {
                        listTache.push({ TacheId: ligne.ListRapportLigneTaches[j].TacheId, Tache: ligne.ListRapportLigneTaches[j].Tache, empty: true });
                    }
                }
                for (var k = 0; k < $scope.Rapport.ListLignes.length; k++) {
                    if (!$scope.Rapport.ListLignes[k].IsDeleted) {
                        var currentLigne = $scope.Rapport.ListLignes[k];
                        for (var l = 0; l < currentLigne.ListRapportLigneTaches.length; l++) {
                            if (!currentLigne.ListRapportLigneTaches[l].IsDeleted && currentLigne.ListRapportLigneTaches[l].HeureTache > 0) {
                                var tache = null;
                                tache = listTache.find((element) => element.TacheId === currentLigne.ListRapportLigneTaches[l].TacheId);
                                if (tache !== null) {
                                    tache.empty = false;
                                }
                            }
                        }
                    }
                }
                for (var m = 0; m < listTache.length; m++) {
                    if (listTache[m].empty) {
                        $scope.listTachesVides.push(listTache[m].Tache);
                    }
                }
            }
        }

        //Fonction de chargement des données de l'item sélectionné dans la picklist
        $scope.handleLookupSelection = function (type, item, pointage) {
            if (item) {
                switch (type) {
                    case "CodeAbsence":
                        pointage.CodeAbsence = item;
                        pointage.HeureAbsence = $scope.HeuresAbsenceDefaut(pointage);
                        pointage.CodeAbsenceId = item.IdRef;
                        pointage.CodeAbsenceField = item.Code;

                        pointage.CodeAbsenceSelectionne = item.Code + " - " + item.Libelle;
                        pointage.CodeAbsenceAbrege = item.Code;

                        $q.when()
                            .then(actionOnBegin)
                            .then(function () { return pointage; })
                            .then(actionApplyRGCodeAbsenceOnPointage)
                            .finally(actionOnFinally);
                        break;
                    case "CodeDeplacement":
                        pointage.CodeDeplacement = item;
                        pointage.CodeDeplacementId = item.IdRef;
                        pointage.CodeDeplacementSelectionne = item.Code + " - " + item.Libelle;
                        pointage.CodeDeplacementAbrege = item.Code;
                        break;
                    case "CodeZoneDeplacement":
                        pointage.CodeZoneDeplacement = item;
                        pointage.CodeZoneDeplacementId = item.IdRef;
                        pointage.CodeZoneDeplacementSelectionne = item.Code + " - " + item.Libelle;
                        pointage.CodeZoneDeplacementAbrege = item.Code;
                        break;
                    case "Personnel":
                        pointage.CodeDeplacementId = null;
                        pointage.CodeDeplacement = null;
                        pointage.CodeDeplacementSelectionne = null;
                        pointage.CodeDeplacementAbrege = null;
                        pointage.CodeZoneDeplacementId = null;
                        pointage.CodeZoneDeplacement = null;
                        pointage.CodeZoneDeplacementSelectionne = null;
                        pointage.CodeZoneDeplacementAbrege = null;
                        pointage.DeplacementIV = false;
                        pointage.CodeAbsence = null;
                        pointage.CodeAbsenceAbrege = null;
                        pointage.HeureAbsence = 0;
                        pointage.CodeAbsenceId = null;


                        if (item.IdRef) {

                            $q.when()
                                .then(actionOnBegin)
                                .then(function () {
                                    pointage.Personnel = item;
                                    pointage.PersonnelId = item.IdRef;
                                    pointage.PrenomNomTemporaire = item.LibelleRef;
                                    pointage.Personnel.SocieteCode = item.Societe.Code;
                                    pointage.Personnel.Matricule = item.Matricule;
                                    pointage.CI = $scope.Rapport.CI;

                                    pointage.PersonnelSelectionne = item.LibelleLong;

                                    pointage.CI = $scope.Rapport.CI;

                                    $scope.CanBeLockedBeforeSave = true;
                                    $scope.Rapport.CanBeLocked = $scope.isUserFayatTP ? $scope.Rapport.ListLignes.filter(rl => rl.PointageId !== pointage.PointageId && rl.PersonnelId === null && rl.PrenomNomTemporaire !== null && !rl.IsDeleted).length === 0 : true;
                                    actionCalculateHeuresTravailles(pointage);
                                    actionCalculateTotalHeureMateriel(pointage);

                                    return pointage;
                                })
                                .then(actionGetOrCreateIndemniteDeplacement)
                                .then(function () { return pointage; })
                                .then(actionSetMateriel)
                                .finally(actionOnFinally);
                        }
                        else {
                            pointage.PrenomNomTemporaire = item.CodeRef;
                            pointage.PersonnelSelectionne = item.CodeRef;
                            $scope.CanBeLockedBeforeSave = $scope.isUserFayatTP ? false : true;
                            actionCalculateHeuresTravailles(pointage);
                            actionCalculateTotalHeureMateriel(pointage);
                        }
                        break;
                    case "CodeMajoration":
                        pointage.CodeMajoration = item;
                        pointage.CodeMajorationId = item.IdRef;
                        pointage.CodeMajorationField = item.Code;
                        pointage.CodeMajorationSelectionne = item.Code + " - " + item.Libelle;
                        pointage.CodeMajorationAbrege = item.Code;
                        break;
                    case "ListCodeMajoration":
                        actionAddMajoration(item);
                        break;
                    case "Prime":
                        actionAddPrime(item);
                        break;
                    case "Tache":
                        actionAddTache(item);
                        break;
                    case "CI":
                        if (FeatureFlags.getFlagStatus('ModificationCiRapportJournalier')) {
                            if ($scope.Rapport.CiId && $scope.Rapport.CiId !== item.CiId) {
                                var modalInstance = $uibModal.open({
                                    animation: true,
                                    component: 'confirmationModificationCiComponent',
                                    backdrop: 'static',
                                    resolve: {
                                        resources: function () { return $scope.resources; }
                                    }
                                });

                                modalInstance.result.then(function () {
                                    $scope.Rapport.CiId = item.CiId;
                                    $scope.Rapport.CI = item;
                                    $scope.filter.CI = item;
                                    sessionStorage.setItem('rapportDetailFilter', JSON.stringify($scope.filter));
                                    $scope.busy = true;
                                    ProgressBar.start();
                                    RapportService.DuplicateForNewCI($scope.Rapport).then(function (response) {
                                        $scope.Rapport = response.data;
                                        $scope.ListPrimes = Array.from($scope.Rapport.ListPrimes);
                                        $scope.ListTaches = Array.from($scope.Rapport.ListTaches);
                                        handleInitHorairesChantier();
                                    })
                                        .then(actionInitRapportLigneLibelle)
                                        .then(actionRefreshRapportLigneCounter)
                                        .then($scope.addTempList)
                                        .catch(function (error) { console.log(error); })
                                        .finally(actionOnFinally);
                                });
                            } else if (!$scope.Rapport.CiId) {
                                $q.when()
                                    .then(actionOnBegin)
                                    .then(actionDeleteFieldLinkWithCI)
                                    .then(function () {
                                        $scope.Rapport.CiId = item.CiId;
                                        $scope.Rapport.CI = item;
                                        $scope.filter.CI = item;
                                        sessionStorage.setItem('rapportDetailFilter', JSON.stringify($scope.filter));
                                    })
                                    .then(actionGetRole)
                                    .then(actionApplyRGCIOnRapport)
                                    .then(toResize)
                                    .then(function () { $timeout(function () { onScroll(); }); })
                                    .finally(actionOnFinally);
                            }
                        } else {
                            $q.when()
                                .then(actionOnBegin)
                                .then(actionDeleteFieldLinkWithCI)
                                .then(function () {
                                    $scope.Rapport.CiId = item.CiId;
                                    $scope.Rapport.CI = item;
                                    $scope.filter.CI = item;
                                    sessionStorage.setItem('rapportDetailFilter', JSON.stringify($scope.filter));
                                })
                                .then(actionGetRole)
                                .then(actionApplyRGCIOnRapport)
                                .then(toResize)
                                .then(function () { $timeout(function () { onScroll(); }); })
                                .finally(actionOnFinally);
                        }
                        break;
                    case "Materiel":
                        if (item.MaterielId) {
                            pointage.Materiel = item;
                            pointage.MaterielId = item.MaterielId;
                            pointage.MaterielNomTemporaire = item.LibelleLong;
                            pointage.Materiel.Societe.Code = item.Societe.Code;
                            pointage.Materiel.Code = item.Code;

                            pointage.MaterielSelectionne = item.LibelleLong;
                        }
                        else {
                            pointage.MaterielNomTemporaire = item.CodeRef;
                            pointage.MaterielSelectionne = item.CodeRef;
                        }
                        actionCalculateHeuresTravailles(pointage, $scope.Rapport.Verrouille || !$scope.IsRolePaie);
                        actionCalculateTotalHeureMateriel(pointage);
                        break;
                }
            }

            actionIsUpdated(pointage);
        };

        //Fonction de chargement des roles utilisateur en fonction du niveau de paie
        function actionGetUserRoles() {
            if ($scope.Rapport && $scope.Rapport.CiId) {
                return RapportService.GetUserPaieLevel($scope.Rapport.CiId)
                    .then(function (level) {
                        $scope.IsRolePaie = level >= roleLevels.LevelCSP;
                        $scope.IsGSP = level >= roleLevels.LevelGSP;
                        $scope.IsRoleChantier = level >= roleLevels.LevelCDC;
                    });
            }
        }

        //Fonction construction d'url du referentiel RH
        $scope.getReferentielRhUrl = function (statut) {
            if ($scope.isUserFes) {
                switch (statut) {
                    case "3": return "isCadre=true&";
                    case "2": return "isETAM=true&";
                    case "1": return "isOuvrier=true&";
                    default: return "";
                }
            }
            return "";
        }
        //Fonction d'initialisation des données de la picklist 
        $scope.handleLookupUrl = function (val, refLigne) {
            $scope.refLigne = refLigne;

            var basePrimeControllerUrl = '/api/' + val + '/SearchLight/?societeId={0}&ciId={1}&groupeId={2}&materielId={3}';
            switch (val) {
                case "CodeMajoration":
                    if ($scope.IsFes && $scope.Rapport && $scope.Rapport.CI && $scope.Rapport.ListLignes[0].Personnel) {
                        var baseMajorationByGroupeControllerUrl = '/api/CodeMajoration/SearchLight/?groupeId={0}&isHeureNuit={1}&isEtam={2}&';
                        basePrimeControllerUrl = String.format(baseMajorationByGroupeControllerUrl, $scope.groupeId, false, true);
                    }
                    if (!$scope.IsFes && refLigne.Personnel && refLigne.Personnel.Societe) {
                        basePrimeControllerUrl = String.format(basePrimeControllerUrl, null, $scope.Rapport.CiId, null, null);
                    }
                    break;
                // Refacto CodeDeplacement / CodeZoneDeplacement --> Traitement identique
                case "CodeAbsence":
                case "CodeDeplacement":
                case "CodeZoneDeplacement":
                    if (refLigne.Personnel) {
                        basePrimeControllerUrl = String.format(basePrimeControllerUrl, null, $scope.Rapport.CiId, null, null);
                    }
                    break;
                case "Prime":
                    if ($scope.IsFes) {
                        var basePrimeByGroupeControllerUrl = '/api/Prime/SearchLight/?societeId={0}&groupeId={1}&isRapportPrime={2}&';
                        basePrimeControllerUrl = String.format(basePrimeByGroupeControllerUrl, $scope.Rapport.CI.SocieteId, $scope.groupeId, false);
                    }
                    else {
                        basePrimeControllerUrl = String.format(basePrimeControllerUrl, null, $scope.Rapport.CiId, null, null);
                    }
                    break;
                // Refacto Tache / Materiel --> Traitement identique
                case "Tache":
                case "Materiel":
                    if ($scope.Rapport) {
                        basePrimeControllerUrl = String.format(basePrimeControllerUrl, null, $scope.Rapport.CiId, null, null);
                    }
                    break;
                case "CI":
                    if (FeatureFlags.getFlagStatus('ModificationCiRapportJournalier')) {
                        if ($scope.Rapport.CiId) {
                            return `/api/CI/SearchLightBySocieteId?personnelSocieteId=${$scope.Rapport.CI.SocieteId}&includeSep=false&`;
                        } else {
                            basePrimeControllerUrl = '/api/' + val + '/SearchLight/';
                        }
                    } else {
                        basePrimeControllerUrl = '/api/' + val + '/SearchLight/';
                    }
                    break;
                default:
                    basePrimeControllerUrl = '/api/' + val + '/SearchLight/';
                    break;
            }
            //Remplace les paramètre par null pour les informations qui n'ont pas été remplacée auparavant
            //ex : le personnel du rapport n'a pas encore été sélectionné
            basePrimeControllerUrl = String.format(basePrimeControllerUrl, null, null, null, null);

            return basePrimeControllerUrl;
        };

        // Action changement du code majoration
        function actionChangeCodeMajoration(ligne) {
            if (ligne.CodeMajoration && ligne.CodeMajorationField !== ligne.CodeMajoration.Code) {
                ligne.CodeMajorationId = null;
                ligne.CodeMajoration = null;
                ligne.HeureMajoration = 0;
            }
        }

        // Action changement du code absence
        function actionChangeCodeAbsence(ligne) {
            if (ligne.CodeAbsence && ligne.CodeAbsenceField !== ligne.CodeAbsence.Code) {
                ligne.CodeAbsenceId = null;
                ligne.CodeAbsence = null;
                ligne.HeureAbsence = 0;
                ligne.NumSemaineIntemperieAbsence = null;
            }

            if (ligne.CodeAbsence === null || ligne.CodeAbsence.Code.length === 0) {
                ligne.HeureAbsenceReadOnly = false;
            }
        }

        /**
         * Règle de gestion CI
         * @returns {any} promise
         */
        function actionApplyRGCIOnRapport() {
            return RapportService.ApplyRGCIOnRapport($scope.Rapport)
                .then(function (value) {
                    actionCopy($scope.Rapport, value);
                    actionGetOrCreateIndemniteDeplacement($scope.Rapport.ListLignes[0]);
                    actionConvertToLocaleDate();
                    $scope.ListPrimes = Array.from($scope.Rapport.ListPrimes);
                    $scope.ListTaches = Array.from($scope.Rapport.ListTaches);
                    if ($scope.IsFes) {
                        $scope.ListMajorations = Array.from($scope.Rapport.ListMajorations);
                    }

                    actionInitRapportLigneLibelle();
                    actionInitCommentaire($scope.Rapport.ListTaches);
                    $scope.addTempList();
                })
                .then(actionRefreshRapportLigneCounter)
                .catch(actionOnError);
        }

        /**
         * Règle de gestion Code absence
         * @param {any} pointage pointage courant
         * @returns {any} promise
         */
        function actionApplyRGCodeAbsenceOnPointage(pointage) {
            return RapportService.ApplyRGCodeAbsenceOnPointage(pointage)
                .then(function (value) { actionCopy(pointage, value); })
                .catch(actionOnError);
        }

        /**
         * Action récupération ou création d'indemnité déplacement
         * @param {any} pointage Pointage
         * @returns {any} promise
         */
        function actionGetOrCreateIndemniteDeplacement(pointage) {
            if (pointage && pointage.PrenomNomTemporaire) {

                return RapportService.GetOrCreateIndemniteDeplacement(pointage)
                    .then(function (value) {
                        updateIndemniteDeplacement(pointage, value);
                    })
                    .then(actionInitRapportLigneLibelle)
                    .catch(actionOnError);
            }
        }

        // Action de rafraichissement d'une indemnité de déplacement
        function actionRefreshIndemniteDeplacement(pointage) {
            if (pointage && pointage.PrenomNomTemporaire) {

                return RapportService.RefreshIndemniteDeplacement(pointage)
                    .then(function (value) {
                        updateIndemniteDeplacement(pointage, value);
                    })
                    .then(actionInitRapportLigneLibelle)
                    .catch(actionOnError);
            }
        }

        // Met à jour un pointage après une récupération ou création d'une indemnité de déplacement.
        function updateIndemniteDeplacement(pointage, value) {
            // On vide tous les champs déplacements
            pointage.CodeDeplacementSelectionne = null;
            pointage.CodeDeplacementAbrege = null;
            pointage.CodeZoneDeplacementSelectionne = null;
            pointage.CodeZoneDeplacementAbrege = null;

            // On applique les valeurs issue du calcul des indemnités
            pointage.CodeDeplacement = value.CodeDeplacement;
            pointage.CodeZoneDeplacement = value.CodeZoneDeplacement;
            pointage.CodeDeplacementId = value.CodeDeplacementId;
            pointage.CodeZoneDeplacementId = value.CodeZoneDeplacementId;
            pointage.DeplacementIV = value.DeplacementIV;
            actionIsUpdated(pointage);
        }

        function actionSetMateriel(pointage) {
            if (pointage.PersonnelId > 0) {
                return RapportService.GetMaterielDefault(pointage.PersonnelId)
                    .then(function (value) {
                        if (value && pointage.MaterielId === null) {
                            pointage.Materiel = value;
                            pointage.MaterielId = value.MaterielId;
                            pointage.MaterielNomTemporaire = value.LibelleLong;
                            actionInitRapportLigneLibelle();
                        }
                    })
                    .catch(actionOnError);
            }
        }

        $scope.handleShowErrors = function (rapportLigne) {
            $scope.errorList = rapportLigne.ListErreurs;
        };

        $scope.handleShowWarnings = function () {
            $scope.warningList = [resources.Rapport_Detail_Avec_Personnel_Temp_Bloquer_Verrouillage_Warning];
        };

        $scope.showIconeWarning = function (rapportLigne) {
            return $scope.isUserFayatTP ? (!rapportLigne.Personnel || !rapportLigne.PersonnelId || !rapportLigne.Personnel.PersonnelId) && rapportLigne.PrenomNomTemporaire : false;
        };

        // Calcul des heurs normales
        function actionCalculHeuresNormales(rapportLigne) {
            // On calcul les heures normales que pour le pointage d'un personnel (pas pour le pointage d'un matériel)
            if (rapportLigne.PrenomNomTemporaire) {
                var nbHeuresTaches = actionCalculHeuresTaches(rapportLigne);
                if ($scope.IsFes) {

                    rapportLigne.TotalHeuresMajorees = actionCalculHeuresMajorations(rapportLigne);
                    if ($scope.IsFes && rapportLigne.TotalHeuresMajorees > rapportLigne.MaxMajorationHours && !rapportLigne.IsHouresMajorationMax) {
                        rapportLigne.ListErreurs.push($scope.resources.Rapport_Majoration_Max_Bloquant_Error);
                        rapportLigne.IsHouresMajorationMax = true;
                    }
                    if (rapportLigne.TotalHeuresMajorees <= rapportLigne.MaxMajorationHours) {
                        rapportLigne.IsHouresMajorationMax = false;
                        deleteErrorFromErrorList(rapportLigne, $scope.resources.Rapport_Majoration_Max_Bloquant_Error);
                    }

                    rapportLigne.HeureNormale = nbHeuresTaches;
                    CheckBooleanRapportListligneAttribute($scope.Rapport.ListLignes, $scope.ValidationProp.IsHouresMajorationMax, $scope.ValidationProp.IsMajorationMax);
                }
                else {
                    var majoration = isNaN(parseFloat(rapportLigne.HeureMajoration)) ? 0 : parseFloat(rapportLigne.HeureMajoration);
                    rapportLigne.HeureNormale = nbHeuresTaches - majoration;
                }
            }
        }

        function CheckBooleanRapportListligneAttribute(listLigne, attr, scopeValue) {
            if ($scope.IsFes) {
                var j = 0;
                for (var i = 0; i < listLigne.length; i++) {
                    if (listLigne[i][attr]) {
                        j++;
                        $scope[scopeValue] = true;
                        break;
                    }
                }

                if (j === 0) {
                    $scope[scopeValue] = false;
                }
            }
        }

        // Somme des taches pour une ligne
        function actionCalculHeuresTaches(rapportLigne) {
            var nbHeuresTaches = 0;

            for (var i = 0; i < rapportLigne.ListRapportLigneTaches.length; i++) {
                // On ne teste pas les taches supprimées
                if (!rapportLigne.ListRapportLigneTaches[i].IsDeleted) {
                    var h = rapportLigne.ListRapportLigneTaches[i].HeureTache;

                    if (!isNaN(h)) {
                        nbHeuresTaches += parseFloat(h);
                    }
                }
            }
            return nbHeuresTaches;
        }

        function actionCalculHeuresMajorations(rapportLigne) {
            var nbHeuresMajorations = 0;

            for (var i = 0; i < rapportLigne.ListRapportLigneMajorations.length; i++) {
                // On ne teste pas les taches supprimées
                if (!rapportLigne.ListRapportLigneMajorations[i].IsDeleted) {
                    var h = rapportLigne.ListRapportLigneMajorations[i].HeureMajoration;
                    nbHeuresMajorations += isNaN(parseFloat(h)) ? 0 : parseFloat(h);
                }
            }

            return nbHeuresMajorations;
        }

        // Action changement du PrenomNomTemporaire
        function actionChangePrenomNomTemporaire(rapportLigne) {
            // On supprime tout ce qui a trait au pointage personnel si pas de personnel saisi
            if (!rapportLigne.PrenomNomTemporaire) {
                rapportLigne.HeureNormale = 0;
                rapportLigne.CodeMajoration = null;
                rapportLigne.CodeMajorationField = null;
                rapportLigne.HeureMajoration = 0;
                rapportLigne.CodeAbsence = null;
                rapportLigne.CodeAbsenceField = null;
                rapportLigne.HeureAbsence = 0;
                rapportLigne.NumSemaineIntemperieAbsence = null;
                rapportLigne.CodeDeplacement = null;
                rapportLigne.CodeZoneDeplacement = null;
                rapportLigne.DeplacementIV = false;

                actionInitialiserListeDesPrimes(rapportLigne);
            }
        }

        // Action changement du MaterielNomTemporaire
        function actionChangeMaterielNomTemporaire(rapportLigne) {
            // On supprime tout ce qui a trait au pointage personnel si pas de personnel saisi
            if (!rapportLigne.MaterielNomTemporaire) {
                rapportLigne.MaterielMarche = 0;
                rapportLigne.MaterielArret = 0;
                rapportLigne.MaterielPanne = 0;
                rapportLigne.MaterielIntemperie = 0;
                actionCalculateTotalHeureMateriel(rapportLigne);
            }
        }

        // Validation des horaires avant sauvegarde
        function checkTimesOnSave() {
            if (FeatureFlags.getFlagStatus('RapportsHorairesObligatoires')) {
                $scope.handleCheckTimes();
                if ($scope.formRapport.$error.trancheNotOk) {
                    throw new Error(resources.Rapport_Detail_StatutValidation_Horaire_Tranche_Erreur);
                }
                else if ($scope.formRapport.$error.premiereTrancheManquante) {
                    throw new Error(resources.Rapport_Detail_StatutValidation_Horaire_PremiereTrancheManquante);
                }
            }
        }

        // Action vérification des tranches horaires d'un CI 
        $scope.handleCheckTimes = function () {
            if (!$scope.Rapport) return;

            var horairesMValid = !isNullOrEmpty($scope.HoraireDebutM) && !isNullOrEmpty($scope.HoraireFinM) || isNullOrEmpty($scope.HoraireDebutM) && isNullOrEmpty($scope.HoraireFinM);
            var horairesSValid = !isNullOrEmpty($scope.HoraireDebutS) && !isNullOrEmpty($scope.HoraireFinS) || isNullOrEmpty($scope.HoraireDebutS) && isNullOrEmpty($scope.HoraireFinS);
            var tranchesValide = !isNullOrEmpty($scope.HoraireDebutS) ? !isNullOrEmpty($scope.HoraireDebutM) : true;

            $scope.formRapport.$setValidity("trancheNotOk", horairesSValid && horairesMValid);
            $scope.formRapport.$setValidity("premiereTrancheManquante", tranchesValide);
            // Affiche l'erreur : force le rafraichissement
            $timeout(angular.noop);
        };

        function isNullOrEmpty(string) {
            return string === null || string === undefined || string === "";
        }

        /* -------------------------------------------------------------------------------------------------------------
        *                                            GESTIONS DIVERSES
        * -------------------------------------------------------------------------------------------------------------
        */

        //Vérification de la validité de la date de rapport saisie
        $scope.handleCheckDates = function () {
            var selectedDate = $scope.Rapport.DateChantier;

            // Garder l'ancienne valeur
            // Si on est sur FES on passe par la dialog de confirmation pour changer la date sinon on le modifie directement
            $scope.Rapport.DateChantier = $scope.BeforeChangeRapportDate;

            if ($scope.IsFes
                && $scope.BeforeChangeRapportDate !== null
                && selectedDate !== $scope.BeforeChangeRapportDate) {

                confirmDialog.confirm($scope.resources, "Attention le changement de date du rapport va initialiser toute les sorties astreintes")
                    .then(function () {
                        $scope.BeforeChangeRapportDate = selectedDate;
                        $scope.Rapport.DateChantier = selectedDate;
                        InitializeAstreintesInformations();
                    });
            }
            else {
                $scope.Rapport.DateChantier = selectedDate;
                var bool = $scope.Rapport && $scope.Rapport.DateChantier !== null;
                $scope.formRapport.DateChantier.$setValidity("datesNotOk", bool);
            }
        };

        // Copie les valeurs d'un object dans un autre de type identique mais ne pète pas la référence
        function actionCopy(to, from) {
            for (var i in from) {
                if (from.hasOwnProperty(i)) {
                    to[i] = from[i];
                }
            }
        }

        /*
         * @description Calcul du total d'heures travaillées
         */
        function actionCalculateHeuresTravailles(ligne, updateMaterielOnly) {
            if (!$scope.Rapport.Cloture || !$scope.Rapport.ValidationSuperieur || !$scope.Rapport.Verrouille) {
                if (ligne !== undefined) {
                    var res = 0;

                    res = actionCalculHeuresTaches(ligne);

                    if (res !== 0) {
                        res = TruncateDecimal(res, 2);
                    }

                    if (ligne.MaterielNomTemporaire !== null) {
                        ligne.MaterielMarche = isNaN(res) ? 0 : res;
                    }

                    // Materiel uniquement : pas d'update des heures personnel
                    if (updateMaterielOnly)
                        return;

                    if (ligne.PrenomNomTemporaire) {
                        ligne.HeureTotalTravail = isNaN(res) ? 0 : res;
                        if (!$scope.IsFes) {
                            ligne.HeureNormale = parseFloat(ligne.HeureTotalTravail) - parseFloat(ligne.HeureMajoration);
                        }
                        if ($scope.IsFes) {
                            ligne.HeureNormale = parseFloat(ligne.HeureTotalTravail);
                            if (ligne.HeureTotalTravail > 10 && !ligne.IsWorkHouresMax) {
                                ligne.IsWorkHouresMax = true;
                                ligne.ListErreurs.push($scope.resources.Rapport_Hour_Week_Max);

                            }
                            if (ligne.HeureTotalTravail <= 10) {
                                ligne.IsWorkHouresMax = false;
                                deleteErrorFromErrorList(ligne, $scope.resources.Rapport_Hour_Week_Max);
                            }
                            CheckBooleanRapportListligneAttribute($scope.Rapport.ListLignes, $scope.ValidationProp.IsWorkHouresMax, $scope.ValidationProp.IsWorkHoursMax);
                        }
                    }


                }
            }
        }

        //Permet de supprimer un erreur de la liste des erreurs
        function deleteErrorFromErrorList(ligne, error) {
            for (var i = ligne.ListErreurs.length - 1; i >= 0; i--) {
                if (ligne.ListErreurs[i] === error) {
                    ligne.ListErreurs.splice(i, 1);
                }
            }
        }

        /*
         * @description Calcul du total d'heure matériel
         */
        function actionCalculateTotalHeureMateriel(l) {
            if (l) {
                var marche = isNaN(parseFloat(l.MaterielMarche)) ? 0 : parseFloat(l.MaterielMarche);
                var arret = isNaN(parseFloat(l.MaterielArret)) ? 0 : parseFloat(l.MaterielArret);
                var panne = isNaN(parseFloat(l.MaterielPanne)) ? 0 : parseFloat(l.MaterielPanne);
                var intemperie = isNaN(parseFloat(l.MaterielIntemperie)) ? 0 : parseFloat(l.MaterielIntemperie);

                l.TotalHeureMateriel = marche + arret + panne + intemperie;
            }
            return TruncateDecimal(l.TotalHeureMateriel, 2);
        }

        /*
         * @description Calcul du total d'une colonne
         */
        $scope.handleGetTotalColonne = function (property) {
            var res = 0;
            if ($scope.Rapport.ListLignes !== undefined) {
                for (var i = 0; i < $scope.Rapport.ListLignes.length; i++) {
                    var ligne = $scope.Rapport.ListLignes[i];
                    if (!ligne.IsDeleted) {
                        res += parseFloat(ligne[property]);
                    }
                }

                if (res !== 0) {
                    res = TruncateDecimal(res, 2);
                }
            }
            return isNaN(res) ? 0 : res;
        };

        /*
         * @description Handle sum of week hours : FES
         */
        $scope.handleGetWeekTotalHours = function () {
            if ($scope.IsFes) {
                var res = 0;
                $scope.weekHoursError = new Array();

                for (var i = 0; i < arguments.length; i++) {

                    res += $scope.handleGetTotalColonne(arguments[i]);
                }

                if (!isNaN(res) && res >= 35) {
                    $scope.IsWeekHoursMax = false;
                    $scope.weekHoursError.push($scope.resources.Rapport_Week_Sum_Error_NB.toString());
                    if (res >= 48) {
                        $scope.IsWeekHoursMax = true;
                        $scope.weekHoursError.push($scope.resources.Rapport_Week_Sum_Error_B.toString());
                    }

                    $scope.displayWeekHoursError = true;
                    return true;
                }
            }

            $scope.displayWeekHoursError = false;
            return false;
        };

        /* Total d'une ligne en fonction d'une property */
        $scope.GetTotalLigne = function (property, ligne) {
            if (property === "TotalHeureMateriel") {
                return actionCalculateTotalHeureMateriel(ligne);
            }
        };

        $scope.handleGetTotalTache = function (index) {
            var res = 0;

            // Pour cette tache du rapport
            if ($scope.ListTaches[index] !== undefined) {
                var tache = $scope.ListTaches[index];

                // Pour chaque ligne
                for (var i = 0; i < $scope.Rapport.ListLignes.length; i++) {
                    var ligne = $scope.Rapport.ListLignes[i];

                    if (!ligne.IsDeleted) {
                        // On recherche la tache correspondante (ne doit pas être deleted)
                        for (var j = 0; j < ligne.ListRapportLigneTaches.length; j++) {
                            var ligneTache = ligne.ListRapportLigneTaches[j];

                            // Si pas deleted et qu'on l'a trouvé
                            if (!ligneTache.IsDeleted && ligneTache.Tache.TacheId === tache.TacheId) {
                                res += parseFloat(ligneTache.HeureTache);
                            }
                        }
                    }
                }

                if (res !== 0) {
                    res = TruncateDecimal(res, 2);
                }
            }
            return isNaN(res) ? 0 : res;
        };

        // Fonction de test d'existence d'un objet de type rérential dans une liste
        function actionContainsObject(obj, list) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].IdRef === obj.IdRef) {
                    return true;
                }
            }
            return false;
        }

        // Fonction permettant de tronquer un décimal
        function TruncateDecimal(valeur, decimalPrecision) {
            var roundRatio = Math.pow(10, decimalPrecision);
            return parseFloat(Math.round(valeur * roundRatio) / roundRatio);
        }

        // Gestion du comptage du nombre de lignes du rapport IsDeleted à false
        function actionRefreshRapportLigneCounter() {
            $scope.RapportLigneCounter = 0;
            $scope.Rapport.ListLignes.forEach(function (element) {
                if (!element.IsDeleted)
                    $scope.RapportLigneCounter = $scope.RapportLigneCounter + 1;
            });
        }

        // gestion de l'ouverture du calendrier date chantier 
        function actionOpenDateChantierDatePicker() {
            var element = $window.document.getElementById("DateChantier");
            if (element) {
                var datePickerElement = angular.element(element).children().first();
                if (datePickerElement) {
                    datePickerElement.data('DateTimePicker').show();
                }
            }
        }

        //////////////////////////////////////////////////////
        //                     MODALS                        //
        ///////////////////////////////////////////////////////

        // Ouverture de popup de confirmation
        $scope.popupDeleteRapport = function () {
            $("#confirmationDeleteRapportModal").modal();
        };

        // Ouverture de popup de confirmation
        $scope.popupRemoveCI = function () {
            $("#confirmationDeleteCIModal").modal();
        };

        // Ouverture de popup de confirmation
        $scope.popupRemovePersonnel = function (rapportLigne) {
            $scope.RowPersoDeleted = rapportLigne;
            $("#confirmationDeletePersonnelModal").modal();
        };

        // Ouverture de popup de confirmation
        $scope.popupRemoveMateriel = function (rapportLigne) {
            $scope.RowMaterielDeleted = rapportLigne;
            $("#confirmationDeleteMaterielModal").modal();
        };

        // Ouverture de popup de confirmation
        $scope.popupDoublonPersonnel = function () {
            $("#confirmationDoublonPersonnelModal").modal();
        };

        // Ouverture de popup de confirmation
        $scope.popupTacheVide = function () {
            $("#confirmationTacheVideModal").modal();
        };

        // Ouverture de popup de confirmation
        $scope.handleClickValidate = function () {

            $q.when()
                .then(actionOnBegin)
                .then(actionValidationRapport)
                .finally(actionOnFinally);

        };

        $scope.HeuresAbsenceMin = function (rapportLigne) {
            if (rapportLigne.CodeAbsence) {
                if ($scope.StatutsETAM.indexOf(rapportLigne.Personnel.Statut) !== -1) {
                    return rapportLigne.CodeAbsence.NBHeuresMinETAM;
                }
                if ($scope.StatutsCO.indexOf(rapportLigne.Personnel.Statut) !== -1) {
                    return rapportLigne.CodeAbsence.NBHeuresMinCO;
                }
                else {
                    return 0;
                }
            }
            else {
                return 0;
            }
        };

        $scope.HeuresAbsenceMax = function (rapportLigne) {
            if (rapportLigne.CodeAbsence) {
                if ($scope.StatutsETAM.indexOf(rapportLigne.Personnel.Statut) !== -1) {
                    return rapportLigne.CodeAbsence.NBHeuresMaxETAM;
                }
                if ($scope.StatutsCO.indexOf(rapportLigne.Personnel.Statut) !== -1) {
                    return rapportLigne.CodeAbsence.NBHeuresMaxCO;
                }
                else {
                    return 12;
                }
            }
            else {
                return 12;
            }
        };

        $scope.HeuresAbsenceDefaut = function (rapportLigne) {
            if (rapportLigne.CodeAbsence) { 
                if ($scope.StatutsETAM.indexOf(rapportLigne.Personnel.Statut) !== -1) {
                    return rapportLigne.CodeAbsence.NBHeuresDefautETAM;
                }
                if ($scope.StatutsCO.indexOf(rapportLigne.Personnel.Statut) !== -1) {
                    return rapportLigne.CodeAbsence.NBHeuresDefautCO;
                }
                else {
                    return 0;
                }
            }
            else {
                return 0;
            }
        };

        /*
         * @description Met le champ IsUpdated à true si le pointage est en modification
         */
        function actionIsUpdated(pointage) {
            if (pointage) {
                pointage.IsUpdated = pointage.PointageId > 0 ? true : false;
                $scope.CanBeLockedBeforeSave = $scope.isUserFayatTP ? $scope.Rapport.ListLignes.filter(rl => (rl.PersonnelId === null || rl.Personnel === null || rl.Personnel.PersonnelId === null || rl.Personnel.PersonnelId === undefined) && rl.PrenomNomTemporaire !== null && !rl.IsDeleted).length === 0 : true;
                $scope.Rapport.CanBeLocked = $scope.isUserFayatTP ? $scope.CanBeLockedBeforeSave : true;
            }
        }

        function actionSetBoldProperty() {
            if ($scope.Rapport.DateValidationCDC !== null) {
                for (var i = 0; i < $scope.Rapport.ListLignes.length; i++) {
                    if ($scope.Rapport.ListLignes[i].DateModification !== null) {
                        $scope.Rapport.ListLignes[i].IsBold = moment(new Date($scope.Rapport.DateValidationCDC)) < moment(new Date($scope.Rapport.ListLignes[i].DateModification));
                    }
                }
            }
        }

        //////////////////////////////////////////////////////////////////
        //                     PROMISES ACTIONS                        //
        //////////////////////////////////////////////////////////////////


        function actionOnBegin() {
            $scope.busy = true;
            ProgressBar.start();
        }

        function actionOnError(reason) {
            console.log(reason);
            if (reason.ModelState) {
                if (reason.ModelState.StatusChanged)
                    Notify.error(reason.ModelState.StatusChanged[0]);
                else if (reason.ModelState.DateChantier)
                    Notify.error(reason.ModelState.DateChantier[0]);
                else if (reason.ModelState.RapportExist)
                    Notify.error(reason.ModelState.RapportExist[0]);
                else if (reason.ModelState["rapport.HoraireDebutM"] && reason.ModelState["rapport.HoraireFinM"])
                    Notify.error("Les horaires de matiné doivent être renseignés");
                else if (reason.ModelState["rapport.HoraireDebutM"])
                    Notify.error("L'horaire de début de matiné doit être renseigné");
                else if (reason.ModelState["rapport.HoraireFinM"])
                    Notify.error("L'horaire de fin de matiné doit être renseigné");
                else if (reason.ModelState.HeuresInvalides) {
                    Notify.error(reason.ModelState.HeuresInvalides[0]);
                    validateRapportLigneHeures();
                }
            }
            else if (reason.ExceptionMessage) {
                Notify.error(reason.ExceptionMessage);
            }
            else if (reason.Message) {
                Notify.error(reason.Message);
            }
            else if (reason.message) {
                Notify.error(reason.message);
            }
            else {
                Notify.defaultError();
            }
        }

        function actionOnFinally() {
            $scope.busy = false;
            $timeout(function () { toResize(); });
            ProgressBar.complete();
        }

        function GetOrganisationIdbyPersonnelId(personnelId) {
            return RapportService.GetPersonnelGroupebyId(personnelId)
                .then(function (value) {
                    $scope.groupeId = value.GroupeId;
                });

        }

        function GetOrgnanisationId() {
            UserService.getCurrentUser().then(function (user) {
                $scope.currentUser = user.Personnel;
                GetOrganisationIdbyPersonnelId($scope.currentUser.PersonnelId);
            });
        }

        /// handle affected prime AE | ES | INS for group FES
        function handlePrimeFesAfffectation(prime) {
            var primeAE = $scope.ListPrimes.find(function (prime) {
                return prime.Code === $scope.PrimeCodes.PrimeAE;
            });

            if (prime.Code === $scope.PrimeCodes.PrimeINS || prime.Code === $scope.PrimeCodes.PrimeES) {
                if (primeAE !== undefined && primeAE !== null) {
                    Notify.warning($scope.resources.Rapport_RapportController_PrimeAE_Info);
                }
                else {
                    affectPrime(prime);
                }
            }
            if (prime.Code === $scope.PrimeCodes.PrimeAE) {
                var primeINSAffected = $scope.ListPrimes.find(function (prime) {
                    return prime.Code === $scope.PrimeCodes.PrimeINS;
                });

                if (primeINSAffected !== undefined) {
                    var primeINSAffectedIndex = $scope.ListPrimes.indexOf(primeINSAffected);
                    actionDeletePrime(primeINSAffectedIndex);
                }
                var primeESAffected = $scope.ListPrimes.find(function (prime) {
                    return prime.Code === $scope.PrimeCodes.PrimeES;
                });
                if (primeESAffected !== undefined) {
                    var primeESAffectedIndex = $scope.ListPrimes.indexOf(primeESAffected);
                    actionDeletePrime(primeESAffectedIndex);
                }

                affectPrime(prime);
            }
            if (prime.Code !== $scope.PrimeCodes.PrimeAE && prime.Code !== $scope.PrimeCodes.PrimeINS && prime.Code !== $scope.PrimeCodes.PrimeES) {
                affectPrime(prime);
            }
        }

        /// affect a prime to a rapport ligne prime
        function affectPrime(prime) {
            $scope.ListPrimes.push(prime);
            for (var i = 0; i < $scope.Rapport.ListLignes.length; i++) {
                var ligne = $scope.Rapport.ListLignes[i];
                var rapportLignePrime = {
                    RapportLignePrimeId: 0,
                    RapportLigneId: ligne.RapportLigneId,
                    PrimeId: prime.PrimeId,
                    Prime: prime,
                    IsChecked: false,
                    HeurePrime: null,
                    IsCreated: true,
                    IsUpdated: false,
                    IsDeleted: false
                };
                ligne.ListRapportLignePrimes.push(rapportLignePrime);
                ligne.tempListRapportLignePrimes.push(rapportLignePrime);
            }
        }

        /// Gère la plupart des ng-disabled de l'écran
        $scope.rapportIsReadOnly = function () {
            return $scope.Rapport.ValidationSuperieur || $scope.Rapport.Cloture;
        };

        $scope.rapportIsLocked = function () {
            return $scope.Rapport.ValidationSuperieur || $scope.Rapport.Cloture || $scope.Rapport.Verrouille
                && (!$scope.IsRolePaie || $scope.IsSOMOPA);
        };

        $scope.rapportIsPartialLocked = function () {
            return $scope.Rapport.ValidationSuperieur || $scope.Rapport.Cloture
                || $scope.Rapport.Verrouille && !$scope.IsRolePaie && !$scope.IsRoleChantier
                || $scope.Rapport.Verrouille && $scope.IsSOMOPA;
        };

        $scope.personnelDataLocked = function (rapportLigne) {
            return !rapportLigne.PrenomNomTemporaire || $scope.rapportIsLocked();
        };

        $scope.personnelMajorationLocked = function (rapportLigne, ligneMajoration) {
            var locked = !rapportLigne.PrenomNomTemporaire || $scope.rapportIsLocked();
            if ($scope.isUserFes) {
                switch (rapportLigne.Personnel.Statut) {
                    //Ouvrier
                    case "1": return !ligneMajoration.CodeMajoration.IsOuvrier || locked;
                    //ETAM
                    case "2": return !ligneMajoration.CodeMajoration.IsETAM || locked;
                    //Cadre
                    case "3": return !ligneMajoration.CodeMajoration.IsCadre || locked;
                    default: return locked;
                }
            }
            return locked;
        };
        $scope.personnelPrimeLocked = function (rapportLigne, lignePrime) {
            var locked = !rapportLigne.PrenomNomTemporaire || $scope.rapportIsLocked();
            if ($scope.isUserFes) {
                switch (rapportLigne.Personnel.Statut) {
                    //Ouvrier
                    case "1": return !lignePrime.Prime.IsOuvrier || locked;
                    //ETAM
                    case "2": return !lignePrime.Prime.IsETAM || locked;
                    //Cadre
                    case "3": return !lignePrime.Prime.IsCadre || locked;
                    default: return locked;
                }
            }
            return locked;
        };


        $scope.personnelDataLockedExceptPaie = function (rapportLigne) {
            return !rapportLigne.PrenomNomTemporaire || $scope.Rapport.ValidationSuperieur || $scope.Rapport.Cloture
                || $scope.Rapport.Verrouille && !$scope.IsRolePaie
                || $scope.Rapport.Verrouille && $scope.IsSOMOPA;
        };

        $scope.materielDataLocked = function (rapportLigne) {
            return !rapportLigne.MaterielNomTemporaire || $scope.rapportIsPartialLocked();
        };

        $scope.rapportLigneIsDeletable = function (rapportLigne) {
            return !$scope.rapportIsLocked()
                || !$scope.rapportIsReadOnly() && !rapportLigne.PersonnelId;
        };

        $scope.dateSelectionLocked = function () {
            return $scope.Rapport.Verrouille || $scope.Rapport.Cloture
                || $scope.Rapport.DateValidationCDC !== undefined && $scope.Rapport.DateValidationCDC !== null
                || $scope.Rapport.DateValidationCDT !== undefined && $scope.Rapport.DateValidationCDT !== null
                || $scope.Rapport.DateValidationDRC !== undefined && $scope.Rapport.DateValidationDRC !== null;
        };

        $scope.saveButtonLocked = function () {
            return $scope.busy || $scope.IsMajorationMax || $scope.IsWeekHoursMax || $scope.rapportIsPartialLocked();
        };

        function actionGetFilter() {
            if (sessionStorage.getItem('rapportDetailFilter') !== null) {
                $scope.filter = JSON.parse(sessionStorage.getItem('rapportDetailFilter'));
                $scope.Rapport.CI = $scope.filter.CI;
                $scope.Rapport.CiId = $scope.Rapport.CI.CiId;

                actionGetRole();
            }
        }

        function actionGetRole() {
            // A Faire : Refactor des GET Role
            $q.when()
                .then(actionGetUserRoles)
                .then(actionActualiseIsFes)
                .then(actionActualiseIsSOMOPA)
                .then(actionGetContextualAuthorizationForValidateBtn)
                .then($scope.addTempList);
        }

        function CheckAtreinteHours() {
            var heurDebutNuit = moment().set({ hour: 21, minute: 0 }).format('HH:mm');
            var heureMinuit = moment().set({ hour: 0, minute: 0 }).format('HH:mm');
            var minuteBeforeMinuit = moment().set({ hour: 23, minute: 59 }).format('HH:mm');
            var heureFinNuit = moment().set({ hour: 6, minute: 0 }).format('HH:mm');

            var dateDebutAstreinte = $scope.Rapport
                .ListLignes[$scope.RapportLigneIndexToUpdateAstreinte]
                .ListRapportLigneAstreintes[$scope.AstreinteIndexToUpdate]
                .DateDebutAstreinte;
            var heureDateDebutAstreinte = moment(dateDebutAstreinte).format('HH:mm');

            var dateFinAstreinte = $scope.Rapport
                .ListLignes[$scope.RapportLigneIndexToUpdateAstreinte]
                .ListRapportLigneAstreintes[$scope.AstreinteIndexToUpdate]
                .DateFinAstreinte;
            var heureDateFinAstreinte = moment(dateFinAstreinte).format('HH:mm');
            if (heureDateFinAstreinte !== heureDateDebutAstreinte) {
                if (heureDateDebutAstreinte < heurDebutNuit && heureDateDebutAstreinte >= heureFinNuit &&
                    (heureDateFinAstreinte > heurDebutNuit && heureDateFinAstreinte <= minuteBeforeMinuit || heureDateFinAstreinte >= heureMinuit && heureDateFinAstreinte <= heureFinNuit) ||
                    (heureDateDebutAstreinte >= heurDebutNuit && heureDateDebutAstreinte <= minuteBeforeMinuit || heureDateDebutAstreinte >= heureMinuit && heureDateDebutAstreinte < heureFinNuit) &&
                    heureDateFinAstreinte > heureFinNuit && heureDateFinAstreinte <= heurDebutNuit) {
                    Notify.error(resources.RapportHebdo_Warning_Heures_Astreintes);
                    $scope.Rapport
                        .ListLignes[$scope.RapportLigneIndexToUpdateAstreinte]
                        .ListRapportLigneAstreintes[$scope.AstreinteIndexToUpdate]
                        .DateFinAstreinte = moment(dateDebutAstreinte).add(1, "hours");
                }
            }
        }

        // controle toutes les heures de lignes de rapport et affiche une erreur de ligne si les heures ne correspondent pas
        function validateRapportLigneHeures() {
            if (!$scope.Rapport.ListLignes)
                return;

            // liste des lignes de rapport avec un personnel de défini
            var lignesPersonnel = $scope.Rapport.ListLignes.filter(p => p.PersonnelId && p.PersonnelId !== -1);

            if (lignesPersonnel) {
                lignesPersonnel.forEach(function (ligne) {
                    ligne.ListErreurs.splice(ligne.ListErreurs.indexOf(resources.Pointage_Error_Heures_Taches_Invalides));
                    var totalHeuresTaches = 0;
                    if (ligne.ListRapportLigneTaches) {
                        ligne.ListRapportLigneTaches.forEach(function (tache) {
                            if (!tache.IsDeleted) {
                                totalHeuresTaches += Number(tache.HeureTache);
                            }
                        });
                    }
                    if (ligne.HeureTotalTravail !== totalHeuresTaches)
                        ligne.ListErreurs.push(resources.Pointage_Error_Heures_Taches_Invalides);
                });
            }
        }

        function handleInitHorairesChantier() {
            $scope.HoraireDebutM = convertDateToHHMMFormat($filter('toLocaleDate')($scope.Rapport.HoraireDebutM));
            $scope.HoraireFinM = convertDateToHHMMFormat($filter('toLocaleDate')($scope.Rapport.HoraireFinM));
            $scope.HoraireDebutS = convertDateToHHMMFormat($filter('toLocaleDate')($scope.Rapport.HoraireDebutS));
            $scope.HoraireFinS = convertDateToHHMMFormat($filter('toLocaleDate')($scope.Rapport.HoraireFinS));
        }

        /*
         * @convertit les horaires du format HHMM vers le type Date
         */
        function handleSaveHorairesChantier() {
            $scope.Rapport.HoraireDebutM = convertHHMMToDateFormat($scope.HoraireDebutM);
            $scope.Rapport.HoraireFinM = convertHHMMToDateFormat($scope.HoraireFinM);
            $scope.Rapport.HoraireDebutS = convertHHMMToDateFormat($scope.HoraireDebutS);
            $scope.Rapport.HoraireFinS = convertHHMMToDateFormat($scope.HoraireFinS);
        }

        function disableCodeZone(rapportLigne) {
            if ($scope.Rapport.CI) {
                return $scope.personnelDataLockedExceptPaie(rapportLigne) || !$scope.IsGSP && !$scope.Rapport.CI.ZoneModifiable;
            }
            else {
                return true;
            }
        }

        function convertDateToHHMMFormat(date) {
            if (date && angular.isDate(date)) {
                var heures = ("0" + date.getHours()).slice(-2);
                var minutes = ("0" + date.getMinutes()).slice(-2);
                return heures + minutes;
            }
            else {
                return null;
            }
        }

        function convertHHMMToDateFormat(time) {
            if (time) {
                var newDate = new Date();
                var hour = time.substr(0, 2);
                var minutes = time.substr(2, 4);
                newDate.setHours(hour);
                newDate.setMinutes(minutes);
                newDate.setSeconds(0);
                return newDate;
            }
            else {
                return null;
            }
        }

        function SaveEvenements() {
            handleSaveHorairesChantier();
            $scope.Rapport.Meteo = $scope.Meteo;
            $scope.Rapport.Evenements = $scope.Evenements;
            $scope.CloseFredSide();
        }

        function CancelEvenements() {
            InitEvenements();
            $scope.CloseFredSide();
        }

        function InitEvenements() {
            handleInitHorairesChantier();
            $scope.Meteo = $scope.Rapport.Meteo;
            $scope.Evenements = $scope.Rapport.Evenements;
        }

        $scope.CloseFredSide = function () {
            $scope.fredSide = false; $scope.evenement = false; $scope.validation = false;
        };
    }
}(angular));