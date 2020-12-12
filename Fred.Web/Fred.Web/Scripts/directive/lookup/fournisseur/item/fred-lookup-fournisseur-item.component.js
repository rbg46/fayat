(function () {
    'use strict';
    angular
        .module('Fred')
        .component('fredLookupFournisseurItemComponent', {
            templateUrl: '/Scripts/directive/lookup/fournisseur/item/fred-lookup-fournisseur-item.template.html',
            require: {
                fredLookupFournisseurPanelComponent: '^fredLookupFournisseurPanelComponent'
            },
            bindings: {
                fournisseur: '=',
                textsToHighlight: '=',
                showButtonAgence: '='
            },
            controller: FredLookupFournisseurItemController
        });


    FredLookupFournisseurItemController.$inject = [];

    function FredLookupFournisseurItemController() {

        var $ctrl = this;

        $ctrl.selectFournisseur = function () {
            $ctrl.fredLookupFournisseurPanelComponent.onSelectFournisseur($ctrl.fournisseur);
        };

        $ctrl.openAgence = function () {
            $ctrl.fredLookupFournisseurPanelComponent.onOpenAgence($ctrl.fournisseur);
        };
    }

})();
