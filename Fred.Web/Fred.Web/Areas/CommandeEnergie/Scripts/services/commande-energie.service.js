(function () {
    'use strict';

    angular.module('Fred').service('CommandeEnergieService', CommandeEnergieService);

    CommandeEnergieService.$inject = ['$http', '$filter'];

    function CommandeEnergieService($http, $filter) {

        var service =
        {
            Search: Search,
            GetFilter: GetFilter,
            Preloading: Preloading,
            Add: Add,
            Update: Update,
            Delete: Delete,
            Get: Get,
            ExportExcel: ExportExcel,
            DownloadExportFile: DownloadExportFile,
            Validate: Validate
        };

        return service;

        function Search(filter) {
            var period = $filter('date')(filter.Periode, 'MM-dd-yyyy');
            return $http.get(`/api/CommandeEnergie/Search?periode=${period}&ciId=${filter.Ci ?filter.Ci.CiId:null}&fournisseurId=${filter.Fournisseur? filter.Fournisseur.FournisseurId:null}&page=${filter.page||1}&pageSize=${filter.pageSize||20}`);
        }

        function GetFilter(periode, ciId, fournisseurId, page, pageSize) {
            var filter =
            {
                periode: periode ? periode : null,
                ciId: ciId ? ciId : null,
                ci: null,
                fournisseurId: fournisseurId ? fournisseurId : null,
                fournisseur: null,
                page: page ? page : null,
                pageSize: pageSize ? pageSize : null
            };

            return filter;
        }

        function Preloading(commande) {
            return $http.post(`/api/CommandeEnergie/Preloading`, commande);
        }

        function Get(commandeId) {
            return $http.get(`/api/CommandeEnergie/${commandeId}`);
        }

        function Add(commande) {
            return $http.post(`/api/CommandeEnergie/`, commande);
        }

        function Update(commande) {
            return $http.put(`/api/CommandeEnergie/`, commande);
        }

        function Delete(commandeId) {
            return $http.delete(`/api/CommandeEnergie/${commandeId}`);
        }

        function Validate(commande) {
            return $http.post(`/api/CommandeEnergie/Validate`, commande);
        }

        function ExportExcel(commandeId) {
            return $http.post(`/api/CommandeEnergie/GenerateExportCommandeEnergie/${commandeId}`);
        }

        function DownloadExportFile(guidIdExport, fileName) {
            window.location.href = '/api/CommandeEnergie/GetExport/' + guidIdExport + '/' + fileName;
        }
    }
})();
