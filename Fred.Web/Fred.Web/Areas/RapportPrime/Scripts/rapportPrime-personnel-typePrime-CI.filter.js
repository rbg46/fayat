(function (angular) {
    'use strict';

    angular
        .module('Fred')
        .filter('rapportPrimePersonnelTypePrimeCIFilter', function () {
            return function (items, searchText) {

                if (searchText) {
                    var filtered = [];
                    for (var i = 0; i < items.length; i++) {
                        var item = items[i];

                        if (item.Personnel && item.PersonnelSelectionne.toLowerCase().indexOf(searchText.toLowerCase()) >= 0
                            || item.Prime && item.Prime.Code.toLowerCase().indexOf(searchText.toLowerCase()) >= 0
                            || item.Ci && item.Ci.CodeLibelle.toLowerCase().indexOf(searchText.toLowerCase()) >= 0) {

                            filtered.push(item);
                        }
                    }
                    return filtered;
                }
                return items;
            };
        });

}(angular));