/*
 * Ce service sert a recuperer les informations sur l'utilisateur courrant.
 */
(function() {
    'use strict';

    angular
        .module('Fred')
        .service('UserService', UserService);

    UserService.$inject = ['$http', '$localStorage', '$q', '$rootScope'];

    function UserService($http, $localStorage, $q, $rootScope) {

        var getCurrentUserPromise = null;

        var service = {
            getCurrentUser: getCurrentUser,
            loadImages: loadImages,
            getSavedImages: getSavedImages,
            setLogoImage: setLogoImage,
            setLoginImage: setLoginImage,
            getVersionInfo: getVersionInfo,
            getModeMenu: getMenuMode,
            setModeMenu: setMenuMode,
            getPhotoProfil: getPhotoProfil,
            setPhotoProfil: setPhotoProfil
        };

        return service;

        function getCurrentUser() {
            var currentUser = loadCurrentUser();

            if (isCurrentUserSet(currentUser))
                return $q.resolve(currentUser);

            if (getCurrentUserPromise === null) {
                getCurrentUserPromise = $http.get("/api/Utilisateur/CurrentUser").then(function(response) {
                    var utilisateur = response.data;
                    saveCurrentUser(utilisateur);

                    return utilisateur;
                });
            }

            return getCurrentUserPromise;
        }

        function isCurrentUserSet(currentUser) {
            return currentUser !== undefined && currentUser !== null;
        }

        function loadCurrentUser() {
            return $localStorage.CurrentUser;
        }

        function saveCurrentUser(user) {
            $localStorage.CurrentUser = user;
        }

        function getVersionInfo() {
            return $http.get("/api/Assembly/VersionInfo/").then(function(response) {
                return response.data;
            });
        }

        function getPhotoProfil() {
            getCurrentUser().then(function(user) {
                if (user.Personnel.PhotoProfilBase64) {
                    var imgSrcBase = 'data:image/png;base64,';

                    return imgSrcBase + user.Personnel.PhotoProfilBase64;
                }

                return "/medias/app/personnel/1.jpg";
            });
        }

        function loadImages(utilisateur) {
            var defer = $q.defer();

            var allImagesPromises = [];
            allImagesPromises.push(getUserSocieteLoginImage(utilisateur.Personnel.SocieteId));
            allImagesPromises.push(getUserSocieteLogoImage(utilisateur.Personnel.SocieteId));
            
            $q.all(allImagesPromises).finally(function() {
                defer.resolve(true);
            });
        }

        function getUserSocieteLoginImage(SocieteId) {
            if ($localStorage.CurrentUserSocieteLoginImage !== undefined) {
                emitLoginFromLocalStorage();
            } else {
                return $http.get('/api/Societe/' + SocieteId + '/GetLoginImage').then(function(response) {
                    var loginImage = response.data;
                    $localStorage.CurrentUserSocieteLoginImage = { "loginImage": loginImage };
                    emitLoginFromLocalStorage();
                });
            }
        }

        function emitLoginFromLocalStorage() {
            $rootScope.$emit('login.changed', $localStorage.CurrentUserSocieteLoginImage);
        }

        function getUserSocieteLogoImage(SocieteId) {
            if ($localStorage.CurrentUserSocieteLogoImage !== undefined) {
                emitLogoFromLocalStorage();
            } else {
                return $http.get('/api/Societe/' + SocieteId + '/GetLogoImage').then(function(response) {
                    var logoImage = response.data;
                    $localStorage.CurrentUserSocieteLogoImage = { "logoImage": logoImage };
                    emitLogoFromLocalStorage();
                });
            }
        }

        function emitLogoFromLocalStorage() {
            $rootScope.$emit('logo.changed', $localStorage.CurrentUserSocieteLogoImage);
        }

        function getSavedImages() {
            var login = "/medias/app/societe/screenLogin/0.jpg";
            var logo = "/medias/app/societe/logotype/DEFAULT.png";
            if ($localStorage.CurrentUserSocieteLoginImage) {
                login = $localStorage.CurrentUserSocieteLoginImage.loginImage.Path;
            }
            if ($localStorage.CurrentUserSocieteLogoImage) {
                logo = $localStorage.CurrentUserSocieteLogoImage.logoImage.Path;
            }
            return {
                login: login,
                logo: logo
            };
        }

        function setLogoImage(societeId, logoImage) {
            getCurrentUser().then(function(currentUser) {
                if (currentUser.Personnel.SocieteId === societeId) {
                    $localStorage.CurrentUserSocieteLogoImage = { "logoImage": logoImage };
                    $rootScope.$emit('logo.changed', logoImage);
                }
            });
        }

        function setLoginImage(societeId, loginImage) {
            getCurrentUser().then(function(user) {
                if (user.Personnel.SocieteId === societeId) {
                    $localStorage.CurrentUserSocieteLoginImage = { "loginImage": loginImage };
                }
            });
        }

        function setPhotoProfil(personnelId, imgBase64) {
            getCurrentUser().then(function(user) {
                if (user.Personnel.PersonnelId === personnelId) {
                    var imgSrcBase = 'data:image/png;base64,';
                    $localStorage.CurrentUser.Personnel.PhotoProfilBase64 = imgBase64;
                    $rootScope.$emit('photoProfil.changed', imgSrcBase + imgBase64);
                }
            });
        }

        function setMenuMode(val) {
            $localStorage.CurrentUserSettings = { "menuMode": val };
            $rootScope.$emit('menuMode.changed', val);
        }


        function getMenuMode() {
            if (!$localStorage.CurrentUserSettings)
                return "classic";

            return $localStorage.CurrentUserSettings.menuMode;
        }
    }
})();

