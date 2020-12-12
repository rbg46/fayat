(function (angular) {
    'use strict';

    angular.module('Fred').component('commandeEnergieHeaderModalComponent', {
        templateUrl: '/Areas/CommandeEnergie/Scripts/modals/commande-energie-header-modal.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: commandeEnergieHeaderModalController
    });

    commandeEnergieHeaderModalController.$inject = ['$filter', '$q', 'CommandeEnergieService', 'CommandeEnergieHelperService', 'Notify', 'ProgressBar'];

    function commandeEnergieHeaderModalController($filter, $q, CommandeEnergieService, CommandeEnergieHelperService, Notify, ProgressBar) {
        var $ctrl = this;

        angular.extend($ctrl, {
            // Fonctions            
            handleCancel: handleCancel,
            handleValidate: handleValidate,
            handleChangeTypeEnergie: handleChangeTypeEnergie,
            handleLookupSelection: handleLookupSelection,
            handleLookupDeletion: handleLookupDeletion
        });

        /*
         * Initilisation du controller de la modal
         */
        $ctrl.$onInit = function () {
            $ctrl.resources = $ctrl.resolve.resources;
            $ctrl.commande = $ctrl.resolve.commande;
            $ctrl.typeEnergies = $ctrl.resolve.typeEnergies;
            $ctrl.commandeHeaderErrors = [];
            $ctrl.commande.Date = new Date();
            $ctrl.busy = false;

        };

        /**
         * validation
         */
        function handleValidate() {
            $ctrl.commandeHeaderErrors = [];
            if ($ctrl.form.$valid) {
                $q.when()
                    .then(ProgressBar.start)
                    .then(onGetPreloadedCommande)
                    .then(ProgressBar.complete);
            }
            else {
                if (!$ctrl.form.typeEnergieInput.$valid) {
                    $ctrl.commandeHeaderErrors.push($ctrl.resources.Notification_Type_Energie_Obligatoire);
                }
                if (!$ctrl.form.periodeInput.$valid) {
                    $ctrl.commandeHeaderErrors.push($ctrl.resources.Notification_Periode_Obligatoire);
                }
                if (!$ctrl.form.ciInput.$valid) {
                    $ctrl.commandeHeaderErrors.push($ctrl.resources.Notification_Ci_Obligatoire);
                }
                if (!$ctrl.form.fournisseurInput.$valid) {
                    $ctrl.commandeHeaderErrors.push($ctrl.resources.Notification_Fournisseur_Obligatoire);
                }
            }
        }

        /**
         * Action au changement du type d'énergie 
         */
        function handleChangeTypeEnergie() {
            $ctrl.commande.TypeEnergie = $filter('filter')($ctrl.typeEnergies, { TypeEnergieId: $ctrl.commande.TypeEnergieId }, true)[0];
        }


        function handleLookupSelection(type, item) {
            switch (type) {
                case 'CI':
                    $ctrl.commande.CiId = item.IdRef;
                    $ctrl.commande.Fournisseur = null;
                    $ctrl.commande.FournisseurId = null;
                    break;
                case 'Fournisseur':
                    $ctrl.commande.FournisseurId = item.IdRef;
                    break;
                default: break;
            }
        }

        function handleLookupDeletion(type) {
            switch (type) {
                case 'CI':
                    $ctrl.commande.CI = null;
                    $ctrl.commande.CiId = null;
                    break;
                case 'Fournisseur':
                    $ctrl.commande.Fournisseur = null;
                    $ctrl.commande.FournisseurId = null;
                    break;
                default: break;
            }
        }

        /* 
         * @function handleCancel ()
         * @description Quitte la modal
         */
        function handleCancel() {
            $ctrl.dismiss({ $value: 'cancel' });
        }

        function onGetPreloadedCommande() {
            $ctrl.busy = true;
            return CommandeEnergieService.Preloading($ctrl.commande)
                .then(function (response) {
                    if (response) {
                        $ctrl.commande = response.data;
                        CommandeEnergieHelperService.commandeCalculation($ctrl.commande);
                        Notify.message($ctrl.resources.Notification_Prechargement_Succes);
                        $ctrl.close({ $value: $ctrl.commande });
                    }
                })
                .catch(function (err) {
                    onError(err);
                })
                .finally(() => $ctrl.busy = false);
        }

        function onError(error) {
            if (error && error.data && error.data.ModelState) {
                Notify.error(error.data.ModelState);
            }
            if (error && error.data && error.data.Message) {
                Notify.error(error.data.Message);
            }
            else {
                Notify.error($ctrl.resources.Global_Notification_Error);
            }
        }
    }

}(angular));