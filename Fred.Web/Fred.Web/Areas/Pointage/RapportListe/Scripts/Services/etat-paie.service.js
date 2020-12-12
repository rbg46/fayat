(function () {
    'use strict';

    angular.module('Fred').service('EtatPaieService', EtatPaieService);

    EtatPaieService.$inject = ['$http', '$q'];

    function EtatPaieService($http, $q) {

        return {

            /* Service de génération d'excel */
            GenerateExcelVerificationTemps: function (etatPaieExportModel) {
                return $q(function (resolve, reject) {
                    $http.post('/api/EtatPaie/GenerateExcelVerificationTemps', etatPaieExportModel)
                        .success(function (data) {
                            var url = '/api/EtatPaie/ExtractDocument/' + data.id + '/' + 'VerificationTemps' + new Date(etatPaieExportModel.Annee, etatPaieExportModel.Mois, 1).toISOString().slice(0, 10).replace(/-/g, '').substring(0, 6) + '/' + etatPaieExportModel.Pdf;
                            window.location.href = url;
                            resolve(data);
                        })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service de génération d'excel */
            GenerateExcelControlePointages: function (etatPaieExportModel) {
                return $q(function (resolve, reject) {
                    $http.post('/api/EtatPaie/GenerateExcelControlePointages', etatPaieExportModel)
                        .success(function (data) {
                            window.location.href = '/api/EtatPaie/ExtractDocument/' + data.id + '/' + 'ControlePointages' + new Date(etatPaieExportModel.Annee, etatPaieExportModel.Mois, 1).toISOString().slice(0, 10).replace(/-/g, '').substring(0, 6) + '/' + etatPaieExportModel.Pdf;
                            resolve(data);
                        })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service de génération d'excel Spécifiques a Fes*/
            GenerateExcelControlePointagesFes: function (etatPaieExportModel) {
                return $q(function (resolve, reject) {
                    $http.post('/api/EtatPaie/GenerateExcelControlePointagesFes', etatPaieExportModel)
                        .success(function (data) {
                            window.location.href = '/api/EtatPaie/ExtractDocument/' + data.id + '/' + 'ControlePointages' + new Date(etatPaieExportModel.Annee, etatPaieExportModel.Mois, 1).toISOString().slice(0, 10).replace(/-/g, '').substring(0, 6) + '/' + etatPaieExportModel.Pdf;
                            resolve(data);
                        })
                        .error(function (data) { reject(data); });
                });
            },
            
            /* Service de génération d'excel */
            GenerateExcelControlePointagesHebdomadaire: function (etatPaieExportModel) {
                return $q(function (resolve, reject) {
                    $http.post('/api/EtatPaie/GenerateExcelControlePointagesHebdomadaire', etatPaieExportModel)
                        .success(function (data) {
                            window.location.href = '/api/EtatPaie/ExtractDocument/' + data.id + '/' + 'ControlePointages' + etatPaieExportModel.Date.toISOString().slice(0, 10).replace(/-/g, '').substring(0, 6) + '/' + etatPaieExportModel.Pdf;
                            resolve(data);
                        })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service de génération du document pdf ou excel pour la liste des primes */
            GenerateDocumentListePrimes: function (etatPaieExportModel) {                
                return $q(function (resolve, reject) {
                    $http.post('/api/EtatPaie/GenerateDocumentListePrimes', etatPaieExportModel)
                        .success(function (data) {
                            window.location.href = '/api/EtatPaie/ExtractDocument/' + data.id + '/' + 'ListePrimes' + new Date(etatPaieExportModel.Annee, etatPaieExportModel.Mois, 1).toISOString().slice(0, 10).replace(/-/g, '').substring(0, 6) + '/' + etatPaieExportModel.Pdf;
                            resolve(data);
                        })
                        .error(function (data) { reject(data); });
                });
            },


            /* Service de génération du document pdf ou excel pour la liste des IGD */
            GenerateDocumentListeIGD: function (etatPaieExportModel) {
                return $q(function (resolve, reject) {
                    $http.post('/api/EtatPaie/GenerateDocumentListeIGD', etatPaieExportModel)
                        .success(function (data) {
                            window.location.href = '/api/EtatPaie/ExtractDocument/' + data.id + '/' + 'ListeIGD' + new Date(etatPaieExportModel.Annee, etatPaieExportModel.Mois, 1).toISOString().slice(0, 10).replace(/-/g, '').substring(0, 6) + '/' + etatPaieExportModel.Pdf;
                            resolve(data);
                        })
                        .error(function (data) { reject(data); });
                });
            },

            /* Service de génération du document pdf ou excel pour la situation de salarie Acompte */
            GenerateExcelSalarieAcompte: function (etatPaieExportModel) {
                return $q(function (resolve, reject) {
                    $http.post('/api/EtatPaie/GenerateExcelSalarieAcompte', etatPaieExportModel)
                        .success(function (data) {
                            window.location.href = '/api/EtatPaie/ExtractDocument/' + data.id + '/' + 'SalarieAcompte' + new Date(etatPaieExportModel.Annee, etatPaieExportModel.Mois, 1).toISOString().slice(0, 10).replace(/-/g, '').substring(0, 6) + '/' + etatPaieExportModel.Pdf;
                            resolve(data);
                        })
                        .error(function (data) { reject(data); });
                });
            },
            /* Service de génération du document pdf ou excel pour la liste des Heures Spécifiques */
            GenerateDocumentListeHeuresSpecifiques: function (etatPaieExportModel) {
                return $q(function (resolve, reject) {
                    $http.post('/api/EtatPaie/GenerateDocumentListeHeuresSpecifiques', etatPaieExportModel)
                        .success(function (data) {
                            window.location.href = '/api/EtatPaie/ExtractDocument/' + data.id + '/' + 'ListeHeuresSpecifiques' + new Date(etatPaieExportModel.Annee, etatPaieExportModel.Mois, 1).toISOString().slice(0, 10).replace(/-/g, '').substring(0, 6) + '/' + etatPaieExportModel.Pdf;
                            resolve(data);
                        })
                        .error(function (data) { reject(data); });
                });
            },
            /* Service de génération du document pdf ou excel pour la liste des Absences */
            GenerateExcelListAbsencesMensuels: function (etatPaieExportModel) {
                return $q(function (resolve, reject) {
                    $http.post('/api/EtatPaie/GenerateExcelListAbsencesMensuels', etatPaieExportModel)
                        .success(function (data) {
                            window.location.href = '/api/EtatPaie/ExtractDocument/' + data.id + '/' + 'ListeAbsencesMensuelles' + new Date(etatPaieExportModel.Annee, etatPaieExportModel.Mois, 1).toISOString().slice(0, 10).replace(/-/g, '').substring(0, 6) + '/' + etatPaieExportModel.Pdf;
                            resolve(data);
                        })
                        .error(function (data) { reject(data); });
                });
            },

            GetEtatPaieExportModel: function() {
                return $http.get("/api/EtatPaie/GetEtatPaieExportModel");
            }
        };
    }
})();