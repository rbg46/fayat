////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////// CALCUL DU STATUS DATES CLOTURE COMPTABLE ///////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
(function () {
  'use strict';

  angular.module('Fred').service('DatesClotureComptableStatusService', DatesClotureComptableStatusService);

  DatesClotureComptableStatusService.$inject = ['DatesClotureComptableHelperService'];

  function DatesClotureComptableStatusService(DatesClotureComptableHelperService) {

    var service = {
      periodInactive: periodInactive,
      periodeClosed: periodeClosed,
      periodInProgress: periodInProgress
    };
    return service;


    /*
      Date du jour (=J1)
      La période MM/AAAA est "Clôturée" pour le CI si sa date de clôture (J2) est positionnée dans le passé (J2<=J1).
     */
    function periodeClosed(dcc, now) {
      if (dateClotureIsMissing(dcc)) {
        return false;
      }

      var newDateCloture = DatesClotureComptableHelperService.formatDate(dcc.DateCloture);

      if (now < newDateCloture) {
        return false;
      }
      else if (newDateCloture <= now) {
        return true;
      }
    }


    /*
      * Date du jour (=J1)
      * La période MM/AAAA est "Inactive" pour le CI si sa date de clôture (J2) est absente ou positionnée dans le futur (J2>J1),
      * ET si la date du jour (J1) est antérieure au 1er jour du mois M (ie le mois M n'est pas encore entamé : J1<01/MM/AAA).
      * 
      */
    function periodInactive(dcc, now) {

      DatesClotureComptableHelperService.verifyDatesClotureComptableIsNotNull(dcc);

      var dateIsMissing = dateClotureIsMissing(dcc);

      var dateIsInFutur = dateClotureIsInFutur(dcc, now);

      var firstDayOfPeriod = getFirstDayOfPeriode(dcc);

      var nowIsPreviousOfFirstDayOfPeriod = now < firstDayOfPeriod;

      if ((dateIsMissing || dateIsInFutur) && nowIsPreviousOfFirstDayOfPeriod) {
        return true;
      }

      return false;

    }



    /*
     * Retourne le  premier jour d'un dcc
     */
    function getFirstDayOfPeriode(dcc) {
      var firstDayOfPeriod = new Date(dcc.Annee, dcc.Mois - 1, 1, 12);
      return firstDayOfPeriod;
    }

    function dateClotureIsMissing(dcc) {
      if (dcc.DateCloture === null || dcc.DateCloture === undefined) {
        return true;
      }
      return false;
    }

    function dateClotureIsInFutur(dcc, now) {
      var newDateCloture = DatesClotureComptableHelperService.formatDate(dcc.DateCloture);
      var isInFutur = newDateCloture > now;
      return isInFutur;
    }


    /*
     * Date du jour (=J1)
     * La période M est "En cours" pour le CI si sa date de clôture (J2) est absente ou positionnée dans le futur (J2>J1),
     *  ET si la date du jour (J1) est postérieure ou égale au 1er jour du mois M (ie le mois M est entamé : J1>=01/MM/AAA).
     */
    function periodInProgress(dcc, now) {


      DatesClotureComptableHelperService.verifyDatesClotureComptableIsNotNull(dcc);

      var dateIsMissing = dateClotureIsMissing(dcc);

      var dateIsInFutur = dateClotureIsInFutur(dcc, now);

      var firstDayOfPeriod = getFirstDayOfPeriode(dcc);

      var nowIsNextOrEqualOfFirstDayOfPeriod = now >= firstDayOfPeriod;

      if ((dateIsMissing || dateIsInFutur) && nowIsNextOrEqualOfFirstDayOfPeriod) {
        return true;
      }

      return false;

    }

  }
})();

