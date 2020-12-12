(function () {
    'use strict';
    angular
        .module('Fred')
        .component('fredLookupSearchComponent', {
            templateUrl: '/Scripts/directive/lookup/fournisseur/tabs/search/fred-lookup-search.template.html',
            bindings: {
                onSearchTextChanged: '&',
                searchText: '=',
                searchText2: '=',
                placeholder1: '@',
                placeholder2: '@',
                title: '@',
                title1: '@',
                title2: '@'
            },
            controller: FredLookupSearchController
        });


    FredLookupSearchController.$inject = [];

    function FredLookupSearchController() {

        var $ctrl = this;

        $ctrl.searchTextChanged = function (searchText) {
            $ctrl.onSearchTextChanged({

                searchText: $ctrl.searchText,
                searchText2: $ctrl.searchText2

            });
        };
    }
})();
