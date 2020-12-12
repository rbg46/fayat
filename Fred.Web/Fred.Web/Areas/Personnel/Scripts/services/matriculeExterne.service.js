(function () {
    'use strict';
  
    angular.module('Fred').service('MatriculeExterneService', MatriculeExterneService);
  
    MatriculeExterneService.$inject = ['$http'];
  
    function MatriculeExterneService($http) {

      this.GetMatriculeExterneByMatriculeAndSource = function (matriculeExterne) {
        return $http.post("/api/MatriculeExterne/Exist", matriculeExterne);
      };

      this.GetMatriculeExterneByPersonnelId = function (personnelId) {
        return $http.get("/api/MatriculeExterne/Personnel/"+ personnelId);
      };
      

   }
  })();