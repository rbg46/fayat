(function (angular) {
    'use strict';

    angular.module('Fred').controller('ReferentielEtenduController', ReferentielEtenduController);

    ReferentielEtenduController.$inject = ['Notify', 'ProgressBar', 'confirmDialog', 'ReferentielEtenduService', 'ReferentielEtenduFilterService', 'NatureService', '$q', '$filter', 'TypeSocieteService'];

    /**
     * Controller du Référentiel Etendu.
     *       
     * @param {any} Notify Notify
     * @param {any} ProgressBar ProgressBar
     * @param {any} confirmDialog confirmDialog 
     * @param {any} ReferentielEtenduService ReferentielEtenduService
     * @param {any} ReferentielEtenduFilterService ReferentielEtenduFilterService
     * @param {any} NatureService NatureService
     * @param {any} $q $q
     * @param {any} $filter $filter
     * @param {any} TypeSocieteService TypeSocieteService
     * @returns {CIController} $ctrl
     */
    function ReferentielEtenduController(Notify, ProgressBar, confirmDialog, ReferentielEtenduService, ReferentielEtenduFilterService, NatureService, $q, $filter, TypeSocieteService) {

        // assignation de la valeur du scope au controller pour les templates non mis à jour
        var $ctrl = this;

        // méthodes exposées
        angular.extend($ctrl, {
            handleLoadData: handleLoadData,
            handleSave: handleSave,
            handleCancel: handleCancel,
            handleExport: handleExport,
            handleAllocateNature: handleAllocateNature,
            handleDesallocateNature: handleDesallocateNature,
            handleSelectNature: handleSelectNature,
            handleChangeAchats: handleChangeAchats,
            handleShowLookupUnite: handleShowLookupUnite,
            handleRemoveUnite: handleRemoveUnite,
            handleAddUnite: handleAddUnite,
            handleOpenUnitesModal: handleOpenUnitesModal,
            handleCancelUnitesModal: handleCancelUnitesModal,
            handleValidateUnitesModal: handleValidateUnitesModal,
            checkAtLeastOneUnite: checkAtLeastOneUnite,

            handleChapitreChange: handleChapitreChange,
            handleSousChapitreChange: handleSousChapitreChange,
            handleRessourceChange: handleRessourceChange,

            handleCloseAll: handleCloseAll,
            handleOpenAll: handleOpenAll,

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

            });

            $ctrl.errorUnite.Message = $ctrl.resources.ReferentielEtendu_Index_UniteModal_Error;

            // Récupération et formattage des type société Interne pour paramétrer l'url de la lookup société            
            $ctrl.typeSocieteCodesParams = TypeSocieteService.GetQueryParamTypeSocieteCodes([TypeSocieteService.TypeSocieteCodes.INTERNE]);

            ProgressBar.complete();
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            HANDLERS
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @function handleSave()
         * @description Gestion de la sauvegarde des modifications du référentiel étendu
         */
        function handleSave() {
            if ($ctrl.params_LimitationUnitesRessource && checkUniteRessource()) {
                Notify.error("Toutes les ressources achats doivent être associées à au moins une unité.")
            }
            else {
                confirmDialog.confirm($ctrl.resources, $ctrl.resources.Global_Modal_ConfirmationEnregistrement).then(function () {

                    ProgressBar.start();

                    ReferentielEtenduService.manageReferentielEtenduList($ctrl.modifiedReferentielEtenduList)
                        .then(function (response) {

                            // Mise à jour des éléments modifiés ou ajoutés (ajout ou modification de nature)
                            angular.forEach(response.data, function (val, key) {
                                var ressource = $filter('filter')($ctrl.allRessourceList, { RessourceId: val.RessourceId }, true)[0];

                                if (ressource) {
                                    ressource.ReferentielEtenduId = val.ReferentielEtenduId;
                                    ressource.ReferentielEtendus[0].ReferentielEtenduId = val.ReferentielEtenduId;
                                    ressource.ReferentielEtendus[0].UniteReferentielEtendus = val.UniteReferentielEtendus;
                                }
                            });

                            // Mise à jour des éléments dont la nature a été supprimée
                            angular.forEach($ctrl.modifiedReferentielEtenduList, function (val, key) {
                                if (val.NatureId === null) {
                                    var ressource = $filter('filter')($ctrl.allRessourceList, { RessourceId: val.RessourceId }, true)[0];

                                    if (ressource) {
                                        ressource.ReferentielEtenduId = 0;
                                        ressource.ReferentielEtendus[0].ReferentielEtenduId = 0;
                                        ressource.ReferentielEtendus[0].NatureId = null;
                                        ressource.ReferentielEtendus[0].Nature = null;
                                        ressource.ReferentielEtendus[0].NatureCode = null;
                                        ressource.ReferentielEtendus[0].NatureLibelle = null;
                                    }
                                }
                            });

                            // Mise à jour de la liste initiale (pour le bouton "annulation des modifications")
                            $ctrl.initialReferentielEtenduList = angular.copy($ctrl.allChapitreList);

                            $ctrl.modifiedReferentielEtenduList = [];
                            Notify.message($ctrl.resources.Global_Notification_Enregistrement_Success);
                        })
                        .catch(Notify.defaultError)
                        .finally(ProgressBar.complete);
                });
            }
        }

        function checkUniteRessource() {
            var bool = false;
            angular.forEach($ctrl.referentielEtenduList, function (val, key) {
                if (val.CountUnitesRessourceToBeTreated > 0) {
                    bool = true;
                }
            })
            return bool;
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
         * @function handleExport()
         * @description Gestion de l'export d'un référentiel étendu
         */
        function handleExport() {
            var refEtenduList = [];
            ProgressBar.start();
            angular.forEach($ctrl.allRessourceList, function (val, key) {
                refEtenduList.push(val.ReferentielEtendus[0]);
            });

            ReferentielEtenduService.exportToExcel($ctrl.selectedSociete.SocieteId)
                .then(function (response) {
                    window.location.href = '/api/ReferentielEtendu/ExtractExcel/' + response.data.id;
                })
                .catch(Notify.defaultError)
                .finally(ProgressBar.complete);
        }

        /*
         * @function handleAllocateNature()
         * @description Alloue une nature à toutes les ressources sélectionnées
         */
        function handleAllocateNature() {
            var checkedRessourceList = $filter('filter')($ctrl.allRessourceList, { IsChecked: true }, true);

            if (checkedRessourceList !== null && checkedRessourceList.length > 0) {

                angular.forEach(checkedRessourceList, function (val, key) {
                    var sousChapitre = $filter('filter')($ctrl.allSousChapitreList, { SousChapitreId: val.SousChapitreId }, true)[0];
                    var chapitre = $filter('filter')($ctrl.allChapitreList, { ChapitreId: sousChapitre.ChapitreId }, true)[0];

                    val.ReferentielEtendus[0].NatureId = $ctrl.selectedNature.NatureId;
                    val.ReferentielEtendus[0].Nature = $ctrl.selectedNature;
                    val.ReferentielEtendus[0].SocieteId = $ctrl.selectedSociete.SocieteId;

                    val.IsChecked = false;
                    sousChapitre.IsChecked = false;
                    chapitre.IsChecked = false;

                    actionRecalculeRessourcesToBeTreated(chapitre, sousChapitre, true);
                    actionRecalculeUnitesRessourceToBeTreated(chapitre, sousChapitre);
                    actionAddRessourceToModifiedList(val);
                });
            }
        }

        function handleShowLookupUnite() {
            return '/api/Unite/SearchLight/';
        }

        function checkAtLeastOneUnite() {
            if ($ctrl.currentRessource && $ctrl.currentRessource.ReferentielEtendus[0]) {
                var list = $filter('filter')($ctrl.currentRessource.ReferentielEtendus[0].UniteReferentielEtendus, { IsDeleted: false }, true);
                return list.length > 0;
            }
            else {
                return false;
            }
        }

        function handleOpenUnitesModal(ressource) {
            $ctrl.initRessource = angular.copy(ressource);
            $ctrl.currentRessource = ressource;
            $('#UNITES_DIALOG_ID').modal();
        }

        function handleCancelUnitesModal(ressource) {
            $ctrl.currentRessource = angular.copy($ctrl.initRessource);
            $ctrl.initRessource = null;
            $('#UNITES_DIALOG_ID').modal('hide');
        }

        function handleValidateUnitesModal() {
            if (checkAtLeastOneUnite()) {
                actionAddRessourceToModifiedList($ctrl.currentRessource);
                computeLibelleUniteAbrege();
                var sousChapitre = $filter('filter')($ctrl.allSousChapitreList, { SousChapitreId: $ctrl.currentRessource.SousChapitreId }, true)[0];
                var chapitre = $filter('filter')($ctrl.allChapitreList, { ChapitreId: sousChapitre.ChapitreId }, true)[0];
                actionRecalculeUnitesRessourceToBeTreated(chapitre, sousChapitre);
                $ctrl.initRessource = null;
                $('#UNITES_DIALOG_ID').modal('hide');
            }
            else {
                $ctrl.errorUnite.Display = true;
            }
        }

        function computeLibelleUniteAbrege() {
            if ($ctrl.currentRessource && $ctrl.currentRessource.ReferentielEtendus[0]) {
                var list = $filter('filter')($ctrl.currentRessource.ReferentielEtendus[0].UniteReferentielEtendus, { IsDeleted: false }, true);
                var libelle = list[0].Unite.Code;
                if (list[1]) {
                    libelle += ", " + list[1].Unite.Code;
                }
                if (list[2]) {
                    libelle += ", " + list[2].Unite.Code;
                }
                if (list[3]) {
                    libelle += ", ...";
                }
            }
            else {
                libelle = null;
            }
            $ctrl.currentRessource.ReferentielEtendus[0].ListeUnitesAbregees = libelle;
        }

        function handleAddUnite(unite) {
            var referentielEtendu = $ctrl.currentRessource.ReferentielEtendus[0];
            if ($filter('filter')(referentielEtendu.UniteReferentielEtendus, { UniteId: unite.UniteId }, true).length > 0) {
                if ($filter('filter')(referentielEtendu.UniteReferentielEtendus, { UniteId: unite.UniteId }, true)[0].IsDeleted) {
                    referentielEtendu.UniteReferentielEtendus[key].IsDeleted = false;
                    $ctrl.errorUnite.Display = false;
                }
                else {
                    Notify.error($ctrl.resources.ReferentielEtendu_Index_Unite_Existe);
                }
            }
            else {
                var uniteReferentielEtendus = { UniteReferentielEtenduId: 0, ReferentielEtenduId: referentielEtendu.ReferentielEtenduId, Unite: unite, UniteId: unite.UniteId, IsDeleted: false };
                referentielEtendu.UniteReferentielEtendus.push(uniteReferentielEtendus);
                $ctrl.errorUnite.Display = false;
                actionAddRessourceToModifiedList($ctrl.currentRessource);
            }
        }

        function handleRemoveUnite(uniteId) {
            var referentielEtendu = $ctrl.currentRessource.ReferentielEtendus[0];
            referentielEtendu.UniteReferentielEtendus
            angular.forEach(referentielEtendu.UniteReferentielEtendus, function (val, key) {
                if (val.UniteId === uniteId) {
                    referentielEtendu.UniteReferentielEtendus[key].IsDeleted = true;
                    actionAddRessourceToModifiedList($ctrl.currentRessource);
                }
            });
        }

        /*
         * @function handleDesallocateNature(ressource) 
         * @description Gère la désallocation d'une nature aux ressources
         * @param {any} ressource
         */
        function handleDesallocateNature(ressource) {
            if (ressource) {
                actionDesallocateNature(ressource);
            }
            else {
                var checkedRessourceList = $filter('filter')($ctrl.allRessourceList, { IsChecked: true }, true);

                if (checkedRessourceList !== null && checkedRessourceList.length > 0) {

                    angular.forEach(checkedRessourceList, function (val, key) {
                        actionDesallocateNature(val);
                    });
                }
            }
        }

        /*
         * @function handleDesallocateNature(selectedNature)
         * @description Gère la sélection d'une nature
         */
        function handleSelectNature(selectedNature) {
            $ctrl.selectedNature = selectedNature;
        }

        /*
         * @function handleLoadData()
         * @description Chargement des données en fonction de la Société sélectionnée
         */
        function handleLoadData() {
            actionLoadData();
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

        function handleChangeAchats(ressource) {
            actionAddRessourceToModifiedList(ressource);
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

        /* -------------------------------------------------------------------------------------------------------------
         *                                            ACTIONS
         * -------------------------------------------------------------------------------------------------------------
         */

        /*
         * @function actionDesallocateNature(ressource)
         * @description Désalloue la nature associé à la ressource
         * @param {any} ressource
         */
        function actionDesallocateNature(ressource) {
            var sousChapitre = $filter('filter')($ctrl.allSousChapitreList, { SousChapitreId: ressource.SousChapitreId }, true)[0];
            var chapitre = $filter('filter')($ctrl.allChapitreList, { ChapitreId: sousChapitre.ChapitreId }, true)[0];

            ressource.ReferentielEtendus[0].NatureId = null;
            ressource.ReferentielEtendus[0].Nature = null;
            ressource.ReferentielEtendus[0].ListeUnitesAbregees = "";
            ressource.ReferentielEtendus[0].UniteReferentielEtendus = [];
            actionRecalculeUnitesRessourceToBeTreated(chapitre, sousChapitre);
            ressource.IsChecked = false;
            chapitre.IsChecked = false;
            sousChapitre.IsChecked = false;

            actionRecalculeRessourcesToBeTreated(chapitre, sousChapitre, false);
            actionAddRessourceToModifiedList(ressource);
        }

        /*
         * @function actionAddRessourceToModifiedList(ressource)
         * @description Ajoute une ressource à la liste des éléments modifiés     
         */
        function actionAddRessourceToModifiedList(ressource) {

            var alreadyModified = $filter('filter')($ctrl.modifiedReferentielEtenduList, { RessourceId: ressource.RessourceId }, true)[0];

            if (alreadyModified === undefined) {
                $ctrl.modifiedReferentielEtenduList.push(ressource.ReferentielEtendus[0]);
            }

            angular.forEach($ctrl.modifiedReferentielEtenduList, function (val, key) {
                if (val.NatureId === null && val.ReferentielEtenduId === 0) {
                    $ctrl.modifiedReferentielEtenduList.splice(key, 1);
                }
            });
        }

        /*
         * @function actionRecalculeRessourcesToBeTreated(chapitre, sousChapitre, allocate)
         * @description Recalcule le nombre de ressources à traiter par chapitre et sous chapitre    
         * @param {any} chapitre
         * @param {any} sousChapitre
         * @param {bool} allocate 
         */
        function actionRecalculeRessourcesToBeTreated(chapitre, sousChapitre, allocate) {

            var totalChapitre = 0;

            angular.forEach(chapitre.SousChapitres, function (val, key) {
                var checkedRessourceList = $filter('filter')(val.Ressources, { ReferentielEtendus: { NatureId: null } }, true);

                if (checkedRessourceList) {
                    val.CountRessourcesToBeTreated = checkedRessourceList.length;
                }
                totalChapitre += val.CountRessourcesToBeTreated;
            });

            chapitre.CountRessourcesToBeTreated = totalChapitre;
        }

        function actionRecalculeUnitesRessourceToBeTreated(chapitre, sousChapitre) {
            var totalChapitre = 0;

            angular.forEach(chapitre.SousChapitres, function (val, key) {
                var totalSousChapitre = 0;
                angular.forEach(val.Ressources, function (val2, key2) {

                    var refEtendu = val2.ReferentielEtendus[0];
                    if (refEtendu.Achats && refEtendu.NatureId && (refEtendu.UniteReferentielEtendus.length == 0 || ($filter('filter')(refEtendu.UniteReferentielEtendus, { IsDeleted: true }, true).length > refEtendu.UniteReferentielEtendus.length))) {
                        totalSousChapitre++;
                    }
                })
                val.CountUnitesRessourceToBeTreated = totalSousChapitre;
                totalChapitre += val.CountUnitesRessourceToBeTreated;

            });
            chapitre.CountUnitesRessourceToBeTreated = totalChapitre;
        }

        /*
         * @function actionLoadData()
         * @description Chargement des données en fonction de la Société sélectionnée
         */
        function actionLoadData() {
            $q.when()
                .then(ProgressBar.start)
                .then(isLimitationUnitesRessource)
                .then(actionGetReferentielEtenduList)
                .then(actionGetNatureList)
                .then(ProgressBar.complete);
        }

        /*
         * @function actionGetReferentielEtenduList()
         * @description Récupère le référentiel étendu de la société sélectionée
         */
        function actionGetReferentielEtenduList() {
            ReferentielEtenduService.get($ctrl.selectedSociete.SocieteId)
                .then(function (response) {
                    $ctrl.initialReferentielEtenduList = response.data;
                    if (response && response.length === 0) {
                        Notify.error(resources.Global_Notification_AucuneDonnees);
                    }
                    actionRestoreState();
                })
                .catch(Notify.defaultError);
        }

        /*
         * @function actionGetNatureList() 
         * @description Récupère la liste des natures de la société sélectionée
         */
        function actionGetNatureList() {
            return NatureService.GetNatureListBySocieteId($ctrl.selectedSociete.SocieteId)
                .then(function (response) {
                    $ctrl.natureList = response.data;
                })
                .catch(Notify.defaultError);
        }

        /*
         * @function actionRestoreState() 
         * @description Remet la liste du référentiel étendu à l'état initial (avec les données de base)
         */
        function actionRestoreState() {
            $ctrl.referentielEtenduList = [];
            $ctrl.allChapitreList = [];
            $ctrl.allSousChapitreList = [];
            $ctrl.allRessourceList = [];
            $ctrl.modifiedReferentielEtenduList = [];

            $ctrl.referentielEtenduList = angular.copy($ctrl.initialReferentielEtenduList);

            for (var i = 0; i < $ctrl.referentielEtenduList.length; i++) {
                $ctrl.allChapitreList.push($ctrl.referentielEtenduList[i]);
                for (var j = 0; j < $ctrl.referentielEtenduList[i].SousChapitres.length; j++) {
                    $ctrl.allSousChapitreList.push($ctrl.referentielEtenduList[i].SousChapitres[j]);
                    for (var k = 0; k < $ctrl.referentielEtenduList[i].SousChapitres[j].Ressources.length; k++) {
                        $ctrl.allRessourceList.push($ctrl.referentielEtenduList[i].SousChapitres[j].Ressources[k]);
                    }
                }
            }
        }

        function isLimitationUnitesRessource() {
            ReferentielEtenduService.isLimitationUnitesRessource($ctrl.selectedSociete.SocieteId)
                .then(function (response) {
                    $ctrl.params_LimitationUnitesRessource = response.data;
                })
                .catch(Notify.defaultError);
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            DIVERS
         * -------------------------------------------------------------------------------------------------------------
         */

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
    }
}(angular));