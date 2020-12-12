(function () {
    'use strict';

    angular.module('Fred').service('lookupKeyDownService', LookupKeyDownService);

    /**
     * @description Service du clavier dans la barre de recherche
     * @returns {any} Constructeur du service
     */
    function LookupKeyDownService() {
        var service = {
            onKeyDown: onKeyDown
        };

        return service;


        function onKeyDown(e, ctrl, onEnterCallback) {
            // Up (38)
            if (e.keyCode === 38 && ctrl.selectedIndex - 1 >= 0) {
                ctrl.selectedIndex--;
            }
            // Down (40)
            if (e.keyCode === 40 && ctrl.selectedIndex + 1 < ctrl.referentiels.length) {
                ctrl.selectedIndex++;
            }
            // Enter (13)
            if (e.keyCode === 13 && ctrl.referentiels.length > 0) {
                //on attend que la dernière recherche effctuée soit achevée
                while (ctrl.busy) {
                    setTimeout(function () {
                        console.log('waiting for watcher end');
                    }, 200);
                }
                //Appel de la fonction de sélection pour l'élément sélectionné dans la liste
                onEnterCallback(ctrl.referentiels[ctrl.selectedIndex]);
                e.preventDefault();
            }

            if (e.keyCode === 27) {
                ctrl.isOpen = false;
            }
        }


    }
})();
