(function () {
    'use strict';

    var sidebarDetailComponent = {
        templateUrl: '/Areas/Pointage/ListePointagePersonnel/Scripts/Components/sidebar-detail.component.html',
        bindings: {
            resources: '<',
            readOnly: '<',
            displayBanner: '=',
            isGsp: '<',
            isSomopa: '<'
        },
        require: {
            parentCtrl: '^ngController'
        },
        controller: sideBarDetailController

    };

    angular.module('Fred').component('sidebarDetailComponent', sidebarDetailComponent);

    angular.module('Fred').controller('sideBarDetailController', sideBarDetailController);

    sideBarDetailController.$inject = ['UserService', '$scope', 'ProgressBar', 'PointageHelperService', 'DatePickerService', 'PersonnelPickerService', 'PointagePersonnelService', 'Notify'];

    function sideBarDetailController(UserService, $scope, ProgressBar, PointageHelperService, DatePickerService, PersonnelPickerService, PointagePersonnelService, Notify) {

        var $ctrl = this;

        //////////////////////////////////////////////////////////////////
        // Constantes                                                   //
        //////////////////////////////////////////////////////////////////
        const codePrimeEgout = "AE";
        const codePrimeSalissure = "ES";
        const codePrimeInsalubrite = "INS";
        const codePrimeTicketRestaurant = "TR";
        const codePrimeIndemniteRepas = "IR";
        const maxHour = 10;


        //////////////////////////////////////////////////////////////////
        // Déclaration des propriétés publiques                         //
        //////////////////////////////////////////////////////////////////
        $ctrl.pointage = {};
        $ctrl.notValidToSave = false;
        $ctrl.PointagePersonnelService = PointagePersonnelService;

        //////////////////////////////////////////////////////////////////
        // Déclaration des fonctions publiques                          //
        //////////////////////////////////////////////////////////////////
        $ctrl.handleClickValidate = handleClickValidate;
        $ctrl.handleChangeHeureTache = handleChangeHeureTache;
        $ctrl.handleBlurHeureTacheIac = handleBlurHeureTacheIac;
        $ctrl.handleChangeHeureMajoration = handleChangeHeureMajoration;
        $ctrl.handleDeletePickListCodeMajoration = handleDeletePickListCodeMajoration;
        $ctrl.handleDeletePickListCodeMajorationFES = handleDeletePickListCodeMajorationFES;
        $ctrl.handleDeletePickListCodeAbsence = handleDeletePickListCodeAbsence;
        $ctrl.handleDeletePickListCodeDeplacement = handleDeletePickListCodeDeplacement;
        $ctrl.handleDeletePickListCodeZoneDeplacement = handleDeletePickListCodeZoneDeplacement;
        $ctrl.handleDeletePickListMateriel = handleDeletePickListMateriel;
        $ctrl.handleDeletePickListTache = handleDeletePickListTache;
        $ctrl.handleDeletePrime = handleDeletePrime;
        $ctrl.handleDeleteTache = handleDeleteTache;
        $ctrl.handleDeletePickListPrime = handleDeletePickListPrime;
        $ctrl.heuresAbsenceMin = heuresAbsenceMin;
        $ctrl.heuresAbsenceMax = heuresAbsenceMax;
        $ctrl.handleSelectCI = handleSelectCI;
        $ctrl.handleSelectCodeMajoration = handleSelectCodeMajoration;
        $ctrl.handleAddMajoration = handleAddMajoration;
        $ctrl.handleSelectCodeAbsence = handleSelectCodeAbsence;
        $ctrl.handleSelectCodeDeplacement = handleSelectCodeDeplacement;
        $ctrl.handleSelectCodeZoneDeplacement = handleSelectCodeZoneDeplacement;
        $ctrl.handleSelectPrime = handleSelectPrime;
        $ctrl.handleSelectMateriel = handleSelectMateriel;
        $ctrl.handleSelectTache = handleSelectTache;
        $ctrl.handleShowPickList = handleShowPickList;
        $ctrl.handleClickIndemnites = handleClickIndemnites;
        $ctrl.actionRefreshTotalHeure = actionRefreshTotalHeure;
        $ctrl.handleChangeTacheCommentaire = handleChangeTacheCommentaire;
        $ctrl.handleChangeHeureMateriel = handleChangeHeureMateriel;
        $ctrl.handleChangeHeureAbsence = handleChangeHeureAbsence;
        $ctrl.handleBlurHeureAbsenceIac = handleBlurHeureAbsenceIac;
        $ctrl.handleChangeIVD = handleChangeIVD;
        $ctrl.handleChangeHeurePrime = handleChangeHeurePrime;
        $ctrl.handleChangeCommentaire = handleChangeCommentaire;
        $ctrl.handleCheckPrime = handleCheckPrime;
        $ctrl.handleCancel = handleCancel;
        $ctrl.isEtamIac = false;
        $ctrl.IsPrimeHoraire = IsPrimeHoraire;
        $ctrl.IsPrimeJournaliere = IsPrimeJournaliere;
        $ctrl.userOrganizationId = null;
        $ctrl.getDateTime = getDateTime;
        $ctrl.getReferentielRhUrl = getReferentielRhUrl;
        $ctrl.materielTypeList = [
            { Id: 1, Libelle: "Interne" },
            { Id: 2, Libelle: "Externe" }
        ];

        //////////////////////////////////////////////////////////////////
        // Init                                                         //
        //////////////////////////////////////////////////////////////////
        $ctrl.$onInit = function () {
            $scope.$on('chargePointage', function (event) {
                if (PointageHelperService.getPointage() !== $ctrl.pointage) {
                    angular.copy(PointageHelperService.getPointage(), $ctrl.pointage);
                    checkStatut();
                    handleHeureIac();
                }
            });
            $scope.$on('list.total.ci.show', function (evt, data) {
                $ctrl.listPointage = data.listPointages;
            });

            UserService.getCurrentUser().then(function(user) {
                $ctrl.userOrganizationId = user.Personnel.Societe.Organisation.OrganisationId;
                $ctrl.isUserFes = user.Personnel.Societe.Groupe.Code.trim() === 'GFES' ? true : false;
                $ctrl.groupeId = user.Personnel.Societe.GroupeId;
                $ctrl.currentUser = user.Personnel;
            });
        };

        function getReferentielRhUrl() {
            if ($ctrl.isUserFes) {
                if (PersonnelPickerService.IsETAM())
                    return "isETAM=true&";;
                if (PersonnelPickerService.IsOuvrier())
                    return "isOuvrier=true&";
                if (PersonnelPickerService.IsCadre())
                    return "isCadre=true&";
                else
                    return "";
            }
            return "";
        }

        function checkStatut() {
            $ctrl.isIac = PersonnelPickerService.getIsIac();
            if (PersonnelPickerService.IsEtamIac()) {
                $ctrl.isEtamIac = true;
            }
            else {
                $ctrl.isEtamIac = false;
            }
        }
        //////////////////////////////////////////////////////////////////
        // Handlers                                                     //
        //////////////////////////////////////////////////////////////////
        function handleClickValidate() { actionValidate(); }

        function handleChangeHeureTache() {
            actionRefreshTotalHeure();
            actionRefreshHeureNormale();
            PointageHelperService.actionIsUpdated($ctrl.pointage);
            if ($ctrl.isUserFes) {
                checkPointageHeurePerDay();
            }
        }

        function handleHeureIac() {
            if ($ctrl.isUserFes && PersonnelPickerService.getIsIac()) {
                InitHeureAbsenceIac();
                InitHeureTacheIac();
            }
        }

        function InitHeureAbsenceIac() {
            if ($ctrl.pointage.HeureAbsence) {
                $ctrl.pointage.HeureAbsenceIac = $ctrl.pointage.HeureAbsence / 7;
            }
            else {
                $ctrl.pointage.HeureAbsenceIac = 0;
            }
        }

        function InitHeureTacheIac() {
            if ($ctrl.pointage.ListRapportLigneTaches && $ctrl.pointage.ListRapportLigneTaches.length > 0) {
                for (var i = 0; i < $ctrl.pointage.ListRapportLigneTaches.length; i++) {
                    var pointageTache = $ctrl.pointage.ListRapportLigneTaches[i];
                    pointageTache.HeureTacheIac = pointageTache.HeureTache / 7;
                }
            }
        }

        function handleBlurHeureTacheIac(pointageTache) {
            var modulo = pointageTache.HeureTacheIac % 0.25;
            if (modulo !== 0) {
                checkPointageHeurePerDay();
                Notify.error($ctrl.resources.PointagePersonnel_SideBar_SaisieTacheEnJours);
                pointageTache.HeureTacheIac = 0;
                pointageTache.HeureTache = 0;
            }
            else {
                pointageTache.HeureTache = pointageTache.HeureTacheIac * 7;
                handleChangeHeureTache();
            }
        }

        function handleBlurHeureAbsenceIac() {
            var modulo = $ctrl.pointage.HeureAbsenceIac % 0.5;
            if (modulo !== 0) {
                checkPointageHeurePerDay();
                Notify.error($ctrl.resources.PointagePersonnel_SideBar_SaisieAbsenceEnJours);
                $ctrl.pointage.HeureAbsenceIac = 0;
                $ctrl.pointage.HeureAbsence = 0;
            }
            else {
                $ctrl.pointage.HeureAbsence = $ctrl.pointage.HeureAbsenceIac * 7;
                handleChangeHeureAbsence();
            }
        }

        function handleChangeHeureMajoration(majoration) {
            if ($ctrl.isUserFes) {
                checkPointageHeurePerDay();
                checkMajorationTrajetAllerRetour(majoration);
            }

            if (isNaN(parseFloat($ctrl.pointage.HeureMajoration))) {
                $ctrl.pointage.HeureMajoration = 0;
            }
            actionRefreshHeureNormale();
            PointageHelperService.actionIsUpdated($ctrl.pointage);
        }

        // Pour Vérifier pour une journée les majorations Aller/Retour
        function checkMajorationTrajetAllerRetour(majoration) {
            $ctrl.listMajorationCourante = new Array();
            $ctrl.listMajorationCiNotCourante = new Array();
            var majocourante = majoration.CodeMajorationId;
            var day = $ctrl.pointage.DatePointage;
            var ciId = $ctrl.pointage.CiId;
            var majorationAller = 5;
            var majorationRetour = 6;

            //Pour une journée et pour un ci
            $ctrl.listMajorationWithOutCourante = $ctrl.pointage.ListRapportLigneMajorations.filter(x => x.CodeMajorationId !== majocourante);
            $ctrl.listMajorationWithOutCourante.forEach(function (item) {
                if (item.CodeMajorationId === majorationAller && item.HeureMajoration > 0 || item.CodeMajorationId === majorationRetour && item.HeureMajoration > 0) {
                    if (majoration.HeureMajoration > 0 && (majoration.CodeMajorationId === majorationAller || majoration.CodeMajorationId === majorationRetour)) {
                        alertMajorationAllerRetour(majoration);
                    }
                }
            });


            //Pour une journée sur tout les ci 
            $ctrl.listMajorationCiNotCourante = $ctrl.listPointage.filter(x => x.DatePointage === day && x.CiId !== ciId);
            for (var ci = 0; ci < $ctrl.listMajorationCiNotCourante.length; ci++) {
                for (var y = 0; y < $ctrl.listMajorationCiNotCourante[ci].ListRapportLigneMajorations.length; y++) {
                    if ($ctrl.listMajorationCiNotCourante[ci].ListRapportLigneMajorations[y].CodeMajorationId === majorationAller && $ctrl.listMajorationCiNotCourante[ci].ListRapportLigneMajorations[y].HeureMajoration > 0 || $ctrl.listMajorationCiNotCourante[ci].ListRapportLigneMajorations[y].CodeMajorationId === majorationRetour && $ctrl.listMajorationCiNotCourante[ci].ListRapportLigneMajorations[y].HeureMajoration > 0) {
                        if (majoration.HeureMajoration > 0 && (majoration.CodeMajorationId === majorationAller || majoration.CodeMajorationId === majorationRetour)) {
                            alertMajorationAllerRetour(majoration);
                        }
                    }
                }
            }
        }

        function alertMajorationAllerRetour(majoration) {
            Notify.error($ctrl.resources.RapportHebdo_Error_MajorationAller_Retour);
            majoration.HeureMajoration = 0;
            return;
        }

        function getDateTime(dateStr) {
            if (dateStr !== "") {
                var date = new Date(dateStr);
                return date.getUTCHours() + ':' + date.getUTCMinutes();
            }
        }

        function handleChangeHeureAbsence() {
            checkPointageHeurePerDay();
            PointageHelperService.actionIsUpdated($ctrl.pointage);
        }

        function handleChangeHeureMateriel() { PointageHelperService.actionIsUpdated($ctrl.pointage); }

        function handleChangeIVD() { PointageHelperService.actionIsUpdated($ctrl.pointage); }

        function handleChangeHeurePrime() { PointageHelperService.actionIsUpdated($ctrl.pointage); }

        function handleCheckPrime() { PointageHelperService.actionIsUpdated($ctrl.pointage); }

        function handleChangeCommentaire() { PointageHelperService.actionIsUpdated($ctrl.pointage); }

        function handleSelectCI(item) {
            actionSelectCi(item);
            PointageHelperService.actionIsUpdated($ctrl.pointage);
        }

        function handleChangeTacheCommentaire() { PointageHelperService.actionIsUpdated($ctrl.pointage); }

        function handleSelectCodeMajoration(item, index) { actionSelectCodeMajoration(item, index); }

        function handleAddMajoration(item) {
            if (!$ctrl.readOnly) { actionSelectNewMajoration(item); }
        }

        function handleSelectCodeAbsence(item) { actionSelectCodeAbsence(item); }

        function handleSelectCodeDeplacement(item) { actionSelectCodeDeplacement(item); }

        function handleSelectCodeZoneDeplacement(item) { actionSelectCodeZoneDeplacement(item); }

        function handleSelectMateriel(item) { actionSelectMateriel(item); }

        function handleSelectPrime(item) { actionSelectPrime(item); }

        function handleSelectTache(item) { actionSelectTache(item); }

        function handleShowPickList(api) {
            if (api === "CodeAbsence") {
                return '/api/' + api + '/SearchLight/?page={1}&ciId=' + $ctrl.pointage.CiId;
            }
            if (api === "Materiel") {
                return '/api/' + api + '/SearchLight/?page={1}&ciId=' + $ctrl.pointage.CiId;
            }
            if (api === "Tache") {
                return '/api/' + api + '/SearchLight/?page={1}&ciId=' + $ctrl.pointage.CiId;
            }
            if (api === "Prime") {
                if ($ctrl.isUserFes) {
                    var basePrimeByGroupeControllerUrl = '/api/Prime/SearchLight/?societeId={0}&groupeId={1}&';
                    return String.format(basePrimeByGroupeControllerUrl, $ctrl.pointage.Ci.SocieteId, $ctrl.groupeId);
                }
                return '/api/' + api + '/SearchLight/?page={1}&ciId=' + $ctrl.pointage.CiId;
            }
            if (api === "CodeDeplacement") {
                return '/api/' + api + '/SearchLight/?page={1}&ciId=' + $ctrl.pointage.CiId;
            }
            if (api === "CodeZoneDeplacement") {
                return '/api/' + api + '/SearchLight/?page={1}&ciId=' + $ctrl.pointage.CiId;
            }
            return '/api/' + api + '/SearchLight/?page={1}';
        }

        function handleDeletePickListCodeMajoration() {
            $ctrl.pointage.CodeMajoration = null;
            $ctrl.pointage.CodeMajorationId = null;
            $ctrl.pointage.HeureMajoration = null;
            actionRefreshHeureNormale();
            PointageHelperService.actionIsUpdated($ctrl.pointage);
        }

        function handleDeletePickListCodeMajorationFES(index) {
            if ($ctrl.pointage.ListRapportLigneMajorations[index]) {
                if ($ctrl.pointage.ListRapportLigneMajorations[index].IsCreated) {
                    $ctrl.pointage.ListRapportLigneMajorations.splice(index, 1);
                }
                else {
                    $ctrl.pointage.ListRapportLigneMajorations[index].IsDeleted = true;
                }
                actionRefreshHeureNormale();
                PointageHelperService.actionIsUpdated($ctrl.pointage);
            }
        }

        function handleDeletePickListCodeAbsence() {
            $ctrl.pointage.CodeAbsence = null;
            $ctrl.pointage.CodeAbsenceId = null;
            $ctrl.pointage.HeureAbsence = null;
            $ctrl.pointage.NumSemaineIntemperieAbsence = null;
            PointageHelperService.actionIsUpdated($ctrl.pointage);
            $ctrl.notValidToSave = false;
            $ctrl.parentCtrl.saveEnable = true;
        }

        function handleDeletePickListCodeDeplacement() {
            $ctrl.pointage.CodeDeplacement = null;
            $ctrl.pointage.CodeDeplacementId = null;
            PointageHelperService.actionIsUpdated($ctrl.pointage);
        }

        function handleDeletePickListCodeZoneDeplacement() {
            $ctrl.pointage.CodeZoneDeplacement = null;
            $ctrl.pointage.CodeZoneDeplacementId = null;
            PointageHelperService.actionIsUpdated($ctrl.pointage);
        }

        function handleDeletePickListMateriel() {
            $ctrl.pointage.Materiel = null;
            $ctrl.pointage.MaterielId = null;
            $ctrl.pointage.MaterielNomTemporaire = null;
            $ctrl.pointage.MaterielMarche = 0;
            $ctrl.pointage.MaterielPanne = 0;
            $ctrl.pointage.MaterielIntemperie = 0;
            $ctrl.pointage.MaterielArret = 0;
            PointageHelperService.actionIsUpdated($ctrl.pointage);
        }

        function handleDeletePickListPrime(pointagePrime) {
            pointagePrime.Prime = null;
            pointagePrime.PrimeId = null;
            PointageHelperService.actionIsUpdated($ctrl.pointage);
        }

        function handleDeletePickListTache(pointageTache) {
            if (!$ctrl.readOnly && $ctrl.pointage.MonPerimetre && !$ctrl.pointage.Cloture) {
                pointageTache.Tache = null;
                pointageTache.TacheId = null;
                pointageTache.IsDeleted = true;
                actionRefreshHeureNormale();
                PointageHelperService.actionIsUpdated($ctrl.pointage);
            }
        }

        function handleDeletePrime(pointagePrime) {
            if (!$ctrl.readOnly && $ctrl.pointage.MonPerimetre && !$ctrl.pointage.Cloture && !$ctrl.pointage.IsGenerated) {
                if (pointagePrime.RapportLignePrimeId !== 0) {
                    pointagePrime.IsDeleted = true;
                }
                else {
                    for (var i = 0; i < $ctrl.pointage.ListRapportLignePrimes.length; i++) {
                        if ($ctrl.pointage.ListRapportLignePrimes[i].PrimeId === pointagePrime.PrimeId) {
                            $ctrl.pointage.ListRapportLignePrimes.splice(i, 1);
                        }
                    }
                }
            }
            PointageHelperService.actionIsUpdated($ctrl.pointage);
            PointageHelperService.refreshLibelleAbregePrime($ctrl.pointage);
        }

        function handleDeleteTache(pointageTache) {
            if (!$ctrl.readOnly && $ctrl.pointage.MonPerimetre && !$ctrl.pointage.Cloture && !$ctrl.pointage.IsGenerated) {
                if (pointageTache.RapportLigneTacheId !== undefined && pointageTache.RapportLigneTacheId !== 0) {
                    pointageTache.IsDeleted = true;
                }
                else {
                    for (var i = 0; i < $ctrl.pointage.ListRapportLigneTaches.length; i++) {
                        if ($ctrl.pointage.ListRapportLigneTaches[i].TacheId === pointageTache.TacheId) {
                            $ctrl.pointage.ListRapportLigneTaches.splice(i, 1);
                        }
                    }
                }
            }
            actionRefreshTotalHeure();
            actionRefreshHeureNormale();
            PointageHelperService.actionIsUpdated($ctrl.pointage);
        }

        function handleClickIndemnites() {
            if (!$ctrl.readOnly && $ctrl.pointage.MonPerimetre && !$ctrl.pointage.Cloture && !$ctrl.pointage.IsGenerated) {
                actionRefreshIndemnites();
                PointageHelperService.actionIsUpdated($ctrl.pointage);
            }
        }

        function handleCancel() {
            $ctrl.displayBanner = false;
            $ctrl.parentCtrl.actionToggleSidebar($ctrl.displayBanner);
        }
        //////////////////////////////////////////////////////////////////
        // Actions                                                      //
        //////////////////////////////////////////////////////////////////
        function actionValidate() {
            if (!$ctrl.busy) {
                $ctrl.displayBanner = false;
                ProgressBar.start(true);
                $ctrl.parentCtrl.actionToggleSidebar($ctrl.displayBanner);
                $ctrl.busy = true;
                var day = $ctrl.pointage.Day;
                if ($ctrl.isUserFes) {
                    checkListMajoration();
                }
                // Passe le pointage au serveur, vérifie les erreurs et retourne le pointage
                PointagePersonnelService.CheckPointage($ctrl.pointage).then(function (value) {
                    var result = value.data;
                    result.View = $ctrl.pointage.View;
                    result.Day = day;
                    result.IsRapportLocked = $ctrl.pointage.IsRapportLocked;
                    PointageHelperService.refreshLibelleAbregePrime(result);
                    PointageHelperService.setPointage(result);
                    $scope.$emit('validatePointage');
                    $ctrl.busy = false;
                    ProgressBar.complete();
                }, function (reason) {
                    $ctrl.busy = false;
                    ProgressBar.complete();
                    //$scope.sendNotificationError(reason.Message);
                });
            }
        }

        function actionRefreshTotalHeure() {
            return PointageHelperService.refreshTotalHeure($ctrl.pointage);
        }

        function actionRefreshHeureNormale() {
            PointageHelperService.refreshHeureNormale($ctrl.pointage);
        }

        function heuresAbsenceMin(pointage) {
            return PointageHelperService.heuresAbsenceMin(pointage);
        }

        function heuresAbsenceMax(pointage) {
            return PointageHelperService.heuresAbsenceMax(pointage);
        }

        // Retourne vrai si la prime est de type horaire
        function IsPrimeHoraire(prime) {
            return PointageHelperService.IsPrimeHoraire(prime);
        }

        // Retourne vrai si la prime est de type journaliere
        function IsPrimeJournaliere(prime) {
            return PointageHelperService.IsPrimeJournaliere(prime);
        }

        function actionSelectCi(item) {
            $ctrl.pointage.CiId = item.CiId;
            $ctrl.pointage.Ci = item;
            var date = new Date(DatePickerService.getPeriode());
            var year = date.getFullYear();
            var month = date.getMonth() + 1; // l'index du premier mois est 0
            PointagePersonnelService.IsInMyPerimetre($ctrl.pointage.CiId)
                .then(setPointagePerimetre)
                .catch(actionFailLoadServer);
            PointagePersonnelService.CiIsCloture($ctrl.pointage.CiId, year, month)
                .then(setPointageCloture)
                .catch(actionFailLoadServer);
        }

        function checkListMajoration() {
            var listToDelete = [];
            if ($ctrl.pointage.ListRapportLigneMajorations) {
                for (var index = 0; index < $ctrl.pointage.ListRapportLigneMajorations.length; index++) {
                    if (!$ctrl.pointage.ListRapportLigneMajorations[index].HeureMajoration || $ctrl.pointage.ListRapportLigneMajorations[index].HeureMajoration === '' || parseFloat($ctrl.pointage.ListRapportLigneMajorations[index].HeureMajoration) === 0) {
                        if ($ctrl.pointage.ListRapportLigneMajorations[index].IsCreated) {
                            listToDelete.push(index);
                        }
                        else {
                            $ctrl.pointage.ListRapportLigneMajorations[index].IsDeleted = true;
                        }
                    }
                }
                listToDelete.sort(function (a, b) { return b - a; });
                listToDelete.forEach(function (i) {
                    $ctrl.pointage.ListRapportLigneMajorations.splice(i, 1);
                });
                actionRefreshHeureNormale();
            }
        }

        function actionSelectCodeMajoration(item, index) {
            if (index !== null && $ctrl.isUserFes) {
                $ctrl.pointage.ListRapportLigneMajorations[index].CodeMajorationId = item.CodeMajorationId;
                $ctrl.pointage.ListRapportLigneMajorations[index].CodeMajoration = item;
            }
            else if (!$ctrl.isUserFes) {
                $ctrl.pointage.CodeMajorationId = item.CodeMajorationId;
                $ctrl.pointage.CodeMajoration = item;
            }
            PointageHelperService.actionIsUpdated($ctrl.pointage);
        }

        function actionSelectNewMajoration(item) {

            //$ctrl.pointage.ListRapportLigneMajorations.push()
            var pointageMajorationExistant = {};
            if ($ctrl.pointage.ListRapportLigneMajorations.some(x => x.CodeMajorationId === item.CodeMajorationId)) {
                pointageMajorationExistant = $ctrl.pointage.ListRapportLigneMajorations.find(x => x.CodeMajorationId === item.CodeMajorationId);
                if (pointageMajorationExistant.IsDeleted) {
                    pointageMajorationExistant.IsChecked = true;
                    pointageMajorationExistant.IsDeleted = false;
                }
                else {
                    Notify.error('Majoration ' + item.Code + ' existe déjà.');
                }
            }
            else {
                var pointageMajoration = {
                    HeureMajoration: null,
                    IsChecked: true,
                    IsCreated: true,
                    IsDeleted: false,
                    IsUpdated: false,
                    CodeMajoration: item,
                    CodeMajorationId: item.CodeMajorationId,
                    RapportLigne: null,
                    RapportLigneId: $ctrl.pointage.PointageId,
                    RapportLigneMajorationId: 0
                };
                $ctrl.pointage.ListRapportLigneMajorations.push(pointageMajoration);
            }
            PointageHelperService.actionIsUpdated($ctrl.pointage);
        }

        function actionSelectCodeAbsence(item) {
            var previousSomme = 0;
            $ctrl.pointage.CodeAbsenceId = item.CodeAbsenceId;
            $ctrl.pointage.CodeAbsence = item;

            if (!$ctrl.isUserFes) {
                PointageHelperService.setNumSemaineIntemperie($ctrl.pointage);
            }

            PointageHelperService.refreshHeuresAbsenceDefaut($ctrl.pointage);
            PointageHelperService.actionIsUpdated($ctrl.pointage);
            var list = $ctrl.listPointage.filter(x => x.DatePointage === $ctrl.pointage.DatePointage
                && x.CiId !== $ctrl.pointage.CiId
                && !x.IsDeleted);
            list.forEach(function (node) {
                previousSomme += parseFloat(node.HeureNormale) + parseFloat(node.HeureAbsence);
            });

            var somme = $ctrl.pointage.HeureNormale + $ctrl.pointage.HeureAbsence;
            if (somme + previousSomme > maxHour) {
                Notify.error($ctrl.resources.PointagePersonnel_Depassement_journee_FES);
                $ctrl.notValidToSave = true;
            }
            else {
                $ctrl.notValidToSave = false;
            }
        }

        function actionSelectCodeDeplacement(item) {
            $ctrl.pointage.CodeDeplacementId = item.CodeDeplacementId;
            $ctrl.pointage.CodeDeplacement = item;
            PointageHelperService.actionIsUpdated($ctrl.pointage);
        }

        function actionSelectCodeZoneDeplacement(item) {
            $ctrl.pointage.CodeZoneDeplacementSaisiManuellement = true;
            $ctrl.pointage.CodeZoneDeplacementId = item.CodeZoneDeplacementId;
            $ctrl.pointage.CodeZoneDeplacement = item;
            PointageHelperService.actionIsUpdated($ctrl.pointage);
        }

        function actionSelectMateriel(item) {
            $ctrl.pointage.MaterielId = item.MaterielId;
            $ctrl.pointage.Materiel = item;
            $ctrl.pointage.MaterielNomTemporaire = item.LibelleLong;
            actionRefreshTotalHeure();
            PointageHelperService.actionIsUpdated($ctrl.pointage);
        }

        function actionSelectPrime(prime) {
            if (IsPrimeFesOK(prime)) {
                var pointagePrimeExistant = {};
                if ($ctrl.pointage.ListRapportLignePrimes.some(x => x.PrimeId === prime.PrimeId)) {
                    pointagePrimeExistant = $ctrl.pointage.ListRapportLignePrimes.find(x => x.PrimeId === prime.PrimeId);
                    if (pointagePrimeExistant.IsDeleted) {
                        pointagePrimeExistant.IsChecked = true;
                        pointagePrimeExistant.IsDeleted = false;
                    }
                    else {
                        Notify.error($ctrl.resources.Rapport_RapportController_PrimeExisteDeja_Info);
                    }
                }
                else {
                    var pointagePrime = {
                        HeurePrime: null,
                        IsChecked: true,
                        IsCreated: true,
                        IsDeleted: false,
                        IsUpdated: false,
                        Prime: prime,
                        PrimeId: prime.PrimeId,
                        RapportLigne: null,
                        RapportLigneId: $ctrl.pointage.PointageId,
                        RapportLignePrimeId: 0
                    };
                    $ctrl.pointage.ListRapportLignePrimes.push(pointagePrime);
                }
                PointageHelperService.actionIsUpdated($ctrl.pointage);
            }
        }

        function actionSelectTache(item) {
            if (CheckTachesForCiTypeEtude(item)) {
                var exist = false;
                for (var i = 0; i < $ctrl.pointage.ListRapportLigneTaches.length; i++) {
                    var ligneTache = $ctrl.pointage.ListRapportLigneTaches[i];
                    if (ligneTache.TacheId === item.TacheId) {
                        exist = true;
                        if (ligneTache.IsDeleted) {
                            ligneTache.IsDeleted = false;
                            ligneTache.HeureTache = 0;
                        }
                        else {
                            Notify.error($ctrl.resources.Pointage_PointageController_TacheExisteDeja_Info);
                        }
                    }
                }
                if (!exist) {
                    var pointageTache = {
                        Commentaire: "",
                        HeureTache: null,
                        IsCreated: true,
                        IsDeleted: false,
                        IsUpdated: false,
                        RapportLigne: null,
                        RapportLigneId: $ctrl.pointage.PointageId,
                        RapportLigneTacheId: 0,
                        Tache: item,
                        TacheId: item.TacheId
                    };
                    $ctrl.pointage.ListRapportLigneTaches.push(pointageTache);
                }
                PointageHelperService.actionIsUpdated($ctrl.pointage);
            }
        }

        function CheckTachesForCiTypeEtude(item) {
            if ($ctrl.isUserFes && item.Code === $ctrl.resources.CI_Etude_DefaultTaskCode && $ctrl.pointage.Ci.TypeCI === $ctrl.resources.CI_Search_CIType_Etude) {
                Notify.error($ctrl.resources.Message_Use_SpecificTask_For_CiTypeEtude);
                return false;
            }

            return true;
        }

        function actionRefreshIndemnites() {
            ProgressBar.start();
            PointagePersonnelService.RefreshIndemniteDeplacement($ctrl.pointage)
                .then(function (value) {
                    var result = value.data;
                    $ctrl.pointage.CodeDeplacement = result.CodeDeplacement;
                    $ctrl.pointage.CodeDeplacementId = result.CodeDeplacementId;
                    $ctrl.pointage.CodeZoneDeplacement = result.CodeZoneDeplacement;
                    $ctrl.pointage.CodeZoneDeplacementId = result.CodeZoneDeplacementId;
                    $ctrl.pointage.DeplacementIV = result.DeplacementIV;
                    $ctrl.pointage.CodeZoneDeplacementSaisiManuellement = result.CodeZoneDeplacementSaisiManuellement;
                    if (result.Warnings) {
                        angular.forEach(result.Warnings, function (val, key) {
                            Notify.warning(val);
                        });
                    }
                })
                .catch(function () { Notify.error($ctrl.resources.Global_Notification_Error); })
                .finally(ProgressBar.complete);
        }

        function setPointageCloture(result) {
            $ctrl.pointage.Cloture = result.data;
        }

        function setPointagePerimetre(result) {
            $ctrl.pointage.MonPerimetre = result.data;
        }

        function actionFailLoadServer() {
            console.log(reason);
        }

        $ctrl.handleLookupUrl = function (api) {
            if (api === "CodeMajoration") {
                var baseMajorationByGroupeControllerUrl = '/api/CodeMajoration/SearchLight/?groupeId={0}&isHeureNuit={1}&';
                if ($ctrl.isUserFes) {
                    return String.format(baseMajorationByGroupeControllerUrl, $ctrl.groupeId, false);
                }

                return String.format(baseMajorationByGroupeControllerUrl, $ctrl.groupeId, null);
            }
            if (api === "Prime") {
                if ($ctrl.isUserFes) {
                    var basePrimeControllerUrl = '/api/Prime/SearchLight/?groupeId={0}&ciId={1}&isRapportPrime={2}&';
                    return String.format(basePrimeControllerUrl, $ctrl.groupeId, $ctrl.pointage.CiId, $ctrl.isEtamIac);
                }

                return '/api/' + "Prime" + '/SearchLight/?ciId=' + $ctrl.pointage.CiId + "&";
            }
        };

        function checkPointageHeurePerDay() {
            var previousTotalHours = 0;
            var sumTachePointageCourant = 0;
            var sumMajorationTNHCourant = 0;
            var totalHourAbsence = isNaN(parseFloat($ctrl.pointage.HeureAbsence)) ? 0 : parseFloat($ctrl.pointage.HeureAbsence);
            if ($ctrl.pointage.ListRapportLigneTaches !== undefined) {
                sumTachePointageCourant += calculatesumTachePointageCourant($ctrl.pointage);
            }

            var pointageIndex = PointageHelperService.getPointageIndex();
            let listpointage = [...$ctrl.listPointage];
            listpointage.splice(pointageIndex.index, 1);
            var ListHourBack = listpointage.filter(p => p.DatePointage === $ctrl.pointage.DatePointage && !p.IsDeleted);
            ListHourBack.forEach(function (node) {
                previousTotalHours += parseFloat(node.HeureNormale) + parseFloat(node.HeureMajoration) + parseFloat(node.HeureAbsence);
            });

            var pointageList = $ctrl.listPointage.filter(x => !x.IsDeleted && x.DatePointage === $ctrl.pointage.DatePointage);
            if (!$ctrl.isIac) {
                if ($ctrl.pointage.ListRapportLigneMajorations !== undefined) {
                    sumMajorationTNHCourant += calculatesumMajorationNuitPointageCourant($ctrl.pointage);
                }

                if (pointageList) {
                    for (var k = 0; k < pointageList.length; k++) {
                        if (pointageList[k].ListRapportLigneMajorations !== undefined) {
                            sumMajorationTNHCourant += calculatesumMajorationNuitPointageCourant(pointageList[k]);
                        }
                    }
                }
            }

            var totalday = sumTachePointageCourant + sumMajorationTNHCourant + totalHourAbsence + parseFloat(previousTotalHours);
            if ($ctrl.isUserFes && !$ctrl.isIac && totalday > maxHour) {
                Notify.error($ctrl.resources.PointagePersonnel_Depassement_journee_FES);
                $ctrl.notValidToSave = true;
                return;
            }

            if ($ctrl.isUserFes && $ctrl.isIac && totalday > 7) {
                Notify.error($ctrl.resources.PointagePersonnel_Depassement_journee_FES_IAC);
                $ctrl.notValidToSave = true;
                return;
            }

            $ctrl.notValidToSave = false;
            $ctrl.parentCtrl.saveEnable = true;
        }

        function IsPrimeFesOK(prime) {
            if ($ctrl.isUserFes && $ctrl.pointage.ListRapportLignePrimes && $ctrl.pointage.ListRapportLignePrimes.length > 0) {
                if (prime.Code === codePrimeEgout && $ctrl.pointage.ListRapportLignePrimes.some(x => !x.IsDeleted && (x.Prime.Code === codePrimeInsalubrite || x.Prime.Code === codePrimeSalissure))
                    || (prime.Code === codePrimeInsalubrite || prime.Code === codePrimeSalissure) && $ctrl.pointage.ListRapportLignePrimes.some(x => !x.IsDeleted && x.Prime.Code === codePrimeEgout)) {
                    Notify.error($ctrl.resources.Erreur_Prime_Egout);
                    return false;
                }
                if (prime.Code === codePrimeIndemniteRepas && $ctrl.pointage.ListRapportLignePrimes.some(x => !x.IsDeleted && x.Prime.Code === codePrimeTicketRestaurant)
                    || prime.Code === codePrimeTicketRestaurant && $ctrl.pointage.ListRapportLignePrimes.some(x => !x.IsDeleted && x.Prime.Code === codePrimeIndemniteRepas)) {
                    Notify.error($ctrl.resources.Erreur_Prime_Repas);
                    return false;
                }
                else return true;
            }
            else {
                return true;
            }
        }

        function calculatesumTachePointageCourant(pointageList) {
            var sumTachePointage = 0;
            for (var z = 0; z < pointageList.ListRapportLigneTaches.length; z++) {
                if (!pointageList.ListRapportLigneTaches[z].IsDeleted &&
                    !isNaN(parseFloat(pointageList.ListRapportLigneTaches[z].HeureTache))) {
                    sumTachePointage += parseFloat(pointageList.ListRapportLigneTaches[z].HeureTache);
                }
            }

            return sumTachePointage;
        }

        function calculatesumMajorationNuitPointageCourant(pointageList) {
            var sumMajorationNuit = 0;
            for (var j = 0; j < pointageList.ListRapportLigneMajorations.length; j++) {
                if (pointageList.ListRapportLigneMajorations[j].CodeMajoration.IsHeureNuit &&
                    !isNaN(parseFloat(pointageList.ListRapportLigneMajorations[j].HeureMajoration))) {
                    sumMajorationNuit += parseFloat(pointageList.ListRapportLigneMajorations[j].HeureMajoration);
                }
            }

            return sumMajorationNuit;
        }
    }
})();