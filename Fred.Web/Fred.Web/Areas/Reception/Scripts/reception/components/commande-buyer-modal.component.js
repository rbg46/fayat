(function (angular) {
    'use strict';

    angular.module('Fred').component('commandeBuyerComponent', {
        templateUrl: '/Areas/Reception/Scripts/reception/components/commande-buyer-modal.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: 'CommandeBuyerComponentController'
    });


    angular.module('Fred').controller('CommandeBuyerComponentController', CommandeBuyerComponentController);

    CommandeBuyerComponentController.$inject = ['$timeout', '$filter', 'ReceptionService'];

    function CommandeBuyerComponentController($timeout, $filter, ReceptionService) {
        var $ctrl = this;

        angular.extend($ctrl, {
            // Fonctions
            handleSearch: handleSearch,
            handleImport: handleImport,
            handleCancel: handleCancel,

            handleDateValidation: handleDateValidation,
            handleLookupSelection: handleLookupSelection,
            handleLookupDeletion: handleLookupDeletion,

            // Variables                
            cmdBuyerForm: {},
            cmdBuyer: {},
            etablissementComptableList: [],
            cmdBuyerList: [],
            isBusy: false,
            searchDone: true,
            message: "",
            cmdBuyerCount: 0,
        });

        /*
         * Initilisation du controller de la modal
         */
        $ctrl.$onInit = function () {
            $ctrl.resources = $ctrl.resolve.resources;
            $ctrl.today = $ctrl.resolve.today;
            $ctrl.modalTitle = $ctrl.resolve.modalTitle;
            $ctrl.handleShowLookup = $ctrl.resolve.handleShowLookup;
        };

        /*
         * @function handleImport()
         * @description Lance l'import des commandes buyer
         */
        function handleImport() {
            var param = { EtablissementComptableCode: "08", DateDebut: $filter('date')($ctrl.cmdBuyer.DateDebut, 'MM-dd-yyyy'), DateFin: $filter('date')($ctrl.cmdBuyer.DateFin, 'MM-dd-yyyy') };

            $ctrl.message = $ctrl.resources.Reception_Controller_ModalBuyer_ImportationEnCours;
            $ctrl.isBusy = true;
            $ctrl.searchDone = false;

            if ($ctrl.cmdBuyerCount > 0) {
                ReceptionService.ImportCommandesBuyer(param.EtablissementComptableCode, param.DateDebut, param.DateFin).then(function () {
                    $ctrl.message = "Les " + $ctrl.resources.Reception_Controller_ModalBuyer_ImportationDonnees_Success;
                })
                    .catch(function (reason) {
                        $ctrl.message = $ctrl.resources.Reception_Controller_ModalBuyer_ImportationDonnees_Error;
                        console.log(reason);
                    })
                    .finally(function () {
                        $ctrl.isBusy = false;
                        $ctrl.searchDone = true;
                        $ctrl.cmdBuyerCount = 0;
                    });
            }
        }

        /*
         * @function handleSearch()
         * @description Lance la recherche de commande buyer
         */
        function handleSearch() {
            var param = { EtablissementComptableCode: "08", DateDebut: $filter('date')($ctrl.cmdBuyer.DateDebut, 'MM-dd-yyyy'), DateFin: $filter('date')($ctrl.cmdBuyer.DateFin, 'MM-dd-yyyy') };

            if ($ctrl.cmdBuyer.EtablissementComptableCode && $ctrl.cmdBuyer.DateDebut && $ctrl.cmdBuyer.DateFin) {
                $ctrl.isBusy = true;
                $ctrl.searchDone = false;
                $ctrl.message = "Recherche...";

                ReceptionService.GetNbCommandesBuyer(param.EtablissementComptableCode, param.DateDebut, param.DateFin).then(function (value) {
                    if (value > 0) {
                        $ctrl.message = String.format($ctrl.resources.Reception_Controller_ModalBuyer_NbrCommandeTrouvee, value);
                    }
                    else {
                        $ctrl.message = $ctrl.resources.Reception_Controller_ModalBuyer_AucuneCommandeAImporter;
                    }
                    $ctrl.cmdBuyerCount = value;
                })
                    .catch(function (reason) {
                        $ctrl.message = $ctrl.resources.Reception_Controller_ModalBuyer_Recherche_Error;
                        console.log(reason);
                    })
                    .finally(function () {
                        $ctrl.isBusy = false;
                        $ctrl.searchDone = true;
                    });

            }
        }

        /* 
         * @function handleCancel ()
         * @description Quitte la modal commande buyer
         */
        function handleCancel() {
            $ctrl.dismiss({ $value: 'cancel' });
        }

        /*
         * @function handleDateValidation
         * @description Valide les dates de début et de fin
         */
        function handleDateValidation() {
            $ctrl.cmdBuyerCount = 0;
            $ctrl.cmdBuyerForm.DateDebut.$setValidity("RangeError", ($ctrl.cmdBuyer.DateDebut < $ctrl.cmdBuyer.DateFin));
            // Permet de forcer le rafraichissement de la vue
            $timeout(angular.noop);
        };

        /*
         * @description Gestion de la sélection par la Lookup
         */
        function handleLookupSelection(item) {
            $ctrl.cmdBuyer.EtablissementComptable = item;
            $ctrl.cmdBuyer.EtablissementComptableId = item.IdRef;
            $ctrl.cmdBuyer.EtablissementComptableCode = item.Code;
        }

        /*
         * @description Fonction suppression d'un item dans la lookup en fonction du type de lookup
         */
        function handleLookupDeletion() {
            $ctrl.cmdBuyer.EtablissementComptable = null;
            $ctrl.cmdBuyer.EtablissementComptableId = null;
            $ctrl.cmdBuyer.EtablissementComptableCode = null;
        }

    }

}(angular));
