(function () {
    'use strict';
  
    angular.module('Fred').service('ZoneDeTravailService', ZoneDeTravailService);
  
    ZoneDeTravailService.$inject = ['$http'];
  
    function ZoneDeTravailService($http) {

        this.GetZoneDeTravailByContratId = function (contratId) {
            return $http.get("/api/ZoneDeTravail/"+ contratId);
          };

        this.AddZoneDeTravail = function (zoneDeTravail) {
            var zoneDeTravailCopy = angular.copy(zoneDeTravail);
            zoneDeTravailCopy = initZoneDeTravail(zoneDeTravailCopy);
            return $http.post("/api/ZoneDeTravail/Add", zoneDeTravailCopy);
        };

        this.DeleteZoneDeTravail = function (zoneDeTravail) {
            var zoneDeTravailCopy = angular.copy(zoneDeTravail);
            zoneDeTravailCopy = initZoneDeTravail(zoneDeTravailCopy);
            return $http.post("/api/ZoneDeTravail/Delete", zoneDeTravailCopy);
        };
    
        function initZoneDeTravail(zoneDeTravail){
            zoneDeTravail.EtablissementComptable = null;
            zoneDeTravail.Contrat = null;
            return zoneDeTravail;
        }
   }
})();