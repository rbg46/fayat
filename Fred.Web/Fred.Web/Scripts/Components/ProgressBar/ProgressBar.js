(function (angular) {
    'use strict';

    /**
     * Encapsulation de la progressBar pour permettre une configuration unique et une instanciation automatique.
     * 
     */
    function ProgressBarProvider() {
        var provider = this;

        provider.options = {
            height: '7px',
            color: '#FDD835'
        };

        /**
         * Définit les options de la progress bar.
         * 
         * @param {any} options 
         */
        provider.setOptions = function (options) {
            if (!angular.isObject(options)) throw new Error("Options should be an object!");
            provider.options = angular.extend({}, provider.options, options);
        };

        /**
         * $get du provider.
         * 
         * @param {any} ngProgressFactory ngProgressFactory
         */
        provider.$get = ['ngProgressFactory', function (ngProgressFactory) {
            var vm = this;

            init(provider.options);

            /**
             * Initialisation de l'objet ProgressBar
             * 
             */
            function init(options) {
                vm.started = 0;
                vm.progressBar = ngProgressFactory.createInstance();
                vm.progressBar.setHeight(options.height);
                vm.progressBar.setColor(options.color);
            }


            /**
             * Démarre la progress bar s'il s'agit du premier traitement en cours.
             * @param {boolean} blockScreenExceptMenu indique si l'écran, sauf les menus, doit être bloqué durant la progression. Peut-être undefined.
             */
            function start(blockScreenExceptMenu) {
                var first = vm.started === 0;
                vm.started++;
                if (first) {
                    vm.progressBar.start(blockScreenExceptMenu);
                }
            }

            /**
             * Arrête la progress bar mais continue à l'afficher.
             * 
             */
            function stop() {
                vm.progressBar.stop();
            }

            /**
             * Termine l'affichage de la progress bar s'il s'agissait du dernier traitement en cours..
             * 
             */
            function complete() {
                vm.started--;
                var last = vm.started === 0;
                if (last) {
                    vm.progressBar.complete();
                }
            }

            // fonctions exposées
            angular.extend(vm, {
                start: start,
                stop: stop,
                complete: complete
            });

            return vm;
        }];
    }

    angular.module('Fred').provider('ProgressBar', ProgressBarProvider);

})(angular);