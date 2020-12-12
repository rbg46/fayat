
(function (angular) {
    'use strict';

    angular.module('Fred').controller("RoleController", RoleController);

    RoleController.$inject = ['$scope', '$timeout', 'Notify', 'RoleService', 'RoleModalService', 'confirmDialog', 'RoleStateManagerService', 'ProgressBar', '$q', 'authorizationService', 'fonctionnaliteModeService'];

    function RoleController($scope, $timeout, Notify, RoleService, RoleModalService, confirmDialog, RoleStateManagerService, ProgressBar, $q, authorizationService, fonctionnaliteModeService) {

        /* -------------------------------------------------------------------------------------------------------------
         *                                            INIT
         * -------------------------------------------------------------------------------------------------------------
         */


        //Initialisation de ressources
        $scope.resources = resources;

        $scope.deviseRef = {};
        $scope.selectedRef = {};

        $scope.searchRole = "";
        $scope.roleList = [];
        $scope.selectedRole = null;

        $scope.searchFeature = "";
        $scope.roleFeatures = [];
        $scope.roleFeatureSelected = null;


        $scope.searchSeuilValidation = "";
        $scope.seuilValidationList = [];
        $scope.selectedSeuil = null;



        $scope.societeSelected = null;
        $scope.showMessageNoRoleForSociete = false;
        $scope.stateModel = null;

        $scope.showInactifsRoles = false;

        $scope.isChangingMode = false;//flag pour savoir si nous somme en train de changé le mode d'une fonctionnalité.

        /* -------------------------------------------------------------------------------------------------------------
         *                                            INIT
         * -------------------------------------------------------------------------------------------------------------
         */


        $scope.init = function () {
            $scope.modes = fonctionnaliteModeService.getModes();
            $scope.stateModel = RoleStateManagerService.setState('initial');
            var currentUserIsNotSuperAdmin = !authorizationService.currentUserIsSuperAdmin();
            if (currentUserIsNotSuperAdmin) {
                var currentUserSocieteId = authorizationService.currentUserSocieteId();
                $scope.onSelectSocieteLookup({ SocieteId: currentUserSocieteId });
            }
        };

        /* -------------------------------------------------------------------------------------------------------------
         *                                            HANDLERS
         * -------------------------------------------------------------------------------------------------------------
         */

        $scope.handleDuplicateRole = handleDuplicateRole;



        $scope.handleDeleteSeuilValidation = actionDeleteSeuilValidation;

        $scope.handleAddRole = RoleModalService.openCreateModal;
        $scope.handleEditRole = RoleModalService.openEditModal;


        /* -------------------------------------------------------------------------------------------------------------
        *                                            GESTION DU LOOKUP SOCIETE
        * -------------------------------------------------------------------------------------------------------------
        */



        $scope.onSelectSocieteLookup = function (societe) {

            //nettoyage des listes
            clearAll();

            ProgressBar.start();
            $scope.stateModel = RoleStateManagerService.setState('busy', $scope.stateModel);
            $scope.societeSelected = societe;
            RoleService.getRolesBySocieteId(societe.SocieteId)
                .then(getRolesBySocieteIdSucessed)
                .catch(getRolesBySocieteIdFail)
                .finally(getRolesBySocieteIdFinally);
        };

        function getRolesBySocieteIdSucessed(response) {
            $scope.roleList = response.data;
            if ($scope.roleList.length > 0) {
                $scope.stateModel = RoleStateManagerService.setState('showList');
            } else {
                $scope.stateModel = RoleStateManagerService.setState('showDashBord');
            }
        }

        function getRolesBySocieteIdFail(error) {
            Notify.error(error.data.Message);
        }

        function getRolesBySocieteIdFinally() {
            ProgressBar.complete();
            $scope.stateModel = RoleStateManagerService.setState('notBusy', $scope.stateModel);
        }

        $scope.onDeleteSocieteLookup = function () {
            $scope.societeSelected = null;
            clearAll();
        };

        /* -------------------------------------------------------------------------------------------------------------
        *                                            GESTION DU BUTTON PRIVILEGE
        * -------------------------------------------------------------------------------------------------------------
        */
        $scope.modeChange = function (roleFeature, currentValue) {
            if ($scope.isChangingMode) {
                return;
            }
            $scope.isChangingMode = true;
            ProgressBar.start();
            RoleService.changeMode(roleFeature.RoleFonctionnaliteId, currentValue)
                // De maniere a pouvoir faire un rollback en cas d'erreur, j'utilise le concept de closure.
                // Normalement ici on pointe vers une fonction. Par exemple : '.then(changeModeSuccess)'
                // La fonction 'changeModeSuccessFn' retourne un pointeur de fonction. cela permet de capturer le context (ici roleFeature)
                .then(changeModeSuccessFn(roleFeature))
                .catch(changeModeFailFn(roleFeature))
                .finally(changeModeFinally);
        };

        function changeModeSuccessFn(roleFeature) {
            // Ici roleFeature est capturé pour etre réutilisé lorsque le retour de l'appel serveur est un succé.
            // En effet, la valeur passé dans le component fredTreeStateToggleComponent (currentValue) 
            // est independante de roleFeature.Mode car elle est déclaré comme ceci => currentValue: '<'
            // En mettant currentValue: '=', le probleme aurait été de sauvegarder l'etat avant la modification et donc avant l'appel a la methode $scope.modeChange.
            return function (response) {
                //Il faut donc que je mette a jour mon object roleFeature 
                //je met a jour le mode, avec la valeur du serveur.
                roleFeature.Mode = response.data.Mode;
                Notify.message(resources.Global_Notification_Enregistrement_Success);
            };
        }

        function changeModeFailFn(roleFeature) {
            //ici roleFeature est capturé pour etre réutilisé lorsque le retour de l'appel serveur est un echec.
            return function (error) {
                // Ici l'astuce est de remettre l'ancienne valeur.
                // Pour cela, je change la valeur en la mettant a null.
                // Une bloucle du digest est effectuée et la valeur change.       
                var lastMode = roleFeature.Mode;
                roleFeature.Mode = null;
                $timeout(function () {
                    // Dans le $timeout, qui provoquera une autre boucle du digest, je remet l'ancienne valeur. 
                    // la valeur revient a l'etat initial. Tout cela est fait car il faut dire au component fredTreeStateToggleComponent de change de valeur.
                    // voir le cshtml : current-value="roleFeature.Mode"
                    roleFeature.Mode = lastMode;
                });
                Notify.error($scope.resources.Global_Notification_Error);
            };
        }

        function changeModeFinally() {
            $scope.isChangingMode = false;
            ProgressBar.complete();
        }

        /* -------------------------------------------------------------------------------------------------------------
       *                                          OUVERTURE POPUP DETAIL ROLE-FONCTIONNALITE
       * -------------------------------------------------------------------------------------------------------------
       */
        $scope.openRoleFonctionnaliteDetail = function (roleFonctionnaliteId) {
            RoleModalService.openRoleFonctionnaliteDetail(roleFonctionnaliteId);
        };

        /* -------------------------------------------------------------------------------------------------------------
        *                                           SELECTION D UN ROLE
        * -------------------------------------------------------------------------------------------------------------
        */

        $scope.handleSelectRole = function (selectedRole) {

            ProgressBar.start();

            clearRoleFonctionnalitesAndSeuils();

            $scope.selectedRole = selectedRole;

            var roleFonctionnalitesRequest = RoleService.GetRoleFonctionnalitesByRoleId(selectedRole.RoleId);

            var seuilValidationsRequest = RoleService.getSeuilValidations(selectedRole.RoleId);

            $q.all([roleFonctionnalitesRequest, seuilValidationsRequest])
                .then(selectedRoleSucess)
                .catch(selectedRoleFail)
                .finally(selectedRoleFinally);
        };

        function selectedRoleSucess(responses) {
            $scope.roleFeatures = responses[0].data;
            $scope.seuilValidationList = responses[1].data;
        }

        function selectedRoleFail() {
            Notify.error($scope.resources.Global_Notification_Error);
        }

        function selectedRoleFinally() {
            ProgressBar.complete();
        }

        /* -------------------------------------------------------------------------------------------------------------
        *                                            OUVERTURE MODAL MODIFICATION D UN ROLE
        * -------------------------------------------------------------------------------------------------------------
        */

        $scope.handleEditRole = function (role) {
            RoleModalService.openEditModal(role, $scope.societeSelected.SocieteId).then(function (roleUpdated) {
                if (!roleUpdated.Actif && !$scope.showInactifsRoles) {
                    clearRoleFonctionnalitesAndSeuils();
                    $scope.selectedRole = null;
                }
            });
        };


        /* -------------------------------------------------------------------------------------------------------------
       *                                            OUVERTURE MODAL AJOUT FONCTIONNALITE AU ROLE
       * -------------------------------------------------------------------------------------------------------------
       */
        $scope.canOpenAddRoleFeatureModal = function () {
            return $scope.selectedRole !== null;
        };

        $scope.openAddRoleFeatureModal = function () {
            RoleModalService.openAddRoleFeatureModal($scope.selectedRole.RoleId, $scope.societeSelected.SocieteId).then(function (response) {
                $scope.roleFeatures.push(response);
            });
        };

        /* -------------------------------------------------------------------------------------------------------------
        *                                           SELECTION D UNE FONCTIONNALITE
        * -------------------------------------------------------------------------------------------------------------
        */

        $scope.selectFeature = function (roleFeature) {
            $scope.roleFeatureSelected = roleFeature;
        };

        /* -------------------------------------------------------------------------------------------------------------
        *                                           SUPRESSION D UNE ROLE-FONCTIONNALITE
        * -------------------------------------------------------------------------------------------------------------
        */
        $scope.deleteFeature = function (roleFonctionnalite) {

            confirmDialog.confirm($scope.resources, resources.Global_Modal_ConfirmationSuppression).then(function () {

                RoleService.deleteRoleFonctionnaliteById(roleFonctionnalite.RoleFonctionnaliteId).then(function () {
                    var index = $scope.roleFeatures.indexOf(roleFonctionnalite);
                    $scope.roleFeatures.splice(index, 1);

                    Notify.message(resources.Global_Notification_Suppression_Success);
                }).catch(function (error) { Notify.error(error.data.Message); });
            });
        };



        /* -------------------------------------------------------------------------------------------------------------
        *                                            GESTION DES ETAS DE LA PAGE 
        * -------------------------------------------------------------------------------------------------------------
        */


        $scope.clickTileCreateManually = function () {
            $scope.stateModel = RoleStateManagerService.setState('selectTileCreateManuallyState', $scope.stateModel);
        };

        $scope.clickTileCopy = function () {
            $scope.societeSelectedForDuplication = null;
            $scope.stateModel = RoleStateManagerService.setState('selectTileCopyState', $scope.stateModel);
        };

        $scope.clickTileCompany = function () {
            $scope.stateModel = RoleStateManagerService.setState('selectTileCompanyState', $scope.stateModel);
        };

        $scope.clickTileExportFonctionnalites = function () {
            $scope.stateModel = RoleStateManagerService.setState('toogleTileExportFonctionnalitesState', $scope.stateModel);
        };

        $scope.clickTileExportSeuils = function () {
            $scope.stateModel = RoleStateManagerService.setState('toogleTileExportSeuilsState', $scope.stateModel);
        };

        $scope.clickTileValidate = function () {
            if ($scope.societeSelectedForDuplication && !$scope.stateModel.isBusy) {
                clonesRoles();
            }
            else {
                $scope.stateModel = RoleStateManagerService.setState('validateState', $scope.stateModel);
            }
        };

        /* -------------------------------------------------------------------------------------------------------------
        *                                           Duplication des roles sur dashboard
        * -------------------------------------------------------------------------------------------------------------
        */

        // #region Duplication des roles sur dashboard

        /*
         * Action executés lors de la selection d'une société dans le lookup situé dans le tile de la duplication
         */
        $scope.onSelectSocieteForDuplicationLookup = function (societe) {
            $scope.societeSelectedForDuplication = societe;
            $scope.stateModel = RoleStateManagerService.setState('companySelectedOnLookupState', $scope.stateModel);
        };

        /*
         * Action executée lors de la selection de la tuile 'validé'
         */
        function clonesRoles() {
            ProgressBar.start();

            $scope.stateModel = RoleStateManagerService.setState('busy', $scope.stateModel);

            var societeSourceId = $scope.societeSelectedForDuplication.SocieteId;
            var societeTargetId = $scope.societeSelected.SocieteId;
            var copyfeatures = $scope.stateModel.tileExportFonctionnalitesSelected;
            var copythreshold = $scope.stateModel.tileExportSeuilsSelected;


            RoleService.cloneRoles(societeSourceId, societeTargetId, copyfeatures, copythreshold)
                .then(cloneRoleSucessed)
                .catch(cloneRoleFail)
                .finally(cloneRoleFinally);
        }


        function cloneRoleSucessed(response) {
            $scope.roleList = response.data;
            $scope.stateModel = RoleStateManagerService.setState('validateState', $scope.stateModel);

            return RoleService.getRolesBySocieteId($scope.societeSelected.SocieteId)
                .then(function (response) {
                    $scope.roleList = response.data;
                    if ($scope.roleList.length > 0) {
                        $scope.stateModel = RoleStateManagerService.setState('showList');
                    }
                });

        }

        function cloneRoleFail(error) {
            Notify.error(error.data.Message);
        }

        function cloneRoleFinally() {
            ProgressBar.complete();
            $scope.stateModel = RoleStateManagerService.setState('notBusy', $scope.stateModel);
        }

        // #endregion

        /* -------------------------------------------------------------------------------------------------------------
         *                                            ROLES
         * -------------------------------------------------------------------------------------------------------------
         */

        $scope.roleFilter = function (showInactifsRoles) {
            return function (role) {
                if (showInactifsRoles) {
                    return true;
                }
                if (!role.Actif && !showInactifsRoles) {
                    return false;
                }
                return true;
            };
        };

        $scope.onClickShowInatifsRoles = function () {
            $scope.showInactifsRoles = true;
            clearTablesIfNecessary();

        };

        $scope.onClickShowActifsAndInatifsRoles = function () {
            $scope.showInactifsRoles = false;
            clearTablesIfNecessary();
        };

        function clearTablesIfNecessary() {
            //Je me sert du filtre sur les roles.
            // cette fonction retourne un fonction
            var roleFilterFn = $scope.roleFilter($scope.showInactifsRoles);

            if ($scope.selectedRole !== null && !roleFilterFn($scope.selectedRole)) {
                $scope.selectedRole = null;
                clearRoleFonctionnalitesAndSeuils();
            }
        }


        // Action Duplication d'un rôle
        function handleDuplicateRole(role, societeId) {
            RoleModalService.openDuplicateRoleModal(role, $scope.roleList, societeId);
        }

        /**
         * Suppression d'un rôle
         * @param {any} role Role to delete
         */
        $scope.handleDeleteRole = function (role) {

            confirmDialog.confirm($scope.resources, resources.Role_Index_FonctionnaliteSupprimer_Message).then(function () {
                RoleService.deleteRole(role.RoleId).then(function () {
                    var index = $scope.roleList.indexOf(role);
                    $scope.roleList.splice(index, 1);
                    $scope.selectedRole = null;
                    Notify.message(resources.Global_Notification_Suppression_Success);
                }).catch(function (error) {
                    Notify.error(error.data.Message);
                });
            });
        };

        /* -----------------------------------------------------------------------------------------------------------
        *                                          SEUILS
        * -------------------------------------------------------------------------------------------------------------
        */

        $scope.canAddSeuilValidation = function () {
            return $scope.selectedRole !== null;
        };

        //Ajout d'un seuil
        $scope.handleAddSeuilValidation = function (seuilValidationList, idSelectedModule, parentScope) {
            RoleModalService.openCreateSeuilValidationModal(seuilValidationList, $scope.selectedRole.RoleId, idSelectedModule, parentScope, $scope.selectedRole);
        };

        //edition d'un seuils
        $scope.handleEditSeuilValidation = function (seuilValidation, parentScope) {
            RoleModalService.openEditSeuilValidationModal(seuilValidation, parentScope, $scope.selectedRole);
        };

        // Action Suppresionn d'un seuil de validation associé à un rôle
        function actionDeleteSeuilValidation(seuilValidation) {
            confirmDialog.confirm($scope.resources, resources.Global_Modal_ConfirmationSuppression).then(function () {
                RoleService.deleteSeuilValidationById(seuilValidation.SeuilValidationId).then(function () {
                    var index = $scope.seuilValidationList.indexOf(seuilValidation);
                    $scope.seuilValidationList.splice(index, 1);
                    //$scope.idSelectedSeuilValidation = null;
                    $scope.selectedSeuil = null;
                    Notify.message(resources.Global_Notification_Suppression_Success);
                }).catch(function (error) { Notify.error(error.data.Message); });
            });
        }

        /* -------------------------------------------------------------------------------------------------------------
         *                                            SEUILS
         * -------------------------------------------------------------------------------------------------------------
         */
        // Action Sélection d'un seuil de validation
        $scope.handleSelectSeuilValidation = function (seuil) {
            //$scope.idSelectedSeuilValidation = seuil.SeuilValidationId;
            $scope.selectedSeuil = seuil;
        };


        /* -------------------------------------------------------------------------------------------------------------
       *                                            NETTOYAGE
       * -------------------------------------------------------------------------------------------------------------
       */

        function clearAll() {

            $scope.roleList = [];
            $scope.selectedRole = null;

            clearRoleFonctionnalitesAndSeuils();
        }

        function clearRoleFonctionnalitesAndSeuils() {

            $scope.roleFeatures = [];
            $scope.seuilValidationList = [];

            $scope.roleFeatureSelected = null;
            $scope.selectedSeuil = null;
        }
    }
})(angular);