(function () {
    'use strict';
  
    angular.module('Fred').service('DelegationService', DelegationService);
  
    DelegationService.$inject = ['$http'];
  
    function DelegationService($http) {


      this.GetDelegationByPersonnelId = function (personnelId) {
        return $http.get("/api/Delegation/"+ personnelId);
      };

      this.GetDelegationAlreadyActive = function (delegation) {
        var delegationCopy = angular.copy(delegation)
        delegationCopy = initPersonnel(delegationCopy)
        return $http.post("/api/Delegation/Active", delegationCopy);
      };

      this.AddDelegation = function (delegation) {
        var delegationCopy = angular.copy(delegation)
        delegationCopy = initPersonnel(delegationCopy)
        return $http.post("/api/Delegation/Add", delegationCopy);
      };

      this.UpdateDelegation = function (delegation) {
        var delegationCopy = angular.copy(delegation)
        delegationCopy = initPersonnel(delegationCopy)
        return $http.put("/api/Delegation/Update", delegationCopy);
      };

      this.DesactivateDelegation = function (delegation) {
        var delegationCopy = angular.copy(delegation)
        delegationCopy = initPersonnel(delegationCopy)
        return $http.put("/api/Delegation/Desactivate", delegationCopy);
      };

      this.DeleteDelegation = function (delegation) {
        var delegationCopy = angular.copy(delegation)
        delegationCopy = initPersonnel(delegationCopy)
        return $http.put("/api/Delegation/Delete", delegationCopy);
      };
  
      function initPersonnel(delegation){
        delegation.PersonnelAuteur = null;
        delegation.PersonnelDelegant = null;
        delegation.PersonnelDelegue = null;
        return delegation;
      }
   }
  })();