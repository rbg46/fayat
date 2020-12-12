(function (angular) {
    'use strict';

    var controleVracComponent = {
        templateUrl: '/Areas/ValidationPointage/Scripts/Modals/controle-vrac-modal.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: controleVracController
    };

    angular.module('Fred').component('controleVracComponent', controleVracComponent);

    angular.module('Fred').controller('controleVracController', controleVracController);

    controleVracController.$inject = ['$timeout', '$filter', 'Notify'];

    function controleVracController($timeout, $filter, Notify) {
        var $ctrl = this;

        angular.extend($ctrl, {
            // Fonctions
            handleValidate: handleValidate,
            handleCancel: handleCancel,

            handleLookupSelection: handleLookupSelection,
            handleLookupDeletion: handleLookupDeletion,
            handleEtablissementPaieOption: handleEtablissementPaieOption,
            handleShowLookupSociete: handleShowLookupSociete,
            handleChangePersonnelStatutList: handleChangePersonnelStatutList,
            handleShowLookupEtabPaie: handleShowLookupEtabPaie,

            // Variables                        
            filter: null,
            isAllEtablissement: true,
            emptyEtablissementList: true
        });

        /*
         * Initilisation du controller de la modal
         */
        $ctrl.$onInit = function () {
            $ctrl.resources = $ctrl.resolve.resources;
            $ctrl.handleShowLookup = $ctrl.resolve.handleShowLookup;
            $ctrl.filter = $ctrl.resolve.filter;
            $ctrl.isCurrentUserFes = $ctrl.resolve.isCurrentUserFes;
            $ctrl.statutOuvrier = $ctrl.filter.StatutPersonnelList.includes("1");
            $ctrl.statutEtam = $ctrl.filter.StatutPersonnelList.includes("2");
            $ctrl.statutCadre = $ctrl.filter.StatutPersonnelList.includes("3");
        };

        /*
         * @function handleValidate()
         * @description Lance le contrôle vrac
         */
        function handleValidate() {
            var isFormNok = (!$ctrl.filter.EtablissementPaieList || $ctrl.filter.EtablissementPaieList.length === 0) && !$ctrl.isAllEtablissement;

            if ($ctrl.controleVracForm.$valid && !isFormNok) {
                $ctrl.close({ $value: $ctrl.filter });
            }
            else if (isFormNok) {
                $ctrl.errorMessage = "Veuillez sélectionner au moins un établissement. Sinon choisissez Tous les établissements.";
            }
        }

        /* 
         * @function handleCancel()
         * @description Annulation de la création
         */
        function handleCancel() {
            $ctrl.dismiss({ $value: 'cancel' });
        }

        /*
         * @description Gestion du choix des établissements
         */
        function handleEtablissementPaieOption() {
            if ($ctrl.isAllEtablissement) {
                $ctrl.filter.EtablissementPaieIdList = [];
                $ctrl.filter.EtablissementPaieList = [];
                $ctrl.emptyEtablissementList = true;
                $ctrl.errorMessage = "";
            }
        }

        /*
         * @description Gestion de la sélection par la Lookup
         */
        function handleLookupSelection(type, item) {

            switch (type) {
                case "Societe":
                    if (item !== null) {
                        $ctrl.filter.Societe = item;
                        $ctrl.filter.SocieteId = item.IdRef;
                    }
                    break;
                case "EtablissementPaie":
                    if (item !== null) {

                        if ($ctrl.filter.EtablissementPaieIdList.indexOf(item.IdRef) > -1) {
                            Notify.error($ctrl.resources.VPWeb_EtablissementDejaChoisi);
                        }
                        else {
                            $ctrl.filter.EtablissementPaieIdList.push(item.IdRef);
                            $ctrl.filter.EtablissementPaieList.push(item);
                            $ctrl.emptyEtablissementList = false;
                            $ctrl.errorMessage = "";
                        }

                    }
                    break;
            }
        }

        /*
         * @description Fonction suppression d'un item dans la lookup en fonction du type de lookup
         */
        function handleLookupDeletion(type, item) {

            switch (type) {
                case "Societe":
                    $ctrl.filter.Societe = null;
                    $ctrl.filter.SocieteId = null;
                    $ctrl.filter.EtablissementPaieIdList = [];
                    $ctrl.filter.EtablissementPaieList = [];
                    $ctrl.emptyEtablissementList = true;
                    break;
                case "EtablissementPaie":
                    if (item) {
                        var i1 = $ctrl.filter.EtablissementPaieIdList.indexOf(item.IdRef);
                        var i2 = $ctrl.filter.EtablissementPaieList.indexOf(item);
                        $ctrl.filter.EtablissementPaieIdList.splice(i1, 1);
                        $ctrl.filter.EtablissementPaieList.splice(i2, 1);
                        $ctrl.emptyEtablissementList = $ctrl.filter.EtablissementPaieList.length === 0;
                    }
                    break;
            }
        }

        function handleShowLookupSociete() {
            if ($ctrl.isCurrentUserFes) {
                return '/api/Societe/GetSocietesListForRemonteeVracFesAsync';
            }
            var listTypeSociete = ['INT'];
            return '/api/Societe/SearchLight/?typeSocieteCodes=' + listTypeSociete + '&';
        }

        function handleChangePersonnelStatutList(type) {
            var value, boolean;

            switch (type) {
                case "StatutOuvrier":
                    value = "1";
                    boolean = $ctrl.statutOuvrier;
                    break;
                case "StatutEtam":
                    value = "2";
                    boolean = $ctrl.statutEtam;
                    break;
                case "StatutCadre":
                    value = "3";
                    boolean = $ctrl.statutCadre;
                    break;
            }

            if (boolean) {
                $ctrl.filter.StatutPersonnelList.push(value);
            }
            else {
                $ctrl.filter.StatutPersonnelList.splice($ctrl.filter.StatutPersonnelList.indexOf(value), 1);
            }
        }

        function handleShowLookupEtabPaie(societeId) {
            if ($ctrl.isCurrentUserFes) {
                return String.format('/api/EtablissementPaie/GetEtabPaieListForValidationPointageVracFesAsync/?page=1&pageSize=20&societeId={0}', societeId);
            }

            return String.format('/api/EtablissementPaie/SearchLight/?page=1&pageSize=20&societeId={0}', societeId);
        }
    }

}(angular));