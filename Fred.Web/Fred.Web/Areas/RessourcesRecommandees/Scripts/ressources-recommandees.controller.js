(function (angular) {
    'use strict';

    angular.module('Fred').controller('RessourcesRecommandeesController', RessourcesRecommandeesController);

    RessourcesRecommandeesController.$inject = ['UserService', 'Notify', 'ProgressBar', 'confirmDialog', 'RessourcesRecommandeesService', 'ReferentielEtenduFilterService', '$q', '$filter', 'TypeSocieteService'];

    function RessourcesRecommandeesController(UserService, Notify, ProgressBar, confirmDialog, RessourcesRecommandeesService, ReferentielEtenduFilterService, $q, $filter, TypeSocieteService) {

        // assignation de la valeur du scope au controller pour les templates non mis à jour
        var $ctrl = this;

        // méthodes exposées
        angular.extend($ctrl, {
            handleLoadData: handleLoadData,
            handleCancel: handleCancel,
            handleLookupSelection: handleLookupSelection,
            handleLookupDeletion: handleLookupDeletion,

            handleChapitreChange: handleChapitreChange,
            handleSousChapitreChange: handleSousChapitreChange,
            handleRessourceChange: handleRessourceChange,

            handleCloseAll: handleCloseAll,
            handleOpenAll: handleOpenAll,
            handleRecommandee: handleRecommandee,
            handleSave: handleSave,

            // Filter Paramétrage des consommations des ressources (referentiel-etendu-filter.service.js)
            ressourcesFilter: ReferentielEtenduFilterService.ressourcesFilter,
            sousChapitresFilter: ReferentielEtenduFilterService.sousChapitresFilter,
            chapitresFilter: ReferentielEtenduFilterService.chapitresFilter,
            handlCheckboxRecommandee: handlCheckboxRecommandee
        });

        init();
        return $ctrl;

        /**
        * Initialisation du controller.     
        */
        function init() {
            ProgressBar.start();
            angular.extend($ctrl, {
                // Instanciation Objet Ressources
                resources: resources,

                selectedSociete: null,
                selectedNature: null,
                searchRessource: "",
                searchNature: "",
                initRessource: null,
                errorUnite: { Message: "", Display: false },

                initialReferentielEtenduList: [],
                params_LimitationUnitesRessource: false,
                referentielEtenduList: [],
                natureList: [],

                allChapitreList: [],
                allSousChapitreList: [],
                allRessourceList: [],
                modifiedReferentielEtenduList: [],
                ressourcesRecommandeesList: []
            });

            UserService.getCurrentUser().then(function(user) {
                $ctrl.currentUser = user.Personnel;
                $ctrl.SocieteId = $ctrl.currentUser.SocieteId;
            });

            $ctrl.errorUnite.Message = $ctrl.resources.ReferentielEtendu_Index_UniteModal_Error;

            // Récupération et formattage des type société Interne pour paramétrer l'url de la lookup société            
            $ctrl.typeSocieteCodesParams = TypeSocieteService.GetQueryParamTypeSocieteCodes([TypeSocieteService.TypeSocieteCodes.INTERNE]);

            ProgressBar.complete();
        }

        function handleLookupSelection(item) {
            actionLoadData();
        }

        function handleLookupDeletion() {
            $ctrl.Etablissement = null;
            clearData();
        }

        /*
      * @function handleLoadData()
      * @description Chargement des données en fonction de la Société sélectionnée
      */
        function handleLoadData() {
            actionLoadData();
        }

        /*
        * @function actionLoadData()
        * @description Chargement des données en fonction de la Société sélectionnée
        */
        function actionLoadData() {
            $q.when()
                .then(ProgressBar.start)
                .then(clearData)
                .then(actionGetReferentielEtenduList)
                .then(ProgressBar.complete)
                .catch(function (err) { Notify.error(resources.Global_Notification_Chargement_Error); });
        }

        /*
        * @function actionGetReferentielEtenduList()
        * @description Récupère le référentiel étendu de la société sélectionée
        */
        function actionGetReferentielEtenduList() {
            return RessourcesRecommandeesService.get($ctrl.Etablissement.SocieteId, $ctrl.Etablissement.Organisation.OrganisationId)
                .then(function (response) {
                    var dataTemp = response.data;
                    $ctrl.initialReferentielEtenduList = [];
                    for (var i = 0; i < dataTemp.length; i++) {
                        for (var j = 0; j < dataTemp[i].SousChapitres.length; j++) {
                            for (var k = 0; k < dataTemp[i].SousChapitres[j].Ressources.length; k++) {
                                if (dataTemp[i].SousChapitres[j].Ressources[k].ReferentielEtendus[0].ReferentielEtenduId && $ctrl.initialReferentielEtenduList.indexOf(dataTemp[i]) === -1) {
                                    $ctrl.initialReferentielEtenduList.push(dataTemp[i]);
                                }
                            }
                        }
                    }
                    if (response && response.length === 0) {
                        Notify.error(resources.Global_Notification_AucuneDonnees);
                    }
                    actionRestoreState();
                })
                .catch(Notify.defaultError);
        }
        /*
        * @function handleCancel()
        * @description Gestion de l'annulation des modifications du référentiel étendu
        */
        function handleCancel() {
            confirmDialog.confirm($ctrl.resources, $ctrl.resources.Global_Modal_ConfirmationAnnulation).then(function () {
                actionRestoreState();
            });
        }

        /*
        * @function clearData()
        * @description efface les données des tableaux d'affichage
        */
        function clearData() {
            $ctrl.referentielEtenduList = [];
            $ctrl.allChapitreList = [];
            $ctrl.allSousChapitreList = [];
            $ctrl.allRessourceList = [];
            $ctrl.modifiedReferentielEtenduList = [];
        }

        /*
        * @function actionRestoreState() 
        * @description Remet la liste du référentiel étendu à l'état initial (avec les données de base)
        */
        function actionRestoreState() {
            clearData();
            $ctrl.referentielEtenduList = angular.copy($ctrl.initialReferentielEtenduList);
        }

        /*
        * @function handleCloseAll() 
        * @description Ferme tous les panels
        */
        function handleCloseAll() {
            $('.collapse.in').collapse('hide');
        }

        /*
         * @function handleOpenAll() 
         * @description Ouvre tous les panels
         */
        function handleOpenAll() {
            $('.collapse').collapse('show');
        }

        function handleRecommandee(bool) {
            angular.forEach($ctrl.referentielEtenduList, function (chapitre) {
                angular.forEach(chapitre.SousChapitres, function (SousChapitre) {
                    angular.forEach(SousChapitre.Ressources, function (Ressource) {
                        if (bool !== undefined && Ressource.IsChecked) {
                            Ressource.IsRecommandee = bool;
                        }
                    });
                });
            });
        }

        function handlCheckboxRecommandee(ressource) {
            var refEtendu = $filter('filter')($ctrl.ressourcesRecommandeesList, { ReferentielEtenduId: ressource.ReferentielEtendus[0].ReferentielEtenduId }, true)[0];


            if (ressource.IsRecommandee) {

                if (!refEtendu) {
                    var obj = { ReferentielEtenduId: ressource.ReferentielEtendus[0].ReferentielEtenduId, OrganisationId: $ctrl.Etablissement.Organisation.OrganisationId, IsRecommandee: true };
                    $ctrl.ressourcesRecommandeesList.push(obj);
                }

            }
            else {
                if (refEtendu) {
                    var i = $ctrl.ressourcesRecommandeesList.indexOf(refEtendu);
                    if (i > -1) {
                        i.IsRecommandee = false;
                    }
                }

            }
        }

        /*
         * @function handleSousChapitreChange(ssChapitre, chapitre)
         * @description Gère la checkbox d'un Sous Chapitre
         */
        function handleSousChapitreChange(ssChapitre, chapitre) {
            var item_selected = ssChapitre.IsChecked;
            var uncheckedSousChapitres = 0;

            angular.forEach(ssChapitre.Ressources, function (item) {
                item.IsChecked = item_selected;
            });

            angular.forEach(chapitre.SousChapitres, function (item) {
                if (!item.IsChecked) {
                    uncheckedSousChapitres++;
                }
            });
            chapitre.IsChecked = (uncheckedSousChapitres === 0);
        }

        /*
       * @function handleRessourceChange(ressource, chapitre, sousChapitre) 
       * @description Gère la checkbox d'une Ressource
       */
        function handleRessourceChange(ressource, chapitre, sousChapitre) {
            var uncheckedRessources = 0;

            angular.forEach(sousChapitre.Ressources, function (item) {
                if (!item.IsChecked) {
                    uncheckedRessources++;
                }
            });
            sousChapitre.IsChecked = (uncheckedRessources === 0);

            var uncheckedSousChapitres = 0;

            angular.forEach(chapitre.SousChapitres, function (item) {
                if (!item.IsChecked) {
                    uncheckedSousChapitres++;
                }
            });

            chapitre.IsChecked = (uncheckedSousChapitres === 0);
        }

        /*
        * @function handleChapitreChange(chapitre)
        * @description Gère la checkbox d'un chapitre
        */
        function handleChapitreChange(chapitre) {
            angular.forEach(chapitre.SousChapitres, function (ssChap) {
                ssChap.IsChecked = chapitre.IsChecked;
                angular.forEach(ssChap.Ressources, function (ressource) {
                    ressource.IsChecked = chapitre.IsChecked;
                });
            });
        }

        function handleSave() {
            $ctrl.ressourcesRecommandeesList = [];
            angular.forEach($ctrl.referentielEtenduList, function (chapitre) {
                angular.forEach(chapitre.SousChapitres, function (SousChapitre) {
                    angular.forEach(SousChapitre.Ressources, function (Ressource) {
                        var referentielEtendu = Ressource.ReferentielEtendus[0];
                        var ressourcesRecommandee = referentielEtendu.RessourcesRecommandees ? referentielEtendu.RessourcesRecommandees[0] : null;

                        var obj = {
                            RessourceRecommandeeId: ressourcesRecommandee ? ressourcesRecommandee.RessourceRecommandeeId : 0,
                            ReferentielEtenduId: referentielEtendu.ReferentielEtenduId,
                            OrganisationId: $ctrl.Etablissement.Organisation.OrganisationId,
                            IsRecommandee: Ressource.IsRecommandee
                        };
                        if (obj.RessourceRecommandeeId === 0 && obj.IsRecommandee === true || obj.RessourceRecommandeeId !== 0 && Ressource.IsRecommandee === false) {
                            $ctrl.ressourcesRecommandeesList.push(obj);
                        }
                    });
                });
            });
            ProgressBar.start();
            RessourcesRecommandeesService.save($ctrl.ressourcesRecommandeesList)
                .then(function (response) {
                    var updatedRessources = response.data;
                    // mise à jour des données de la collection affichée avec les données updatées
                    angular.forEach(updatedRessources, function (updatedRessource) {
                        angular.forEach($ctrl.referentielEtenduList, function (chapitre) {
                            angular.forEach(chapitre.SousChapitres, function (sousChapitre) {
                                angular.forEach(sousChapitre.Ressources, function (ressource) {
                                    if (ressource.ReferentielEtendus && ressource.ReferentielEtendus[0].ReferentielEtenduId === updatedRessource.ReferentielEtenduId)
                                        if (updatedRessource.IsRecommandee) {
                                            ressource.ReferentielEtendus[0].RessourcesRecommandees = [updatedRessource];
                                        }
                                        else {
                                            ressource.ReferentielEtendus[0].RessourcesRecommandees = null;
                                            ressource.IsRecommandee = false;
                                        }
                                });
                            });
                        });
                    });
                    Notify.message(resources.Global_Notification_Enregistrement_Success);
                })
                .catch(function (err) { Notify.error(resources.Global_Notification_Error); })
                .finally(ProgressBar.complete);


        }
    }
}(angular));