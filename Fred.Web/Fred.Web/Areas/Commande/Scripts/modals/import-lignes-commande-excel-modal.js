(function (angular) {
    'use strict';

    angular.module('Fred').component('importLignesExcelModal', {
        templateUrl: '/Areas/Commande/Scripts/modals/import-lignes-commande-excel-modal.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: 'importLignesExcelModal'
    });

    angular.module('Fred').controller('importLignesExcelModal', importLignesExcelModal);

    importLignesExcelModal.$inject = ['$scope', '$q', '$filter', '$timeout', 'CommandeImporExportExcelService', 'ProgressBar', 'Notify', 'fredDialog','ModelStateErrorManager'];

    function importLignesExcelModal($scope, $q, $filter, $timeout, CommandeImporExportExcelService, ProgressBar, Notify, fredDialog, ModelStateErrorManager) {
        /*
         * Initilisation du controller de la modal
         */
        this.$onInit = function () {
            $scope.resources = this.resolve.resources;
            $scope.commandelignes = this.resolve.commandelignes;
            $scope.newcommandelignes = [];
            $scope.ciId = this.resolve.parms.ciId;
            $scope.checkinValue = this.resolve.parms.checkinValue;//passer la valeur numéro de commande si en parle des avenants sinon Date de la commande
            $scope.isAvenant = this.resolve.parms.isAvenant;
            $scope.optionImportFile = false;
            $scope.optionOpenFile = false;
            $scope.SelectedFile = [];
            $scope.acceptedAttachmentFormat = ".xls, .xlsx, .csv";
            $scope.attachmentMaxSizeForOneFile = 10000;
        };

        //handels
        $scope.changeOptionImportFile = function (value) {
            if (!value) {
                $scope.filename = undefined;
                event.preventDefault();
            } //disable seconde clic open dialog file !!important
            $scope.optionImportFile = value;
            $scope.optionOpenFile = false;
        };

        $scope.changeOptionOpenFile = function (value) {
            $scope.optionOpenFile = value;
            $scope.optionImportFile = false;
        };

        $scope.handleSelectFile = (event) => {
            $timeout(function () { angular.noop(); }, 0);

            if (event.target.files && event.target.files[0]) {
                var file = event.target.files[0];
                if (isValidFileSize(file, $scope.attachmentMaxSizeForOneFile)) {
                    var formData = new FormData();
                    $scope.filename = file.name;
                    formData.append("file", file, file.name);
                    $scope.SelectedFile = formData;
                }
            }
        };


        function uploadFile() {
            if ($scope.filename !== undefined) {
                ProgressBar.start();

                CommandeImporExportExcelService.ImportCommadeLignes($scope.SelectedFile, $scope.checkinValue, $scope.ciId, $scope.isAvenant)
                    .then(result => {

                        if (result.data.ErrorMessages && result.data.ErrorMessages.length === 0) {
                            $q.when(addLigneCommande(result.data.CommandeLignes))
                                .then(closeModal)
                                .then(() => {
                                    if ($scope.newcommandelignes.length > 0) {
                                        fredDialog.generic($scope.resources.Commande_Popin_ImportLignes_FileLoadSucces_Text,
                                            $scope.resources.Commande_Popin_ImportLignes_FileLoadSucces_Titre, 'falticon flaticon flaticon-shuffle-1',
                                            $scope.resources.Global_Bouton_Valider, '');
                                    }
                                    else {
                                        fredDialog.generic("Fichier vide",
                                            $scope.resources.Commande_Popin_ImportLignes_FileLoadFealed_Titre, 'falticon flaticon flaticon-shuffle-1',
                                            $scope.resources.Global_Bouton_Valider, '');
                                    }
                                });
                        }
                        else {
                            //Create file Anomalie.  
                            var fileName = "File Erreurs";
                            fredDialog.generic($scope.resources.Commande_Popin_ImportLignes_FileLoadFealed_Text,
                                $scope.resources.Commande_Popin_ImportLignes_FileLoadFealed_Titre, 'falticon flaticon flaticon-shuffle-1', '', $scope.resources.Commande_popin_ImportLignes_Btn_OpenAnomlie, null,
                                _ => { exportFile(fileName, 'txt', result.data.ErrorMessages); });
                        }
                    })
                    .then(closeModal)
                    .catch(error => {
                        Notify.error($scope.resources.Commande_Popin_Importlignes_Notify_Invalid_FileFormat);
                    })
                    .finally(() => ProgressBar.complete());
            }
            else {
                Notify.error($scope.resources.Commande_Popin_ImportLignes_EmtyFileselected);
            }
        }

        function openExempleFile() {
            ProgressBar.start();
            var namenewfile= $scope.checkinValue!== null ? $scope.checkinValue : moment(new Date()).format('DD-MM-YYYY');
            CommandeImporExportExcelService.OpenFileExempleLignesCommande($scope.ciId, $scope.isAvenant)
                .then(r => {
                    if (r.data === null) {
                        Notify.error($scope.resources.Global_Notification_Error);
                    }
                    else {
                        CommandeImporExportExcelService.GetExempleExcelLignesCommande(r.data.Id, "_" + namenewfile );
                    }
                })
                .then(closeModal)
                .catch(() => Notify.error($scope.resources.Commande_Popin_Importlignes_Notify_Invalid_FileLoad))
                .finally(() => ProgressBar.complete());
        }

        function avenantCount(list) {
            var count = 0;
            angular.forEach(list, function (value, key) {
                if (value.AvenantLigne !== null ? !value.AvenantLigne.IsDeleted : false) {
                    count++;
                }
            });
            return count;
        }

        function addLigneCommande(data) {

            var avenantViewId = avenantCount($scope.commandelignes) + 1;

            data.forEach(function (element) {
                var newRow = angular.copy($scope.commandelignes[0]);
                newRow.CommandeLigneId = 0;
                newRow.TacheId = element.Tache ? element.Tache.TacheId : null;
                newRow.Tache = element.Tache;
                newRow.RessourceId = element.Ressource ? element.Ressource.RessourceId : null;
                newRow.Ressource = element.Ressource;
                newRow.UniteId = element.Unite ? element.Unite.UniteId : null;
                newRow.Unite = element.Unite;
                newRow.DepensesReception = null;
                newRow.Libelle = element.Libelle;
                newRow.Quantite = element.Quantite ? element.Quantite :0;
                newRow.PUHT = element.PuHT ? element.PuHT :0;
                newRow.MontantHT = element.Quantite * element.PuHT;
                newRow.IsCreated = true;
                newRow.IsUpdated = false;
                newRow.IsDeleted = false;
                newRow.IsCommande = !$scope.isAvenant;
                newRow.IsAvenantValide = false;
                newRow.IsAvenantNonValide = $scope.isAvenant;
                newRow.AvenantLigne = $scope.isAvenant ? { IsDiminution: element.IsDiminution } : null;
                newRow.ViewId = $scope.isAvenant ? avenantViewId++ : null;
                $scope.newcommandelignes.push(newRow);
            });
        }

        function closeModal() {
            return $scope.$ctrl.close({ $value: $scope.newcommandelignes });
        }

        /* 
         * @function handleCancel ()
         * @description Quitte la modal
         */

        $scope.handleCancel = function () {
            $scope.$ctrl.dismiss({ $value: 'cancel' });
        };

        $scope.handleConfirm = function () {
            if ($scope.optionImportFile) {
                uploadFile();
            }
            if ($scope.optionOpenFile) {
                openExempleFile();
            }

        };

        /**
         * Vérifier si la taille du fichier est valide
         * @param {any} file Fichier à vérifier
         */

      function getFileSize(file) {
            return Math.floor(file.size / 1024);
        }

        /**
         * Vérifier si la taille du fichier est valide
         * @param {any} file Fichier à vérifier
         * @param {any} maxFileSize Taille maximale acceptée
         */

        function isValidFileSize(file, maxFileSize) {
            var fileSize = getFileSize(file); // Conversion en Mo
            var isValid = fileSize < maxFileSize;
            // Notify
            if (!isValid) {
                Notify.error(resources.PieceJointe_Error_MaxTailleFichier.replace('{0}', file.name));
            }
            return isValid;
        }
    }
}(angular));