(function () {
    'use strict';

    angular.module('Fred').service('OperationDiverseService', OperationDiverseService);

    OperationDiverseService.$inject = ['$http', '$q'];

    function OperationDiverseService($http, $q) {

        return {
            CanImportEcritureComptables: function (societeId, ciId, date) {
                return $http.get("/api/OperationDiverse/CanImportEcritureComptables/" + societeId + "/" + ciId + "/" + date);
            },

            ImportEcritureComptables: function (societeId, ciId, date) {
                return $http.post("/api/OperationDiverse/ImportEcritureComptables/" + societeId + "/" + ciId + "/" + date);
            },

            ImportEcritureComptablesRange: function (societeId, ciId, dateDebut, dateFin) {
                return $http.post("/api/OperationDiverse/ImportEcritureComptables/" + societeId + "/" + ciId + "/" + dateDebut + "/" + dateFin);
            },

            GetRepartitionsEcarts: function (ciId, month) {
                return $http.get("/api/OperationDiverse/GetRepartitionsEcarts/" + ciId + "/" + month);
            },

            Save: function (ciId, month, operarionDiverses) {
                return $http.post("/api/OperationDiverse/" + ciId + "/" + month, operarionDiverses);
            },

            GetByCiIdAndChapitreCodesAndMonth: function (chapitreCodes, ciId, month) {
                return $http.get("/api/OperationDiverse/GetByCiIdAndChapitreCodesAndMonth/" + chapitreCodes + "/" + ciId + "/" + month);
            },

            GetConsolidationDatas: function (ciId, date) {
                return $http.get("/api/OperationDiverse/GetConsolidationDatas/" + ciId + "/" + date);
            },

            GetConsolidationDatasWithRange: function (ciId, dateDebut, dateFin) {
                return $http.get("/api/OperationDiverse/GetConsolidationDatas/" + ciId + "/" + dateDebut + "/" + dateFin);
            },

            GetEcritureComptables: function (ciId, date, famille) {
                return $http.get("/api/OperationDiverse/GetEcritureComptables/" + ciId + "/" + date + "/" + famille);
            },

            GetNotRelatedOD: function (ciId, date, famille) {
                return $http.get("/api/OperationDiverse/GetNotRelatedOD/" + ciId + "/" + date + "/" + famille);
            },

            GetRelatedOD: function (ciId, date, famille, selectedAccountingEntries) {
                return $http.get("/api/OperationDiverse/GetRelatedOD/" + ciId + "/" + date + "/" + famille + "/" + selectedAccountingEntries);
            },

            GetListFrequenceAbonnement: function () {
                return $http.get("/api/OperationDiverse/GetListFrequenceAbonnement");
            },

            /* Récupération de la dernière date de génération de réception */
            GetLastDayOfODAbonnement: function (firstODAbonnementDate, frequenceAbonnement, nombreReccurence) {
                return $q(function (resolve, reject) {
                    $http.get('/api/OperationDiverse/GetLastDayOfODAbonnement/' + firstODAbonnementDate + '/' + frequenceAbonnement + '/' + nombreReccurence)
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            CreateOD: function (operationDiverse) {
                if (operationDiverse.EstUnAbonnement === true) {
                    return $http.post("/api/OperationDiverse/CreateODAbonnement/", operationDiverse, { cache: false });
                }
                else {
                    return $http.post("/api/OperationDiverse/CreateOD/", operationDiverse, { cache: false });
                }
            },

            Update: function (operationDiverse) {
                if (operationDiverse.EstUnAbonnement === true) {
                    this.UpdateAbonnement(operationDiverse);
                }
                else {
                    return $http.post("/api/OperationDiverse/Update/", operationDiverse, { cache: false });
                }
            },

            UpdateAbonnement: function (operationDiverse) {
                return $http.post("/api/OperationDiverse/UpdateAbonnement/", operationDiverse, { cache: false });
            },

            UpdateList: function (operationsDiverses) {
                return $http.post("/api/OperationDiverse/UpdateList/", operationsDiverses, { cache: false });
            },

            Delete: function (operationDiverse) {
                if (operationDiverse.EstUnAbonnement === true) {
                    return $http.post('/api/OperationDiverse/DeleteAbonnement/', operationDiverse, { cache: false });
                }
                else {
                    return $http.post('/api/OperationDiverse/Delete/', operationDiverse, { cache: false });
                }
              
            },

            GetCiSocietyId: function (organisationCiId) {
                return $http.get("/api/OperationDiverse/GetCiSocietyId/" + organisationCiId);
            },

            //Appel à l'API pour creer un fichier excel
            PostExempleExcelOD: function (ciId, dateComptable) {
                return $http.post('/api/OperationDiverse/GenerateExempleExcel/' + ciId + "/" + dateComptable);
            },

            //Appel à l'API pour afficher un fichier excel
            GetExempleExcelOD: function (reponseId) {
                return window.location.href = '/api/OperationDiverse/ExtractExempleExcel/' + reponseId;
            },

            ImportOperationDiverses: function (date, attachment) {
                return $q(function (resolve, reject) {
                    $http.post('/api/OperationDiverse/ImportOperationDiverses/' + date, attachment, { headers: { 'Content-Type': undefined } })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            PostImportODResult: function (erreurs) {
                return $http.post('/api/OperationDiverse/GenerateExcelImportODResult/', erreurs);
            },

            GetImportODResult: function (reponseId) {
                return window.location.href = '/api/OperationDiverse/ExtractExcelImportODResult/' + reponseId;
            },

            GetPreFillingOperationDiverse: function (ciId, ecritureComptableId, familleOdId) {
                return $http.get("/api/OperationDiverse/GetPreFillingOD/" + ciId + "/" + ecritureComptableId + "/" + familleOdId);
            }
        };
    }
})();