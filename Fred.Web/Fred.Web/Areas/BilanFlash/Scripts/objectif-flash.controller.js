(function (angular) {
    'use strict';

    angular.module('Fred').controller('ObjectifFlashController', ObjectifFlashController);

    ObjectifFlashController.$inject = ['$scope', '$uibModal', 'Notify', 'ProgressBar', 'fredSubscribeService', 'ObjectifFlashService', 'ModelStateErrorManager', 'fredDialog', '$location'];

    function ObjectifFlashController($scope, $uibModal, Notify, ProgressBar, fredSubscribeService, ObjectifFlashService, ModelStateErrorManager, fredDialog, $location) {
        // assignation de la valeur du scope au controller pour les templates non mis à jour
        var $ctrl = this;
        $ctrl.resources = resources;
        $ctrl.IsActif = false;
        $ctrl.IsJournalise = false;
        $ctrl.DateMin = new Date();
        $ctrl.DateMax = new Date();
        $ctrl.DateMax.setMonth($ctrl.DateMin.getMonth() + 7);
        $ctrl.errorList = new Array();
        $ctrl.isModifiedJournalisation = false;

        $ctrl.handleLookupSelection = handleLookupSelection;
        $ctrl.handleLookupDeletionUnite = handleLookupDeletionUnite;
        $ctrl.handleShowErrors = handleShowErrors;
        $ctrl.handleReportObjectif = handleReportObjectif;
        $ctrl.handleSave = handleSave;
        $ctrl.handleConfirmation = handleConfirmation;
        $ctrl.handleLigneTacheLookupSelection = handleLigneTacheLookupSelection;
        $ctrl.handleJournalisation = handleJournalisation;
        $ctrl.handleLigneRessourceLookupSelection = handleLigneRessourceLookupSelection;
        $ctrl.handleDeleteTache = handleDeleteTache;
        $ctrl.handleDeleteRessource = handleDeleteRessource;
        $ctrl.handleCalculMontant = handleCalculMontant;
        $ctrl.handleSelectInput = handleSelectInput;
        $ctrl.handleRatioRepartitionAndTache = handleRatioRepartitionAndTache;
        $ctrl.handleExportObjectifFlash = handleExportObjectifFlash;
        $ctrl.handleRessourceRepartitionSelection = handleRessourceRepartitionSelection;

        $scope.init = function init(id) {
            fredSubscribeService.subscribe({ eventName: 'goBack', callback: actionReturnToBilanFlashList, tooltip: "BACK TO BILAN FLASH" });
            $scope.$on('objectifFlashRessourceSelected', function (event, arg) {
                handleLigneTacheLookupSelection(arg.ressource, $ctrl.tacheSelected, 'Ressource');
            });

            getObjectifFlash(id);
        };

        /******************** Méthodes public ********************/

        /********** ObjectifFlash **********/

        function handleLookupSelection(item, type) {
            switch (type) {
                case "CI":
                    $ctrl.objectifFlash.Ci = item;
                    $ctrl.objectifFlash.CiId = item.IdRef;
                    $ctrl.objectifFlash.Taches = [];

                    getDeviseRefForCi();
                    break;
                case "Tache":
                    if (!existTacheInObjectifFlash(item)) {
                        var tache = {
                            Tache: item,
                            TacheId: item.TacheId,
                            Libelle: item.CodeLibelle,
                            Unite: null,
                            UniteId: null,
                            QuantiteObjectif: 0,
                            TotalMontantRessource: 0,
                            Ressources: []
                        };

                        $ctrl.objectifFlash.Taches.push(tache);
                        if ($ctrl.IsJournalise) {
                            showPopUpAjoutQuantite(tache, null);
                        }
                    } else {
                        Notify.error($ctrl.resources.ObjectifFlash_Error_TacheDejaPresente);
                    }
                    break;
            }
        }

        function handleLookupDeletionUnite(item) {
            item.Unite = null;
            item.UniteId = null;
        }

        function handleShowErrors(item) {
            $ctrl.errorList = item.ListErreurs;
        }

        function handleReportObjectif() {
            var modalInstance = $uibModal.open({
                animation: true,
                component: 'reportEtDuplicationObjectifFlashComponent',
                resolve: {
                    resources: function () { return $ctrl.resources; },
                    dateDebut: function () { return $ctrl.objectifFlash.DateDebut; },
                    dateFin: function () { return $ctrl.objectifFlash.DateFin; },
                    duplicate: function () {return false;}
                }
            });

            modalInstance.result.then(function (date) {
                ProgressBar.start();
                ObjectifFlashService.GetReportedJournalisation(date, $ctrl.objectifFlash)
                    .then(getObjectifFlashOnSuccess)
                    .catch(getObjectifFlashOnError)
                    .finally(getObjectifFlashOnFinally);
            });
        }

        function handleSave() {
            if ($ctrl.isModifiedJournalisation) {
                handleConfirmation('Save');
            } else {
                actionSave();
            }
        }

        function handleConfirmation(type) {
            var modalInstance = $uibModal.open({
                animation: true,
                component: 'confirmationComponent',
                resolve: {
                    resources: function () { return $ctrl.resources; },
                    type: function () { return type; }
                }
            });

            modalInstance.result.then(function () {
                switch (type) {
                    case 'Cancel':
                        getObjectifFlash($ctrl.objectifFlash.ObjectifFlashId);
                        break;
                    case 'Activate':
                        actionActivate();
                        break;
                    case 'Close':
                        $ctrl.objectifFlash.IsClosed = true;
                        actionSave();
                        break;
                    case 'Save':
                        actionSave();
                        break;
                }
            });

        }

        function handleChangeUrl() {
            $location.path('/BilanFlash/BilanFlash/ObjectifFlash/' + $ctrl.objectifFlash.ObjectifFlashId);
            return $ctrl.objectifFlash.ObjectifFlashId;

        }

        function handleSelectInput(id) {
            document.getElementById(id).select();
        }

        /********** Tache **********/

        function handleLigneTacheLookupSelection(item, tache, type) {
            switch (type) {
                case "Unite":
                    tache.Unite = item;
                    tache.UniteId = item.IdRef;
                    break;
                case "Ressource":
                    var ressource = {
                        Ressource: item,
                        RessourceId: item.RessourceId,
                        Libelle: item.CodeLibelle,
                        Unite: item.Unite,
                        UniteId: item.Unite !== null ? item.Unite.UniteId : null,
                        ChapitreCode: item.ChapitreCode,
                        PuHT: item.PuHT,
                        QuantiteObjectif: 0,
                        Montant: 0,
                        IsRepartitionKey: false
                    };
                    tache.Ressources.push(ressource);
                    if ($ctrl.IsJournalise) {
                        showPopUpAjoutQuantite(ressource, tache);
                    }
                    break;
            }
        }

        function handleDeleteTache(tache) {
            var indexTache = $ctrl.objectifFlash.Taches.indexOf(tache);
            $ctrl.objectifFlash.Taches.splice(indexTache, 1);
        }

        function handleCalculMontant(ressource, tache) {
            tache.TotalMontantRessource = 0;
            ressource.Montant = ressource.PuHT * ressource.QuantiteObjectif;
            tache.Ressources.map(ressource => tache.TotalMontantRessource += ressource.Montant);
            if ($ctrl.IsJournalise) {
                calculChangementProprieteGeneralRessource(tache);
            }
        }

        function handleRatioRepartitionAndTache(tache, ressource, colIndex) {
            $ctrl.isModifiedJournalisation = true;
            if (tache && !ressource) {
                if(!tache.TacheJournalisations[colIndex].QuantiteObjectif || tache.TacheJournalisations[colIndex].QuantiteObjectif === ""){
                    tache.TacheJournalisations[colIndex].QuantiteObjectif = 0;
                }
                let indexRessourceRepartitionKey = tache.Ressources.findIndex(ressource => ressource.IsRepartitionKey);
                let ratio = tache.QuantiteObjectif / tache.Ressources[indexRessourceRepartitionKey].QuantiteObjectif;
                tache.Ressources[indexRessourceRepartitionKey].TacheRessourceJournalisations[colIndex].QuantiteObjectif = arrondiMillierDecimal(tache.TacheJournalisations[colIndex].QuantiteObjectif / ratio);
                calculChangementJournalisation(tache, tache.Ressources[indexRessourceRepartitionKey], colIndex);

            }
            else if (tache && ressource) {
                if(!ressource.TacheRessourceJournalisations[colIndex].QuantiteObjectif || ressource.TacheRessourceJournalisations[colIndex].QuantiteObjectif === ""){
                    ressource.TacheRessourceJournalisations[colIndex].QuantiteObjectif = 0;
                }
                if (ressource.IsRepartitionKey) {
                    let ratio = tache.QuantiteObjectif / ressource.QuantiteObjectif;
                    tache.TacheJournalisations[colIndex].QuantiteObjectif = arrondiMillierDecimal(ressource.TacheRessourceJournalisations[colIndex].QuantiteObjectif * ratio);
                }
                calculChangementJournalisation(tache, ressource, colIndex);
            }
        }

        /********** Ressource **********/

        function handleLigneRessourceLookupSelection(item, ressource, type) {
            switch (type) {
                case "Unite":
                    ressource.Unite = item;
                    ressource.UniteId = item.IdRef;
                    break;
            }
        }

        function handleDeleteRessource(tache, ressource) {
            var indexRessource = tache.Ressources.indexOf(ressource);
            tache.Ressources.splice(indexRessource, 1);
            if (tache.Ressources.length > 0) {
                tache.Ressources.map(r => tache.TotalMontantRessource += r.Montant);
            } else {
                tache.TotalMontantRessource = 0;
            }
            calculChangementProprieteGeneralRessource(tache);
        }

        function handleRessourceRepartitionSelection(tache, ressourceRepartition) {
            for (var ressource of tache.Ressources) {
                if (ressource !== ressourceRepartition) {
                    ressource.IsRepartitionKey = false;
                }
            }
        }

        /********** Journalisation **********/

        function handleJournalisation() {
            if ($ctrl.IsJournalise) {
                fredDialog.confirmation('Voulez-vous remettre toutes les données de journalisation avec leur valeur par défaut ?', '', '', '', '', clearJournalisation);
            }
            else {
                processedNewJournal();
            }

        }

        function processedNewJournal() {
            ProgressBar.start();
            ObjectifFlashService.GetNewJournalisation($ctrl.objectifFlash)
                .then(getObjectifFlashJournalisationSuccess)
                .catch(getObjectifFlashOnError)
                .finally(getObjectifFlashOnFinally);
        }

        function handleExportObjectifFlash() {
            var modalInstance = $uibModal.open({
                animation: true,
                component: 'exportBilanFlashComponent',
                resolve: {
                    resources: function () { return $ctrl.resources; },
                    dateDebut: function () { return $ctrl.objectifFlash.DateDebut; },
                    dateFin: function () { return $ctrl.objectifFlash.DateFin; }
                }
            });

            modalInstance.result.then(function (data) {
                ProgressBar.start();
                var fileName = data.templateName + (data.isPdf ? '.pdf' : '.xlsx');
                ObjectifFlashService.ExportBilanFlash(data.templateName, $ctrl.objectifFlash.ObjectifFlashId, data.dateDebut, data.dateFin, data.isPdf)
                    .then(function (data) {
                        ObjectifFlashService.ExportBilanFlashDownload(data.data.id, fileName);
                    })
                    .catch(getObjectifFlashOnError)
                    .finally(getObjectifFlashOnFinally);
            });
        }

        /******************** Méthodes private ********************/
        function actionReturnToBilanFlashList() {
            window.location = '/BilanFlash/BilanFlash';
        }

        function getObjectifFlash(id) {
            ProgressBar.start();
            if (id && id !== 0) {
                ObjectifFlashService.getObjectifFlash(id)
                    .then(getObjectifFlashOnSuccess)
                    .then(getDeviseRefForCi)
                    .catch(getObjectifFlashOnError)
                    .finally(getObjectifFlashOnFinally);
            } else {
                ObjectifFlashService.getNewObjectifFlash()
                    .then(getObjectifFlashOnSuccess)
                    .catch(getObjectifFlashOnError)
                    .finally(getObjectifFlashOnFinally);
            }
        }

        function getObjectifFlashOnSuccess(data) {
            scrollLeftZero();
            $ctrl.objectifFlash = data.data;
            $ctrl.IsActif = $ctrl.objectifFlash.IsActif;
            $ctrl.objectifFlash.DateDebut = convertToDate($ctrl.objectifFlash.DateDebut);
            $ctrl.objectifFlash.DateFin = convertToDate($ctrl.objectifFlash.DateFin);
            $ctrl.IsClosed = $ctrl.objectifFlash.IsClosed;
            $ctrl.IsJournalise = $ctrl.objectifFlash.Journalisations.length > 0;
            $ctrl.isModifiedJournalisation =  false; 
        }

        function actionSave() {
            // Add or Update
            if ($ctrl.objectifFlash.ObjectifFlashId !== 0) {
                ProgressBar.start();
                ObjectifFlashService.updateObjectifFlash($ctrl.objectifFlash)
                    .then(getObjectifFlashOnSuccess)
                    .then(notifyOnSucces)
                    .catch(getObjectifFlashOnError)
                    .finally(getObjectifFlashOnFinally);
            } else {
                ObjectifFlashService.addNewObjectifFlash($ctrl.objectifFlash)
                    .then(getObjectifFlashOnSuccess)
                    .then(notifyOnSucces)
                    .then(handleChangeUrl)
                    .catch(getObjectifFlashOnError);
            }
        }

        function actionActivate(){
            ProgressBar.start();
            ObjectifFlashService.updateObjectifFlash($ctrl.objectifFlash)
                .then(getObjectifFlashOnSuccess)
                .then(activateObjectifFlash)
                .catch(getObjectifFlashOnError)
                .finally(getObjectifFlashOnFinally);
        }

        function activateObjectifFlash(){
            return ObjectifFlashService.activateObjectifFlash($ctrl.objectifFlash.ObjectifFlashId)
                .then(function () { $ctrl.objectifFlash.IsActif = true; $ctrl.IsActif = true; })
                .then(notifyOnSucces)
                .catch(getObjectifFlashOnError);
        }

        function notifyOnSucces() {
            Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
        }

        function getObjectifFlashOnError(error) {
            var validationError = ModelStateErrorManager.getErrors(error);
            if (validationError) {
                Notify.error(validationError.replace(/\n/g, "<br />"));
                return;
            }

            Notify.error($ctrl.resources.Global_Notification_Error);
        }

        function getObjectifFlashOnFinally() {
            ProgressBar.complete();
        }

        function getDeviseRefForCi() {
            ProgressBar.start();
            ObjectifFlashService.getDeviseRefForCi($ctrl.objectifFlash.CiId)
                .then(getDeviseRefForCiOnSuccess)
                .catch(getObjectifFlashOnError)
                .finally(getObjectifFlashOnFinally);
        }

        function getDeviseRefForCiOnSuccess(devise) {
            $ctrl.deviseSelected = devise.data;
        }

        function getObjectifFlashJournalisationSuccess(data) {
            $ctrl.objectifFlash = data.data;
            $ctrl.IsJournalise = true;
        }

        function clearJournalisation() {
            for (var tache of $ctrl.objectifFlash.Taches) {
                tache.TacheJournalisations = [];
                for (var ressource of tache.Ressources) {
                    ressource.TacheRessourceJournalisations = [];
                }
            }
            $ctrl.objectifFlash.Journalisations = [];
            $ctrl.IsJournalise = false;
        }

        function existTacheInObjectifFlash(item) {
            var isExist = false;
            $ctrl.objectifFlash.Taches.forEach(tache => {
                if (tache.TacheId === item.TacheId) {
                    isExist = true;
                }
            });

            return isExist;
        }

        function showPopUpAjoutQuantite(item, tache) {
            var modalInstance = $uibModal.open({
                animation: true,
                component: 'ajoutQuantiteComponent',
                windowClass: 'quantite-modal',
                backdrop: 'static',
                resolve: {
                    item: function () { return item; },
                    resources: function () { return $ctrl.resources; },
                    tache: function () { return tache; },
                    dateDebut: function () { return $ctrl.objectifFlash.DateDebut; },
                    dateFin: function () { return $ctrl.objectifFlash.DateFin; }
                }
            });

            modalInstance.result.then(function (quantite) {
                var quantiteObjectif = quantite;
                var listJournalisation = initJournalisationForAddTacheAndRessource(quantiteObjectif, tache);

                if (tache) {
                    var ressourceIndex = tache.Ressources.indexOf(item);
                    tache.Ressources[ressourceIndex].QuantiteObjectif = quantiteObjectif;
                    tache.Ressources[ressourceIndex].TacheRessourceJournalisations = listJournalisation;
                    tache.Ressources[ressourceIndex].Montant = tache.Ressources[ressourceIndex].PuHT * quantiteObjectif;
                    tache.Ressources.map(ressource => tache.TotalMontantRessource += ressource.Montant);
                    calculTotalQuantiteJournaliseInRessource(tache.Ressources[ressourceIndex]);
                    calculChangementProprieteGeneralRessource(tache);
                }
                else {
                    var tacheIndex = $ctrl.objectifFlash.Taches.indexOf(item);
                    $ctrl.objectifFlash.Taches[tacheIndex].QuantiteObjectif = quantiteObjectif;
                    $ctrl.objectifFlash.Taches[tacheIndex].TacheJournalisations = listJournalisation;
                    calculTotalQuantiteJournaliseInTache($ctrl.objectifFlash.Taches[tacheIndex]);
                }

                scrollLeftZero();
            });
        }

        function initJournalisationForAddTacheAndRessource(quantiteObjectif, tache) {
            $ctrl.sommeJournalisation = 0;
            var listJournalisation = [];
            var nombreJour = $ctrl.objectifFlash.Journalisations.filter(journalisation => !journalisation.IsWeekEndOrHoliday).length;
            $ctrl.jour = 1;
            $ctrl.objectifFlash.Journalisations.forEach(function (journalisation) {
                if (tache) {
                    var journalisationRessource = { QuantiteObjectif: 0 };
                    listJournalisation.push(calculJournalisation(journalisation, journalisationRessource, quantiteObjectif, nombreJour));
                }
                else {
                    var journalisationTache = { QuantiteObjectif: 0, TotalMontantRessource: 0 };
                    listJournalisation.push(calculJournalisation(journalisation, journalisationTache, quantiteObjectif, nombreJour));
                }
            });

            return listJournalisation;
        }

        function calculJournalisation(journalisation, journalisationObject, quantiteObjectif, nombreJour) {

            if(!journalisation.IsWeekEndOrHoliday){
                if($ctrl.jour !== nombreJour){
                    journalisationObject.QuantiteObjectif = arrondiMillierDecimal(quantiteObjectif / nombreJour);
                    $ctrl.sommeJournalisation += journalisationObject.QuantiteObjectif;
                }
                else{
                    journalisationObject.QuantiteObjectif = arrondiMillierDecimal(quantiteObjectif - $ctrl.sommeJournalisation);
                }
                $ctrl.jour++;
            }

            return journalisationObject;
        }

        function calculChangementJournalisation(tache, ressource, colIndex) {
            calculTotalQuantiteJournaliseInRessource(ressource);
            calculTotalQuantiteJournaliseInTache(tache);
            calculTotalMontantJournaliseInTache(tache);
            calculTotalMontantRessourceInTacheJournalisations(tache, colIndex);
            calculTotalMontantJournaliseInObjectifFlash();
            calculTotalMontantInJournalisation(colIndex);
        }

        function calculChangementProprieteGeneralRessource(tache) {
            calculTotalMontantJournaliseInTache(tache);
            calculAllTotalMontantRessourceInTacheJournalisations(tache);
            calculTotalMontantJournaliseInObjectifFlash();
            calculAllTotalMontantInJournalisation();
        }


        function calculTotalQuantiteJournaliseInRessource(ressource) {
            ressource.TotalQuantiteJournalise = 0;
            ressource.TacheRessourceJournalisations.map(tacheRessourceJournalisation => ressource.TotalQuantiteJournalise += parseFloat(tacheRessourceJournalisation.QuantiteObjectif));
        }

        function calculTotalQuantiteJournaliseInTache(tache) {
            tache.TotalQuantiteJournalise = 0;
            tache.TacheJournalisations.map(tacheJournalisation => tache.TotalQuantiteJournalise += parseFloat(tacheJournalisation.QuantiteObjectif));
        }

        function calculTotalMontantJournaliseInTache(tache) {
            tache.TotalMontantJournalise = 0;
            tache.Ressources.map(ressource => tache.TotalMontantJournalise += ressource.TotalQuantiteJournalise * ressource.PuHT);
        }

        function calculTotalMontantRessourceInTacheJournalisations(tache, colIndex) {
            tache.TacheJournalisations[colIndex].TotalMontantRessource = 0;
            tache.Ressources.map(ressource => tache.TacheJournalisations[colIndex].TotalMontantRessource += ressource.TacheRessourceJournalisations[colIndex].QuantiteObjectif * ressource.PuHT);
        }

        function calculTotalMontantJournaliseInObjectifFlash() {
            $ctrl.objectifFlash.TotalMontantJournalise = 0;
            $ctrl.objectifFlash.Taches.map(tache => $ctrl.objectifFlash.TotalMontantJournalise += tache.TotalMontantJournalise);
        }

        function calculTotalMontantInJournalisation(colIndex) {
            $ctrl.objectifFlash.Journalisations[colIndex].TotalMontant = 0;
            $ctrl.objectifFlash.Taches.map(tache => $ctrl.objectifFlash.Journalisations[colIndex].TotalMontant += tache.TacheJournalisations[colIndex].TotalMontantRessource);
        }

        function calculAllTotalMontantRessourceInTacheJournalisations(tache) {
            tache.TacheJournalisations.forEach(function (tacheJournalisation) {
                var index = tache.TacheJournalisations.indexOf(tacheJournalisation);
                tacheJournalisation.TotalMontantRessource = 0;
                tache.Ressources.forEach(function (ressource) {
                    tacheJournalisation.TotalMontantRessource += ressource.TacheRessourceJournalisations[index].QuantiteObjectif * ressource.PuHT;
                });
            });
        }

        function calculAllTotalMontantInJournalisation() {
            $ctrl.objectifFlash.Journalisations.forEach(function (journalisation) {
                var index = $ctrl.objectifFlash.Journalisations.indexOf(journalisation);
                journalisation.TotalMontant = 0;
                $ctrl.objectifFlash.Taches.forEach(function (tache) {
                    journalisation.TotalMontant += tache.TacheJournalisations[index].TotalMontantRessource;
                });
            });

        }

        function scrollLeftZero() {
            document.getElementById('FLEX_TABLE').scrollLeft = 0;
        }

        function arrondiMillierDecimal(valeur){
            return Math.round(valeur * 1000) / 1000;
        }

        function convertToDate(dateString) {
            if (!dateString) {
                return dateString;
            }
            let dateTime = new Date(dateString);

            return new Date(Date.UTC(dateTime.getFullYear(), dateTime.getMonth(), dateTime.getDate(), 0, 0, 0, 0));
        }
    }
}(angular));
