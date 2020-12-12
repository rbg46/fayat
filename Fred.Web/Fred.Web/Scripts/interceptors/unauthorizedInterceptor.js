/*
 * Cet interceptor permet de faire un redirect si une 401
 * est recu par angular lors d'un appel http.
 */
(function () {
  'use strict';

  angular
      .module('Fred')
      .factory('unauthorizedInterceptor', unauthorizedInterceptor);

  unauthorizedInterceptor.$inject = ['$q'];

  function unauthorizedInterceptor($q) {

   
    var service = {
      responseError: responseError    
    };
    return service;

    function responseError(response) {
      if (response.status == 401) {
        var actualLocation = window.location.pathname;
        window.location = "/Authentication/Connect?ReturnUrl=" + actualLocation;
      }
      return $q.reject(response);
    };  
  }

})();
