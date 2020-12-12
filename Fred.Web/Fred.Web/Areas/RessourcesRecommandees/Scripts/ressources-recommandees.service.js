
(function () {
    'use strict';
    angular.module('Fred').service('RessourcesRecommandeesService', RessourcesRecommandeesService);

    RessourcesRecommandeesService.$inject = ['$http'];

    function RessourcesRecommandeesService($http) {
        var uriBase = "/api/RessourcesRecommandees/";

        return {
            get: function (societeId, organisationId) {
                return $http.get(uriBase + '?societeId=' + societeId + '&organisationId=' + organisationId);
            },
            save: function (list) {
                return $http.post(uriBase + 'Save', list);
            }
        };
    }
})();
