/// <reference path="../../module/authorization/permissionKeys.js" />

(function () {
    'use strict';

    angular
        .module('Fred')
        .component('fredMenuHome', {
            templateUrl: '/Scripts/Controllers/menu/fred-menu-home.html',
            bindings: {
                user: '<',
                criteria: "<",
                modulesAutorized: "<",
                home: "<"
            },
            controller: 'fredMenuHomeController'
        });

    angular.module('Fred').controller('fredMenuHomeController', fredMenuHomeController);

    fredMenuHomeController.$inject = ['$scope', 'favorisService', '$localStorage'];

    function fredMenuHomeController($scope, favorisService, $localStorage) {

        var $ctrl = this;
        $ctrl.resources = resources;
        $ctrl.showDeleteFavoris = showDeleteFavoris;
        $ctrl.hideDeleteFavoris = hideDeleteFavoris;
        $ctrl.openDeleteFavori = openDeleteFavori;

        $ctrl.$onInit = function () {
            $scope.$on('delete.favori', function (event, args) {
                deleteFavori(args.favori);
            });
            $scope.$on('add.favori', function (event, args) {
                loadFavorisListInLocalStorage();
            });
            loadFavorisList();
        };

        function loadFavorisList() {
            if ($localStorage.CurrentUserFavoris !== undefined) {
                $ctrl.favoris = $localStorage.CurrentUserFavoris;
            } else {
                loadFavorisListInLocalStorage();
            }
        }

        function loadFavorisListInLocalStorage() {
            favorisService.GetList().then(function (value) {
                setFavoris(value);
            })
                .catch(function (reason) {
                    cleanFavoris();
                    console.log(reason);
                });
        }

        function setFavoris(favoris) {
            $localStorage.CurrentUserFavoris = $ctrl.favoris = favoris;
        }

        function cleanFavoris() {
            delete $localStorage.CurrentUserFavoris;
            $ctrl.favoris = {};
        }

        function showDeleteFavoris(valeur) {
            if ($ctrl.home) {
                $(document).ready(function () {
                    $("#favori" + valeur).fadeIn();
                });
            } else {
                $(document).ready(function () {
                    $("#fav" + valeur).fadeIn();
                });
            }
        }

        function hideDeleteFavoris(valeur) {
            if ($ctrl.home) {
                $(document).ready(function () {
                    $("#favori" + valeur).fadeOut();
                });
            } else {
                $(document).ready(function () {
                    $("#fav" + valeur).fadeOut();
                });
            }
        }

        function openDeleteFavori(favori) {
            favorisService.OpenModalDeleteFavori(favori, $ctrl.home);
        }

        function deleteFavori(favori) {
            var indexFavoriteToDelete = $ctrl.favoris.indexOf(favori);

            if (indexFavoriteToDelete !== -1) {
                $ctrl.favoris.splice(indexFavoriteToDelete, 1);
            }
        }
    }
})();