(function () {
    'use strict';

    angular
        .module('Fred')
        .service('BudgetDataService', BudgetDataService);

    BudgetDataService.$inject = ['$http', '$resource'];

    function BudgetDataService($http, $resource) {
        var vm = this;



        vm.GetChapitres = function (ciSelected, deviseId, searchText, page, pageSize) {
            return $http.get("/api/Budget/GetChapitres?ciId=" + ciSelected.CiId + "&deviseId=" + deviseId + "&filter=" + searchText + "&page=" + page + "&pageSize=" + pageSize);
        };



        vm.GetBudgetRevisionTaches = function (revisionId, ciId) {
            return $http.get("/api/Budget/Revision?revisionId=" + revisionId + "&ciId=" + ciId);
        };


        vm.GetDeviseRefByCiId = function (ciId) {
            return $http.get("/api/CI/Devises/" + ciId);
        };

        vm.UpdateTacheWithRessourceTaches = function (ciId, task) {
            return $http.put("/api/Budget/Tache?ciId=" + ciId, task);
        };

        vm.UpdateBudgetRevisionTaches = function (tasks) {
            return $http.put("/api/Budget/Revision", tasks);
        };

        vm.CreateRessource = function (newRessource, ciId) {
            return $http.post("/api/Budget/Ressource?ciId=" + ciId, newRessource);
        };

        vm.UpdateRessource = function (existingRessource, ciId) {
            return $http.put("/api/Budget/UpdateRessource?ciId=" + ciId, existingRessource);
        };

        vm.UpdateParamRefEtendu = function (existingParamRefEtendu) {
            return $http.put("/api/Budget/UpdateParamRefEtendu", existingParamRefEtendu);
        };

        vm.UpdateBudgetRevision = function (updateBudgetRevision) {
            return $http.put("/api/Budget/UpdateBudgetRevision", updateBudgetRevision);
        };

    }
})();

