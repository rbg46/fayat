


(function () {
    'use strict';

    angular
        .module('Fred')
        .component('lookupPanel', {
            templateUrl: '/Scripts/Components/LookupPanel/lookup-panel.template.html',
            transclude: true,
            bindings: {

                // Template du header
                templateHeader: '@?',

                // Template d'un nouveau item
                templateNewItem: '@?',

                // Template d'un item
                templateItem: '@?',

                // Event élément sélectionné
                onSelect: '&',

                // Event filtre changé
                onFilterChange: '&',

                // Event chargement plus de de pages
                onLoadMore: '&',

                // Event de création d'un nouvel élément
                onAdd: '&',

                // Event fermeture
                onCancel: "&",

                // Filtre utilisé (à utiliser si des champs par défaut)
                filter: '<',

                // Élément sélectionné
                value: '<',

                // Items récupérés du parent
                items: '<',

                // Indicateur de chargement
                busy: '<',

                // Ajouter un nouvel élément
                canAdd: '<'

            },
            controller: ['$scope', LookupPanel]
        });


    function LookupPanel($scope) {

        var $ctrl = this;

        $ctrl.$onInit = function () {

            // Init Ressources
            $ctrl.resources = resources;
            
        };

        // ################# HANDLERS ###################

        $ctrl.handleLoadMore = function () {

            // Demander le chargement des pages suivantes
            $ctrl.onLoadMore();

        };

        $ctrl.handleSelectItem = function (item) {

            // Init current value
            $ctrl.value = item;
             
            // Appeler parent
            $ctrl.onSelect({ item: item });
        };


        $ctrl.handleAdd = function (itemToAdd) {

            // Notifier parent
            $ctrl.onAdd({ itemToAdd: itemToAdd });

        };

        $ctrl.handleFilterChange = function () {

            // Notifier le parent du changement du filtre
            $ctrl.onFilterChange({ filter: $ctrl.filter });

        };


        $ctrl.handleCancel = function () {

            // Appeler parent
            $ctrl.onCancel();

        };

        // ################# ACTIONS ###################


    }
})();