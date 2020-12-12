(function (angular) {
    'use strict';

    angular.module('Fred').component('formulaireContratInterimComponent', {
        templateUrl: '/Areas/Personnel/Scripts/modals/formulaire-contratInterim-modal.html',
        bindings: {
            resolve: '<',
            close: '&',
            dismiss: '&'
        },
        controller: 'FormulaireContratInterimComponentController'
    });

    FormulaireContratInterimComponentController.$inject = ['ZoneDeTravailService', 'ContratInterimService', '$uibModal', 'FeatureFlags', '$filter'];

    function FormulaireContratInterimComponentController(ZoneDeTravailService, ContratInterimService, $uibModal, FeatureFlags, $filter) {
        var $ctrl = this;

        angular.extend($ctrl, {
            // Fonctions
            handleSave: handleSave,
            handleCancel: handleCancel,
            handleDelete: handleDelete,
            handleChangeValorisation: handleChangeValorisation,
            handleValorisationValidation: handleValorisationValidation,
            handleSelectedItem: handleSelectedItem,
            handleDateValidation: handleDateValidation,
            AddZoneDeTravail: AddZoneDeTravail,
            DeleteZoneDeTravail: DeleteZoneDeTravail,
            showPickList: showPickList,
            GetContratInterimByNumeroContrat: GetContratInterimByNumeroContrat,
            GetCiDevise: GetCiDevise,
            FillMotifRemplacement: FillMotifRemplacement,
            showPickListFournisseur: showPickListFournisseur,
            handleSelectedItemFournisseur: handleSelectedItemFournisseur,
            handleDeleteFournisseur: handleDeleteFournisseur,

            // Variables
            selectedRef: null,
            contratInterimForm: {},
            errorNumeroExist: false,
            errorContratExist: false,
            errorCiDevise: false,
            errorCompteInterneSepIdNull: false,
            CiLibelleList: null,
            error: false,
            energieDisabled: true,
            libelleCiWithCode: null
        });

        /*
         * Initilisation du controller de la modal
         */
        $ctrl.$onInit = function () {

            ContratInterimService.GetMotifRemplacement().then(function (value) {
                $ctrl.MotifRemplacementList = value.data;
            });

            $ctrl.ContratInterimModal = $ctrl.resolve.contratInterim;
            $ctrl.resources = $ctrl.resolve.resources;
            $ctrl.readOnly = $ctrl.resolve.readOnly;

            if ($ctrl.ContratInterimModal.ContratInterimaireId !== 0 && !$ctrl.readOnly) {
                ContratInterimService.GetPointageForContratInterimaire($ctrl.ContratInterimModal).then(function (value) {
                    if (value.data !== 0) {
                        $ctrl.readOnly = true;
                    }
                });
            }

            if ($ctrl.ContratInterimModal.ContratInterimaireId !== 0) {
                ContratInterimService.GetCiInRapportLigneByDateContratInterimaire($ctrl.ContratInterimModal).then(function (value) {
                    $ctrl.CiLibelleList = value.data;
                });

                $ctrl.libelleCiWithCode = $ctrl.ContratInterimModal.Ci.Code + ' - ' + $ctrl.ContratInterimModal.Ci.Libelle;
            }

            checkEnergie(true);

            // Setting Fournisseur validity
            if ($ctrl.ContratInterimModal && $ctrl.contratInterimForm && $ctrl.contratInterimForm.Fournisseur) {
                $ctrl.contratInterimForm.Fournisseur.$setValidity('siren', $ctrl.ContratInterimModal.Fournisseur.SIREN && $ctrl.ContratInterimModal.Fournisseur.SIREN !== '0' && $ctrl.ContratInterimModal.Fournisseur.SIREN !== '000000000');
            }

            $ctrl.blocageFournisseurSansETTFeature = FeatureFlags.getFlagStatus('blocageFournisseursSansSIRET');
        };

        /*
         * @function handleSave()
         * @description Enregistrement de la nouvelle delegation : Renvoie les valeurs au controller principal
         */
        function handleSave() {
            if (!$ctrl.contratInterimForm.$invalid) {
                GetContratInterimAlreadyActif();
                GetCiDevise();
                GetContratInterimByNumeroContrat();
                checkCompteInterneSepId($ctrl.ContratInterimModal.Ci);
                if (!$ctrl.errorCiDevise && !$ctrl.errorNumeroExist && !$ctrl.errorContratExist && !$ctrl.errorCompteInterneSepIdNull) {
                    if ($ctrl.ContratInterimModal.ContratInterimaireId > 0) {
                        $ctrl.close({ $value: $ctrl.ContratInterimModal });
                    } else {
                        document.getElementsByClassName('app-modal-window')[0].style.display = 'none';
                        var modalInstance = $uibModal.open({
                            animation: true,
                            component: 'validateContratInterimComponent',
                            backdrop: 'static',
                            resolve: {
                                ContratInterimModal: function () { return $ctrl.ContratInterimModal; },
                                resources: function () { return $ctrl.resources; }
                            }
                        });

                        modalInstance.result.then(function (result) {
                            if (result.validate) {
                                $ctrl.close({ $value: result.contratInterim });
                            } else {
                                document.getElementsByClassName('app-modal-window')[0].style.display = 'block';
                            }
                        });
                    }

                }
            }
        }

        function GetCiDevise() {
            if ($ctrl.ContratInterimModal.CiId !== null) {
                ContratInterimService.GetCiDevise($ctrl.ContratInterimModal.CiId).then(function (value) {
                    if (value.data !== null) {
                        $ctrl.errorCiDevise = false;
                        if (!$ctrl.errorNumeroExist && !$ctrl.errorContratExist && !$ctrl.errorCompteInterneSepIdNull) {
                            $ctrl.error = false;
                        }
                    }
                    else {
                        $ctrl.errorCiDevise = true;
                        $ctrl.error = true;
                    }
                }).catch(function (error) {
                    console.log(error);
                });
            }
        }

        function GetContratInterimByNumeroContrat() {
            if ($ctrl.ContratInterimModal.NumContrat !== null) {
                ContratInterimService.GetContratInterimByNumeroContrat($ctrl.ContratInterimModal.NumContrat, $ctrl.ContratInterimModal.ContratInterimaireId).then(function (value) {
                    if (value.data === null) {
                        $ctrl.errorNumeroExist = false;
                        if (!$ctrl.errorCiDevise && !$ctrl.errorContratExist && !$ctrl.errorCompteInterneSepIdNull) {
                            $ctrl.error = false;
                        }
                    }
                    else {
                        $ctrl.errorNumeroExist = true;
                        $ctrl.error = true;
                    }
                }).catch(function (error) {
                    console.log(error);
                });
            }
        }

        function GetContratInterimAlreadyActif() {
            if ($ctrl.ContratInterimModal.DateDebut !== null && $ctrl.ContratInterimModal.DateFin !== null) {
                ContratInterimService.GetContratInterimActif($ctrl.ContratInterimModal.ContratInterimaireId, $ctrl.ContratInterimModal.InterimaireId, $ctrl.ContratInterimModal.DateDebut, $ctrl.ContratInterimModal.DateFin)
                    .then(function (value) {
                        if (value.data === null) {
                            $ctrl.errorContratExist = false;
                            if (!$ctrl.errorCiDevise && !$ctrl.errorNumeroExist && !$ctrl.errorCompteInterneSepIdNull) {
                                $ctrl.error = false;
                            }
                        }
                        else {
                            $ctrl.errorContratExist = true;
                            $ctrl.error = true;
                        }
                    }).catch(function (error) {
                        console.log(error);
                    });
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
         * @function handleDateValidation
         * @description Valide les dates de début et de fin de délégation
         */
        function handleDateValidation() {
            $ctrl.contratInterimForm.DateDebut.$setValidity("RangeError", $ctrl.ContratInterimModal.DateDebut <= $ctrl.ContratInterimModal.DateFin);
            GetContratInterimAlreadyActif();
        }


        /*
        * @function handleDateValidation
        * @description Valide les dates de début et de fin de délégation
        */
        function handleChangeValorisation() {
            if ($ctrl.ContratInterimModal.Valorisation === null || $ctrl.ContratInterimModal.TarifUnitaire > $ctrl.ContratInterimModal.Valorisation) {
                $ctrl.ContratInterimModal.Valorisation = $ctrl.ContratInterimModal.TarifUnitaire;
            }
        }

        /*
        * @function handleDateValidation
        * @description Valide les dates de début et de fin de délégation
        */
        function handleValorisationValidation() {
            if ($ctrl.ContratInterimModal.TarifUnitaire !== null && $ctrl.ContratInterimModal.TarifUnitaire !== undefined) {
                $ctrl.contratInterimForm.Valorisation.$setValidity("RangeError", $ctrl.ContratInterimModal.TarifUnitaire <= $ctrl.ContratInterimModal.Valorisation);
            }
        }

        function AddZoneDeTravail(item) {
            var zoneDeTravailFiltered = $ctrl.ContratInterimModal.ZonesDeTravail.filter(z => z.EtablissementComptable.EtablissementComptableId === Number(item.IdRef));
            if (zoneDeTravailFiltered.length === 0) {
                var zoneDeTravail = {
                    EtablissementComptableId: item.IdRef,
                    EtablissementComptable: item,
                    ContratInterimaireId: $ctrl.ContratInterimModal.ContratInterimaireId
                };
                $ctrl.ContratInterimModal.ZonesDeTravail.push(zoneDeTravail);
            }
        }

        function DeleteZoneDeTravail(zoneDeTravail) {
            if (!$ctrl.readOnly) {
                var index = $ctrl.ContratInterimModal.ZonesDeTravail.indexOf(zoneDeTravail);
                $ctrl.ContratInterimModal.ZonesDeTravail.splice(index, 1);
            }
        }

        function manageZoneDeTravailWhenCiChanged() {
            $ctrl.ContratInterimModal.ZonesDeTravail = null;
            if ($ctrl.ContratInterimModal.Ci.EtablissementComptableId) {
                var zoneDeTravail = {
                    EtablissementComptableId: $ctrl.ContratInterimModal.Ci.EtablissementComptableId,
                    EtablissementComptable: $ctrl.ContratInterimModal.Ci.EtablissementComptable,
                    ContratInterimaireId: $ctrl.ContratInterimModal.ContratInterimaireId
                };
                $ctrl.ContratInterimModal.ZonesDeTravail = [];
                $ctrl.ContratInterimModal.ZonesDeTravail.push(zoneDeTravail);
            }
        }

        /*
         * -------------------------------------------------------------------------------------------------------------
         *                                            GESTION DE LA PICKLIST
         * -------------------------------------------------------------------------------------------------------------
         */
        function FillMotifRemplacement() {
            $ctrl.ContratInterimModal.MotifRemplacement = $ctrl.MotifRemplacementList.find(motif => motif.MotifRemplacementId === parseInt($ctrl.ContratInterimModal.MotifRemplacementId));
        }

        function handleDelete(type) {
            switch (type) {
                case "Societe":
                    $ctrl.ContratInterimModal.SocieteId = null;
                    $ctrl.ContratInterimModal.Societe = null;
                    //initialiser le ci a chaque changement de societe
                    $ctrl.ContratInterimModal.CiId = null;
                    $ctrl.ContratInterimModal.Ci = null;
                    // RG_6472_006: initialiser la zone de travail
                    $ctrl.ContratInterimModal.ZonesDeTravail = null;
                    $ctrl.libelleCiWithCode = null;
                    break;
                case "CI":
                    $ctrl.ContratInterimModal.CiId = null;
                    $ctrl.ContratInterimModal.Ci = null;
                    // RG_6472_006: initialiser la zone de travail
                    $ctrl.ContratInterimModal.ZonesDeTravail = null;
                    $ctrl.libelleCiWithCode = null;
                    break;
                case "PersonnelRemplace":
                    $ctrl.ContratInterimModal.PersonnelRemplaceId = null;
                    $ctrl.ContratInterimModal.PersonnelRemplace = null;
                    break;
                case "Fournisseur":
                    $ctrl.ContratInterimModal.FournisseurId = null;
                    $ctrl.ContratInterimModal.Fournisseur = null;

                    // Feature Flipping Fournisseur sans SIRET
                    if ($ctrl.blocageFournisseurSansETTFeature && $ctrl.contratInterimForm && $ctrl.contratInterimForm.Fournisseur) {
                        $ctrl.contratInterimForm.Fournisseur.$setValidity('siren', true);
                        angular.noop();
                    }
                    break;
                case "Ressource":
                    $ctrl.ContratInterimModal.RessourceId = null;
                    $ctrl.ContratInterimModal.Ressource = null;
                    break;
                case "Unite":
                    $ctrl.ContratInterimModal.UniteId = null;
                    $ctrl.ContratInterimModal.Unite = null;
                    break;
            }
        }

        function handleSelectedItem(item, type) {
            switch (type) {
                case "Societe":
                    $ctrl.ContratInterimModal.SocieteId = item.IdRef;
                    $ctrl.ContratInterimModal.Societe = item;
                    //initialiser le ci a chaque changement de societe
                    $ctrl.ContratInterimModal.CiId = null;
                    $ctrl.ContratInterimModal.Ci = null;
                    // RG_6472_006: initialiser la zone de travail
                    $ctrl.ContratInterimModal.ZonesDeTravail = null;
                    break;
                case "CI":
                    $ctrl.ContratInterimModal.CiId = item.IdRef;
                    $ctrl.ContratInterimModal.Ci = item;
                    GetCiDevise();
                    checkCompteInterneSepId(item);
                    checkEnergie(false);
                    manageZoneDeTravailWhenCiChanged();
                    $ctrl.libelleCiWithCode = $ctrl.ContratInterimModal.Ci.Code + ' - ' + $ctrl.ContratInterimModal.Ci.Libelle;
                    break;
                case "PersonnelRemplace":
                    $ctrl.ContratInterimModal.PersonnelRemplaceId = item.IdRef;
                    $ctrl.ContratInterimModal.PersonnelRemplace = item;
                    break;
                case "Fournisseur":

                    // Feature Flipping Fournisseur sans SIRET
                    if ($ctrl.blocageFournisseurSansETTFeature && $ctrl.contratInterimForm && $ctrl.contratInterimForm.Fournisseur) {
                        $ctrl.contratInterimForm.Fournisseur.$setValidity('siren', item.SIREN && item.SIREN !== '0' && item.SIREN !== '000000000');
                        angular.noop();
                    }
                    $ctrl.ContratInterimModal.FournisseurId = item.IdRef;
                    $ctrl.ContratInterimModal.Fournisseur = item;
                    break;
                case "Ressource":
                    $ctrl.ContratInterimModal.RessourceId = item.IdRef;
                    $ctrl.ContratInterimModal.Ressource = item;
                    break;
                case "Unite":
                    $ctrl.ContratInterimModal.UniteId = item.IdRef;
                    $ctrl.ContratInterimModal.Unite = item;
                    break;
            }
        }

        function checkCompteInterneSepId(item) {
            // RG_6472_002b 
            if ($ctrl.ContratInterimModal.Ci.Societe.TypeSociete.Code === 'SEP' && !item.CompteInterneSepId) {
                $ctrl.errorCompteInterneSepIdNull = true;
            } else {
                $ctrl.errorCompteInterneSepIdNull = false;
            }
        }

        function checkEnergie(checkOnlyDisable) {
            // RG_6472_004
            if ($ctrl.ContratInterimModal.Ci && $ctrl.ContratInterimModal.Societe) {
                if ($ctrl.ContratInterimModal.Ci.Societe.TypeSociete.Code === 'SEP') {
                    if ($ctrl.ContratInterimModal.Societe.TypeSociete.Code === 'INT') {
                        // Si le CI sélectionné est lié à une société SEP, 
                        // et si la « Société Contractante » est de type Interne : 
                        // il faut passer automatiquement le toggle « Energies » à « Faux » 
                        // et l’activer afin que l’utilisateur puisse ensuite le passer à « Vrai » s’il le souhaite
                        if (!checkOnlyDisable) {
                            $ctrl.ContratInterimModal.Energie = false;
                        }
                        $ctrl.energieDisabled = false;
                    } else if ($ctrl.ContratInterimModal.Societe.TypeSociete.Code === 'PAR') {
                        // Si le CI sélectionné est lié à une société SEP, 
                        // et si la « Société Contractante » est de type Partenaire : 
                        // il faut passer automatiquement le toggle « Energies » à « Vrai » 
                        // et le désactiver afin que l’utilisateur NE puisse PAS le modifier
                        if (!checkOnlyDisable) {
                            $ctrl.ContratInterimModal.Energie = true;
                        }
                        $ctrl.energieDisabled = true;
                    }
                } else if ($ctrl.ContratInterimModal.Ci.Societe.TypeSociete.Code === 'INT' && $ctrl.ContratInterimModal.Societe.TypeSociete.Code === 'INT') {
                    // Si le CI sélectionné est lié à une société Interne, 
                    // et si la « Société Contractante » est de type Interne : 
                    // il faut passer automatiquement le toggle « Energies » à « Faux » 
                    // et le désactiver afin que l’utilisateur NE puisse PAS le modifier 
                    if (!checkOnlyDisable) {
                        $ctrl.ContratInterimModal.Energie = false;
                    }
                    $ctrl.energieDisabled = true;
                }
            }
        }

        function showPickList(val) {

            var searchLightUrl = '/api/' + val + '/SearchLight/?page={0}&societeId={1}&ciId={2}&groupeId={3}';

            switch (val) {
                case "Societe":
                    var typeSocieteInterne = 'INT';
                    var typeSocietePartenaire = 'PAR';
                    searchLightUrl = '/api/Societe/SearchLight/?typeSocieteCodes=' + typeSocieteInterne + '&typeSocieteCodes=' + typeSocietePartenaire + "&";
                    break;
                case "CI":
                    searchLightUrl = String.format('/api/CI/SearchLightBySocieteId?personnelSocieteId={0}&includeSep=true&', $ctrl.ContratInterimModal.SocieteId);
                    break;
                case "Personnel":
                    searchLightUrl = String.format(searchLightUrl, 1, null, null, null);
                    break;
                case "Fournisseur":
                    searchLightUrl = "/api/Fournisseur/ETT/" + $ctrl.ContratInterimModal.Societe.GroupeId;
                    break;
                case "EtablissementComptable":
                    searchLightUrl = String.format(searchLightUrl, 1, $ctrl.ContratInterimModal.Ci.SocieteId, null, null);
                    break;
                case "RessourcePersonnel":
                    if ($ctrl.ContratInterimModal.SocieteId !== null) {
                        searchLightUrl = String.format(searchLightUrl, 1, $ctrl.ContratInterimModal.SocieteId, null, null);
                    }
                    break;
                case "Unite":
                    searchLightUrl = String.format(searchLightUrl, 1, null, null, null);
                    break;
            }
            return searchLightUrl;
        }

        function showPickListFournisseur() {

            var searchLightUrl = "/api/Fournisseur/ETT/" + $ctrl.ContratInterimModal.Societe.GroupeId;
            return searchLightUrl;
        }

        function handleSelectedItemFournisseur(item) {

            if ($ctrl.blocageFournisseurSansETTFeature && $ctrl.contratInterimForm && $ctrl.contratInterimForm.Fournisseur) {
                $ctrl.contratInterimForm.Fournisseur.$setValidity('siren', item.SIREN && item.SIREN !== '0' && item.SIREN !== '000000000');
                angular.noop();
            }
            $ctrl.ContratInterimModal.FournisseurId = item.IdRef;
            $ctrl.ContratInterimModal.Fournisseur = item;
        }

        function handleDeleteFournisseur() {

            $ctrl.ContratInterimModal.FournisseurId = null;
            $ctrl.ContratInterimModal.Fournisseur = null;

            // Feature Flipping Fournisseur sans SIRET
            if ($ctrl.blocageFournisseurSansETTFeature && $ctrl.contratInterimForm && $ctrl.contratInterimForm.Fournisseur) {
                $ctrl.contratInterimForm.Fournisseur.$setValidity('siren', true);
                angular.noop();
            }
        }

    }

    angular.module('Fred').controller('FormulaireContratInterimComponentController', FormulaireContratInterimComponentController);

}(angular));