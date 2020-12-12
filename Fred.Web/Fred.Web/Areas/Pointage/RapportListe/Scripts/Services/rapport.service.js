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

            /* Service de création d'une nouvelle instance de rapport */
            NewPointageMensuel: function () {
                return $q(function (resolve, reject) {
                    $http.get('/api/Rapport/NewPointageMensuel/', { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Duplication d'un rapport sur une periode */
            duplicate: function (duplicateModel) {
                return $http.post('/api/Rapport/Duplicate', duplicateModel);
            },

            /* Duplication d'un rapport sur une periode */
            duplicateForNewCI: function (duplicateModel) {
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

            GetListRapportIdWithError: function (listRapportId) {
                return $q(function (resolve, reject) {
                    $http.post('/api/Rapport/GetListRapportIdWithError', JSON.stringify(listRapportId), { cache: false })
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

            // Ajout d'une prime dans le rapport
            AddPrimeToRapport: function (rapportAndPrimeModel) {
                return $q(function (resolve, reject) {
                    $http.post("/api/Rapport/AddPrimeToRapport", rapportAndPrimeModel, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            // Ajout d'une tache dans le rapport
            AddTacheToRapport: function (rapportAndTacheModel) {
                return $q(function (resolve, reject) {
                    $http.post("/api/Rapport/AddTacheToRapport", rapportAndTacheModel, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            // Supprime un rapport
            DeleteRapport: function (rapportModel, fromListeRapport) {
                if (fromListeRapport) {
                    return $q(function (resolve, reject) {
                        $http.post("/api/Rapport/DeleteRapport/" + fromListeRapport, rapportModel, { cache: false })
                            .success(function (data) { resolve(data); })
                            .error(function (data) { reject(data); });
                    });
                }
                else {
                    return $q(function (resolve, reject) {
                        $http.post("/api/Rapport/DeleteRapport", rapportModel, { cache: false })
                            .success(function (data) { resolve(data); })
                            .error(function (data) { reject(data); });
                    });
                }
            },

            // Récupération de la liste des rapports
            SearchRapportWithFilters: function (filters, page, pageSize) {
                var filtersCopy = angular.copy(filters);
                filtersCopy.Organisation = null;
                return $q(function (resolve, reject) {
                    $http.post('/api/Rapport/SearchRapportWithFilters/' + page + "/" + pageSize, filtersCopy)
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            // Vérifie si un rapport peut être supprimé
            CanBeDeleted: function (rapportModel) {
                return $q(function (resolve, reject) {
                    $http.post('/api/Rapport/CanBeDeleted', rapportModel, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service Récupération de la liste des pointages journaliers */
            GetPointageMensuel: function (annee, mois, organisation, organisationId, tri) {
                return $q(function (resolve, reject) {
                    $http.get('/api/Rapport/GetPointageMensuel/' + annee + '/' + mois + '/' + organisation + '/' + organisationId + '/' + tri + '/', annee, mois, organisation, organisationId, tri, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service de génération d'excel par CI */
            GenerateExcel: function (annee, mois, organisation, organisationId, tri) {
                return $q(function (resolve, reject) {
                    $http.get('/api/Rapport/GenerateExcel/' + annee + '/' + mois + '/' + organisation + '/' + organisationId + '/' + tri + '/', annee, mois, organisation, organisationId, tri, { cache: false })
                        .success(function (data) {
                            window.location.href = '/api/Rapport/ExtractExcel/' + data.id;
                            resolve(data);
                        })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service de génération d'excel par personnel */
            GenerateExcelPerson: function (annee, mois, organisation, organisationId, tri) {
                return $q(function (resolve, reject) {
                    $http.get('/api/Rapport/GenerateExcelPerson/' + annee + '/' + mois + '/' + organisation + '/' + organisationId + '/' + tri + '/', annee, mois, organisation, organisationId, tri, { cache: false })
                        .success(function (data) {
                            window.location.href = '/api/Rapport/ExtractExcel/' + data.id;
                            resolve(data);
                        })
                        .error(function (data) { reject(data); });
                });
            },

            GeneratePDF: function (annee, mois, organisation, organisationId, tri) {
                return $q(function (resolve, reject) {
                    $http.get('/api/Rapport/GeneratePdfPointageMensuel/' + annee + '/' + mois + '/' + organisation + '/' + organisationId + '/' + tri + '/', annee, mois, organisation, organisationId, tri, { cache: false })
                        .success(function (data) {
                            //window.location.href = '/api/Rapport/ExtractExcel/' + data.id;
                            resolve(data);
                        })
                        .error(function (data) { reject(data); });
                });
            },

            // Service Get objet de recherche
            GetFilter: function () {
                return $q(function (resolve, reject) {
                    $http.get('/api/Rapport/Filter/', { cache: false })
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

            ApplyRGCodeDeplacementOnPointage: function (pointage) {
                return $q(function (resolve, reject) {
                    $http.post('/api/Rapport/ApplyRGCodeDeplacementOnPointage', pointage)
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

            IsRoleChantier: function (ciId) {
                return $q(function (resolve, reject) {
                    $http.get('/api/Utilisateur/IsRoleChantier/' + ciId + '/', ciId, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            IsRolePaie: function (ciId) {
                return $q(function (resolve, reject) {
                    $http.get('/api/Utilisateur/IsRolePaie/' + ciId + '/', ciId, { cache: false })
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
            },

            GetPointageVerrouillerByPersonnelId: function (personnelId) {
                return $http.get("/api/Rapport/Pointage/Personnel/Verrouiller/" + personnelId);
            },

            GetSocieteByOrganisationId: function (organisationId) {
                return $http.get("/api/Societe/Organisation/" + organisationId);
            },

            VerrouillerAll: function (filter) {
                return $q(function (resolve, reject) {
                    $http.post('/api/Rapport/Verrouiller/All', filter, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            DeverrouillerAll: function (filter) {
                return $q(function (resolve, reject) {
                    $http.post('/api/Rapport/Deverrouiller/All', filter, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            VerrouillerList: function (list) {
                return $q(function (resolve, reject) {
                    $http.post('/api/Rapport/Verrouiller/List', list, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            DeverrouillerList: function (list) {
                return $q(function (resolve, reject) {
                    $http.post('/api/Rapport/Deverrouiller/List', list, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            IsExcelControlePointagesNotEmpty: function (etatPaieExportModel) {
                return $q(function (resolve, reject) {
                    $http.post('/api/Rapport/IsExcelControlePointagesNotEmpty', etatPaieExportModel, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            IsUserHasMenuEditionPermission: function (ciId) {
                return $q(function (resolve, reject) {
                    $http.get('/api/Utilisateur/IsUserHasMenuEditionPermission/' + ciId + '/', ciId, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            ExporterAnalytiqueForTibco: function (model) {
                return $q(function (resolve, reject) {
                    $http.post('/api/Rapport/ExporterAnalytiqueForTibco', model, { cache: false })
                       .success(function (data) { resolve(data); })
                       .error(function (data) { reject(data); });
                });
            },

            ControlerSaisiesForTibco: function (model) {
                return $q(function (resolve, reject) {
                    $http.post('/api/Rapport/ControlerSaisiesForTibco', model, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            },

            RecupererAllEtablissementCompta: function (organisationId) {
                return $q(function (resolve, reject) {
                    $http.get('/api/EtablissementComptable/SearchLight/?organisationPereId=' + organisationId, { cache: false })
                        .success(function (data) { resolve(data); })
                        .error(function (data) { reject(data); });
                });
            }
        };
    }
})();