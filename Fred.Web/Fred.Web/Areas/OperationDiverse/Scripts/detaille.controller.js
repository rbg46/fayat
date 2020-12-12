(function (angular) {
    'use strict';
    angular.module('Fred').controller('DetailleController', DetailleController);

    DetailleController.$inject = ['$scope', 'Notify', 'ProgressBar', 'OperationDiverseService', 'fredSubscribeService', 'CIService', 'UserService', '$q', '$filter', 'fredDialog'];

    function DetailleController($scope, Notify, ProgressBar, OperationDiverseService, fredSubscribeService, CIService, UserService, $q, $filter, fredDialog) {

        var attachToAnOD;
        var backupListSelectedAccountingEntries = []; // Lors de certaines mises à jour, on recharge les ECs et on perd leur valeur "Selected"
        var $ctrl = this;
        $ctrl.resources = resources;
        $ctrl.handleCancel = handleCancel;
        $ctrl.handleCreate = handleCreate;
        $ctrl.handleSave = handleSave;
        $ctrl.handleCheckFullList = handleCheckFullList;
        $ctrl.handleCheckAllAccountingEntries = handleCheckAllAccountingEntries;
        $ctrl.handleCheckAccountingEntry = handleCheckAccountingEntry;
        $ctrl.handleCheckLinkedOD = handleCheckLinkedOD;
        $ctrl.handleCheckUnlinkedOD = handleCheckUnlinkedOD;
        $ctrl.handleEditOD = handleEditOD;
        $ctrl.handleDelete = handleDelete;
        $ctrl.handleLinkSelectedUnlinkedOD = handleLinkSelectedUnlinkedOD;
        $ctrl.handleUnlinkSelectedLinkedOD = handleUnlinkSelectedLinkedOD;
        $ctrl.handleChangeAboInputs = handleChangeAboInputs;
        $ctrl.isBusy = false;
        $ctrl.initDatepicker = false;
        $scope.checkDisplayOptions = "close-right-panel";
        $ctrl.detailOdMontantComptaLibelle = $ctrl.resources.Libelle_MontantCompta;

        $scope.init = function init(societeId, id, year, month, familleOdId) {
            $scope.societeId = societeId;
            $scope.id = id;
            $scope.year = year;
            $scope.month = month;
            var dateMinAbonnement = moment(new Date(year, month, 1)).add(-1, 'M');
            $ctrl.DateMinAbonnement = dateMinAbonnement;
            var dateMaxAbonnement = moment(new Date(year, month, 0));
            $ctrl.DateMaxAbonnement = dateMaxAbonnement;
            $scope.familleOperationDiverse = familleOdId;
            $ctrl.firstSelectedAccountingEntry = null;
            $ctrl.preFillingInformationOd = false;

            GetListFrequenceAbonnement();

            ProgressBar.start();
            $q.when(familleOdId)
                .then(LoadAccountingEntries)
                .then(function () { return familleOdId; })
                .then(LoadNotRelatedOD)
                .then(function () { return id; })
                .then(GetCI)
                .finally(ProgressBar.complete);

            fredSubscribeService.subscribe({ eventName: 'goBack', callback: actionReturnToOdList, tooltip: resources.Commande_Detail_RetourListeCommande_Tooltip });

            UserService.getCurrentUser().then(function (user) {
                if (user.Personnel.Societe.Groupe.Code === 'GFTP') {
                    $ctrl.detailOdMontantComptaLibelle = $ctrl.resources.Libelle_MontantCompta_FTP;
                }
                if (user.Personnel.Societe.Groupe.Code === 'GRZB') {
                    $ctrl.preFillingInformationOd = true;
                }
            });
        };

        $ctrl.showLookUpTask = function () {
            return '/api/Tache/SearchLight/?page=1&pageSize=20&societeId=&ciId=' + $scope.id;
        };

        $ctrl.selectTask = function (operationDiverse, task) {
            operationDiverse = $scope.operationDiverse;
            operationDiverse.Tache = task;
            operationDiverse.TacheId = task.TacheId;
            operationDiverse.TacheLibelle = task.Libelle;
        };

        $ctrl.selectRessource = function (operationDiverse, ressource) {
            operationDiverse = $scope.operationDiverse;
            operationDiverse.Ressource = ressource;
            operationDiverse.RessourceId = ressource.RessourceId;
            operationDiverse.Ressource.CodeLibelle = ressource.CodeLibelle;
        };

        $ctrl.showLookUpResource = function () {
            return '/api/Ressource/SearchLight/?societeId=' + $scope.societeId;
        };

        $ctrl.showLookUpUnite = function () {
            return '/api/Unite/SearchLightBySocieteId/?societeId=' + $scope.societeId;
        };

        $ctrl.selectUnite = function (operationDiverse, unite) {
            operationDiverse = $scope.operationDiverse;
            operationDiverse.Unite = unite;
            operationDiverse.UniteId = unite.UniteId;
            operationDiverse.UniteLibelle = unite.Libelle;
        };

        $ctrl.checkZeroValue = function () {
            $scope.formOperationDiverse.quantite.$setValidity('notZero', parseFloat($scope.operationDiverse.Quantite) !== 0);
            $scope.formOperationDiverse.pu.$setValidity('notZero', parseFloat($scope.operationDiverse.PUHT) !== 0);
        };

        function actionGetLastDateOfOperationDiverseAbonnement() {
            if ($scope.operationDiverse.DateProchaineODAbonnement && $scope.operationDiverse.FrequenceAbonnementModel && $scope.operationDiverse.DureeAbonnement) {
                $scope.operationDiverse.DatePremiereODAbonnement = $filter('toLocaleDate')($scope.operationDiverse.DateProchaineODAbonnement);
                var firstODAbonnementDate = $filter('date')($scope.operationDiverse.DatePremiereODAbonnement, 'MM-dd-yyyy');
                return OperationDiverseService.GetLastDayOfODAbonnement(firstODAbonnementDate, $scope.operationDiverse.FrequenceAbonnementModel.Value, $scope.operationDiverse.DureeAbonnement)
                    .then(function (date) {
                        $scope.operationDiverse.DateDerniereODAbonnement = $filter('toLocaleDate')(date);
                    });
            }
        }

        ///// Private Methods /////

        function getSelectedAccountingEntries() {
            var listSelectedAccountingEntries = [];
            $ctrl.firstSelectedAccountingEntry = null;
            $ctrl.ecritureComptableList.forEach(item => {
                if (item.Selected) {
                    listSelectedAccountingEntries.push(item.EcritureComptableId);
                    SaveFirstSelectedAccountingEntry(item);
                }
            });
            return listSelectedAccountingEntries;
        }

        function SaveFirstSelectedAccountingEntry(item) {
            if ($ctrl.preFillingInformationOd) {
                if ($ctrl.firstSelectedAccountingEntry === undefined || $ctrl.firstSelectedAccountingEntry === null) {
                    $ctrl.firstSelectedAccountingEntry = item;
                }
            }
        }

        /// Chargement des Ecriture Comptable ///
        function LoadAccountingEntries(familleOdId, listSelectedAccountingEntries = []) {
            $ctrl.isBusy = true;
            var date = moment(new Date($scope.year, $scope.month - 1, 1)).format('YYYY-MM-DD');

            return OperationDiverseService.GetEcritureComptables($scope.id, date, familleOdId)
                .success(function (accountingEntries) {
                    LoadAccountingEntriesSuccess(accountingEntries, listSelectedAccountingEntries);
                })
                .finally(function () {
                    $ctrl.isBusy = false;
                });
        }

        function GetCI(ciId) {
            $ctrl.isBusy = true;
            return CIService.GetById({ ciId: ciId }).$promise.then(function (data) { $ctrl.CI = data; });
        }

        function LoadAccountingEntriesSuccess(accountingEntries, selectedAccountingEntriesIds = []) {
            $ctrl.ecritureComptableList = accountingEntries;
            $ctrl.ecritureComptableList.forEach(ec => {
                ec.Selected = selectedAccountingEntriesIds.indexOf(ec.EcritureComptableId) !== -1 || backupListSelectedAccountingEntries.indexOf(ec.EcritureComptableId) !== -1;
            });
            $ctrl.TotalEcritureComptable = sum($ctrl.ecritureComptableList);
            $ctrl.TotalRelatedAmount = 0;
            $ctrl.TotalRelatedAmount = sumOD($ctrl.ecritureComptableList);
        }
        /// Fin Chargement Ecritures comptable


        /// Chargement OD Non Attachée
        function LoadNotRelatedOD(familleOdId) {
            $ctrl.isBusy = true;
            var date = moment(new Date($scope.year, $scope.month - 1, 1)).format('YYYY-MM-DD');
            OperationDiverseService.GetNotRelatedOD($scope.id, date, familleOdId)
                .success(DisplayNotRelatedODList)
                .catch(DisplayNotRelatedODListError)
                .finally(DisplayNotRelatedODFinally);
        }

        function DisplayNotRelatedODList(NotRelatedODList) {
            $ctrl.NotRelatedODList = NotRelatedODList;
            $ctrl.TotalNotRelatedAmount = sum(NotRelatedODList);
        }

        function DisplayNotRelatedODListError() {
            Notify.error($ctrl.resources.Global_Notification_Error);
        }

        function DisplayNotRelatedODFinally() {
            $ctrl.isBusy = false;
        }
        //Fin chargement OD Non Attachée

        /// Chargement OD Attachée
        function LoadRelatedOD(listSelectedAccountingEntries) {
            var date = moment(new Date($scope.year, $scope.month - 1, 1)).format('YYYY-MM-DD');
            ProgressBar.start();
            $ctrl.isBusy = true;
            var listToSend = listSelectedAccountingEntries.length > 0 ? listSelectedAccountingEntries.toString() : null;
            return OperationDiverseService.GetRelatedOD($scope.id, date, $scope.familleOperationDiverse, listToSend)
                .success(DisplayRelatedODList)
                .catch(DisplayRelatedODListError)
                .finally(DisplayRelatedODFinally);
        }

        function DisplayRelatedODList(RelatedODList) {
            $ctrl.RelatedODList = RelatedODList;
            return RelatedODList;
        }

        function DisplayRelatedODListError() {
            Notify.error($ctrl.resources.Global_Notification_Error);
        }

        function DisplayRelatedODFinally() {
            $ctrl.isBusy = false;
            ProgressBar.complete();
        }

        function CreateNotRelatedOD() {
            if ($ctrl.isBusy === false) {
                ProgressBar.start();
                $ctrl.isBusy = true;
                var date = moment(new Date($scope.year, $scope.month - 1, 15)).format('YYYY-MM-DD');
                $scope.operationDiverse.DateComptable = date;
                $scope.operationDiverse.Montant = $scope.operationDiverse.PUHT * $scope.operationDiverse.Quantite;
                OperationDiverseService.CreateOD($scope.operationDiverse)
                    .success(CreateNotRelatedODSuccess)
                    .catch(CreateNotRelatedError)
                    .finally(CreateNotRelatedODFinally);
            }
        }

        function CreateNotRelatedODSuccess() {
            $scope.checkDisplayOptions = "close-right-panel";
        }

        function CreateNotRelatedError() {
            Notify.error($ctrl.resources.Global_Notification_Error);
        }

        function CreateNotRelatedODFinally() {
            LoadNotRelatedOD($scope.familleOperationDiverse);
            $ctrl.isBusy = false;
            ProgressBar.complete();
        }

        function CreateRelatedOD(ecritureComptable) {
            if ($ctrl.isBusy === false) {
                ProgressBar.start();
                $ctrl.isBusy = true;
                var date = moment(new Date($scope.year, $scope.month - 1, 1)).format('YYYY-MM-DD');
                $scope.operationDiverse.DateComptable = date;
                $scope.operationDiverse.EcritureComptableId = ecritureComptable.EcritureComptableId;
                $scope.operationDiverse.Montant = $scope.operationDiverse.PUHT * $scope.operationDiverse.Quantite;
                OperationDiverseService.CreateOD($scope.operationDiverse)
                    .success(CreateRelatedODSuccess)
                    .catch(CreateRelatedODError)
                    .finally(CreateRelatedODFinally);
            }
        }

        function CreateRelatedODSuccess() {
            $scope.checkDisplayOptions = "close-right-panel";
            LoadAccountingEntries($scope.familleOperationDiverse);
        }

        function CreateRelatedODError() {
            Notify.error($ctrl.resources.Global_Notification_Error);
        }

        function CreateRelatedODFinally() {
            handleCheckAccountingEntry();
            $ctrl.isBusy = false;
            ProgressBar.complete();
        }

        function UnlinkODs(ODsToUnlink) {
            ProgressBar.start();
            if ($ctrl.isBusy === false) {
                $ctrl.isBusy = true;
                ODsToUnlink.forEach(operationDiverse => {
                    var AccountingEntriesToUpdate = $ctrl.ecritureComptableList.filter(ec => ec.EcritureComptableId === operationDiverse.EcritureComptableId)[0];
                    AccountingEntriesToUpdate.NombreOD--;
                    AccountingEntriesToUpdate.MontantTotalOD -= operationDiverse.Montant;
                    operationDiverse.EcritureComptableId = null;
                });
                OperationDiverseService.UpdateList(ODsToUnlink)
                    .success(UnlinkODsSuccess(ODsToUnlink))
                    .error(UpdateError);
            }
        }

        function UpdateSuccess() {
            var listSelectedAccountingEntries = getSelectedAccountingEntries();
            LoadAccountingEntries($scope.familleOperationDiverse, listSelectedAccountingEntries);
            LoadRelatedOD(listSelectedAccountingEntries);
            $scope.formOperationDiverse.$setPristine();
            $scope.checkDisplayOptions = "close-right-panel";
            $ctrl.isBusy = false;
            ProgressBar.complete();
        }

        function UnlinkODsSuccess(items) {
            items.forEach(item => {
                item.Selected = false;
                $ctrl.NotRelatedODList.push(item);
                var index = $ctrl.RelatedODList.findIndex(od => od.OperationDiverseId === item.OperationDiverseId);
                $ctrl.RelatedODList.splice(index, 1);
                $ctrl.TotalRelatedAmount -= item.Montant;
                $ctrl.TotalNotRelatedAmount += item.Montant;
            });
            $scope.checkAllLinkedOD = false;
            $scope.formOperationDiverse.$setPristine();
            $ctrl.isBusy = false;
            ProgressBar.complete();
        }

        function LinkODs(ODsToLink, currentEcritureComptableId) {
            ProgressBar.start();
            if ($ctrl.isBusy === false) {
                $ctrl.isBusy = true;
                var listSelectedAccountingEntries = getSelectedAccountingEntries();
                ODsToLink.forEach(operationDiverse => {
                    var newLinkedAccountyEntry = listSelectedAccountingEntries[0];
                    operationDiverse.EcritureComptableId = newLinkedAccountyEntry;
                    var AccountingEntriesToUpdate = $ctrl.ecritureComptableList.filter(ec => ec.EcritureComptableId === newLinkedAccountyEntry)[0];
                    AccountingEntriesToUpdate.NombreOD++;
                    AccountingEntriesToUpdate.MontantTotalOD += operationDiverse.Montant;
                });
                ODsToLink.forEach(operationDiverse => operationDiverse.EcritureComptableId = currentEcritureComptableId);
                OperationDiverseService.UpdateList(ODsToLink)
                    .success(LinkODsSuccess(ODsToLink))
                    .error(UpdateError);
            }
        }

        function LinkODsSuccess(items) {
            items.forEach(item => {
                item.Selected = false;
                $ctrl.RelatedODList.push(item);
                var index = $ctrl.NotRelatedODList.findIndex(od => od.OperationDiverseId === item.OperationDiverseId);
                $ctrl.NotRelatedODList.splice(index, 1);
                $ctrl.TotalRelatedAmount += item.Montant;
                $ctrl.TotalNotRelatedAmount -= item.Montant;
                $ctrl.ecritureComptableList.filter(function (obj) { return obj.EcritureComptableId === item.EcritureComptableId; })[0].MontantTotalOD = sum($ctrl.RelatedODList);
            });
            $scope.checkAllUnlinkedOD = false;
            $ctrl.isBusy = false;
            ProgressBar.complete();
        }

        function UpdateError() {
            $ctrl.isBusy = false;
            ProgressBar.complete();
            Notify.error($ctrl.resources.Global_Notification_Error);
        }

        function UpdateFinally() {
            $ctrl.NotRelatedODList = LoadNotRelatedOD($scope.familleOperationDiverse);
            $ctrl.RelatedODList = LoadRelateOD($scope.familleOperationDiverse, $ctrl.currentEcritureComptable.EcritureComptableId);
            $ctrl.isBusy = false;
            ProgressBar.complete();
        }

        function sum(obj) {
            var sum = 0;
            for (var el in obj) {
                if (obj.hasOwnProperty(el)) {
                    sum += parseFloat(obj[el].Montant);
                }
            }
            return sum;
        }

        function sumOD(obj) {
            var sum = 0;
            for (var el in obj) {
                if (obj.hasOwnProperty(el)) {
                    sum += parseFloat(obj[el].MontantTotalOD);
                }
            }
            return sum;
        }

        function EditOD(operationDiverse) {
            $scope.checkDisplayOptions = "open-right-panel";
            $scope.operationDiverse = angular.copy(operationDiverse);
            $ctrl.initDatepicker = false;
        }

        function DeleteOD(operationDiverse) {
            ProgressBar.start();
            if ($ctrl.isBusy === false) {
                $ctrl.isBusy = true;
                OperationDiverseService.Delete(operationDiverse)
                    .success(DeleteSuccess)
                    .error(DeleteError)
                    .finally(DeleteFinally);
            }
        }

        function GetListFrequenceAbonnement() {
            OperationDiverseService.GetListFrequenceAbonnement()
                .then((response) => {
                    $scope.frequenceAboList = response.data;
                });
        }

        function DeleteSuccess() {
            $scope.checkDisplayOptions = "close-right-panel";
            LoadAccountingEntries($scope.familleOperationDiverse);
        }

        function DeleteError() {
            Notify.error($ctrl.resources.Global_Notification_Error);
        }

        function DeleteFinally() {
            $ctrl.NotRelatedODList = LoadNotRelatedOD($scope.familleOperationDiverse);
            backupListSelectedAccountingEntries = getSelectedAccountingEntries();
            $ctrl.RelatedODList = LoadRelatedOD(backupListSelectedAccountingEntries);
            $scope.allAccountingEntriesChecked = backupListSelectedAccountingEntries.length === $ctrl.ecritureComptableList.length;
            $ctrl.isBusy = false;
            ProgressBar.complete();
        }

        function UpdateRelatedOD(operationDiverse) {
            ProgressBar.start();
            if ($ctrl.isBusy === false) {
                $ctrl.isBusy = true;
                operationDiverse.Montant = operationDiverse.Quantite * operationDiverse.PUHT;
                OperationDiverseService.Update(operationDiverse)
                    .success(UpdateSuccess)
                    .error(UpdateError)
                    .finally(UpdateFinally);
            }
        }

        function UpdateAbonnementRelatedOD(operationDiverse) {
            ProgressBar.start();
            if ($ctrl.isBusy === false) {
                $ctrl.isBusy = true;
                operationDiverse.Montant = operationDiverse.Quantite * operationDiverse.PUHT;
                OperationDiverseService.UpdateAbonnement(operationDiverse)
                    .success(UpdateSuccess)
                    .error(UpdateError)
                    .finally(UpdateFinally);
            }
        }

        function actionReturnToOdList() {
            window.location = "/OperationDiverse/OperationDiverse/?CiId=" + $scope.id + "&Year=" + $scope.year + "&Month=" + $scope.month;
        }

        function fredDialogConfirmationUpdateConfiRm() {
            UpdateRelatedOD($scope.operationDiverse);
        }

        function fredDialogConfirmationUpdate() {
            fredDialog.confirmation($ctrl.resources.OperationDiverse_Confirmation_Doublon_Abonnnement, ``, ``, $ctrl.resources.OperationDiverse_Oui, $ctrl.resources.OperationDiverse_Annuler, fredDialogConfirmationUpdateConfiRm);
        }

        ///// HANDLER ////

        function handleCancel() {
            $scope.checkDisplayOptions = "close-right-panel";
            $scope.formOperationDiverse.$setPristine();
        }

        function handleCreate(shouldBeAttachToAnOD) {
            $ctrl.actionODName = "Ajouter une OD";
            $ctrl.isBusy = true;
            attachToAnOD = shouldBeAttachToAnOD;
            if (attachToAnOD && $ctrl.currentEcritureComptable === undefined) {
                Notify.error($ctrl.resources.Global_Notification_Error);
            }
            else {
                $scope.formOperationDiverse.$setPristine();

                var firstSelectedAccountingEntryId = $ctrl.firstSelectedAccountingEntry !== null ? $ctrl.firstSelectedAccountingEntry.EcritureComptableId : null;

                return OperationDiverseService.GetPreFillingOperationDiverse($scope.id, firstSelectedAccountingEntryId, $scope.familleOperationDiverse)
                    .success(function (preFillingOd) {
                        $scope.operationDiverse = preFillingOd;
                        $scope.checkDisplayOptions = "open-right-panel";
                    })
                    .error(function () {
                        Notify.error($ctrl.resources.Global_Notification_Error);
                    })
                    .finally(function () {
                        $ctrl.isBusy = false;
                    });
            }
        }

        function handleSave() {
            $ctrl.checkZeroValue();
            if ($scope.formOperationDiverse.$valid) {
                if ($scope.operationDiverse.EcritureComptableId !== undefined && $scope.operationDiverse.EcritureComptableId !== null) {
                    var dateProchaineODAbonnement = $filter('toLocaleDate')($scope.operationDiverse.DateProchaineODAbonnement);
                    var dateDerniereODAbonnement = $filter('toLocaleDate')($scope.operationDiverse.DateDerniereODAbonnement);
                    var datePreviousODAbonnement = $filter('toLocaleDate')($scope.operationDiverse.DatePreviousODAbonnement);

                    if ($scope.operationDiverse.EstUnAbonnement === true && datePreviousODAbonnement >= dateProchaineODAbonnement && datePreviousODAbonnement <= dateDerniereODAbonnement) {
                        fredDialogConfirmationUpdate();
                    }
                    else {
                        if ($scope.operationDiverse.FrequenceAbonnementModel) {
                            // Cas où on avait un OD abonnement initialement et on ne le souhaite plus (maj de l'OD en décochant la case Abonnement)
                            UpdateAbonnementRelatedOD($scope.operationDiverse);
                        }
                        else {
                            if ($ctrl.preFillingInformationOd) {
                                // Cas où on pré-remplie les champs de la création d'une OD avec les informations de l'écriture comptable sélectionnée (seulement groupe RZB)
                                CreateRelatedOD($ctrl.firstSelectedAccountingEntry);
                            }
                            else {
                                UpdateRelatedOD($scope.operationDiverse);
                            }
                        }
                    }
                }
                else if (attachToAnOD === true || ($scope.operationDiverse.EcritureComptableId !== undefined && $scope.operationDiverse.EcritureComptableId !== null)) {
                    CreateRelatedOD($ctrl.currentEcritureComptable);
                }
                else {
                    CreateNotRelatedOD();
                }
            }
            else {
                Notify.error('Au moins une information est manquante, impossible d\'enregistrer');
            }
        }

        // Sur clic checkbox "sélectionner tout", check toute la liste associée
        function handleCheckFullList(list, checkAll) {
            list.forEach(item => item.Selected = checkAll);
        }

        // Traitement spécifique au check de toutes les écritures comptables
        function handleCheckAllAccountingEntries() {
            var listSelectedAccountingEntries = getSelectedAccountingEntries();
            LoadRelatedOD(listSelectedAccountingEntries);
        }

        // Traitement du check d'une écriture comptable
        function handleCheckAccountingEntry() {
            var listSelectedAccountingEntries = getSelectedAccountingEntries();
            $scope.allAccountingEntriesChecked = listSelectedAccountingEntries.length === $ctrl.ecritureComptableList.length;
            LoadRelatedOD(listSelectedAccountingEntries);
        }

        // Traitement du check d'une OD rattachée
        function handleCheckLinkedOD() {
            $scope.checkAllLinkedOD = true;
            $ctrl.RelatedODList.forEach(item => {
                if (!item.Selected) {
                    $scope.checkAllLinkedOD = false;
                }
            });
        }

        // Traitement du check d'une OD non rattachée
        function handleCheckUnlinkedOD() {
            $scope.checkAllUnlinkedOD = true;
            $ctrl.NotRelatedODList.forEach(item => {
                if (!item.Selected) {
                    $scope.checkAllUnlinkedOD = false;
                }
            });
        }

        // Traitement du détachement d'ODs rattachées
        function handleUnlinkSelectedLinkedOD() {
            if ($ctrl.RelatedODList) {
                var ODsToUnlink = [];
                $ctrl.RelatedODList.forEach(item => {
                    if (item.Selected) {
                        ODsToUnlink.push(item);
                    }
                });
                if (ODsToUnlink.length > 0) {
                    UnlinkODs(ODsToUnlink);
                }
                else {
                    Notify.error($ctrl.resources.OperationDiverse_AucuneOperationDiverseSelectionnee);
                }
            }
            else {
                Notify.error($ctrl.resources.OperationDiverse_AucuneOperationDiverseSelectionnee);
            }
        }

        // Traitement du rattachement d'ODs détachées
        function handleLinkSelectedUnlinkedOD() {
            if ($ctrl.NotRelatedODList) {
                var listSelectedAccountingEntries = getSelectedAccountingEntries();
                if (listSelectedAccountingEntries.length === 1) {
                    var ODsToLink = [];
                    $ctrl.NotRelatedODList.forEach(item => {
                        if (item.Selected) {
                            ODsToLink.push(item);
                        }
                    });
                    if (ODsToLink.length > 0) {
                        LinkODs(ODsToLink, listSelectedAccountingEntries[0]);
                    }
                    else {
                        Notify.error($ctrl.resources.OperationDiverse_AucuneOperationDiverseSelectionnee);
                    }
                }
                else if (listSelectedAccountingEntries.length > 1) {
                    Notify.error($ctrl.resources.OperationDiverse_MultipleEcrituresComptablesSelected_Error);
                }
                else {
                    Notify.error($ctrl.resources.OperationDiverse_NoEcritureComptableSelected_Error);
                }
            }
        }

        function handleEditOD(operationDiverse) {
            $ctrl.actionODName = "Modifier une OD";
            $ctrl.operationDiverse = operationDiverse;
            EditOD(operationDiverse);
        }

        function handleDelete() {
            DeleteOD($ctrl.operationDiverse);
        }

        function handleChangeAboInputs() {
            if ($ctrl.initDatepicker === true) {
                $q.when()
                    .then(ProgressBar.start)
                    .then(actionGetLastDateOfOperationDiverseAbonnement)
                    .finally(ProgressBar.complete);
            }
            $ctrl.initDatepicker = true;
        }
        //// FIN HANDLER ///
    }
}(angular));
