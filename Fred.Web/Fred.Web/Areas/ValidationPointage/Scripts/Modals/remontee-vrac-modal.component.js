(function (angular) {
    'use strict';

    var remonteeVracComponent = {
        templateUrl: '/Areas/ValidationPointage/Scripts/Modals/remontee-vrac-modal.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: remonteeVracController
    };

    angular.module('Fred').component('remonteeVracComponent', remonteeVracComponent);

    angular.module('Fred').controller('remonteeVracController', remonteeVracController);

    remonteeVracController.$inject = ['Notify', 'UserService'];

    function remonteeVracController(Notify, UserService) {
        var $ctrl = this;

        angular.extend($ctrl, {
            // Fonctions
            handleValidate: handleValidate,
            handleCancel: handleCancel,
            handleLookupSelection: handleLookupSelection,
            handleLookupDeletion: handleLookupDeletion,
            handleEtablissementPaieOption: handleEtablissementPaieOption,
            handleMatriculeOption: handleMatriculeOption,
            handleShowLookupSociete: handleShowLookupSociete,
            handleChangePersonnelStatutList: handleChangePersonnelStatutList,
            handleShowLookupPersonnelFes: handleShowLookupPersonnelFes,
            handleShowLookupEtabPaie: handleShowLookupEtabPaie,
            // Variables                        
            filter: null,
            isAllEtablissement: true,
            isAllMatricule: true,
            errorMessage: "",
            remonteeVracForm: {}
        });

        /*
         * Initilisation du controller de la modal
         */
        $ctrl.$onInit = function () {
            UserService.getCurrentUser().then(function(user) {
                $ctrl.isUserFayatTp = user.Personnel.Societe.Groupe.Code.trim() === 'GFTP' ? true : false;
                $ctrl.isCurrentUserFes = $ctrl.resolve.isCurrentUserFes;
                $ctrl.resources = $ctrl.resolve.resources;
                $ctrl.handleShowLookup = $ctrl.resolve.handleShowLookup;
                $ctrl.filter = $ctrl.resolve.filter;
                $ctrl.isCurrentUserFes = $ctrl.resolve.isCurrentUserFes;
                $ctrl.statutOuvrier = $ctrl.filter.StatutPersonnelList.includes("1");
                $ctrl.statutEtam = $ctrl.filter.StatutPersonnelList.includes("2");
                $ctrl.statutCadre = $ctrl.filter.StatutPersonnelList.includes("3");
                if ($ctrl.isUserFayatTp || $ctrl.isCurrentUserFes) {
                    $ctrl.filter.UpdateAbsence = true;
                }
            });
        };

        /*
         * @function handleValidate()
         * @description Lance le contrôle vrac
         */
        function handleValidate() {
            var isFormNok = actionCheckForm();
            if ($ctrl.remonteeVracForm.$valid && !isFormNok) {
                $ctrl.close({ $value: $ctrl.filter });
            }
            else if (isFormNok) {
                $ctrl.errorMessage = "Veuillez saisir soit un ou plusieurs établissement(s), soit un matricule.";
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
            }
        }

        /*
         * @description Gestion du choix des matricules
         */
        function handleMatriculeOption() {
            if ($ctrl.isAllMatricule) {
                $ctrl.filter.Matricule = null;
                $ctrl.filter.Personnel = null;
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
                        $ctrl.filter.Personnel = null;
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
                        }

                    }
                    break;
                case "Personnel":
                    $ctrl.filter.Matricule = item.Matricule;
                    $ctrl.filter.Personnel = item;
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
                    $ctrl.filter.EtablissementPaieList = [];
                    $ctrl.filter.EtablissementPaieIdList = [];
                    $ctrl.filter.Matricule = null;
                    $ctrl.filter.Personnel = null;
                    break;
                case "EtablissementPaie":
                    if (item) {
                        var i1 = $ctrl.filter.EtablissementPaieIdList.indexOf(item.IdRef);
                        var i2 = $ctrl.filter.EtablissementPaieList.indexOf(item);
                        $ctrl.filter.EtablissementPaieIdList.splice(i1, 1);
                        $ctrl.filter.EtablissementPaieList.splice(i2, 1);
                    }
                    break;
                case "Personnel":
                    $ctrl.filter.Matricule = null;
                    $ctrl.filter.Personnel = null;
                    break;
            }
        }

        /*
         * @description Validation du formulaire
         */
        function actionCheckForm() {
            return $ctrl.filter.EtablissementPaieList && $ctrl.filter.EtablissementPaieList.length > 0 && $ctrl.filter.Matricule;
        }

        function handleShowLookupSociete() {
            if ($ctrl.isCurrentUserFes) {
                return '/api/Societe/GetSocietesListForRemonteeVracFesAsync';
            }

            var listTypeSociete = ['INT'];
            return '/api/Societe/SearchLight/?typeSocieteCodes=' + listTypeSociete + '&';
        }

        function handleShowLookupPersonnelFes(societeId) {
            var url = String.format('/api/Personnel/GetPersonnelListForValidationPointageVracFesAsync/?page=1&pageSize=20&societeId={0}&dateDebut={1}&', societeId, $ctrl.resolve.periode);

            $ctrl.filter.StatutPersonnelList.forEach(statut => {
                url = url + 'statutPersonnelList=' + statut + '&';
            });

            return url;
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