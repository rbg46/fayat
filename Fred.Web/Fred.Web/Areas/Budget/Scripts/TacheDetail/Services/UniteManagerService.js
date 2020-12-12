
(function () {
  'use strict';

  angular
      .module('Fred')
      .service('UniteManagerService', UniteManagerService);

  UniteManagerService.$inject = ['ParametrageReferentielEtenduService'];

  function UniteManagerService(ParametrageReferentielEtenduService) {


    var service = {
      getParamIsHourTime: getParamIsHourTime,
      getParamIsDayTime: getParamIsDayTime,
      /*
      * Change l'unite pour tous les parametrageReferentielEtendu d'une ressource.
      */
      changeUniteForAllParametrageReferentielEtendus: changeUniteForAllParametrageReferentielEtendus   
    };

    return service;



    //////////////////////////////////////////////////////////////////
    // Publics methodes                                             //
    //////////////////////////////////////////////////////////////////


    /*
     * Permet de savoir si le param a une unite de type heure
     */
    function getParamIsHourTime(ressourceTask, deviseSelected) {
      var unite = getUniteOfParameterByDeviseAndRessourceTask(ressourceTask, deviseSelected);
      if (unite) {
        return unite.UniteId === 4;
      }
    }

    /*
    * Permet de savoir si le param a une unite de type jour
    */
    function getParamIsDayTime(ressourceTask, deviseSelected) {
      var unite = getUniteOfParameterByDeviseAndRessourceTask(ressourceTask, deviseSelected);
      if (unite) {
        return unite.UniteId === 5;
      }
    }

    /*
     * Change l'unite pour tous les parametrageReferentielEtendu d'une ressource.
     */
    function changeUniteForAllParametrageReferentielEtendus(ressource, unite) {
      var devicesDistinct = ParametrageReferentielEtenduService.getDevisesOfParametrageReferentielEtendusByRessource(ressource);
      for (var i = 0; i < devicesDistinct.length; i++) {
        var deviseSelected = devicesDistinct[i];
        var lastParamForDevise = ParametrageReferentielEtenduService.getLastParametrageReferentielEtenduForDeviseByRessource(ressource, deviseSelected);
        lastParamForDevise.Unite = unite;
        lastParamForDevise.UniteId = unite.UniteId;
      }
    }

  
    //////////////////////////////////////////////////////////////////
    // Private methodes                                             //
    //////////////////////////////////////////////////////////////////


    function getUniteOfParameterByDeviseAndRessourceTask(ressourceTask, deviseSelected) {
      var lastParam = ParametrageReferentielEtenduService.getLastParametrageReferentielEtenduForDevise(ressourceTask, deviseSelected);
      if (!lastParam) return null;
      return lastParam.Unite;
    }

   

  }
})();

