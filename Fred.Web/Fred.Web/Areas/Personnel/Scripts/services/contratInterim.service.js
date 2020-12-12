(function () {
    'use strict';
  
    angular.module('Fred').service('ContratInterimService', ContratInterimService);
  
    ContratInterimService.$inject = ['$http', '$filter'];
  
    function ContratInterimService($http, $filter) {

      this.GetContratInterimByPersonnelId = function (personnelId) {
        return $http.get("/api/ContratInterimaire/Personnel/"+ personnelId);
      };
      
      this.GetMotifRemplacement = function () {
        return $http.get("/api/ContratInterimaire/MotifRemplacement");
      };

      this.GetCiDevise = function (ciid){
        return $http.get("/api/CI/DeviseRef/"+ ciid);
      }

      this.GetPointageForContratInterimaire = function (contratInterim) {
        var contratInterimCopy = angular.copy(contratInterim);
        contratInterimCopy = initContrat(contratInterimCopy);
        return $http.post("/api/ContratInterimaire/Pointage", contratInterimCopy);
      };

      this.GetCiInRapportLigneByDateContratInterimaire = function (contratInterim) {
        var contratInterimCopy = angular.copy(contratInterim);
        contratInterimCopy = initContrat(contratInterimCopy);
        return $http.post("/api/ContratInterimaire/LibelleCi", contratInterimCopy);
      };

      this.GetContratInterimByNumeroContrat = function (numContrat, contratInterimaireId) {
        var numeroContratInterimaireModel = {
          NumContrat : numContrat,
          ContratInterimaireId : contratInterimaireId
        }
        return $http.post("/api/ContratInterimaire/Numero", numeroContratInterimaireModel);
      };

      this.GetContratInterimActif = function (contratInterimaireId, interimaireId, dateDebut, dateFin) {
        dateDebut = getPeriode(dateDebut);
        dateFin = getPeriode(dateFin);
        return $http.get("/api/ContratInterimaire/Actif/" + contratInterimaireId + "/" + interimaireId + "/" + dateDebut + "/" + dateFin);
      };

      this.AddContratInterim = function (contratInterim) {
        var contratInterimCopy = angular.copy(contratInterim);
        contratInterimCopy = initContrat(contratInterimCopy);
        return $http.post("/api/ContratInterimaire/Add", contratInterimCopy);
      };

      this.UpdateContratInterim = function (contratInterim) {
        var contratInterimCopy = angular.copy(contratInterim);
        contratInterimCopy = initContrat(contratInterimCopy);
        return $http.put("/api/ContratInterimaire/Update", contratInterimCopy);
      };

      this.DeleteContratInterim = function (contratInterimId) {
        return $http.delete("/api/ContratInterimaire/Delete/"+ contratInterimId);
      };
  
      function initContrat(contratInterim){
        contratInterim.Interimaire = null;
        contratInterim.Societe = null;
        contratInterim.Fournisseur = null;
        contratInterim.Ci = null;
        contratInterim.Ressource = null;
        contratInterim.Unite = null;
        contratInterim.MotifRemplacement = null;
        contratInterim.PersonnelRemplace = null;
        contratInterim.CommandeContratInterimaires = null;
        for (var i = 0; i < contratInterim.ZonesDeTravail.length; i++) {
            contratInterim.ZonesDeTravail[i].Contrat = null;
            contratInterim.ZonesDeTravail[i].EtablissementComptable = null;
        }
        return contratInterim;
      }

      function getPeriode(date) {
        return $filter('date')(date, 'MM-dd-yyyy');
      }
   }
  })();