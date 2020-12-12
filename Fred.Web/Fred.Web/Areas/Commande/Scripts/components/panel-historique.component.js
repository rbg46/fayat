(function () {
    'use strict';
    angular
        .module('Fred')
        .component('panelHistorique', {
            templateUrl: '/Areas/Commande/Scripts/components/panel-historique.template.html',
            bindings: {

                historique: '<',

                busy : '<',

                onRequestClose: '&'

            },
            controller: ['Enums', PanelHistorique]
        });


    function PanelHistorique(Enums) {

        var $ctrl = this;

        // Assignations
        $ctrl.EnumTypeCommandeEvent = Enums.EnumTypeCommandeEvent;

        // Ressources
        $ctrl.resources = resources;

        // ################# HANDLERS ###################

        $ctrl.handleHide = function () {
            // Request close
            $ctrl.onRequestClose();
        };

    }
})();