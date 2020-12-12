(function (angular) {
    'use strict';

    angular.module('Fred').controller('DetailCommandeController', DetailCommandeController);

    DetailCommandeController.$inject = ['$scope',
        '$q',
        '$window',
        '$filter',
        'CommandeService',
        'Notify',
        'confirmDialog',
        'ProgressBar',
        'fredSubscribeService',
        'CommandeHelperService',
        '$location',
        '$uibModal',
        'PieceJointeService',
        'Enums',
        'CommandeDiversService',
        'CommandeAddAvenantProviderService',
        'CommandeHistoriqueService',
        'fredDialog',
        'authorizationService',
        'CommandeLigneManagerService',
        'CommandeAttachementService',
        'CommandeLigneLockService',
        'UserService'];

    const stormExternalSystem = "STORM_COMMANDE_RZB";

    function DetailCommandeController($scope,
        $q,
        $window,
        $filter,
        CommandeService,
        Notify,
        confirmDialog,
        ProgressBar,
        fredSubscribeService,
        CommandeHelperService,
        $location,
        $uibModal,
        PieceJointeService,
        Enums,
        CommandeDiversService,
        CommandeAddAvenantProviderService,
        CommandeHistoriqueService,
        fredDialog,
        authorizationService,
        CommandeLigneManagerService,
        CommandeAttachementService,
        CommandeLigneLockService,
        UserService) {
        /* -------------------------------------------------------------------------------------------------------------
         *                                            INIT
         * -------------------------------------------------------------------------------------------------------------
         */

        var avenantViewId = 0;

        // Instanciation Objet Ressources
        $scope.resources = resources;
        $scope.permissionKeys = PERMISSION_KEYS;
        $scope.commande = null;
        $scope.typeCommande = null;
        $scope.location = null;
        $scope.prestation = null;
        $scope.commandeReadonly = false;
        $scope.avenantReadonly = true;
        $scope.commandeErrors = new Array();
        $scope.commandeHeaderErrors = new Array();
        $scope.busy = false;
        $scope.maxDateCommande = new Date();
        $scope.CodeSocieteGerante = null;
        $scope.dateAbonnementMin = new Date();
        $scope.limitationUnitesRessource = false;
        $scope.defaultTache = null;

        $scope.statutcommande = [];
        $scope.oldstatutcommande = 'BR'; //inititalisation
        $scope.etatcommande = {
            IsStatutValidee: false,
            IsStatutBrouillon: true,
            IsStatutManuelleValidee: false,
            IsStatutCloturee: false
        };

        /*Gestion de l'affichage des détails en grand ou petit*/
        $scope.statusDisplay = true;
        $scope.modeDisplayDetailCommande = 'detailCommandeRow';
        $scope.modeDisplayLigneCommande = 'ligneCommandeRow';
        $scope.mode = 'row';

        /* A Supprimer quand les accords cadres seront développés ! */
        $scope.codeEUR = "EUR";
        $scope.codeRazelBec = "RB";
        $scope.montantAccordCadreRzb = 15000;
        $scope.maxSeuilMontant = "9999999";

        $scope.typeFrequenceAbo = {
            jour: { Label: 'Jour ouvré', Value: 0, Key: 'days', Unit: 1 },
            semaine: { Label: 'Semaine', Value: 1, Key: 'weeks', Unit: 1 },
            mois: { Label: 'Mois', Value: 2, Key: 'months', Unit: 1 },
            trimestre: { Label: 'Trimestre', Value: 3, Key: 'months', Unit: 3 },
            annee: { Label: 'Année', Value: 4, Key: 'years', Unit: 1 }
        };

        $scope.frequenceAboList = [$scope.typeFrequenceAbo.jour, $scope.typeFrequenceAbo.semaine, $scope.typeFrequenceAbo.mois, $scope.typeFrequenceAbo.trimestre, $scope.typeFrequenceAbo.annee];

        $scope.typeFacturations = {
            Facturation: 1,
            CoutAdditionnel: 2,
            FacturationMontant: 4,
            AvoirQuantite: 7,
            AvoirMontant: 8
        };

        $scope.RenvoyerVersStorm = false;

        // Paramètres globaux : Pièces jointes
        $scope.attachmentAcceptedFormats = "image/jpeg, image/png, image/jpg, application/pdf, application/msword, application/vnd.openxmlformats-officedocument.wordprocessingml.document, application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, text/plain";

        $scope.attachments = null;

        // Paramètres globaux : Historique commande
        $scope.commandeHistorique = [];
        $scope.isLoadingHistorique = false;

        // Paramètres globaux : Avenants
        $scope.currentAvenantId = null;


        // Panel Avis
        $scope.isPanelAvisVisible = false;

        /****************************************************************************************************/

        /*
         * Attendre le chargement de la page pour etre sur que le localStorage est remplit
         */
        $window.onload = function () {
            UserService.getCurrentUser().then(function(user) {
                $scope.currentUser = user.Personnel;
            });
        };

        /*
         * @description Fonction d'initialisation
         */
        $scope.init = function (id, duplicate, returnUrl, refreshUrl, location, prestation, colorBr, colorAv, colorVa, colorCl, colorMva) {
            ProgressBar.start();

            //Initialisation des lignes de commandes dans le Grid
            $scope.commandeId = id;
            $scope.duplicate = duplicate;
            $scope.returnUrl = returnUrl;
            $scope.refreshUrl = refreshUrl;
            $scope.location = location;
            $scope.prestation = prestation;
            $scope.colorBR = colorBr;
            $scope.colorAV = colorAv;
            $scope.colorVA = colorVa;
            $scope.colorCL = colorCl;
            $scope.colorMVA = colorMva;
            $scope.disabledCommandeHeader = false;

            // Authorisations
            $scope.canAddFournisseurProvisoire = authorizationService.getPermission(PERMISSION_KEYS.CreationCommandeBrouillonFournisseurTemporaire) ? true : false;

            // super important !!!!! LE CODE SUIVANT PERMET DE CLIQUER SUR LA RECHERCHE DE LA PICKLIST 
            // Oui je confirme ! J'ai passé 1 jour pour merger à cause de ce truc : Très très important
            $('#edit-command').on('shown.bs.modal', function () {
                $(document).off('focusin.modal');
            });

            if ($scope.commandeId === "" || $scope.duplicate === true) {
                $('#edit-command').modal('show');
            }

            actionLoadCommandeTypeList();
            onGetStatutCommande();
            fredSubscribeService.subscribe({ eventName: 'goBack', callback: actionReturnToCommandeList, tooltip: resources.Commande_Detail_RetourListeCommande_Tooltip });

            var promise = null;
            if ($scope.duplicate) {
                promise = CommandeService.Duplicate($scope.commandeId);
            }
            else {
                promise = CommandeService.Detail($scope.commandeId);
            }

            promise
                .then(onLoadSucess)
                .then(CommandeLigneLockService.initializeHasFeatureToLockUnlockCommandeLigne)
                .catch(function (error) {
                    if (error && error.ModelState && error.ModelState.DateSuppression) {
                        disabledCommande(error);
                    } else {
                        Notify.error(resources.Global_Notification_Chargement_Error);
                    }
                })
                .finally(ProgressBar.complete);
        };

        function disabledCommande(error) {
            // Desactiver les boutons
            $scope.busy = true;
            $scope.commandeReadonly = true;
            $scope.disabledCommandeHeader = error.ModelState.DateSuppression !== null;

            // Messages errors
            $scope.commandeErrors = actionGetErrors(error.ModelState);
            Notify.error($scope.resources.Commande_Detail_Notification_Erreur_Affichage_Commande_Supprimee);
        }

        function onLoadSucess(data) {

            // Si édition de commande
            if ($scope.commandeId && !$scope.duplicate) {

                // Charger les pieces jointes
                PieceJointeService.GetAttachements(Enums.EnumTypeEntite.Commande.Value, $scope.commandeId)
                    .then(function (result) {

                        $scope.attachments = result.data;
                        $scope.commande.PiecesJointesCommande = result.data;
                        // Mettre en non supprimable les pièces jointes
                        // Si commande validée
                        if (data.DateValidation) {
                            for (var i = 0; i < ($scope.attachments || []).length; i++) {
                                let fileDateCreation = $filter('toLocaleDate')($scope.attachments[i].DateCreation);
                                if (fileDateCreation < data.DateValidation) {
                                    $scope.attachments[i].IsLocked = true;
                                }
                            }
                        }
                    })
                    .catch(function (error) {
                        Notify.error(resources.PieceJointe_Error_ChargementFichiers);
                    });
            }

            CommandeHelperService.actionToLocaleDate(data);
            $scope.commande = data;
            $scope.setUrlRessourcesRecommandeesEnabled();
            if ($scope.commande) {

                if ($scope.commande.FrequenceAbonnement >= 0) {
                    $scope.selectedFrequenceAbo = $filter('filter')($scope.frequenceAboList, { Value: $scope.commande.FrequenceAbonnement }, true)[0];
                }

                if ($scope.commande.DureeAbonnement >= 0) {
                    $scope.selectedDureeAbo = $scope.commande.DureeAbonnement;
                }

                if ($scope.commande.Type) {
                    $scope.typeCommande = $scope.commande.Type.Code;
                }

                // Avenant
                avenantViewId = 0;
                for (var i = 0; i < $scope.commande.Lignes.length; i++) {
                    var ligne = $scope.commande.Lignes[i];
                    if (ligne.AvenantLigne !== null) {
                        ligne.IsCommande = false;
                        ligne.ViewId = avenantViewId++;
                        CommandeHelperService.actionCalculLigneMontantHT(ligne);
                        ligne.IsCreated = false;
                        ligne.IsUpdated = false;
                        ligne.IsDeleted = false;
                        if (ligne.AvenantLigne.Avenant.DateValidation === null) {
                            ligne.IsAvenantValide = false;
                            ligne.IsAvenantNonValide = true;
                        }
                        else {
                            ligne.IsAvenantValide = true;
                            ligne.IsAvenantNonValide = false;
                            // Note : "|=" retourne un integer...
                            //$scope.RenvoyerVersStorm = $scope.RenvoyerVersStorm || ligne.AvenantLigne.Avenant.HangfireJobId === null;
                        }
                    }
                    else {
                        ligne.IsCommande = true;
                        ligne.IsAvenantValide = false;
                        ligne.IsAvenantNonValide = false;
                    }
                }


                CommandeHelperService.updateTotalCommande($scope.commande);

                if ($scope.commande.CI) {
                    $q.when()
                        .then(function () { return $scope.commande.CI; })
                        .then(() => actionLoadSocieteGerante($scope.commande.CI))
                        .then(getOneValueinList)
                        .then(function () {
                            if ($scope.commande.Devise) {
                                actionCheckAccordCadre();
                            }
                        });
                }

                if ($scope.commande.Devise && $scope.commande.Devise.Symbole) {
                    $scope.selectedDeviseSymbole = $scope.commande.Devise.Symbole;
                }

                // Check problèmes hangfire; commande / avenant en lecture seule
                var commandeValidee = $scope.commande.IsStatutValidee || $scope.commande.IsStatutManuelleValidee;

                if (!commandeValidee || $scope.commande.HangfireJobId) {
                    $scope.RenvoyerVersStorm = false;
                } else if ($scope.commande.SystemeExterne && $scope.commande.SystemeExterne.Code === stormExternalSystem) {
                    $scope.RenvoyerVersStorm = false;
                }
                else {
                    $scope.RenvoyerVersStorm = true;
                }

                if (commandeValidee || $scope.commande.IsStatutCloturee) {
                    $scope.commandeReadonly = true;
                }

                $scope.avenantReadonly = !CommandeAddAvenantProviderService.canAddAvenant(commandeValidee, $scope.commande, $scope.prestation);

                if ($scope.selectedFrequenceAbo) {
                    // Calcul de la dernière date de génération d'une réception abonnement          
                    actionGetLastDateOfReceptionGeneration();
                }
            }
        }

        /*
         * @description Fonction qui retourne si un avenant est entrain d'être créé
        */
        $scope.isAddingAvenant = function () {
            return !$scope.avenantReadonly && $scope.handleCountLigneAvenantNonValide() > 0;
        };

        $scope.getLibelleFournisseur = function () {
            if ($scope.commande) {
                if ($scope.commande.FournisseurProvisoire) {
                    return $scope.commande.FournisseurProvisoire;
                } else {
                    if ($scope.commande.Agence) {
                        return $scope.commande.Agence.Libelle;
                    } else {
                        if ($scope.commande.Fournisseur) {
                            return $scope.commande.Fournisseur.Libelle;
                        }
                    }
                }
            }
            return null;
        };

        $scope.getCodeFournisseur = function () {
            if ($scope.commande) {
                if ($scope.commande.Agence) {
                    return $scope.commande.Agence.Code;
                } else {
                    if ($scope.commande.Fournisseur) {
                        return $scope.commande.Fournisseur.Code;
                    }
                }
            }
            return null;
        };

        /*
         * @description Fonction changement de type de commande dans la dropdownlist
         */
        $scope.handleChangeAboInputs = function () {
            if ($scope.selectedFrequenceAbo) {
                $scope.commande.FrequenceAbonnement = $scope.selectedFrequenceAbo.Value;

                if ($scope.commande.FrequenceAbonnement >= 0) {
                    $scope.selectedFrequenceAbo = $filter('filter')($scope.frequenceAboList, { Value: $scope.commande.FrequenceAbonnement }, true)[0];
                }

                if ($scope.commande.DateProchaineReception) {
                    // Calcul de la dernière date de génération d'une réception abonnement
                    $q.when()
                        .then(ProgressBar.start)
                        .then(actionGetLastDateOfReceptionGeneration)
                        .finally(ProgressBar.complete);
                }
            }
        };

        $scope.handleChangeDureeAbonnement = function () {
            if ($scope.commande.DureeAbonnement !== $scope.selectedDureeAbo) {
                $scope.commande.DureeAbonnement = parseInt($scope.selectedDureeAbo);
                $scope.commande.DateProchaineReception = $scope.dateAbonnementMin;
                $scope.handleChangeAboInputs();
            }
        };

        /*
         * @description Gestion de l'affichage de la commande : toggle
         */
        $scope.handletoggleDisplayMode = function () {
            if ($scope.mode === 'row') {
                actionChangeDisplayMode('column');
            }
            else {
                actionChangeDisplayMode('row');
            }
        };

        /*
         * @description Gestion du changement CommandeManuelle : toggle
         */
        $scope.handletoggleCommandeManuelle = function () {
            if (!$scope.commande.CommandeManuelle && $scope.commande.NumeroCommandeExterne) {
                $scope.commande.NumeroCommandeExterne = null;
            }
        };

        /*
        * @description indique si on peut afficher le numéro de commande externe
        */
        $scope.canShowNumeroCommandeExterne = function () {
            return $scope.commande !== null && $scope.commande.NumeroCommandeExterne !== null && $scope.commande.NumeroCommandeExterne !== "";
        };

        $scope.handleDisableCollapsiblePanel = function (ev) {
            if ($scope.commande.IsStatutValidee || $scope.commande.IsStatutCloturee) {
                ev.stopPropagation();
            }
        };

        /*
         * @description Gestion de la suppression dans une lookup
         */
        $scope.handleLookupDeletion = function (type) {
            actionLookupDeletion(type);
        };

        /*
         * @description Gestion de la sélection dans une lookup
         */
        $scope.handleLookupSelection = function (type, item) {

            switch (type) {
                case "CI":
                    if ($scope.commande.CI !== null) {
                        actionUpdateAdresses();
                        actionLookupDeletion('Devise');

                        actionRemoveAllTacheAndRessource();
                        $scope.commande.CiId = $scope.commande.CI.IdRef;

                        if ($scope.commande.CiId) {

                            CommandeService.DeviseRef($scope.commande.CiId).then(function (value) {
                                if (value) {
                                    $scope.commande.Devise = value;
                                    $scope.commande.DeviseId = value.DeviseId;
                                    $scope.selectedDeviseSymbole = $scope.commande.Devise.Symbole;
                                }
                            })
                                .catch(function (reason) {
                                    Notify.error(resources.Commande_Detail_SectionDetail_Devise_Error);
                                });

                            CommandeService.IsCiHaveManyDevises($scope.commande.CiId).then(function (value) {
                                if (value) {
                                    $scope.commande.CI.IsCiHaveManyDevises = value;
                                }
                            })
                                .catch(function (reason) {
                                    Notify.error('Erreur lors de la récupération des devises');
                                });


                            CommandeService.IsLimitationUnitesRessource($scope.commande.CiId).then(function (value) {
                                if (value) {
                                    $scope.limitationUnitesRessource = value;
                                }
                            })
                                .catch(function (reason) {
                                    Notify.error('Erreur lors de la récupération du paramétrage LimitationUnitesRessource');
                                });

                            actionLoadSocieteGerante($scope.commande.CI);
                            $scope.setUrlRessourcesRecommandeesEnabled();
                        }
                    }
                    break;
                case "Devise":
                    if ($scope.commande.Devise !== null) {
                        $scope.commande.DeviseId = $scope.commande.Devise.IdRef;
                        $scope.selectedDeviseSymbole = $scope.commande.Devise.Symbole;
                    }
                    break;
                case "Fournisseur":
                    if ($scope.commande.Fournisseur !== null) {
                        $scope.commande.FournisseurId = $scope.commande.Fournisseur.IdRef;
                        $scope.commande.FournisseurAdresse = $scope.commande.Fournisseur.Adresse;
                        $scope.commande.FournisseurCPostal = $scope.commande.Fournisseur.CodePostal;
                        $scope.commande.FournisseurVille = $scope.commande.Fournisseur.Ville;
                        $scope.commande.FournisseurPaysId = $scope.commande.Fournisseur.PaysId;
                        $scope.commande.FournisseurPays = $scope.commande.Fournisseur.Pays;
                    }
                    break;
                case "PersonnelSuivi":
                    $scope.commande.Suivi = item;
                    $scope.commande.SuiviId = item.IdRef;
                    break;
                case "PersonnelContact":
                    $scope.commande.Contact = item;
                    $scope.commande.ContactId = item.IdRef;
                    $scope.commande.ContactTel = item.Telephone1;
                    break;
            }
        };

        /*
         * @description Gestion de la sélection dans une lookup de ligne de commande
         */
        $scope.handleCommandeLigneLookupSelection = function (type, item, ligne) {
            lookupSelection(type, item, ligne);
            $scope.handleUpdateLigneCommande(ligne);
        };

        /*
        * @description Gestion de la sélection dans une lookup de ligne d'avenant
        */
        $scope.handleAvenantNonValideLigneLookupSelection = function (type, item, ligne) {
            lookupSelection(type, item, ligne);
            $scope.handleUpdateLigneAvenantNonValide(ligne);
        };

        /*
        * @description Gestion de la sélection dans une lookup de ligne de commande ou d'avenant
        */
        function lookupSelection(type, item, ligne) {
            switch (type) {
                case "Tache":
                    ligne.TacheId = item.IdRef;
                    break;
                case "Ressource":
                    if ($scope.limitationUnitesRessource) {
                        ligne.RessourceId = item.IdRef;
                        CommandeService.GetListUniteByRessourceId($scope.commande.CiId, ligne.RessourceId).then(function (value) {
                            if (value) {
                                var listUnites = value;
                                if (ligne.Unite) {
                                    if ($filter('filter')(listUnites, { UniteId: ligne.Unite.UniteId }, true).length === 0) {
                                        ligne.Unite = null;
                                        ligne.UniteId = null;
                                    }
                                }
                                else {
                                    if (listUnites.length === 1) {
                                        ligne.Unite = listUnites[0];
                                        ligne.UniteId = listUnites[0].UniteId;
                                    }
                                }
                            }
                        })
                            .catch(function (reason) {
                                Notify.error('Erreur lors de la récupération du paramétrage LimitationUnitesRessource');
                            });
                    }
                    else {
                        ligne.RessourceId = item.IdRef;
                    }
                    $scope.setUrlRessourcesRecommandeesEnabled();
                    break;
                case "Unite":
                    ligne.UniteId = item.IdRef;
                    break;
            }
        }

        /*
         * @description Gestion de l'URL Lookup
         */
        $scope.showPickList = function (val) {

            var searchLightUrl = '/api/' + val + '/SearchLight/?page={0}&societeId={1}&ciId={2}&groupeId={3}';

            switch (val) {
                case "CI":
                    searchLightUrl = String.format(searchLightUrl, 1, null, null, null);
                    break;
                case "Personnel":
                    searchLightUrl = String.format(searchLightUrl, 1, null, null, null);
                    break;
                case "Fournisseur":
                    if ($scope.commande.CI && $scope.commande.CI.Societe) {
                        searchLightUrl = String.format(searchLightUrl, 1, null, null, $scope.commande.CI.Societe.GroupeId);
                    }
                    break;
                case "Devise":
                    searchLightUrl = String.format(searchLightUrl, 1, null, $scope.commande.CiId, null);
                    break;
                case "Tache":
                    if ($scope.commande.CiId) {
                        searchLightUrl = String.format(searchLightUrl, 1, null, $scope.commande.CiId, null);
                    }
                    break;
                case "Ressource":
                    if ($scope.commande.CiId && $scope.commande.CI.SocieteId) {
                        searchLightUrl = String.format(searchLightUrl, 1, $scope.commande.CI.SocieteId, null, null) + '&achats=true';
                    }
                    break;
            }
            return searchLightUrl;
        };

        /*
         * @description Fonction de duplication de la commande
         */
        $scope.handleDuplicateCommande = function (commande) {
            window.location.href = "../../Detail/" + commande.CommandeId + "/true";
        };

        /*
         * @description Fonction de validaton de la commande
         */
        $scope.handleValidateCommande = function (commande) {
            $scope.commande.Signature = null; // ! Sinon, erreur de conversion json => bitmap, si quelqu'un trouve la solution c'est top...
            var errorUniteMaterielAPointer = false;
            if (commande.IsMaterielAPointer) {
                var listCodeUnite = ["H", "JR", "SM"];
                commande.Lignes.forEach(ligne => {
                    if (!listCodeUnite.includes(ligne.Unite.Code) && ligne.Ressource && ligne.Ressource.TypeRessource && ligne.Ressource.TypeRessource.Code === 'MAT') {
                        errorUniteMaterielAPointer = true;
                    }
                });

                if (errorUniteMaterielAPointer) {
                    var modalInstance = $uibModal.open({
                        animation: true,
                        component: 'verifyExternalMaterialUnityModalComponent',
                        resolve: {
                            commande: function () { return commande; },
                            resources: function () { return $scope.resources; }
                        }
                    });

                    modalInstance.result.then(function (commande) {
                        ConfirmeValidate(commande, false);
                    });
                } else {
                    ConfirmeValidate(commande, false);
                }

            } else {
                ConfirmeValidate(commande, false);
            }

        };

        /**
        * @description Fonction d'envoi de la commande pour validation
        * @param {any} validationRequestModel modele de validationRequest
        * @param {any} isAvenant de type avenant 
        * @returns {any} promise 
        * */
        function actionRequestValidation(validationRequestModel, isAvenant) {

            return CommandeService.RequestValidation(validationRequestModel)
                .then(() => notificationAction(!isAvenant ? 'S&S' : 'SA&S'))
                .catch(function (err) {
                    onSaveError(err);
                });
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////// GESTION AJOUT DUPLICATION MODIFICATION LIGNES DE COMMANDES /////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /*
        * @description Fonction de mise à jour d'une ligne de commande
        */
        $scope.handleUpdateLigneCommande = function (ligne) {
            CommandeLigneManagerService.handleUpdateLigneCommande($scope.commande, ligne);
            actionCheckAccordCadre();
        };

        /*
        * @description Fonction de mise à jour d'une ligne d'avenant
        */
        $scope.handleUpdateLigneAvenantNonValide = function (ligne) {
            CommandeLigneManagerService.handleUpdateLigneAvenantNonValide($scope.commande, ligne);
            actionCheckAccordCadre();
        };

        /*
        * @description Fonction ajout d'une ligne de commande
        */
        $scope.handleAddLigneCommande = function () {
            CommandeLigneManagerService.handleAddLigneCommande($scope.commande, $scope.defaultTache);
            actionCheckAccordCadre();
        };

        /*
        * @description Fonction ajout d'une ligne d'avenant
        */
        $scope.handleAddLigneAvenantNonValide = function () {
            CommandeLigneManagerService.handleAddLigneAvenantNonValide($scope.commande, $scope.defaultTache, avenantViewId++);
            actionCheckAccordCadre();
        };

        /*
         * @description Fonction de duplication d'une ligne de commande
         */
        $scope.handleDuplicateLigneCommande = function (row) {
            CommandeLigneManagerService.handleDuplicateLigneCommande($scope.commande, row);
            actionCheckAccordCadre();
        };

        /*
        * @description Fonction de duplication d'une ligne d'avenant
        */
        $scope.handleDuplicateLigneAvenantNonValide = function (row) {
            CommandeLigneManagerService.handleDuplicateLigneAvenantNonValide($scope.commande, row, avenantViewId++);
            actionCheckAccordCadre();
        };

        /*
         * @description Fonction Suppression d'une ligne de commande
         */
        $scope.handleDeleteLigneCommande = function (row, index) {
            CommandeLigneManagerService.handleDeleteLigneCommande($scope.commande, row, index);
            actionCheckAccordCadre();
        };

        /*
        * @description Fonction Suppression d'une ligne d'avenant
        */
        $scope.handleDeleteLigneAvenantNonValide = function (row) {
            CommandeLigneManagerService.handleDeleteLigneAvenantNonValide($scope.commande, row);
            actionCheckAccordCadre();
        };

        /*
         * @description Fonction de comptage du nombre de ligne de commande
         */
        $scope.handleCountLigneCommande = function (row) {
            return CommandeLigneManagerService.handleCountLigneCommande($scope.commande, row);
        };

        /*
        * @description Fonction de comptage du nombre de ligne d'avenant non validé et non supprimé
        */
        $scope.handleCountLigneAvenantNonValide = function () {
            return CommandeLigneManagerService.handleCountLigneAvenantNonValide($scope.commande);
        };

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        $scope.handleSaveCommandeBrouillon = function (commande) {
            actionSaveUpdate(commande, 'BR')
                .catch(function (err) {
                    onSaveError(err);
                });
        };

        $scope.setUrlRessourcesRecommandeesEnabled = function () {
            $scope.resssourcesRecommandeesOnly = 0;
            if ($scope.commande.CI && $scope.commande.CI.EtablissementComptable && $scope.commande.CI.EtablissementComptable.RessourcesRecommandeesEnabled) {
                $scope.resssourcesRecommandeesOnly = 1;
            }
        };

        /**
         * Modification de l'URL en cas de création d'une nouvelle commande
         * @param {any} commandeId Identifiant de la commande
         * @returns {any} Identifiant commande
         */
        function modifyBrowserUrl(commandeId) {
            $location.path('/Commande/Commande/Detail/' + commandeId + '/false');
            return commandeId;
        }

        /**
         * Gestion des erreurs
         * @param {any} error erreur requête http
         */
        /**
        * Gestion des erreurs sur la validation
        * @param {any} error erreur requête http
        */
        function onSaveError(error) {
            if (error && error.Status && error.Status === 502) {
                Notify.error($scope.resources.Commande_FredIe_not_ok_for_export);
            }
            if (error && error.ModelState) {
                if (error.ModelState.DateSuppression) {
                    Notify.error($scope.resources.Commande_Detail_Notification_Erreur_Validation);
                } else {
                    $scope.commandeErrors = new Array();
                    $scope.commandeHeaderErrors = new Array();
                    $scope.commandeErrors = actionGetErrors(error.ModelState);
                    Notify.error($scope.resources.Commande_Notification_Saisie_Error);
                }
            }
            else {
                Notify.error($scope.resources.Global_Notification_Error);
            }
        }

        function onSaveFinally() {
            $scope.busy = false;
            $scope.commande.CommandeAvaliderProvisoire = false;
            ProgressBar.complete();
        }

        /*
         * @description Fonction d'update de la commande: passage au statut "à valider"
         */
        $scope.handleSaveCommandeAValider = function (commande) {
            var status;

            if (!commande.IsStatutValidee && !commande.IsStatutManuelleValidee) {
                status = 'AV';
            }

            if (commande.FournisseurProvisoire) {
                status = 'BR';
                commande.CommandeAvaliderProvisoire = true;
            }

            actionSaveUpdate(commande, status)
                .catch(function (err) {
                    onSaveError(err);
                });
        };

        $scope.handleValideEnteteCommande = function (commande) {
            CommandeService.ValidateHeader(commande)
                .then(function () {
                    $('#edit-command').modal('hide');
                    $scope.commandeHeaderErrors = new Array();
                    $scope.commandeErrors = new Array();
                })
                .then(() => $scope.setUrlRessourcesRecommandeesEnabled) //Au moment de validation l'en-tête de la commande on vérifie une autre fois si jamais le CI à changer le ressource recommander
                .then(getOneValueinList)///checker les listes par défaut lors d chanchment d'un CI
                .catch(function (response) {
                    $scope.commandeHeaderErrors = new Array();
                    $scope.commandeErrors = new Array();
                    $scope.commandeHeaderErrors = actionGetErrors(response.ModelState);
                    Notify.error($scope.resources.Commande_Notification_Saisie_Error);
                });
        };

        ///*
        // * @description Fonction d'enregistrement d'un avenant.
        // *              Permet également de valider l'avenant.
        // */
        $scope.handleSaveAvenantNonValides = function (commande, toValidate) {
            if (toValidate) {
                ConfirmeValidate(commande, true);
            }
            else {
                actionSaveUpdateAvenant(commande, false)
                    .catch(function (err) {
                        onSaveError(err);
                    });
            }
        };

        function actionSaveUpdateAvenant(commande, toValidate) {
            $scope.busy = true;
            $scope.commande.Signature = null; // ! Sinon, erreur de conversion json => bitmap, si quelqu'un trouve la solution c'est top...
            ProgressBar.start();

            if (!commande.AccordCadre) {
                commande.Justificatif = null;
            }

            UpdateNumberlignes(commande);

            return CommandeService.SaveAvenant($scope.commande, toValidate)
                .then(function (result) { saveAvenantNonValideThen(result, toValidate); })
                .then(function () {
                    $scope.commandeErrors = [];
                    $scope.commandeHeaderErrors = new Array();
                    CommandeHelperService.updateTotalCommande($scope.commande);
                })
                .then(() => CommandeAttachementService.actionSaveOrDeleteAttachment($scope.commande.CommandeId, $scope.attachments))
                .then((result) => $scope.attachments = result.attachments)
                .then(() => notificationAction(toValidate ? 'AvV' : $scope.isAddingAvenant() ? 'AvS' : ''))
                .then(() => redirectionModal(toValidate, $scope.resources.Commande_PostAvenantValidationModal_Firstlabel, $scope.resources.Commande_PostAvenantValidationModal_secondlabel))
                .finally(onSaveFinally);
        }

        /*
         * @description Appelé après l'enregistrement d'un avenant
         */
        function saveAvenantNonValideThen(result, validated) {

            if (!result) {
                return;
            }

            // Important : Pour gérer les accès concurrent
            // Maj date de modification de la commande
            $scope.commande.DateModification = $filter('toLocaleDate')(result.DateModification);

            // Assigner l'Id de l'avenant courant
            if (result && result.Avenant) {
                $scope.currentAvenantId = result.Avenant.AvenantId;
            }

            for (var i = 0; i < $scope.commande.Lignes.length; i++) {
                var ligne = $scope.commande.Lignes[i];
                if (!ligne.IsAvenantNonValide) {
                    continue;
                }

                if (ligne.IsDeleted) {
                    // Supprime la ligne d'avenant de la liste
                    $scope.commande.Lignes.splice(i, 1);
                    i--;
                }
                else {
                    ligne.AvenantLigne.Avenant = result.Avenant;

                    // Met à jour les identifiants des lignes d'avenant
                    for (var j = 0; j < result.ItemsCreated.length; j++) {
                        var created = result.ItemsCreated[j];
                        if (created.ViewId === ligne.ViewId) {
                            ligne.CommandeLigneId = created.CommandeLigneId;
                            result.ItemsCreated.splice(j, 1);
                            break;
                        }
                    }
                    if (validated) {
                        ligne.IsAvenantNonValide = false;
                        ligne.IsAvenantValide = true;
                    }
                }

                ligne.IsCreated = false;
                ligne.IsUpdated = false;
                ligne.IsDeleted = false;
            }
        }

        /**
        * @description Fonction d'extraction d'un brouillon de commande
        */
        $scope.handleExtractBrouillonBonDeCommande = function () {
            $scope.busy = true;
            $scope.commande.Signature = null; // ! Sinon, erreur de conversion json => bitmap, si quelqu'un trouve la solution c'est top...

            // Si une pièce jointe a été ajoutée mais pas encore téléversée => afficher un message pour demander la sauvegarde
            if (checkForUnuploadedPieceJointe()) {
                return;
            }

            var commande = GetCommandeToPrint(true);
            var promise = CommandeService.ExtractPDFBrouillonDeBonDeCommande(commande);
            processExtractBonDeCommande(promise, commande);

            $scope.busy = false;
        };

        /**
         * @description Fonction d'extraction des commandes à l'état brouillon
         */
        $scope.handleExtractBonDeCommandeDeCommandeBrouillon = function () {
            $scope.busy = true;
            $scope.commande.Signature = null; // ! Sinon, erreur de conversion json => bitmap, si quelqu'un trouve la solution c'est top...

            // Si une pièce jointe a été ajoutée mais pas encore téléversée => afficher un message pour demander la sauvegarde
            if (checkForUnuploadedPieceJointe()) {
                return;
            }

            var commande = GetCommandeToPrint(false);
            var promise = CommandeService.ExtractPDFBonDeCommandeDeCommandeBrouillon(commande);
            processExtractBonDeCommande(promise, commande);
            $scope.busy = false;
        };

        /**
         * @description Fonction d'extraction des commandes
         */
        $scope.handleExtractBonDeCommande = function () {
            $scope.busy = true;
            $scope.commande.Signature = null; // ! Sinon, erreur de conversion json => bitmap, si quelqu'un trouve la solution c'est top...

            // Si une pièce jointe a été ajoutée mais pas encore téléversée => afficher un message pour demander la sauvegarde
            if (checkForUnuploadedPieceJointe()) {
                return;
            }

            var commande = GetCommandeToPrint(false);
            var promise = CommandeService.ExtractPDFBonDeCommande(commande);
            processExtractBonDeCommande(promise, commande);

            $scope.busy = false;
        };

        function checkForUnuploadedPieceJointe() {
            for (var j = 0; j < ($scope.attachments || []).length; j++) {
                if (!$scope.attachments[j].hasOwnProperty('PieceJointeId')) {
                    Notify.message($scope.resources.Commande_Detail_PieceJointe_CommandeModifiee);
                    return true;
                }
            }
        }

        /**
        * @description Fonction de recuperation de l'export de commande générée
        * @param {any} promise la promise de la generation du fichier
        * @param {any} commande la commande
        */
        function processExtractBonDeCommande(promise, commande) {
            promise.then(function (response) {
                var url = '/api/Commande/ExtractPdfBonDeCommande/' + response.id + "/" + commande.Numero;
                window.open(url, '_blank');

                // Download Attachments
                for (var i = 0; i < ($scope.attachments || []).length; i++) {
                    PieceJointeService.Download($scope.attachments[i].PieceJointeId);
                }
            })
                .catch(Notify.defaultError);
        }

        /*
         * @description Fonction changement de type de commande dans la dropdownlist
         */
        $scope.handleChangeType = function () {
            $scope.commande.IsMaterielAPointer = false;
            $scope.typeCommande = $scope.commande.Type.Code;
            $scope.commande.TypeId = $scope.commande.Type.CommandeTypeId;

            if ($scope.typeCommande === $scope.location) {
                actionOnTypeLocation();
            }
        };

        /*
         * @description Gestion de la suppression d'une commande
         */
        $scope.handleDeleteCommande = function () {
            confirmDialog.confirm(resources, resources.Global_Modal_ConfirmationSuppression).then(function () {

                $scope.commande.Signature = null; // ! Sinon, erreur de conversion json => bitmap, si quelqu'un trouve la solution c'est top...
                $scope.busy = true;
                ProgressBar.start();
                CommandeService.Delete($scope.commande.CommandeId)
                    .then(function (value) {
                        Notify.message($scope.resources.Global_Notification_Suppression_Success);
                        actionReturnToCommandeList();
                    })
                    .catch(function (error) {
                        if (error && error.ModelState && error.ModelState.DateSuppression) {
                            Notify.error($scope.resources.Commande_Detail_Notification_Erreur_Suppression);
                        } else {
                            if (error.Status === 409) {
                                Notify.error($scope.resources.Commande_Notification_ErreurBadUser_Error);
                            }
                            else {
                                Notify.error($scope.resources.Global_Notification_Error);
                            }
                        }
                    })
                    .finally(onSaveFinally);
            });
        };

        /*
         * @description Fonction permettant de retourner à la liste des commandes
         */
        $scope.handleReturnToCommandeList = function () {
            actionReturnToCommandeList();
        };

        /*
         * @description Gestion du collapse et collapsed des lignes du tableau
         */
        $scope.handleCollapse = function (id) {
            var e = document.querySelector(id);
            var isExpanded = angular.element(e).attr("aria-expanded");
            var action = "";
            action = isExpanded === "true" ? "hide" : "show";
            angular.element(e).collapse(action);
        };

        /**
         * Action clique bouton Renvoyer vers SAP
         */
        $scope.handleExportToSap = function () {
            if (!$scope.busy) {
                $scope.busy = true;
                ProgressBar.start();

                CommandeService.ReturnCommandeToSap($scope.commande.CommandeId)
                    .then(returnCommandeToSapThen)
                    .catch(function (error) { console.log(error); })
                    .finally(onSaveFinally);
            }
        };

        function returnCommandeToSapThen(result) {

            if (result.CommandeTraitee) {
                $scope.commande.HangfireJobId = result.CommandeHangfireJobId;
            }
            $scope.RenvoyerVersStorm = $scope.commande.HangfireJobId === null;

            for (var i = 0; i < $scope.commande.Lignes.length; i++) {
                var ligne = $scope.commande.Lignes[i];
                if (ligne.IsAvenantValide) {
                    for (var j = 0; j < result.Avenants.length; j++) {
                        var avenant = result.Avenants[j];
                        if (ligne.AvenantLigne.Avenant.NumeroAvenant === avenant.NumeroAvenant) {
                            // Les lignes de commande d'un même avenant n'ont pas la même référence objet sur cet avenant...
                            // Il faut donc mettre à jour toutes les lignes corespondantes
                            ligne.AvenantLigne.Avenant.HangfireJobId = avenant.HangfireJobId;
                        }
                    }

                    if (ligne.AvenantLigne.Avenant.HangfireJobId === null) {
                        $scope.RenvoyerVersStorm = true;
                    }
                }
            }
        }

        /*
         * @description Fonction d'envoie de la commande par mail
         */
        $scope.handleSendByMail = function () {
            $scope.commande.Signature = null; // ! Sinon, erreur de conversion json => bitmap, si quelqu'un trouve la solution c'est top...
            var commande = GetCommandeToPrint();
            $scope.busy = true;
            ProgressBar.start();

            // NPI : passer l'id de la commande suffirait ici, mais il y a une histoire avec du cache (a analyser avant modification)
            CommandeService.SendByMail(commande)
                .then(function () {
                    Notify.message($scope.resources.Commande_Notification_EmailEnvoyer_Success);
                })
                .catch(function (data) {
                    $scope.commandeErrors = actionGetErrors(data.ModelState);
                    Notify.error($scope.resources.Commande_Notification_EmailEnvoyer_Error);
                })
                .finally(onSaveFinally);
        };

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////   ATTACHMENTS      /////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        $scope.handleCheckBeforeSelectFile = function (event, element) {
            CommandeAttachementService.handleCheckBeforeSelectFile(event, element, $scope.commande, $scope.resources);
        };

        $scope.handleSelectFile = function (event) {
            if ($scope.attachments === null) {
                $scope.attachments = [];
            }
            CommandeAttachementService.handleSelectFile(event, $scope.attachments);
        };

        $scope.handleDeleteAttachment = function ($index) {
            CommandeAttachementService.handleDeleteAttachment($index, $scope.attachments);
        };

        $scope.handleDownloadAllAttachments = function () {
            CommandeAttachementService.handleDownloadAllAttachments($scope.attachments, resources);
        };

        $scope.handleDownloadAttachment = function (pieceJointeId, libelle) {
            CommandeAttachementService.handleDownloadAttachment(pieceJointeId, libelle, resources);
        };
        /* -------------------------------------------------------------------------------------------------------------
         *                                            ACTIONS
         * -------------------------------------------------------------------------------------------------------------
         */

        /**
         * Mise à jour du booleen accord cadre en fonction du montant total de la commande
         */
        function actionCheckAccordCadre() {
            $scope.commande.AccordCadre = $scope.commande.Devise.IsoCode === $scope.codeEUR && $scope.commande.TotalCommande > $scope.montantAccordCadreRzb;
            $scope.isAccordCadre = angular.copy($scope.commande.AccordCadre);
        }

        /*
         * @description Gestion de l'affichage de la commande : en colonne ou en ligne
         */
        function actionChangeDisplayMode(mode) {
            $scope.mode = mode;
            if (mode === "row") {
                $scope.modeDisplayDetailCommande = "detailCommandeRow";
                $scope.modeDisplayLigneCommande = "ligneCommandeRow";
                $scope.modeDisplayRowDetailCommande = 'none';

                $scope.statusDisplay = true;
            } else if (mode === 'column') {
                $scope.modeDisplayDetailCommande = "detailCommandeColumn";
                $scope.modeDisplayRowDetailCommande = 'row-one-column'; /*Force les sections du formulaire à passer en flow*/
                $scope.modeDisplayLigneCommande = "ligneCommandeColumn";

                $scope.statusDisplay = false;
            }
        }

        /*
         * @description Chargement de la société gérante 
         */
        function actionLoadSocieteGerante(ci) {
            return CommandeService.SocieteGerante(ci)
                .then(function (value) {
                    if (value.Code) {
                        $scope.CodeSocieteGerante = value.Code;
                    }
                })
                .catch(function (reason) {
                    Notify.error(resources.Global_Notification_Chargement_Error);
                });
        }

        /*
         * @description Fonction de récupération des erreurs métier
         */
        function actionGetErrors(modelState) {
            if (modelState) {
                var errors = new Array();
                $scope.commandeHeaderErrors = new Array();
                for (var key in modelState) {
                    var cle = key.substring(0, 6);
                    if (cle !== "Lignes") {
                        var value = modelState[key][0];

                        if (key === "AccordCadre") {
                            $scope.isAccordCadre = true;
                            $scope.commande.AccordCadre = true;
                        }

                        errors.push(value);
                    }
                    else {
                        for (var numError in modelState[key]) {
                            if (!errors.includes(modelState[key][numError]))
                                errors.push(modelState[key][numError]);
                        }
                    }
                }
                return errors;
            }
            else {
                return null;
            }
        }

        /*
         * @description Fonction de mise à jour des adresses de la commande en fonction du CI choisie
         */
        function actionUpdateAdresses() {
            $scope.commande.LivraisonEntete = $scope.commande.CI.EnteteLivraison;
            $scope.commande.LivraisonAdresse = $scope.commande.CI.AdresseLivraison;
            $scope.commande.LivraisonCPostale = $scope.commande.CI.CodePostalLivraison;
            $scope.commande.LivraisonVille = $scope.commande.CI.VilleLivraison;
            $scope.commande.LivraisonPays = $scope.commande.CI.PaysLivraison;
            $scope.commande.LivraisonPaysId = $scope.commande.CI.PaysLivraisonId;

            $scope.commande.FacturationAdresse = $scope.commande.CI.AdresseFacturation;
            $scope.commande.FacturationCPostale = $scope.commande.CI.CodePostalFacturation;
            $scope.commande.FacturationVille = $scope.commande.CI.VilleFacturation;
            $scope.commande.FacturationPays = $scope.commande.CI.PaysFacturation;
            $scope.commande.FacturationPaysId = $scope.commande.CI.PaysFacturationId;
        }

        /*
         * @description Retour à la liste des commandes
         */
        function actionReturnToCommandeList() {
            window.location = '/Commande/Commande/Index';
        }

        /*
         * @description Gestion de suppression de l'élément sélectionné dans la lookup
         */
        function actionLookupDeletion(type) {
            switch (type) {
                case 'PersonnelSuivi':
                    $scope.commande.Suivi = null;
                    $scope.commande.SuiviId = null;
                    break;
                case 'PersonnelContact':
                    $scope.commande.Contact = null;
                    $scope.commande.ContactId = null;
                    $scope.commande.ContactTel = null;
                    break;
                case 'Devise':
                    $scope.commande.Devise = null;
                    $scope.commande.DeviseId = null;
                    $scope.selectedDeviseSymbole = null;
                    break;
                case 'Fournisseur':
                    $scope.commande.Fournisseur = null;
                    $scope.commande.FournisseurId = null;
                    break;
                case 'CI':
                    $scope.commande.CI = null;
                    $scope.commande.CiId = null;
                    $scope.commande.Fournisseur = null;
                    $scope.commande.FournisseurId = null;
                    $scope.commande.Devise = null;
                    $scope.commande.DeviseId = null;
                    $scope.selectedDeviseSymbole = null;
                    actionRemoveAllTacheAndRessource();
                    break;
            }
        }

        /*
         * @description Suppression de toutes les tâches et ressources d'une commande
         */
        function actionRemoveAllTacheAndRessource() {
            for (var i = 0; i < $scope.commande.Lignes.length; i++) {
                if ($scope.commande.Lignes[i].RessourceId !== undefined) {
                    $scope.commande.Lignes[i].Ressource = null;
                    $scope.commande.Lignes[i].RessourceId = null;
                }
                if ($scope.commande.Lignes[i].TacheId !== undefined) {
                    $scope.commande.Lignes[i].Tache = null;
                    $scope.commande.Lignes[i].TacheId = null;
                }
            }
        }

        /*
         * @description Chargement de la liste des types de commande
         */
        function actionLoadCommandeTypeList() {
            CommandeService.CommandeTypeList()
                .then(function (value) {
                    $scope.commandeTypeList = value.filter(v => v.Code !== 'I');
                })
                .catch(Notify.defaultError);
        }

        /*
         * @description A la selection du type Location, toutes les checkbox spécifiques à une commande Location passe à vrai
         */
        function actionOnTypeLocation() {
            $scope.commande.MOConduite = true;
            $scope.commande.EntretienMecanique = true;
            $scope.commande.EntretienJournalier = true;
            $scope.commande.Carburant = true;
            $scope.commande.Lubrifiant = true;
            $scope.commande.FraisAmortissement = true;
            $scope.commande.FraisAssurance = true;
        }

        $scope.handleCloseAll = function () {
            angular.element(document.querySelectorAll('.commande-ligne-validee .collapse')).collapse('hide');
            angular.element(document.querySelectorAll('.avenant-ligne-validee .collapse')).collapse('hide');
        };

        $scope.handleOpenAll = function () {
            angular.element(document.querySelectorAll('.commande-ligne-validee .collapse:not(.in)')).collapse('show');
            angular.element(document.querySelectorAll('.avenant-ligne-validee .collapse:not(.in)')).collapse('show');
        };

        function actionGetLastDateOfReceptionGeneration() {
            if ($scope.commande.DateProchaineReception && $scope.selectedFrequenceAbo && $scope.commande.DureeAbonnement) {
                var nextReceptionDate = $filter('date')($scope.commande.DateProchaineReception, 'MM-dd-yyyy');

                return CommandeService.GetLastDayOfReceptionGeneration(nextReceptionDate, $scope.selectedFrequenceAbo.Value, $scope.commande.DureeAbonnement)
                    .then(function (date) {
                        $scope.commande.DateDerniereReception = $filter('toLocaleDate')(date);
                    });
            }
        }

        /*
         * @description Retourne la commande à imprimer (exclut les lignes non validées d'avenant)
         */
        function GetCommandeToPrint(isBrouillon) {
            var commande = angular.copy($scope.commande);
            commande.Lignes = [];
            for (var i = 0; i < $scope.commande.Lignes.length; i++) {
                var ligne = $scope.commande.Lignes[i];
                if (!ligne.IsAvenantNonValide) {
                    commande.Lignes.push(ligne);
                }
                else {
                    if (isBrouillon) {
                        // On ajoute un faut numéro d'avenant pour que la ligne soit prise en compte dans le brouillon
                        var lignewithFakeNumeroAvenant = angular.copy(ligne);
                        lignewithFakeNumeroAvenant.AvenantLigne = {
                            Avenant: {
                                NumeroAvenant: 99
                            }
                        };
                        commande.Lignes.push(lignewithFakeNumeroAvenant);
                    }
                    else {
                        commande.MontantHT -= ligne.MontantHT;
                    }
                }
            }
            commande.TotalCommande = commande.MontantHT.toFixed(2);
            return commande;
        }



        /*
        * @description Fonction de création / modification de la commande
        */
        function actionSaveUpdate(commande, status) {

            $scope.commande.Signature = null; // ! Sinon, erreur de conversion json => bitmap, si quelqu'un trouve la solution c'est top...
            $scope.busy = true;
            ProgressBar.start();

            ///pour l'instant elle est toujours false pour touts etats de commande (si jamais elle change suivant l'etat de commande=> créer une méthode avec switch status)
            var setdefaultTache = false;
            var promise = {};

            if (!commande.AccordCadre) {
                commande.Justificatif = null;
            }
            UpdateNumberlignes(commande);
            if (status) {
                saveandchangeStatus(commande, status);
            }
            if (status !== 'VA') {
                if (commande.CommandeId !== 0) {
                    promise = CommandeService.Update(commande)
                        .then(function () {
                            $scope.commandeErrors = [];
                            $scope.commandeHeaderErrors = new Array();
                            $scope.commandeId = commande.CommandeId;
                            return $scope.commandeId;
                        });
                }
                else {
                    promise = CommandeService.Save(commande, setdefaultTache)
                        .then(function (response) {
                            $scope.commandeErrors = [];
                            $scope.commandeHeaderErrors = new Array();
                            $scope.commandeId = response.CommandeId;
                            return $scope.commandeId;
                        })
                        .then(modifyBrowserUrl);
                }
            } else {
                promise = CommandeService.Valider(commande.CommandeId, commande.statutcommande)
                    .then(function () {
                        $scope.commandeErrors = [];
                        $scope.commandeHeaderErrors = new Array();
                        return commande.CommandeId;
                    })
                    .then(modifyBrowserUrl);
            }
            return promise
                .then(function (commandeId) {
                    return CommandeAttachementService.actionSaveOrDeleteAttachment(commandeId, $scope.attachments);
                })
                .then(function(result) {
                    $scope.attachments = result.attachments;
                    
                    return result.commandId;
                })
                .then(CommandeService.Detail)
                .then(onLoadSucess)
                .then(() => notificationAction(status))
                .then(() => redirection(status))
                .then(actionOnSaveOrUpdateSuccess)
                .finally(onSaveFinally);
        }

        function redirection(status) {
            if (status !== 'VA') { return; }
            return redirectionModal(true, $scope.resources.Commande_PostCommandeValidationModal_Firstlabel, $scope.resources.Commande_PostCommandeValidationModal_secondlabel);
        }
        function actionOnSaveOrUpdateSuccess() {

            // Si commande provisoire => Notifier l'utilisateur
            if ($scope.commande && $scope.commande.StatutCommande && $scope.commande.StatutCommande.Code === 'BR' && $scope.commande.FournisseurProvisoire) {

                // Afficher dialog
                fredDialog.generic(
                    $scope.resources.Notif_Brouillon_With_Fournisseur_Provisoire_Message,
                    $scope.resources.Notif_Brouillon_With_Fournisseur_Provisoire,
                    'flaticon flaticon-checked',
                    null,
                    $scope.resources.Global_Bouton_Fermer,
                    null,
                    null);
            }
        }

        function redirectionModal(open, fistLabel, secondLabel) {

            if (!open) { return; }
            // Ouverture de la modal post validation d'une commande
            $uibModal.open({
                animation: true,
                windowClass: 'post-commande-validation-modal',
                component: 'postCommandeValidationModalComponent',
                resolve: {
                    resources: function () { return $scope.resources; },
                    commande: function () { return $scope.commande; },
                    params: function () {
                        return {
                            firstlabel: fistLabel,
                            secondlabel: secondLabel
                        };
                    }
                }
            });
        }

        function onGetStatutCommande() {
            return CommandeDiversService.GetAll()
                .then(function (data) {
                    $scope.statutcommande = data;
                })
                .catch(Notify.defaultError);
        }

        /*
               * @description Fonction D'initialisation des champs liste avec un seul valeur de retour
               */
        function getOneValueinList() {
            checkOneValuesInList()
                .then(data => { SetOneValuesInList(data); })
                .catch(function () {
                    Notify.error('Erreur lors de la récupération des données...');
                });
        }

        function checkOneValuesInList() {
            return checkonevalueReturn('Tache'); //initialise la liste Tache en cas d'un seul élement retourné
        }

        function checkonevalueReturn(val) {
            var searchLightUrl = $scope.showPickList(val);
            return CommandeDiversService.GetData(searchLightUrl)
                .then(function (data) {
                    if (data.length === 1) {
                        switch (val) {
                            case 'Tache': { return data[0]; }
                            default: return null;
                        }
                    }
                    return null;
                });
        }

        function SetOneValuesInList(tache) {
            $scope.defaultTache = tache;
            if (tache) {
                var values = $scope.commande.Lignes;
                angular.forEach(values, function (value, key) {
                    $scope.commande.Lignes[key].TacheId = tache.TacheId;
                    $scope.commande.Lignes[key].Tache = tache;
                });
            }
        }



        function notificationAction(statut) {
            switch (statut) {
                case 'BR':
                    return Notify.message($scope.resources.Commande_Notification_Creation_Commande_Brouillon_Success);
                case 'VA':
                    return Notify.message($scope.resources.Commande_Notification_Creation_Commande_Save_Success);
                case 'AvS'://avenant sauvegarder
                    return Notify.message($scope.resources.Commande_WorkflowPopin_Avenant_SaveSuccess);
                case 'AvV'://avenant Valider
                    return Notify.message($scope.resources.Commande_WorkflowPopin_Avenant_ValidationSuccess);
                case 'S&S'://save commande and Send Email seulement
                    return Notify.message($scope.resources.Commande_WorkflowPopin_SaveAndSendSuccess);
                case 'SA&S'://save Avenant and Send Email seulement
                    return Notify.message($scope.resources.Commande_WorkflowPopin_Avenant_SaveAndSendSuccess);
                default:
                    return Notify.message($scope.resources.Commande_Notification_Commande_Sauvegarde_Success);
            }
        }
        //gérer l'etat et Statut de la commande
        function saveandchangeStatus(commande, status) {
            var stats = $filter('filter')($scope.statutcommande, { Code: status }, true)[0];
            commande.StatutCommandeId = stats.StatutCommandeId;
            commande.statutcommande = stats;
        }

        /*
         * @description Fonction inbitialise la première ligne vide
         */

        function ConfirmeValidate(commande, isavenant) {
            var promise = null;
            //enregistrement des données
            if (isavenant) {
                promise = actionSaveUpdateAvenant(commande, false);
            } else {
                promise = actionSaveUpdate(commande, 'AV');
            }

            promise
                .then(() => openModalAvis(isavenant))
                .catch(function (reason) { onSaveError(reason); });
        }

        function UpdateNumberlignes(commande) {
            var countcmd = 1, countAv = 1;
            angular.forEach(commande.Lignes, function (items, key) {
                if (!items.IsDeleted && items.AvenantLigne === null) {
                    items.NumeroLigne = countcmd++;
                    items.IsUpdated = true;
                }
                if (!items.IsDeleted && items.AvenantLigne !== null) {
                    items.NumeroLigne = countAv++;
                    items.IsUpdated = true;
                }
            });
        }

        function disablefocusButtonValide() {
            angular.element('#BtnValide').blur(); //problème Focus pour Fire Fox :(
        }

        function openModalAvis(isavenant) {
            var validateCommandeModalInstance = $uibModal.open({
                animation: true,
                component: 'ValidateCommandeModalComponent',
                resolve: {
                    commande: function () { return $scope.commande; },
                    resources: function () { return $scope.resources; },
                    isAvenant: isavenant
                }
            });

            validateCommandeModalInstance.result.then(function (model) {
                if (model.directValidation) {
                    // validation de la commande
                    if (isavenant) {
                        return actionSaveUpdateAvenant(model.commande, true)
                            .catch(function (err) {
                                onSaveError(err);
                            });
                    } else {
                        return actionSaveUpdate(model.commande, 'VA')
                            .catch(function (err) {
                                onSaveError(err);
                            });
                    }
                }
                else {
                    //ne pas valider encore la commande
                    var validationRequestModel = {
                        CommandeId: model.commande.CommandeId,
                        CommandeAvenantId: isavenant ? $scope.currentAvenantId : null,
                        Commentaire: model.comment,
                        TypeAvis: model.typeAvis,
                        ExpediteurId: model.senderId,
                        DestinataireId: model.recipient.IdRef
                    };
                    return actionRequestValidation(validationRequestModel, isavenant);
                }
            })
                .finally(disablefocusButtonValide);
        }

        $scope.handleShowDualPicklist = function () {

            // Event d'affichage de la dualpicklist
            $scope.$broadcast("showDualPicklist");
        };

        $scope.handleClearAgenceAndFournisseur = function () {
            $scope.commande.FournisseurProvisoire = null;
            $scope.commande.FournisseurId = null;
            $scope.commande.Fournisseur = null;
            $scope.commande.AgenceId = null;
            $scope.commande.Agence = null;
        };

        $scope.handleSelectFournisseurAgence = function (fournisseur, agence) {
            var replaceAdresse = !$scope.commande.FournisseurProvisoire || !$scope.commande.FournisseurAdresse;
            // Init sélection
            $scope.handleClearAgenceAndFournisseur();

            $scope.commande.FournisseurId = fournisseur.FournisseurId;
            $scope.commande.Fournisseur = fournisseur;

            // Si sélection 
            if (agence && !agence.IsAgencePrincipale) {

                $scope.commande.AgenceId = agence.AgenceId;
                $scope.commande.Agence = agence;

                // Remplir les champs de saisie
                if (agence.Adresse && replaceAdresse) {
                    $scope.commande.FournisseurAdresse = agence.Adresse.Ligne;
                    $scope.commande.FournisseurCPostal = agence.Adresse.CodePostal;
                    $scope.commande.FournisseurVille = agence.Adresse.Ville;
                    $scope.commande.FournisseurPaysId = agence.Adresse.PaysId;
                    $scope.commande.FournisseurPays = agence.Adresse.Pays;
                }

            } else if (replaceAdresse) {

                // Remplir les champs de saisie
                $scope.commande.FournisseurAdresse = fournisseur.Adresse;
                $scope.commande.FournisseurCPostal = fournisseur.CodePostal;
                $scope.commande.FournisseurVille = fournisseur.Ville;
                $scope.commande.FournisseurPaysId = fournisseur.PaysId;
                $scope.commande.FournisseurPays = fournisseur.Pays;
            }
        };

        $scope.handleNewFournisseur = function (text) {
            // Init sélection
            $scope.handleClearAgenceAndFournisseur();

            // Ajouter un fournisseur temporaire
            $scope.commande.FournisseurProvisoire = text;

        };

        // ###################  Avis

        // #### Avis : Functions
        function loadHistorique() {

            // Débuter le chargement de l'historique
            $scope.isLoadingHistorique = true;

            // Charger l'historique d'une commande
            CommandeHistoriqueService.GetHistorique($scope.commandeId)
                .then(function (result) {
                    // Affectation de l'historique de la commande
                    $scope.commandeHistorique = result.data;
                })
                .catch(function (error) {
                    Notify.error(resources.Commande_Error_ChargementHistorique);
                })
                .finally(function () {
                    // Finaliser le chargement de l'historique
                    $scope.isLoadingHistorique = false;
                });
        }

        // #### Avis : Handlers
        $scope.handleShowPanelAvis = function () {
            loadHistorique();
            $scope.isPanelAvisVisible = true;
        };

        $scope.handleHidePanelAvis = function () {
            $scope.isPanelAvisVisible = false;
        };


        // ###################  Import lignes Commande

        $scope.openModal = function () {
            if (!$scope.commande) { return null; }
            var importlignes = $uibModal.open({
                animation: true,
                component: 'importLignesExcelModal',
                resolve: {
                    parms: function () {
                        return {
                            //passer la valeur numéro de commande si en parle des avenants sinon Date de la commande
                            checkinValue: $scope.commande.Numero,
                            ciId: $scope.commande.CiId,
                            isAvenant: !$scope.avenantReadonly
                        };
                    },
                    resources: function () { return $scope.resources; },
                    commandelignes: function () { return $scope.commande.Lignes; }
                }
            });

            importlignes.result.then(function (model) {
                if (model.length !== 0) {
                    if ($scope.commande.Lignes ? $scope.commande.Lignes.length === 1 //existe une seule ligne 
                        && ($scope.commande.Lignes[0].Libelle === null && $scope.commande.Lignes[0].RessourceId === null && $scope.commande.Lignes[0].UniteId === null) //vérifier qu'il a aucune donnée saisie
                        : true) {
                        $scope.commande.Lignes = [];
                    }
                    model.forEach(function (element) {

                        $scope.commande.Lignes.push(element);
                    });
                    CommandeHelperService.updateTotalCommande($scope.commande);
                }
            });
        };
    }
})(angular);
