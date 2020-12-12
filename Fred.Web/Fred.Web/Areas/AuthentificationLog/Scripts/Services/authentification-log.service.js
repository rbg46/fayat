/*
 * Ce service sert a recuperer les informations sur l'utilisateur courrant.
 */
(function () {
  'use strict';

  angular
      .module('Fred')
      .service('authentificationLogService', authentificationLogService);

  authentificationLogService.$inject = ['$http'];

  function authentificationLogService($http) {
       
    var service = {
      getByLogin: getByLogin,
      getDetail: getDetail,
      deleteAuthentificationLogs: deleteAuthentificationLogs
    };

    return service;

    /*
     * Recupere les logs
     */
    function getByLogin(login,skip,take) {
      return $http.get("/api/AuthentificationLog/GetByLogin?login=" + login + "&skip=" + skip + "&take=" + take);
    }   


    function getDetail(id) {
      return $http.get("/api/AuthentificationLog/GetDetail/" + id);
    }


    function deleteAuthentificationLogs(ids) {
      return $http({
        url: "/api/AuthentificationLog/deleteAuthentificationLogs",
        data: { Ids: ids },
        headers: { 'Content-Type': 'application/json' },
        method: "DELETE"
      });
    }
  }
})();


