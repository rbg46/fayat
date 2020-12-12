(function (angular) {
    'use strict';

    angular.module('Fred').controller('CommandeEnergieDetailController', CommandeEnergieDetailController);

    CommandeEnergieDetailController.$inject = ['$q', 'Notify', 'ProgressBar', 'CommandeEnergieService', '$uibModal', 'TypeEnergieService', 'CommandeEnergieHelperService', 'fredDialog', '$location'];

    function CommandeEnergieDetailController($q, Notify, ProgressBar, CommandeEnergieService, $uibModal, TypeEnergieService, CommandeEnergieHelperService, fredDialog, $location) {

        // assignation de la valeur du scope au controller pour les templates non mis à jour
        var $ctrl = this;

        //******************** méthodes publiques ********************
        $ctrl.handleAddAdditionalCommandeLigne = handleAddAdditionalCommandeLigne;
        $ctrl.handleDeleteAdditionalCommandeLigne = handleDeleteAdditionalCommandeLigne;
        $ctrl.handleDuplicateAdditionalCommandeLigne = handleDuplicateAdditionalCommandeLigne;
        $ctrl.handleCommandeLigneLookupSelection = CommandeEnergieHelperService.onCommandeLigneLookupSelection;
        $ctrl.handleCommandeLigneLookupDeletion = CommandeEnergieHelperService.onCommandeLigneLookupDeletion;
        $ctrl.handleOnUpdateCommandeLigne = handleOnUpdateCommandeLigne;
        $ctrl.handleInit = init;
        $ctrl.handleSave = handleSave;
        $ctrl.handleDelete = handleDelete;
        $ctrl.handleBackToList = handleBackToList;
        $ctrl.handleValidate = handleValidate;
        $ctrl.handleExportExcel = handleExportExcel;
        $ctrl.handleSearch = handleExportExcel;

        // Filter pour ng-repeat des lignes de commandes
        $ctrl.commandeEnergieLigneFilter = commandeEnergieLigneFilter;
        $ctrl.additionalCommandeEnergieLigneFilter = additionalCommandeEnergieLigneFilter;

        //******************** variables ********************
        $ctrl.resources = resources;
        $ctrl.commande = {};
        $ctrl.busy = false;
        $ctrl.today = moment();
        $ctrl.typeEnergies = [];
        $ctrl.commandeErrors = [];

        $ctrl.display = {
            showBtnSave: true,
            showBtnDelete: true,
            disabledBtnValidate: true,
            showBtnValidate: true,
            readOnly: false,
            showBtnDuplicateRow: true,
            showBtnDeleteRow: true,
            showBtnAddRow: true
        };

        $ctrl.typeEnergieStyles =
            {
                P: { class: 'pers' },
                M: { class: 'mat' },
                I: { class: 'int' },
                D: { class: 'dvr' }
            };

        $ctrl.statutCommandeCodes =
            {
                VA: 'VA',
                CL: 'CL',
                BR: 'BR',
                AV: 'AV'
            };

        $ctrl.allValidationSteps =
            {
                STEP_2: 'STEP_2',
                STEP_3: 'STEP_3'
            };

        $ctrl.permissionKeys = PERMISSION_KEYS;

        return $ctrl;

        function init(id) {

            $q.when()
                .then(onBeginRequest)
                .then(onGetTypeEnergies)
                .then(() => id)
                .then(onGetCommande)
                .then(onOpenHeaderModal)
                .then(onFinallyRequest);
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            PUBLIC
         * -------------------------------------------------------------------------------------------------------------
         */

        function commandeEnergieLigneFilter(item) {
            return (item.PersonnelId && item.PersonnelId > 0) || (item.MaterielId && item.MaterielId > 0);
        }


        function additionalCommandeEnergieLigneFilter(item) {
            return !item.PersonnelId && !item.MaterielId && !item.IsDeleted;
        }


        function handleValidate() {
            $q.when()
                .then(onBeginRequest)
                .then(onValidate)
                .then(onGetCommande)
                .then(onFinallyRequest);
        }


        function handleAddAdditionalCommandeLigne() {
            var newRow = {};
            newRow.CommandeLigneId = 0;
            newRow.CommandeId = $ctrl.commande.CommandeId;
            newRow.TacheId = null;
            newRow.Tache = null;
            newRow.RessourceId = null;
            newRow.Ressource = null;
            newRow.UniteId = null;
            newRow.Unite = null;
            newRow.DepensesReception = null;
            newRow.Libelle = "";
            newRow.Quantite = 0;
            newRow.PUHT = 0;
            newRow.MontantHT = 0;
            newRow.PersonnelId = null;
            newRow.MaterielId = null;

            $ctrl.commande.Lignes.push(newRow);
        }


        function handleDuplicateAdditionalCommandeLigne(ligne) {
            var newRow = angular.copy(ligne);
            newRow.CommandeLigneId = 0;
            $ctrl.commande.Lignes.push(newRow);
            CommandeEnergieHelperService.commandeCalculation($ctrl.commande);
        }


        function handleDeleteAdditionalCommandeLigne(ligne) {
            ligne.IsUpdated = false;
            ligne.IsDeleted = true;

            if (!ligne.CommandeLigneId) {
                var i = $ctrl.commande.Lignes.indexOf(ligne);
                $ctrl.commande.Lignes.splice(i, 1);
            }

            CommandeEnergieHelperService.commandeCalculation($ctrl.commande);
        }


        function handleOnUpdateCommandeLigne(ligne) {
            ligne.IsUpdated = true;
            CommandeEnergieHelperService.commandeCalculation($ctrl.commande);
        }


        /**
         *  Ajout ou mise à jour de la commande énergie
         */
        function handleSave() {
            var promise = $ctrl.commande.CommandeId > 0 ? onUpdateCommande : onAddCommande;

            $q.when()
                .then(onBeginRequest)
                .then(promise)
                .then(onFinallyRequest);
        }


        function handleDelete() {
            var msg = $ctrl.resources.Confirmation_Suppression_Commande_Energie;

            fredDialog.confirmation(msg, null, "flaticon flaticon-warning", $ctrl.resources.Global_Bouton_Supprimer, '', function () {
                $q.when()
                    .then(onBeginRequest)
                    .then(onDeleteCommande)
                    .then(onFinallyRequest);
            });
        }


        function handleExportExcel() {

            ProgressBar.start();

            CommandeEnergieService.ExportExcel($ctrl.commande.CommandeId)
                .then(response => CommandeEnergieService.DownloadExportFile(response.data.id, 'CommandeEnergie'))
                .catch(Notify.defaultError)
                .finally(ProgressBar.complete);
        }


        function handleBackToList() {
            location.href = '/CommandeEnergie/CommandeEnergie/Index';
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            PRIVATE
         * -------------------------------------------------------------------------------------------------------------
         */

        function onOpenHeaderModal() {
            // Si nouvelle commande énergie, on ouvre la modal par défaut à l'ouverture de la page
            if (!$ctrl.commande || !$ctrl.commande.CommandeId) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    component: 'commandeEnergieHeaderModalComponent',
                    windowClass: 'cmd-nrg-modal',
                    controllerAs: '$ctrl',
                    backdrop: 'static',
                    resolve: {
                        resources: () => $ctrl.resources,
                        commande: () => $ctrl.commande,
                        typeEnergies: () => $ctrl.typeEnergies
                    }
                });

                modalInstance.result.then(modalResponse => {
                    $ctrl.commande = modalResponse;
                    setDisplayableControls();
                });
            }
        }


        function onGetCommande(commandeId) {
            if (commandeId) {
                $ctrl.commandeErrors = [];
                return CommandeEnergieService.Get(commandeId)
                    .then(response => onGetCommandeSuccess(response))
                    .catch(function (error) {
                        console.error(error);
                        Notify.defaultError();
                    });
            }
        }

        function onAddCommande() {
            $ctrl.commandeErrors = [];
            return CommandeEnergieService.Add($ctrl.commande)
                .then(response => onAddCommandeSuccess(response))
                .catch(onAddOrUpdateError);
        }


        function onUpdateCommande() {
            $ctrl.commandeErrors = [];
            return CommandeEnergieService.Update($ctrl.commande)
                .then(response => onUpdateCommandeSuccess(response))
                .catch(onAddOrUpdateError);
        }


        function onDeleteCommande() {
            return CommandeEnergieService.Delete($ctrl.commande.CommandeId)
                .then(() => {
                    handleBackToList();
                    Notify.message($ctrl.resources.Global_Notification_Suppression_Success);
                })
                .catch(error => {
                    console.error(error);
                    Notify.defaultError();
                });
        }


        function onGetTypeEnergies() {
            return TypeEnergieService.GetAll().then(response => $ctrl.typeEnergies = response.data);
        }


        function onBeginRequest() {
            $ctrl.busy = true;
            ProgressBar.start();
        }

        function onFinallyRequest() {
            $ctrl.busy = false;
            ProgressBar.complete();
        }


        function onAddOrUpdateError(error) {
            if (error && error.data && error.data.ModelState) {
                $ctrl.commandeErrors = CommandeEnergieHelperService.getErrors(error.data.ModelState);
                Notify.error($ctrl.resources.Notification_Saisie_Error);
            }
            else {
                Notify.error($ctrl.resources.Global_Notification_Error);
            }
        }

        function onGetCommandeSuccess(response) {
            $ctrl.commande = response.data;
            setDisplayableControls();
            CommandeEnergieHelperService.commandeCalculation($ctrl.commande);
            CommandeEnergieHelperService.toLocaleDate($ctrl.commande);
            return $ctrl.commande;
        }

        function onAddCommandeSuccess(response) {
            $ctrl.commande = response.data;
            $location.path('/CommandeEnergie/CommandeEnergie/Detail/' + $ctrl.commande.CommandeId);
            CommandeEnergieHelperService.commandeCalculation($ctrl.commande);
            setDisplayableControls();
            Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
        }

        function onUpdateCommandeSuccess(response) {
            $ctrl.commande.Lignes = response.data.Lignes;
            CommandeEnergieHelperService.commandeCalculation($ctrl.commande);
            setDisplayableControls();
            Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
        }

        function onValidate() {
            return CommandeEnergieService.Validate($ctrl.commande)
                .then(() => {
                    Notify.message($ctrl.resources.Global_Notification_Validation_Success);
                    $ctrl.commandeErrors = [];
                    return $ctrl.commande.CommandeId;
                })
                .catch(error => {
                    console.error(error);

                    if (error && error.data && error.data.ModelState) {
                        var modelState = error.data.ModelState;
                        $ctrl.commandeErrors = [];

                        if (modelState[$ctrl.allValidationSteps.STEP_2]) {
                            $ctrl.commandeErrors = [modelState[$ctrl.allValidationSteps.STEP_2][0]];
                            Notify.error(`${$ctrl.resources.Notification_Echec_Validation}<br/>${modelState[$ctrl.allValidationSteps.STEP_2][0]}`);
                        }
                        else if (modelState[$ctrl.allValidationSteps.STEP_3]) {
                            $ctrl.commandeErrors = [modelState[$ctrl.allValidationSteps.STEP_3][0]];
                            Notify.warning(`${$ctrl.resources.Global_Notification_Validation_Success}<br />⚠ ${modelState[$ctrl.allValidationSteps.STEP_3][0]}`);
                        }
                        else if (modelState != null) {
                            $ctrl.commandeErrors = CommandeEnergieHelperService.getErrors(error.data.ModelState);
                            Notify.error(`${$ctrl.resources.Notification_Saisie_Error}`);
                        }
                    }
                });
        }

        function setDisplayableControls() {
            $ctrl.display.showBtnSave = $ctrl.commande.StatutCommande.Code !== $ctrl.statutCommandeCodes.VA && $ctrl.commande.StatutCommande.Code !== $ctrl.statutCommandeCodes.CL;
            $ctrl.display.showBtnDelete = $ctrl.commande.StatutCommande.Code !== $ctrl.statutCommandeCodes.VA && $ctrl.commande.StatutCommande.Code !== $ctrl.statutCommandeCodes.CL;
            $ctrl.display.showBtnValidate = $ctrl.commande.StatutCommande.Code !== $ctrl.statutCommandeCodes.CL;
            $ctrl.display.disabledBtnValidate = $ctrl.commande.CommandeId === 0;
            $ctrl.display.readOnly = $ctrl.commande.StatutCommande.Code === $ctrl.statutCommandeCodes.VA || $ctrl.commande.StatutCommande.Code === $ctrl.statutCommandeCodes.CL;
            $ctrl.display.showBtnDuplicateRow = $ctrl.commande.StatutCommande.Code !== $ctrl.statutCommandeCodes.VA && $ctrl.commande.StatutCommande.Code !== $ctrl.statutCommandeCodes.CL;
            $ctrl.display.showBtnDeleteRow = $ctrl.commande.StatutCommande.Code !== $ctrl.statutCommandeCodes.VA && $ctrl.commande.StatutCommande.Code !== $ctrl.statutCommandeCodes.CL;
            $ctrl.display.showBtnAddRow = $ctrl.commande.StatutCommande.Code !== $ctrl.statutCommandeCodes.VA && $ctrl.commande.StatutCommande.Code !== $ctrl.statutCommandeCodes.CL;
        }
    } 
}(angular));
