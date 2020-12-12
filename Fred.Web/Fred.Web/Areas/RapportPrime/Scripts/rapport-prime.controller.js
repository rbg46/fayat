(function (angular) {
    'use strict';

    angular.module('Fred').controller('RapportPrimeController', RapportPrimeController);

    RapportPrimeController.$inject = ['$scope', '$filter', '$timeout', 'Notify', 'RapportPrimeService', 'ProgressBar', '$q', 'UserService', 'confirmDialog', 'fredDialog', 'DatesClotureComptableService', 'authorizationService'];

    function RapportPrimeController($scope, $filter, $timeout, Notify, RapportPrimeService, ProgressBar, $q, UserService, confirmDialog, fredDialog, DatesClotureComptableService, authorizationService) {

        /* -------------------------------------------------------------------------------------------------------------
         *                                            INIT
         * -------------------------------------------------------------------------------------------------------------
         */

        // assignation de la valeur du scope au controller pour les templates non mis à jour
        var $ctrl = this;
        $ctrl.dateRapportPrimeCourant = new Date();
        $ctrl.searchFilter = "";
        $ctrl.viewLinesFromCurrentUser = false;

        // Instanciation Objet Ressources
        $ctrl.resources = resources;
        $ctrl.RapportPrime = {};

        $scope.init = function () {
            // On passe en langue FR pour MomentJS pour qu'il puisse parser le DatePicker affichant la date du RapportPrime
            moment.locale('fr');

            // Récupération de l'utilisateur courant
            GetCurrentUser();
            $ctrl.rights = authorizationService.getRights(PERMISSION_KEYS.AffichageMenuRapportPrimesIndex);
            $ctrl.validatedButtonPrimes = authorizationService.getRights(PERMISSION_KEYS.AffichageButtonValidationPrimes);

            var today = moment(new Date()).format("MM-DD-YYYY");
            $ctrl.RapportPrime.DateRapportPrimePicker = moment(today).format("MMM-YY");

            $q.when()
                .then(actionOnBegin)
                .then(actionGetRapportPrime(today))
                .finally(actionOnFinally);
        };

        /* -------------------------------------------------------------------------------------------------------------
        *                                            ACTIONS
        * -------------------------------------------------------------------------------------------------------------
        */
        // Action de rafraichissement du rapports de prime
        $ctrl.refreshData = function (response) {
            if (response.data !== null) {
                fillRapportPrimeData(response.data);
            } else {
                if (isDatePickerSelectedOnCurrentMonth()) {
                    $q.when()
                        .then(actionAddRapportPrime());
                } else {
                    switchDatePickerToPreviousDate();
                }
            }
        };

        function fillRapportPrimeData(data) {
            $timeout(function () { onScroll(); });
            // On formatte la date provenant du retour de l'API pour mettre à jour la date du picker du mois
            data.DateRapportPrimePicker = moment(data.DateRapportPrime).format("MMM-YY");

            $ctrl.RapportPrime = data;
            $ctrl.dateRapportPrimeCourant = $ctrl.RapportPrime.DateRapportPrime;

            processLigneRapportPrime();

            // Initialisation des DatePickers
            actionInitDatePickers();
        }

        function isDatePickerSelectedOnCurrentMonth() {
            return moment($ctrl.RapportPrime.DateRapportPrimePicker, "MMM-YY").month() === new Date().getMonth();
        }

        function switchDatePickerToPreviousDate() {
            $('.datepicker-daterapport').datepicker('setDate', new Date($ctrl.RapportPrime.DateRapportPrime));

            $ctrl.RapportPrime.DateRapportPrimePicker = moment($ctrl.dateRapportPrimeCourant, "YYYY-MM-DD").format("MMM-YY");

            // RG_3206_022_2 (suite au BUG_6188)
            Notify.warning($ctrl.resources.RapportPrime_Notify_Warning_RapportPrimeInexistant);
        }

        // Action au changement du Mois en cours => on charge le rapport de prime de ce mois s'il existe
        $ctrl.changeDateRapportPrime = function () {
            if (!IsSamePeriod(new Date($ctrl.dateRapportPrimeCourant), new Date($ctrl.RapportPrime.DateRapportPrimePicker))) {
                // Date du mois en cours
                // Conversion date "févr.-18" vers "2018-02-01"
                var dateFiltree = moment($ctrl.RapportPrime.DateRapportPrimePicker, "MMM-YY").format("MM-DD-YYYY");

                $q.when()
                    .then(actionOnBegin)
                    .then(actionGetRapportPrime(dateFiltree))
                    .finally(actionOnFinally);
            }
        };

        function onScroll() {
            var scrollLeft = $(".rapport-table tbody").scrollLeft();

            $('.rapport-table thead').css("left", -scrollLeft); //fix the thead relative to the body scrolling

            for (var i = 0; i < 3; i++) {
                $('.rapport-table thead th:nth-child(' + i.toString() + ')').css("left", scrollLeft); //fix the first cell of the header
                //$('.rapport-table tbody td:nth-child(' + i.toString() + ')').css("left", scrollLeft); //fix the first column of tdbody
            }
        }

        function processLigneRapportPrime() {

            var header = $ctrl.RapportPrime.ListPrimesHeader;
            // Traitement à faire pour chaque ligne de rapport de prime
            angular.forEach($ctrl.RapportPrime.ListLignes, function (rpl) {
                rpl.IsPeriodClosed = false; // Ne devrait on pas faire le test de la période cloturée (seulement si un CI est présent sur la ligne) : DatesClotureComptableService.GetPeriodStatus ?
                actionInitRapportPrimeLigneLibelle(rpl);

                if (rpl.ListAstreintes) {
                    rpl.ListAstreintes = actionFormatAstreinteListDateToListDay(rpl.ListAstreintes);
                }

                var primeIds = rpl.ListPrimes.map(function (x) { return x.PrimeId; });
                var headerPrimeIds = header.map(function (x) { return x.PrimeId; });

                var primesToAdd = headerPrimeIds.filter(function (x) { return primeIds.indexOf(x) < 0; });


                angular.forEach(primesToAdd, function (primeId) {
                    var fakePrime = $filter('filter')(header, { PrimeId: primeId }, true)[0];
                    if (fakePrime) {
                        var newFakePrime = { Prime: fakePrime, PrimeId: fakePrime.PrimeId };
                        rpl.ListPrimes.push(newFakePrime);
                    }
                });

                var ordering = {};

                for (var i = 0; i < headerPrimeIds.length; i++)
                    ordering[headerPrimeIds[i]] = i;

                rpl.ListPrimes.sort(function (a, b) {
                    return ordering[a.PrimeId] - ordering[b.PrimeId];

                });

                // Calcul du nombre de jour d'astreinte sélectionné
                actionCalculNombreJourAstreinte(rpl);
            });
        }

        // Handler de changement de date d'astreinte
        $ctrl.changeDatePickerAstreinte = function (ligne) {
            actionCalculNombreJourAstreinte(ligne);
        };

        // Handler de click sur le bouton Ajouter une ligne
        $ctrl.handleClickAddRow = function () {
            actionAddRow();
        };

        // Handler de click sur le bouton Supprimer une ligne
        $ctrl.handleClickDeleteRow = function (ligne) {
            actionDeleteRow(ligne);
        };

        // Handler de click sur le bouton Dupliquer une ligne
        $ctrl.handleClickDuplicateRow = function (ligne) {
            actionDuplicateRow(ligne);

            // Initialisation des DatePickers une fois la ligne dupliquée
            actionInitDatePickers();
        };

        // Handler de click sur le bouton supprimer une prime
        $ctrl.handleClickDeletePrime = function (prime) {

            // Fonctions de vérifications imbriquées
            var montantExistDansLigne = function (rplp) {
                return rplp.PrimeId === prime.PrimeId
                    && rplp.Montant
                    && rplp.Montant !== "0.00";
            };

            var montantExist = function (rpl) {
                return !rpl.IsDeleted && rpl.ListPrimes.some(montantExistDansLigne);
            };

            // Si il y a au moins un Montant qui est renseigné parmis l'ensemble des lignes du RapportPrime (tout createur confondu)
            if ($ctrl.RapportPrime.ListLignes.some(montantExist)) {
                // Boite de dialogue oui/non de confirmation : "Voulez vous supprimer cette prime bien qu'elle contienne au moins un Montant de renseigné ?"
                confirmDialog.confirm($ctrl.resources, $ctrl.resources.RapportPrime_ModelSuppressionPrime_MessageConfirmation, "flaticon flaticon-warning").then(function () {
                    // On met à 0 l'ensemble des Montant uniquement sur les lignes crées par l'utilisateur courant (celles sur lesquelles il a filtré)
                    resetAmountForUnvalidatedLinesForCurrentUser(prime);
                });
            } else {
                // Sinon il y a aucune lignes possédant un Montant différent de 0 parmis l'ensemble des lignes (non supprimée) du RapportPrime

                // On supprime physiquement la colonne
                actionDeletePrime(prime);
            }
        };

        // Fonction de chargement des données de l'item sélectionné dans le lookup
        $ctrl.handleLookupSelection = function (type, item, rapportPrimeLigne) {
            if (item) {
                switch (type) {
                    case "Prime":
                        actionAddPrime(item);
                        break;
                    case "Personnel":
                        // Met à jour les ID vis à vis des lookup qui ne modifient que les entités et pas les ID
                        if (rapportPrimeLigne.Personnel !== null) {
                            rapportPrimeLigne.PersonnelId = rapportPrimeLigne.Personnel.PersonnelId;
                        }
                        actionInitRapportPrimeLigneLibelle(rapportPrimeLigne);
                        break;
                    case "CI":
                        // Met à jour les ID vis à vis des lookup qui ne modifient que les entités et pas les ID
                        if (rapportPrimeLigne.Ci !== null) {
                            rapportPrimeLigne.CiId = rapportPrimeLigne.Ci.CiId;
                        }

                        // Test sur l'état du CI vis à vis de la période des dates de clotures comptables
                        var dateRapportPrime = new Date($ctrl.RapportPrime.DateRapportPrime);
                        var dateSelectedYear = dateRapportPrime.getFullYear();
                        var dateSelectedMonth = dateRapportPrime.getMonth() + 1;

                        // Récupération de l'état de la Période pour savoir si elle est cloturée pour ce CI ou non
                        DatesClotureComptableService.GetPeriodStatus(item.CiId, dateSelectedYear, dateSelectedMonth)
                            .then(function (status) {
                                rapportPrimeLigne.IsPeriodClosed = status.data;
                            })
                            .catch(actionOnError);
                        break;
                }
            }
            actionIsUpdated(rapportPrimeLigne);
        };

        // Fonction de suppression du personnel sélectionné dans la picklist
        $ctrl.handleDeletePickListItemPersonnel = function () {
            var rapportPrimeLigne = $ctrl.RowPersoDeleted;
            rapportPrimeLigne.Personnel = null;
            rapportPrimeLigne.PersonnelId = null;
            rapportPrimeLigne.PrenomNomTemporaire = null;
            rapportPrimeLigne.PersonnelSelectionne = $ctrl.resources.Global_ReferentielPersonnel_Placeholder;

            actionIsUpdated(rapportPrimeLigne);
        };

        // Fonction de suppression du ci sélectionné dans la picklist
        $ctrl.handleDeletePickListCI = function () {
            var rapportPrimeLigne = $ctrl.RowCIDeleted;

            rapportPrimeLigne.Ci = null;
            rapportPrimeLigne.CiId = 0;

            actionIsUpdated(rapportPrimeLigne);
        };

        // Fonction de changement du Montant
        $ctrl.handleMontantChanged = function (ligneChanged, lignePrimeChanged) {
            //0) On passe la ligne à l'état modifié
            actionIsUpdated(ligneChanged);

            //1) Récupèration du Seuil de la Prime liée à la lignePrimeChanged
            var seuilPrime = lignePrimeChanged.SeuilMensuel;

            var cumulMontantPrime = 0;

            //2) Pour chaque ligne ayant le meme personnel que le mien
            angular.forEach($ctrl.RapportPrime.ListLignes, function (rpl) {
                if (!rpl.IsDeleted
                    && rpl.Personnel
                    && rpl.Personnel.PersonnelId === ligneChanged.Personnel.PersonnelId
                    && ligneChanged !== rpl) {

                    // Pour chaque lignePrime dans la ligne
                    angular.forEach(rpl.ListPrimes, function (rplp) {
                        if (rplp.PrimeId === lignePrimeChanged.PrimeId
                            && rplp.Montant
                            && rplp.Montant !== "0.00") {

                            // Cumul du montant de la meme prime que moi
                            cumulMontantPrime += Number(rplp.Montant);
                        }
                    });
                }
            });

            // Si le cumul est supérieur au seuil de la prime 
            if (cumulMontantPrime + Number(lignePrimeChanged.Montant) > seuilPrime) {
                // Alors alerte visuelle
                Notify.warning(String.format(resources.RapportPrime_Notify_Warning_CumulSeuilPrime, seuilPrime));

                // Et on met le montant à 0
                lignePrimeChanged.Montant = 0.00;
            }
        };

        // Gestion de la validation d'une ligne
        $ctrl.handleValidation = function (ligneAValider) {
            actionValidation(ligneAValider);
        };

        // Gestion de la validation de masse
        $ctrl.handleMassValidation = function (checkMassValidation) {
            let rplList = $ctrl.RapportPrime.ListLignes;

            // On récupère la liste affichée à l'écran si l'utilisateur a fait un filtre
            if ($ctrl.searchFilter) {
                rplList = $filter('rapportPrimePersonnelTypePrimeCIFilter')($ctrl.RapportPrime.ListLignes, $ctrl.searchFilter);
            }
            else if ($ctrl.viewLinesFromCurrentUser) {
                rplList = $filter('filter')($ctrl.RapportPrime.ListLignes, { IsDeleted: false, AuteurCreationId: $ctrl.currentUser.PersonnelId }, true);
            }
            angular.forEach(rplList, function (rpl) {
                // Si la période comptable est cloturée on ne valide pas la ligne
                if (!rpl.IsPeriodClosed) {
                    rpl.IsValidated = checkMassValidation;

                    // Validation de la ligne
                    actionValidation(rpl);
                }
            });
        };

        // Sauvegarde directe
        $ctrl.handleSave = function (rapportPrimeModel) {
            $q.when()
                .then(actionOnBegin)
                .then(function () {
                    actionSave(rapportPrimeModel);
                    Notify.message(resources.Global_Notification_Enregistrement_Success);
                })
                .catch(actionOnError);
        };

        // Ouverture de popup de confirmation
        $ctrl.popupRemovePersonnel = function (rapportPrimeLigne) {
            $ctrl.RowPersoDeleted = rapportPrimeLigne;
            $("#confirmationDeletePersonnelModal").modal();
        };

        // Ouverture de popup de confirmation
        $ctrl.popupRemoveCI = function (rapportPrimeLigne) {
            $ctrl.RowCIDeleted = rapportPrimeLigne;
            $("#confirmationDeleteCIModal").modal();
        };

        // Handler changement de CI
        $ctrl.handleClickChangeCI = function () {
            return $ctrl.handleLookupUrl('CI');
        };

        // Fonction d'initialisation des données de la picklist 
        $ctrl.handleLookupUrl = function (val) {
            var baseControllerUrl = '/api/' + val + '/SearchLight/?isRapportPrime={0}';

            // Mettre un Switch lorsqu'il y aura plus de 2 choix possibles
            if (val === 'Prime') {
                if ($ctrl.RapportPrime) {
                    baseControllerUrl = String.format(baseControllerUrl, true);
                }
            } else {
                baseControllerUrl = '/api/' + val + '/SearchLight/';
            }

            return baseControllerUrl;
        };

        // Handler pour show ou non les lignes
        $ctrl.handleShowLigne = function (rapportLigne) {
            return actionShowLigne(rapportLigne);
        };

        // Fonction pour savoir si on montre la ligne
        function actionShowLigne(rapportLigne) {
            var showLigne = false;

            // Si on veux visualiser que les lignes crées par l'utilisateur
            if ($ctrl.viewLinesFromCurrentUser) {
                if (rapportLigne.IsDeleted === false && $ctrl.currentUser.PersonnelId === rapportLigne.AuteurCreationId) {
                    showLigne = true;
                }
            } else { // Sinon on trie uniquement sur les lignes non supprimées
                if (rapportLigne.IsDeleted === false) {
                    showLigne = true;
                }
            }

            return showLigne;
        }

        // Action de calcul du nombre de jour d'astreinte sélectionné
        function actionCalculNombreJourAstreinte(ligne) {
            if (ligne.ListAstreintes.length > 0) {

                let nombreJourDateAtreintes = null;
                if (!Array.isArray(ligne.ListAstreintes)) {
                    nombreJourDateAtreintes = ligne.ListAstreintes.split(",");
                } else {
                    nombreJourDateAtreintes = ligne.ListAstreintes;
                }

                ligne.NombreJourAstreinte = nombreJourDateAtreintes.length + "J";
            }
            else {
                ligne.NombreJourAstreinte = "";
            }
        }

        // Action de récupération d'un rapportPrime existant
        function actionGetRapportPrime(date) {
            return RapportPrimeService.GetRapportPrime(date)
                .then($ctrl.refreshData)
                .catch(actionOnError);
        }

        function actionAddRapportPrime() {
            return RapportPrimeService.AddRapportPrime()
                .then($ctrl.fillRapportPrimeDataPromise)
                .catch(actionOnError);
        }

        $ctrl.fillRapportPrimeDataPromise = function (response) {
            fillRapportPrimeData(response.data);
        }

        function actionAddRow() {
            var newLine = {};
            initLineData(newLine);
            newLine.ListPrimes = [];
            newLine.ListAstreintes = [];

            for (var i = 0; i < $ctrl.RapportPrime.ListPrimesHeader.length; i++) {
                var prime = $ctrl.RapportPrime.ListPrimesHeader[i];
                prime.RapportPrimeLigneId = newLine.RapportPrimeLigneId;
                prime.IsCreated = true;
                newLine.ListPrimes.push(prime);
            }

            $ctrl.RapportPrime.ListLignes.push(newLine);  
            
            actionInitDatePickers();
        }

        // Action suppression d'une ligne de rapportPrime
        function actionDeleteRow(ligne) {
            ligne.IsCreated = false;
            ligne.IsUpdated = false;
            ligne.IsDeleted = true;
        }

        // Action duplication d'une ligne de rapport
        function actionDuplicateRow(ligne) {
            var lineToDuplicate = angular.copy(ligne);
            initLineData(lineToDuplicate);
            
            // On met l'ID technique des primes à 0
            for (var i = 0; i < lineToDuplicate.ListPrimes.length; i++) {
                var lignePrime = lineToDuplicate.ListPrimes[i];

                lignePrime.RapportPrimeLignePrimeId = 0;
                lignePrime.IsDeleted = ligne.ListPrimes[i].IsDeleted;
                lignePrime.IsCreated = true;
                lignePrime.IsUpdated = false;
            }
            $ctrl.RapportPrime.ListLignes.push(lineToDuplicate);

            $timeout(function () { onScroll(); });
        }

        function initLineData(line) {
            line.RapportPrimeLigneId = 0;
            line.IsDeleted = false;
            line.IsCreated = true;
            line.IsUpdated = false;
            line.AuteurCreation = $ctrl.currentUser;
            line.AuteurCreationId = $ctrl.currentUser.PersonnelId;
            line.DateCreation = null;
            line.AuteurModification = null;
            line.AuteurModificationId = null;
            line.DateModification = null;
            line.AuteurValidation = null;
            line.AuteurValidationId = null;
            line.DateValidation = null;
        }

        // Action de création d'un rapportPrime
        function actionSave(rapportPrimeModel) {
            rapportPrimeModel.ListLignes = rapportPrimeModel.ListLignes.filter(x => x.IsUpdated || x.IsCreated || x.IsDeleted);

            rapportPrimeModel = actionFormatAstreinteListDayToListDate(rapportPrimeModel);
            
            var rapportPrimeUpdateModel = formatRapportPrimeForUpdate(rapportPrimeModel);

            return $q.when()
                .then(actionUpdateRapportPrime(rapportPrimeModel.RapportPrimeId, rapportPrimeUpdateModel))
                .then(function () { $timeout(function () { onScroll(); }); });
        }

        // Action de formattage de la liste des datetime en liste de jours, pour les dates d'astreintes (Back vers Front)
        function actionFormatAstreinteListDateToListDay(listAstreintesDate) {
            // Tableau vide temporaire qui contiendras les Dates d'astreintes formattées
            let listAstreintesJour = [];

            // Pour chaque date
            for (let i = 0; i < listAstreintesDate.length; i++) {

                // Date temporaire du Rapport de Prime (Mois+Année)
                let jourAstreinte = moment(listAstreintesDate[i]).format("DD");

                // Ajout dans le tableau du jour
                listAstreintesJour.push(jourAstreinte);
            }
            return listAstreintesJour;
        }

        // Action de formattage de la liste des jours en liste de dates, pour les dates d'astreintes (Front vers Back)
        function actionFormatAstreinteListDayToListDate(rapportPrimeModel) {
            // Pour chaque ligne
            for (let i = 0; i < rapportPrimeModel.ListLignes.length; i++) {
                let ligne = rapportPrimeModel.ListLignes[i];

                // Si il y a au moins une Date
                if (ligne.ListAstreintes.length > 0) {
                    // On écrase le tableau de jours de la ligne par ce nouveau tableau de date formatté
                    ligne.ListAstreintes = formatLineAstreinteListDayToListDate(ligne.ListAstreintes);
                }
            }
            return rapportPrimeModel;
        }

        // Formattage de la liste des jours en liste de dates, pour les dates d'astreintes (Front vers Back), d'une seule ligne de rapport de prime
        function formatLineAstreinteListDayToListDate(listAstreintes) {
            // Tableau vide temporaire qui contiendras les Dates d'astreintes formattées
            let newListAstreintes = [];

            let listeDateAtreintes = null;
            if (!Array.isArray(listAstreintes)) {
                // Construit un tableau de date séparée par des virgules
                listeDateAtreintes = listAstreintes.split(",");
            } else {
                listeDateAtreintes = listAstreintes;
            }

            // Pour chaque Date de la ligne
            for (let y = 0; y < listeDateAtreintes.length; y++) {
                // Date temporaire du Rapport de Prime (Mois+Année)
                let dateAstreinte = moment($ctrl.RapportPrime.DateRapportPrime);

                // On ajoute le jours sur le Mois+Année
                dateAstreinte.set('date', listeDateAtreintes[y]);

                // Ajout dans le tableau de la date formattée
                newListAstreintes.push(moment(dateAstreinte));
            }
            return newListAstreintes;
        }

        function formatRapportPrimeForUpdate(rapportPrimeModel) {
            return {
                DateRapportPrime : rapportPrimeModel.DateRapportPrime,
                LinesToCreate: rapportPrimeModel.ListLignes.filter(l => l.IsCreated),
                LinesToUpdate: rapportPrimeModel.ListLignes.filter(l => l.IsUpdated),
                LinesToDelete: rapportPrimeModel.ListLignes.filter(l => l.IsDeleted).map(l => l.RapportPrimeLigneId)
            }
        }

        // Action de mise à jour d'un rapportPrime
        function actionUpdateRapportPrime(rapportPrimeId, rapportPrimeUpdateModel) {
            return RapportPrimeService.UpdateRapportPrime(rapportPrimeId, rapportPrimeUpdateModel)
                .then($ctrl.refreshData)
                .catch(actionOnError)
                .finally(actionOnFinally);
        }

        // Initialisation des libelles à afficher dans les directives picklistcaller
        function actionInitRapportPrimeLigneLibelle(rapportPrimeLigne) {
            //Personnel
            rapportPrimeLigne.PersonnelSelectionne = rapportPrimeLigne.Personnel && rapportPrimeLigne.Personnel.PersonnelId ? rapportPrimeLigne.Personnel.CodeSocieteMatriculePrenomNom
                : rapportPrimeLigne.PrenomNomTemporaire !== null ? rapportPrimeLigne.PrenomNomTemporaire : resources.Global_ReferentielPersonnel_Placeholder;
        }

        // Initialisation des DatePickers à afficher sur la page
        function actionInitDatePickers() {
            $timeout(function () {
                $q.when()
                    .then(actionInitDatePickersAstreintes())
                    .then(actionInitDatePickerDateRapport());
            });
        }

        // Configuration propre aux datepickers multidate (d'Astreintes)
        function actionInitDatePickersAstreintes() {
            var dateFin = moment($ctrl.RapportPrime.DateRapportPrimePicker, "MMM-YY");

            // Dernier jour du mois
            var lastDay = new Date(moment(dateFin, "MM-DD-YYYY").endOf('month'));
            // Premier jour du mois
            var firstDay = new Date(moment(dateFin, "MM-DD-YYYY").startOf('month'));

            $('.datepicker-astreintes').datepicker({
                endDate: lastDay,
                startDate: firstDay,
                language: "fr"
            });

            // Cette boucle pour forcer la MAJ des dates avec ce que contient le rapportprimeligne.ListAstreintes (en ajoutant le mois et l'année aux jours)
            for (var i = 0; i < $ctrl.RapportPrime.ListLignes.length; i++) {
                var ligne = $ctrl.RapportPrime.ListLignes[i];

                var listAstreintesFormatDate = [];
                var listAstreintesFormatMoment = formatLineAstreinteListDayToListDate(ligne.ListAstreintes);

                for (let y = 0; y < listAstreintesFormatMoment.length; y++) {
                    listAstreintesFormatDate.push(new Date(listAstreintesFormatMoment[y]));
                }

                $('#datepicker-astreintes' + i).datepicker('setUTCDate', listAstreintesFormatDate);
            }
        }

        // Configuration au datepicker de la Date du Rapport
        function actionInitDatePickerDateRapport() {
            $('.datepicker-daterapport').datepicker({
                startDate: '-1y',
                todayHighlight: true,
                language: "fr"
            });
        }

        // Action ajout d'une prime dans le rapportPrime
        function actionAddPrime(prime) {
            // Test d'existance de la prime dans le rapportPrime
            if (!actionContainsObject(prime, $ctrl.RapportPrime.ListPrimesHeader)) {
                $ctrl.RapportPrime.ListPrimesHeader.push(prime);
                for (var i = 0; i < $ctrl.RapportPrime.ListLignes.length; i++) {
                    var ligne = $ctrl.RapportPrime.ListLignes[i];
                    var rapportPrimeLignePrime = {
                        RapportPrimeLignePrimeId: 0,
                        RapportPrimeLigneId: ligne.RapportPrimeLigneId,
                        PrimeId: prime.PrimeId,
                        Prime: prime,
                        IsCreated: true,
                        IsUpdated: false,
                        IsDeleted: false
                    };
                    ligne.ListPrimes.push(rapportPrimeLignePrime);
                }
            }
            else {
                Notify.warning($ctrl.resources.RapportPrime_PrimeExisteDeja_Info);
            }
        }

        /*
        * @description Remet le champ Montant à 0 pour les lignes créées par l'utilisateur courant
        */
        function resetAmountForUnvalidatedLinesForCurrentUser(prime) {
            const listLignesNotValidated = $ctrl.RapportPrime.ListLignes.filter(ligne => !ligne.IsValidated);
            angular.forEach(listLignesNotValidated, function (rpl) {
                if (!rpl.IsDeleted && $ctrl.currentUser.PersonnelId === rpl.AuteurCreationId) {
                    const listPrimesWithMontant = rpl.ListPrimes.filter(currentPrime => currentPrime.Montant && currentPrime.Montant !== "0.00");
                    angular.forEach(listPrimesWithMontant, function (rplp) {
                        if (rplp.PrimeId === prime.PrimeId) {
                            rplp.Montant = 0.00;
                        }
                    });
                }
            });
        }

        /*
        * @description Action suppression d'une prime
        */
        function actionDeletePrime(prime) {
            //Si la prime est différente d'une prime mensuelle

            if (prime.PrimeType !== 2) {
                fredDialog.erreur("Cette prime ne peut pas être retirée du rapport de primes car son type a été modifié ou elle a été supprimée");
                return;
            }

            // On supprime le Header de prime dans la liste des Primes du Rapport
            var index = $ctrl.RapportPrime.ListPrimesHeader.indexOf(prime);
            $ctrl.RapportPrime.ListPrimesHeader.splice(index, 1);

            // On supprime les RapportPrimeLignePrime ayant la meme Prime que celle que l'on vient de supprimer dans la liste des headers
            for (var i = 0; i < $ctrl.RapportPrime.ListLignes.length; i++) {
                var rpl = $ctrl.RapportPrime.ListLignes[i];

                // Je ne sors pas de la boucle tant que toutes les primes n'ont pas été testées car dans la saisie d'un rapport je peux ajouter
                // une prime, la supprimer, mais en fait la rajouter donc j'ai potentiellement dans ma liste plusieurs fois la même prime
                // mais avec les attributs de CRUD différents
                for (var j = 0; j < rpl.ListPrimes.length; j++) {
                    var rplp = rpl.ListPrimes[j];
                    if (rplp.PrimeId === prime.PrimeId) {
                        if (rplp.IsCreated) {
                            rpl.ListPrimes.splice(j, 1);
                        }
                        else if (!rplp.IsDeleted) {
                            rplp.IsDeleted = true;

                            // Si une prime a un montant de renseigné, on met la date de modif de la ligne à jour
                            if (rplp.Montant > 0) {
                                actionIsUpdated(rpl);
                            }
                        }
                    }
                }
            }
        }

        /*
        * @description Fonction de test d'existence d'un objet de type rérential dans une liste
        */
        function actionContainsObject(obj, list) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].IdRef === obj.IdRef) {
                    return true;
                }
            }
            return false;
        }

        /*
        * @description Indique si 2 dates sont sur la même période. return : true si les 2 dates sont sur la même période, sinon false
        */
        function IsSamePeriod(date1, date2) {
            return date1.getFullYear() === date2.getFullYear() && date1.getMonth() === date2.getMonth();
        }

        /*
        * @description Met le champ IsUpdated à true si la ligne de RapportPrime est en modification
        */
        function actionIsUpdated(rapportPrimeLigne) {
            if (rapportPrimeLigne) {
                rapportPrimeLigne.IsUpdated = rapportPrimeLigne.RapportPrimeLigneId > 0 ? true : false;
            }
        }

        /*
        * @descritpion Vérifie si il y a des erreur et ou des warning sur la ligne à valider
        */
        function actionValidation(ligneAValider) {
            // On sauvegarder l'état validé ou non de la ligne avant l'action
            var isValidatedbeforeAction = !ligneAValider.IsValidated;

            if (actionValidationError(ligneAValider)) {
                // On signale que la ligne n'est pas en erreur
                ligneAValider.ErrorValidation = false;
            } else {
                // On ne valide pas la ligne
                ligneAValider.IsValidated = false;

                // On signale que la ligne est en erreur
                ligneAValider.ErrorValidation = true;
            }

            // Initialisation des warning de la ligne en cours de validation
            ligneAValider.WarningValidation = false;

            if (actionValidationWarning(ligneAValider)) {
                // La ligne en cours de validation possède des Warning 
                // (le Personnel a déjà reçu une ou plusieurs Primes identique à celles de la ligne en cours de validation)
                ligneAValider.WarningValidation = true;
            }

            // Si il y a eu un changement sur la ligne (une validation ou une dévalidation) alors on signale l'état modifié de la ligne
            if (isValidatedbeforeAction !== ligneAValider.IsValidated) {
                actionIsUpdated(ligneAValider);
            }
        }

        /*
        * @description Met le champ IsUpdated à true si la ligne de RapportPrime est en modification
        */
        function actionValidationError(ligneAValider) {
            var isValid = false;

            // On doit vérifier que la ligne de rapport de prime contienne un personnel, un CI et soit une Date d'Astreinte soit un RapportPrimeLignePrime
            if (ligneAValider) {
                if (ligneAValider.Ci && ligneAValider.Personnel) {

                    if (ligneAValider.ListAstreintes.length > 0) {
                        isValid = true;
                    } else {
                        angular.forEach(ligneAValider.ListPrimes, function (rplp) {
                            if (rplp.Montant && rplp.Montant !== "0.00") {
                                isValid = true;
                            }
                        });
                    }
                }
            }
            return isValid;
        }

        /*
        * @description Retourne si il y a un warning sur la ligne à valider
        */
        function actionValidationWarning(ligneAValider) {
            var isWarning = false;

            // On initialise la liste des Auteur de doublon des primes à vide
            ligneAValider.ListeAuteurDoublonPrime = [];

            // Si l'utilisateur possède des Montant de primes différents de 0 sur cette ligne, alors on vérifie toutes les lignes 
            // pour voir si ce Personnel a déjà reçu ces primes avec un Montant différent de 0
            angular.forEach(ligneAValider.ListPrimes, function (primeAValider) {

                if (primeAValider.Montant && primeAValider.Montant > 0) {
                    // La prime possède un montant non null donc on va vérifier cette prime dans la liste des lignes
                    angular.forEach($ctrl.RapportPrime.ListLignes, function (rpl) {
                        if (!rpl.IsDeleted && rpl.Personnel && rpl.Personnel.PersonnelId === ligneAValider.Personnel.PersonnelId && ligneAValider !== rpl) {

                            angular.forEach(rpl.ListPrimes, function (rplp) {
                                if (rplp.PrimeId === primeAValider.PrimeId && rplp.Montant && rplp.Montant > 0) {
                                    // Alors le Personnel a déjà reçu cette Prime

                                    // La ligne en cours de validation possède des Warning 
                                    // (le Personnel a déjà reçu une ou plusieurs Primes identique à celles de la ligne en cours de validation)
                                    isWarning = true;

                                    // Si l'auteur n'existe pas déjà dans notre tableau d'auteur de doublon de prime on l'ajoute
                                    if (rpl.AuteurCreation
                                        && ligneAValider.ListeAuteurDoublonPrime.indexOf(rpl.AuteurCreation.NomPrenom) === -1) {
                                        // On stocke l'auteur de la prime déjà saisie
                                        ligneAValider.ListeAuteurDoublonPrime.push(rpl.AuteurCreation.NomPrenom);
                                    }
                                }
                            });
                        }
                    });
                }
            });

            return isWarning;
        }

        function actionOnBegin() {
            $ctrl.busy = true;
            ProgressBar.start();
        }

        function actionOnFinally() {
            $ctrl.busy = false;
            $timeout(function () { onScroll(); });
            ProgressBar.complete();
        }

        function actionOnError(reason) {
            if (reason.message) {
                if (reason.message === 'response.data is null') {
                    Notify.error($ctrl.resources.RapportPrime_Rapport_Inexistant);
                }
                else {
                    Notify.error(reason.message);
                }

            }
            else {
                Notify.defaultError();
            }
        }

        function GetCurrentUser() {
            UserService.getCurrentUser().then(function(user) {
                $ctrl.currentUser = user.Personnel;
            });
        }

        $ctrl.saveButtonLocked = function () {
            return $ctrl.busy || $ctrl.rights.isReadOnly;
        };
    }
})(angular);