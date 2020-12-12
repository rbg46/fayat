(function (angular) {
    'use strict';

    angular.module('Fred').controller('RessourcesSpecifiquesCIController', RessourcesSpecifiquesCIController);

    RessourcesSpecifiquesCIController.$inject = ['Notify', 'ProgressBar', 'confirmDialog', 'RessourcesSpecifiquesCIService', 'ReferentielEtenduFilterService'];

    function RessourcesSpecifiquesCIController(Notify, ProgressBar, confirmDialog, RessourcesSpecifiquesCIService, ReferentielEtenduFilterService) {

        var $ctrl = this;

        // méthodes exposées
        angular.extend($ctrl, {
            handleExport: handleExport,
            onCiChanged: onCiChanged,
            handleOpenAll: handleOpenAll,
            handleCloseAll: handleCloseAll,
            handleInitCreation: handleInitCreation,
            handleInitEdition: handleInitEdition,
            handleSave: handleSave,
            handleDelete: handleDelete,
            handleValidateLibelle: handleValidateLibelle,

            // Filter Paramétrage des consommations des ressources (referentiel-etendu-filter.service.js)
            ressourcesFilter: ReferentielEtenduFilterService.ressourcesFilter,
            chapitresCascadeFilter: ReferentielEtenduFilterService.chapitresCascadeFilter,
            sousChapitresCascadeFilter: ReferentielEtenduFilterService.sousChapitresCascadeFilter,

            errorLibelle: false
        });

        init();

        return $ctrl;

        //Initialisation du controller
        function init() {
            ProgressBar.start();
            angular.extend($ctrl, {
                resources: resources,
                selectedCI: null,
                searchRessource: "",
                referentielList: [],
                currentRessource: null,
                modeCreation: false
            });

            ProgressBar.complete();
        }

        //Appelé pour l'export Excel
        function handleExport() {
            ProgressBar.start();
            RessourcesSpecifiquesCIService.generateExcel($ctrl.selectedCI.CiId)
                .then(r => RessourcesSpecifiquesCIService.extractExcel(r.data.id))
                .catch(Notify.defaultError)
                .finally(ProgressBar.complete);
        }

        //Appelé lorsque l'utilisateur change le CI
        function onCiChanged() {
            ProgressBar.start();
            RessourcesSpecifiquesCIService.getResources($ctrl.selectedCI.CiId)
                .then(r => {
                    if (r && r.length === 0) {
                        Notify.error($ctrl.resources.RessourcesSpecifiquesCI_Notification_AucuneDonnees);
                    }
                    $ctrl.referentielList = r.data;
                })
                .catch(Notify.defaultError)
                .finally(ProgressBar.complete);
        }

        //Tout déplier
        function handleOpenAll() {
            $('.collapse').collapse('show');
        }

        //Tout replier
        function handleCloseAll() {
            $('.collapse.in').collapse('hide');
        }

        //permet d'initialiser la pop-in en mode creation
        function handleInitCreation(ressource, ciId) {
            $ctrl.modeCreation = true;
            $ctrl.currentRessource = RessourcesSpecifiquesCIService.cloneRessource(ressource, ciId);
            RessourcesSpecifiquesCIService.getNextCodeRessource(ressource.RessourceId).then(r => { $ctrl.currentRessource.Code = r.data; }).catch(e => console.log(e));
        }

        //Permet d'initialiser la pop-in en mode edition
        function handleInitEdition(ressource) {
            $ctrl.modeCreation = false;
            $ctrl.currentRessource = angular.copy(ressource);
        }

        //Permet de sauvegarder la ressource
        function handleSave() {
            $ctrl.modeCreation ?
                RessourcesSpecifiquesCIService.addRessource($ctrl.currentRessource)
                    .then(r => {
                        RessourcesSpecifiquesCIService.insertInList($ctrl.referentielList, r.data);
                    })
                    .catch(e => {
                        console.log(e);
                        Notify.error($ctrl.resources.Global_Notification_Error);
                    }) :
                RessourcesSpecifiquesCIService.updateRessource($ctrl.currentRessource)
                    .then(r => { RessourcesSpecifiquesCIService.updateFromList($ctrl.referentielList, $ctrl.currentRessource); })
                    .catch(e => {
                        console.log(e);
                        Notify.error($ctrl.resources.Global_Notification_Error);
                    });
        }

        //Permet de supprimer une ressource
        function handleDelete() {
            RessourcesSpecifiquesCIService.deleteRessource($ctrl.currentRessource)
                .then(RessourcesSpecifiquesCIService.removeFromList($ctrl.referentielList, $ctrl.currentRessource))
                .catch(e => {
                    console.log(e);
                    Notify.error($ctrl.resources.Global_Notification_Error);
                });
        }

        function handleValidateLibelle() {
            if ($ctrl.currentRessource.Libelle == null || $ctrl.currentRessource.Libelle == "") {
                $ctrl.errorLibelle = true;
            } else {
                $ctrl.errorLibelle = false;
            }
        }

    }

}(angular));