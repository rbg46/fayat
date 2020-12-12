
(function () {
  'use strict';

  angular
      .module('Fred')
      .service('DevisesManagerService', DevisesManagerService);

  DevisesManagerService.$inject = ['$log'];

  function DevisesManagerService($log) {

    var devises = null;

    var service = {
      setDevises: setDevises,
      getDevises: getDevises
    };

    return service;

    //////////////////////////////////////////////////////////////////
    // PUBLICS METHODES                                             //
    //////////////////////////////////////////////////////////////////


    function setDevises(ci) {
      devises = ci;
    }


    function getDevises() {
      return devises;
    }

  }
})();

