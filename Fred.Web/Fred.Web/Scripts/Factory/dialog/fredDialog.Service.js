(function () {
    'use strict';

    angular
        .module('Fred')
        .factory('fredDialog', fredDialogService);

    fredDialogService.$inject = ['$uibModal'];

    function fredDialogService($uibModal) {

        var service = {
            confirmation: confirmation,
            information: information,
            question: question,
            message: message,
            erreur: erreur,
            generic: generic
        };

        return service;

        /**
         * Modal generic
         * @param {any} message message a afficher dans la modale
         * @param {any} title Titre de la modale, par défaut : vide
         * @param {any} titleIcon Icone de la modale, par défaut : vide
         * @param {any} leftTextButon Texte du bouton gauche de la modale, par défaut : vide
         * @param {any} rightTextButon Texte du bouton droit de la modale, par défaut : vide
         * @param {any} leftActionButon Action du bouton gauche, par défaut : close();
         * @param {any} rightActionButon Action du bouton droit, par défaut : close();
         * @param {any} optionTextButon Texte du bouton d'option de la modale, par défaut : vide
         * @param {any} optionActionButon Action du bouton d'option, par défaut : close()
         * @param {any} divContentStyle Class CSS de la modal
         * @param {any} maxHeight hauteur max de la modale, par défaut : 0
         * @returns {any} modalInstance.result modale
         */
        function generic(message, title, titleIcon, leftTextButon, rightTextButon, leftActionButon, rightActionButon, optionTextButon, optionActionButon, divContentStyle, maxHeight) {

            var modalInstance = $uibModal.open({
                animation: true,
                windowClass: divContentStyle,
                component: 'fredDialogComponent',
                resolve: {
                    message: function () {
                        return message;
                    },
                    title: function () {
                        return title;
                    },
                    titleIcon: function () {
                        return titleIcon;
                    },
                    leftTextButon: function () {
                        return leftTextButon;
                    },
                    rightTextButon: function () {
                        return rightTextButon;
                    },
                    leftActionButon: function () {
                        return leftActionButon;
                    },
                    rightActionButon: function () {
                        return rightActionButon;
                    },
                    optionTextButon: function () {
                        return optionTextButon;
                    },
                    optionActionButon: function () {
                        return optionActionButon;
                    },
                    divContentStyle: function () {
                        return divContentStyle;
                    },
                    maxHeight: function () {
                        return maxHeight ? maxHeight : "0";
                    }
                }
            });

            return modalInstance.result;
        }

        /**
         * Modal de confirmation
         * @param {any} message message a afficher dans la modale
         * @param {any} title Titre de la modale, par défaut : Confirmation
         * @param {any} titleIcon Icone de la modale, par défaut : flaticon flaticon-question
         * @param {any} leftTextButon Texte du bouton gauche de la modale, par défaut : Valider
         * @param {any} rightTextButon Texte du bouton droit de la modale, par défaut : Annuler
         * @param {any} leftActionButon Action du bouton gauche, par défaut : close();
         * @param {any} rightActionButon Action du bouton droit, par défaut : close();
         * @param {any} optionTextButon Texte du bouton d'option de la modale, par défaut : vide
         * @param {any} optionActionButon Action du bouton d'option, par défaut : close()
         * @param {any} divContentStyle Class CSS de la modal
         * @param {any} maxHeight hauteur max de la modale, par défaut : 0
         * @returns {any} modalInstance.result modale
         */
        function confirmation(message, title, titleIcon, leftTextButon, rightTextButon, leftActionButon, rightActionButon, optionTextButon, optionActionButon, divContentStyle, maxHeight) {

            var modalInstance = $uibModal.open({
                animation: true,
                windowClass: divContentStyle,
                component: 'fredDialogComponent',
                resolve: {
                    message: function () {
                        return message;
                    },
                    title: function () {
                        return title ? title : "Confirmation";
                    },
                    titleIcon: function () {
                        return titleIcon ? titleIcon : "flaticon flaticon-question";
                    },
                    leftTextButon: function () {
                        return leftTextButon ? leftTextButon : "Valider";
                    },
                    rightTextButon: function () {
                        return rightTextButon ? rightTextButon: "Annuler";
                    },
                    leftActionButon: function () {
                        return leftActionButon;
                    },
                    rightActionButon: function () {
                        return rightActionButon;
                    },
                    optionTextButon: function () {
                        return optionTextButon;
                    },
                    optionActionButon: function () {
                        return optionActionButon;
                    },
                    maxHeight: function () {
                        return maxHeight ? maxHeight : "0";
                    }
                }
            
            });

            return modalInstance.result;
        }

        /**
         * Modal d'information
         * @param {any} message message a afficher dans la modale
         * @param {any} title Titre de la modale, par défaut : Information
         * @param {any} titleIcon Icone de la modale, par défaut : flaticon flaticon-info
         * @param {any} leftTextButon Texte du bouton gauche de la modale, par défaut : Valider
         * @param {any} rightTextButon Texte du bouton droit de la modale, par défaut : vide
         * @param {any} leftActionButon Action du bouton gauche, par défaut : close();
         * @param {any} rightActionButon Action du bouton droit, par défaut : close();
         * @param {any} optionTextButon Texte du bouton d'option de la modale, par défaut : vide
         * @param {any} optionActionButon Action du bouton d'option, par défaut : close()
         * @param {any} divContentStyle Class CSS de la modal
         * @param {any} maxHeight hauteur max de la modale, par défaut : 0
         * @returns {any} modalInstance.result modale
         */
        function information(message, title, titleIcon, leftTextButon, rightTextButon, leftActionButon, rightActionButon, optionTextButon, optionActionButon, divContentStyle, maxHeight) {

            var modalInstance = $uibModal.open({
                animation: true,
                windowClass: divContentStyle,
                component: 'fredDialogComponent',
                resolve: {
                    message: function () {
                        return message;
                    },
                    title: function () {
                        return title ? title : "Information";
                    },
                    titleIcon: function () {
                        return titleIcon ? titleIcon : "flaticon flaticon-info";
                    },
                    leftTextButon: function () {
                        return leftTextButon ? leftTextButon : "Valider";
                    },
                    rightTextButon: function () {
                        return rightTextButon;
                    },
                    leftActionButon: function () {
                        return leftActionButon;
                    },
                    rightActionButon: function () {
                        return rightActionButon;
                    },
                    optionTextButon: function () {
                        return optionTextButon;
                    },
                    optionActionButon: function () {
                        return optionActionButon;
                    },
                    maxHeight: function () {
                        return maxHeight ? maxHeight : "0";
                    }
                }
            });

            return modalInstance.result;
        }

        /**
         * Modal de question
         * @param {any} message message a afficher dans la modale
         * @param {any} title Titre de la modale, par défaut : Question
         * @param {any} titleIcon Icone de la modale, par défaut : flaticon flaticon-info
         * @param {any} leftTextButon Texte du bouton gauche de la modale, par défaut : Oui
         * @param {any} rightTextButon Texte du bouton droit de la modale, par défaut : Non
         * @param {any} leftActionButon Action du bouton gauche, par défaut : close();
         * @param {any} rightActionButon Action du bouton droit, par défaut : close();
         * @param {any} optionTextButon Texte du bouton d'option de la modale, par défaut : vide
         * @param {any} optionActionButon Action du bouton d'option, par défaut : close()
         * @param {any} divContentStyle Class CSS de la modal
         * @param {any} maxHeight hauteur max de la modale, par défaut : 0
         * @returns {any} modalInstance.result modale
         */
        function question(message, title, titleIcon, leftTextButon, rightTextButon, leftActionButon, rightActionButon, optionTextButon, optionActionButon, divContentStyle, maxHeight) {

            var modalInstance = $uibModal.open({
                animation: true,
                windowClass: divContentStyle,
                component: 'fredDialogComponent',
                resolve: {
                    message: function () {
                        return message;
                    },
                    title: function () {
                        return title ? title : "Question";
                    },
                    titleIcon: function () {
                        return titleIcon ? titleIcon : "flaticon flaticon-info";
                    },
                    leftTextButon: function () {
                        return leftTextButon ? leftTextButon : "Oui";
                    },
                    rightTextButon: function () {
                        return rightTextButon ? rightTextButon : "Non";
                    },
                    leftActionButon: function () {
                        return leftActionButon;
                    },
                    rightActionButon: function () {
                        return rightActionButon;
                    },
                    optionTextButon: function () {
                        return optionTextButon;
                    },
                    optionActionButon: function () {
                        return optionActionButon;
                    },
                    maxHeight: function () {
                        return maxHeight ? maxHeight : "0";
                    }
                }
            });

            return modalInstance.result;
        }

        /**
         * Modal de message
         * @param {any} message message a afficher dans la modale
         * @param {any} title Titre de la modale, par défaut : Message
         * @param {any} titleIcon Icone de la modale, par défaut : flaticon flaticon-menu
         * @param {any} leftTextButon Texte du bouton gauche de la modale, par défaut : Valider
         * @param {any} rightTextButon Texte du bouton droit de la modale, par défaut : vide
         * @param {any} leftActionButon Action du bouton gauche, par défaut : close();
         * @param {any} rightActionButon Action du bouton droit, par défaut : close();
         * @param {any} optionTextButon Texte du bouton d'option de la modale, par défaut : vide
         * @param {any} optionActionButon Action du bouton d'option, par défaut : close()
         * @param {any} divContentStyle Class CSS de la modal
         * @param {any} maxHeight hauteur max de la modale, par défaut : 0
         * @returns {any} modalInstance.result modale
         */
        function message(message, title, titleIcon, leftTextButon, rightTextButon, leftActionButon, rightActionButon, optionTextButon, optionActionButon, divContentStyle, maxHeight) {

            var modalInstance = $uibModal.open({
                animation: true,
                windowClass: divContentStyle,
                component: 'fredDialogComponent',
                resolve: {
                    message: function () {
                        return message;
                    },
                    title: function () {
                        return title ? title : "Message";
                    },
                    titleIcon: function () {
                        return titleIcon ? titleIcon : "flaticon flaticon-menu";
                    },
                    leftTextButon: function () {
                        return leftTextButon ? leftTextButon : "Valider";
                    },
                    rightTextButon: function () {
                        return rightTextButon;
                    },
                    leftActionButon: function () {
                        return leftActionButon;
                    },
                    rightActionButon: function () {
                        return rightActionButon;
                    },
                    optionTextButon: function () {
                        return optionTextButon;
                    },
                    optionActionButon: function () {
                        return optionActionButon;
                    },
                    maxHeight: function () {
                        return maxHeight ? maxHeight : "0";
                    }                    
                }
            });

            return modalInstance.result;
        }

        /**
         * Modal d'erreur
         * @param {any} message message a afficher dans la modale
         * @param {any} title Titre de la modale, par défaut : Erreur
         * @param {any} titleIcon Icone de la modale, par défaut : flaticon flaticon-error
         * @param {any} leftTextButon Texte du bouton gauche de la modale, par défaut : Valider
         * @param {any} rightTextButon Texte du bouton droit de la modale, par défaut : vide
         * @param {any} leftActionButon Action du bouton gauche, par défaut : close();
         * @param {any} rightActionButon Action du bouton droit, par défaut : close();
         * @param {any} optionTextButon Texte du bouton d'option de la modale, par défaut : vide
         * @param {any} optionActionButon Action du bouton d'option, par défaut : close()
         * @param {any} divContentStyle Class CSS de la modal
         * @param {any} maxHeight hauteur max de la modale, par défaut : 0
         * @returns {any} modalInstance.result modale
         */
        function erreur(message, title, titleIcon, leftTextButon, rightTextButon, leftActionButon, rightActionButon, optionTextButon, optionActionButon, divContentStyle, maxHeight) {

            var modalInstance = $uibModal.open({
                animation: true,
                windowClass: divContentStyle,
                component: 'fredDialogComponent',
                resolve: {
                    message: function () {
                        return message;
                    },
                    title: function () {
                        return title ? title: "Erreur";
                    },
                    titleIcon: function () {
                        return titleIcon ? titleIcon : "flaticon flaticon-error";
                    },
                    leftTextButon: function () {
                        return leftTextButon ? leftTextButon : "Valider";
                    },
                    rightTextButon: function () {
                        return rightTextButon;
                    },
                    leftActionButon: function () {
                        return leftActionButon;
                    },
                    rightActionButon: function () {
                        return rightActionButon;
                    },
                    optionTextButon: function () {
                        return optionTextButon;
                    },
                    optionActionButon: function () {
                        return optionActionButon;
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