(function () {
    'use strict';

    angular.module('Fred').service('ReceptionTableauService', ReceptionService);

    ReceptionService.$inject = ['$http', '$q'];

    function ReceptionService($http, $q) {

        return {

            /* Service de génération d'excel */
            GenerateExcel: function (texte, soldee, isAbonnement) {
                return $http.get("/api/Reception/GenerateExcel/?text=" + texte + "&soldee=" + soldee + "&isAbonnement=" + isAbonnement);
            },

            /* Récupère un nouveau filtre des dépenses achat de type Réception */
            Filter: function () {
                return $http.get("/api/Reception/Filter");
            },

            /* Récupère la liste des Dépenses Achat de type Réception selon le filtre */
            SearchReceptions: function (filter, page, pageSize) {
                return $http.post("/api/Reception/Search/" + page + "/" + pageSize, filter, { cache: false });
            },

            SearchNextReceptions: function (filter, receptionIds) {
                var datefrom = filter.DateFrom;
                if (filter.DateFrom === undefined) {
                    datefrom = null;
                }
                var dateTo = filter.DateTo;
                if (filter.DateTo === undefined) {
                    dateTo = null;
                }
                var model = {
                    ReceptionIds: receptionIds,
                    DateFrom: datefrom,
                    DateTo: dateTo
                };
                return $http.post("/api/Reception/SearchNextReceptions", model, { cache: false });
            },

            /* Viser les réceptions a partir de leurs Ids*/
            StampReceptionsByIds: function (receptionIds) {
                return $http.post("/api/Reception/StampReceptionsByIds", receptionIds, { cache: false });
            },

            /* Enregister une liste de réceptions */
            UpdateReceptions: function (receptions) {
                return $http.put("/api/Reception/UpdateReceptions/", receptions, { cache: false });
            },

            /* Export des réceptions */
            Export: function (filtre) {
                return $http.post("/api/Reception/Export/", filtre, { cache: false });
            },

            /* Vérifie si au moins une réception possède une date dont la période est bloquée en réception ou non */
            IsBlockedInReception: function (receptions) {
                return $http.post("/api/Reception/IsBlockedInReception/", receptions, { cache: false });
            }


        };
    }
})();
