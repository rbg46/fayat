(function() {
    'use strict';

    angular
        .module('Fred')
        .component('fredApplicationHeader', {
            templateUrl: '/Scripts/Controllers/Header/fred-application-header.html',
            bindings: {
                resources: '<',
                user: '<',
                logo: '<',
                version: '<',
                modeMenu: '=',
                displayMenuModern: '='
            },
            controller: 'fredApplicationHeaderController'
        });

    angular.module('Fred').controller('fredApplicationHeaderController', fredApplicationHeaderController);

    fredApplicationHeaderController.$inject = ['$scope', '$rootScope', 'UserService', 'ServerNotificationsService', '$window', '$http', '$uibModal'];

    function fredApplicationHeaderController($scope, $rootScope, UserService, ServerNotificationsService, $window, $http, $uibModal) {

        var $ctrl = this;
        $ctrl.resources = resources;
        $ctrl.logoSrc = null;
        $ctrl.photoProfilSrc = null;
        $scope.permissionKeys = PERMISSION_KEYS;
        $ctrl.$rootScope = $rootScope;
        $ctrl.numberOfNotifications = 0;
        $ctrl.rafraichePeriode = 30000;
        $ctrl.notifications = null;
        $ctrl.notificationsToDelete = [];
        $ctrl.searchText = '';
        $ctrl.hub = null;

        //////////////////////////////////////////////////////////////////
        // Déclaration des fonctions publiques                          //
        //////////////////////////////////////////////////////////////////
        $ctrl.openUserPanel = openUserPanel;
        $ctrl.openCloseModernMenu = openCloseModernMenu;
        $ctrl.openTuto = openTuto;
        //////////////////////////////////////////////////////////////////
        // Init                                                         //
        //////////////////////////////////////////////////////////////////

        $ctrl.$onInit = function() {
            UserService.getCurrentUser().then(function(user) {
                var personnel = user.Personnel;

                $ctrl.utilisateurId = personnel.UtilisateurId;
                $ctrl.organisationId = personnel.Societe.OrganisationId;
            });

            handleDisplayNotifLoockUp(false, false);
            getUnreadableNotificationsNumberByUserId();
            $ctrl.photoProfilSrc = UserService.getPhotoProfil();
            $rootScope.$on('photoProfil.changed', function(event, img) {
                $ctrl.photoProfilSrc = img;
            });

            ServerNotificationsService.SubscribeToUserNotificationCountUpdates(updateNotificationCount);
        };

        $ctrl.$onChanges = function(changesObj) {
            if (changesObj.logo && changesObj.logo.currentValue) {
                $ctrl.logoSrc = changesObj.logo.currentValue;
            }
            if (changesObj.user && changesObj.user.currentValue) {
                $ctrl.photoProfilSrc = UserService.getPhotoProfil();
            }
        };

        function updateNotificationCount(notificationCount) {
            $ctrl.numberOfNotifications = notificationCount;

            $scope.$apply();
        }

        //////////////////////////////////////////////////////////////////
        // Evenements                                                   //
        //////////////////////////////////////////////////////////////////



        //////////////////////////////////////////////////////////////////
        // Handlers                                                     //
        //////////////////////////////////////////////////////////////////

        function openUserPanel() {
            $scope.$emit('open.user.panel');
        }

        function openTuto() {
            $http.get("/api/Groupe/GetUrlTutoByGroupe").then(function(result) {
                if (result.data) {
                    $window.open(result.data);
                }
            });
        }

        /**
         * Affichage du menu mode Modern avec propagation du changement
         */
        function openCloseModernMenu() {
            $scope.$emit('open.close.modern.menu');
        }

        /**
         * Récuperer le nombre des notifications non lues
         */
        function getUnreadableNotificationsNumberByUserId() {
            if ($ctrl.utilisateurId) {
                $http.get("/api/Notification/GetUnreadableNotificationsNumberByUserId/" + $ctrl.utilisateurId).then(function(result) {
                    if (result.data) {
                        $ctrl.numberOfNotifications = parseInt(result.data);
                    }
                });
            }
        }

        /**
         * Close Notification lockUp methode
         */
        $ctrl.handleCloseLoockUpNotifications = function() {
            $http.post("/api/Notification/MarkAsRead/" + $ctrl.utilisateurId).then(function(result) {
                if (result.data) {
                    getNotificationsbyUtilisateurId();
                    DeleteAffectationList();
                }
            });
            handleDisplayNotifLoockUp(false, false);
        };

        /**
         * Open Notification lockUp methode
         */
        $ctrl.openNotificationLoockup = function() {
            handleDisplayNotifLoockUp(true, true);
            getNotificationsbyUtilisateurId();
        };

        /**
         * Récuperer les notification pour un utilisateur
         */
        function getNotificationsbyUtilisateurId() {
            $ctrl.notifications = null;
            $http.get("/api/Notification").then(function(notificationsUser) {
                $ctrl.notifications = notificationsUser.data;
            });
        }

        /**
         * Convert date de creation d'une notif
         * @param {date} date - Date de creation
         * @return {date} date
         */
        $ctrl.handleDateNotification = function(date) {
            if (date) {
                return moment(date).set({ hour: 0, minute: 0 }).format("DD/MM/YYYY");
            }
            return "";
        };

        /**
         * Supprimer tous les notifications d'un utilisateur
         */
        $ctrl.handleDeleteAllNotifications = function() {
            var modalInstance = $uibModal.open({
                animation: true,
                component: 'deleteNotificationComponent',
                backdrop: 'static',
                windowClass: 'notification-modal',
                resolve: {
                    resources: function() { return $ctrl.resources; }
                }
            });

            // Traitement des données renvoyées par la modal
            modalInstance.result.then(function() {
                ConfirmDeleteAllNotifications();
            });
        };

        /**
         * Pop up de confirmation du suppression de toutes les notifications 
         */
        function ConfirmDeleteAllNotifications() {
            $http.post("/api/Notification/DeleteAllNotificationsByUtilisateurId/" + $ctrl.utilisateurId)
                .then(function(result) {
                    if (result.data) {
                        getNotificationsbyUtilisateurId();
                    }
                });

            handleDisplayNotifLoockUp(false, false);
        }

        /**
         * Supprimer une notification
         * @param {notification} notification - notification
         */
        $ctrl.handleDeleteNotification = function(notification) {
            var modalInstance = $uibModal.open({
                animation: true,
                component: 'deleteNotificationComponent',
                backdrop: 'static',
                windowClass: 'notification-modal',
                resolve: {
                    notification: function() { return notification; },
                    resources: function() { return $ctrl.resources; }
                }
            });

            // Traitement des données renvoyées par la modal
            modalInstance.result.then(function(value) {
                ConfirmDeleteNotification(value);
            });
        };

        /**
        * pop up confirmation pour supprimer une notification
        * @param {notification} notification - notification
        */
        function ConfirmDeleteNotification(notification) {
            notification.IsDeleted = true;
            for (var i = 0; i < $ctrl.notifications.length; i++) {
                if ($ctrl.notifications[i].NotificationId === notification.NotificationId) {
                    $ctrl.notifications.splice(i, 1);
                }
            }

            $ctrl.notificationsToDelete.push(notification);
            handleDisplayNotifLoockUp(true, true);
        }

        /**
         * Suppression des notifications
         */
        function DeleteAffectationList() {
            if ($ctrl.notificationsToDelete !== null && $ctrl.notificationsToDelete.length > 0) {
                $http.post("/api/Notification/DeleteNotifications/", $ctrl.notificationsToDelete)
                    .then(function(result) {
                        if (result.data) {
                            getNotificationsbyUtilisateurId();
                        }
                    });
            }
        }

        /**
         * Récuperer les notifications par recherche
         */
        $ctrl.handleChangeSearchText = function() {
            $ctrl.searchText = actionGetWords($ctrl.searchText);
            $ctrl.notifications = null;
            $http.get("/api/Notification/SearchLight/" + $ctrl.utilisateurId + "/" + $ctrl.searchText).then(function(notificationsUser) {
                $ctrl.notifications = notificationsUser.data;
            });
        };

        /**
         * Handle search text
         * @param {text} text - text
         * @return {text} search text 
         */
        function actionGetWords(text) {
            var words = [];

            text = $ctrl.searchText.trim();

            if (text.indexOf('*') >= 0) {
                words = text.split('*');
                for (var i = 0; i < words.length; i++) {
                    words[i] = words[i].trim();
                }
            }
            else {
                words.push(text);
            }
            return words;
        }

        /**
         * Controller l'affcihage du lockup pour les notifs
         * @param {isCss} isCss - bool
         * @param {isComponent} isComponent - bool
         */
        function handleDisplayNotifLoockUp(isCss, isComponent) {
            $ctrl.isNotificationLoockupOpenCss = isCss;
            $ctrl.isNotificationLoockupOpen = isComponent;
        }
    }
})();
