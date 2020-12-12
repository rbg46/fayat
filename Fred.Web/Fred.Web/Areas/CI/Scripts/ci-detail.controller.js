(function (angular) {
    'use strict';

    angular.module('Fred').controller('CIDetailController', CIDetailController);

    CIDetailController.$inject = ['$q', '$filter', '$timeout', 'Notify', 'CIService', 'AffectationService', 'ProgressBar', 'confirmDialog', 'NgMap', 'GoogleService', 'ReferentielEtenduFilterService', 'DatesClotureComptableService', 'PaysService', 'UserService', '$scope', 'TypeSocieteService', 'FeatureFlags', 'Enums'];

    function CIDetailController($q, $filter, $timeout, Notify, CIService, AffectationService, ProgressBar, confirmDialog, NgMap, GoogleService, ReferentielEtenduFilterService, DatesClotureComptableService, PaysService, UserService, $scope, TypeSocieteService, FeatureFlags, Enums) {
        // assignation de la valeur du scope au controller pour les templates non mis à jour
        var $ctrl = this;

        UserService.getCurrentUser().then(function(user) {
            $scope.userOrganizationId = user.Personnel.Societe.Organisation.OrganisationId;
        });

        // méthodes exposées
        angular.extend($ctrl, {
            handleLoadPage: handleLoadPage,

            /* Gestion picklist */
            handleShowPickList: handleShowPickList,
            handlePickListSelection: handlePickListSelection,
            handlePickListDeletion: handlePickListDeletion,

            handleSave: handleSave,
            handleCancel: handleCancel,
            handleDateValidation: handleDateValidation,
            handleHorairesValidation: handleHorairesValidation,

            // Général            
            handleChangeMontantDevise: handleChangeMontantDevise,

            // Facturation
            handleChangeDeliveryAddress: handleChangeDeliveryAddress,
            handleCopyBillingToDelivery: handleCopyBillingToDelivery,
            handleCopyCIToDelivery: handleCopyCIToDelivery,

            // Paie
            handleCheckGps: handleCheckGps,
            handleOpenPaiePanel: handleOpenPaiePanel,
            handleChangeSelectAdress: handleChangeSelectAdress,
            handleSelectLocation: handleSelectLocation,

            // Devise
            handleDeleteDeviseSec: handleDeleteDeviseSec,

            // Carburant
            handleDeleteOverloadedConso: handleDeleteOverloadedConso,
            handleChangeOverloadedConso: handleChangeOverloadedConso,
            handleChangeDevise: handleChangeDevise,
            handleAddParamCarburant: handleAddParamCarburant,
            handleDeleteParamCarburant: handleDeleteParamCarburant,
            handleChangeParametrageCarburant: handleChangeParametrageCarburant,
            handleVerifObjectifHeureInsertion: handleVerifObjectifHeureInsertion,

            // Equipe et affectation
            handleChangeCalendarDate: handleChangeCalendarDate,
            handleNextWeekAstreints: handleNextWeekAstreints,
            handlePreviousWeekAstreints: handlePreviousWeekAstreints,
            handleToggleTeamFavorite: handleToggleTeamFavorite,
            handleToggleDelegation: handleToggleDelegation,
            handleDeleteAffectation: handleDeleteAffectation,
            blockByPersonnel: blockByPersonnel,
            handleToggleAstreinte: handleToggleAstreinte,
            handlePersonnelLookupSelection: handlePersonnelLookupSelection,
            handleToggleManagingAstreints: handleToggleManagingAstreints,
            handleImportTeam: handleImportTeam,
            handleToggleManagingPointage: handleToggleManagingPointage,

            // Filter Paramétrage des consommations des ressources (referentiel-etendu-filter.service.js)
            ressourcesFilter: ReferentielEtenduFilterService.ressourcesFilter,
            sousChapitresFilter: ReferentielEtenduFilterService.sousChapitresFilter,
            chapitresFilter: ReferentielEtenduFilterService.chapitresFilter

        });

        init();

        return $ctrl;

        /**
         * Initialisation du controller.     
         */
        function init() {
            angular.extend($ctrl, {
                // Instanciation Objet Ressources
                resources: resources,
                searchRessource: "",
                selectedRef: {},
                ciId: 0,
                carto: {
                    latitude: 0,
                    longitude: 0
                },
                panels: {
                    general: { id: "general", data: {} },
                    facturation: { id: "facturation", data: {} },
                    paie: { id: "paie", data: { codeMajorationList: [], codePrimeList: [], address: {} } },
                    devise: { id: "devise", data: { ciDeviseList: [], reference: {}, secondaireList: [] } },
                    carburant: { id: "carburant", data: { ciRessourceList: [], parametrageCarburantList: [], modifiedParametrageCarburantList: [], modifiedCIRessourceList: [], CarburantActif: null } },
                    team: {
                        id: "teams", data: {
                            calendarDates: {},
                            searchFilter: "",
                            calendarAffectations: {
                                AffectationList: []
                            },
                            modifiedAffectationList: [],
                            deletedAffectationList: [],
                            personnelStatutList: [
                                { Id: 1, Libelle: "Ouvrier" },
                                { Id: 2, Libelle: "ETAM" },
                                { Id: 3, Libelle: "IAC" }
                            ],
                            statutFilter: "",
                            teamMembers: []
                        }
                    }
                },

                /* Données initiales */
                initialCI: {},
                initialCISociete: [],
                initialCIDevise: [],
                initialCICarburant: [],
                initialCICodeMajoration: [],
                initialCIPrime: [],
                initialCIRessource: [],
                initialParametrageCarburant: [],
                initialCalendarAffectations: {
                    AffectationList: []
                },
                initialTeamMembers: [],
                closedPeriodList: [],

                generalProperties: [
                    'Libelle', 'Societe', 'SocieteId', 'EtablissementComptable', 'EtablissementComptableId', 'Sep', 'DureeChantier', 'HoraireDebutM', 'HoraireFinM', 'HoraireDebutS',
                    'HoraireFinS', 'TypeCI', 'MontantHT', 'MontantDeviseId', 'MontantDevise', 'TauxHoraire', 'ResponsableChantier', 'ResponsableChantierId', 'PersonnelResponsableChantier', 'ResponsableAdministratif',
                    'ResponsableAdministratifId', 'DateOuverture', 'DateFermeture', 'FraisGeneraux', 'CompteInterneSepId', 'CompteInterneSep', 'ObjectifHeuresInsertion'
                ],
                facturationProperties: [
                    'FacturationEtablissement', 'EnteteLivraison', 'AdresseLivraison', 'CodePostalLivraison', 'VilleLivraison', 'PaysLivraisonId', 'PaysLivraison',
                    'AdresseFacturation', 'CodePostalFacturation', 'VilleFacturation', 'PaysFacturationId', 'PaysFacturation'
                ],
                paieProperties: [
                    'Adresse', 'Adresse2', 'Adresse3', 'CodePostal', 'Ville', 'PaysId', 'Pays', 'LatitudeLocalisation', 'LongitudeLocalisation', 'ZoneModifiable'
                ],
                carburantProperties: [
                    'CarburantActif'
                ],
                formParamGeneral: {},
                isGFes: false,
                typeSocieteSep: null
            });

            TypeSocieteService.GetByCode('SEP').then(function (response) { $ctrl.typeSocieteSep = response.data });

            // Initialisation de GoogleMap
            initGoogleMap();
            actionIsRolePaie();
            $ctrl.BlockPersonnel = false;
            $ctrl.disableSaveWhenLoad = false;
        }

        /*
         * @function actionLoadCi
         */
        function actionLoadCi() {
            return $q.when().then(actionGetCi);
        }

        /*
         * @function handleLoadPage(ciId)
         * @description Gère le chargement initial de la page de détail d'un CI
         */
        function handleLoadPage(ciId) {
            $ctrl.ciId = ciId;
            $q.when()
                .then(ProgressBar.start)
                .then(actionLoadDevise)            // Chargement des devises (référence et secondaire)    
                .then(actionLoadCi)                // Chargement du CI
                .then(actionGetClosedMonth)        // Chargement des périodes clôturées                
                .then(actionLoadPaie)              // Chargement des Codes Prime et Codes Majoration       
                .then(actionLoadCarburant)         // Chargement des Prix du carburant et Consommation des ressources matérielles
                .then(actionLoadTeam)              // Chargement des affectations pour la semaine actuelle
                .finally(ProgressBar.complete);
        }

        /*
         * @function actionGetCi()
         */
        function actionGetCi() {
            return CIService.GetById({ ciId: $ctrl.ciId }).$promise
                .then(function (response) {
                    $ctrl.initialCI = response;
                    if (response.Societe.Groupe.Code.trim() === "GFES") {
                        $ctrl.isGFes = true;
                    }
                    actionFormattingDates();
                    // Initisalition des objets des panels Général et Facturation (Module)
                    actionRestoreState($ctrl.panels.general);
                    actionRestoreState($ctrl.panels.facturation);
                })
                .then(handleHorairesValidation)
                .catch(function (error) {
                    if (error.status === 403) {
                        // Gestion de redirection si droits insuffisants pour accéder aux détails du CI
                        // [TSA] : Rediriger directement vers une page indiquant que l'utilisateur n'a pas les droits 
                        Notify.error(resources.CI_Controller_ErrorMetier_DroitsInsuffisants + resources.CI_Controller_ErrorMetier_RedirectionCI);
                        $timeout(function () {
                            window.location.href = "/Index";
                        }, 5000);
                    } else {
                        Notify.error(error);
                    }
                });
        }

        /*
         *  @function actionGetClosedMonth()
         *  @remark Les mois en JS sont numérotés de 0 à 11 (0 = janvier, 1 = février, etc..)
         */
        function actionGetClosedMonth() {

            DatesClotureComptableService
                .GetPreviousCurrentAndNextMonths($ctrl.ciId)
                .then(function (response) {
                    for (var i = 0; i < response.data.length; i++) {
                        var month = response.data[i];
                        var newMonth = month.Mois - 1;
                        $ctrl.closedPeriodList.push({
                            period: month.Mois + '' + month.Annee,
                            isClosed: month.IsClosed,
                            month: newMonth,
                            year: month.Annee
                        });
                    }
                });

        }
        /* -------------------------------------------------------------------------------------------------------------
         *                                            PANEL 1 - PARAMETRAGE GENERAL
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @function actionUpdateGeneral()
         * @description Action de sauvegarde du paramétrage général 
         */
        function actionUpdateCi() {
            handleHorairesChantier();
            handleChangeCIType($ctrl.initialCI);
            return CIService.Update($ctrl.initialCI).$promise
                .then(function (response) {
                    $ctrl.initialCI = response;
                    Notify.message(resources.Global_Notification_Enregistrement_Success);
                    return response;
                })
                .catch(function (error) {
                    Notify.error(resources.Global_Notification_Error);
                    console.log(error);
                });
        }

        function handleChangeCIType(ci) {
            if ($ctrl.isGFes) {
                switch (ci.TypeCI) {
                    case "CI_Search_CIType_Affaire":
                        ci.CITypeId = 1;
                        break;
                    case "CI_Search_CIType_Etude":
                        ci.CITypeId = 2;
                        break;
                    default:
                        ci.CITypeId = 3;
                }
            }
        }

        /*
         * @function handleChangeMontantDevise()   
         */
        function handleChangeMontantDevise() {
            $ctrl.panels.general.data.MontantDevise = $filter('filter')($ctrl.panels.devise.data.ciDeviseList, { DeviseId: $ctrl.panels.general.data.MontantDeviseId }, true)[0].Devise;
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            PANEL 3 - FACTURATION (MODULE)
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @function actionUpdateFacturation()
         * @description Action de sauvegarde de la section Facturation
         */
        function actionUpdateFacturation() {
            if (!$ctrl.formModuleCommande.$invalid) {
                actionUpdateCi().then(function () {
                    actionRestoreState($ctrl.panels.general);
                    actionRestoreState($ctrl.panels.facturation);
                });
            }
        }

        /*
         * @function handleChangeDeliveryAddress()     
         */
        function handleChangeDeliveryAddress() {
            if ($ctrl.panels.facturation.data.FacturationEtablissement) {
                $ctrl.panels.facturation.data.AdresseFacturation = $ctrl.initialCI.EtablissementComptable.Adresse;
                $ctrl.panels.facturation.data.CodePostalFacturation = $ctrl.initialCI.EtablissementComptable.CodePostal;
                $ctrl.panels.facturation.data.VilleFacturation = $ctrl.initialCI.EtablissementComptable.Ville;
                $ctrl.panels.facturation.data.PaysFacturation = $ctrl.initialCI.EtablissementComptable.Pays;
                $ctrl.panels.facturation.data.PaysFacturationId = $ctrl.initialCI.EtablissementComptable.PaysId;
            }
            else {
                $ctrl.panels.facturation.data.AdresseFacturation = $ctrl.initialCI.AdresseFacturation;
                $ctrl.panels.facturation.data.CodePostalFacturation = $ctrl.initialCI.CodePostalFacturation;
                $ctrl.panels.facturation.data.VilleFacturation = $ctrl.initialCI.VilleFacturation;
                $ctrl.panels.facturation.data.PaysFacturation = $ctrl.initialCI.PaysFacturation;
                $ctrl.panels.facturation.data.PaysFacturationId = $ctrl.initialCI.PaysFacturationId;
            }
        }

        /*
         * @function handleCopyBillingToDelivery() 
         */
        function handleCopyBillingToDelivery() {
            $ctrl.panels.facturation.data.AdresseLivraison = $ctrl.panels.facturation.data.AdresseFacturation;
            $ctrl.panels.facturation.data.CodePostalLivraison = $ctrl.panels.facturation.data.CodePostalFacturation;
            $ctrl.panels.facturation.data.VilleLivraison = $ctrl.panels.facturation.data.VilleFacturation;
            $ctrl.panels.facturation.data.PaysLivraison = $ctrl.panels.facturation.data.PaysFacturation;
            $ctrl.panels.facturation.data.PaysLivraisonId = $ctrl.panels.facturation.data.PaysFacturationId;
        }

        function handleCopyCIToDelivery() {
            if ($ctrl.panels.paie.data.address) {
                var address = actionIsNullOrEmpty($ctrl.panels.paie.data.address.Adresse) +
                    actionIsNullOrEmpty($ctrl.panels.paie.data.address.Adresse2) +
                    actionIsNullOrEmpty($ctrl.panels.paie.data.address.Adresse3);

                $ctrl.panels.facturation.data.AdresseLivraison = address;
                $ctrl.panels.facturation.data.CodePostalLivraison = actionIsNullOrEmpty($ctrl.panels.paie.data.address.CodePostal);
                $ctrl.panels.facturation.data.VilleLivraison = actionIsNullOrEmpty($ctrl.panels.paie.data.address.Ville);
                $ctrl.panels.facturation.data.PaysLivraison = actionIsNullOrEmpty($ctrl.panels.paie.data.address.Pays);
                $ctrl.panels.facturation.data.PaysLivraisonId = actionIsNullOrEmpty($ctrl.panels.paie.data.address.PaysId);
            }
        }

        function actionIsNullOrEmpty(string) {
            return string !== null && string !== undefined && string !== "" ? string : "";
        }

        function isNullOrEmpty(string) {
            return string === null || string === undefined || string === "";
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            PANEL 4 - PAIE
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @function actionLoadPaie()
         * @description Chargement du panel Paramétrage de la paye
         */
        function actionLoadPaie() {
            $q.when()
                .then(actionGetCodeMajorationList)
                .then(actionGetCodePrimeList);
        }

        /*
         * @function actionUpdatePaie()
         * @description Action de sauvegarde de la section Paie
         */
        function actionUpdatePaie() {
            $q.when()
                .then(actionUpdateCi)
                .then(actionUpdateCodeMajoration)
                .then(actionUpdateCodePrime);
        }

        /*
         * @function actionGetCodeMajorationList()
         * @description Récupération de la liste des codes prime
         */
        function actionGetCodeMajorationList() {
            if ($ctrl.initialCI.Societe) {
                return CIService.GetCICodeMajorationList({ ciId: $ctrl.ciId, groupeId: $ctrl.initialCI.Societe.GroupeId }).$promise
                    .then(function (response) {
                        angular.forEach(response, function (value) {
                            value.IsLinkedToCI = value.IsLinkedToCI ? value.IsLinkedToCI : value.EtatPublic;
                        });
                        $ctrl.initialCICodeMajoration = response;
                        actionRestoreState($ctrl.panels.paie);
                    })
                    .catch(function (error) {
                        Notify.error(resources.Global_Notification_Error);
                        console.log(error);
                    });
            }

            return null;
        }

        /*
         * @function actionGetCodePrimeList()
         * @description Récupération de la liste des codes prime
         */
        function actionGetCodePrimeList() {
            return CIService.GetCICodePrimeList({ ciId: $ctrl.ciId }).$promise
                .then(function (response) {
                    angular.forEach(response, function (value) {
                        value.IsLinkedToCI = value.IsLinkedToCI ? value.IsLinkedToCI : value.Publique;
                    });
                    $ctrl.initialCIPrime = response;
                    actionRestoreState($ctrl.panels.paie);
                })
                .catch(function (error) {
                    Notify.error(resources.Global_Notification_Error);
                    console.log(error);
                });
        }

        function handleVerifObjectifHeureInsertion(){
            $ctrl.panels.general.data.ObjectifHeuresInsertion = $ctrl.panels.general.data.ObjectifHeuresInsertion.replace('-', '');
        }

        /*
         * @function actionUpdateCodeMajoration()
         * @description Action de mise à jour des codes majoration du CI
         */
        function actionUpdateCodeMajoration() {
            var ciCodeMajorationList = [];
            for (var codeMajorationKey in $ctrl.panels.paie.data.codeMajorationList) {
                if ($ctrl.panels.paie.data.codeMajorationList.hasOwnProperty(codeMajorationKey)) {
                    var codeMajoration = $ctrl.panels.paie.data.codeMajorationList[codeMajorationKey];
                    if (!codeMajoration.EtatPublic && codeMajoration.IsLinkedToCI) {
                        var ciCodeMajoration = {
                            CiCodeMajorationId: 0,
                            CiId: $ctrl.ciId,
                            CodeMajorationId: codeMajoration.CodeMajorationId
                        };
                        ciCodeMajorationList.push(ciCodeMajoration);
                    }
                }
            }
            return CIService.ManageCICodeMajoration({ ciId: $ctrl.ciId }, ciCodeMajorationList).$promise
                .then(function () {
                    Notify.message(resources.Global_Notification_Enregistrement_Success);
                })
                .catch(function (error) {
                    Notify.error(resources.Global_Notification_Error);
                    console.log(error);
                });
        }

        /*
         * @function actionUpdateCodePrime()
         * @description Action de mise à jour des codes prime du CI
         */
        function actionUpdateCodePrime() {
            var ciCodePrimeList = [];
            for (var codePrimeKey in $ctrl.panels.paie.data.codePrimeList) {
                if ($ctrl.panels.paie.data.codePrimeList.hasOwnProperty(codePrimeKey)) {
                    var codePrime = $ctrl.panels.paie.data.codePrimeList[codePrimeKey];
                    if (!codePrime.Publique && codePrime.IsLinkedToCI) {
                        var ciCodePrime = { CiCodePrimeId: 0, CiId: $ctrl.ciId, PrimeId: codePrime.PrimeId };
                        ciCodePrimeList.push(ciCodePrime);
                    }
                }
            }
            return CIService.ManageCIPrime({ ciId: $ctrl.ciId }, ciCodePrimeList).$promise
                .then(function () {
                    Notify.message(resources.Global_Notification_Enregistrement_Success);
                })
                .catch(function (error) {
                    Notify.error(resources.Global_Notification_Error);
                    console.log(error);
                });
        }

        /*
         * @function handleCheckGps
         * @description Recherche et résolution d'Adresse via le service Google. Récupère une liste d'adresses correspondant à notre recherche     
         */
        function handleCheckGps() {
            var a = $ctrl.panels.paie.data.address;
            var pays = a.Pays ? a.Pays.Libelle : "";

            GoogleService.geocode(a.Adresse, a.Adresse2, a.Adresse3, a.CodePostal, a.Ville, pays).then(function (response) {
                var data = response.data;
                if (data && data.length > 0) {
                    $ctrl.addressList = data;
                    $ctrl.panels.paie.data.address.LatitudeLocalisation = data[0].Geometry.Location.Lat;
                    $ctrl.panels.paie.data.address.LongitudeLocalisation = data[0].Geometry.Location.Lng;
                    $ctrl.showAddressList = true;
                }
            }).catch(function (error) {
                Notify.error(error);
            });
        }

        function handleSelectLocation() {
            $ctrl.map.markers.mapSelectorMap.setMap(null);
            $ctrl.carto.latitude = $ctrl.mapSelector.Latitude;
            $ctrl.carto.longitude = $ctrl.mapSelector.Longitude;
            GoogleService.inverserGeocode($ctrl.carto.latitude, $ctrl.carto.longitude).then(function (response) {
                var data = response.data;
                if (data && data.length > 0) {
                    setAddressWithInverseGeocode(data[0], $ctrl.carto.latitude, $ctrl.carto.longitude);
                }
            }).catch(function (error) {
                Notify.error(error.data.Message);
            });
        }

        function setAddressWithInverseGeocode(item, lat, lng) {
            $ctrl.panels.paie.data.address.Adresse = item.Adresse.Adresse1;
            $ctrl.panels.paie.data.address.Adresse2 = item.Adresse.Adresse2;
            $ctrl.panels.paie.data.address.Adresse3 = item.Adresse.Adresse3;
            $ctrl.panels.paie.data.address.CodePostal = item.Adresse.CodePostal;
            $ctrl.panels.paie.data.address.Ville = item.Adresse.Ville;
            $ctrl.panels.paie.data.address.LatitudeLocalisation = Number(lat);
            $ctrl.panels.paie.data.address.LongitudeLocalisation = Number(lng);
            $ctrl.showAddressList = false;

            var codePays = extractCodePays(item);
            actionGetPays(codePays);
        }

        /*
         * @function handleChangeSelectAdress(item)
         * @description Gère la sélection d'une adresse proposée: rempli les champs à l'écran et repositionne le marker     
         * @param {any} item : adresse choisie
         */
        function handleChangeSelectAdress(item) {
            $ctrl.panels.paie.data.address.Adresse = item.Adresse.Adresse1;
            $ctrl.panels.paie.data.address.Adresse2 = item.Adresse.Adresse2;
            $ctrl.panels.paie.data.address.Adresse3 = item.Adresse.Adresse3;
            $ctrl.panels.paie.data.address.CodePostal = item.Adresse.CodePostal;
            $ctrl.panels.paie.data.address.Ville = item.Adresse.Ville;
            $ctrl.panels.paie.data.address.LatitudeLocalisation = item.Geometry.Location.Lat;
            $ctrl.panels.paie.data.address.LongitudeLocalisation = item.Geometry.Location.Lng;
            $ctrl.showAddressList = false;

            var codePays = extractCodePays(item);
            actionGetPays(codePays);
        }

        /*
         * @function: ExtractCodePays
         * @description Permet d'extraire le code pays du composant google    
         */
        function extractCodePays(item) {
            for (var i = 0; i < item.Address_components.length; i++)
                for (var j = 0; j < item.Address_components[i].Types.length; j++)
                    if (item.Address_components[i].Types[j] === 'country') return item.Address_components[i].Short_name;
            return "";
        }

        /*
         * @function: handleOpenPaiePanel
         * @description Charge la carte google map à l'ouverture du panel Contact     
         */
        function handleOpenPaiePanel() {
            var center = $ctrl.map.getCenter();
            $timeout(function () {
                google.maps.event.trigger($ctrl.map, 'resize');
                $ctrl.map.setCenter(center);
            }, 0);
        }

        /**
         * Récupération du pays par son libellé
         * @param {any} code code du pays récupéré de google
         * @returns {any} Pays
         */
        function actionGetPays(code) {
            return PaysService.GetByCode(code)
                .then(function (result) {
                    if (result.data) {
                        $ctrl.panels.paie.data.address.Pays = result.data;
                        $ctrl.panels.paie.data.address.PaysId = result.data.PaysId;
                    } else {
                        $ctrl.panels.paie.data.address.Pays = null;
                        $ctrl.panels.paie.data.address.PaysId = null;
                    }
                }).catch(Notify.defaultError);
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            PANEL 5 - DEVISE
         * -------------------------------------------------------------------------------------------------------------
         */

        function actionLoadDevise() {
            return CIService.GetCIDeviseList({ ciId: $ctrl.ciId }).$promise
                .then(function (response) {

                    angular.forEach(response, function (val) {
                        val.typeDevise = val.Reference ? $ctrl.resources.CI_Controller_Reference : $ctrl.resources.CI_Controller_Secondaires;
                    });

                    $ctrl.initialCIDevise = response;
                    actionRestoreState($ctrl.panels.devise);
                })
                .catch(function (error) {
                    Notify.error(resources.Global_Notification_Error);
                    console.log(error);
                });
        }

        /*
         * @function actionUpdateDevise()
         * @description Action de sauvegarde de la section Devise
         */
        function actionUpdateDevise() {
            var deviseRef = $filter('filter')($ctrl.panels.devise.data.ciDeviseList, { Reference: true }, true)[0];
            if (deviseRef && deviseRef.DeviseId !== null) {
                return CIService.ManageCIDevise({ ciId: $ctrl.ciId }, $ctrl.panels.devise.data.ciDeviseList).$promise
                    .then(function (response) {
                        $ctrl.initialCIDevise = response;
                        actionRestoreState($ctrl.panels.devise);
                        Notify.message(resources.Global_Notification_Enregistrement_Success);
                    })
                    .catch(function (error) {
                        Notify.error(resources.Global_Notification_Error);
                        console.log(error);
                    });
            } else {
                Notify.error(resources.CI_Controller_ErrorMetier_RequiredReferenceDevise);
            }

            return null;
        }

        /*
         * @description Vérifie si la devise secondaire est déjà dans la liste ou pas
         */
        function actionCheckExistDeviseSec(ciDevise) {
            var isAlreadyDeviseSec = $filter('filter')($ctrl.panels.devise.data.secondaireList, { DeviseId: ciDevise.DeviseId }, true);
            return ciDevise && isAlreadyDeviseSec && isAlreadyDeviseSec.length > 0;
        }

        /*
         * @description Vérifie si la devise sélectionnée n'est pas déjà la devise de référence
         */
        function actionIsDeviseReference(devise) {
            return devise && devise.DeviseId === $ctrl.panels.devise.data.reference.DeviseId;
        }

        /*
         * @description Vérifie si la devise sélectionnée n'est pas déjà une devise secondaire
         */
        function actionIsDeviseSecondaire(devise) {
            var isDeviseSec = $filter('filter')($ctrl.panels.devise.data.secondaireList, { DeviseId: devise.DeviseId }, true);
            return devise && isDeviseSec && isDeviseSec.length > 0;
        }

        /*
         * @description Gère la suppression d'une devise secondaire
         */
        function handleDeleteDeviseSec(ciDevise) {
            if (ciDevise) {
                var i = $ctrl.panels.devise.data.secondaireList.indexOf(ciDevise);
                $ctrl.panels.devise.data.secondaireList.splice(i, 1);

                i = $ctrl.panels.devise.data.ciDeviseList.indexOf(ciDevise);
                $ctrl.panels.devise.data.ciDeviseList.splice(i, 1);
            }
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            PANEL 6 - CARBURANT
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @function handleChangeParametrageCarburant(paramCarburant)
         * @description Gestion du changement de la période
         */
        function handleChangeParametrageCarburant(paramCarburant, carburant) {
            var alreadyExist = $filter('filter')($ctrl.panels.carburant.data.modifiedParametrageCarburantList, { CarburantOrganisationDeviseId: paramCarburant.CarburantOrganisationDeviseId }, true)[0];
            var isPeriodValid = !carburant ? true : actionValidatePeriode(carburant, false);
            var isPrixValid = !carburant ? true : actionValidatePrix(carburant, false);

            // Focus sur l'input du prix au changement de période
            // Utilisation du haskey de l'objet car c'est le seul ID de la ligne invariable
            var id = paramCarburant.$$hashKey.split(':')[1];
            $timeout(function () {
                angular.element(document.querySelector('#prixCarburant-' + id))[0].focus();
            });


            if (isPeriodValid && isPrixValid) {
                if (!alreadyExist) {
                    $ctrl.panels.carburant.data.modifiedParametrageCarburantList.push(paramCarburant);
                }
            }
        }

        /*
         * @function handleAddParamCarburant(carburant)
         * @description Gestion de l'ajout d'une ligne dans un paramétrage de carburant
         */
        function handleAddParamCarburant(carburant) {
            if (carburant) {

                // Ouverture du panel s'il est fermé
                var currentCarburantPanel = "#parametrage-" + carburant.CarburantId;
                if (!angular.element(document.querySelector(currentCarburantPanel)).hasClass('in')) {
                    angular.element(document.querySelector(currentCarburantPanel)).collapse('show');
                }

                // Nouvelle date pour la période (par défaut M+1)
                var periode = new Date();
                periode.setDate(1);
                periode.setMonth(periode.getMonth() + 1);

                // Nouvel objet à ajouter
                var paramCarburant = {
                    CarburantOrganisationDeviseId: 0,
                    CarburantId: carburant.CarburantId,
                    DeviseId: $ctrl.panels.carburant.data.selectedDevise.DeviseId,
                    Devise: $ctrl.panels.carburant.data.selectedDevise,
                    OrganisationId: $ctrl.initialCI.Organisation.OrganisationId,
                    Prix: null,
                    Periode: periode,
                    Periodes: actionGetPeriodeList(periode),
                    Cloture: false
                };

                // Ajout du nouvel objet 
                carburant.ParametrageCarburants.push(paramCarburant);

                // Ajout à la liste des éléments envoyés au serveur.
                $ctrl.panels.carburant.data.modifiedParametrageCarburantList.push(paramCarburant);
            }
        }

        /*
         * @function handleDeleteParamCarburant(carburant, paramCarburant)
         * @description Gestion de la suppression d'une ligne de paramétrage carburant
         */
        function handleDeleteParamCarburant(carburant, paramCarburant) {
            if (carburant && paramCarburant) {
                paramCarburant.DateSuppression = new Date();
                var index;

                // Suppression dans la liste des éléments modifiés
                if (paramCarburant.CarburantOrganisationDeviseId === 0) {
                    index = $ctrl.panels.carburant.data.modifiedParametrageCarburantList.indexOf(paramCarburant);
                    $ctrl.panels.carburant.data.modifiedParametrageCarburantList.splice(index, 1);
                }
                else {
                    var alreadyModified = $filter('filter')($ctrl.panels.carburant.data.modifiedParametrageCarburantList, { CarburantOrganisationDeviseId: paramCarburant.CarburantOrganisationDeviseId }, true)[0];
                    if (!alreadyModified) {
                        $ctrl.panels.carburant.data.modifiedParametrageCarburantList.push(paramCarburant);
                    }
                }

                // Suppression dans la liste affichée
                index = carburant.ParametrageCarburants.indexOf(paramCarburant);
                carburant.ParametrageCarburants.splice(index, 1);

                actionValidatePeriode(carburant, false);
                actionValidatePrix(carburant, false);
            }
        }

        /*
         * @function handleChangeDevise()
         * @description Charge la liste des paramétrages carburant en fonction de la devise sélectionnée
         */
        function handleChangeDevise() {
            actionGetParametrageCarburantList();
        }

        /*
         * @function actionValidatePeriode(carburant)
         * @description Valide un paramétrage carburant
         * @param {any} paramCarburant ligne paramétrage à valider
         */
        function actionValidatePeriode(carburant, submit) {
            var dico = [];
            var i;

            // Remise à valid de tous les formPeriodeCell
            for (i = 0; i < carburant.ParametrageCarburants.length; i++) {
                carburant.ParametrageCarburants[i].formPeriodeCell.periodeCarburant.$setValidity("doublePeriode", true);
                if (submit) { carburant.ParametrageCarburants[i].formPeriodeCell.$setSubmitted(); }
            }

            // Recherche de périodes en double
            for (i = 0; i < carburant.ParametrageCarburants.length; i++) {
                var duplicates = carburant.ParametrageCarburants.filter(function (paramCarb) {
                    return paramCarb.Periode.getMonth() === carburant.ParametrageCarburants[i].Periode.getMonth() &&
                        paramCarb.Periode.getFullYear() === carburant.ParametrageCarburants[i].Periode.getFullYear();
                });
                if (duplicates.length > 1) {
                    dico.push({ data: duplicates });
                }
            }

            if (dico.length > 0) {
                for (i = 0; i < dico.length; i++) {
                    for (var j = 0; j < dico[i].data.length; j++) {
                        dico[i].data[j].formPeriodeCell.periodeCarburant.$setValidity("doublePeriode", false);
                    }
                }
                return false;
            }
            return true;
        }

        /*
         * @function actionValidatePrix(carburant)
         */
        function actionValidatePrix(carburant, submit) {
            var ok = true;
            var i;
            // Remise à valid de tous les formPeriodeCell
            for (i = 0; i < carburant.ParametrageCarburants.length; i++) {
                carburant.ParametrageCarburants[i].formPrixCell.prixCarburant.$setValidity("prixObligatoire", true);
                if (submit) { carburant.ParametrageCarburants[i].formPrixCell.$setSubmitted(); }
            }

            for (i = 0; i < carburant.ParametrageCarburants.length; i++) {
                var valid = true;
                if (carburant.ParametrageCarburants[i].Prix === null || carburant.ParametrageCarburants[i].Prix === undefined) {
                    valid = false;
                    ok = false;
                }
                carburant.ParametrageCarburants[i].formPrixCell.prixCarburant.$setValidity("prixObligatoire", valid);
            }
            return ok;
        }

        /*
         * @function actionValidateCarburant()
         */
        function actionValidateCarburant() {
            var isPeriodeOk = true, isPrixOk = true;

            // Vérification des périodes
            angular.forEach($ctrl.panels.carburant.data.parametrageCarburantList, function (val) {
                if (!actionValidatePeriode(val, true)) {
                    isPeriodeOk = false;
                }
            });

            // Vérification des prix
            angular.forEach($ctrl.panels.carburant.data.parametrageCarburantList, function (val) {
                if (!actionValidatePrix(val, true)) {
                    isPrixOk = false;
                }
            });

            if (!isPrixOk) {
                Notify.error($ctrl.resources.CI_Controller_ErrorMetier_PrixObligatoire);
            }
            if (!isPeriodeOk) {
                Notify.error($ctrl.resources.CI_Detail_Carburant_TableCarburant_ErrorPeriodeUnique);
            }

            return isPeriodeOk && isPrixOk;
        }

        /*
         * @function actionGetPeriodeList(currentDate)
         * @description Récupère une liste de date entre M-delta et M+delta en fonction de la date entrée en paramètre
         *              Exclue les périodes qui sont clôturés
         *              Liste utilisée pour la dropdownlist du choix de période d'application               
         */
        function actionGetPeriodeList(currentDate) {
            var result = [];

            //Suppression des mois clots
            result = $ctrl.closedPeriodList.filter(function (period) {
                if (period.isClosed === false) {
                    return true;
                } else {
                    return false;
                }
            });

            //conversion en date des données recu du serveur.
            result = result.map(function (d) {
                return new Date(d.year, d.month, 1);
            });

            // suppression du mois courrant car il sera rajouter a la fin, car l'oject sera lié a la vue automatiquement.
            result = result.filter(function (date) {
                var sameYear = date.getFullYear() === currentDate.getFullYear();
                var sameMonth = date.getMonth() === currentDate.getMonth();
                return !(sameYear && sameMonth);
            });

            result.push(currentDate);

            return result;
        }

        /*
         * @function actionGetParametrageCarburantList()
         * @description Récupère la liste des Carburants associés au CI
         */
        function actionGetParametrageCarburantList() {
            if ($ctrl.initialCI.Organisation !== undefined && $ctrl.initialCI.Organisation.OrganisationId && $ctrl.panels.carburant.data.selectedDevise) {

                return CIService.GetParametrageCarburantList({ organisationId: $ctrl.initialCI.Organisation.OrganisationId, deviseId: $ctrl.panels.carburant.data.selectedDevise.DeviseId }).$promise
                    .then(function (response) {

                        $ctrl.initialParametrageCarburant = [];
                        $ctrl.panels.carburant.data.parametrageCarburantList = [];

                        angular.forEach(response, function (carburant) {
                            // Modification du code de l'Unité (ex: Unité "L/h" : on ne prend que le "L") pour avoir un affichage "Symbole Devise/Unité" correct
                            carburant.Unite.Code = carburant.Unite.Code.split('/')[0];

                            angular.forEach(carburant.ParametrageCarburants, function (paramCarburant) {

                                // Création dynamique d'un nouveau champ "Periodes" contenant la liste des périodes (M-1, M, M+1) + Formatage date période                
                                paramCarburant.Periode = $filter('toLocaleDate')(paramCarburant.Periode);
                                paramCarburant.Periodes = paramCarburant.Cloture ? [paramCarburant.Periode] : actionGetPeriodeList(paramCarburant.Periode);
                            });
                        });

                        $ctrl.initialParametrageCarburant = response;
                        actionRestoreState($ctrl.panels.carburant);
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            }
            return null;
        }

        /*
        * @function actionUpdateCiRessource()
        * @description Mise à jour des CIRessources 
        */
        function actionUpdateParametrageCarburant() {
            if ($ctrl.panels.carburant.data.modifiedParametrageCarburantList.length > 0) {
                if (actionValidateCarburant()) {
                    return CIService.ManageParametrageCarburant($ctrl.panels.carburant.data.modifiedParametrageCarburantList).$promise
                        .then(function (response) {

                            angular.forEach(response, function (val) {
                                var tmp = $filter('filter')($ctrl.panels.carburant.data.parametrageCarburantList, { CarburantId: val.CarburantId }, true)[0];
                                val.Periode = new Date(val.Periode);
                                var periodeMonthYear = val.Periode.getMonth().toString() + val.Periode.getFullYear().toString();

                                angular.forEach(tmp.ParametrageCarburants, function (val1) {
                                    var p1 = val1.Periode.getMonth().toString() + val1.Periode.getFullYear().toString();
                                    if (val1.CarburantId === val.CarburantId && p1 === periodeMonthYear && val1.DeviseId === val.DeviseId) {
                                        val1.CarburantOrganisationDeviseId = val.CarburantOrganisationDeviseId;
                                    }
                                });
                            });
                            $ctrl.panels.carburant.data.modifiedParametrageCarburantList = [];
                            Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
                        })
                        .catch(function (error) {
                            Notify.error(resources.Global_Notification_Error);
                            console.log(error);
                        });
                }
            }
            return null;
        }

        /*
         * @description Récupère la liste des Ressources associées au CI
         */
        function actionGetCiRessourceList() {
            if ($ctrl.initialCI.Societe) {
                return CIService.GetCIRessourceList({ ciId: $ctrl.initialCI.CiId, societeId: $ctrl.initialCI.Societe.SocieteId }).$promise
                    .then(function (response) {
                        $ctrl.initialCIRessource = response;
                        actionRestoreState($ctrl.panels.carburant);
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            }

            return null;
        }

        /*
         * @function handleDeleteOverloadedConso(ressource)
         * @description Supprime la valeur de la surcharge de la consommation d'une ressource
         */
        function handleDeleteOverloadedConso(ciRessource) {
            if (ciRessource) {
                ciRessource.Consommation = null;
                handleChangeOverloadedConso(ciRessource);
            }
        }

        /*
         * @function handleChangeOverloadedConso(ciRessource)
         * @description Gère l'évènement de modification de l'input "Consommation CI"
         */
        function handleChangeOverloadedConso(ciRessource) {
            if (ciRessource) {
                var alreadyModified = false;
                if ($ctrl.panels.carburant.data.modifiedCIRessourceList.length > 0) {
                    angular.forEach($ctrl.panels.carburant.data.modifiedCIRessourceList, function (val, key) {
                        if (val.CiId === ciRessource.CiId && val.RessourceId === ciRessource.RessourceId) {
                            alreadyModified = true;
                            if (ciRessource.Consommation === null && ciRessource.CiRessourceId === 0) {
                                $ctrl.panels.carburant.data.modifiedCIRessourceList.splice(key, 1);
                            }
                        }
                    });
                }
                if (!alreadyModified) {
                    $ctrl.panels.carburant.data.modifiedCIRessourceList.push(ciRessource);
                }
            }
        }

        /*
         * @function actionUpdateCiRessource()
         * @description Mise à jour des CIRessources 
         */
        function actionUpdateCiRessource() {
            if ($ctrl.panels.carburant.data.modifiedCIRessourceList.length > 0) {
                return CIService.ManageCIRessource($ctrl.panels.carburant.data.modifiedCIRessourceList).$promise
                    .then(function (response) {
                        // Mise à jour du front avec les valeurs du serveur : update des deux listes $ctrl.initialCIRessource et $ctrl.panels.carburant.data.ciRessourceList
                        // TODO : à améliorer...
                        for (var i = 0; i < $ctrl.initialCIRessource.length; i++) {
                            for (var j = 0; j < $ctrl.initialCIRessource[i].SousChapitres.length; j++) {
                                for (var k = 0; k < $ctrl.initialCIRessource[i].SousChapitres[j].Ressources.length; k++) {

                                    var initialRessource = $ctrl.initialCIRessource[i].SousChapitres[j].Ressources[k].CIRessources[0];
                                    var ressource = $ctrl.panels.carburant.data.ciRessourceList[i].SousChapitres[j].Ressources[k].CIRessources[0];
                                    var newRessource = $filter('filter')(response, { RessourceId: initialRessource.RessourceId }, true)[0];

                                    if (newRessource) {
                                        if (initialRessource.CiRessourceId === 0 && ressource.CiRessourceId === 0) {
                                            initialRessource.CiRessourceId = newRessource.CiRessourceId;
                                            ressource.CiRessourceId = newRessource.CiRessourceId;
                                        }
                                        else if (newRessource.Consommation === null) {
                                            initialRessource.CiRessourceId = 0;
                                            ressource.CiRessourceId = 0;
                                        }
                                    }
                                }
                            }
                        }
                        $ctrl.panels.carburant.data.modifiedCIRessourceList = [];
                        Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
                    })
                    .catch(function (error) {
                        Notify.error(resources.Global_Notification_Error);
                        console.log(error);
                    });
            }

            return null;
        }

        /*
         * @function actionUpdateCarburant()
         * @description Action de sauvegarde de la section Carburant
         */
        function actionUpdateCarburant() {
            var promise = $q.when()
                .then(actionUpdateParametrageCarburant)
                .then(actionUpdateCiRessource);

            // Si l'activation du carburant a été modifiée, on met à jour le CI                     
            if ($ctrl.panels.carburant.data.CarburantActif !== $ctrl.panels.general.data.CarburantActif) {
                promise = promise
                    .then(actionUpdateCi)
                    .then(function () {
                        actionRestoreState($ctrl.panels.general);
                        $ctrl.panels.carburant.data.CarburantActif = $ctrl.initialCI.CarburantActif;
                    });
            }
            return promise;
        }

        /*
         * @function actionLoadCarburant() 
         * @description Chargement du panel Paramétrage du Carburant : Prix du Carburant + Consommation des Ressources liées au CI
         */
        function actionLoadCarburant() {
            // Chargement par défaut avec la devise de référence du CI
            $ctrl.panels.carburant.data.selectedDevise = $ctrl.panels.devise.data.reference.Devise;
            $q.when()
                .then(actionGetParametrageCarburantList)
                .then(actionGetCiRessourceList);
        }


        /* -------------------------------------------------------------------------------------------------------------
         *                                            PANEL 7 - Equipe et affectations
         * -------------------------------------------------------------------------------------------------------------
         */
        /*
        * @function actionLoadTeam()
        * @description Chargement du panel équipe et affectations
        */
        function actionLoadTeam() {
            $q.when()
                .then(actionInitializeCalendarDates)
                .then(actionGetAffectationList)
                .then(actionGetTeamMembers);
        }

        /*
         * @function actionInitializeCalendarDates()
         * @description Initialisation des dates de calendrier
         */
        function actionInitializeCalendarDates() {

            // Initialisation des dates
            $ctrl.panels.team.data.calendarDates.selectedDate = moment(new Date());
            actionRefereshCalendarDates();
        }

        /*
         * @function actionGetAffectationList()
         * @description Récuperation de la liste des affectations
         */
        function actionGetAffectationList() {
            AffectationService.GetAffectationListByCi({
                ciId: $ctrl.ciId,
                dateDebut: moment($ctrl.panels.team.data.calendarDates.selectedDate).isoWeekday(1).format('YYYY-MM-DD'),
                dateFin: moment($ctrl.panels.team.data.calendarDates.selectedDate).isoWeekday(7).format('YYYY-MM-DD')
            }).$promise
                .then(function (response) {
                    if (response) {
                        $ctrl.panels.team.data.calendarAffectations = response;
                        $ctrl.disablePointage = !$ctrl.panels.team.data.calendarAffectations.IsDisableForPointage;
                    }
                    $ctrl.panels.team.data.modifiedAffectationList = [];
                    $ctrl.panels.team.data.deletedAffectationList = [];
                    cloneAll($ctrl.panels.team.data.calendarAffectations, $ctrl.initialCalendarAffectations);

                })
                .catch(function (error) {
                    if (error.status === 403) {
                        // Gestion de redirection si droits insuffisants pour accéder aux détails du CI
                        // [TSA] : Rediriger directement vers une page indiquant que l'utilisateur n'a pas les droits 
                        Notify.error(resources.CI_Controller_ErrorMetier_DroitsInsuffisants + resources.CI_Controller_ErrorMetier_RedirectionCI);
                        $timeout(function () {
                            window.location.href = "/Index";
                        }, 5000);
                    } else {
                        Notify.error(error);
                    }
                });

        }

        /*
         * @function actionGetTeamMembers()
         * @description Récuperer les membres de l'equipe favourite
         */
        function actionGetTeamMembers() {
            return AffectationService.GetEquipePersonnelsByProprietaireId().$promise
                .then(function (response) {
                    $ctrl.panels.team.data.teamMembers = response;
                    cloneAll($ctrl.panels.team.data.teamMembers, $ctrl.initialTeamMembers);
                })
                .catch(function (error) {
                    if (error.status === 403) {
                        // Gestion de redirection si droits insuffisants pour accéder aux détails du CI
                        // [TSA] : Rediriger directement vers une page indiquant que l'utilisateur n'a pas les droits 
                        Notify.error(resources.CI_Controller_ErrorMetier_DroitsInsuffisants + resources.CI_Controller_ErrorMetier_RedirectionCI);
                        $timeout(function () {
                            window.location.href = "/Index";
                        }, 5000);
                    } else {
                        Notify.error(error);
                    }
                });
        }

        /*
         * @function handleChangeCalendarDate()
         * @description Gère l'évènement de la modification de la date du calendrier
         */
        function handleChangeCalendarDate() {
            var selectedMondayDate = moment($ctrl.panels.team.data.calendarDates.selectedDate).isoWeekday(1).format('DD/MM/YYYY');
            var IsDateChanged = $ctrl.panels.team.data.calendarDates.mondayDate === selectedMondayDate ? false : true;
            if (IsDateChanged) {
                if (actionIsCalendarModified()) {
                    confirmDialog.confirm($ctrl.resources, $ctrl.resources.Global_Modal_ConfirmationEnregistrement)
                        .then(function () {
                            $q.when()
                                .then(actionUpdateTeam);
                        });
                }
                else {
                    $q.when()
                        .then(actionSaveLoadStart)
                        .then(actionRefereshCalendar)
                        .then(actionSaveLoadEnd);
                }
            }
        }

        /*
         * @function handleNextWeekAstreints()
         * @description Gére l'evenement du bouton semaine prochaine
         */
        function handleNextWeekAstreints() {
            if (actionIsCalendarModified()) {
                confirmDialog.confirm($ctrl.resources, $ctrl.resources.Global_Modal_ConfirmationEnregistrement)
                    .then(function () {
                        $q.when()
                            .then(actionSaveLoadStart)
                            .then(actionAddOrUpdateAffectations)
                            .then(actionDeleteAffectations)
                            .then(actionGetNextWeekAstreints)
                            .then(actionSaveLoadEnd);
                    });
            }
            else {
                actionSaveLoadStart();
                actionGetNextWeekAstreints();
                actionSaveLoadEnd();
            }
        }

        /*
         * @function actionGetNextWeekAstreints()
         * @description Passer à la date de la semaine prochaine et actualiser les dates du calendrier
         */
        function actionGetNextWeekAstreints() {
            $ctrl.panels.team.data.calendarDates.selectedDate = moment($ctrl.panels.team.data.calendarDates.selectedDate).startOf('isoWeek').add(7, 'days');
            actionRefereshCalendar();
        }

        /*
         * @function handlePreviousWeekAstreints()
         * @description Gére l'evenement du bouton semaine précedente
         */
        function handlePreviousWeekAstreints() {
            if (actionIsCalendarModified()) {
                confirmDialog.confirm($ctrl.resources, $ctrl.resources.Global_Modal_ConfirmationEnregistrement)
                    .then(function () {
                        $q.when()
                            .then(actionSaveLoadStart)
                            .then(actionAddOrUpdateAffectations)
                            .then(actionDeleteAffectations)
                            .then(actionGetPreviousWeekAstreints)
                            .then(actionSaveLoadEnd);
                    });
            }
            else {
                actionSaveLoadStart();
                actionGetPreviousWeekAstreints();
                actionSaveLoadEnd();
            }
        }

        /*
         * @function actionGetPreviousWeekAstreints()
         * @description Passer à la date de la semaine précedente et actualiser les dates du calendrier
         */
        function actionGetPreviousWeekAstreints() {
            $ctrl.panels.team.data.calendarDates.selectedDate = moment($ctrl.panels.team.data.calendarDates.selectedDate).startOf('isoWeek').subtract(7, 'days');
            actionRefereshCalendar();
        }

        /*
         * @function handleToggleTeamFavorite(affectation)
         * @description Gére l'evenement du bouton equipe favourite: ajouter ou supprimer un personnel depuis l'equipe favourite
         */
        function handleToggleTeamFavorite(affectation) {
            if (affectation) {
                if (affectation.Statut === $ctrl.resources.CI_Details_Team_Worker) {
                    var indexAffectationList = $ctrl.panels.team.data.calendarAffectations.AffectationList.indexOf(affectation);

                    $ctrl.panels.team.data.calendarAffectations.AffectationList[indexAffectationList].IsInFavoriteTeam = !$ctrl.panels.team.data.calendarAffectations.AffectationList[indexAffectationList].IsInFavoriteTeam;
                    actionUpdateModifiedAffectationList(affectation);
                    actionUpdateTeamMembersList(affectation);
                }
                else {
                    Notify.warning($ctrl.resources.CI_Details_Team_Only_Workers_Can_Be_In_Team);
                }
            }
        }

        /*
         * @function handleToggleDelegation(affectation)
         * @description Gére l'evenement du bouton delegation: ajouter ou supprimer delegation pour un personnel
         */
        function handleToggleDelegation(affectation) {
            if (affectation) {
                var indexAffectationList = $ctrl.panels.team.data.calendarAffectations.AffectationList.indexOf(affectation);

                $ctrl.panels.team.data.calendarAffectations.AffectationList[indexAffectationList].IsDelegate = !$ctrl.panels.team.data.calendarAffectations.AffectationList[indexAffectationList].IsDelegate;
                actionUpdateModifiedAffectationList(affectation);
            }
        }

        /*
         * @function handleDeleteAffectation(affectation)
         * @description Gére l'evenement du bouton supprimer affectation
         */
        function handleDeleteAffectation(affectation) {
            if (affectation) {
                if ($ctrl.isGFes) {
                    CheckPersonnelBeforDelete(affectation);
                }
                else {
                    affectation.IsDelete = true;
                    $ctrl.panels.team.data.modifiedAffectationList.push(affectation);
                    var indexAffectationList = $ctrl.panels.team.data.calendarAffectations.AffectationList.indexOf(affectation);
                    $ctrl.panels.team.data.calendarAffectations.AffectationList.splice(indexAffectationList, 1);
                }
            }
        }

        function blockByPersonnel(item) {
            if (item.IsDelete) {
                return true;
            }
            return false;
        }

        function CheckPersonnelBeforDelete(affectation) {
            AffectationService.CheckPersonnelBeforDelete({ personnelId: affectation.PersonnelId, ciId: $ctrl.ciId }).$promise
                .then(function (result) {
                    if (result) {
                        var jsonResult = result.toJSON();
                        if (jsonResult) {
                            checkaffectation(affectation, jsonResult);
                        }
                    }
                });
        }

        function checkaffectation(affectation, result) {
            if (result[0] === "F") {
                confirmDialog.confirm($ctrl.resources, $ctrl.resources.Global_Modal_ConfirmationSuppression)
                    .then(function () {
                        var indexAffectationList = $ctrl.panels.team.data.calendarAffectations.AffectationList.indexOf(affectation);
                        var indexModifiedAffectationList = $ctrl.panels.team.data.modifiedAffectationList.indexOf(affectation);

                        if (indexAffectationList !== -1) {
                            $ctrl.panels.team.data.calendarAffectations.AffectationList.splice(indexAffectationList, 1);
                        }
                        if (indexModifiedAffectationList !== -1) {
                            $ctrl.panels.team.data.modifiedAffectationList.splice(indexModifiedAffectationList, 1);
                        }
                        if (affectation.affectationId !== 0) {
                            $ctrl.panels.team.data.deletedAffectationList.push(affectation);
                        }
                    });
            }
            else {
                if (affectation.IsDelete) {
                    Notify.message($ctrl.resources.Global_Notification_Personnel_Affected);
                    affectation.IsDelete = false;
                    blockByPersonnel(affectation);
                }
                else {
                    Notify.message($ctrl.resources.Global_Notification_Personnel_Desaffected);
                    affectation.IsDelete = true;
                    blockByPersonnel(affectation);
                }
            }
        }

        /*
         * @function actionIsCalendarModified()
         * @description Vérifie si le calendrier a été modifié
         */
        function actionIsCalendarModified() {
            return $ctrl.panels.team.data.modifiedAffectationList.length !== 0 ||
                $ctrl.panels.team.data.deletedAffectationList.length !== 0 ||
                $ctrl.panels.team.data.calendarAffectations.IsAstreinteActive !== $ctrl.initialCalendarAffectations.IsAstreinteActive;
        }

        /* 
         * @function actionRefereshCalendar()
         * @description Rafraîchir les calendrier des affectations en fonction des dates choisies
         */
        function actionRefereshCalendar() {
            $q.when()
                .then(actionRefereshCalendarDates)
                .then(actionGetAffectationList);
        }

        /*
         * @function actionRefereshCalendarDates()
         * @description Actualiser les dates de la calendrier en fonction de la date choisie dans l'input
         */
        function actionRefereshCalendarDates() {
            var firstDayOfActualWeek = moment($ctrl.panels.team.data.calendarDates.selectedDate).startOf('isoWeek');
            $ctrl.panels.team.data.calendarDates.mondayDate = firstDayOfActualWeek.isoWeekday(1).format('DD/MM/YYYY');
            $ctrl.panels.team.data.calendarDates.tuesdayDate = firstDayOfActualWeek.isoWeekday(2).format('DD/MM/YYYY');
            $ctrl.panels.team.data.calendarDates.wendnesdayDate = firstDayOfActualWeek.isoWeekday(3).format('DD/MM/YYYY');
            $ctrl.panels.team.data.calendarDates.thursdayDate = firstDayOfActualWeek.isoWeekday(4).format('DD/MM/YYYY');
            $ctrl.panels.team.data.calendarDates.fridayDate = firstDayOfActualWeek.isoWeekday(5).format('DD/MM/YYYY');
            $ctrl.panels.team.data.calendarDates.saturdayDate = firstDayOfActualWeek.isoWeekday(6).format('DD/MM/YYYY');
            $ctrl.panels.team.data.calendarDates.sundayDate = firstDayOfActualWeek.isoWeekday(7).format('DD/MM/YYYY');

            $ctrl.panels.team.data.calendarDates.selectedWeekLabel = actionGenerateSelectedWeekLabel();
        }

        /*
         * @function actionGenerateSelectedWeekLabel()
         * @description Générer libelle pour la semaine selectionné
         */
        function actionGenerateSelectedWeekLabel() {
            var mondayDayNumber = moment($ctrl.panels.team.data.calendarDates.selectedDate).startOf('isoWeek').format('DD');
            var sundayDayNumber = moment($ctrl.panels.team.data.calendarDates.selectedDate).isoWeekday(7).format('DD');
            var mondayMonth = moment($ctrl.panels.team.data.calendarDates.selectedDate).startOf('isoWeek').format('MM');
            var sundayMonth = moment($ctrl.panels.team.data.calendarDates.selectedDate).isoWeekday(7).format('MM');
            var mondayYear = moment($ctrl.panels.team.data.calendarDates.selectedDate).startOf('isoWeek').format('YYYY');
            var sundayYear = moment($ctrl.panels.team.data.calendarDates.selectedDate).isoWeekday(7).format('YYYY');

            var mondayMonthLabel = mondayMonth === sundayMonth ? '' : $ctrl.resources['Global_Month_' + mondayMonth];
            var sundayMonthLabel = $ctrl.resources['Global_Month_' + sundayMonth];
            var mondayYearLabel = mondayYear === sundayYear ? '' : mondayYear;

            return $ctrl.resources.Global_From + " " + mondayDayNumber + " " + mondayMonthLabel + " " + mondayYearLabel + " " +
                $ctrl.resources.Global_To + " " + sundayDayNumber + " " + sundayMonthLabel + " " + sundayYear;
        }

        /*
         * @function actionUpdateModifiedAffectationList(affectation)
         * @description Mettre à jour la liste des affectations à modifiées
         */
        function actionUpdateModifiedAffectationList(affectation) {
            var indexModifiedAffectationList = $ctrl.panels.team.data.modifiedAffectationList.indexOf(affectation);
            if (actionHasAffectationOriginalValues(affectation)) {
                if (indexModifiedAffectationList !== -1) {
                    $ctrl.panels.team.data.modifiedAffectationList.splice(indexModifiedAffectationList, 1);
                }
            }
            else {
                if (indexModifiedAffectationList === -1) {
                    $ctrl.panels.team.data.modifiedAffectationList.push(affectation);
                }
                else {
                    $ctrl.panels.team.data.modifiedAffectationList[indexModifiedAffectationList] = affectation;
                }
            }
        }

        /*
         * @function actionUpdateTeamMembersList(affectation)
         * @description Mettre à jour la liste des membres d'équipe
         */
        function actionUpdateTeamMembersList(affectation) {
            var indexTeamMember = $ctrl.panels.team.data.teamMembers.findIndex(obj => obj.PersonnelId === affectation.PersonnelId);

            if (!affectation.IsInFavoriteTeam && indexTeamMember !== -1) {
                $ctrl.panels.team.data.teamMembers.splice(indexTeamMember, 1);
            }
            else if (affectation.IsInFavoriteTeam && indexTeamMember === -1) {
                $ctrl.panels.team.data.teamMembers.push(createTeamMember(affectation));
            }
        }

        /*
         * @function actionHasAffectationOriginalValues(affectation)
         * @description Vérifier si une affectation a les valeurs initiales
         */
        function actionHasAffectationOriginalValues(affectation) {
            if (!affectation) {
                return false;
            }

            var indexInitialAffectationList = $ctrl.initialCalendarAffectations.AffectationList.findIndex(obj => obj.PersonnelId === affectation.PersonnelId);
            if (indexInitialAffectationList === -1) {
                return false;
            }

            return affectation.IsInFavoriteTeam === $ctrl.initialCalendarAffectations.AffectationList[indexInitialAffectationList].IsInFavoriteTeam
                && affectation.IsDelegate === $ctrl.initialCalendarAffectations.AffectationList[indexInitialAffectationList].IsDelegate
                && !actionHasAffectationAModifiedAstreinte(affectation);
        }

        /*
         * @function actionHasAffectationAModifiedAstreinte(affectation)
         * @description Vérifier si une affectation contient des astreintes modifiées
         */
        function actionHasAffectationAModifiedAstreinte(affectation) {
            var affectationHasAModifiedAstreinte = false;
            if (affectation) {
                angular.forEach(affectation.Astreintes, function (val) {
                    if (val.IsModified) {
                        affectationHasAModifiedAstreinte = true;
                    }
                });
            }
            return affectationHasAModifiedAstreinte;
        }

        /*
         * @function handleToggleAstreinte(personnelId, astreinte)
         * @description Gére l'évenement modification astreinte
         */
        function handleToggleAstreinte(personnelId, astreinte) {
            if (astreinte) {
                if (astreinte.IsRapportLigneVerouille) {
                    Notify.warning(resources.CI_Desaffectation_Astreintes_Verouiller);
                }
                else {
                    astreinte.IsAstreinte = !astreinte.IsAstreinte;
                    astreinte.IsModified = !astreinte.IsModified;
                    var affectation = $ctrl.panels.team.data.calendarAffectations.AffectationList.find(obj => obj.PersonnelId === personnelId);
                    actionUpdateModifiedAffectationList(affectation);
                }
            }
        }

        /*
         * @function handlePersonnelLookupSelection(item)
         * @description Gérer la sélection au niveau de la lookup
         */
        function handlePersonnelLookupSelection(item) {
            var newAffectation = toPersonnel(item);
            if (!item || !newAffectation) {
                Notify.error(resources.CI_Detail_Team_Error_Affecation);
                return;
            }

            if ($ctrl.panels.team.data.calendarAffectations.AffectationList.some(function (el) { return el.PersonnelId === newAffectation.PersonnelId; })) {
                Notify.warning(resources.CI_Detail_Team_Notification_Personnel_Existant);
                return;
            }

            if ($ctrl.panels.team.data.modifiedAffectationList.some(function (el) { return el.PersonnelId === newAffectation.PersonnelId; })) {
                var addedAffectation = $ctrl.panels.team.data.modifiedAffectationList.find(function (element) {
                    return element.PersonnelId = newAffectation.PersonnelId;
                });
                var indexAffectationList = $ctrl.panels.team.data.modifiedAffectationList.indexOf(addedAffectation);
                $ctrl.panels.team.data.modifiedAffectationList.splice(indexAffectationList, 1);
                newAffectation.IsDelete = false;
                newAffectation.AffectationId = addedAffectation.AffectationId;
            }

            $ctrl.panels.team.data.calendarAffectations.AffectationList.push(newAffectation);
            $ctrl.panels.team.data.modifiedAffectationList.push(newAffectation);
        }

        /*
         * @function actionUpdateTeam()
         * @description Action de sauvegarde de la section Equipe
         */
        function actionUpdateTeam() {
            $q.when()
                .then(actionSaveLoadStart)
                .then(actionAddOrUpdateAffectations)
                .then(actionDeleteAffectations)
                .then(actionRefereshCalendar)
                .then(SaveMassage)
                .then(actionSaveLoadEnd);
        }

        /*
         * @function handleToggleManagingAstreints()
         * @description Gérer l'evenement activer / désactiver la gestion des astreintes
         */
        function handleToggleManagingAstreints() {
            // Keeping the old value unless the user confirm the dialog
            $ctrl.panels.team.data.calendarAffectations.IsAstreinteActive = !$ctrl.panels.team.data.calendarAffectations.IsAstreinteActive;
            if ($ctrl.initialCI.TypeCI === 'CI_Search_CIType_Affaire') {
                if ($ctrl.panels.team.data.calendarAffectations.IsAstreinteActive && $ctrl.panels.team.data.calendarAffectations.IsCiAffectationsHasAstreintes) {
                    Notify.warning($ctrl.resources.Gestion_CI_Disable_Atreintes_Warning);
                }
                else {
                    $ctrl.panels.team.data.calendarAffectations.IsAstreinteActive = !$ctrl.panels.team.data.calendarAffectations.IsAstreinteActive;
                }
            }
            else {
                Notify.warning($ctrl.resources.CI_Detail_Team_Notification_Managing_Astreintes_IsOnlyFor_Affaire_Type);
            }

        }

        /*
         * @function handleImportTeam()
         * @description Gérer l'evenement importer équipe
         */
        function handleImportTeam() {
            if ($ctrl.panels.team.data.teamMembers.length > 0) {
                var message = new Array($ctrl.resources.CI_Detail_Team_Confirm_Import_All_Team_Members);
                for (var i = 0; i < $ctrl.panels.team.data.teamMembers.length; i++) {
                    var member = $ctrl.panels.team.data.teamMembers[i];
                    message.push(member.Nom + " " + member.Prenom + " - " + member.Matricule + " - " + member.Statut);
                }
                confirmDialog.confirm($ctrl.resources, message.join("<br />"), NaN, "200")
                    .then(actionImportTeam);
            }
        }

        /*
         * @function actionImportTeam()
         * @description action importer l'équipe favorite
         */
        function actionImportTeam() {
            angular.forEach($ctrl.panels.team.data.teamMembers, function (member) {
                if (!$ctrl.panels.team.data.calendarAffectations.AffectationList.some(function (el) { return el.PersonnelId === member.PersonnelId; })) {
                    var affectation = createAffectation(member);
                    $ctrl.panels.team.data.calendarAffectations.AffectationList.push(affectation);
                    $ctrl.panels.team.data.modifiedAffectationList.push(affectation);
                }
            });
        }

        /*
         * @function createAffectation(teamMember)
         * @description Créer une nouvelle affectation pour un membre d'équipe
         */
        function createAffectation(teamMember) {
            var affectation = {
                AffectationId: 0,
                PersonnelId: teamMember.PersonnelId,
                Nom: teamMember.Nom,
                Prenom: teamMember.Prenom,
                Statut: teamMember.Statut,
                Matricule: teamMember.Matricule,
                CodeSociete: teamMember.CodeSociete,
                IsInFavoriteTeam: true,
                IsDelegate: false,
                Astreintes: []
            };

            [0, 1, 2, 3, 4, 5, 6].forEach(function (dayOfWeek) {
                var astreinte = {
                    DayOfWeek: dayOfWeek,
                    IsAstreinte: false,
                    AstreinteId: 0,
                    IsModified: false
                };
                affectation.Astreintes.push(astreinte);
            });

            return affectation;
        }

        /*
         * @function createTeamMember(teamMember)
         * @description Créer un membre d'équipe à partir d'une affectation
         */
        function createTeamMember(affectation) {
            return {
                PersonnelId: affectation.PersonnelId,
                Nom: affectation.Nom,
                Prenom: affectation.Prenom,
                Statut: affectation.Statut,
                Matricule: affectation.Matricule,
                CodeSociete: affectation.CodeSociete
            };
        }

        /*
         * @function actionDeleteAffectations()
         * @description Supprimer une liste des affectations depuis le centre d'imputation
         */
        function actionDeleteAffectations() {
            var affectationIdList = $ctrl.panels.team.data.deletedAffectationList.map(function (affectation) {
                return affectation.AffectationId;
            });

            if (affectationIdList && affectationIdList.length > 0) {
                return AffectationService.DeleteAffectations({ ciId: $ctrl.ciId }, affectationIdList).$promise
                    .then(function (response) {
                    })
                    .catch(function (error) {
                        Notify.error(resources.Global_Notification_Error);
                    });
            }
        }

        /*
         * @function actionAddOrUpdateAffectations()
         * @description ajouter ou modifier les affectations de centre d'imputation
         */
        function actionAddOrUpdateAffectations() {
            var calendarAffectations = {
                CiId: $ctrl.panels.team.data.calendarAffectations.CiId,
                StartDateOfTheWeek: $ctrl.panels.team.data.calendarAffectations.StartDateOfTheWeek,
                EndDateOfTheWeek: $ctrl.panels.team.data.calendarAffectations.EndDateOfTheWeek,
                IsAstreinteActive: $ctrl.panels.team.data.calendarAffectations.IsAstreinteActive,
                AffectationList: $ctrl.panels.team.data.modifiedAffectationList,
                IsDisableForPointage: $ctrl.panels.team.data.calendarAffectations.IsDisableForPointage
            };

            return AffectationService.AddOrUpdateAffectationList(calendarAffectations).$promise
                .then(function (response) {
                })
                .catch(function (error) {
                    Notify.error(resources.Global_Notification_Error);
                    console.log(error);
                });
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            DIVERS
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @function handleShowPickList(api)
         * @description Construit l'URL du référentiel
         */
        function handleShowPickList(api) {
            if (api === "Devise" || api === 'Personnel') {
                return '/api/' + api + '/SearchLight/?page={1}&societeId=' + $ctrl.initialCI.SocieteId;
            }
            return '/api/' + api + '/SearchLight/?page={1}';
        }

        /*
         * @function cloneAll(source,target)
         * @description Recopie complète d'un objet
         * @param {any} source 
         * @param {any} target
         */
        function cloneAll(source, target) {
            for (var property in source) {
                if (source.hasOwnProperty(property)) {
                    target[property] = angular.copy(source[property]);
                }
            }
        }

        /*
         * @function cloneAll(source, target, properties)
         * @description Recopie partielle d'un objet
         * @param {any} source 
         * @param {any} target
         * @param {any} properties
         */
        function clonePartial(source, target, properties) {
            for (var propertyKey in properties) {
                if (properties.hasOwnProperty(propertyKey)) {
                    var property = properties[propertyKey];
                    if (source.hasOwnProperty(property)) {
                        target[property] = angular.copy(source[property]);
                    }
                }
            }
        }

        /*
         * @function actionRestoreState(panel)
         */
        function actionRestoreState(panel) {
            switch (panel.id) {
                case $ctrl.panels.general.id: {
                    $ctrl.panels.general.data = [];
                    clonePartial($ctrl.initialCI, $ctrl.panels.general.data, $ctrl.generalProperties);
                    actionFormattingHours();

                    // Valider les horaires
                    handleHorairesValidation();
                    break;
                }
                case $ctrl.panels.facturation.id: {
                    $ctrl.panels.facturation.data = [];
                    clonePartial($ctrl.initialCI, $ctrl.panels.facturation.data, $ctrl.facturationProperties); break;
                }
                case $ctrl.panels.paie.id: {
                    $ctrl.panels.paie.data.address = [];
                    clonePartial($ctrl.initialCI, $ctrl.panels.paie.data.address, $ctrl.paieProperties);
                    cloneAll($ctrl.initialCICodeMajoration, $ctrl.panels.paie.data.codeMajorationList);
                    cloneAll($ctrl.initialCIPrime, $ctrl.panels.paie.data.codePrimeList);
                    break;
                }
                case $ctrl.panels.devise.id: {
                    $ctrl.panels.devise.data.ciDeviseList = [];
                    cloneAll($ctrl.initialCIDevise, $ctrl.panels.devise.data.ciDeviseList);
                    var ciDeviseRef = $filter('filter')($ctrl.panels.devise.data.ciDeviseList, { Reference: true }, true)[0];
                    var ciDeviseSec = $filter('filter')($ctrl.panels.devise.data.ciDeviseList, { Reference: false }, true);

                    if (ciDeviseRef) {
                        $ctrl.panels.devise.data.reference = ciDeviseRef;
                    }
                    if (ciDeviseSec) {
                        $ctrl.panels.devise.data.secondaireList = ciDeviseSec;
                    }
                    angular.forEach($ctrl.panels.devise.data.ciDeviseList, function (val) {
                        val.typeDevise = val.Reference ? $ctrl.resources.CI_Controller_Reference : $ctrl.resources.CI_Controller_Secondaires;
                    });
                    break;
                }
                case $ctrl.panels.carburant.id: {
                    $ctrl.panels.carburant.data.modifiedParametrageCarburantList = [];
                    $ctrl.panels.carburant.data.modifiedCIRessourceList = [];
                    $ctrl.panels.carburant.data.ciRessourceList = [];
                    $ctrl.panels.carburant.data.parametrageCarburantList = [];
                    cloneAll($ctrl.initialCIRessource, $ctrl.panels.carburant.data.ciRessourceList);
                    cloneAll($ctrl.initialParametrageCarburant, $ctrl.panels.carburant.data.parametrageCarburantList);
                    clonePartial($ctrl.initialCI, $ctrl.panels.general.data, $ctrl.carburantProperties);
                    $ctrl.panels.carburant.data.CarburantActif = $ctrl.initialCI.CarburantActif;
                    break;
                }
                case $ctrl.panels.team.id: {
                    $ctrl.panels.team.data.calendarAffectations = {};
                    $ctrl.panels.team.data.modifiedAffectationList = [];
                    $ctrl.panels.team.data.deletedAffectationList = [];
                    $ctrl.panels.team.data.teamMembers = [];
                    cloneAll($ctrl.initialCalendarAffectations, $ctrl.panels.team.data.calendarAffectations);
                    cloneAll($ctrl.initialTeamMembers, $ctrl.panels.team.data.teamMembers);
                    break;
                }
                default: break;
            }
        }

        /*
         * @function actionGetState(panelId)
         */
        function actionGetState(panel) {
            switch (panel.id) {
                case $ctrl.panels.general.id: { clonePartial($ctrl.panels.general.data, $ctrl.initialCI, $ctrl.generalProperties); break; }
                case $ctrl.panels.facturation.id: { clonePartial($ctrl.panels.facturation.data, $ctrl.initialCI, $ctrl.facturationProperties); break; }
                case $ctrl.panels.paie.id: { clonePartial($ctrl.panels.paie.data.address, $ctrl.initialCI, $ctrl.paieProperties); break; }
                case $ctrl.panels.devise.id: { break; }
                case $ctrl.panels.carburant.id: { clonePartial($ctrl.panels.general.data, $ctrl.initialCI, $ctrl.carburantProperties); break; } // Pour mise à jour du champ CarburantActif
                case $ctrl.panels.team.id: {
                    $ctrl.initialCalendarAffectations = {};
                    $ctrl.initialTeamMembers = [];
                    cloneAll($ctrl.panels.team.data.calendarAffectations, $ctrl.initialCalendarAffectations);
                    cloneAll($ctrl.panels.team.data.teamMembers, $ctrl.initialTeamMembers);
                    break;
                }
                default: break;
            }
        }

        /*
         * @function handleSave(panel)
         */
        function handleSave(panel) { actionSave(panel); }

        /*
         * @function handleCancel(panel)
         */
        function handleCancel(panel) { actionCancel(panel); }

        /*
         * @function actionUpdate(panel)
         * @description Action de mise à jour d'une section
         */
        function actionUpdate(panel) {
            switch (panel.id) {
                case $ctrl.panels.general.id: { actionUpdateCi(); break; }
                case $ctrl.panels.facturation.id: { actionUpdateFacturation(); break; }
                case $ctrl.panels.paie.id: { actionUpdatePaie(); break; }
                case $ctrl.panels.devise.id: { actionUpdateDevise(); break; }
                case $ctrl.panels.carburant.id: { actionUpdateCarburant(); break; }
                case $ctrl.panels.team.id: { actionUpdateTeam(); break; }
                default: console.log("Le panel à mettre à jour n'est pas défini"); break;
            }
        }

        /*
         * @function actionSave(panel)
         * @description Action de sauvegarde générique
         */
        function actionSave(panel) {
            actionGetState(panel);
            actionUpdate(panel);
            // Referme le panneau à l'enregistrement
            angular.element("#panel-heading-" + panel.id).addClass('collapsed');
            switch (panel.id) {
                case $ctrl.panels.general.id:
                    if (!$ctrl.formParamGeneral.$invalid) {
                        angular.element("#collapse-" + panel.id).removeClass('in');
                    }
                    break;
                case $ctrl.panels.facturation.id:
                    if (!$ctrl.formModuleCommande.$invalid) {
                        angular.element("#collapse-" + panel.id).removeClass('in');
                    }
                    break;
                default:
                    angular.element("#collapse-" + panel.id).removeClass('in');
            }
        }

        /*
         * @function actionCancel(panel)
         * @description Action de d'annulation générique
         */
        function actionCancel(panel) {
            confirmDialog.confirm($ctrl.resources, $ctrl.resources.Global_Modal_ConfirmationAnnulation)
                .then(function () {
                    actionRestoreState(panel);
                });
        }

        /*
         * @function actionFormattingDates() 
         * @description Convertit les dates UTC issues du serveur en date Locales
         * @et convertit les date au format HHMM
         */
        function actionFormattingDates() {
            $ctrl.initialCI.DateOuverture = $filter('toLocaleDate')($ctrl.initialCI.DateOuverture);
            $ctrl.initialCI.DateFermeture = $filter('toLocaleDate')($ctrl.initialCI.DateFermeture);
            actionFormattingHours();
        }

        /*
         * @function actionFormattingHours()
         * @description Convertit les heures UTC issues du serveur en heures Locales
         */
        function actionFormattingHours() {
            $ctrl.HoraireDebutM = convertDateToHHMMFormat($filter('toLocaleDate')($ctrl.initialCI.HoraireDebutM));
            $ctrl.HoraireFinM = convertDateToHHMMFormat($filter('toLocaleDate')($ctrl.initialCI.HoraireFinM));
            $ctrl.HoraireDebutS = convertDateToHHMMFormat($filter('toLocaleDate')($ctrl.initialCI.HoraireDebutS));
            $ctrl.HoraireFinS = convertDateToHHMMFormat($filter('toLocaleDate')($ctrl.initialCI.HoraireFinS));
        }

        /*
         * @convertit les horaires du format HHMM vers le type Date
         */
        function handleHorairesChantier() {
            $ctrl.initialCI.HoraireDebutM = convertHHMMToDateFormat($ctrl.HoraireDebutM);
            $ctrl.initialCI.HoraireFinM = convertHHMMToDateFormat($ctrl.HoraireFinM);
            $ctrl.initialCI.HoraireDebutS = convertHHMMToDateFormat($ctrl.HoraireDebutS);
            $ctrl.initialCI.HoraireFinS = convertHHMMToDateFormat($ctrl.HoraireFinS);
        }

        function convertDateToHHMMFormat(date) {
            if (date && angular.isDate(date)) {
                var heures = ("0" + date.getHours()).slice(-2);
                var minutes = ("0" + date.getMinutes()).slice(-2);
                return heures + minutes;
            }
            else {
                return null;
            }
        }

        function convertHHMMToDateFormat(time) {
            if (time) {
                var newDate = new Date();
                var hour = time.substr(0, 2);
                var minutes = time.substr(2, 4);
                newDate.setHours(hour);
                newDate.setMinutes(minutes);
                newDate.setSeconds(0);
                return newDate;
            }
            else {
                return null;
            }
        }

        /*
         * @description Fonction de vérification de la cohérence des dates saisies (début avant fin)
         */
        function handleDateValidation() {
            if ($ctrl.panels.general.data !== null) {
                var valid = $ctrl.panels.general.data.DateOuverture && $ctrl.panels.general.data.DateFermeture ? $ctrl.panels.general.data.DateFermeture >= $ctrl.panels.general.data.DateOuverture : true;
                $ctrl.formParamGeneral.DateOuverture.$setValidity("datesNotOk", valid);
                // Affiche l'erreur : force le rafraichissement 
                $timeout(angular.noop);
            }
        }

        /*
         * @description Fonction de vérification de la cohérence des tranches horaires saisies
         */
        function handleHorairesValidation() {
            if ($ctrl.panels.general.data !== null) {

                if (FeatureFlags.getFlagStatus('RapportsHorairesObligatoires')) {
                    var trancheMatinValide = isNullOrEmpty($ctrl.HoraireDebutM) && isNullOrEmpty($ctrl.HoraireFinM) || !isNullOrEmpty($ctrl.HoraireDebutM) && !isNullOrEmpty($ctrl.HoraireFinM);
                    var trancheSoirValide = isNullOrEmpty($ctrl.HoraireDebutS) && isNullOrEmpty($ctrl.HoraireFinS) || !isNullOrEmpty($ctrl.HoraireDebutS) && !isNullOrEmpty($ctrl.HoraireFinS);
                    var tranchesValide = !isNullOrEmpty($ctrl.HoraireDebutS) ? !isNullOrEmpty($ctrl.HoraireDebutM) : true;
                    $ctrl.formParamGeneral.$setValidity("trancheNotOk", trancheMatinValide && trancheSoirValide);
                    $ctrl.formParamGeneral.$setValidity("premiereTrancheManquante", tranchesValide);
                    // Affiche l'erreur : force le rafraichissement 
                    $timeout(angular.noop);
                }
            }
        }

        /*
         * @function handlePickListSelection(type, item)
         * @description Gestion de la selection d'un item dans une PickList 
         */
        function handlePickListSelection(type, item) {
            var ciDeviseModel;
            switch (type) {
                case "ResponsableAdministratif": { $ctrl.panels.general.data.ResponsableAdministratifId = item.IdRef; break; }
                case "ResponsableChantier": {
                    $ctrl.panels.general.data.ResponsableChantier = item.PrenomNom;
                    $ctrl.panels.general.data.ResponsableChantierId = item.IdRef; break;
                }
                case "DeviseRef": {
                    if (actionIsDeviseSecondaire(item)) {
                        $ctrl.panels.devise.data.reference.DeviseId = null;
                        $ctrl.panels.devise.data.reference.Devise = null;
                        Notify.error(resources.CI_Controller_ErrorMetier_DeviseDejaSecondaire);
                        return;
                    }
                    else {
                        $ctrl.panels.devise.data.reference.DeviseId = item.DeviseId;
                        $ctrl.panels.devise.data.reference.Devise = item;
                        var deviseRef = $filter('filter')($ctrl.panels.devise.data.ciDeviseList, { Reference: true }, true)[0];
                        if (!deviseRef) {
                            ciDeviseModel = { CiId: $ctrl.ciId, DeviseId: item.DeviseId, Devise: item, Reference: true, typeDevise: $ctrl.resources.CI_Controller_Reference };
                            $ctrl.panels.devise.data.ciDeviseList.push(ciDeviseModel);
                        }
                    }
                    break;
                }
                case "DeviseSec":
                    {
                        // Check devise not already exist in list
                        ciDeviseModel = { CiId: $ctrl.ciId, DeviseId: item.DeviseId, Devise: item, Reference: false, typeDevise: $ctrl.resources.CI_Controller_Secondaires };
                        if (actionCheckExistDeviseSec(ciDeviseModel)) { Notify.error(resources.CI_Controller_ErrorMetier_DoubleSelectionDevise); return; }
                        else if ($ctrl.panels.devise.data.secondaireList.length >= 4) { Notify.error(resources.CI_Controller_ErrorMetier_NbrMaxDevise); return; }
                        else if (actionIsDeviseReference(ciDeviseModel)) { Notify.error(resources.CI_Controller_ErrorMetier_DeviseDejaReference); return; }
                        else {
                            $ctrl.panels.devise.data.secondaireList.push(ciDeviseModel);
                            $ctrl.panels.devise.data.ciDeviseList.push(ciDeviseModel);
                            break;
                        }
                    }
                case 'CompteInterneSep':
                    {
                        $ctrl.panels.general.data.CompteInterneSepId = item.IdRef;
                        break;
                    }
                default: break;
            }
        }

        /*
         * @function handlePickListDeletion(type)
         * @description Gestion de la suppression de l'item sélectionné d'une PickList 
         */
        function handlePickListDeletion(type) {
            switch (type) {
                case "ResponsableAdministratif": { $ctrl.panels.general.data.ResponsableAdministratif = null; $ctrl.panels.general.data.ResponsableAdministratifId = null; break; }
                case "ResponsableChantier": {
                    $ctrl.panels.general.data.PersonnelResponsableChantier = null;
                    $ctrl.panels.general.data.ResponsableChantier = null;
                    $ctrl.panels.general.data.ResponsableChantierId = null; break;
                }
                case "DeviseRef": { $ctrl.panels.devise.data.reference.Devise = null; $ctrl.panels.devise.data.reference.DeviseId = null; break; }
                case 'CompteInterneSep':
                    {
                        $ctrl.panels.general.data.CompteInterneSepId = null;
                        $ctrl.panels.general.data.CompteInterneSep = null;
                        break;
                    }
                default: break;
            }
        }

        /* @function initGoogleMap
         * @description Initialise la carte Google Map     
         */
        function initGoogleMap() {
            NgMap.getMap("map").then(function (map) {
                $ctrl.map = map;
                $ctrl.map.setOptions({ disableDoubleClickZoom: true });

                GoogleService.addSelectPositionButton($ctrl.resources, $ctrl.map, $ctrl.mapSelector, $ctrl.handleSelectLocation);

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

        /* @function toPersonnel(item)
         * @description Initialise un nouveau objet affecation en se basant sur les informations du personnel   
         */
        function toPersonnel(item) {
            if (!item) {
                return null;
            }

            var personnel = {
                AffectationId: 0,
                PersonnelId: item.PersonnelId,
                Nom: item.Nom,
                Prenom: item.Prenom,
                Statut: toPersonnelStatutLibelle(item.Statut),
                Matricule: item.Matricule,
                CodeSociete: item.Societe.Code,
                IsInFavoriteTeam: $ctrl.panels.team.data.teamMembers.some(function (e) { return e.PersonnelId === item.PersonnelId; }),
                IsDelegate: false,
                Astreintes: []
            };

            [0, 1, 2, 3, 4, 5, 6].forEach(function (dayOfWeek) {
                var astreinte = {
                    DayOfWeek: dayOfWeek,
                    IsAstreinte: false,
                    AstreinteId: 0,
                    IsModified: false
                };
                personnel.Astreintes.push(astreinte);
            });

            return personnel;
        }

        /* @function Convert un statut du personnel venant de la base de données à un Libelle
         * @description Assure la conversion d'un statut en libelle. 
         * Les différentes valeurs de la conversion sont issues de la classe : TypePersonnel
         */
        function toPersonnelStatutLibelle(statut) {
            var libelle = '';
            switch (statut) {
                case "1":
                    libelle = Enums.EnumPersonnelStatus.Ouvrier.Label;
                    break;
                case "2":
                    libelle = Enums.EnumPersonnelStatus.Etam.Label;
                    break;
                case "3":
                    libelle = Enums.EnumPersonnelStatus.Cadre.Label;
                    break;
                case "4":
                    libelle = Enums.EnumPersonnelStatus.Etam.Label;
                    break;
                case "5":
                    libelle = Enums.EnumPersonnelStatus.Etam.Label;
                    break;
                default:
                    break;
            }

            return libelle;
        }

        /*
         *@description Gestion du progress bar lors du fin d'enregistrement  et activation des actions (partie_team)
         */
        function actionSaveLoadEnd() {
            $q.when()
                .then(ProgressBar.complete)
                .then(disableSaveWhenLoad(false));
        }

        /*
         *@description Gestion du progress bar lors du début d'enregistrement  et désactivation des actions (partie_team)
         */
        function actionSaveLoadStart() {
            $q.when()
                .then(ProgressBar.start)
                .then(disableSaveWhenLoad(true));
        }

        /*
         *@description Activation ou désactivation des actions (partie_team)
         */
        function disableSaveWhenLoad(disable) {
            if (disable) {
                $ctrl.disableSaveWhenLoad = disable;
            }
            else {
                $timeout(function () { $ctrl.disableSaveWhenLoad = disable; }, 4000);
            }
        }

        function SaveMassage() {
            Notify.message(resources.Global_Notification_Enregistrement_Success);
        }

        function actionIsRolePaie() {
            //Initialisation de variables indiquant l'appartenance de l'utilisateur à des rôles concernant la paie
            CIService.IsRolePaie($ctrl.initialCI.CiId).$promise
                .then(function (response) {
                    $ctrl.IsRoleGSP = response.value;
                })
                .catch(Notify.defaultError);
        }

        /*
         * @function handleToggleManagingPointage
         * @description Gérer l'evenement activer / désactiver la gestion des pointages
         */
        function handleToggleManagingPointage() {
            // Keeping the old value unless the user confirm the dialog
            $ctrl.disablePointage = !$ctrl.panels.team.data.calendarAffectations.IsDisableForPointage;
        }
    }
}(angular));