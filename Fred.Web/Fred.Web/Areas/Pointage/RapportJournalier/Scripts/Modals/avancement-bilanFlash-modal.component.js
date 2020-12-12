(function (angular) {
    'use strict';

    angular.module('Fred').component('avancementBilanFlashComponent', {
        templateUrl: '/Areas/Pointage/RapportJournalier/Scripts/Modals/avancement-bilanFlash-modal.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: 'AvancementBilanFlashComponentController'
    });

    AvancementBilanFlashComponentController.$inject = ['Notify'];

    function AvancementBilanFlashComponentController(Notify) {
        var $ctrl = this;

        $ctrl.$onInit = $onInit;
        $ctrl.handleSave = handleSave;
        $ctrl.handleCancel = handleCancel;

        function $onInit() {
            //$ctrl.objectifFlashModal = angular.copy($ctrl.resolve.objectifFlash);
            $ctrl.objectifFlashList = $ctrl.resolve.objectifFlashList;
            $ctrl.resources = $ctrl.resolve.resources;
            $ctrl.dateChantier = moment($ctrl.resolve.rapport.DateChantier).format('DD/MM/YYYY');
            $ctrl.modalText = $ctrl.resources.Rapport_ModalProductionFlash_Titre_Popup;
        }

        function handleSave() {
            var tacheRealisations = [];
            let quantiteError = false;
            $ctrl.objectifFlashList.forEach((objectifFlash) => {
                objectifFlash.Taches.forEach((tache) => {
                    if (!tache.TacheRealisations[0] || !tache.TacheRealisations[0].QuantiteRealise) {
                        quantiteError = true;
                    }

                    tacheRealisations.push(tache.TacheRealisations[0]);
                });
            });

            if (quantiteError) {
                Notify.error($ctrl.resources.Rapport_ModalProductionFlash_Error_SaisieQuantiteProduction);
                return;
            }

            $ctrl.close({ $value: tacheRealisations });
        }

        function handleCancel() {
            $ctrl.dismiss({ $value: 'cancel' });
        }
    }

    angular.module('Fred').controller('AvancementBilanFlashComponentController', AvancementBilanFlashComponentController);

}(angular));