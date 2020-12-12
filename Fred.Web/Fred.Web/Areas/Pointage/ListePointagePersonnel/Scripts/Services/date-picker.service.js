(function () {
  'use strict';

  angular.module('Fred').service('DatePickerService', DatePickerService);

  DatePickerService.$inject = ['$log', '$filter'];

  function DatePickerService($log, $filter) {

    var periodeSelected = new Date();
    var isPeriodeWeek = false;

    var service = {
      setPeriode: setPeriode,
      getPeriode: getPeriode,
      getDate: getDate,
      setIsPeriodeWeek: setIsPeriodeWeek,
      getIsPeriodeWeek: getIsPeriodeWeek
    };

    return service;

    //////////////////////////////////////////////////////////////////
    // PUBLICS METHODES                                             //
    //////////////////////////////////////////////////////////////////

    function setPeriode(periode) {
      if (periode) {
        periodeSelected = periode;
        return true;
      } else {
        return false;
      }
    }


    function getPeriode() {
      return $filter('date')(periodeSelected, 'MM-dd-yyyy');
    }

    function getDate() {
      return periodeSelected;
    }
    function getIsPeriodeWeek() {
      return  isPeriodeWeek;
    }
    function setIsPeriodeWeek(isWeek) {
      isPeriodeWeek = isWeek;
    }

  }
})();