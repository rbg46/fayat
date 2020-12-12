
(function () {
    'use strict';

    angular.module('Fred')
        .service('BilanFlashService', BilanFlashService);

    BilanFlashService.$inject = ['$http'];

    function BilanFlashService($http) {

        // Liste des OFs valides pour le rapport
        this.getObjectifsFlashListForRapport = function (rapportModel) {
            return $http.post("/api/ObjectifFlash/GetObjectifFlashRapport", rapportModel, { cache: false });
        };

        // Enregistrement d'une production pour bilan flash
        this.saveObjectifFlashTacheRapportRealise = function (rapportId, tacheRealises) {
            return $http.post("/api/ObjectifFlash/UpdateObjectifFlashTacheRapportRealise/" + rapportId, tacheRealises, { cache: false });
        };

    }
})();