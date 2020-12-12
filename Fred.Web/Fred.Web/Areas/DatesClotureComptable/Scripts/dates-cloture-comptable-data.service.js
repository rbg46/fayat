////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////// FORMATAGE ET RECUPERATION DATES CLOTURE COMPTABLE //////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
(function () {
  'use strict';

  angular
      .module('Fred')
      .service('DatesClotureComptableDataService', DatesClotureComptableDataService);

  DatesClotureComptableDataService.$inject = ['DatesClotureComptableHelperService'];

  function DatesClotureComptableDataService(DatesClotureComptableHelperService) {



    var service = {
      getMonth: getMonth,
      formatDataForServer: formatDataForServer,
      closePeriod: closePeriod,
      unClosePeriod: unClosePeriod
    };
    return service;

    function getMonth(serverData, ciId, year, monthNumber) {
      var monthData = serverData.filter(function (monthData) {
        return monthData.Mois === monthNumber && monthData.Annee === year;
      });
      if (monthData.length === 0) {
        return getDefaultMonthData(ciId, year, monthNumber);
      }
      return getMonthData(monthData[0]);
    }

    function getDefaultMonthData(ciId, year, monthNumber) {
      return {
        DatesClotureComptableId: 0,
        CiId: ciId,
        Annee: parseInt(year),
        Mois: parseInt(monthNumber),
        Periode: parseInt(year + '' + monthNumber),
        DateArretSaisie: null,
        DateTransfertFAR: null,
        DateCloture: null
      };
    }

    function getMonthData(monthData) {
      return {
        DatesClotureComptableId: monthData.DatesClotureComptableId,
        CiId: monthData.CiId,
        Annee: parseInt(monthData.Annee),
        Mois: parseInt(monthData.Mois),
        Periode: parseInt(monthData.Annee + '' + monthData.Mois),
        DateArretSaisie: createDateIfNotNull(monthData.DateArretSaisie),
        DateTransfertFAR: createDateIfNotNull(monthData.DateTransfertFAR),
        DateCloture: createDateIfNotNull(monthData.DateCloture)
      };
    }

    function createDateIfNotNull(date) {
      return date ? new Date(date) : null;
    }


    function formatDataForServer(dcc) {
      var datesClotureComptable =
            {
              DatesClotureComptableId: dcc.DatesClotureComptableId,
              CiId: dcc.CiId,
              Annee: dcc.Annee,
              Mois: dcc.Mois,
              DateArretSaisie: DatesClotureComptableHelperService.formatDate(dcc.DateArretSaisie),
              DateTransfertFAR: DatesClotureComptableHelperService.formatDate(dcc.DateTransfertFAR),
              DateCloture: DatesClotureComptableHelperService.formatDate(dcc.DateCloture)
            };
      return datesClotureComptable;
    }



    function closePeriod(dcc, now) {
      DatesClotureComptableHelperService.verifyDatesClotureComptableIsNotNull(dcc);
      dcc.DateCloture = now;
    }


    function unClosePeriod(dcc) {
      DatesClotureComptableHelperService.verifyDatesClotureComptableIsNotNull(dcc);
      dcc.DateCloture = null;
    }



  }
})();

