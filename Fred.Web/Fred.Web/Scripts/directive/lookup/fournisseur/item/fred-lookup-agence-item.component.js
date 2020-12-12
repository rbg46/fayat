(function () {
    'use strict';
    angular
        .module('Fred')
        .component('fredLookupAgenceItemComponent', {
            templateUrl: '/Scripts/directive/lookup/fournisseur/item/fred-lookup-agence-item.template.html',
            require: {
                fredLookupFournisseurPanelComponent: '^fredLookupFournisseurPanelComponent',
                fredLookupAgenceTabComponent: '^fredLookupAgenceTabComponent'
            },
            bindings: {
                agence: '=',
                textsToHighlight: '=',
            },
            controller: FredLookupAgenceItemController
        });


    FredLookupAgenceItemController.$inject = [];

    function FredLookupAgenceItemController() {

        var $ctrl = this;

        $ctrl.selectAgence = function () {
            $ctrl.fredLookupAgenceTabComponent.onSelectAgenceOnItem($ctrl.agence);
        };

    }

})();
