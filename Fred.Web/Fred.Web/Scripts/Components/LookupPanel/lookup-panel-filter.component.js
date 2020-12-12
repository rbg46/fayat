(function () {
    'use strict';
    angular
        .module('Fred')
        .component('lookupPanelFilter', {
            templateUrl: '/Scripts/Components/LookupPanel/lookup-panel-filter.template.html',
            bindings: {

                onCancel: "&",

                onItemSelected: '&',

                onFilterChange: '&',

                // Item sélectionné
                value: '<',

                // Items récupérés du parent
                items: '<',

                //texte affiché comme titre dans le panneaux de recherche.
                title: "@?",

                //texte affiché comme Placeholder dans le panneaux de recherche.
                placeholder: "@?",

                // Tooltip du champ de recherche
                searchTooltip: "@?",

                itemTemplate: '@?'

            },
            controller: ['$scope', LookupPanelFilter]
        });


    function LookupPanelFilter($scope) {

        var $ctrl = this;

        $ctrl.$onInit = function () {

            $ctrl.searchText = '';
            $ctrl.selectedItem = null;
            $ctrl.busy = false;

        };

        // ################# HANDLERS ###################

        $ctrl.handleChangeSearchText = function handleChangeSearchText() {

            // Notifier le parent du changement du filtre
            $ctrl.onFilterChange({ text: $ctrl.searchText });

        };


        $ctrl.handleCancel = function () {

            // Appeler parent
            $ctrl.onCancel();
        };


        $ctrl.handleSelectItem = function (item) {

            // Appeler parent
            $ctrl.onItemSelected({ item: item });
        };

        // ################# ACTIONS ###################

    }
})();