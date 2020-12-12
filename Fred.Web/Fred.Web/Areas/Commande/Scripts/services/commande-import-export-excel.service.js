(function () {
    'use strict';

    angular
        .module('Fred')
        .service('CommandeImporExportExcelService', CommandeImporExportExcelService);

    CommandeImporExportExcelService.$inject = ['$http', '$q'];

    function CommandeImporExportExcelService($http, $q) {

        return {
            //Appel à l'API pour creer un fichier excel
            OpenFileExempleLignesCommande: function (ciId,isAvenant) {
                return $http.post('/api/commande/GenerateExempleExcel/' + ciId  +'/' + isAvenant);
            },

            //Appel à l'API pour afficher un fichier excel
            GetExempleExcelLignesCommande: function (reponseId, filename) {
                return window.location.href='/api/commande/ExtractBonDeCommande?id='+reponseId +'&numero='+ filename+'&isPdf=false';
            },

            //appel importer les lignes de commandes
            ImportCommadeLignes: function (attachment, checkinValue, ciId, isAvenant) {
                return $http.post(
                    '/api/commande/ImportCommandeLignes/?checkinValue=' + checkinValue + '&ciId=' + ciId+'&isAvenant='+isAvenant,
                    attachment,
                    {
                        cache: false,
                        headers: { 'Content-Type': undefined }
                    }
                );
            }
        };

    }
})();