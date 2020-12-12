
(function () {
    'use strict';

    angular.module('Fred').service('TachesService', TachesService);

    TachesService.$inject = ['$http'];

    function TachesService($http) {
        var baseUrl = '/api/ReferentielTaches';

        var service = {
            getTachesByCIId: getTachesByCIId,
            getTache: getTache,
            addTache: addTache,
            updateTache: updateTache,
            deleteTache: deleteTache,
            getNextTaskCode: getNextTaskCode,
            copyCI: copyCI,
            GetTacheLevel1: GetTacheLevel1,
            copyPartialTache: copyPartialTache
        };

        return service;

        function getTachesByCIId(id) {
            return $http.get(baseUrl + '/' + id);
        }

        function getTache(id) {
            return $http.get(baseUrl + '/GetTache/' + id);
        }

        function addTache(tache) {
            var postData = JSON.stringify(tache);
            return $http.post(baseUrl, postData);
        }

        function updateTache(tache) {
            var postData = JSON.stringify(tache);
            return $http.put(baseUrl, postData);
        }

        function deleteTache(item) {
            var tacheId = item.TacheId;
            return $http.delete(baseUrl + '/' + tacheId);
        }

        function getNextTaskCode(item) {
            var tacheId = item.TacheId;
            return $http.get(baseUrl + '/GetNextTaskCode/' + tacheId);
        }

        function copyCI(object) {
            return $http.post(baseUrl + '/CopyCI/' + object.source + '/' + object.destination);
        }

        function copyPartialTache(object) {
            return $http.post(baseUrl + '/CopyTachePartial/' + object.source + '/' + object.destination, object.listIdTache );
        }
        function GetTacheLevel1(item) {
            var ciId = item.ciId;
            var level = item.level;
            return $http.get(baseUrl + '/GetActiveTacheByCiAndLevel/' + ciId + '/' + level);
        }

    }
})();