(function (angular) {
    'use strict';

    angular.module('Fred').controller('PersonnelEditController', PersonnelEditController);

    PersonnelEditController.$inject = ['$q',
        '$filter',
        '$window',
        '$timeout',
        '$uibModal',
        'PersonnelService',
        'ContratInterimService',
        'MatriculeExterneService',
        'RapportService',
        'DelegationService',
        'UtilisateurService',
        'ProgressBar',
        'Notify',
        'confirmDialog',
        'NgMap',
        'GoogleService',
        'SocieteService',
        'PersonnelEditContactService',
        'PersonnelEditCartoService',
        'PersonnelEditPersisteStateService',
        'PersonnelEditFieldsCleanerService',
        'PersonnelEditManageLookup',
        'PaysService',
        'UserService',
        'TypeSocieteService'];

    function PersonnelEditController($q,
        $filter,
        $window,
        $timeout,
        $uibModal,
        PersonnelService,
        ContratInterimService,
        MatriculeExterneService,
        RapportService,
        DelegationService,
        UtilisateurService,
        ProgressBar,
        Notify,
        confirmDialog,
        NgMap,
        GoogleService,
        SocieteService,
        PersonnelEditContactService,
        PersonnelEditCartoService,
        PersonnelEditPersisteStateService,
        PersonnelEditFieldsCleanerService,
        PersonnelEditManageLookup,
        PaysService,
        UserService,
        TypeSocieteService) {

        var $ctrl = this;
        var DelegationListInit = new Array();
        var ContratInterimListInit = new Array();
        $ctrl.JustDelegationShow = false;

        UserService.getCurrentUser().then(function (user) {
            $ctrl.userOrganisationId = user.Personnel.Societe.Organisation.OrganisationId;
        });

        $ctrl.defaultCi = null;
        // méthodes exposées
        angular.extend($ctrl, {
            goBack: goBack,
            loadPage: loadPage,
            loadData: loadData,

            //Event
            onFilterDelegation: onFilterDelegation,
            onFilterContratInterim: onFilterContratInterim,
            onSearchContratInterim: onSearchContratInterim,
            compareToDateDelegation: compareToDateDelegation,

            // Handlers INFORMATIONS GENERALES
            handleChangeTypeRattachement: handleChangeTypeRattachement,
            handleCheckNomPrenomExist: handleCheckNomPrenomExist,
            handleToggleInterimaire: handleToggleInterimaire,
            handleCheckSocieteMatriculeExist: handleCheckSocieteMatriculeExist,
            handleCheckMatriculeAndSourceExist: handleCheckMatriculeAndSourceExist,
            handleEmptyDateHeuresInsertion: handleEmptyDateHeuresInsertion,
            handleValidateDateHeuresInsertion: handleValidateDateHeuresInsertion,

            // Handlers CONTACT
            handleCheckGps: handleCheckGps,
            handleChangeSelectAdress: handleChangeSelectAdress,
            handleOpenContactPanel: handleOpenContactPanel,
            handleSelectLocation: handleSelectLocation,
            handleGetEmailRequired: handleGetEmailRequired,
            // Handlers GESTION DES DELEGATION
            handleOpenFormDelegationModal: handleOpenFormDelegationModal,
            handleOpenDeleteDelegationModal: handleOpenDeleteDelegationModal,

            // Handlers GESTION DES CONTRATS D'INTERIMAIRES
            handleOpenFormContratInterimModal: handleOpenFormContratInterimModal,
            handleOpenDeleteContratInterimModal: handleOpenDeleteContratInterimModal,

            // Handlers INFORMATIONS UTILISATEUR 
            handleCanChangeActiveEmailSummary: handleCanChangeActiveEmailSummary,
            handleGetSubscribeToEmailSummaryLabel: handleGetSubscribeToEmailSummaryLabel,
            handleChangeUser: handleChangeUser,
            handleChangeLogin: handleChangeLogin,
            handleChangeLoginInterne: handleChangeLoginInterne,
            handleChangeFolio: handleChangeFolio,
            handleSelectLoginFromActiveDirectory: handleSelectLoginFromActiveDirectory,
            handleChangePassword: handleChangePassword,

            // Handlers HABILITATIONS
            handleDeleteRole: handleDeleteRole,
            handleDeleteOrganisation: handleDeleteOrganisation,
            handleDeleteDevise: handleDeleteDevise,
            handleOpenReferentialOrganisation: handleOpenReferentialOrganisation,
            handleOpenReferentialDevise: handleOpenReferentialDevise,
            handleExportExcel: handleExportExcel,

            // Handlers SIGNATURE PROFIL
            handleSelectImage: handleSelectImage,
            handleRotation: handleRotation,
            handleValidateImage: handleValidateImage,
            handleReset: handleReset,
            handleDeleteImage: handleDeleteImage,

            // PICKLIST
            handleDelete: handleDelete,
            showPickList: showPickList,

            // VALIDATION
            handleValidateDates: handleValidateDates,

            // GLOBAL
            handleCancel: handleCancel,
            handleSave: handleSave,
            formIsSubmitted: formIsSubmitted
        });

        init();

        /**
         * Initialisation du controller.
         * 
         */
        function init() {
            ProgressBar.start();

            UserService.getCurrentUser().then(function (user) {
                angular.extend($ctrl, {
                    // Instanciation Objet Ressources
                    resources: resources,

                    // Initialisation des données  
                    ContratInterimActifOrPlanifie: null,
                    showPartialUtilisateur: true,
                    searchContrat: "",
                    selectedRef: {},
                    typeRattachementList: {},
                    adressePersonnel: {},
                    initialPersonnel: {},
                    initialPersoForUser: {},
                    initialPersoForContact: {},
                    initialListRoles: {},
                    persoForUser: {},
                    persoForContact: {},
                    FolioRegle: null,
                    UtilisateurCheckFolioError: false,
                    Personnel: {},
                    showAddressList: false,
                    addressList: [],
                    formGestionPersonnel: {},
                    roleList: [],
                    today: new Date(),
                    hasMorePage: false,
                    busy: false,
                    paging: { pageSize: 10, currentPage: 1 },
                    validCleSecu: 0,
                    sameNomPrenomPersonnel: null,
                    sameSocieteMatriculePersonnel: null,
                    carto: {
                        latitude: '0',
                        longitude: '0'
                    },
                    societeForAffectation: null,
                    acceptedImageFormat: "image/jpeg,image/png",
                    inputFileType: {
                        signature: {
                            id: '#signature-raw',
                            label: 'Signature',
                            container: '#signature-raw-container'
                        },
                        photoProfil: {
                            id: '#photo-profil-raw',
                            label: 'Photo de profil',
                            container: '#photo-profil-raw-container'
                        }
                    },
                    imgSrcBase: 'data:image/png;base64,',
                    signatureCroppedOpts: {
                        maxWidth: 500,
                        maxHeight: 200,
                        fillColor: '#fff',
                        imageSmoothingEnabled: true,
                        imageSmoothingQuality: 'high'
                    },
                    photoProfilCroppedOpts: {
                        maxWidth: 200,
                        maxHeight: 200,
                        fillColor: '#fff',
                        imageSmoothingEnabled: true,
                        imageSmoothingQuality: 'high'
                    },
                    MatriculeExterne: {
                        MatriculeExterneId: 0,
                        Matricule: '',
                        PersonnelId: 0,
                        Source: 'SAP'
                    },
                    currentUser: user.Personnel
                });
            });

            if (sessionStorage.getItem('viewDelegationActive') === "1") {
                document.getElementById("panel-heading-general").className = "panel-heading collapsed";
                document.getElementById("collapse-general").className = "panel-collapse collapse";
                document.getElementById("panel-heading-delegation").className = "panel-heading";
                document.getElementById("collapse-delegation").className = "panel-collapse collapse delegation in";
                sessionStorage.removeItem('viewDelegationActive');
            }
            if (sessionStorage.getItem('JustDelegationShow') === "1") {
                document.getElementById("panel-heading-delegation").className = "panel-heading";
                document.getElementById("collapse-delegation").className = "panel-collapse collapse delegation in";
                $ctrl.JustDelegationShow = true;
                sessionStorage.removeItem('JustDelegationShow');
            }

            // Récupération et formattage des type société Interne pour paramétrer l'url de la lookup société            
            $ctrl.typeSocieteCodesParams = TypeSocieteService.GetQueryParamTypeSocieteCodes([TypeSocieteService.TypeSocieteCodes.INTERNE, TypeSocieteService.TypeSocieteCodes.PARTENAIRE]);
        }

        return $ctrl;

        function goBack() {
            $window.location.href = "/Personnel/Personnel";
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            INFORMATIONS GENERALES
         * -------------------------------------------------------------------------------------------------------------
         */

        function formIsSubmitted() {
            return $ctrl.formContact.$submitted || $ctrl.formGestionPersonnel.$submitted;
        }

        // Enregistrement des modifications de la section Générale
        function actionSaveGeneral() {
            $q.when()
                .then(handleCheckSocieteMatriculeExist)
                .then(handleCheckMatriculeAndSourceExist)
                .then(handleValidateDateHeuresInsertion)
                .then(function () {

                    if ($ctrl.Personnel.Societe.Groupe.Code === 'GFTP') {
                        $ctrl.MatriculeExterne.Matricule = $ctrl.MatriculeSAP;
                        $ctrl.MatriculeExterne.PersonnelId = $ctrl.Personnel.PersonnelId;
                        $ctrl.Personnel.MatriculeExterne.splice(0, 1, $ctrl.MatriculeExterne);
                    }

                    if ($ctrl.sameSocieteMatriculePersonnel === null) {

                        if ($ctrl.Personnel.PersonnelId > 0) {
                            if (!$ctrl.formGestionPersonnel.$invalid) {
                                return actionUpdatePersonnel($ctrl.Personnel);
                            }
                        }
                        else {
                            if (!$ctrl.formGestionPersonnel.$invalid && !$ctrl.formContact.$invalid) {
                                return actionCreatePersonnel($ctrl.Personnel);
                            }
                        }
                    }

                });
        }

        /*
         * @function UpdatePersonnel
         * @description Création d'un personnel Externe
         * @param {personnelModel} personnel
         */
        function actionCreatePersonnel(personnel) {

            if (personnel.Materiel) {
                personnel.Materiel.Societe = null; // TODO: Corriger côté back (Attach entity)
            }

            // On enregistre également la partie Contact lors de la création d'un nouveau personnel
            PersonnelEditContactService.copyContact($ctrl.persoForContact, personnel);

            return PersonnelService.Create(personnel).$promise.then(function (personnel) {
                modifyBrowserUrl(personnel.PersonnelId, personnel.IsInterimaire);
                Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
                loadPage(personnel.PersonnelId, '');
            })
                .catch(function (reason) {
                    Notify.error(resources.Global_Notification_Error);
                });
        }


        function modifyBrowserUrl(personnelId, isInterimaire) {
            var newUrl = null;
            if (isInterimaire) {
                newUrl = window.location.href.replace('/Personnel/Personnel/Edit/0/interimaire', '/Personnel/Personnel/Edit/' + personnelId);
            }
            else {
                newUrl = window.location.href.replace('/Personnel/Personnel/Edit/0/externe', '/Personnel/Personnel/Edit/' + personnelId);
            }
            if (newUrl !== null) {
                history.pushState({}, '', newUrl);
            }

        }

        /*
         * @function UpdatePersonnel
         * @description Mise à jour des informations générales du personnel
         * @param {personnelModel} personnel
         */
        function actionUpdatePersonnel(personnel) {
            return PersonnelService.Update(personnel).$promise.then(function (value) {
                Notify.message(resources.Global_Notification_Enregistrement_Success);
                persisteState($ctrl.Personnel, "general");
                actionGetPersonnel();
            }, function (reason) {
                handleErrors(reason.data.ModelState);
            });
        }

        /*
         * @description Fonction de récupération des erreurs métier
         */
        function handleErrors(modelState) {
            if (modelState) {
                for (var key in modelState) {
                    var value = modelState[key][0];
                    Notify.error(value);
                }
            }
        }

        /*
         * @function actionCancelGeneral
         * @description Annulation des modifications de la section Générale
         */
        function actionCancelGeneral() {
            restoreState("general");
        }

        /*
         * @function handleChangeTypeRattachement
         * @description Sélection dans la dropdownlist des types de rattachement du personnel
         */
        function handleChangeTypeRattachement(oldValue) {
            if ($ctrl.Personnel.TypeRattachementModel) {
                if ($ctrl.Personnel.TypeRattachementModel.Code === "D") {
                    if ($ctrl.Personnel.EtablissementRattachementId > 0) {

                        confirmDialog.confirm(resources, resources.Personnel_PartialGeneral_EtablissementRattachement_Confirmation, "flaticon flaticon-warning")
                            .then(function () { // Si on confirme le changement de type rattachement
                                $ctrl.Personnel.EtablissementRattachementId = null;
                                $ctrl.Personnel.EtablissementRattachement = null;
                            })
                            .catch(function () { // Si on annule
                                $ctrl.Personnel.TypeRattachement = oldValue.Code;
                                $ctrl.Personnel.TypeRattachementLibelle = oldValue.Libelle;
                                $ctrl.Personnel.TypeRattachementModel = oldValue;
                            });
                    }
                }
                else {
                    $ctrl.Personnel.EtablissementRattachementId = $ctrl.Personnel.EtablissementPaieId;
                    $ctrl.Personnel.EtablissementRattachement = $ctrl.Personnel.EtablissementPaie;
                }

                $ctrl.Personnel.TypeRattachement = $ctrl.Personnel.TypeRattachementModel.Code;
                $ctrl.Personnel.TypeRattachementLibelle = $ctrl.Personnel.TypeRattachementModel.Libelle;
            }

        }

        /*
         * @function handleCheckNomPrenomExist()
         * @description Vérifie si un personnel ne possède pas déjà le même nom et prénom
         */
        function handleCheckNomPrenomExist() {
            if ($ctrl.Personnel.Nom && $ctrl.Personnel.Prenom && $ctrl.Personnel.Societe) {
                PersonnelService.GetByNomPrenom({ nom: $ctrl.Personnel.Nom, prenom: $ctrl.Personnel.Prenom, groupeId: $ctrl.Personnel.Societe.GroupeId }).$promise.then(function (value) {
                    $ctrl.sameNomPrenomPersonnel = value.PersonnelId && value.PersonnelId !== $ctrl.Personnel.PersonnelId ? value : null;
                }).catch(function (error) { console.log(error); });
            }
            else {
                $ctrl.sameNomPrenomPersonnel = null;
            }
        }

        /**
         * Vérifie si un personnel existe déjà dans une société en fonction de son matricule
         * @returns {any} Société trouvée par le matricule
         */
        function handleCheckSocieteMatriculeExist() {
            if ($ctrl.Personnel.Matricule && $ctrl.Personnel.SocieteId) {
                return PersonnelService.GetBySocieteMatricule({ societeId: $ctrl.Personnel.SocieteId, matricule: $ctrl.Personnel.Matricule }).$promise
                    .then(function (value) {
                        $ctrl.sameSocieteMatriculePersonnel = value.PersonnelId && value.PersonnelId !== $ctrl.Personnel.PersonnelId ? value : null;
                    }).catch(function (error) { console.log(error); });
            }
            else {
                $ctrl.sameSocieteMatriculePersonnel = null;
            }
        }

        /**
         * Vérifie si un personnel existe déjà dans une société en fonction de son matricule
         * @returns {any} Société trouvée par le matricule
         */
        function handleCheckMatriculeAndSourceExist() {
            if ($ctrl.MatriculeSAP) {
                $ctrl.MatriculeExterne.Matricule = $ctrl.MatriculeSAP;
                return MatriculeExterneService.GetMatriculeExterneByMatriculeAndSource($ctrl.MatriculeExterne)
                    .then(function (response) {
                        if (response.data) {
                            $ctrl.errorMatriculeExterneExist = true;
                        } else {
                            $ctrl.errorMatriculeExterneExist = false;
                        }
                    }).catch(function (error) { console.log(error); });
            }
            else {
                $ctrl.errorMatriculeExterneExist = false;
            }
        }

        function handleEmptyDateHeuresInsertion() {
            if (!$ctrl.Personnel.HeuresInsertion) {
                $ctrl.Personnel.DateDebutInsertion = null;
                $ctrl.Personnel.DateFinInsertion = null;
                $ctrl.formGestionPersonnel.DateDebutInsertion.$setValidity("RangeError", true);

            }
        }

        function handleValidateDateHeuresInsertion() {
            if ($ctrl.Personnel.HeuresInsertion && $ctrl.Personnel.DateDebutInsertion) {
                var dateDebutInsertion = new Date($ctrl.Personnel.DateDebutInsertion);
                $ctrl.Personnel.DateDebutInsertion = new Date(Date.UTC(dateDebutInsertion.getFullYear(), dateDebutInsertion.getMonth(), dateDebutInsertion.getDate(), 0, 0, 0, 0));

                if ($ctrl.Personnel.DateFinInsertion) {
                    var dateFinInsertion = new Date($ctrl.Personnel.DateFinInsertion);
                    $ctrl.Personnel.DateFinInsertion = new Date(Date.UTC(dateFinInsertion.getFullYear(), dateFinInsertion.getMonth(), dateFinInsertion.getDate(), 0, 0, 0, 0));
                    $ctrl.formGestionPersonnel.DateDebutInsertion.$setValidity("RangeError", $ctrl.Personnel.DateDebutInsertion <= $ctrl.Personnel.DateFinInsertion);
                }
            }
        }

        /*
         * @function handleToggleInterimaire()
         * @description Gestion du toggle Intérimaire
         */
        function handleToggleInterimaire() {
            if ($ctrl.Personnel !== null && $ctrl.Personnel.PersonnelId === 0) {
                if ($ctrl.Personnel.IsInterimaire) {
                    $ctrl.Personnel.IsInterne = false;
                    $q.when()
                        .then(actionGetNextMatriculeInterimaire)
                        .then(actionGetDefaultSocieteInterim);
                }
                else {
                    $ctrl.Personnel.Matricule = "";
                    PersonnelEditFieldsCleanerService.removeSociete($ctrl.Personnel);
                }
            }
        }
        /*
         * @function actionGetNextMatriculeInterimaire
         * @description Récupération d'un nouveau matricule pour le personnel intérimaire
         */
        function actionGetNextMatriculeInterimaire() {
            return PersonnelService.GetNextMatriculeInterimaire().$promise
                .then(function (response) { $ctrl.Personnel.Matricule = response.value; })
                .catch(function (error) { console.log(error); });
        }

        /*
         * @function actionGetDefaultSocieteInterim()
         * @description Récupère la société intérimaire par défaut
         */
        function actionGetDefaultSocieteInterim() {
            return SocieteService.GetDefaultSocieteInterim()
                .then(function (response) {
                    $ctrl.Personnel.Societe = response.data;
                    $ctrl.Personnel.SocieteId = response.data.SocieteId;
                })
                .catch(function (error) { console.log(error); });
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            CONTACT
         * ------------------------------------------------------------------------------------------------------------- 
         */

        /*
        * @function initMapWithContactInfo
        * @description Initialise la carte avec les données de l'utilisateur     
        */
        function initMapWithContactInfo() {
            //si je suis en train de crrer un personnel, je zoom sur la france
            var isNewPersonnel = PersonnelEditContactService.getPersonnelIsNew($ctrl.personnelId);
            if (isNewPersonnel) {
                $timeout(function () {
                    google.maps.event.trigger($ctrl.map, 'resize');
                    PersonnelEditCartoService.zoomOnFrenchCountry();
                }, 0);
                return;
            }
            //si le personnel a deja des coordonnees, je zoom sur la position
            if (PersonnelEditCartoService.getPersonnelHasCoordinates($ctrl.persoForContact)) {
                PersonnelEditCartoService.zoomOnPersonnel($ctrl.persoForContact);
            }
        }

        /*
         * @function handleCheckGps
         * @description Recherche et résolution d'Adresse via le service Google. Récupère une liste d'adresses correspondant à notre recherche     
         */
        function handleCheckGps() {
            var p = $ctrl.persoForContact;
            var pays = !p.Pays ? "" : p.Pays.Code;
            GoogleService.geocode(p.Adresse1, p.Adresse2, p.Adresse3, p.CodePostal, p.Ville, pays).then(function (response) {
                var data = response.data;
                if (data && data.length > 0) {
                    $ctrl.addressList = data;
                    PersonnelEditCartoService.zoomOn($ctrl.Personnel, data);
                    PersonnelEditCartoService.setCoordinateOnPersonnel($ctrl.Personnel);
                    $ctrl.showAddressList = true;
                }
            }).catch(function (error) {
                Notify.error(error.data.Message);
            });
        }

        function handleSelectLocation() {
            $ctrl.map.markers.mapSelectorMap.setMap(null);
            $ctrl.carto.latitude = $ctrl.mapSelector.Latitude;
            $ctrl.carto.longitude = $ctrl.mapSelector.Longitude;
            GoogleService.inverserGeocode($ctrl.carto.latitude, $ctrl.carto.longitude).then(function (response) {
                var data = response.data;
                if (data && data.length > 0) {
                    $ctrl.addressList = data;
                    PersonnelEditContactService.setContactAddressWithInverseGeocode($ctrl.persoForContact, data[0], $ctrl.carto.latitude, $ctrl.carto.longitude);
                    actionGetPays(data[0].Adresse.Pays);
                    $ctrl.showAddressList = false;
                }
            }).catch(function (error) {
                Notify.error(error.data.Message);
            });


        }

        /*
         * @function handleChangeSelectAdress(item)
         * @description Gère la sélection d'une adresse proposée: rempli les champs à l'écran et repositionne le marker     
         * @param {any} item : adresse choisie
         */
        function handleChangeSelectAdress(item) {
            $ctrl.addressList = item;
            PersonnelEditContactService.setContactAdress($ctrl.persoForContact, item);
            actionGetPays(item.Adresse.Pays);
            PersonnelEditCartoService.setCoordinateOnPersonnel($ctrl.persoForContact);
            $ctrl.showAddressList = false;
        }

        /*
         * @function: handleOpenContactPanel
         * @description Charge la carte google map à l'ouverture du panel Contact     
         */
        function handleOpenContactPanel() {
            var center = $ctrl.map.getCenter();
            $timeout(function () {
                google.maps.event.trigger($ctrl.map, 'resize');
                $ctrl.map.setCenter(center);
            }, 0);
        }

        /*
         * @function : actionSaveContact
         * @description Sauvegarde de la partie Contact : adresse du personnel     
         */
        function actionSaveContact() {
            if (!$ctrl.formContact.$invalid) {
                return actionUpdateContact($ctrl.persoForContact);
            }
        }
        /*
         * @function UpdatePersonnel
         * @description Mise à jour des informations générales du personnel
         * @param {personnelModel} personnel
         */
        function actionUpdateContact(personnel) {
            return PersonnelService.Update(personnel).$promise.then(function (value) {
                Notify.message(resources.Global_Notification_Enregistrement_Success);
                persisteState($ctrl.persoForContact, "contact");
            }, function (result) {
                if (result && result.data && result.data.ModelState) {
                    var errorMessageList = actionGetErrors(result.data.ModelState);
                    Notify.error(errorMessageList.join('</br>'));
                }
                else {
                    Notify.error(resources.Global_Notification_Error);
                }
            });
        }
        /*
         * @function : handleCancelAdresse
         * @description Annulation des modification de la section Contact     
         */
        function actionCancelContact() {
            restoreState("contact");
        }


        function handleGetEmailRequired() {
            if ($ctrl.persoForUser && $ctrl.persoForUser.hasSubscribeToEmailSummary) {
                return $ctrl.persoForUser.hasSubscribeToEmailSummary;
            }

            return false;
        }


        /* -------------------------------------------------------------------------------------------------------------
         *                                            GESTION DES DELEGATIONS
         * -------------------------------------------------------------------------------------------------------------
         */
        function actionGetDelegation() {
            return DelegationService.GetDelegationByPersonnelId($ctrl.Personnel.PersonnelId).then(function (value) {
                $ctrl.DelegationList = value.data;
                var personnel;
                $ctrl.DelegationList.forEach(function (delegation) {
                    if (delegation.Commentaire === "") {
                        delegation.Commentaire = "Aucun commentaire";
                    }
                    if (delegation.PersonnelDelegantId === $ctrl.Personnel.PersonnelId) {
                        delegation.Issued = true;
                        personnel = delegation.PersonnelDelegue;
                    } else {
                        personnel = delegation.PersonnelDelegant;
                        delegation.Issued = false;
                    }
                    delegation.StatutRH = GetStatutRH(personnel);
                });
                DelegationListInit = angular.copy($ctrl.DelegationList);
                snapshotState("delegation");
            }).catch(function (error) { console.log(error); });
        }

        function GetStatutRH(personnel) {
            switch (personnel.Statut) {
                case "1":
                    return "Ouvrier";
                case "2":
                    return "ETAM Chantier";
                case "3":
                    return "Cadre";
                case "4":
                    return "ETAM Bureau";
                case "5":
                    return "ETAM Article 36";
            }
        }
        /* 
        * @function handleOpenDelegationModal(delegationExistante)
        * @description Ouvre la modal permettant d'ajouter une nouvelle délégation
        * @param {object} Object Delegation    
        */
        function handleOpenFormDelegationModal(delegationExistante) {
            var delegation = {
                DelegationId: 0,
                PersonnelAuteurId: 0,
                PersonnelDelegantId: $ctrl.Personnel.PersonnelId,
                PersonnelDelegueId: 0,
                Activated: false,
                DateDeDebut: new Date(),
                DateDeFin: new Date(),
                Commentaire: "",
                DateCreation: new Date(),
                DateModification: null,
                DateDesactivation: null
            };

            if (delegationExistante !== undefined && delegationExistante !== null) {
                delegation = delegationExistante;
                if (delegation.Commentaire === "Aucun commentaire") {
                    delegation.Commentaire = "";
                }
            }

            var modalInstance = $uibModal.open({
                animation: true,
                component: 'formulaireDelegationComponent',
                resolve: {
                    delegation: function () { return delegation; },
                    societeId: function () { return $ctrl.Personnel.SocieteId; },
                    resources: function () { return $ctrl.resources; }
                }
            });


            // Traitement des données renvoyées par la modal
            modalInstance.result.then(function (delegation) {
                DelegationTraitment(delegation);
            });
        }

        function handleOpenDeleteDelegationModal(delegation) {
            var modalInstance = $uibModal.open({
                animation: true,
                component: 'deleteDelegationComponent',
                resolve: {
                    delegation: function () { return delegation; },
                    resources: function () { return $ctrl.resources; }
                }
            });

            // Traitement des données renvoyées par la modal
            modalInstance.result.then(function () {
                if (delegation.Activated) {
                    DesactivateDelegation(delegation);
                } else {
                    DeleteDelegation(delegation);
                }
            });
        }

        function DelegationTraitment(delegation) {
            UtilisateurService.GetCurrentUser().$promise.then(function (value) {
                delegation.PersonnelAuteurId = value.UtilisateurId;
                if (delegation !== undefined && delegation !== null) {
                    if (delegation.DelegationId > 0) {
                        UpdateDelegation(delegation);
                    } else {
                        AddDelegation(delegation);
                    }
                }
            }).catch(function (error) {
                console.log(error);
                Notify.error($ctrl.resources.Global_Notification_Error);
            });
        }

        function AddDelegation(delegation) {
            DelegationService.AddDelegation(delegation).then(function () {
                actionGetDelegation();
                Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
            }).catch(function (error) {
                console.log(error);
                Notify.error($ctrl.resources.Global_Notification_Error);
            });
        }

        function UpdateDelegation(delegation) {
            DelegationService.UpdateDelegation(delegation).then(function () {
                actionGetDelegation();
                Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
            }).catch(function (error) {
                console.log(error);
                Notify.error($ctrl.resources.Global_Notification_Error);
            });
        }

        function DesactivateDelegation(delegation) {
            if (delegation.Commentaire === "Aucun commentaire") {
                delegation.Commentaire = "";
            }
            DelegationService.DesactivateDelegation(delegation).then(function () {
                actionGetDelegation();
                Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
            }).catch(function (error) {
                console.log(error);
                Notify.error($ctrl.resources.Global_Notification_Error);
            });
        }

        function DeleteDelegation(delegation) {
            if (delegation.Commentaire === "Aucun commentaire") {
                delegation.Commentaire = "";
            }
            DelegationService.DeleteDelegation(delegation).then(function () {
                actionGetDelegation();
                Notify.message($ctrl.resources.Global_Notification_Suppression_Success);
            }).catch(function (error) {
                console.log(error);
                Notify.error($ctrl.resources.Global_Notification_Error);
            });
        }

        function onFilterDelegation(sortBy) {
            $ctrl.DelegationList = DelegationListInit;
            if ($ctrl.currentFilter === sortBy) {
                //Dans ce cas la l'utilisateur desactive le filtre courant
                $ctrl.currentFilter = '';
            }
            else {
                $ctrl.currentFilter = sortBy;
                switch (sortBy) {
                    case 'Emise':
                        $ctrl.DelegationList = $ctrl.DelegationList.filter(delegation => delegation.Issued === true);
                        break;
                    case 'Reçue':
                        $ctrl.DelegationList = $ctrl.DelegationList.filter(delegation => delegation.Issued === false);
                        break;
                    case 'Active':
                        $ctrl.DelegationList = $ctrl.DelegationList.filter(delegation => delegation.Activated === true);
                        break;
                    case 'Inactive':
                        $ctrl.DelegationList = $ctrl.DelegationList.filter(delegation => delegation.Activated === false);
                        break;
                }
            }
        }

        function compareToDateDelegation(date) {
            var fieldDate = new Date(date);
            return fieldDate > $ctrl.today;
        }

        /* -------------------------------------------------------------------------------------------------------------
        *                                            GESTION DES CONTRATS D'INTERIMAIRES
        * -------------------------------------------------------------------------------------------------------------
        */

        function actionGetContratInterim() {
            return ContratInterimService.GetContratInterimByPersonnelId($ctrl.Personnel.PersonnelId).then(function (value) {
                $ctrl.ContratInterimList = value.data;
                ContratInterimListInit = JSON.parse(JSON.stringify($ctrl.ContratInterimList));
            }).then(ZoneDeTravailTraitment)
                .then(traitmentInterimaireAsUtilisateur)
                .catch(function (error) { console.log(error); });
        }

        function ZoneDeTravailTraitment() {
            $ctrl.ContratInterimList.forEach(ContratInterim => {
                var zoneDeTravailString = "";
                ContratInterim.ZonesDeTravail.forEach(zoneDeTravail => {
                    zoneDeTravailString += zoneDeTravail.EtablissementComptable.Code + " - " + zoneDeTravail.EtablissementComptable.Libelle + "\n";
                });
                ContratInterim.ZonesDeTravailString = zoneDeTravailString;
            });
        }

        function traitmentInterimaireAsUtilisateur() {
            if ($ctrl.ContratInterimList !== null) {
                var contrat = $ctrl.ContratInterimList.filter(contratInterim => contratInterim.Etat < 3);
                if (contrat !== null && contrat !== undefined && contrat.length > 0) {
                    contrat = contrat.sort(function (a, b) { return a.DateDebut - b.DateDebut; });
                    $ctrl.ContratInterimActifOrPlanifie = contrat[0];
                    $ctrl.showPartialUtilisateur = true;
                    $ctrl.societeForAffectation = $ctrl.ContratInterimActifOrPlanifie.Societe;

                    if (!$ctrl.Personnel.IsUtilisateur) {
                        var dateFin = new Date(contrat[contrat.length - 1].DateFin);
                        var dateExpiration = dateFin.setDate(dateFin.getDate() + contrat[contrat.length - 1].Souplesse);
                        $ctrl.persoForUser.Utilisateur.ExternalDirectory.DateExpiration = new Date(dateExpiration);
                    }
                } else {
                    $ctrl.showPartialUtilisateur = false;
                }
            }
        }
        /* 
        * @function handleOpenFormContratInterimModal(contratInterimExistant)
        * @description Ouvre la modal permettant d'ajouter une nouveau contrat intérimaire
        * @param {object} Object ContratInterimaire
        */
        function handleOpenFormContratInterimModal(contratInterimExistant, type) {
            var readOnly = false;

            var contratInterim = {
                ContratInterimaireId: 0,
                InterimaireId: $ctrl.Personnel.PersonnelId,
                Interimaire: $ctrl.Personnel,
                FournisseurId: null,
                SocieteId: null,
                NumContrat: "",
                DateDebut: null,
                DateFin: null,
                Energie: false,
                CiId: null,
                RessourceId: null,
                Source: "Manuel",
                Qualification: "",
                Statut: "",
                UniteId: 4,
                Unite: { Libelle: "Heure" },
                TarifUnitaire: null,
                Souplesse: 0,
                MotifRemplacementId: null,
                PersonnelRemplaceId: null,
                Commentaire: null,
                Valorisation: null
            };

            if (type === "Update") {
                if (contratInterimExistant !== undefined && contratInterimExistant !== null) {
                    contratInterim = angular.copy(contratInterimExistant);
                    if (contratInterim.MotifRemplacementId) {
                        contratInterim.MotifRemplacementId = contratInterim.MotifRemplacementId.toString();
                    }
                    if (contratInterimExistant.Etat === 3) {
                        readOnly = true;
                    }
                }
            }

            if (type === "Duplicate") {
                if (contratInterimExistant !== undefined && contratInterimExistant !== null) {
                    contratInterim = angular.copy(contratInterimExistant);
                    contratInterim.ContratInterimaireId = 0;
                    if (contratInterim.MotifRemplacementId) {
                        contratInterim.MotifRemplacementId = contratInterim.MotifRemplacementId.toString();
                    }
                    contratInterim.Source = "Manuel";
                    contratInterim.NumContrat = "";
                    contratInterim.DateDebut = null;
                    contratInterim.DateFin = null;
                }
            }

            var modalInstance = $uibModal.open({
                animation: true,
                component: 'formulaireContratInterimComponent',
                windowClass: 'app-modal-window',
                resolve: {
                    contratInterim: function () { return contratInterim; },
                    resources: function () { return $ctrl.resources; },
                    readOnly: function () { return readOnly; }
                }
            });

            modalInstance.result.then(function (contratInterim) {
                actionGetContratInterim();
                if (contratInterim !== undefined && contratInterim !== null) {
                    if (contratInterim.ContratInterimaireId > 0) {
                        UpdateContratInterim(contratInterim);
                    } else {
                        AddContratInterim(contratInterim);
                    }
                }
            });
        }

        function handleOpenDeleteContratInterimModal(contratInterim) {
            var modalInstance = $uibModal.open({
                animation: true,
                component: 'deleteContratInterimComponent',
                resolve: {
                    contratInterim: function () { return contratInterim; },
                    resources: function () { return $ctrl.resources; }
                }
            });

            // Traitement des données renvoyées par la modal
            modalInstance.result.then(function () {
                DeleteContratInterim(contratInterim);
            });
        }

        function AddContratInterim(contratInterim) {
            ContratInterimService.AddContratInterim(contratInterim)
                .then(function () {
                    actionGetPersonnel();
                    Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
                })
                .catch(HandleDisplayError);
        }

        function UpdateContratInterim(contratInterim) {
            ContratInterimService.UpdateContratInterim(contratInterim)
                .then(function () {
                    actionGetPersonnel();
                    Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
                })
                .catch(HandleDisplayError);
        }

        function DeleteContratInterim(contratInterim) {
            ContratInterimService.DeleteContratInterim(contratInterim.ContratInterimaireId).then(function () {
                actionGetContratInterim();
                Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
            }).catch(function (error) {
                console.log(error);
                Notify.error($ctrl.resources.Global_Notification_Error);
            });
        }

        function onSearchContratInterim() {
            $ctrl.ContratInterimList = ContratInterimListInit;
            if ($ctrl.searchContrat !== "" && $ctrl.searchContrat !== null) {
                var regex = new RegExp($ctrl.searchContrat, "g");
                var list;
                list = ContratInterimListInit.filter(contratInterim => contratInterim.NumContrat.search(regex) >= 0);
                list = list.concat(ContratInterimListInit.filter(contratInterim => contratInterim.Fournisseur.Libelle.toLowerCase().search(regex) >= 0));
                list = list.concat(ContratInterimListInit.filter(contratInterim => contratInterim.Fournisseur.Code.toLowerCase().search(regex) >= 0));
                $ctrl.ContratInterimList = Array.from(new Set(list));
            }
        }

        function onFilterContratInterim(sortBy) {
            $ctrl.ContratInterimList = ContratInterimListInit;
            if ($ctrl.currentFilter === sortBy) {
                //Dans ce cas la l'utilisateur desactive le filtre courant
                $ctrl.currentFilter = '';
            }
            else {
                $ctrl.currentFilter = sortBy;
                switch (sortBy) {
                    case 'Planifie':
                        $ctrl.ContratInterimList = $ctrl.ContratInterimList.filter(contratInterim => contratInterim.Etat === 1);
                        break;
                    case 'Actif':
                        $ctrl.ContratInterimList = $ctrl.ContratInterimList.filter(contratInterim => contratInterim.Etat === 2);
                        break;
                    case 'Cloture':
                        $ctrl.ContratInterimList = $ctrl.ContratInterimList.filter(contratInterim => contratInterim.Etat === 3);
                        break;
                }
            }
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            INFORMATIONS UTILISATEUR
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @function upgradePersonnelToUser
         * @description Ajoute un personnel en tant qu'utilisateur
         */
        function upgradePersonnelToUser() {
            return PersonnelService.AddPersonnelAsUtilisateur($ctrl.persoForUser).$promise.then(function (value) {
                $ctrl.persoForUser.UtilisateurId = value.UtilisateurId;
                $ctrl.persoForUser.Utilisateur = value.Utilisateur;
                persisteState($ctrl.persoForUser, "utilisateur");
                actionGetPersonnel();
                Notify.message(resources.Global_Notification_Enregistrement_Success);
            }).catch(function (reason) { Notify.error(resources.Global_Notification_Error); });
        }




        /*
         * @function updateUtilisateur
         * @description Mise à jour de l'utilisateur du personnel     
         */
        function updateUtilisateur() {
            return UtilisateurService.Update($ctrl.persoForUser.Utilisateur).$promise
                .then(function () {
                    persisteState($ctrl.persoForUser, "utilisateur");
                    if ($ctrl.persoForUser.hasSubscribeToEmailSummary) {
                        return PersonnelService.activeEmailSummary($ctrl.persoForUser.PersonnelId);
                    } else {
                        return PersonnelService.disableEmailSummary($ctrl.persoForUser.PersonnelId);
                    }

                }).then(function (value) {
                    Notify.message(resources.Global_Notification_Enregistrement_Success);

                })
                .catch(function (error) {
                    Notify.error(resources.Global_Notification_Error);
                });
        }

        /*
         * @function actionSaveUtilisateur
         * @description Mise à jour de l'utilisateur du personnel     
         * @param {string} mode : soit une création, soit une mise à jour
         */
        function actionSaveUtilisateur() {
            if ($ctrl.persoForUser.hasSubscribeToEmailSummary === true && $ctrl.formContact.Email.$invalid) {
                Notify.error(resources.Personnel_PartialUtilisateur_Erreur_Email_quotidien);
                return;
            }
            // Set validity to true if input are empty
            if (!$ctrl.persoForUser.Utilisateur.MotDePasse && $ctrl.formGestionUtilisateur.MotDePasse) {
                $ctrl.formGestionUtilisateur.MotDePasse.$setValidity("validPasswordLength", true);
                $ctrl.formGestionUtilisateur.MotDePasse.$setValidity("validPasswordLetter", true);
            }
            if ((!$ctrl.persoForUser.Utilisateur.IsActived || !$ctrl.persoForUser.Utilisateur.Login) && !$ctrl.persoForUser.Utilisateur.UtilisateurId > 0) {

                if ($ctrl.Personnel.IsInterne) {
                    $ctrl.formGestionUtilisateur.Login.$setValidity("validLogin", true);
                }
                else {
                    $ctrl.formGestionUtilisateur.LoginExterne.$setValidity("validLogin", true);
                }
            }
            if ($ctrl.Personnel.IsInterne) {
                if (!$ctrl.persoForUser.Utilisateur.Folio) {
                    $ctrl.formGestionUtilisateur.Folio.$setValidity("validFolio", true);
                    $ctrl.formGestionUtilisateur.Folio.$setValidity("validFolioExist", true);
                }
            }
            else {
                if (!$ctrl.persoForUser.Utilisateur.FolioExterne) {
                    $ctrl.formGestionUtilisateur.FolioExterne.$setValidity("validFolio", true);
                    $ctrl.formGestionUtilisateur.FolioExterne.$setValidity("validFolioExist", true);
                }
            }

            if (!$ctrl.formGestionUtilisateur.$invalid) {
                return $ctrl.persoForUser.UtilisateurId > 0 ? updateUtilisateur() : upgradePersonnelToUser();
            }
        }

        // Annulation des modifications de la section Informations Utilisateurs
        function actionCancelUtilisateur() {
            restoreState("utilisateur");
            $ctrl.formGestionUtilisateur.$setPristine();
            if ($ctrl.Personnel.IsInterne) {
                $ctrl.formGestionUtilisateur.Login.$setValidity("validLogin", true);
            }
            else {
                $ctrl.formGestionUtilisateur.LoginExterne.$setValidity("validLogin", true);
            }
            $ctrl.formGestionUtilisateur.MotDePasse.$setValidity("validPasswordLength", true);
            $ctrl.formGestionUtilisateur.MotDePasse.$setValidity("validPasswordLetter", true);
        }

        /**
         * Change des valeurs dépendant de l'activation de l'utilisateur
         */
        function handleChangeUser() {
            if (!$ctrl.persoForUser.Utilisateur.IsActived) {
                $ctrl.persoForUser.Utilisateur.ExternalDirectory.IsActived = false;
                $ctrl.persoForUser.Utilisateur.SuperAdmin = false;
            }
        }

        function actionGetHasSubscribeToEmailSummary() {
            return PersonnelService.hasSubscribeToEmailSummary($ctrl.personnelId)
                .then(function (response) {
                    $ctrl.persoForUser.hasSubscribeToEmailSummary = response.data.HasSusbcribeToMaillingList;
                });
        }

        function handleCanChangeActiveEmailSummary() {
            if (emailOk()) {
                return true;
            } else {
                if ($ctrl.persoForUser.hasSubscribeToEmailSummary === true) {
                    return true;
                }
                return false;
            }
        }

        function emailOk() {
            return $ctrl.persoForContact.Email !== null &&
                $ctrl.persoForContact.Email !== undefined &&
                $ctrl.persoForContact.Email.trim() !== '';
        }

        function handleGetSubscribeToEmailSummaryLabel() {
            if ($ctrl.persoForContact.Email === null ||
                $ctrl.persoForContact.Email === undefined ||
                $ctrl.persoForContact.Email.trim() === '') {
                return resources.Personnel_ControllerEdit_Label_Subscribe_To_Email_Summary_Empty_info;
            } else {
                return resources.Personnel_ControllerEdit_Label_Subscribe_To_Email_Summary_Info + ' ' + $ctrl.persoForContact.Email;
            }
        }

        /*     
         * @function handleChangeLogin(utilisateur)
         * @description Vérifie si le login est déjà utilisé
         */
        function handleChangeLogin() {
            if ($ctrl.Personnel.IsInterne) {
                $ctrl.formGestionUtilisateur.Login.$setValidity("validLogin", true);
            }
            else {
                $ctrl.formGestionUtilisateur.LoginExterne.$setValidity("validLogin", true);
            }

            if ($ctrl.persoForUser.Utilisateur.Login) {
                UtilisateurService.IsLoginExist($ctrl.persoForUser.Utilisateur).$promise
                    .then(function (response) {
                        if ($ctrl.Personnel.IsInterne) {
                            $ctrl.formGestionUtilisateur.Login.$setValidity("validLogin", !response.value);
                        }
                        else {
                            $ctrl.formGestionUtilisateur.LoginExterne.$setValidity("validLogin", !response.value);
                        }
                    })
                    .catch(function (error) { Notify.error(resources.Personnel_ControllerEdit_Notification_VerificationLogin_Erreur); });
            }
            else {
                if ($ctrl.Personnel.IsInterne) {
                    $ctrl.formGestionUtilisateur.Login.$setValidity("validLogin", false);
                }
                else {
                    $ctrl.formGestionUtilisateur.LoginExterne.$setValidity("validLogin", false);
                }
            }
        }

        /*     
         * @function handleChangeLoginInterne(userLoginActiveDirectory)
         * @description Event change Login pour recherche la correspondance avec l'Active Directory           
         * TODO : A finir ??
         */
        function handleChangeLoginInterne() {
            var user = $ctrl.persoForUser.Utilisateur;

            UtilisateurService.IsLoginADExist(user).$promise
                .then(function () { })
                .catch(function (error) { console.log(error); Notify.error(resources.Global_Notification_Error); });
        }

        /*    
         * @function handleChangeFolio(user)
         * @description Vérifie si le Folio/Trigramme n'est pas déjà utilisé
         * 
         * TODO : VOIR SI LE FOLIO PEUT PRENDRE 4 CARACTERES
         *        SI UN FOLIO EST DEJA UTILISE, PROPOSE UN INCREMENTATION PAR PAS DE 1 DU TRIGRAMME
         *        EX : BPE, BPE1, BPE2     
         */
        function handleChangeFolio() {
            var user = $ctrl.persoForUser.Utilisateur;

            if ($ctrl.Personnel.IsInterne) {
                $ctrl.formGestionUtilisateur.Folio.$setValidity("validFolio", true);
                $ctrl.formGestionUtilisateur.Folio.$setValidity("validFolioExist", true);
            }
            else {
                $ctrl.formGestionUtilisateur.FolioExterne.$setValidity("validFolio", true);
                $ctrl.formGestionUtilisateur.FolioExterne.$setValidity("validFolioExist", true);
            }

            if (user.IsActived && user.Folio) {
                if (user.Folio.length === 3) {
                    var currentId = user.UtilisateurId ? user.UtilisateurId : 0;
                    var params = { idCourant: currentId, folio: user.Folio, societeId: $ctrl.Personnel.SocieteId };

                    UtilisateurService.IsFolioExist(params).$promise
                        .then(function (response) {
                            if ($ctrl.Personnel.IsInterne) {
                                $ctrl.formGestionUtilisateur.Folio.$setValidity("validFolioExist", !response.value);
                            }
                            else {
                                $ctrl.formGestionUtilisateur.FolioExterne.$setValidity("validFolioExist", !response.value);
                            }
                        })
                        .catch(function (reason) { Notify.error($ctrl.resources.Personnel_ControllerEdit_Notification_VerificationFolio_Erreur); });
                }
                else {
                    if ($ctrl.Personnel.IsInterne) {
                        $ctrl.formGestionUtilisateur.Folio.$setValidity("validFolio", false);
                    }
                    else {
                        $ctrl.formGestionUtilisateur.FolioExterne.$setValidity("validFolio", false);
                    }
                }
            }
        }

        /* Event Sélection Login Active Directory */
        function handleSelectLoginFromActiveDirectory(item) {
            $ctrl.Personnel.Utilisateur.Login = item.SAMAccountName;
        }

        /* Event Change Mot de passe gestion de la compléxité du mot de passe*/
        /** RG : 
          *    - 6 caractères minimum 
          *    - au moins une lettre
          *    - au moins un chiffre
          */
        function handleChangePassword() {
            $ctrl.formGestionUtilisateur.MotDePasse.$setValidity("validPasswordLength", true);
            $ctrl.formGestionUtilisateur.MotDePasse.$setValidity("validPasswordLetter", true);

            if ($ctrl.persoForUser.Utilisateur.IsActived) {
                var user = $ctrl.persoForUser.Utilisateur;

                if (user.ExternalDirectory && user.ExternalDirectory.MotDePasse) {
                    var password = user.ExternalDirectory.MotDePasse;
                    if (password) {
                        if (password.length < 6) {
                            $ctrl.formGestionUtilisateur.MotDePasse.$setValidity("validPasswordLength", false);
                        }

                        if (password.search(/[a-z]/i) < 0) {
                            $ctrl.formGestionUtilisateur.MotDePasse.$setValidity("validPasswordLetter", false);
                        }
                    }
                }
            }
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            HABILITATIONS
         * -------------------------------------------------------------------------------------------------------------
         */

        /* 
         * @description Ajout d'un Rôle
         */
        function actionAddRole(role) {
            var doublon = $filter('filter')($ctrl.roleList, { RoleId: role.RoleId }, true)[0];
            if (doublon) {
                Notify.error(resources.Personnel_ControllerEdit_Notification_RoleDejaAffecter_Erreur);
            }
            else {
                $ctrl.roleList.push(role);
            }
        }

        /* 
         * @description Ajout d'une association Rôle/Organisation
         */
        function actionAddOrganisation(organisation) {

            var affectationOrganisation = { Organisation: organisation, Affectations: [] };
            var orgaList = $ctrl.roleList[$ctrl.indexRole].AffectationsByOrganisation;

            if (!orgaList) {
                $ctrl.roleList[$ctrl.indexRole].AffectationsByOrganisation = [];
                orgaList.push(affectationOrganisation);
            }
            else {

                var doublon = $filter('filter')(orgaList, { Organisation: { OrganisationId: organisation.OrganisationId } }, true)[0];
                if (doublon) {
                    Notify.error(resources.Personnel_ControllerEdit_Notification_OrganisationDejaAffecter_Erreur);
                }
                else {
                    orgaList.push(affectationOrganisation);
                }
            }
        }

        /* 
         * @description Ajout d'une association Organisation/Devise
         */
        function actionAddDevise(devise) {
            var newAffectation = { DeviseId: devise.DeviseId, Devise: devise, CommandeSeuil: 0 };
            var affectationList = $ctrl.roleList[$ctrl.indexRole].AffectationsByOrganisation[$ctrl.indexOrganisation].Affectations;

            if (!affectationList) {
                $ctrl.roleList[$ctrl.indexRole].AffectationsByOrganisation[$ctrl.indexOrganisation].Affectations = [];
                affectationList.push(newAffectation);
                CreateSeuilValidationTooltip();
            }
            else {
                var doublon = $filter('filter')(affectationList, { Devise: { DeviseId: devise.DeviseId } }, true)[0];
                if (doublon) {
                    Notify.error(resources.Personnel_ControllerEdit_Notification_DeviseDejaAffecter_Erreur);
                }
                else {
                    affectationList.push(newAffectation);
                    CreateSeuilValidationTooltip();
                }
            }
        }

        /*
         * @description Gestion de la suppression d'un Rôle
         */
        function handleDeleteRole(indexRole) {
            $ctrl.roleList.splice(indexRole, 1);
        }

        /*
         * @description Gestion de la suppression d'une Organisation
         */
        function handleDeleteOrganisation(indexRole, indexOrganisation) {
            $ctrl.roleList[indexRole].AffectationsByOrganisation.splice(indexOrganisation, 1);
        }

        /*
         * @description Gestion de la suppression d'une Devise
         */
        function handleDeleteDevise(indexRole, indexOrganisation, indexAffectation) {
            $ctrl.roleList[indexRole].AffectationsByOrganisation[indexOrganisation].Affectations.splice(indexAffectation, 1);
        }

        /*
         * @description appelle l'ouverture du référentiel Organisation
         */
        function handleOpenReferentialOrganisation() {
            if ($ctrl.Personnel.IsInterimaire) {
                return "/api/ContratInterimaire/SearchLight/" + $ctrl.ContratInterimActifOrPlanifie.ContratInterimaireId;
            }

            if (!$ctrl.societeForAffectation) {
                return "/api/Organisation/SearchLightForSocieteId?societeId=null&typeOrganisation=SOCIETE,PUO,UO,ETABLISSEMENT,CI,SCI&";
            }
            var societeId = $ctrl.societeForAffectation.SocieteId;
            return "/api/Organisation/SearchLightForSocieteId?societeId=" + societeId + "&typeOrganisation=SOCIETE,PUO,UO,ETABLISSEMENT,CI,SCI&";
        }

        /*
         * @description appelle l'ouverture du référentiel Devise
         */
        function handleOpenReferentialDevise(apiController, indexRole, indexOrganisation) {
            $ctrl.indexRole = indexRole;
            $ctrl.indexOrganisation = indexOrganisation;
            var organisationId = $ctrl.roleList[indexRole].AffectationsByOrganisation[indexOrganisation].Organisation.OrganisationId;
            return showPickList(apiController, null, organisationId);
        }

        /*
         * @description Gestion de la sauvegarde des habilitations d'un utilisateur 
         */
        function actionSaveHabilitation() {
            if ($ctrl.formHabilitation.$valid) {
                var ListAffectationsGroupByRole = $ctrl.roleList;
                var ListAffectations = [];
                var item = {};
                for (var h = 0; h < ListAffectationsGroupByRole.length; h++) {
                    for (var i = 0; i < ListAffectationsGroupByRole[h].AffectationsByOrganisation.length; i++) {
                        item = ListAffectationsGroupByRole[h].AffectationsByOrganisation[i];
                        if (item.Affectations.length > 0) {
                            for (var j = 0; j < ListAffectationsGroupByRole[h].AffectationsByOrganisation[i].Affectations.length; j++) {
                                var affectation = {
                                    UtilisateurId: $ctrl.Personnel.UtilisateurId,
                                    RoleId: ListAffectationsGroupByRole[h].RoleId,
                                    OrganisationId: item.Organisation.OrganisationId,
                                    DeviseId: item.Affectations[j].DeviseId,
                                    CommandeSeuil: item.Affectations[j].CommandeSeuil
                                };
                                ListAffectations.push(affectation);
                            }
                        }
                        else {
                            var affectation1 = {
                                UtilisateurId: $ctrl.Personnel.UtilisateurId,
                                RoleId:
                                    ListAffectationsGroupByRole[h].RoleId,
                                OrganisationId: item.Organisation.OrganisationId
                            };
                            ListAffectations.push(affectation1);
                        }
                    }
                }
                var params = { utilisateurId: $ctrl.Personnel.UtilisateurId };
                return UtilisateurService.UpdateRole(params, ListAffectations).$promise
                    .then(function (response) {
                        persisteState(null, "habilitation");
                        Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
                    })
                    .catch(function (error) {
                        Notify.error($ctrl.resources.Global_Notification_Error);
                    });
            }
        }

        /*
         * @description Gestion de l'annulation des modifications des habilitations
         */
        function actionCancelHabilitation() {
            restoreState("habilitation");
        }

        // Récupération de la liste des Habilitations
        function actionGetAffectationRole() {
            return UtilisateurService.GetAffectationRole({ utilisateurId: $ctrl.Personnel.UtilisateurId }).$promise
                .then(function (response) {

                    $ctrl.roleList = response;

                    CreateSeuilValidationTooltip();

                    snapshotState("habilitation");
                })
                .catch(Notify.defaultError);
        }

        /*
         * @description Création du tooltip indiquant les seuils de validation au niveau du Rôle et de l'Organisation
         */
        function CreateSeuilValidationTooltip() {

            angular.forEach($ctrl.roleList, function (role, key1) {
                angular.forEach(role.AffectationsByOrganisation, function (orga, key2) {
                    angular.forEach(orga.Affectations, function (affectation, key3) {
                        var roleSeuils = $filter('filter')(role.SeuilsValidation, { RoleId: affectation.RoleId, DeviseId: affectation.DeviseId }, true);
                        var orgaSeuils = $filter('filter')(orga.Organisation.AffectationsSeuilRoleOrga, { OrganisationId: affectation.OrganisationId, RoleId: affectation.RoleId, DeviseId: affectation.DeviseId }, true);
                        affectation.SeuilNiveauRole = roleSeuils && roleSeuils.length > 0 ? roleSeuils[0].Montant : null;
                        affectation.SeuilNiveauOrga = orgaSeuils && orgaSeuils.length > 0 ? orgaSeuils[0].Seuil : null;
                        affectation.SeuilRoleAndOrgaTooltip = "Seuils de validation :";
                        affectation.SeuilRoleAndOrgaTooltip += "\n- Niveau rôle : " + (affectation.SeuilNiveauRole ? affectation.SeuilNiveauRole + " " + affectation.Devise.Symbole : "Indéfini");
                        affectation.SeuilRoleAndOrgaTooltip += "\n- Niveau organisation : " + (affectation.SeuilNiveauOrga ? affectation.SeuilNiveauOrga + " " + affectation.Devise.Symbole : "Indéfini");
                    });
                });
            });

        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            SIGNATURE & PHOTO DE PROFIL
         * -------------------------------------------------------------------------------------------------------------
         */

        /**
         *  Gestion de la sélection d'une image
         * @param {any} event evenement
         * @param {any} inputFileType identifiant profil ou signature
         * @param {any} element element courant
         * @param {any} divToHide div à cacher après sélection de l'image
         * @param {any} buttonsDiv boutons à afficher
         */
        function handleSelectImage(event, inputFileType, element, divToHide, buttonsDiv) {
            var inputFile = angular.element(element);
            // Set filename to input text
            inputFile.parents('.input-group').find(':text').val(inputFile.val().replace(/\\/g, '/').replace(/.*\//, ''));

            angular.element(document.querySelector(divToHide)).hide();
            angular.element(document.querySelector(buttonsDiv)).show();
            actionReadFile(event.target, inputFileType);
        }

        /**
         *  Gestion rotation de l'image
         * @param {any} id identifiant profil ou signature
         */
        function handleRotation(id) {
            if (id === $ctrl.inputFileType.signature.id) {
                $ctrl.signatureCropper.rotate(45);
            }
            else if (id === $ctrl.inputFileType.photoProfil.id) {
                $ctrl.photoProfilCropper.rotate(45);
            }
        }

        /**
         *  Validation de la sélection de l'image
         * @param {any} id identifiant profil ou signature
         */
        function handleValidateImage(id) {
            var imgSrc = "";
            if (id === $ctrl.inputFileType.signature.id) {
                imgSrc = $ctrl.signatureCropper.getCroppedCanvas($ctrl.signatureCroppedOpts).toDataURL();
                $ctrl.signature = imgSrc;
                $ctrl.PersonnelImage.SignatureBase64 = actionGetBase64(imgSrc);
            }
            else if (id === $ctrl.inputFileType.photoProfil.id) {
                var canvas = $ctrl.photoProfilCropper.getCroppedCanvas($ctrl.photoProfilCroppedOpts);
                var roundedCanvas = actionGetRoundedCanvas(canvas);
                imgSrc = roundedCanvas.toDataURL();

                $ctrl.photoProfil = imgSrc;
                $ctrl.PersonnelImage.PhotoProfilBase64 = actionGetBase64(imgSrc);
            }

            $timeout(angular.noop);
        }

        function handleReset(id) {
            if (id === $ctrl.inputFileType.signature.id && $ctrl.signatureCropper) {
                $ctrl.signatureCropper.reset();
            }
            else if (id === $ctrl.inputFileType.photoProfil.id && $ctrl.photoProfilCropper) {
                $ctrl.photoProfilCropper.reset();
            }
        }

        function handleDeleteImage(id) {
            if (id === $ctrl.inputFileType.signature.id && $ctrl.signature) {
                $ctrl.signature = "";
                $ctrl.PersonnelImage.SignatureBase64 = "";
            }
            else if (id === $ctrl.inputFileType.photoProfil.id && $ctrl.photoProfil) {
                $ctrl.photoProfil = "";
                $ctrl.PersonnelImage.PhotoProfilBase64 = "";
            }
        }

        /**
         * Récupération du canvas arrondi
         * @param {any} sourceCanvas canvas carré
         * @returns {any} canvas
         */
        function actionGetRoundedCanvas(sourceCanvas) {
            var canvas = document.createElement('canvas');
            var context = canvas.getContext('2d');
            var width = sourceCanvas.width;
            var height = sourceCanvas.height;
            canvas.width = width;
            canvas.height = height;
            context.imageSmoothingEnabled = true;
            context.drawImage(sourceCanvas, 0, 0, width, height);
            context.globalCompositeOperation = 'destination-in';
            context.beginPath();
            context.arc(width / 2, height / 2, Math.min(width, height) / 2, 0, 2 * Math.PI, true);
            context.fill();
            return canvas;
        }

        function actionSaveSignatureProfil() {
            return actionAddOrUpdateSignatureProfil();
        }

        function actionCancelSignatureProfil() {
            restoreState("signatureProfil");

            $ctrl.signature = $ctrl.imgSrcBase + $ctrl.PersonnelImage.SignatureBase64;
            $ctrl.photoProfil = $ctrl.imgSrcBase + $ctrl.PersonnelImage.PhotoProfilBase64;
            $timeout(angular.noop);
        }

        /**
         * Action enregistrement des images du personnel
         * @returns {any} Résultat de l'opération
         */
        function actionAddOrUpdateSignatureProfil() {
            return PersonnelService.AddOrUpdatePersonnelImage($ctrl.PersonnelImage).$promise
                .then(function (data) {
                    $ctrl.PersonnelImage = data;
                    persisteState($ctrl.PersonnelImage, "signatureProfil");
                    UserService.setPhotoProfil($ctrl.personnelId, $ctrl.PersonnelImage.PhotoProfilBase64);
                    Notify.message(resources.Global_Notification_Enregistrement_Success);
                })
                .catch(HandleDisplayError);
        }

        /*
         * Récupération des images du personnel
         */
        function actionGetPersonnelImage() {
            $ctrl.PersonnelImage = { PersonnelImageId: 0, PersonnelId: $ctrl.personnelId };

            if ($ctrl.personnelId) {
                return PersonnelService.GetPersonnelImage({ personnelId: $ctrl.personnelId }).$promise
                    .then(function (data) {
                        $ctrl.PersonnelImage = data;
                        $ctrl.photoProfil = $ctrl.imgSrcBase + $ctrl.PersonnelImage.PhotoProfilBase64;
                        $ctrl.signature = $ctrl.imgSrcBase + $ctrl.PersonnelImage.SignatureBase64;
                    })
                    .catch(Notify.defaultError);
            }
        }

        /**
         *  Action lecture de l'image
         * @param {any} input element input selection du fichier image
         * @param {any} inputFileType identifiant de la div contenant l'imade à traiter
         */
        function actionReadFile(input, inputFileType) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    actionOnloadImage(inputFileType, e);
                };

                reader.readAsDataURL(input.files[0]);
            }
            else {
                console.error("Impossible de charger l'image, votre navigateur ne supporte pas l'API FileReader. Mettez à jour votre navigateur.");
            }
        }

        /**
         * Action chargement de l'image sélectionnée
         * @param {any} inputFileType identifiant de la div contenant l'imade à traiter
         * @param {any} e event
         */
        function actionOnloadImage(inputFileType, e) {
            angular.element(document.querySelector(inputFileType.container)).show();

            var image = angular.element(document.querySelector(inputFileType.id))[0];
            image.src = e.target.result;

            if (inputFileType.id === $ctrl.inputFileType.signature.id) {
                if ($ctrl.signatureCropper) {
                    $ctrl.signatureCropper.replace(e.target.result);
                }
                else {
                    $ctrl.signatureCropper = new Cropper(image, { dragMode: 'move' });
                }
            }
            else if (inputFileType.id === $ctrl.inputFileType.photoProfil.id) {
                if ($ctrl.photoProfilCropper) {
                    $ctrl.photoProfilCropper.replace(e.target.result);
                }
                else {
                    $ctrl.photoProfilCropper = new Cropper(image, { aspectRatio: 1, dragMode: 'move' });
                }
            }
        }

        /* -------------------------------------------------------------------------------------------------------------
          *                                            DIVERS
          * -------------------------------------------------------------------------------------------------------------
          */

        /**
          * Convertit une dataURI en tableau d'octets
          * @param {any} dataURI data URI au format base64
          * @returns {string} retourne l'URI en base 64
          */
        function actionGetBase64(dataURI) {
            var BASE64_MARKER = ';base64,';
            var base64Index = dataURI.indexOf(BASE64_MARKER) + BASE64_MARKER.length;
            var base64 = dataURI.substring(base64Index);

            return base64;
        }

        /* @function initGoogleMap
          * @description Initialise la carte Google Map     
          */
        function initGoogleMap() {
            NgMap.getMap("map").then(function (map) {
                $ctrl.map = map;
                $ctrl.map.setOptions({ disableDoubleClickZoom: true });
                PersonnelEditCartoService.init($ctrl.carto, $ctrl.map);
                GoogleService.addSelectPositionButton($ctrl.resources, $ctrl.map, $ctrl.mapSelector, $ctrl.handleSelectLocation);
                initMapWithContactInfo();
                google.maps.event.addListener($ctrl.map, 'dblclick', function (e) {
                    $ctrl.map.markers.mapSelectorMap.setMap($ctrl.map);
                    var lat = e.latLng.lat();
                    var lng = e.latLng.lng();
                    $ctrl.mapSelector = {
                        Latitude: lat.toFixed(7),
                        Longitude: lng.toFixed(7)
                    };
                    GoogleService.displaySelectPositionButton($ctrl.mapSelector || $ctrl.map.markers.mapSelectorMap.map);
                    $timeout(angular.noop);
                });
            }).catch(function (error) {
                console.log(error);
            });
        }

        /*
          * @function loadPage(id, typePersonnel)
          * @description Récupère au chargement de la page avec l'id du personnel et du type de personnel (externe ou intérimaire)
          * @param {int} id : identifiant du personnel. 0 si c'est une création
          * @param {string} typePersonnel: soit "externe" soit "interimaire"
          */
        function loadPage(id, typePersonnel) {
            $ctrl.personnelId = parseInt(id, 10);
            $ctrl.typePersonnel = typePersonnel.trim();

            $q.when()
                .then(actionGetPersonnel)
                .then(actionGetPersonnelImage)
                .then(actionGetTypeRattachementList)
                .then(function () { snapshotState('signatureProfil'); });
        }

        function actionGetTypeRattachementList() {
            return PersonnelService.GetTypesRattachement().$promise
                .then(function (value) {
                    $ctrl.typeRattachementList = value;
                })
                .catch(Notify.defaultError);
        }

        function actionNewUtilisateur() {
            return PersonnelService.NewUtilisateur().$promise
                .then(function (user) {
                    actionToLocaleDate(user);
                    $ctrl.Personnel.Utilisateur = user;
                    $ctrl.persoForUser = angular.copy($ctrl.Personnel);
                })
                .catch(Notify.defaultError);
        }

        /*
         * @description Conversion des dates UTC vers Local
         */
        function actionToLocaleDate(value) {
            if (value) {
                value.DateCreation = value.DateCreation ? $filter('toLocaleDate')(value.DateCreation) : null;
                value.DateModification = value.DateModification ? $filter('toLocaleDate')(value.DateModification) : null;
                value.DateSuppression = value.DateSuppression ? $filter('toLocaleDate')(value.DateSuppression) : null;
            }
        }

        /*
          * @description Gestion de l'extraction des données du personnel
          */
        function actionProcessPersonnelData(personnel) {
            actionToLocaleDate(personnel);
            PersonnelService.GetDefaultCi({ personnelId: $ctrl.personnelId }).$promise
                .then(function (data) {
                    $ctrl.defaultCi = data;
                })
                .catch(function (reason) { Notify.error($ctrl.resources.Global_Notification_Chargement_Error); });
            $ctrl.Personnel = personnel;
            $ctrl.persoForUser = angular.copy($ctrl.Personnel);
            $ctrl.persoForContact = angular.copy($ctrl.Personnel);
        }

        /*
          * @description Action nouveau personnel(externe, interimaire)
          */
        function actionNewPersonnel() {
            var promise = PersonnelService.New().$promise
                .then(actionProcessPersonnelData)
                .catch(function (reason) { Notify.error($ctrl.resources.Global_Notification_Chargement_Error); })
                .then(actionNewUtilisateur)
                .then(initGoogleMap);

            switch ($ctrl.typePersonnel) {
                case "interimaire": {
                    promise = promise
                        .then(function () {
                            $ctrl.Personnel.IsInterne = false;
                            $ctrl.Personnel.IsInterimaire = true;
                        })
                        .then(actionGetDefaultSocieteInterim);
                    break;
                }
                default: {
                    promise = promise
                        .then(function () {
                            $ctrl.Personnel.IsInterne = false;
                            $ctrl.Personnel.IsInterimaire = false;
                        });
                    break;
                }
            }

            return promise.then(function () { snapshotState("general"); snapshotState("contact"); snapshotState("utilisateur"); ProgressBar.complete(); });
        }

        /*
          *  @function actionGetPersonnel(personnelId)
          *  @description Récupération des données spécifiques au personnel     
          */
        function actionGetPersonnel() {

            // Création d'un personnel
            if ($ctrl.personnelId === 0 || $ctrl.personnelId === "") {
                actionNewPersonnel();
            }
            // Détail d'un personnel
            else {
                PersonnelService.GetById({ personnelId: $ctrl.personnelId }).$promise
                    .then(actionProcessPersonnelData)
                    .catch(function (reason) { Notify.error($ctrl.resources.Global_Notification_Chargement_Error); })
                    .then(function () {

                        if ($ctrl.Personnel.IsInterimaire) {
                            $ctrl.showPartialUtilisateur = false;
                            actionGetContratInterim();
                        }

                        if ($ctrl.Personnel.Utilisateur === null) {
                            actionNewUtilisateur();
                        }
                        else {
                            actionGetDelegation();
                            actionGetAffectationRole();
                        }

                        if ($ctrl.Personnel.Societe.Groupe !== null && $ctrl.Personnel.Societe.Groupe.Code === 'GFTP' && $ctrl.Personnel.PersonnelId > 0) {
                            MatriculeExterneService.GetMatriculeExterneByPersonnelId($ctrl.Personnel.PersonnelId).then(function (value) {
                                if (value.data[0]) {
                                    $ctrl.MatriculeExterne = value.data[0];
                                    $ctrl.MatriculeSAP = $ctrl.MatriculeExterne.Matricule;
                                    RapportService.GetPointageVerrouillerByPersonnelId($ctrl.Personnel.PersonnelId).then(function (response) {
                                        if (response.data.length > 0) {
                                            $ctrl.hasPointageVerouiller = true;
                                        } else {
                                            $ctrl.hasPointageVerouiller = false;
                                        }
                                    });
                                }
                            });
                        }

                        snapshotState("general");
                        snapshotState("contact");
                        snapshotState("utilisateur");
                    })
                    .then(initGoogleMap)
                    .then(actionGetHasSubscribeToEmailSummary)
                    .then(ProgressBar.complete());
            }
        }

        //Fonction de chargement des donées de l'item sélectionné dans la picklist
        function loadData(type, item, indexRole) {
            var selectedRef;
            if (item === null)
                selectedRef = $ctrl.selectedRef;
            else
                selectedRef = item;

            switch (type) {
                case "EtablissementPaie":
                    PersonnelEditManageLookup.EtablissementPaieChange(selectedRef, $ctrl.Personnel, $ctrl.typeRattachementList);
                    break;
                case "EtablissementRattachement":
                    PersonnelEditManageLookup.EtablissementRattachementChange(selectedRef, $ctrl.Personnel);
                    break;
                case "Pays":
                    PersonnelEditManageLookup.PaysChange(selectedRef, $ctrl.Personnel);
                    break;
                case "Societe":
                    PersonnelEditManageLookup.SocieteChange(selectedRef, $ctrl.Personnel);
                    handleCheckSocieteMatriculeExist();
                    break;
                case "Ressource":
                    PersonnelEditManageLookup.RessourceChange(selectedRef, $ctrl.Personnel);

                    break;
                case "Materiel":
                    PersonnelEditManageLookup.MaterielChange(selectedRef, $ctrl.Personnel);

                    break;
                case "Role":
                    if (selectedRef !== null) {
                        actionAddRole(selectedRef);
                    }
                    break;
                case "Devise":
                    if (selectedRef !== null) {
                        actionAddDevise(selectedRef);
                    }
                    break;
                case "Organisation":
                    if (selectedRef !== null) {
                        $ctrl.indexRole = indexRole;
                        actionAddOrganisation(selectedRef);
                    }
                    break;
            }
        }

        //Fonction d'initialisation des données de la picklist 
        function showPickList(val, societeId, organisationId) {
            $ctrl.apiController = val;
            var baseControllerUrl = '/api/' + val + '/SearchLight/?page={0}&societeId={1}&ciId={2}&groupeId={3}&organisationId={4}';

            switch (val) {
                case "EtablissementPaie":
                    baseControllerUrl = '/api/EtablissementPaie/SearchLight/?page={0}&societeId={1}&ciId={2}&groupeId={3}&organisationId={4}?&isHorsRegion={5}&isAgenceRattachement={6}';
                    baseControllerUrl = String.format(baseControllerUrl, 1, $ctrl.Personnel.Societe.SocieteId, null, null, null, null, true);
                    break;
                case "EtablissementRattachement":
                    baseControllerUrl = '/api/EtablissementPaie/SearchLight/?page={0}&societeId={1}&agenceId={2}&isHorsRegion={3}&isAgenceRattachement={4}';
                    baseControllerUrl = String.format(baseControllerUrl, 1, $ctrl.Personnel.Societe.SocieteId, $ctrl.Personnel.EtablissementPaieId, false, null);
                    break;
                case "Societe":
                    baseControllerUrl = '/api/' + val + '/SearchLight';
                    break;
                case "RessourcePersonnel":
                    baseControllerUrl = String.format(baseControllerUrl, 1, societeId, null, null, null);
                    break;
                case "Materiel":
                    baseControllerUrl = String.format(baseControllerUrl, 1, societeId, null, null, null);
                    break;
                case "Fournisseur":
                    baseControllerUrl = String.format(baseControllerUrl, 1, null, null, $ctrl.Personnel.Societe.GroupeId, null);
                    break;
                case "Role":
                    baseControllerUrl = String.format(baseControllerUrl, 1, $ctrl.societeForAffectation.SocieteId, null, null, null);
                    break;
                case "Devise":
                    baseControllerUrl = String.format(baseControllerUrl, 1, null, null, null, organisationId);
                    break;
                default:
                    baseControllerUrl = String.format(baseControllerUrl, 1, $ctrl.Personnel.Societe.SocieteId, null, null, null);
                    break;
            }
            return baseControllerUrl;
        }

        /*     
          * @description Handler : Suppression d'un élément sélectionné dans une picklist     
          * @param {any} type Type de la picklist
          */
        function handleDelete(type) {
            switch (type) {
                case "Societe":
                    PersonnelEditFieldsCleanerService.removeSociete($ctrl.Personnel);
                    break;
                case "EtablissementPaie":
                    PersonnelEditFieldsCleanerService.removeEtablissementPaie($ctrl.Personnel);
                    break;
                case "EtablissementRattachement":
                    PersonnelEditFieldsCleanerService.removeEtablissementRattachement($ctrl.Personnel);
                    break;
                case "Ressource":
                    PersonnelEditFieldsCleanerService.removeRessource($ctrl.Personnel);
                    break;
                case "Materiel":
                    PersonnelEditFieldsCleanerService.removeMateriel($ctrl.Personnel);
                    break;
            }
        }

        /*
          * @description Fonction de vérification de la cohérence des dates saisies (début avant fin)
          */
        function handleValidateDates() {
            if ($ctrl.Personnel !== null) {
                if ($ctrl.Personnel.DateSortie && $ctrl.Personnel.DateEntree !== null) {
                    // Set l'heure à 00:00:00 pour comparer que la date
                    $ctrl.Personnel.DateEntree.setHours(0, 0, 0, 0);
                    $ctrl.Personnel.DateSortie.setHours(0, 0, 0, 0);
                    $ctrl.formGestionPersonnel.DateEntree.$setValidity("RangeError", $ctrl.Personnel.DateSortie > $ctrl.Personnel.DateEntree);
                }
                else {
                    $ctrl.formGestionPersonnel.DateEntree.$setValidity("RangeError", true);
                }
                $timeout(angular.noop);
            }
        }

        /*
          * @function snapshotState(category)
          * @description Sauvegarde l'état initial d'une catégorie passée en paramètre
          * @param {string} category Catégorie du personnel
          */
        function snapshotState(category) {
            switch (category) {
                case "general": $ctrl.initialPersonnel = angular.copy($ctrl.Personnel); break;
                case "contact": $ctrl.initialPersoForContact = angular.copy($ctrl.Personnel); break;
                case "delegation": $ctrl.initialPersoForDelegation = angular.copy($ctrl.Personnel); break;
                case "utilisateur": $ctrl.initialPersoForUser = angular.copy($ctrl.Personnel); break;
                case "habilitation": $ctrl.initialListRoles = angular.copy($ctrl.roleList); break;
                case "signatureProfil": $ctrl.initialPersonnelImage = angular.copy($ctrl.PersonnelImage); break;
            }
        }

        /*
          * @function restoreState(category)
          * @description Restaure l'état initial d'une catégorie passée en paramètre
          * @param {string} category Catégorie du personnel
          */
        function restoreState(category) {
            switch (category) {
                case "general": $ctrl.Personnel = angular.copy($ctrl.initialPersonnel); break;
                case "contact": $ctrl.persoForContact = angular.copy($ctrl.initialPersoForContact); break;
                case "delegation": $ctrl.PersoForDelegation = angular.copy($ctrl.initialPersoForDelegation); break;
                case "habilitation": $ctrl.roleList = angular.copy($ctrl.initialListRoles); break;
                case "utilisateur": $ctrl.persoForUser = angular.copy($ctrl.initialPersoForUser); break;
                case "signatureProfil": $ctrl.PersonnelImage = angular.copy($ctrl.initialPersonnelImage); break;
            }
        }

        function persisteState(partialModified, category) {
            var partialsPersonnel = [$ctrl.Personnel, $ctrl.persoForContact, $ctrl.persoForUser];
            PersonnelEditPersisteStateService.persisteState(partialModified, category, partialsPersonnel);
            persisteStateOnInitial(category);
        }

        function persisteStateOnInitial(category) {
            switch (category) {
                case "general": $ctrl.initialPersonnel = angular.copy($ctrl.Personnel); break;
                case "contact": $ctrl.initialPersoForContact = angular.copy($ctrl.persoForContact); break;
                case "delegation": $ctrl.initialPersoForDelegation = angular.copy($ctrl.PersoForDelegation); break;
                case "utilisateur": $ctrl.initialPersoForUser = angular.copy($ctrl.persoForUser); break;
                case "habilitation": $ctrl.initialListRoles = angular.copy($ctrl.roleList); break;
                case "signatureProfil": $ctrl.initialPersonnelImage = angular.copy($ctrl.PersonnelImage); break;
            }
        }

        /*
          * @function handleCancel(category)
          * @description Annule les modifications en cours
          * @param {string} category Catégorie du personnel
          */
        function handleCancel(category) {
            switch (category) {
                case "general": confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationAnnulation, "flaticon flaticon-warning").then(actionCancelGeneral); break;
                case "contact": confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationAnnulation, "flaticon flaticon-warning").then(actionCancelContact); break;
                case "habilitation": confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationAnnulation, "flaticon flaticon-warning").then(actionCancelHabilitation); break;
                case "utilisateur": confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationAnnulation, "flaticon flaticon-warning").then(actionCancelUtilisateur); break;
                case "signatureProfil": confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationAnnulation, "flaticon flaticon-warning").then(actionCancelSignatureProfil); break;
            }
        }

        /*
          * @function handleSave(category)
          * @description Sauvegarde les modifications
          * @param {string} category Catégorie du personnel
          */
        function handleSave(category) {
            var promise = null;
            switch (category) {
                case "general": promise = actionSaveGeneral(); break;
                case "contact": promise = actionSaveContact(); break;
                case "habilitation": promise = actionSaveHabilitation(); break;
                case "utilisateur": promise = actionSaveUtilisateur(); break;
                case "signatureProfil": promise = actionSaveSignatureProfil(); break;
            }
            if (promise) {
                promise.then(function () { return category; })
                    .then(actionClosePanel);
            }
        }

        /*
         * @function handleExportExcel()
         * @description Export Excel des habilitations
         */
        function handleExportExcel() {
            ProgressBar.start();
            PersonnelService.getExportExcelUserHabilitations($ctrl.Personnel.UtilisateurId)
                .then(response => {
                    PersonnelService.downloadExcelUserHabilitations(response.data.id, $ctrl.Personnel.UtilisateurId);
                })
                .catch(error => {
                    Notify.error("Erreur lors de l'export excel");
                })
                .finally(() => ProgressBar.complete());
        }

        /*
          * @description Referme le panneau à la réussite de l'enregistrement
          */
        function actionClosePanel(panel) {
            angular.element("#panel-heading-" + panel).addClass('collapsed');
            angular.element("#collapse-" + panel).removeClass('in');
        }

        /**
          * Récupération du pays par son libellé
          * @param {any} libelle libellé du pays récupéré de google
          * @returns {any} Pays
          */
        function actionGetPays(libelle) {
            return PaysService.GetByLibelle(libelle)
                .then(function (result) {
                    $ctrl.persoForContact.Pays = result.data;
                    $ctrl.persoForContact.PaysId = result.data.PaysId;
                }).catch(Notify.defaultError);
        }

        /**
         * Création d'une liste d'erreurs
         * Erreurs issues de la validation par les Validators de FRED
         * @param {any} modelState objet model avec erreurs
         * @returns {any} Liste des erreurs
         */
        function actionGetErrors(modelState) {
            if (modelState) {
                var errors = [];
                for (var key in modelState) {
                    for (var numError in modelState[key]) {
                        if (!errors.includes(modelState[key][numError])) {
                            errors.push(modelState[key][numError]);
                        }
                    }
                }
                return errors;
            }
            return [];
        }

        /**
         * Gestion des messages d'erreurs après ajout ou mise à jour d'un contrat intérimaire
         * @param {any} error erreur catchée
         */
        function HandleDisplayError(error) {
            if (error && error.data && error.data.ModelState) {
                var errorMessageList = actionGetErrors(error.data.ModelState);
                Notify.error(errorMessageList.join('</br>'));
            }
            else {
                Notify.error($ctrl.resources.Global_Notification_Error);
            }
        }
    }
})(angular);
