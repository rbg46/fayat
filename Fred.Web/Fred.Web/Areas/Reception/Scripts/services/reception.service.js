(function () {
    'use strict';

    angular.module('Fred').service('ReceptionService', ReceptionService);

    ReceptionService.$inject = ['$http', '$q'];

    function ReceptionService($http, $q) {

        return {

            /* Service de création d'une nouvelle instance de commande */
            New: function (commandeLigneId) {
                return $q(function (resolve, reject) {
                    $http.get("/api/Reception/New/" + commandeLigneId, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },
            /* Service Save */
            Save: function (reception) {
                return $http.post("/api/Reception/", reception, { cache: false });
            },
            /* Service Update */
            Update: function (reception) {
                return $http.put("/api/Reception/", reception, { cache: false });
            },
            /* Service Delete */
            Delete: function (id) {
                return $http.put("/api/Reception/" + id, { cache: false });
            },

            IsLimitationUnitesRessource: function (societeId) {
                return $http.get("/api/Societe/IsModificationRessourceReceptions/" + societeId, { cache: false });
            },

            /* Service de récupération des commandes externes */
            GetNbCommandesBuyer: function (etabCode, dateDebut, dateFin) {
                return $q(function (resolve, reject) {
                    $http.get("/api/Commande/GetNombreCommandesBuyer/" + etabCode + "/" + dateDebut + "/" + dateFin, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service de génération d'excel */
            GenerateExcel: function (filter) {
                return $http.post("/api/Reception/GenerateExcel/", filter, { cache: false });
            },

            /* Service d'import des commandes externes */
            ImportCommandesBuyer: function (etabCode, dateDebut, dateFin) {
                return $q(function (resolve, reject) {
                    $http.post("/api/Commande/ImporterCmdsBuyer/" + etabCode + "/" + dateDebut + "/" + dateFin, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Vérifie si un CI est bloqué en réception ou non */
            IsCiBlockedInReception: function (ciId, period) {
                return $http.get("/api/Reception/IsCiBlockedInReception/" + ciId + "/" + period);
            },

            /* Dupliquer la réception */
            Duplicate: function (commandeLigneId, receptionId) {
                return $http.get("/api/Reception/Duplicate/" + commandeLigneId + "/" + receptionId);
            },
            /* Récupère la liste des commandes à réceptionner */
            GetCommandesToReceive: function (filter, page, pageSize) {
                return $http.post("/api/Reception/GetCommandesToReceive/" + page + "/" + pageSize, filter);
            },
            /* Verrouille une commande ligne */
            Lock: function (commandeLigneId) {
                return $http.put("/api/Reception/CommandeLigne/" + commandeLigneId + "/Lock");
            },
            /* DéVerrouille une commande ligne */
            UnLock: function (commandeLigneId) {
                return $http.put("/api/Reception/CommandeLigne/" + commandeLigneId + "/Unlock");
            },
            /*Donne l'info comme quoi la ligne est verrouillé */
            IsLocked: function (commandeLigneId) {
                return $http.put("/api/Reception/CommandeLigne/" + commandeLigneId + "/IsLocked");
            }

        };
    }
})();
