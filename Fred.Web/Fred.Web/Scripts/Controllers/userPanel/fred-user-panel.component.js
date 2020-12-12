(function () {
    'use strict';

    angular
        .module('Fred')
        .component('fredUserPanel', {
            templateUrl: '/Scripts/Controllers/userPanel/fred-user-panel.html',
            bindings: {
                resources: '<',
                user: '<',
                photoProfil: '<',
                version: '<',
                isOpen: '<'
            },
            controller: 'fredUserPanelController'
        });

    angular.module('Fred').controller('fredUserPanelController', fredUserPanelController);

    fredUserPanelController.$inject = ['$scope', '$window', 'UserService', 'UtilisateurService', 'Notify'];

    function fredUserPanelController($scope, $window, UserService, UtilisateurService, Notify) {

        var $ctrl = this;
        $ctrl.isBusy = false;
        $ctrl.userOrganizationId = 0;

        //////////////////////////////////////////////////////////////////
        // Déclaration des fonctions publiques                          //
        //////////////////////////////////////////////////////////////////
        $ctrl.redirect = redirect;
        $ctrl.closePanel = closePanel;
        $ctrl.handleChangeModeMenu = handleChangeModeMenu;
        $ctrl.GetRightPersonnelManagement = GetRightPersonnelManagement;

        //////////////////////////////////////////////////////////////////
        // Init                                                         //
        //////////////////////////////////////////////////////////////////

        $ctrl.$onInit = function () {
            /*Récupération du mode d'affichage sélectionné par l'utilisateur*/
            $ctrl.ModeMenu = UserService.getModeMenu();
            $ctrl.resources = resources;
            GetUser();
        };

        $ctrl.$onChanges = function (changesObj) {
            if (changesObj.photoProfil && changesObj.photoProfil.currentValue) {
                $ctrl.photoProfil = changesObj.photoProfil.currentValue;
            }
        };

        //////////////////////////////////////////////////////////////////
        // Handlers                                                     //
        //////////////////////////////////////////////////////////////////

        function redirect() {
            $window.location = "/Authentication/Logout";
            sessionStorage.clear();
        }

        function GetUser() {
            UserService.getCurrentUser().then(function (utilisateur) {
                $ctrl.utilisateur = utilisateur;
                $ctrl.userOrganizationId = $ctrl.utilisateur.Personnel.Societe.Organisation.OrganisationId;
                GetAffection();
            }).catch(function (error) {
                Notify.error($ctrl.resources.Global_Notification_Error);
                console.log(error);
            });
        }

        function GetAffection() {
            UtilisateurService.GetAffectationSeuilUtilisateurByUtilisateurId($ctrl.utilisateur.UtilisateurId).then(function (val) {
                $ctrl.utilisateurRole = val.data;
            }).catch(function (error) {
                Notify.error($ctrl.resources.Global_Notification_Error);
                console.log(error);
            });
        }

        function GetRightPersonnelManagement() {
            UtilisateurService.GetRightPersonnelManagement($ctrl.utilisateurRole.RoleId).then(function (v) {
                $ctrl.droit = v.data;
            }).then(function () {
                ChoiceRedirection();
            }).catch(function (error) {
                Notify.error($ctrl.resources.Global_Notification_Error);
                console.log(error);
            });
        }

        function ChoiceRedirection() {
            if ($ctrl.droit.Mode === 2) {
                sessionStorage.setItem('personnelFilter', JSON.stringify({ 'IsInterne': true, 'IsUtilisateur': true }));
                sessionStorage.setItem('delegationActive', 1);
                $window.location = "/Personnel/Personnel";
            }
            else {
                sessionStorage.setItem('JustDelegationShow', 1);
                $window.location = "/Personnel/Personnel/Edit/" + $ctrl.utilisateur.UtilisateurId;
            }
        }

        function closePanel() {
            $scope.$emit('close.user.panel');
        }

        /*
         * @description Gestion du changement du mode de menu. Cette information est chargée dans les setting de l'utilisateur
         */
        function handleChangeModeMenu() {

            /**
             * récupération du paramètre actuellement en oeuvre.
             */
            var getModeMenu = UserService.getModeMenu();

            /* Inversion du changement*/
            if (getModeMenu === 'classic') {
                UserService.setModeMenu('modern');
            } else {
                UserService.setModeMenu('classic');
            }

            /*Passage de la nouvelle configuration pour propagation*/
            $ctrl.ModeMenu = UserService.getModeMenu();
        }
    }
})();