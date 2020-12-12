////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////// HELPERS POUR  DATES CLOTURE COMPTABLE //////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
(function () {
  'use strict';

  angular
      .module('Fred')
      .service('DatesClotureComptableHelperService', DatesClotureComptableHelperService);

  function DatesClotureComptableHelperService() {


    var service = {
      getNow: getNow,
      formatDate: formatDate,
      verifyDatesClotureComptableIsNotNull: verifyDatesClotureComptableIsNotNull,
      dateIsInPast: dateIsInPast,
      dateIsToday: dateIsToday
    };
    return service;


    function getNow() {
      var date = new Date();
      var now = new Date(date.getFullYear(), date.getMonth(), date.getDate(), 12, 0, 0);
      return now;
    }


    function formatDate(date) {
      if (date === null || date === undefined) {
        return null;
      }
      var newDate = new Date(date.getFullYear(), date.getMonth(), date.getDate(), 12, 0, 0);
      return newDate;
    }

    /*
     * Determine si la date est dans le passé.
     */
    function dateIsInPast(date, now) {
      if (date === null || date === undefined) {
        return false;
      }
      var newDate = new Date(date.getFullYear(), date.getMonth(), date.getDate(), 12, 0, 0);
      return newDate < now;
    }

    /*
     * Determine si la date est dans le passé.
     */
    function dateIsToday(date, now) {
      if (date === null || date === undefined) {
        return false;
      }
      //var newDate = new Date(date.getFullYear(), date.getMonth(), date.getDate(), 12, 0, 0);
      return date.getFullYear() === now.getFullYear() && date.getMonth() === now.getMonth() && date.getDate() === now.getDate();
    }

    function verifyDatesClotureComptableIsNotNull(dcc) {
      if (dcc === null) {
        throw new Error('DatesClotureComptable ne peux pas etre null.');
      }
    }

  }
})();

