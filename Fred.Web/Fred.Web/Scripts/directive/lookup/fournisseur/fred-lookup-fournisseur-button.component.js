(function (angular) {
    'use strict';

    angular.module('Fred').component('fredLookupFournisseurButtonComponent', {
        templateUrl: '/Scripts/directive/lookup/fournisseur/fred-lookup-fournisseur-button.template.html',
        require: 'ngModel',
        bindings: {
            //texte affiché       
            text: "<",//ok           
            //tooltip sur le bouton d'ouverture
            tooltip: "<",//ok
            // Action executé lors du click sur le bouton supprimé
            onDelete: '&',//ok
            //Active ou desactive le lookup.
            ngDisabled: '=ngDisabled',
            // Placeholder de la lookup
            placeholder: '@?',
            // Ne pas renseigné MAIS il faut fournir un ngModel a la directive.
            buttonModel: '=ngModel',
            // OBLIGATORE : Définit la clé ouvrira lea picklist correspondante
            key: '@',
            // OBLIGATORE : Définit la clé de la picklist qui s'ouvrira
            panelKey: '@'
        },
        controller: FredLookupFournisseurButtonController
    });

    FredLookupFournisseurButtonController.$inject = ['$timeout', 'fredSubscribeService'];

    function FredLookupFournisseurButtonController($timeout, fredSubscribeService) {

        var $ctrl = this;

        //////////////////////////////////////////////////////////////////
        // Init                                                         //
        //////////////////////////////////////////////////////////////////

        $ctrl.$onInit = function () {
            fredSubscribeService.subscribe({ eventName: 'fredPickListItemSelected' + $ctrl.key, callback: itemSelectedInPickListPanel });
        };

        function itemSelectedInPickListPanel(newItem) {
            $ctrl.buttonModel = newItem.model;
        }

        $ctrl.$onDestroy = function () {
            fredSubscribeService.unsubscribe({ eventName: 'fredPickListItemSelected' + $ctrl.key, callback: itemSelectedInPickListPanel });
        };

        $ctrl.handleDelete = function ($event) {
            $ctrl.onDelete();
            $event.stopPropagation();
        };

        $ctrl.click = function () {
            fredSubscribeService.raiseEvent('fredLookupOpen' + $ctrl.panelKey, $ctrl.key);
        };


    }
}(angular));
