(function () {
    'use strict';

    angular
        .module('Fred')
        .factory('confirmDialog', confirmDialogService);

    confirmDialogService.$inject = ['$rootScope','$uibModal'];
    function confirmDialogService($rootScope,$uibModal) {

        var service = {
            confirm: confirm,
            confirmWithOption: confirmWithOption
        };

        return service;

        function confirm(ressources, message, titleIcon, maxHeight) {

            var modalInstance = $uibModal.open({
                appendTo :angular.element[0],
                animation: true,
                closeAllDialogs: true,
                component: 'confirmDialogComponent',
                resolve: {
                    resources: function () {
                        return resources;
                    },
                    message: function () {
                        return message;
                    },
                    titleIcon: function () {
                        return titleIcon ? titleIcon : "flaticon flaticon-garbage-2";
                    },
                    maxHeight: function () {
                        return maxHeight ? maxHeight : "0";
                    },
                    okayButtonText: function () {
                        return ressources.Global_Bouton_Valider;
                    },
                    cancelButtonText: function () {
                        return ressources.Global_Bouton_Annuler;
                    }
                }
            });

            return modalInstance.result;
        }

        /*
         * Permet d'avoir une boite de dialogue avec un troisieme bouton
         * Quand le trosieme bouton est cliqué le resultat de la reponse contient un object {option:true} 
         * Cette fonction permet aussi de préciser le texte à utiliser pour les deux boutons présents de base : okay et annuler
         */
        function confirmWithOption(ressources, message, optionButtonText, okayButtonText, cancelButtonText, titleIcon, maxHeight) {

            var modalInstance = $uibModal.open({
                animation: true,
                component: 'confirmDialogComponent',
                resolve: {
                    resources: function () {
                        return resources;
                    },
                    message: function () {
                        return message;
                    },
                    option: function () {
                        return true;
                    },
                    optionButtonText: function () {
                        return optionButtonText;
                    },
                    okayButtonText: function () {
                        return okayButtonText ? okayButtonText : ressources.Global_Bouton_Valider;
                    },
                    cancelButtonText: function () {
                        return cancelButtonText ? cancelButtonText : ressources.Global_Bouton_Annuler;
                    },
                    titleIcon: function () {
                        return titleIcon ? titleIcon : "flaticon flaticon-garbage-2";
                    },
                    maxHeight: function () {
                        return maxHeight ? maxHeight : "0";
                    }
                }
            });

            return modalInstance.result;
        }
    }
})();