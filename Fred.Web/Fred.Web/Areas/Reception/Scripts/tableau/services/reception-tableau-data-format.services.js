/*
 * Ce service sert a formater les données recu du back
 */
(function () {
    'use strict';

    angular.module('Fred').service('ReceptionTableauDataFormatService', ReceptionTableauDataFormatService);

    ReceptionTableauDataFormatService.$inject = ['$filter'];

    function ReceptionTableauDataFormatService($filter) {

        var service = {
            formatReception: formatReception
        };
        return service;

        /**
         * @description sert aformater les données du serveur
         * @param {reception} reception sur laquelle on fait le formatage
        */
        function formatReception(reception) {

            reception.Date = $filter('toLocaleDate')(reception.Date);
            reception.DateComptable = $filter('toLocaleDate')(reception.DateComptable);
            reception.DateVisaReception = $filter('toLocaleDate')(reception.DateVisaReception);
            reception.DateCreation = $filter('toLocaleDate')(reception.DateCreation);
            reception.DateTransfertFar = $filter('toLocaleDate')(reception.DateTransfertFar);
            reception.Date = $filter('toLocaleDate')(reception.Date);

            if (reception.CommandeLigne && reception.CommandeLigne.Commande) {
                reception.CommandeLigne.Commande.Date = $filter('toLocaleDate')(reception.CommandeLigne.Commande.Date);
            }

            if (reception.FacturationsReception && reception.FacturationsReception.length > 0) {

                var datePieceSapList = [];
                var dateComptableList = [];
                var montantTotalHTList = [];

                angular.forEach(reception.FacturationsReception, function (facturation) {
                    facturation.DatePieceSap = $filter('toLocaleDate')(facturation.DatePieceSap);
                    facturation.DateCreation = $filter('toLocaleDate')(facturation.DateCreation);
                    facturation.DateSaisie = $filter('toLocaleDate')(facturation.DateSaisie);
                    facturation.DateComptable = $filter('toLocaleDate')(facturation.DateComptable);

                    datePieceSapList.push($filter('date')(facturation.DatePieceSap, 'dd/MM/yyyy'));
                    dateComptableList.push($filter('date')(facturation.DateComptable, 'dd/MM/yyyy'));
                    montantTotalHTList.push($filter('number')(facturation.MontantTotalHT, 2) + " " + reception.Devise.Symbole);
                });

                reception.DatesFacture = datePieceSapList.join(' ; ');
                reception.DatesRapprochement = dateComptableList.join(' ; ');
                reception.MontantTotauxHT = montantTotalHTList.join(' ; ');
            }

        }
    }

})();
