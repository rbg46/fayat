(function (angular) {
    'use strict';

    /**
     * Encapsulation de la progressBar pour permettre une configuration unique et une instanciation automatique.
     * 
     */
    function NotifyProvider() {
        var provider = this;

        provider.options = {
            errorPositionX: 'right',
            errorPositionY: 'bottom'
        };

        /**
         * Définit les options des notifications.
         * 
         * @param {any} options options
         */
        provider.setOptions = function (options) {
            if (!angular.isObject(options)) throw new Error("Options should be an object!");
            provider.options = angular.extend({}, provider.options, options);
        };

        /**
         * $get du provider.
         * 
         * @param {any} Notification notification
         * @returns {Notification} notification
         */
        provider.$get = ['Notification', function (Notification) {
            var vm = this;

            init(provider.options);

            /**
             * Initialisation.
             * @param {any} options options
             * 
             */
            function init(options) {
            }


            /**
             * Gestion des notifications de succes
             * 
             * @param {string} message message de la notification
             */
            function message(message) {
                Notification({ message: message, title: window.resources.Global_Notification_Titre });
            }

            /**
             * Notifie avec le message d'erreur passé en paramètre.
             * 
             * @param {string} message  message de la notification
             */
            function error(message) {
                Notification.error({
                    message: message,
                    positionY: vm.options.errorPositionY,
                    positionX: vm.options.errorPositionX
                });
            }

            /**
             * Notifie avec le message de warning passé en paramètre.
             * 
             * @param {string} message  message de la notification
             */
            function warning(message) {
              Notification.warning({
                message: message,
                positionY: vm.options.errorPositionY,
                positionX: vm.options.errorPositionX
              });
            }
            
            /**
             * Notifie avec le message d'erreur par défaut.
             * 
             */
            function defaultError() {
                error(resources.Global_Notification_Error);
            }

            // fonctions exposées
            angular.extend(vm, {
                message: message,
                error: error,
                warning: warning,
                defaultError: defaultError
            });

            return vm;
        }];
    }

    angular.module('Fred').provider('Notify', NotifyProvider);

})(angular);