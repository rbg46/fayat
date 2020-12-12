(function (angular) {
    'use strict';

    angular.module('Fred').controller('ModuleController', ModuleController);

    ModuleController.$inject = ['$scope',
        '$filter',
        'ModuleService',
        'ModuleModalService',
        'confirmDialog',
        'Notify',
        'ProgressBar',
        'ModulePermissionHelperService',
        '$q',
        'fredSubscribeService',
        'moduleVisibilityManagerService'];

    function ModuleController($scope,
        $filter,
        ModuleService,
        ModuleModalService,
        confirmDialog,
        Notify,
        ProgressBar,
        ModulePermissionHelperService,
        $q,
        fredSubscribeService,
        moduleVisibilityManagerService) {


        /* -------------------------------------------------------------------------------------------------------------
         *                                            INIT
         * -------------------------------------------------------------------------------------------------------------
         */


        // Instanciation Objet Ressources
        $scope.resources = resources;

        $scope.societeSelected = null;

        $scope.searchModule = "";
        $scope.moduleList = [];
        $scope.selectedModule = null;

        $scope.searchFeature = "";
        $scope.featureList = [];

        $scope.searchPermissionFunctionnalite = "";
        $scope.permissionFonctionnaliteList = [];
        $scope.selectedPermissionFonctionnaliteId = null;


        $scope.selectedModuleId = null;
        $scope.selectedFeatureId = null;
        $scope.selectedModule = {};
        $scope.selectedFeature = {};
        $scope.isBusy = false;

        $scope.arboIsOpen = false;

        var idsOfModulesPartiallyDisabled = [];
        var idsOfFonctionnalitesPartiallyDisabled = [];

        /* -------------------------------------------------------------------------------------------------------------
        *                                            INIT
        * -------------------------------------------------------------------------------------------------------------
        */

        $scope.init = function () {
            // lors de la sauvegarde de l'arbo, on demande a ce que les informations sur : 
            // actif/inactif des modules sur la societe 
            // partiellement actif/inactif  
            fredSubscribeService.subscribe({ eventName: 'module-reload-infos-inactives', callback: reloadInfosInactives });
        };


        /* -------------------------------------------------------------------------------------------------------------
        *                                            LOOKUP SOCIETE
        * -------------------------------------------------------------------------------------------------------------
        */
        $scope.onSelectSocieteLookup = function (societe) {
            if (!societe) {
                return;
            }
            ProgressBar.start();

            var moduleRequest = ModuleService.getModuleList();

            var moduleDesactivesRequest = ModuleService.getInactifModulesForSocieteId(societe.SocieteId);

            var modulesAndFonctionnalitesPartiallyDisabledRequest = ModuleService.getModulesAndFonctionnalitesPartiallyDisabled();

            $q.all([moduleRequest, moduleDesactivesRequest, modulesAndFonctionnalitesPartiallyDisabledRequest])
                .then(mergeResponse)
                .finally(function () {
                    ProgressBar.complete();
                });
        };

        function mergeResponse(responses) {
            var modules = responses[0].data;
            var moduleDesactivesIds = responses[1].data.map(function (moduleDesactive) {
                return moduleDesactive.ModuleId;
            });
            moduleVisibilityManagerService.markModuleDisabledOrEnableForSociete(modules, moduleDesactivesIds);

            var modulesAndFonctionnalitesPartiallyDisableds = responses[2].data;

            idsOfModulesPartiallyDisabled = modulesAndFonctionnalitesPartiallyDisableds.idsOfModulesPartiallyDisabled;

            idsOfFonctionnalitesPartiallyDisabled = modulesAndFonctionnalitesPartiallyDisableds.idsOfFonctionnalitesPartiallyDisabled;

            moduleVisibilityManagerService.markModulePartiallyDisabledOrEnable(modules, idsOfModulesPartiallyDisabled);

            moduleVisibilityManagerService.markFonctionnalitePartiallyDisabledOrEnable($scope.featureList, idsOfFonctionnalitesPartiallyDisabled);

            $scope.moduleList = modules;
        }


        function reloadInfosInactives() {
            $scope.onSelectSocieteLookup($scope.societeSelected);
        }


        /* -------------------------------------------------------------------------------------------------------------
         *                                            HANDLERS
         * -------------------------------------------------------------------------------------------------------------
         */

        // Handler Suppression d'un module
        $scope.handleDeleteModule = actionDeleteModule;

        // Handler Suppression d'une fonctionnalité
        $scope.handleDeleteFeature = actionDeleteFeature;


        // Handler Modal d'ajout d'une fonctionnalité
        $scope.handleCreateFeature = ModuleModalService.openCreateFeatureModal;

        // Handler Modal de modifier d'une fonctionnalité
        $scope.handleEditFeature = ModuleModalService.openEditFeatureModal;

        // Handler Sélection d'un module
        $scope.handleSelectModule = actionSelectModule;

        // Handler Sélection d'une fonctionnalité
        $scope.handleSelectFeature = actionSelectFeature;

        $scope.canDeletePermissionFonctionnalite = canDeletePermissionFonctionnalite;

        $scope.deletePermissionFonctionnalite = deletePermissionFonctionnalite;

        $scope.onSelectLookupPermission = onSelectLookupPermission;

        $scope.selectPermissionFonctionnaliteId = selectPermissionFonctionnaliteId;

        /* -------------------------------------------------------------------------------------------------------------
       *                                            MODALS MODULE
       * -------------------------------------------------------------------------------------------------------------
       */

        // Handler Modal d'ajout d'un Module
        $scope.handleCreateModule = function (moduleList) {
            ModuleModalService.openCreateModuleModal(moduleList, $scope.societeSelected.SocieteId).then(function (newModule) {
                // Sélectionne le module crée
                $scope.handleSelectModule(newModule);
                reloadInfosInactives();
            });
        };

        // Handler Modal de modification d'un Module
        $scope.handleEditModule = function (module) {
            ModuleModalService.openEditModuleModal(module, $scope.societeSelected.SocieteId).then(function (updatedModule) {
                // Sélectionne le module mis à jour
                $scope.handleSelectModule(updatedModule);
                reloadInfosInactives();
            });
        };

        /* -------------------------------------------------------------------------------------------------------------
         *                                            ACTIONS
         * -------------------------------------------------------------------------------------------------------------
         */

        // Action Sélectionner un module
        function actionSelectModule(module) {
            ProgressBar.start();
            $scope.selectedFeature = {};
            $scope.selectedFeatureId = null;
            $scope.selectedModule = module;
            $scope.selectedModuleId = module.ModuleId;

            ModuleService.getFeatureList($scope.selectedModuleId)
                .then(function (response) {
                    $scope.featureList = response.data;
                    moduleVisibilityManagerService.markFonctionnalitePartiallyDisabledOrEnable($scope.featureList, idsOfFonctionnalitesPartiallyDisabled);
                })
                .catch(function (error) {
                    Notify.error(error.data.Message);
                })
                .finally(function () {
                    ProgressBar.complete();
                });
        }

        // Action Sélectionner une fonctionnalité
        function actionSelectFeature(feature) {
            ProgressBar.start();
            var requestModuleInfo = ModuleService.getModuleById(feature.ModuleId)
                .then(function (response) {
                    $scope.selectedModule = response.data;
                    $scope.selectedModuleId = response.data.ModuleId;
                    $scope.featureList = response.data.Fonctionnalites;
                    moduleVisibilityManagerService.markFonctionnalitePartiallyDisabledOrEnable($scope.featureList, idsOfFonctionnalitesPartiallyDisabled);
                    $scope.selectedFeature = feature;
                    $scope.selectedFeatureId = feature.FonctionnaliteId;
                });

            var requestPermissions = ModuleService.getPermissionFonctionnalites(feature.FonctionnaliteId)
                .then(function (response) {
                    $scope.permissionFonctionnaliteList = [];
                    for (var i = 0; i < response.data.length; i++) {
                        var permissionFonctionnalite = response.data[i];
                        permissionFonctionnalite.Code = permissionFonctionnalite.Permission.Code;
                        permissionFonctionnalite.Libelle = permissionFonctionnalite.Permission.Libelle;
                        $scope.permissionFonctionnaliteList.push(permissionFonctionnalite);
                    }
                });

            $q.all([requestModuleInfo, requestPermissions])
                .catch(function (error) {
                    Notify.error(error.data.Message);
                }).finally(function () {
                    ProgressBar.complete();
                });
        }


        // Action Suppression d'un module
        function actionDeleteModule(moduleId) {
            confirmDialog.confirm($scope.resources, resources.Module_Index_Module_Suppression_Texte).then(function () {
                ModuleService.deleteModule(moduleId).then(function () {
                    $scope.featureList = [];
                    $scope.selectedModule = {};
                    $scope.selectedModuleId = null;
                    var i = $scope.moduleList.indexOf($filter('filter')($scope.moduleList, { ModuleId: moduleId }, true)[0]);
                    $scope.moduleList.splice(i, 1);
                    Notify.message(resources.Global_Notification_Suppression_Success);
                }).catch(onError);
            });
        }

        // Action Suppression d'une fonctionnalité
        function actionDeleteFeature(featureId) {
            confirmDialog.confirm($scope.resources, resources.Module_Index_Fonctionnalite_Suppression_Texte).then(function () {
                ModuleService.deleteFeature(featureId).then(function () {
                    $scope.selectedFeatureId = null;
                    $scope.selectedFeature = {};
                    var i = $scope.featureList.indexOf($filter('filter')($scope.featureList, { FonctionnaliteId: featureId }, true)[0]);
                    $scope.featureList.splice(i, 1);
                    Notify.message(resources.Global_Notification_Suppression_Success);
                }).catch(onError);
            });
        }

        function onError(error) {
            Notify.error(error.data.ModelState.Exist[0]);
        }




        /* -------------------------------------------------------------------------------------------------------------
        *                                            PERMISSION_FONCTIONNALITE
        * -------------------------------------------------------------------------------------------------------------
        */

        /*
         * SELECTION
         */
        function selectPermissionFonctionnaliteId(permissionFonctionnalite) {
            $scope.selectedPermissionFonctionnaliteId = permissionFonctionnalite.PermissionFonctionnaliteId;
        }


        /*
         * SUPPRESSION
         */
        function canDeletePermissionFonctionnalite(permissionFonctionnalite) {
            return ModulePermissionHelperService.canDeletePermissionFonctionnalite(permissionFonctionnalite, $scope.isBusy);
        }

        function deletePermissionFonctionnalite(permissionFonctionnaliteToDelete) {
            confirmDialog.confirm($scope.resources, resources.Module_Index_Permission_Suppression_Text).then(function () {
                $scope.isBusy = true;
                ProgressBar.start();
                ModulePermissionHelperService.deletePermissionFonctionnalite($scope.permissionFonctionnaliteList, permissionFonctionnaliteToDelete)
                    .finally(function () {
                        $scope.isBusy = false;
                        ProgressBar.complete();
                    });
            });
        }

        /*
        * AJOUT
        */


        function onSelectLookupPermission(permissionFonctionnalite) {

            ModulePermissionHelperService.canAddPermissionFonctionnalite($scope.permissionFonctionnaliteList, permissionFonctionnalite, $scope.isBusy)
                .then(function (response) {
                    if (response.canAdd) {
                        ModulePermissionHelperService.addPermissionFonctionnalite($scope.permissionFonctionnaliteList, permissionFonctionnalite, $scope.selectedFeature)
                            .finally(function () {
                                $scope.isBusy = false;
                                ProgressBar.complete();
                            });
                    } else {
                        Notify.error(resources.Module_Service_Notification_Ajout_Permission_impossible);
                        $scope.isBusy = false;
                        ProgressBar.complete();
                    }

                });
            $scope.isBusy = true;
            ProgressBar.start();
        }

        /* -------------------------------------------------------------------------------------------------------------
        *                                            OUVERTURE ARBO A PARTIR D UNE FONCTIONNALITE
        * -------------------------------------------------------------------------------------------------------------
        */
        $scope.openArboWithFeature = function (feature) {
            if ($scope.societeSelected) {
                fredSubscribeService.raiseEvent('module-open-arbo', {
                    societeId: $scope.societeSelected.SocieteId,
                    feature: feature,
                    module: null
                });
            }
        };

        /* -------------------------------------------------------------------------------------------------------------
        *                                            OUVERTURE ARBO A PARTIR D UN MODULE
        * -------------------------------------------------------------------------------------------------------------
        */
        $scope.openArboWithModule = function (module) {
            if ($scope.societeSelected) {
                fredSubscribeService.raiseEvent('module-open-arbo', {
                    societeId: $scope.societeSelected.SocieteId,
                    feature: null,
                    module: module
                });
            }
        };
    }
})(angular);