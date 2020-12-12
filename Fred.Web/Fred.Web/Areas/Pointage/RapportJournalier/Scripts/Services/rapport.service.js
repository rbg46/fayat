(function () {
    'use strict';

    angular.module('Fred').service('RapportService', RapportService);

    RapportService.$inject = ['$http', '$q'];

    function RapportService($http, $q) {

        return {
            /* Service de création d'une nouvelle instance de rapport */
            New: function (CiId) {
                return $q(function (resolve, reject) {
                    $http.get('/api/Rapport/New/' + CiId, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Duplication d'un rapport sur une periode */
            Duplicate: function (duplicateModel) {
                return $http.post('/api/Rapport/Duplicate', duplicateModel);
            },

            /* Duplication d'un rapport sur une periode */
            DuplicateForNewCI: function (duplicateModel) {
                return $http.post('/api/Rapport/DuplicateRapportForNewCi', duplicateModel);
            },
            /* Service de récupéartion d'une instance de rapport */
            Get: function (rapportId, duplicate, validate) {
                return $q(function (resolve, reject) {
                    $http.get('/api/Rapport/Get/' + rapportId + '/' + duplicate + '/' + validate, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service Create */
            AddOrUpdateRapport: function (rapportModel) {
                return $q(function (resolve, reject) {
                    $http.post("/api/Rapport/AddOrUpdateRapport", rapportModel, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service Validation */
            CheckRapport: function (rapportModel) {
                return $q(function (resolve, reject) {
                    $http.post('/api/Rapport/CheckRapport', JSON.stringify(rapportModel), { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service Validation */
            ValidationRapport: function (rapportModel) {
                return $q(function (resolve, reject) {
                    $http.post('/api/Rapport/ValidationRapport', rapportModel, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            VerrouillerRapport: function (rapportModel) {
                return $q(function (resolve, reject) {
                    $http.post('/api/Rapport/VerrouillerRapport', rapportModel, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            DeverrouillerRapport: function (rapportModel) {
                return $q(function (resolve, reject) {
                    $http.post('/api/Rapport/DeverrouillerRapport', rapportModel, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            // Ajout d'une nouvelle ligne vide
            AddNewRapportLigne: function (rapportModel) {
                return $q(function (resolve, reject) {
                    $http.post("/api/Rapport/AddNewRapportLigneToRapport", rapportModel, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            // Supprime un rapport
            DeleteRapport: function (rapportModel) {
                return $q(function (resolve, reject) {
                    $http.post("/api/Rapport/DeleteRapport", rapportModel, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            ApplyRGCIOnRapport: function (rapport) {
                return $q(function (resolve, reject) {
                    $http.post('/api/Rapport/ApplyRGCIOnRapport', rapport)
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            ApplyRGCodeAbsenceOnPointage: function (pointage) {
                return $q(function (resolve, reject) {
                    $http.post('/api/Rapport/ApplyRGCodeAbsenceOnPointage', pointage)
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            GetOrCreateIndemniteDeplacement: function (pointage) {
                return $q(function (resolve, reject) {
                    $http.post('/api/Rapport/GetOrCreateIndemniteDeplacement', pointage)
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            RefreshIndemniteDeplacement: function (pointage) {
                return $q(function (resolve, reject) {
                    $http.post('/api/Rapport/RefreshIndemniteDeplacement', pointage)
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            InitializeAstreintesInformations: function (pointage) {
                return $q(function (resolve, reject) {
                    $http.post('/api/Rapport/InitializeAstreintesInformations', pointage)
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            FulfillAstreintesInformations: function (pointage) {
                return $q(function (resolve, reject) {
                    $http.post('/api/Rapport/FulfillAstreintesInformations', pointage)
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            GetMaterielDefault: function (personnelId) {
                return $q(function (resolve, reject) {
                    $http.get('/api/Personnel/GetMaterielDefault/' + personnelId)
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            GetUserPaieLevel: function (ciId) {
                return $q(function (resolve, reject) {
                    $http.get('/api/Utilisateur/GetUserPaieLevel/' + ciId + '/', ciId, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            IsGSP: function (ciId) {
                return $q(function (resolve, reject) {
                    $http.get('/api/Utilisateur/IsGSP/' + ciId + '/', ciId, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            GetPersonnelGroupebyId: function (personnelId) {
                return $q(function (resolve, reject) {
                    $http.get('/api/Personnel/GetPersonnelGroupebyId/' + personnelId)
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            }

        };
    }
})();
