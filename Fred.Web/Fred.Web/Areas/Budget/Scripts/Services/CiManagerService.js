
(function () {
  'use strict';

  angular
      .module('Fred')
      .service('CiManagerService', CiManagerService);

  CiManagerService.$inject = ['$log'];

  function CiManagerService($log) {

    var ciSelected = null;

    var service = {
      setCi: setCi,
      getCi: getCi
    };

    return service;

    //////////////////////////////////////////////////////////////////
    // PUBLICS METHODES                                             //
    //////////////////////////////////////////////////////////////////


    function setCi(ci) {
      ciSelected = ci;
    }


    function getCi() {
      return ciSelected;
    }

  }
})();

