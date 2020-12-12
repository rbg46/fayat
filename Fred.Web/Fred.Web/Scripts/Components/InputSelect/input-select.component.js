(function () {
    'use strict';
    angular.module('Fred').component('inputSelect', {
        templateUrl: '/Scripts/Components/InputSelect/input-select.template.html',
        bindings: {

            // Text affiché
            text: "<",

            // Etat sélection ou pas
            isSelected: "<",

            // Etat d'édition ou pas
            isEditable: "<",

            // Placeholder
            placeholder: "<",

            // Sélection
            onSelect: '&',

            // Désélectionner
            onDelete: '&'

        },
        controller: function () {

            var $ctrl = this;

            // ################# HANDLERS ###################

            $ctrl.handleSelectClick = function () {

                // Notifier parent
                $ctrl.onSelect();
            }

            $ctrl.handleDeleteClick = function () {

                // Notifier parent
                $ctrl.onDelete();
            }
        }
    });
})();