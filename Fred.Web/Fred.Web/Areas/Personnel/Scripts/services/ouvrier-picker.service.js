(function () {
  'use strict';

  angular.module('Fred').service('OuvrierPickerService', OuvrierPickerService);

  OuvrierPickerService.$inject = ['$log'];

  function OuvrierPickerService($log) {

    var ouvrierSelected = null;

    var service = {
      setOuvrier: setOuvrier,
      getOuvrier: getOuvrier
    };

    return service;

    //////////////////////////////////////////////////////////////////
    // PUBLICS METHODES                                             //
    //////////////////////////////////////////////////////////////////

    function setOuvrier(ouvrier) {
      ouvrierSelected = ouvrier;
    }


    function getOuvrier() {
      return ouvrierSelected;
    }

  }
})();