(function () {
    'use strict';

    angular.module('Fred').service('FournisseurService', FournisseurService);

    FournisseurService.$inject = ['$http', 'Notify'];

    // ############### Service des Fournisseurs   #####################

    function FournisseurService($http, Notify) {
        var vm = this;

        vm.GetFournisseursPromise = function (groupeId, page, pageSize, recherche, recherche2, ciId, withCommandeValide) {

            //'api/Fournisseur/SearchLight/{page?}/{pageSize?}/{recherche?}/{groupeId?}/{recherche2?}/{ciId?}/{withCommandValide?}'

            return $http.get(
                `/api/Fournisseur/SearchLight?page=${page}&pageSize=${pageSize}&recherche=${recherche || ''}&groupeId=${groupeId}&recherche2=${recherche2 || ''}&ciId=${ciId}&withCommandValide=${withCommandeValide}`,
                {
                    cache: false
                }
            );
        };

    }
})();

